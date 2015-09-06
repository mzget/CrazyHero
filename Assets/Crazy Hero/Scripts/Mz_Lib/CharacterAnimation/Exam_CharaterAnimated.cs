using UnityEngine;
using System.Collections;

public class Exam_CharaterAnimated : MonoBehaviour {

	public CharacterAnimationManager animationManager;

	CharacterAnimationManager.List_animations nameList_0;
	CharacterAnimationManager.List_animations nameList_1;
	CharacterAnimationManager.List_animations nameList_2;
	CharacterAnimationManager.List_animations nameList_3;
	CharacterAnimationManager.List_animations nameList_4;

	// Use this for initialization
	void Start () {
		nameList_0 = (CharacterAnimationManager.List_animations)0;
		nameList_1 = (CharacterAnimationManager.List_animations)1;
		nameList_2 = (CharacterAnimationManager.List_animations)2;
		nameList_3 = (CharacterAnimationManager.List_animations)3;
		nameList_4 = (CharacterAnimationManager.List_animations)4;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,  new Vector3(Screen.width/ Main.FixedGameWidth, Screen.height/Main.FixedGameHeight, 1));

		if(GUI.Button(new Rect(0,0, 150,50), nameList_0.ToString())) {
			animationManager.PlayAnimationByName(nameList_0);
		}
		else if(GUI.Button(new Rect(0,50, 150,50), nameList_1.ToString())) {
			animationManager.PlayAnimationByName(nameList_1);
		}
		else if(GUI.Button(new Rect(0,100, 150,50), nameList_2.ToString())) {
			animationManager.PlayAnimationByName(nameList_2);
		}
		else if(GUI.Button(new Rect(0, 150, 150,50), nameList_3.ToString())) {
			animationManager.PlayAnimationByName(nameList_3);
		}
		else if(GUI.Button(new Rect(0, 200, 150,50), nameList_4.ToString())) {
			animationManager.PlayAnimationByName(nameList_4);
		}
	}
}
