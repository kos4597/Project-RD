using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviourSingleton<BattleManager>
{
    [NonSerialized]
    public int Gold = 0;
    [NonSerialized]
    public float battleTime = 0f;
    [NonSerialized]
    public List<Monster> waveList = new List<Monster>();
    [NonSerialized]
    public List<Character> characterList = new List<Character>();


    public BattleUI battleUi = null;

    public override void Initialize()
    {
        InitBattleData();
    }

    public void InitBattleData()
    {
        Gold = 10;
        battleTime = 60f;
        battleUi.InitBattleUI();
    }
    public void SetGoldUI()
    {
        battleUi.SetGoldText();
    }

    

    
}
