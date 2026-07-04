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


    public TilePiece Player => player;

    private Action onGameRestart = null;
    private CheckpointData checkpoint = new();
    private WaitForSeconds loadingWait = new(1.5f);


    public void SubscribeToOnGameRestart(Action callback) => onGameRestart += callback;
    public void UnsubscribeFromOnGameRestart(Action callback) => onGameRestart -= callback;


    public void SetCheckpointData(CheckpointData data) => checkpoint = data;

    public void ChangeGameSceneAsync(string nextScene) => StartCoroutine(ChangeGameScene(nextScene));
    public void RestartSceneAsync() => StartCoroutine(RestartScene());
    public void GoBackToMenuAsync() => StartCoroutine(GoBackToMenu());

    private IEnumerator ChangeGameScene(string nextScene)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        LoadingUI.Instance.StartUI();
        yield return loadingWait;

        yield return SceneManager.UnloadSceneAsync(currentScene);
        yield return SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
        onGameRestart?.Invoke();

        LoadingUI.Instance.EndUI();
    }


    private IEnumerator GoBackToMenu()
    {
        LoadingUI.Instance.StartUI();

        yield return loadingWait;
        yield return SceneManager.LoadSceneAsync(BaseScenes.MENU_SCENE);
        
        LoadingUI.Instance.EndUI();
    }


    private IEnumerator RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        LoadingUI.Instance.StartUI();
        yield return loadingWait;

        yield return SceneManager.UnloadSceneAsync(currentScene);
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
        LoadCheckpoint();
        onGameRestart?.Invoke();

        LoadingUI.Instance.EndUI();
    }


    private void LoadCheckpoint()
    {
        player.SetHealth(checkpoint.playerHealth);
    }
}
