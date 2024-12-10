using System.Collections;
using TMPro;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timeText = null;
    [SerializeField]
    private TMP_Text goldText = null;

    private Coroutine battleTimer = null;

    public void InitBattleUI()
    {
        if (battleTimer != null)
        {
            StopCoroutine(battleTimer);
            battleTimer = null;
        }

        battleTimer = StartCoroutine(battleTimerCor());
        SetGoldText();
    }

    public void SetGoldText()
    {
        goldText.text = $"{BattleManager.Instance.Gold}";
    }

    private IEnumerator battleTimerCor()
    {
        while (BattleManager.Instance.battleTime > 0)
        {
            BattleManager.Instance.battleTime -= 1f;
            timeText.text = $"{BattleManager.Instance.battleTime}";
            yield return new WaitForSeconds(1f);
        }
    }
}
