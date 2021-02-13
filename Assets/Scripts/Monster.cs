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
    public float respawTime = 10;


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

        if(wayPointList.Length > 0){
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

    void Update()
    {
        if(entity.death)
            return;

        if(entity.currentHealth < 1){
            entity.currentHealth = 0;
            Die();
            entity.death = true;
        }

        if(!entity.inCombat){
            if(wayPointList.Length > 0)
                Patrol();
        }
        else{
            if(entity.attackTimer > 0)
                entity.attackTimer -= Time.deltaTime;;

            if(entity.attackTimer < 0)
                entity.attackTimer = 0;

            if(entity.objectTarget != null && entity.inCombat){
                if(!entity.combatCoroutine)
                    StartCoroutine(Attack());
                
            }
            else{
                entity.combatCoroutine = false;
                StopCoroutine(Attack());
            }
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.tag == "Player" && !entity.death){
            Debug.Log("Player nas proximidades");
            entity.inCombat = true;
            entity.objectTarget = collider.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.tag == "Player"){
            Debug.Log("Player saiu das proximidades");
            entity.inCombat = false;
            entity.objectTarget = null;
        }
    }

    IEnumerator Attack(){
        entity.inCombat = true;

        while(true){
            yield return new WaitForSeconds(entity.coolDown);

            if(entity.objectTarget != null && !entity.objectTarget.GetComponent<Player>().entity.death){
                float distance = Vector2.Distance(entity.objectTarget.transform.position, transform.position);

                if(distance < (entity.attackDistance)){
                    int mosterDamage = gameManager.calculateDamage(entity, entity.damage);

                    int targetDefense = gameManager.calculateDefense(entity.objectTarget.GetComponent<Player>().entity, entity.objectTarget.GetComponent<Player>().entity.defense);
                    int resultAttack = mosterDamage - targetDefense;

                    if(resultAttack < 1) resultAttack = 0;

                    entity.objectTarget.GetComponent<Player>().entity.currentHealth -= resultAttack;
                    Debug.Log("Resultado do ataque -> "+ resultAttack);
                }
            }
        }
    }

    IEnumerator Respawn(){
        yield return new WaitForSeconds(respawTime);

        GameObject newMonster = Instantiate(preFabMonster, transform.position, transform.rotation, null);

        newMonster.name = preFabMonster.name; 
        newMonster.GetComponent<Monster>().entity.death = false;

        Destroy(this.gameObject);
    }

    private void Patrol(){
        if(entity.death)
            return;

        bool notMove = false;
      
        float distanceToTarget = Vector2.Distance(transform.position, targetWayPoint.position);

        if(distanceToTarget < arraivalDistance){// || distanceToTarget >= lastDistanceToTarget){
            animator.SetBool("isWalking", false);
            notMove = true;
            if(currentWaitTime < 1){
                currentWayPoint++;

                if(currentWayPoint >= wayPointList.Length)
                    currentWayPoint = 0;

                targetWayPoint = wayPointList[currentWayPoint];
                lastDistanceToTarget = Vector2.Distance(transform.position, targetWayPoint.position);

                currentWaitTime = waitTime;
            }
            else
                 currentWaitTime -= Time.deltaTime;
        }
        else{
            animator.SetBool("isWalking", true);
            notMove = false;
            lastDistanceToTarget = distanceToTarget;
        }

        if(!notMove){
            Vector2 direction = (targetWayPoint.position - transform.position).normalized * 1600;

            animator.SetFloat("InputX", direction.x);
            animator.SetFloat("InputY", direction.y);

            rigidbody2D.MovePosition(rigidbody2D.position + direction * (entity.speed * Time.deltaTime));
        }
    }


    private void Die(){
        entity.death = true;
        entity.inCombat = false;
        entity.objectTarget = null;
        animator.SetBool("isWalking", false);

        //add xp ao player
        //gameManager.gainXp(rewardExperience);

        Debug.Log("Inimigo is defeat");
        StopAllCoroutines();
        StartCoroutine(Respawn());
    }
}
