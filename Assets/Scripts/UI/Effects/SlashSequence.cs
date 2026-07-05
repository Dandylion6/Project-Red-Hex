using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SlashSequence : EffectSequence
{
    [Header("References")]
    [SerializeField] private Image slash = null;

    [Header("Slash Tuning")]
    [SerializeField] private float slashDelaySeconds = 0.2f;
    [SerializeField] private float slashWorldHeightOffset = 1.0f;
    [SerializeField] private float slashMoveDistance = 0.1f;
    [SerializeField] private float slashDissapearDelay = 0.1f;


    public override IEnumerator PlaySeqeunce(Transform attachTo)
    {
        EffectsUI.Instance.AddEffect(this, attachTo, Vector3.up * slashWorldHeightOffset);
        slash.fillAmount = 0.0f;

        PlaySound();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(slash.DOFillAmount(1.0f, 0.14f).SetEase(Ease.InCirc).OnComplete(() => slash.fillClockwise = false));
        sequence.Join(slash.rectTransform.DOLocalMove(Vector3.right * slashMoveDistance, 0.2f).SetEase(Ease.OutSine));
        
        sequence.Append(slash.rectTransform.DOShakePosition(0.2f, 16.0f, 16));
        sequence.Join(slash.DOFillAmount(0.0f, 0.12f).SetEase(Ease.OutQuad).SetDelay(slashDissapearDelay));

        sequence.OnComplete(() => Destroy(gameObject));

        yield return new WaitForSeconds(slashDelaySeconds);
    }


    protected virtual void PlaySound()
    {
        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.RapierHit);
    }
}
