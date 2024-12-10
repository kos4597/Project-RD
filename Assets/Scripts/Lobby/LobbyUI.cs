using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField]
    private Button btnBattle = null;
    [SerializeField]
    private SceneChanger sceneChanger = null;

    private void Awake()
    {
        btnBattle.onClick.AddListener(OnClickEnterBattle);
    }

    private void OnClickEnterBattle()
    {
        sceneChanger.SceneChange(SceneChanger.SceneType.Battle);
    }
}
