using UnityEngine;
using System.Collections;

public class ExpManager {
	private static ExpManager _Instance;
	public static ExpManager GetInstance {
		get {
			if(_Instance == null) {
				Debug.LogError("ExpManager object is null..");
			}
			return _Instance;
		}
	}
	
	
	public float exp;
	public float next_exp;
	public float HERO_EXP_RATIO = 1.8f;
	public float UNIT_EXP_RATIO = 1.3f;

	public static void InitSingleton ()
	{
		if(_Instance == null)
			_Instance = new ExpManager();
	}	
	
	public ExpManager() {
		exp = 150;
	}
	
	internal float Get_NextExp(int current_lv, string type) {
		next_exp = 0;
		if(type == TagManager.HERO) {
			this.CalcExpEquation(current_lv, HERO_EXP_RATIO);//exp * HERO_EXP_RATIO;
		}
		else if(type == TagManager.UNIT) {
			this.CalcExpEquation(current_lv, UNIT_EXP_RATIO);
		}
		
		Debug.Log("next exp: " + next_exp);
		return next_exp; 
	}

	void CalcExpEquation (int _level, float _ratio)
 	{		
		next_exp = exp * (Mathf.Pow(_ratio, (float)_level));
	}
}
