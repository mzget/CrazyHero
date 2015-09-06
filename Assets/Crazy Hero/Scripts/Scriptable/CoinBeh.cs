using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class CoinBeh : MonoBehaviour {

    internal int point = 10;
    Vector3 targetPos;
	Transform[] coinsT;

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds(1f);

        this.GetComponent<Animator>().enabled = false;
		coinsT = this.GetComponentsInChildren<Transform>();		
		targetPos = Camera.main.ScreenToWorldPoint(GUIManager.GetInstance.coinIcon_screenPos);

		foreach (var item in coinsT) {
            HOTween.To(item, 0.1f, new TweenParms().Prop("position", targetPos)
                .Ease(EaseType.EaseInSine)
                .OnComplete(() =>
                {
                    GUIManager.GetInstance.AddGold(point);
                    Destroy(this.gameObject);
                }));
        }
	}
	
	// Update is called once per frame
	void Update () {		
	}
}
