using UnityEngine;

#if UNITY_EDITOR
using VInspector;
#endif

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
#if UNITY_EDITOR
    [Tab("Settings")]
#endif
    [SerializeField] private bool _persistBetweenScenes = false;

    private static T _instance;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                    }
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (_persistBetweenScenes) DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DoAwake();
    }
    protected virtual void DoAwake() { }
}
