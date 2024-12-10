using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public enum SceneType
    {
        Title,
        Lobby,
        Battle,
    }

    private SceneType nowSceneType = SceneType.Title;

    private Coroutine sceneChangeCor = null;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SceneChange(SceneType type)
    {
        if (nowSceneType == type)
            return;

        nowSceneType = type;

        if(sceneChangeCor != null)
        {
            StopCoroutine(sceneChangeCor);
            sceneChangeCor = null;
        }

        sceneChangeCor = StartCoroutine(SceneChangeCor());
    }

    public IEnumerator SceneChangeCor()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync($"{nowSceneType}");

        while (asyncOperation.isDone == false)
        {
            Debug.Log($"Scene Load : + {asyncOperation.progress * 100}%");

            yield return null;
        }

        switch (nowSceneType)
        {
            case SceneType.Title:
                {

                }
                break;

            case SceneType.Lobby:
                {

                }
                break;

            case SceneType.Battle:
                {

                }
                break;
        }
    }
}
