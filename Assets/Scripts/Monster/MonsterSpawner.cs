using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPosition = null;
    [SerializeField]
    private Monster monsterBase = null;

    private Coroutine spawnCor = null;

    [SerializeField]
    private Transform[] wayPoints = null;

    /// <summary>
    /// StageNumber, MonsterPoool
    /// </summary>
    private Dictionary<int, List<MonsterData>> monsterPoolDic = new Dictionary<int, List<MonsterData>>();


    private void Awake()
    {
        // 더미데이터 생성
        List<MonsterData> monsterList = new List<MonsterData>();

        for(int i = 0; i < 10; i++)
        {
            monsterList.Add(new MonsterData(201, 2, 1f, 1, "Ws_Bird"));
        }

        for (int i = 0; i < 10; i++)
        {
            monsterList.Add(new MonsterData(202, 4, 1.2f, 2, "Ws_Law"));
        }

        for (int i = 0; i < 10; i++)
        {
            monsterList.Add(new MonsterData(203, 10, 0.5f, 3, "Ws_Snail"));
        }

        monsterPoolDic.Add(1, monsterList);

        SpawnMonster();
    }

    public void SpawnMonster()
    {
        if (spawnCor != null)
        {
            StopCoroutine(spawnCor);
            spawnCor = null;
        }
        
        spawnCor = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(3f);
        WaitForSeconds delay2 = new WaitForSeconds(1f);

        yield return delay;

        for(int i = 0; i < monsterPoolDic[1].Count; i++)
        {
            Monster temp = Instantiate(monsterBase, spawnPosition.position, Quaternion.identity, transform).GetComponent<Monster>();
            temp.SetMonsterData(monsterPoolDic[1][i]);
            temp.SetMoveTarget(wayPoints, 1);

            BattleManager.Instance.waveList.Add(temp);

            yield return delay2;
        }
    }
}
