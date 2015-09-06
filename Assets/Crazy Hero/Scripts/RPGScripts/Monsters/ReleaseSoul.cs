using UnityEngine;
using System.Collections;

public class ReleaseSoul : MonoBehaviour {

//	// Use this for initialization
	public GameObject soul_obj;

	void Awake() {
		iTween.Init(this.gameObject);
	}

	void Start() {
//        Vector3 worldPos = Camera.main.ScreenToWorldPoint(GUIManager.GetInstance.topCenter_screenPos);
//		iTween.MoveTo(this.gameObject, iTween.Hash("position", worldPos, "time", 1, "easetype", iTween.EaseType.easeInSine));
	}

	public void setPositionSoul(Vector3 postion, GameObject soul) {
		soul_obj = soul;
		soul_obj.transform.position = postion;		
		this.Invoke("DestroyThisSoul", 2);
	}

	private void DestroyThisSoul() {
		Destroy(soul_obj);
	}

}
