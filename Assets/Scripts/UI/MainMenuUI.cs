using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Menu Settings")]
    [SerializeField] private string startingScene = "Scene Name";

    private AudioSource source;
    public void Start()
    {
        source = GetComponent<AudioSource>();

    }

    public void OnPlayPress() => StartCoroutine(LoadGame());


    private IEnumerator LoadGame()
    {
        AudioManager.Instance.StopLoop(source);
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.AudioBank.UIClick, SettingsManager.Instance.GameVolume, false);
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(BaseScenes.CORE_SCENE);
        yield return SceneManager.LoadSceneAsync(startingScene, LoadSceneMode.Additive);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(startingScene));
        Destroy(gameObject);
    }


    public void OnSettingsPress()
    {
        AudioManager.Instance.PlayOneShot(AudioManager.Instance.AudioBank.UIClick, SettingsManager.Instance.GameVolume, false);
    }


    public void OnQuitPress() => Application.Quit();

    
}
