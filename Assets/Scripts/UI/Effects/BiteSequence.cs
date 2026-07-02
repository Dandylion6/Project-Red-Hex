using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BiteSequence : EffectSequence
{
    [Header("References")]
    [SerializeField] private CanvasGroup group = null;
    [SerializeField] private RectTransform upperJaw = null;
    [SerializeField] private RectTransform lowerJaw = null;
    [SerializeField] private float delaySeconds = 0.2f;

    [Header("Bite tuning")]
    [SerializeField] private float biteWorldHeightOffset = 1.0f;
    [SerializeField] private float jawAnticipationOffset = 6.0f;
    [SerializeField] private float upperJawCloseOffset = 10.0f;
    [SerializeField] private float lowerJawCloseOffset = 35.0f;
    [SerializeField] private float holdSeconds = 0.15f;


    public override IEnumerator PlaySeqeunce(TilePiece piece)
    {
        EffectsUI.Instance.AddEffect(this, GameManager.Instance.Player.transform.position + Vector3.up * biteWorldHeightOffset);
        group.alpha = 0.0f;
        group.transform.localScale = Vector3.one;

        float upperOpen = upperJaw.anchoredPosition.y;
        float lowerOpen = lowerJaw.anchoredPosition.y;
        float upperClosed = upperOpen - upperJawCloseOffset;
        float lowerClosed = lowerOpen + lowerJawCloseOffset;
        float upperWide = upperOpen + jawAnticipationOffset;
        float lowerWide = lowerOpen - jawAnticipationOffset;

        Sequence sequence = DOTween.Sequence();

        // Pop in + anticipation: jaws pull apart slightly before the bite
        sequence.Append(group.DOFade(1.0f, 0.08f).SetEase(Ease.OutQuad));
        sequence.Join(upperJaw.DOAnchorPosY(upperWide, 0.08f).SetEase(Ease.OutQuad));
        sequence.Join(lowerJaw.DOAnchorPosY(lowerWide, 0.08f).SetEase(Ease.OutQuad));

        // The bite: fast snap shut, with a squash landing exactly on contact
        sequence.Append(upperJaw.DOAnchorPosY(upperClosed, 0.12f).SetEase(Ease.InQuint));
        sequence.Join(lowerJaw.DOAnchorPosY(lowerClosed, 0.12f).SetEase(Ease.InQuint));
        sequence.Join(group.transform.DOScale(new Vector3(1.4f, 0.5f, 1.0f), 0.12f).SetEase(Ease.OutQuad));

        // Impact settle: overshoot back to normal scale for a punchy finish
        sequence.Append(group.transform.DOScale(Vector3.one, 0.16f).SetEase(Ease.OutBack));

        // Hold the bite so it actually reads before it disappears
        sequence.AppendInterval(holdSeconds);

        // Release
        sequence.Append(group.DOFade(0.0f, 0.18f).SetEase(Ease.InQuad));
        sequence.OnComplete(() => Destroy(gameObject));

        yield return sequence.WaitForCompletion();
    }
}
