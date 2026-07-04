using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ButtonClick : MonoBehaviour
{
    private RectTransform rect = null;
    private Vector2 size = Vector2.one;


    public void OnClick()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.AudioBank.UIClick);
        transform.DOKill();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(rect.DOSizeDelta(size - Vector2.one * 10.0f, 0.1f).SetEase(Ease.OutQuad));
        sequence.Append(rect.DOSizeDelta(size, 0.15f).SetEase(Ease.OutBack));
        sequence.Play();
    }


    private void Start()
    {
        rect = GetComponent<RectTransform>();
        size = rect.sizeDelta;
    }
}
