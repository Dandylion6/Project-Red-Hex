using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BiteSequence : EffectSequence
{
    [Header("References")]
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private CanvasGroup group = null;
    [SerializeField] private RectTransform upperJaw = null;
    [SerializeField] private RectTransform lowerJaw = null;

    [Header("Bite Tuning")]
    [SerializeField] private float biteDelaySeconds = 0.2f;
    [SerializeField] private float biteWorldHeightOffset = 1.0f;
    [SerializeField] private float jawAnticipationOffset = 6.0f;
    [SerializeField] private float upperJawCloseOffset = 10.0f;
    [SerializeField] private float lowerJawCloseOffset = 35.0f;
    [SerializeField][Range(0.0f, 1.0f)] private float cameraBias = 0.5f;


    public override IEnumerator PlaySeqeunce(EffectData data)
    {
        Vector3 direction = (data.target.transform.position - data.piece.transform.position).normalized;
        Vector3 toCameraDirection = (GameManager.Instance.MainCamera.transform.position - data.target.transform.position).normalized;
        Vector3 position = Vector3.Lerp(data.piece.transform.position, data.target.transform.position, 0.5f);
        position += Vector3.up * biteWorldHeightOffset;

        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
        Quaternion atCameraRotation = Quaternion.LookRotation(toCameraDirection, Vector3.up);

        transform.SetPositionAndRotation(position, Quaternion.Slerp(lookRotation, atCameraRotation, cameraBias));
        group.alpha = 0.0f;

        float upperOpen = upperJaw.anchoredPosition.y;
        float lowerOpen = lowerJaw.anchoredPosition.y;
        float upperClosed = upperOpen - upperJawCloseOffset;
        float lowerClosed = lowerOpen + lowerJawCloseOffset;
        float upperWide = upperOpen + jawAnticipationOffset;
        float lowerWide = lowerOpen - jawAnticipationOffset;

        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.WolfAttack);

        Sequence sequence = DOTween.Sequence();

        // Pop in + anticipation: jaws pull apart slightly before the bite
        sequence.Append(group.DOFade(1.0f, 0.08f).SetEase(Ease.OutQuad));
        sequence.Join(upperJaw.DOAnchorPosY(upperWide, 0.08f).SetEase(Ease.OutQuad));
        sequence.Join(lowerJaw.DOAnchorPosY(lowerWide, 0.08f).SetEase(Ease.OutQuad));

        // The bite: fast snap shut, with a squash landing exactly on contact
        sequence.Append(upperJaw.DOAnchorPosY(upperClosed, 0.12f).SetEase(Ease.InQuint));
        sequence.Join(lowerJaw.DOAnchorPosY(lowerClosed, 0.12f).SetEase(Ease.InQuint));
        sequence.Join(group.transform.DOScale(new Vector3(1.4f, 0.5f, 1.0f), 0.12f).SetEase(Ease.OutQuad));
        sequence.Join(upperJaw.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.InQuint));
        sequence.Join(lowerJaw.DOLocalRotate(Vector3.zero, 0.1f).SetEase(Ease.InQuint));

        // Impact settle: overshoot back to normal scale for a punchy finish
        sequence.Append(group.transform.DOScale(Vector3.one, 0.16f).SetEase(Ease.OutBack));
        sequence.Join(group.transform.DOShakePosition(0.24f, 0.22f, 30));

        // Release
        sequence.Append(group.DOFade(0.0f, 0.18f).SetEase(Ease.InQuad));
        sequence.OnComplete(() => Destroy(gameObject));

        yield return new WaitForSeconds(biteDelaySeconds);
    }
}
