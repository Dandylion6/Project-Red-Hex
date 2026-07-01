using System;
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

    private Action onGameRestart = null;
    private CheckpointData checkpoint = new();


    public void SubscribeToOnGameRestart(Action callback) => onGameRestart += callback;
    public void UnsubscribeFromOnGameRestart(Action callback) => onGameRestart -= callback;


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
        string sceneName = currentScene.name;

        yield return SceneManager.UnloadSceneAsync(currentScene);
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        LoadCheckpoint();
        onGameRestart?.Invoke();
    }


    private void LoadCheckpoint()
    {
        player.SetHealth(checkpoint.playerHealth);
    }
}
