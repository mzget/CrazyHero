using UnityEngine;
using System.Collections;

public class HeroCollisionManager : MonoBehaviour {

	private HeroBeh hero;

	// Use this for initialization
	void Start () {
		hero = this.transform.parent.GetComponent<HeroBeh>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		this.transform.localPosition = Vector3.zero;
	}


	void OnCollisionEnter2D(Collision2D _collision) {
		if(_collision.gameObject.tag == TagManager.MONSTER) {
			float r = Random.Range(0f, 100f);
			if(r <= HeroBeh.GetInstance.stat.sumCriRate) {
				int damage = hero.stat.attack + hero.stat.addAtk;
				_collision.gameObject.GetComponent<Base_CharacterBeh>().ReceiveDamage(damage, 2f);
				hero.Reflexive();
			}
			else {
				int damage = hero.stat.attack + hero.stat.addAtk;
				_collision.gameObject.SendMessage("ReceiveDamage", damage, SendMessageOptions.DontRequireReceiver);
				hero.Reflexive();
			}
		}
	} 
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.name == "Potion") {
			Destroy(other.gameObject);
			StartCoroutine_Auto(hero.IEAddRegeneration(200, 5f));
		}
		else if(other.gameObject.name == "Star") { 
			Destroy(other.gameObject);
			hero.AddAgelessStatus(5f);
		}
	}
}
