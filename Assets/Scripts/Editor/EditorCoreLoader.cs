#if UNITY_EDITOR

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

static public class EditorCoreLoader
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        string mainScene = SceneManager.GetActiveScene().name;

        if (mainScene == BaseScenes.CORE_SCENE) return;
        if (mainScene == BaseScenes.MENU_SCENE) return;

        GameObject bootstrapperObject = new("[Editor Bootstrapper]");
        Object.DontDestroyOnLoad(bootstrapperObject);
        bootstrapperObject.AddComponent<BootstrapCoreLoader>().gameScene = mainScene;

        SceneManager.LoadScene(BaseScenes.CORE_SCENE);
    }
}


public class BootstrapCoreLoader : MonoBehaviour
{
    public string gameScene = string.Empty;


    private IEnumerator Start()
    {
        yield return null; // Wait for start to run first.
        yield return SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(gameScene));

        Destroy(gameObject); // Finished bootstrapping for editor.
    }
}

#endif