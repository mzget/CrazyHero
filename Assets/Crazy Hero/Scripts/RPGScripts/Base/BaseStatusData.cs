using System;
using UnityEngine;

[System.Serializable]
public class BaseStatusData
{
	//<@-- Basical status.
	public int level = 1;
	public int maxLevel = 255;
	public int attack;
    public float critical;
	public float regen = 0f;
    public float defense = 1f; // default for all.
	public float maxHP = 100f;
	public float hp;
	public float walkSpeed;
	public float sumWalkSpd;
	public float sumCriRate;
    public float attackSpeed = 1f;
    public string attackEffectName = string.Empty;
	public float cost;	
    public int dropSoul;
    public int givenEXP;
	//<@-- upgrading status.
    public int addAtk = 0;
	public int attackStar = 0;
	public int attackLevel = 0;
	public float addDef = 0;
	public int defenseStar = 0;
	public int defenseLevel = 0;
	public float addCri = 0;
	public int criticalStar = 0;
	public int criticalLevel = 0;
	public float addSpd = 0;
	public int speedStar = 0;
	public int speedLevel = 0;
}

