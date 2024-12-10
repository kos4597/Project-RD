using UnityEngine;
using System.Collections.Generic;

public interface IMonoBehaviourSingleton
{
    void InitializeSingleton();

    void DestroySingleton();
}

public static class MonoBehaviorSingletonManager
{
    public static List<IMonoBehaviourSingleton> singletons = new List<IMonoBehaviourSingleton>();

    public static void DestroyAll(bool bClear_ = false)
    {
        int count = singletons.Count, i;
        for (i = 0; i < count; i++)
        {
            if (singletons[i] != null) { singletons[i].DestroySingleton(); }
        }
        if (bClear_) { singletons.Clear(); }
    }

    public static void OnStarted()
    {
        int count = singletons.Count, i;
        for (i = 0; i < count; i++)
        {
            if (singletons[i] != null) { (singletons[i] as IMonoBehaviourSingleton).InitializeSingleton(); }
        }
        singletons.Clear();
    }
}

public class MonoBehaviourSingleton<T> : MonoBehaviour, IMonoBehaviourSingleton where T : MonoBehaviour
{
    private static bool applicationIsQuitting;

    protected static T _instance;
    public static T Instance
    {
        get
        {
            if (applicationIsQuitting) { return null; }
            if (_instance == null)
            {
#if UNITY_EDITOR
                // �� �÷��̸�忡�� FindObjectOfType�� ����� �۵����� �ʴ� ��찡 �־ �ڵ� ����
                if (!Application.isPlaying)
                {
                    GameObject targetManager = GameObject.Find(typeof(T).ToString());
                    if (null != targetManager)
                    {
                        _instance = targetManager.GetComponent<T>();
                        Debug.LogWarning(typeof(T).ToString() + " is duplicated!");
                    }
                }
                else { _instance = (T)FindObjectOfType(typeof(T)); }
#else
                _instance = (T)FindObjectOfType(typeof(T));
#endif
                if (_instance != null)
                {
                    (_instance as MonoBehaviourSingleton<T>).Initialize();
                    if (FindObjectsOfType(typeof(T)).Length > 1) { Debug.LogError("[MonoBehaviourSingleton] duplicated: " + typeof(T).ToString()); }
                }
                if (_instance == null)
                {
                    GameObject singleton = new GameObject();
                    _instance = singleton.AddComponent<T>();
                    singleton.name = typeof(T).ToString();

#if UNITY_EDITOR
                    // �����Ϳ��� �����ؼ� �̱��� ��ü�� �����Ҷ� ������� �ʰ� ����� �ɼ� �߰�
                    if (!Application.isPlaying) { singleton.hideFlags = HideFlags.DontSaveInEditor; }
#endif
                    (_instance as MonoBehaviourSingleton<T>).Initialize();
                }
                if (_instance != null) { MonoBehaviorSingletonManager.singletons.Add(_instance as IMonoBehaviourSingleton); }
            }
            return _instance;
        }
    }

    [SerializeField]
    protected bool initializeOnAwake;

    protected virtual void Awake()
    {
        useGUILayout = false;
        if (initializeOnAwake)
        {
            _instance = this as T;
            MonoBehaviorSingletonManager.singletons.Add(this);
            Initialize();
        }
#if UNITY_EDITOR
        if (Application.isPlaying) { DontDestroyOnLoad(gameObject); }
#else
        DontDestroyOnLoad(gameObject);
#endif
    }

    public virtual void Initialize()
    {
    }

    #region IMonoBehaviourSingleton implementation
    public void InitializeSingleton()
    {
        applicationIsQuitting = false;
    }

    public void DestroySingleton()
    {
        Destroy(gameObject);
    }
    #endregion

    public void OnDestroy()
    {
        // �۾��� �Ǽ��� ���� ������Ʈ �ı� �� �̱��� ������ ���� ���� ó��
        if (_instance != null)
        {
            if (_instance.gameObject == gameObject)
            {
                applicationIsQuitting = true;
                _instance = null;
            }
            else
            {
                Debug.LogException(new System.Exception(typeof(T) + " destroyed: " + transform.root.name));
            }
        }
    }

    public void DontDestroyOnRestart()
    {
        MonoBehaviorSingletonManager.singletons.Remove(_instance as IMonoBehaviourSingleton);
    }

    public static void ForceApplicationQuittingTrue()
    {
        applicationIsQuitting = false;
    }

    public static void ForceApplicationQuittingFalse()
    {
        applicationIsQuitting = true;
    }

    public static bool IsValidInstance()
    {
        return (_instance != null);
    }

    public static bool IsQuitting()
    {
        return applicationIsQuitting;
    }
}