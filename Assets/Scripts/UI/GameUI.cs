using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    private const string BOSS_SCENE_NAME = "Level_3"; // Hard coded due to time contraints.


    [Header("Quest UI Settings")]
    [SerializeField] private TMP_Text questText = null;
    [SerializeField][TextArea] private string questDescription = string.Empty;
    [SerializeField][TextArea] private string bossQuestDescription = string.Empty;


    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
        TurnManager.Instance.SubscribeToOnTurnChanged(OnTurnChanged);
        OnTurnChanged(null, null);
    }


    private void OnTurnChanged(TilePiece currentPiece, TilePiece lastPiece)
    {
        int collected = 0;
        if (TeleportPoint.Instance != null)
        {
            collected = TeleportPoint.Instance.MoonShardsCollected;
        }
        questText.text = questDescription;
        questText.text += $"\n \n Moon Shards collected ({collected}/{TeleportPoint.REQUIRED_SHARDS})";
    }


    private void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != BOSS_SCENE_NAME) return;
        
        TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);
        questText.text = bossQuestDescription;
    }


    private void OnDestroy() => TurnManager.Instance.UnsubscribeFromOnTurnChanged(OnTurnChanged);

    
}
