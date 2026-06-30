using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public struct CheckpointData
    {
        public int playerHealth;
    }


    [Header("References")]
    [SerializeField] private TilePiece player = null;
    [SerializeField] private SettingsManager settingsManager = null;


    public TilePiece Player => player;

    private CheckpointData checkpoint = new();


    public void SetCheckpointData(CheckpointData data) => checkpoint = data;

    public void ChangeGameSceneAsync(string nextScene) => StartCoroutine(ChangeGameScene(nextScene));
    public void RestartSceneAsync() => StartCoroutine(RestartScene());

    private IEnumerator ChangeGameScene(string nextScene)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        yield return SceneManager.UnloadSceneAsync(currentScene);
        yield return SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
    }

    private IEnumerator RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        
        yield return SceneManager.UnloadSceneAsync(currentScene);
        LoadCheckpoint();
        yield return SceneManager.LoadSceneAsync(currentScene.name);
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene.name));
    }


    private void LoadCheckpoint()
    {
        player.SetHealth(checkpoint.playerHealth);
    }
}
