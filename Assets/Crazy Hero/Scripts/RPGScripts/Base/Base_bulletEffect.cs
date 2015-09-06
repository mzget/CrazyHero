using UnityEngine;
using System;
using System.Collections;

public class Base_bulletEffect : MonoBehaviour {
		
	public event EventHandler ShotTargetComplete_event;
	protected void OnShotTargetComplete_event (EventArgs e)
	{
		if (ShotTargetComplete_event != null)
			ShotTargetComplete_event (this, e);
	}
	
	// Use this for initialization
	protected virtual void Start () {
	
	}
	
	internal virtual void Release(ref GameObject targetEnemy) {
		if(targetEnemy == null) {
			Destroy(this.gameObject);
			return;
		}
	}
}
