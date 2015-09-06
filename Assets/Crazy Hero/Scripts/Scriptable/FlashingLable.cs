using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class FlashingLable : MonoBehaviour {
	
	private tk2dSprite sprite;
	
	// Use this for initialization
	void Start () {
		sprite = this.GetComponent<tk2dSprite>();
		
		Color colorTo = new Color(0.6f, 0.6f, 0.6f, 1f);
		HOTween.To(sprite, 0.5f, new TweenParms().Prop("color", colorTo).Loops(-1, LoopType.Yoyo).Ease(EaseType.EaseInOutSine));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
