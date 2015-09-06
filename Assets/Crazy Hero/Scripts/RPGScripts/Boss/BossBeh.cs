using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class BossBeh : Base_CharacterBeh {
	
	public enum StateBeh { Idle = 0, Attack, Creep, Skill, Recessive, }; 
	[SerializeField] private StateBeh currentStateBeh;

	//int planIndex = 0;
	float startTime;
	float lastUpdate;
    [SerializeField] private string action = "";
	public string GetCurrentAction { get { return action; } }

	
	private int _summonid = 0;
	public int Summonid {
		get {return _summonid;}
		set {_summonid = value;}
	}

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
		StartCoroutine_Auto(this.WaitForManager());

		base.InitStatus();	
		base.CreateHUD();
		base.CreateAttackEffectTransform();

		this.action = StateBeh.Idle.ToString();
		curAnimState = Base_CharacterBeh.AnimationState.skill;
		base.characterState = CharacterState.Active;

		StartCoroutine_Auto(base.IEUpdateRegen());
	}

	IEnumerator WaitForManager ()
	{
		while (!BossManager.GetInstance.isReady) {
			yield return null;
		}
		base.stat = BossManager.GetInstance.GetStatByName(this.name);
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();

        if (characterState != CharacterState.Active) 
			return;
        if (currentEffectStatus == CharacterEffectState.stunning) 
			return;
	}

	void OnCollisionEnter2D(Collision2D _otherCol) {
		if(_otherCol.gameObject.tag == TagManager.HERO) {
			_otherCol.transform.parent.SendMessage("ReceiveDamage", base.stat.attack, SendMessageOptions.DontRequireReceiver);
//			this.Reflexive();
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

	private void CheckFlip(bool _flip)
	{
 		if(animationManager.skeletonAnim) {
			if(_flip) {
            	animationManager.animation_data_obj.transform.localScale = 
					new Vector3( animationManager.original_animationScale.x * (-1), animationManager.original_animationScale.y, animationManager.original_animationScale.z);
			}
	        else {
	            animationManager.animation_data_obj.transform.localScale = animationManager.original_animationScale;
			}
		}
	}
	
	void SufflePlan(List<string> _currentPlan) {		
		StopAllCoroutines();
		
        int Result = Random.Range(0, _currentPlan.Count);
        action = _currentPlan[Result];

        print("Boss action : " + action);
	}
	
    void Exit()
    {
        targetEnemy = null;
    }
	
	void Idle() {
		if(this.curAnimState != AnimationState.idle) {
		    this.curAnimState = AnimationState.idle;
		    this.animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Idle, true);
		}
	}

	void Walk() {
		if(this.curAnimState != AnimationState.walk) {
		    this.curAnimState = AnimationState.walk;
		    this.animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk, true);
        }
	}

    void Attack()
    {
        if (targetEnemy == null || targetEnemy.characterState == Base_CharacterBeh.CharacterState.Dead)
        {
            animationManager.animationComplete_Event -= SkeletonAnimationComplete_Event;
            return;
        }

        if (this.curAnimState != AnimationState.dead)
        {
            if (targetEnemy)
            {
                this.curAnimState = AnimationState.attack;
                if (animationManager.skeletonAnim != null)
                {
                    if (animationManager.skeletonAnim.AnimationName == CharacterAnimationManager.List_animations.Attack.ToString())
                        return;
                    animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Attack, false, stat.attackSpeed);
                    animationManager.animationComplete_Event += new System.EventHandler(SkeletonAnimationComplete_Event);
                }
            }
        }
	}

    private void SkeletonAnimationComplete_Event(object sender, System.EventArgs e)
    {
        animationManager.animationComplete_Event -= SkeletonAnimationComplete_Event;
        if (targetEnemy)
        {
            if (stat.attackEffectName != string.Empty)
            {
                this.PreparingAttackEffect();
            }
            else
            {
                if (targetEnemy.stat.unitType == Status.UnitType.wall)
                    targetEnemy.ReceiveDamage(stat.attack, 2f);
                else
                    targetEnemy.ReceiveDamage(stat.attack);
                this.Idle();
            }
        }
        else
        {
            this.Idle();
        }
    }

    void PreparingAttackEffect()
    {
        Transform target = targetEnemy.transform.Find(CENTER_OF_MASS);
        if (target != null)
        {
            GameObject obj = target.gameObject;
            GameObject attackEffect = this.CreateAttackEffect();
            attackEffect.name = base.stat.attackEffectName;
            BulletBeh bull = attackEffect.GetComponent<BulletBeh>();
            bull.Release(ref obj);
            bull.ShotTargetComplete_event += Handle_ShotTargetComplete_event;
        }
    }

    private GameObject CreateAttackEffect()
    {
        GameObject effect = null;

        effect = GameEffectManager.Instance.CreateBulletEffect(base.stat.attackEffectName, base.gameEffect_transform.position);
        effect.transform.position += Vector3.back;

        return effect;
    }

    void Handle_ShotTargetComplete_event(object sender, System.EventArgs e)
    {
        if (targetEnemy != null)
        {
            targetEnemy.SendMessage("ReceiveDamage", base.stat.attack, SendMessageOptions.DontRequireReceiver);
            this.Idle();
        }
    }

    protected override void OnDeadAnimationState()
    {
        base.OnDeadAnimationState();

        BattleStage stage = MzBaseScene.GetInstance as BattleStage;
        stage.GameEnd();
    }
    protected override void Dead()
    {
		base.Dead();

        Destroy(hud_hpbar);
		Destroy(this.gameObject);
		
		GUIManager.GetInstance.ActiveLevelCompleteWindow();
		//<@-- Share opengraph.
//		if (MzFacebookIntegration.GetInstance == null) 
//			return;
//		MzFacebookIntegration.GetInstance.CreateOG_DefeatedBoss();

        System.GC.Collect();
    }

    protected override void Handle_skeletonAnimated_deadComplete(object sender, System.EventArgs e)
    {
        base.Handle_skeletonAnimated_deadComplete(sender, e);
        
		Destroy (hud_hpbar);
		Destroy (this.gameObject);

        System.GC.Collect();
    }

    protected override void FreezeStatus(int level, float effectTime)
    {
        base.FreezeStatus(level, effectTime);

        HOTween.Pause(this.transform);
    }

    protected override void CompleteNormalStatus()
    {
        base.CompleteNormalStatus();

        HOTween.Play(this.transform);
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
