using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEngine.GraphicsBuffer;

public class Monster : MonoBehaviour
{
    private MonsterData monsterData;
    public MonsterData MonsterData { get { return monsterData; } }

    [SerializeField]
    private SpriteRenderer sprite = null;
    [SerializeField]
    private GameObject damageTextBase = null;
    [SerializeField]
    private Transform damageTr = null;
    [SerializeField]
    private Canvas uiCanvas = null;

    private Transform nowTarget = null;

    private Transform[] waitpoints = null;

    private Coroutine moveCor = null;

    private int currnetIndex = 0;

    public void SetMonsterData(MonsterData data)
    {
        monsterData = data;
        Debug.Log($"Set MonsterData : ID : {monsterData.Id} / Hp : {monsterData.Hp} / Speed : {monsterData.Speed} / {monsterData.SpriteName}");
        sprite.sprite = ResourceManager.spriteDic[monsterData.SpriteName];
    }

    public void SetMoveTarget(Transform[] trans, int index)
    {
        waitpoints = trans;
        currnetIndex = index;
        nowTarget = waitpoints[currnetIndex];
    }

    private void Start()
    {
        Move();
    }

    public void Move()
    {
        if (moveCor != null)
        {
            StopCoroutine(moveCor);
            moveCor = null;
        }

        moveCor = StartCoroutine(MoveCoroutine());
    }

    public void Hit(int damage)
    {
        GameObject damageText = Instantiate(damageTextBase, damageTr);
        damageText.GetComponent<DamageText>().ShowDamageText(damage, Dead);

        monsterData.Hp -= damage;
    }

    public void Dead()
    {
        if (monsterData.Hp <= 0)
        {
            if (moveCor != null)
            {
                StopCoroutine(moveCor);
                moveCor = null;
            }

            BattleManager.Instance.waveList.Remove(this);
            BattleManager.Instance.Gold += 1;
            BattleManager.Instance.SetGoldUI();
            Destroy(gameObject);
        }
    }

    private IEnumerator MoveCoroutine()
    {
        Debug.Log("Start Move Cor");
        while(true)
        {
            Vector3 direction = (nowTarget.position - transform.position).normalized;
            transform.position += direction * monsterData.Speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, nowTarget.position) < 0.1f)
            {
                currnetIndex = (currnetIndex+1) % waitpoints.Length;
                nowTarget = waitpoints[currnetIndex];

                yield return new WaitForEndOfFrame();
            }

            yield return null;
        }
    }
}

public struct MonsterData
{
    public MonsterData(long _id, int _hp, float _speed, string _spriteName)
    {
        id = _id;
        hp = _hp;
        speed = _speed;
        spriteName = _spriteName;
    }


    /// <summary>
    /// Unique Id
    /// </summary>
    private long id;

    public long Id { get { return id; } private set { id = value; } }

    private int hp;
    public int Hp { get { return hp; } set { hp = value; } }

    private float speed;
    public float Speed { get { return speed; } private set { speed = value; } }

    private string spriteName;
    public string SpriteName { get { return spriteName; } private set { spriteName = value; } }
}