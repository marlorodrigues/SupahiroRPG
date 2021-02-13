using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    
    [Header("Player Regen System")]
    public bool regenEnabled_HP = true;
    public float regenTime_HP = 5f;
    public int regenValue_HP = 5;

    public bool regenEnabled_MN = true;
    public float regenTime_MN = 5f;
    public int regenValue_MN = 5;

    public bool regenEnabled_STM = true;
    public float regenTime_STM = 5f;
    public int regenValue_STM = 5;

    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("PlayerUI")]
    public Slider health;
    public Slider mana;
    public Slider stamina;
    public Slider experience;
    public Entity entity;

    void Start()
    {
        if (gameManager == null)
        {
            Debug.Log("Game Manager nao anexado no script player");
            return;
        }

        entity.maxHealth = gameManager.calculateHealth(entity);
        entity.maxMana = gameManager.calculateMana(entity);
        entity.maxStamina = gameManager.calculateStamina(entity);

        int damage = gameManager.calculateDamage(entity, 10); //usado no player
        int defense = gameManager.calculateDefense(entity, 8); //usado no inimigo

        //Set values initials with max values
        entity.currentHealth = entity.maxHealth;
        entity.currentMana = entity.maxMana;
        entity.currentStamina = entity.maxStamina;

        health.maxValue = entity.maxHealth;
        health.value = health.maxValue;

        mana.maxValue = entity.maxMana;
        mana.value = mana.maxValue;

        stamina.maxValue = entity.maxStamina;
        stamina.value = stamina.maxValue;

        experience.value = 0;



        StartCoroutine(regenHealth());
        StartCoroutine(regenMana());
        StartCoroutine(regenStamina());
    }

    private void Update()
    {
        //Get current controller value and apply on player
        health.value = entity.currentHealth;
        mana.value = entity.currentMana;
        stamina.value = entity.currentStamina;


        if(Input.GetKeyDown(KeyCode.Space)){
            entity.currentHealth -= 10;
            entity.currentMana -= 10;
            entity.currentStamina -= 10;
        }
    }

    IEnumerator regenHealth(){
        while (true)
        {
            if(regenEnabled_HP){
                if(entity.currentHealth < entity.maxHealth)
                    entity.currentHealth += regenValue_HP;
                else
                    yield return null;

                yield return new WaitForSeconds(regenTime_HP);
            }
        }
    }

    IEnumerator regenMana(){
        while (true)
        {
            if(regenEnabled_MN){
                if(entity.currentMana < entity.maxMana)
                    entity.currentMana += regenValue_MN;
                else
                    yield return null;

                yield return new WaitForSeconds(regenTime_MN);
            }
            else 
                yield return null;
        }
    }

    IEnumerator regenStamina(){
        while (true)
        {
            if(regenEnabled_STM){
                if(entity.currentStamina < entity.maxStamina){
                    entity.currentStamina += regenValue_STM;
                }
                yield return new WaitForSeconds(regenTime_STM);
            }
            else 
                yield return null;
        }
    }
}
