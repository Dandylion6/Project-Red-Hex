using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Menu Settings")]
    [SerializeField] private string startingScene = "Scene Name";


    public void OnPlayPress() => StartCoroutine(LoadGame());


    private IEnumerator LoadGame()
    {
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(BaseScenes.CORE_SCENE);

        SceneManager.LoadScene(startingScene, LoadSceneMode.Additive);
        Destroy(gameObject);
    }


    public void OnSettingsPress()
    {

    }


    public void OnQuitPress() => Application.Quit();
}
