using UnityEngine;
using System.Collections;

public class MonsterBeh : Base_CharacterBeh {

	private BattleStage stage;

	float distanceToHero;
	float visibleRange = 8;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		base.stat = new Status();
		base.stat.maxHP = 100;
		base.stat.attack = 40;
        base.stat.givenEXP = 20;

        base.hpbarOffsetPos = new Vector3(0, -0.8f, 0);
        base.InitStatus();
		base.CreateHUD();

		stage = (BattleStage)MzBaseScene.GetInstance;
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();

		distanceToHero = Vector2.Distance(this.transform.position, HeroBeh.GetInstance.transform.position);
		if(distanceToHero <= visibleRange) {
			//@-- Check flip sprite.
			this.CheckFlipSprite();
		}
	}

	void CheckFlipSprite() {
		if (HeroBeh.GetInstance.transform.position.x > this.transform.position.x) {
			flip = true;
			flipState = FlipState.isLeft;
		}
		else {
			flip = false;
			flipState = FlipState.isRight;
		}
		
		if(flip) {
			animationManager.animation_data_obj.transform.localScale = 
				new Vector3( animationManager.original_animationScale.x * (-1), animationManager.original_animationScale.y, animationManager.original_animationScale.z);
		}
		else {
			animationManager.animation_data_obj.transform.localScale = animationManager.original_animationScale;
		}
	}

	void OnCollisionEnter2D(Collision2D _collision) {
		if(_collision.gameObject.tag == TagManager.HERO) {
			_collision.transform.parent.SendMessage("ReceiveDamage", base.stat.attack, SendMessageOptions.DontRequireReceiver);
            this.Reflexive();
		}
	}

    private void Reflexive()
    {
        if (flipState == FlipState.isRight) {
            this.transform.Translate(Vector3.right * Time.deltaTime * 60f);
        }
        else if (flipState == FlipState.isLeft) {
            this.transform.Translate(Vector3.left * Time.deltaTime * 60f);
        }
    }

	protected override void OnDeadAnimationState ()
	{
		base.OnDeadAnimationState();

        HeroBeh.GetInstance.ReceiveEXP(base.stat.givenEXP);
		this.RandomDropItems();
        stage.AddMonsterKill();

		Destroy(this.gameObject);
	}

	void RandomDropItems ()
	{
		int r = Random.Range(0, 100);
		if(r < 3) {
			this.DropStar();
		}
		else if(r >= 3 && r < 8)
			this.DropPotion();
		else
			this.DropCoin();
	}

	void DropPotion ()
	{
        Vector3 _vectorEffect = Vector3.zero;
        _vectorEffect = (flipState == FlipState.isRight) ? _vectorEffect = Vector3.right : _vectorEffect = Vector3.left;

		GameObject potion = Instantiate(stage.potion_obj, this.transform.position + new Vector3(_vectorEffect.x, 0.3f, -1f), Quaternion.identity) as GameObject;
        potion.name = "Potion";
		stage.audioEffect.PlayOnecSound(stage.coin_clip);
	}

	void DropStar ()
	{
        Vector3 _vectorEffect = Vector3.zero;
        _vectorEffect = (flipState == FlipState.isRight) ? _vectorEffect = Vector3.right : _vectorEffect = Vector3.left;

		GameObject _star = Instantiate(stage.star_obj, this.transform.position + new Vector3(_vectorEffect.x, 1f, -1f), Quaternion.identity) as GameObject;
        _star.name = "Star";
		stage.audioEffect.PlayOnecSound(stage.coin_clip);
	}

	void DropCoin ()
	{
		GameObject coins = Instantiate(stage.coin_obj, this.transform.position + new Vector3(0, 0.4f, -1f), Quaternion.identity) as GameObject;
		stage.audioEffect.PlayOnecSound(stage.coin_clip);
	}

	internal override void ReceiveDamage (float p_atk)
	{
		base.ReceiveDamage (p_atk);
		
		var randomX = Random.Range(-2f, 2f);
		var effect = Instantiate(MonsterManager.GetInstance.damageEffectObj, this.transform.position + new Vector3(randomX, 0, -1f), Quaternion.identity) as GameObject;
		effect.GetComponent<DamageEffectBeh>().damage = base.damage;
	}

	internal override void ReceiveDamage (int _damage, float _multiplier)
	{
		base.ReceiveDamage (_damage, _multiplier);

		var randomX = Random.Range(-2f, 2f);
		var effect = Instantiate(MonsterManager.GetInstance.damageEffectObj, this.transform.position + new Vector3(randomX, 0, -1f), Quaternion.identity) as GameObject;
		DamageEffectBeh effectBeh = effect.GetComponent<DamageEffectBeh>();
		effectBeh.damage = base.damage;
		effectBeh.textMesh.color = Color.red;
		effectBeh.textMesh.fontSize = 20;
	}
}
