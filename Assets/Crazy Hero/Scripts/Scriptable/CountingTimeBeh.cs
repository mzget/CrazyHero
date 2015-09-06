using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CountingTimeBeh : MonoBehaviour {

    public static List<CountingTimeBeh> countingTimeBeh_list = new List<CountingTimeBeh>();

    public bool canUpdatable = true;
	public float duration;
	public tk2dTextMesh displayTimeRemain_Textmesh;
	public event Action countingTimeComplete_event;

	private float counting;
	private float timeRemain;
	private float nextTime = 1f;
	private float changeTextRate = 1f;
	
	// Use this for initialization
	void Start () { }
	
	// Update is called once per frame
	void Update () {
        if (canUpdatable == false) 
            return;

		counting += Time.deltaTime;
		if(counting >= duration) {
			counting = 0;
			
			if(countingTimeComplete_event != null)
				countingTimeComplete_event();
			
			Destroy(this.gameObject);
		}
		
		timeRemain = duration - counting;
		
		if (Time.time > nextTime) {
            nextTime = Time.time + changeTextRate;
			displayTimeRemain_Textmesh.text = timeRemain.ToString("f0");
			displayTimeRemain_Textmesh.Commit();
        }
	}
}
