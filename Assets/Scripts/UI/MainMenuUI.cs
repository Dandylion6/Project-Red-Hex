using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Menu Settings")]
    [SerializeField] private string startingScene = "Scene Name";

    private readonly WaitForSeconds pressWaitTime = new(1.25f);


    public void OnPlayPress() => StartCoroutine(LoadGame());


    private IEnumerator LoadGame()
    {
        DontDestroyOnLoad(gameObject);

        LoadingUI.Instance.StartUI();
        yield return pressWaitTime;

        yield return SceneManager.LoadSceneAsync(BaseScenes.CORE_SCENE);
        yield return SceneManager.LoadSceneAsync(startingScene, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(startingScene));

        yield return pressWaitTime;
        LoadingUI.Instance.EndUI();
        Destroy(gameObject);
    }


    public void OnQuitPress() => StartCoroutine(ExitGame());


    private IEnumerator ExitGame()
    {
        yield return pressWaitTime;
        Application.Quit();
    }
}
