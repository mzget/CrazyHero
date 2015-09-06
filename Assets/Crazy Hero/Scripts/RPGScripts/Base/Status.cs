using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Status : BaseStatusData {

    public enum RangeType { Malee = 0, Range = 1, Fly = 2, };
    public RangeType rangeType;

    public enum Attributes { none = 0, Strength, Agility, Intelligence, Trap, SlowEffect, Wall };
    public Attributes attributes;

    public enum UnitType { none = 0, soldier, trap, wall, untrap, };
    public UnitType unitType;

    public enum BossType { none = 0, normalBoss, bigBoss, };
    public BossType bossType;

	public void LevelUP ()
	{	
		if(level == maxLevel) {
			Debug.LogWarning("Current level is max level");
			return;
		}
		
		this.level += 1;
       
        if(this.attributes == Status.Attributes.Strength) {
            this.attack += 5;
            //this.attackSpeed += 0.01f;
            this.defense += 0.1f;
            this.maxHP += 150;
            this.regen += 0.5f;
			this.cost += (this.cost / 10f * this.level) * 0.5f;
        }
        else if(this.attributes == Status.Attributes.Agility) {
            this.attack += 6;
            //this.attackSpeed += 0.05f;
            this.defense += 0.08f;
            this.maxHP += 100;
            this.regen += 0.3f;
            this.cost += (this.cost / 10f * this.level) * 0.5f;
        }
        else if(this.attributes == Status.Attributes.Intelligence) {
            this.attack += 7;
            //this.attackSpeed += 0.01f;
            this.defense += 0.05f;
            this.maxHP += 120;
            this.regen += 0.3f;
            this.cost += (this.cost / 10f * this.level) * 0.5f;
        }
        else if(this.attributes == Attributes.Trap) {
            this.attack += 50;
            //this.attackSpeed += 0.1f;
            this.defense += 0.2f;
            this.maxHP += 0;
            this.regen += 0f;
            this.cost += (this.cost / 10f * this.level) * 0.6f;
        }
        else if(this.attributes == Attributes.SlowEffect) {
            //this.attack += 0;
            //this.attackSpeed += 0.1f;
            this.defense += 0.2f;
            this.maxHP += 0;
            this.regen += 0f;
            this.cost += (this.cost / 10f * this.level) * 0.6f;
        }
        else if(this.attributes == Attributes.Wall) {
            this.attack += 0;
            //this.attackSpeed += 0f;
            this.defense += 0.5f;
            this.maxHP += 200;
            this.regen += 0f;
            this.cost += (this.cost / 10f * this.level) * 0.6f;
        }
        else if(this.attributes == Attributes.none) {
			Debug.LogWarning("Attributes stat is none !");
        }
	}
    
    internal void HeroLevelUp()
    {
        this.level += 1;
        if (this.attributes == Status.Attributes.Strength) {
            this.attack += 10;
            this.attackSpeed += 0.01f;
            this.defense += 0.5f;
            this.maxHP += 150;
            this.regen += 0.5f;
        }
        else if (this.attributes == Status.Attributes.Agility) {
            this.attack += 12;
            this.attackSpeed += 0.05f;
            this.defense += 0.3f;
            this.maxHP += 100;
            this.regen += 0.3f;
        }
        else if (this.attributes == Status.Attributes.Intelligence) {
            this.attack += 15;
            this.attackSpeed += 0.02f;
            this.defense += 0.25f;
            this.maxHP += 120;
            this.regen += 0.3f;
        }
    }
}
