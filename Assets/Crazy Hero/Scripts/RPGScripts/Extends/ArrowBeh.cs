using UnityEngine;
using System.Collections;

public class ArrowBeh : BulletBeh {

	protected override void Awake ()
	{
		base.Awake ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected override void Shot (ref GameObject targetEnemy)
	{			
		base.Shot (ref targetEnemy);		
		
		float deltaY = target.transform.position.y - this.transform.position.y;
		float deltaX = target.transform.position.x - this.transform.position.x;
		float angle = Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI;
		this.transform.Rotate(new Vector3(0,0, angle));
	}
}
