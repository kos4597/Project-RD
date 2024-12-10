using System;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    public enum Grade
    {
        Normal = 1,
        Rare = 2,
        Unique = 3,
        Legendary = 4,
        Hidden = 5,
    }

    /// <summary>
    /// Unique Id
    /// </summary>
    private long id = 0;

    public long Id { get { return id; } set { id = value; } }

    private int damage = 0;
    public int Damage { get { return damage; }  set { damage = value; } }

    private int exp = 0;
    public int Exp { get { return exp; } set {  exp = value; } }

    [NonSerialized]
    public Grade grade = 0;

    /// <summary>
    /// Max 10000
    /// </summary>
    private int weight = 0;
    public int Weight { get { return weight; } set { weight = value; } }

    public abstract void Set();

    public abstract void Create();

    public abstract void Attack();
}
