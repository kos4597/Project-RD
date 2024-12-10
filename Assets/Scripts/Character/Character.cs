using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Character : MonoBehaviour
{
    [NonSerialized]
    public CharacterData characterData;
    [SerializeField]
    private SpriteRenderer sprite = null;

    private Monster nowTarget = null;

    private Coroutine attackCor = null;


    public void SetCharacterData(CharacterData data)
    {
        characterData = data;
        Debug.Log($"Set CharacterData : ID : {data.Id} / Weight : {data.Weight} / Damage : {data.Damage} / {data.SpriteName}");
        sprite.sprite = ResourceManager.spriteDic[data.SpriteName];
    }

    private void Start()
    {
        if (attackCor != null)
        {
            StopCoroutine(attackCor);
            attackCor = null;
        }

        attackCor = StartCoroutine(AttackCoroutine());
    }

    private IEnumerator AttackCoroutine()
    {
        while (true)
        {
            if(FindTarget())
            {
                Attack();
                yield return new WaitForSeconds(characterData.AttackSpeed);
            }
            yield return null;
        }
    }

    public void Attack()
    {
        if(nowTarget != null)
            nowTarget.Hit(characterData.Damage);
    }

    private bool FindTarget()
    {
        if (BattleManager.Instance.waveList.Count > 0)
        {
            nowTarget = BattleManager.Instance.waveList.Find(x => x.MonsterData.Hp > 0);
            return true;
        }
        return false;
    }
}

    public struct CharacterData
    {
        public enum EGrade
        {
            Normal = 1,
            Rare = 2,
            Unique = 3,
            Legendary = 4,
            Hidden = 5,
        }

        public CharacterData(long _id, int _weight, int _damage, float _attackSpeed, EGrade _grade, string _spriteName)
        {
            id = _id;
            weight = _weight;
            damage = _damage;
            attackSpeed = _attackSpeed;
            grade = _grade;
            spriteName = _spriteName;
        }


        /// <summary>
        /// Unique Id
        /// </summary>
        private long id;

        public long Id { get { return id; } private set { id = value; } }

        /// <summary>
        /// Max 10000
        /// </summary>
        private int weight;
        public int Weight { get { return weight; } private set { weight = value; } }

        private int damage;
        public int Damage { get { return damage; } private set { damage = value; } }

        private float attackSpeed;
        public float AttackSpeed { get { return attackSpeed; } private set { attackSpeed = value; } }

        private EGrade grade;
        private EGrade Grade { get { return grade; } set { grade = value; } }

        private string spriteName;
        public string SpriteName { get { return spriteName; } private set { spriteName = value; } }
    }
