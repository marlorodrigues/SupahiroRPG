using System.Collections;
using System.Collections.Generic;

//Criar um sistema de xp para mosters tbm

using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("Controler")]
    public Entity entity = new Entity();
    public Animator animator;
    public GameManager gameManager;
    public Rigidbody2D rigidbody2D;

    [Header("Patrol")]
    public Transform[] wayPointList;
    public float arraivalDistance = 0.5f;
    public float waitTime = 5f;
    private Transform targetWayPoint;
    private int currentWayPoint = 0;
    private float lastDistanceToTarget = 0f;
    private float currentWaitTime = 0f;

    [Header("Reward")]
    public int experience;
    public int lootGoldMin = 1;
    public int lootGoldMax = 10;
    
    [Header("Respaw")]
    public GameObject preFabMonster;
    public bool respaw = true;
    public float respawTime = 10f;


    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        entity.maxHealth = gameManager.calculateHealth(entity);
        entity.maxMana = gameManager.calculateMana(entity);
        entity.maxStamina = gameManager.calculateStamina(entity);

        //Set values initials with max values
        entity.currentHealth = entity.maxHealth;
        entity.currentMana = entity.maxMana;
        entity.currentStamina = entity.maxStamina;

        currentWaitTime = waitTime;

        if(wayPointList.length > 0){
            targetWayPoint = wayPointList[currentWayPoint];
            lastDistanceToTarget = Vector2.Distance(transform.position, targetWayPoint.position);
        }

        // health.maxValue = entity.maxHealth;
        // health.value = health.maxValue;
        // mana.maxValue = entity.maxMana;
        // mana.value = mana.maxValue;
        // stamina.maxValue = entity.maxStamina;
        // stamina.value = stamina.maxValue;
        // experience.value = 0;
    }



}
