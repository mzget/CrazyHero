using UnityEngine;
using System;
using System.Collections;

public class TavernBeh : MonoBehaviour {

	float distanceToHero;
	float visibleRange = 6;
	bool _isHeroStaying = false;
	bool _isHeroExitFromTavern = true;

	public event Action heroStayTavern_event;
	protected virtual void OnHeroStayTavern_event ()
	{
		if (heroStayTavern_event != null)
			heroStayTavern_event ();
	}
	protected event Action heroeExitTavern_event;
	protected virtual void OnHeroeExitTavern_event ()
	{
		if (heroeExitTavern_event != null)
			heroeExitTavern_event ();
	}

	// Use this for initialization
	void Start () {
		this.heroStayTavern_event += Handle_heroStayTavern_event;
		this.heroeExitTavern_event += Handle_heroeExitTavern_event;
	}

	void Handle_heroeExitTavern_event ()
	{
		HeroBeh.GetInstance.stat.regen -= 20;
		GUIManager.GetInstance.HideTavernTag();
	}

	void Handle_heroStayTavern_event ()
	{
		MonsterManager.GetInstance.MonsterResurrection();
		HeroBeh.GetInstance.stat.regen += 20;
		HeroBeh.GetInstance.SaveRebornPos(this.transform.position.x);
		GUIManager.GetInstance.ShowTavernTag();
	}
	
	// Update is called once per frame
	void Update () {
		distanceToHero = Vector3.Distance(this.transform.position, HeroBeh.GetInstance.transform.position);
		if(distanceToHero < visibleRange) {
			//<@-- active.
			_isHeroExitFromTavern = false;
			if(_isHeroStaying == false) {
				_isHeroStaying = true;
				Debug.Log("_isHeroStaying: " + _isHeroStaying);
				this.OnHeroStayTavern_event();
			}
		}
		else {
			_isHeroStaying = false;
			if(_isHeroExitFromTavern == false) {
				_isHeroExitFromTavern = true;
				Debug.Log("_isHeroExitFromTavern: " + _isHeroExitFromTavern);
				this.OnHeroeExitTavern_event();
			}
		}
    }
}
