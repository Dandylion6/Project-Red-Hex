using DG.Tweening;
using UnityEngine;

public class EndScreenUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup group = null;


    public void OnBossDefeated()
    {
        group.alpha = 0.0f;
        group.DOFade(1.0f, 0.5f).SetEase(Ease.InOutSine).SetUpdate(true).Play();
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.0f, 0.5f).SetEase(Ease.OutSine).SetUpdate(true).Play();
        group.interactable = true;
        group.blocksRaycasts = true;
    }


    public void BackToMenu()
    {
        GameManager.Instance.GoBackToMenuAsync();
    }
}
