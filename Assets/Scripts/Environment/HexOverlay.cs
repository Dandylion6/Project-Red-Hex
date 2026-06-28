using DG.Tweening;
using System;
using UnityEngine;

[RequireComponent(typeof(HexTile))]
public class HexOverlay : MonoBehaviour
{
    public enum Type
    {
        None,
        Target,
        Range,
    }


    [Header("Overlay Settings")]
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Color rangeColor = Color.white;
    [SerializeField] private Color targetColor = Color.limeGreen;


    public Type CurrentType => currentType;

    private HexTile tile = null;
    private Type currentType = Type.None;


    public void SetType(Type type)
    {
        currentType = type;
        if (tile != null && !tile.IsWalkable)
            currentType = Type.None;

        Color color = Color.white;
        switch (currentType)
        {
            case Type.None:
                {
                    spriteRenderer.DOFade(0.0f, 0.1f).SetEase(Ease.OutCirc).Play();
                    spriteRenderer.transform.DOScale(Vector3.one * 0.1f, 0.2f).SetEase(Ease.OutCirc).OnComplete(() =>
                    {
                        spriteRenderer.enabled = false;
                    }).Play();
                    return;
                }
            case Type.Range: color = rangeColor;
                break;
            case Type.Target: color = targetColor;
                break;
        }

        spriteRenderer.DOFade(1.0f, 0.1f).SetEase(Ease.OutSine).Play();
        spriteRenderer.transform.DOScale(Vector3.one * 0.6f, 0.2f).SetEase(Ease.OutBack).Play();

        spriteRenderer.enabled = true;
        spriteRenderer.color = color;
    }


    private void Start()
    {
        tile = GetComponent<HexTile>();
        SetType(Type.None);
    }
}
