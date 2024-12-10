using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

public class CharacterSpawner : MonoBehaviour 
{
    [SerializeField]
    private Character characterBase = null;
    [SerializeField]
    private GameObject goSpawnArange = null;
    [SerializeField]
    private Button btnCreate = null;
    
    private WeightedRandomPicker<CharacterData> randomPicker = null;

    private List<CharacterData> characterDataSetList = new List<CharacterData>();

    /// <summary>
    /// UniqueId, object
    /// </summary>
    private Dictionary<long,List<CharacterBase>> characterDic = new Dictionary<long, List<CharacterBase>>();

    private void Awake()
    {
        btnCreate.onClick.AddListener(SpawnCaracter);
        ResourceManager.LoadCharacterImage();

        SetCharacterSpawner();
    }

    public void SetCharacterSpawner()
    {
        // 더미데이터 생성
        characterDataSetList.Add(new CharacterData(101, 5000, 1, 1f, CharacterData.EGrade.Normal, "Ws_Baby"));
        characterDataSetList.Add(new CharacterData(102, 2400, 2, 1f, CharacterData.EGrade.Rare, "Ws_Blubman"));
        characterDataSetList.Add(new CharacterData(103, 1500, 4, 0.8f, CharacterData.EGrade.Unique, "Ws_Lasy"));
        characterDataSetList.Add(new CharacterData(104, 600, 8, 0.5f, CharacterData.EGrade.Legendary, "Ws_Scum"));
        characterDataSetList.Add(new CharacterData(105, 100, 20, 0.1f, CharacterData.EGrade.Hidden, "Ws_Silkworm"));

        randomPicker = new WeightedRandomPicker<CharacterData>();
        foreach(var character in characterDataSetList)
        {
            randomPicker.Add(character, character.Weight);
        }
    }

    private void SpawnCaracter()
    {
        if (BattleManager.Instance.Gold < 10)
        {
            Debug.Log($"골드 부족 : {BattleManager.Instance.Gold}");
            return;
        }

        BattleManager.Instance.Gold -= 10;
        BattleManager.Instance.SetGoldUI();

        CharacterData pick = randomPicker.GetRandomPick();
        Character temp = Instantiate(characterBase, GetRandomPosition(), Quaternion.identity, transform);
        temp.SetCharacterData(pick);

    }

    private void CheckGradeUp(CharacterBase pick)
    {
        if (characterDic.ContainsKey(pick.Id))
        {
            if (characterDic[pick.Id] != null)
            {
                if (characterDic[pick.Id].Count + 1 >= characterDic[pick.Id][0].Exp)
                { // 등급업 가능
                    characterDic[pick.Id][0] = null;
                    Destroy(characterDic[pick.Id][0]);
                    characterDic[pick.Id + 1].Add(pick);
                }

                //Instantiate(characterBases.ToList().Find(x => x.Id == pick.Id + 1), GetRandomPosition(), Quaternion.identity, transform);
            }
            else
            {
                characterDic[pick.Id].Add(pick);
                Instantiate(pick, GetRandomPosition(), Quaternion.identity, transform);
            }

        }
        else
        {
            List<CharacterBase> tempList = new List<CharacterBase> { pick };
            characterDic.Add(pick.Id, tempList);
            Instantiate(pick, GetRandomPosition(), Quaternion.identity, transform);
        }
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 basePosition = transform.position;  //오브젝트의 위치
        Vector3 size = goSpawnArange.GetComponent<BoxCollider2D>().size;                   //box colider2d, 즉 맵의 크기 벡터

        //x, y축 랜덤 좌표 얻기
        float posX = basePosition.x + UnityEngine.Random.Range(-size.x / 2f, size.x / 2f);
        float posY = basePosition.y + UnityEngine.Random.Range(-size.y / 2f, size.y / 2f);

        Vector3 spawnPos = new Vector3(posX, posY, 0);

        return spawnPos;
    }
}
