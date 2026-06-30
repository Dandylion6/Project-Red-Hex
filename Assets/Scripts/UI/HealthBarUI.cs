using DG.Tweening;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CanvasGroup group = null;
    [SerializeField] private TMP_Text currentHealthText = null;
    [SerializeField] private TMP_Text dividerText = null;
    [SerializeField] private TMP_Text maxHealthText = null;

    [Header("Health Bar Settings")]
    [SerializeField] private Color allyColor = Color.white;
    [SerializeField] private Color enemyColor = Color.white;
    [SerializeField] private Vector2 screenOffset = Vector2.zero;

    [Header("Animation Settings")]
    [SerializeField][Min(0.01f)] private float fadeTime = 0.2f;
    [SerializeField] private Ease fadeEasing = Ease.InOutSine;
    [SerializeField][Min(0)] private float minDamageShakeIntensity = 0.2f;
    [SerializeField][Min(0)] private float maxDamageShakeIntensity = 1.0f;
    [SerializeField][Min(0)][Tooltip("Amount of damage which will cause the minimum intensity shake.")] private int minIntensityDamage = 1;
    [SerializeField][Min(0)][Tooltip("Amount of damage which will cause the maximum intensity shake.")] private int maxIntensityDamage = 6;
    [SerializeField][Min(0)] private float healPunchStrength = 6.0f;
    [SerializeField][Min(0)] private float healPunchTime = 0.8f;


    private TilePiece piece = null;
    private Camera mainCamera = null;


    public void Initialize(TilePiece piece, bool isEnemy = true)
    {
        if (piece == null) return;

        this.piece = piece;
        mainCamera = Camera.main;

        piece.SubscribeToOnDamageTaken(OnDamageTaken);
        piece.SubscribeToOnHeal(OnHeal);

        Color color = isEnemy ? enemyColor : allyColor;

        currentHealthText.color = color;
        dividerText.color = color;
        maxHealthText.color = color;

        group.DOFade(1.0f, fadeTime).SetEase(fadeEasing).Play();
        UpdateText();
    }


    private void OnDamageTaken(int amount)
    {
        UpdateText();

        group.transform.DOKill();
        currentHealthText.rectTransform.DOKill();
        maxHealthText.rectTransform.DOKill();

        float t = Mathf.InverseLerp(minIntensityDamage, maxIntensityDamage, amount);
        float intensity = Mathf.Lerp(minDamageShakeIntensity, maxDamageShakeIntensity, t);

        group.transform.DOPunchPosition(0.4f * intensity * Vector3.right, 0.4f, 12).Play();
        currentHealthText.rectTransform.DOShakePosition(1.0f, intensity, 16).Play();
        maxHealthText.rectTransform.DOShakePosition(0.8f, intensity * 0.6f, 16).Play();
    }


    private void OnHeal(int amount)
    {
        UpdateText();

        currentHealthText.rectTransform.DOKill();
        dividerText.rectTransform.DOKill();
        maxHealthText.rectTransform.DOKill();

        currentHealthText.rectTransform.DOPunchAnchorPos(healPunchStrength * Vector2.up, healPunchTime).Play();
        dividerText.rectTransform.DOPunchAnchorPos(healPunchStrength * 0.8f * Vector2.up, healPunchTime).SetDelay(0.1f).Play();
        maxHealthText.rectTransform.DOPunchAnchorPos(healPunchStrength * 0.6f * Vector2.up, healPunchTime).SetDelay(0.2f).Play();
    }


    private void UpdateText()
    {
        currentHealthText.text = piece.Health.ToString();
        maxHealthText.text = piece.MaxHealth.ToString();
    }


    private void Awake() => group.alpha = 0.0f;


    private void Update()
    {
        if (piece == null) return;

        Vector3 worldPosition = piece.transform.position + Vector3.up * piece.HealthBarHeight;
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
        transform.position = screenPosition + screenOffset;
    }


    private void OnDestroy()
    {
        if (piece != null) return;
        piece.UnsubscribeFromOnDamageTaken(OnDamageTaken);
        piece.UnsubscribeToOnHeal(OnHeal);
    }
}
