using UnityEngine;
using System.Collections;
using System;

public class BulletBeh : Base_bulletEffect {
	
    protected bool enableShot = false;
    protected GameObject target;

	protected virtual void Awake() {
		iTween.Init(this.gameObject);
	}
	
	internal override void Release (ref GameObject targetEnemy)
	{			
		base.Release (ref targetEnemy);
		this.Shot(ref targetEnemy);
	}
	
    protected virtual void Shot(ref GameObject targetEnemy)
    {
        enableShot = true;
		target = targetEnemy;

		if(enableShot && target != null) {		
			iTween.MoveTo(this.gameObject, iTween.Hash("position", target.transform.position + (Vector3.back * 0.2f), "speed", 18f, "easetype", iTween.EaseType.linear,
			                                           "oncomplete", "ShotComplete", "oncompletetarget", this.gameObject));
		}
    }

    private void ShotComplete() {
		OnShotTargetComplete_event (EventArgs.Empty);
        Destroy(this.gameObject);
    }
}
