using UnityEngine;
using System.Collections;

public class DamageEffectBeh : MonoBehaviour {

	internal int damage;
	internal TextMesh textMesh;

	void Awake() {
		textMesh = this.GetComponent<TextMesh>();
	}

	// Use this for initialization
	IEnumerator Start () {
		textMesh.text = damage.ToString();

		yield return new WaitForSeconds(3f);

		Destroy(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position += Vector3.up * (Time.deltaTime * 10f);
	}
}
