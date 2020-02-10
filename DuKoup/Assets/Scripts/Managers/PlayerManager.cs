using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Not a singleton : two players

    // Fields
    [SerializeField] private int strength;

    public int getStrength(){
        return this.strength;
    }

    public void addStrength(int toAdd){
        this.strength += toAdd;
    }

    public void decreaseStrength(int toDecre){
        this.strength -= toDecre;
    }

    public void changeStrength(int new_strength){
        this.strength = new_strength;
    }
}
