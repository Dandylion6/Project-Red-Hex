using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseScenes
{
    public const string CORE_SCENE = "Core_Scene";
    public const string MENU_SCENE = "Menu_Scene";
}

[InitializeOnLoad]
static public class EditorCoreLoader
{
    static EditorCoreLoader()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }


    static void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (!Application.isEditor) return;

        if (scene.name == BaseScenes.CORE_SCENE) return;
        if (scene.name == BaseScenes.MENU_SCENE)
        {
            SceneManager.sceneLoaded -= OnSceneLoad; // No need to add core scene anymore.
            return;
        }

        // Loads the core scene for any other scene additively
        SceneManager.LoadScene("CoreScene", LoadSceneMode.Additive);
        SceneManager.sceneLoaded -= OnSceneLoad; // Only happens once.
    }
}
