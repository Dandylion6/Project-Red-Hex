using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : SingletonPersistent<LoadingUI>
{
    [Header("References")]
    [SerializeField] private CanvasGroup group = null;
    [SerializeField] private Image veil = null;

    [SerializeField] private RectTransform jaw = null;
    [SerializeField] private RectTransform upperJaw = null;
    [SerializeField] private RectTransform lowerJaw = null;

    [Header("Loading Animation")]
    [SerializeField] private float biteDelaySeconds = 0.2f;
    [SerializeField] private float jawAnticipationOffset = 6.0f;
    [SerializeField] private float upperJawCloseOffset = 10.0f;
    [SerializeField] private float lowerJawCloseOffset = 35.0f;



    Sequence loadingAnimation = null;


    public void StartUI()
    {
        Time.timeScale = 1.0f;
        veil.fillOrigin = 1;
        veil.DOKill();

        loadingAnimation.Restart();

        veil.DOFillAmount(1.0f, 0.5f).SetEase(Ease.InSine).OnComplete(() =>
        {
            group.interactable = true;
            group.blocksRaycasts = true;
            group.alpha = 1.0f;

            veil.fillOrigin = 0;
            veil.DOKill();
            veil.DOFillAmount(0.0f, 0.5f).SetEase(Ease.OutSine).Play();

        }).Play();
    }


    public void EndUI()
    {
        Time.timeScale = 1.0f;
        veil.fillOrigin = 1;
        veil.DOKill();

        veil.DOFillAmount(1.0f, 0.5f).SetEase(Ease.InSine).OnComplete(() =>
        {
            group.interactable = false;
            group.blocksRaycasts = false;
            group.alpha = 0.0f;

            veil.fillOrigin = 0;
            veil.DOKill();
            veil.DOFillAmount(0.0f, 0.5f).SetEase(Ease.OutSine).Play();
            loadingAnimation.Pause();

        }).Play();
    }


    private void LoadingAnimation()
    {
        float upperOpen = upperJaw.anchoredPosition.y;
        float lowerOpen = lowerJaw.anchoredPosition.y;
        float upperClosed = upperOpen - upperJawCloseOffset;
        float lowerClosed = lowerOpen + lowerJawCloseOffset;
        float upperWide = upperOpen + jawAnticipationOffset;
        float lowerWide = lowerOpen - jawAnticipationOffset;
        
        loadingAnimation = DOTween.Sequence();

        // Pop in + anticipation: jaws pull apart slightly before the bite
        loadingAnimation.Append(upperJaw.DOAnchorPosY(upperWide, 0.08f).SetEase(Ease.OutQuad));
        loadingAnimation.Join(lowerJaw.DOAnchorPosY(lowerWide, 0.08f).SetEase(Ease.OutQuad));

        // The bite: fast snap shut, with a squash landing exactly on contact
        loadingAnimation.Append(upperJaw.DOAnchorPosY(upperClosed, 0.12f).SetEase(Ease.InQuint));
        loadingAnimation.Join(lowerJaw.DOAnchorPosY(lowerClosed, 0.12f).SetEase(Ease.InQuint));
        loadingAnimation.Join(jaw.DOScale(new Vector3(1.4f, 0.5f, 1.0f), 0.12f).SetEase(Ease.OutQuad));

        // Impact settle: overshoot back to normal scale for a punchy finish
        loadingAnimation.Append(jaw.DOScale(Vector3.one, 0.16f).SetEase(Ease.OutBack));
        loadingAnimation.Join(jaw.DOShakePosition(0.24f, 25.0f, 24));

        loadingAnimation.AppendInterval(biteDelaySeconds);

        loadingAnimation.Append(upperJaw.DOAnchorPosY(upperOpen, 0.08f).SetEase(Ease.OutQuad));
        loadingAnimation.Join(lowerJaw.DOAnchorPosY(lowerOpen, 0.08f).SetEase(Ease.OutQuad));

        loadingAnimation.SetLoops(-1);
    }


    private void Start()
    {
        group.alpha = 0.0f;
        LoadingAnimation();
    }
}
