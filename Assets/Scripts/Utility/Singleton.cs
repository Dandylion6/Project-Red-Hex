using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static public T Instance => instance;

    static private T instance = null;


    private void Awake() => instance = this as T;
}


public class SingletonPersistent<T> : MonoBehaviour where T : SingletonPersistent<T>
{
    static public T Instance => instance;

    static private T instance = null;


    private void Awake() 
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        instance = this as T; 
    }
}