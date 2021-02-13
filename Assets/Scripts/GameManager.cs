using UnityEngine;
using System;


//Formula -> (endurance * 2) + (armorDefense) + (level * 3) + (willPower - Random(1-10)) //if > 0

//When damage around 10, multiplay with willPower attack and defense

public class GameManager : MonoBehaviour
{
    //Formula -> (resistance * 10) + (level * 4) + 10
    public Int32 calculateHealth(Entity entity)
    {
        return (entity.resistance * 10) + (entity.level * 4) + 10;
    }

    //Formula -> (intelligence * 10) + (level * 4) + 10
    public Int32 calculateMana(Entity entity)
    {
        return (entity.intelligence * 10) + (entity.level * 4) + 6;
    }

    //Formula -> (agility * 10) + (level * 4) + 10
    public Int32 calculateStamina(Entity entity)
    {
        return (entity.resistance + entity.willPower) + (entity.level * 2) + 5;
    }

    //Formula -> (strength * 2) + (weaponDamage * 2) + (level * 3) + Random(1, 20)
    public Int32 calculateDamage(Entity entity, int weaponDamage)
    {
        System.Random random = new System.Random();
        return (entity.strength * 2) + (weaponDamage * 2) + (entity.level * 3) + random.Next(1, 20);
    }

    //Formula -> (endurance * 2) + (armorDefense) + (level * 3)
    public Int32 calculateDefense(Entity entity, int armorDefense)
    {
        System.Random random = new System.Random();
        return (entity.endurance * 2) + (entity.level * 3) + armorDefense;
    }
}
