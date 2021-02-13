using UnityEngine;
using System;

[Serializable]
public class Entity
{
    [Header("Nome")]
    public string name;

    public int level;
    
    [Header("Health")]
    public int currentHealth;
    public int maxHealth;

    [Header("Mana")]
    public int currentMana;
    public int maxMana;

    [Header("Stamina")]
    public int currentStamina;
    public int maxStamina;

    [Header("Status")]
    public int strength;
    public int damage;
    public int endurance;
    public int resistance;
    public int defense;
    public float speed = 2.5f;
    public int intelligence = 1;
    public int willPower;

    [Header("Combat")]
    public float attackDistance = 0.5f;
    public float attackTimer = 1f;
    public float coolDown = 2;
    public bool inCombat = false;
    public GameObject objectTarget;
    public bool combatCoroutine = false;
    public bool death = false;


}
