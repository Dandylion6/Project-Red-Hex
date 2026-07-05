using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSequence : EffectSequence
{
    [Header("Fire Tuning")]
    [SerializeField] private List<Sprite> sprites = new();
    [SerializeField] private Image fireImage = null;
    [SerializeField] private float fireDelaySeconds = 0.1f;
    [SerializeField] private float fireWorldHeightOffset = 1.0f;
    [SerializeField] private float fireMoveDistance = 20.0f;
    [SerializeField] private float shakeIntensity = 10.0f;


    public override IEnumerator PlaySeqeunce(Transform attachTo)
    {
        fireImage.sprite = sprites[Random.Range(0, sprites.Count)];
        EffectsUI.Instance.AddEffect(this, attachTo, Vector3.up * fireWorldHeightOffset);

        fireImage.rectTransform.localScale = new(0.0f, 0.0f, 1.0f);
        fireImage.rectTransform.localPosition += Vector3.right * fireMoveDistance;
        fireImage.rectTransform.rotation *= Quaternion.Euler(0.0f, 0.0f, Random.Range(-180.0f, 180.0f));

        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.MusketFire);

        fireImage.rectTransform.DOShakePosition(0.1f, shakeIntensity, 12).Play();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(fireImage.rectTransform.DOScale(1.0f, 0.11f).SetEase(Ease.OutQuad));
        sequence.Append(fireImage.rectTransform.DOScale(0.0f, 0.18f).SetEase(Ease.InOutQuad));
        sequence.Join(fireImage.DOFade(0.0f, 0.16f).SetEase(Ease.InSine));
        sequence.OnComplete(() => Destroy(gameObject));

        yield return new WaitForSeconds(fireDelaySeconds);

        AudioManager.Instance.PlayOneShotRandom(AudioManager.Instance.AudioBank.MusketHit);
    }
}
