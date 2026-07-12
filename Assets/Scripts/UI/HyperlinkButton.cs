using UnityEngine;

public class HyperlinkButton : ButtonClick
{
    [Header("Settings")]
    [SerializeField] private string link = "";


    public override void OnClick()
    {
        base.OnClick();
        Application.OpenURL(link);
    }
}
