using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlashSequence : EffectSequence
{
    [Header("References")]
    [SerializeField] private Image slash = null;
    [SerializeField] private Canvas canvas = null;

    [Header("Slash Tuning")]
    [SerializeField] private float slashDelaySeconds = 0.2f;
    [SerializeField] private float slashWorldHeightOffset = 1.0f;
    [SerializeField] private float slashMoveDistance = 0.1f;
    [SerializeField] private float slashDissapearDelay = 0.1f;


    public override IEnumerator PlaySeqeunce(EffectData data)
    {
        bool lookingRight = data.piece.Rotation >= 30.0f && data.piece.Rotation <= 210.0f;
        slash.transform.localScale = new(1.0f, lookingRight ? 1.0f : -1.0f, 1.0f);

        Vector3 position = data.piece.transform.position + Vector3.up * slashWorldHeightOffset;
        transform.SetPositionAndRotation(position, Quaternion.Euler(0.0f, data.piece.Rotation, 0.0f));
        PlaySound();

        slash.fillAmount = 0.0f;
        Sequence sequence = DOTween.Sequence();

        sequence.Append(slash.DOFillAmount(1.0f, 0.14f).SetEase(Ease.InCirc).OnComplete(() => slash.fillClockwise = false));
        sequence.Join(slash.rectTransform.DOLocalMove(Vector3.forward * slashMoveDistance, 0.2f).SetEase(Ease.OutSine));
        
        sequence.Append(slash.rectTransform.DOShakePosition(0.2f, 0.1f, 16));
        sequence.Join(slash.DOFillAmount(0.0f, 0.12f).SetEase(Ease.OutQuad).SetDelay(slashDissapearDelay));

        sequence.OnComplete(() => Destroy(gameObject));

        yield return new WaitForSeconds(slashDelaySeconds);
    }


    protected virtual void PlaySound()
    {
        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.RapierHit);
    }
}
