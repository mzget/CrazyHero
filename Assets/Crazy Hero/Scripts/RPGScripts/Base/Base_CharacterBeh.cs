using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

[RequireComponent(typeof(CharacterAnimationManager), typeof(AudioSource))]
public class Base_CharacterBeh : ObjectsBeh {

    public enum CharacterState { Idle = 0, Active, Dead, };
    public CharacterState characterState;

    public enum AnimationState : int { idle = 0, walk, attack, dead, skill };
    public AnimationState curAnimState = AnimationState.idle;
	protected AnimationState previousAnimState;

	public enum FlipState { isLeft, isRight };
	public FlipState flipState { get; protected set; }
	public bool flip = false;
	
	internal const string GAME_EFFECT_TRANSFORM = "gameEffect_transform";
	internal const string CENTER_OF_MASS = "center_of_mass";
	internal Transform gameEffect_transform;
	public Transform center_of_mass;
	public Status stat;
	public AudioClip attackEffect_clip;
	public Base_CharacterBeh targetEnemy;
	
	protected GameObject hud_hpbar;
    [SerializeField] 
	protected Vector3 hpbarOffsetPos;

	protected tk2dTextMesh hp_textmesh;
    protected tk2dSprite hpBar_scrolling;
	internal CharacterAnimationManager animationManager;
	[SerializeField] 
	protected HeroCollisionManager collisionManager;
	private float timestamp;
	protected int damage;

	public float PreviousWalkSpeed;
	
	protected virtual void OnDestroy() { }
	
    protected override void Awake()
    {
        base.Awake();
		animationManager = this.GetComponent<CharacterAnimationManager>();
    }

	//<@-- Call this base function after setup any status.
    protected virtual void InitStatus() {
        this.stat.hp = this.stat.maxHP;
    }

	protected void CreateHUD ()
	{
		hud_hpbar = Instantiate (Resources.Load (PathManager.PATH_OF_HUD_OBJECTS + "Hp_bar", typeof(GameObject))) as GameObject;
		hud_hpbar.transform.parent = this.transform;
		hud_hpbar.transform.localPosition = Vector3.zero + hpbarOffsetPos;

        Transform hp_process = hud_hpbar.transform.Find("Processbar_scroll");
		hpBar_scrolling = hp_process.gameObject.GetComponent<tk2dSprite>();
		hpBar_scrolling.color = Color.green;

		Transform hp_textmesh_transform = hud_hpbar.transform.Find("Hp");
		hp_textmesh = hp_textmesh_transform.gameObject.GetComponent<tk2dTextMesh>();
		hp_textmesh.text = ((int)this.stat.hp).ToString() + "/" + this.stat.maxHP.ToString();
		hp_textmesh.Commit();
        
        if (this.gameObject.tag == TagManager.MONSTER) {
			hud_hpbar.SetActive(false); 
			hp_textmesh.gameObject.SetActive(false);
		}
	}
	
	protected void CreateCenterOfMass() {
		GameObject com = new GameObject(CENTER_OF_MASS);
		center_of_mass = com.transform;
		center_of_mass.parent = this.transform;
        //center_of_mass.localPosition = stat.centerOfMassPos;	
	}

	internal void CreateAttackEffectTransform ()
	{
		GameObject obj = new GameObject (GAME_EFFECT_TRANSFORM);
		gameEffect_transform = obj.transform;
		gameEffect_transform.parent = this.transform;
        //gameEffect_transform.localPosition = stat.attackEffectPos;
		
		GameObject com = new GameObject(CENTER_OF_MASS);
		center_of_mass = com.transform;
		center_of_mass.parent = this.transform;
        //center_of_mass.localPosition = stat.centerOfMassPos;
	}
	
	internal virtual void ReceiveDamage(float p_atk) {
        float realDefense = (stat.defense + stat.addDef) * 100f;

        //<@-- take a damage to character with 3 case of receive attack.
        if(p_atk > realDefense) {
			this.damage = (int)p_atk;
        }
        else if(p_atk == realDefense) {
            float damage = p_atk * 0.2f;
			this.damage = (int)damage;
        }
        else if(p_atk < realDefense) {
            float percent = (p_atk * 100f) / realDefense;
            float damage = p_atk * (percent / 100f);
			this.damage = (int)damage;
        }
		
		this.stat.hp -= this.damage;
        StartCoroutine(ShowHP());
        
 		calProcessBar();
	}

    internal virtual void ReceiveDamage(int _damage, float _multiplier)
    {        
        float realDefense = (stat.defense + stat.addDef) * 100f;
        float p_atk = _damage * _multiplier;

        // take a damage to character with 3 case of receive attack.
        if(p_atk > realDefense) {
			this.damage = (int)p_atk;
        }
        else if(p_atk == realDefense) {
            float damage = p_atk * 0.2f;
			this.damage = (int)damage;
        }
        else if(p_atk < realDefense) {
            float percent = (p_atk * 100f) / realDefense;
            float damage = p_atk * (percent / 100f);
			this.damage = (int)damage; 
        }
		
		this.stat.hp -= this.damage;
        StartCoroutine(ShowHP());
        
 		calProcessBar();
    }

	private IEnumerator ShowHP() {
		timestamp = Time.time;
		if(this.stat.hp > 0)
            hud_hpbar.SetActive (true);
		else
            hud_hpbar.SetActive (false);

		if(this.gameObject.tag == TagManager.MONSTER) {
			yield return new WaitForSeconds(5f);
			if(Time.time - timestamp > 4.5f) {
	            hud_hpbar.SetActive (false);
			}
		}
	}

	protected void calProcessBar() {
        float result = this.stat.hp / this.stat.maxHP;

		if(result >= 0.5f) hpBar_scrolling.color = Color.green;
		else if(result >= 0.3f) hpBar_scrolling.color = Color.yellow;
		else if(result >= 0) hpBar_scrolling.color = Color.red;

        hpBar_scrolling.transform.localScale = new Vector3(result, hpBar_scrolling.scale.y, hpBar_scrolling.scale.z);
		if(hp_textmesh != null) {
			hp_textmesh.text = ((int)this.stat.hp).ToString() + "/" + this.stat.maxHP.ToString();
			hp_textmesh.Commit();
		}
	}

	protected IEnumerator IEUpdateRegen() 
	{
		if(this.stat.hp > 0 && this.stat.hp < this.stat.maxHP) {
			this.stat.hp += this.stat.regen;
			if(this.stat.hp > this.stat.maxHP) {
				this.stat.hp = this.stat.maxHP;
			}
			calProcessBar();
		}
		yield return new WaitForSeconds(0.2f);

		StartCoroutine(IEUpdateRegen());
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();

		if (this.stat.hp <= 0) {
			if(this.characterState != CharacterState.Dead) 
			{
				this.characterState = CharacterState.Dead;
                this.curAnimState = AnimationState.dead;
                this.OnDeadAnimationState();

                if(animationManager.animatedSprite != null) {
					animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Dead);
                    animationManager.animatedSprite.AnimationCompleted += Handle_deadAnimationComplete;
				}
                else if (animationManager.skeletonAnim != null) {
					this.animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Dead, false);
                    this.animationManager.animationComplete_Event += Handle_skeletonAnimated_deadComplete;
				}
				else if(animationManager.animator != null) {
					this.animationManager.animator.Play(CharacterAnimationManager.List_animations.Dead.ToString());
					StartCoroutine(this.WaitForDie());
				}
			}
		}
		
	}

    private IEnumerator WaitForDie()
    {
        while (!this.animationManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
            yield return null;

        if (this.animationManager.animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
			var state = this.animationManager.animator.GetCurrentAnimatorStateInfo(0);
            yield return new WaitForSeconds(state.length);
			this.Dead();
        }
    }

	void StopWalking ()
	{
		if(stat.walkSpeed != 0) {
			PreviousWalkSpeed = stat.walkSpeed;
			stat.walkSpeed = 0f;
		}
	}
	
	protected virtual void OnDeadAnimationState ()
	{
		if(this.GetComponent<Collider2D>() != null)
			this.GetComponent<Collider2D>().enabled = false;
		else 
			collisionManager.GetComponent<Collider2D>().enabled = false;

		this.hud_hpbar.SetActive(false);
        //<@-- Clear effect object when character die.
        Destroy(currentEffectObject);
	}
	
	[System.Obsolete("Handle_deadAnimationComplete is deprecated, please use SkeletonPlay_deadAnimationComplete instead.")]
	protected virtual void Handle_deadAnimationComplete (tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2)
	{
		animationManager.animatedSprite.AnimationCompleted -= Handle_deadAnimationComplete;	
	}

    protected virtual void Handle_skeletonAnimated_deadComplete(object sender, System.EventArgs e)
    {
        this.animationManager.animationComplete_Event -= Handle_skeletonAnimated_deadComplete;
    }

	protected virtual void Dead ()
	{

	}

    internal virtual void LevelUP() {
    
    }

    #region <!-- base character effect.

    public enum CharacterEffectState { normal, rebound, slow, freezing, stop, stunning };
    public CharacterEffectState currentEffectStatus;
    private GameObject currentEffectObject;
    public void BridgeEffect(CharacterEffectState _addEffect, float _damage, int _level, float _time) 
	{
		if(this.curAnimState != AnimationState.dead && characterState != CharacterState.Idle) {
			switch (_addEffect) 
			{
                case CharacterEffectState.normal: { break; }
                case CharacterEffectState.rebound: { ReboundStatus(_level, _time); break; }
                case CharacterEffectState.slow: { 
                    if(currentEffectStatus != CharacterEffectState.slow)
                        SlowStatus(_level, _time);
                    break;
				}
                case CharacterEffectState.freezing: { FreezeStatus(_level, _time); break; }
                case CharacterEffectState.stop: { StopStatus(_level, _time); break; }
                case CharacterEffectState.stunning: { StunningStatus(_level, _time); break; }
			}
			
			if(_damage > 0) {
				this.ReceiveDamage(_damage);
			}
		}
	}

    private void StunningStatus(int _level, float _time)
    {
		currentEffectStatus = CharacterEffectState.stunning;
		StopAllCoroutines();
		Destroy(currentEffectObject);
        StartCoroutine_Auto(NormalStatus(CharacterEffectState.stunning, _time));
		
		if(this.tag == TagManager.MONSTER || this.tag == TagManager.Boss) {
			float targetX = this.transform.position.x + (4f * _level);
			if(targetX < 15f) {
				this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(targetX, this.transform.position.y, this.transform.position.z), 2f);
			}
			else {
				this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(15f, this.transform.position.y, this.transform.position.z), 2f);
			}
		}
        else if(this.tag == TagManager.UNIT || this.tag == TagManager.HERO) {
			if(this.transform.position.x > -7.4f)
				this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(-7.4f, this.transform.position.y, this.transform.position.z), 1f);
		}
			
		this.StopWalking();
        previousAnimState = curAnimState;
        curAnimState = AnimationState.idle;
        if (animationManager.animatedSprite)
            animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Idle);
        else
            animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Idle, true, 1);
    }
	
	private void ReboundStatus(int level, float effectTime) {
		Destroy(currentEffectObject);

        HOTween.To(this.transform, 0.5f, new TweenParms().Prop("position", new Vector3(this.transform.position.x + (2.2f + (level / 2f)), this.transform.position.y, this.transform.position.z)).Ease(EaseType.EaseInSine));
	}

	public void SlowStatus(int p_level, float effectTime) {
		currentEffectStatus = CharacterEffectState.slow;
		StopAllCoroutines();
		StartCoroutine(NormalStatus(CharacterEffectState.slow, effectTime));
		Destroy(currentEffectObject);

        if(this.stat.walkSpeed != 0)
            PreviousWalkSpeed = this.stat.walkSpeed;
        this.stat.walkSpeed = stat.walkSpeed / (2 * p_level);
	}
	
	public void StopStatus(int level, float effectTime) {
		Destroy(currentEffectObject);
		
		this.stat.walkSpeed = 0f;
		if(this.curAnimState != AnimationState.idle) {
		    this.curAnimState = AnimationState.idle;
			if(animationManager.animatedSprite) {
	        	this.animationManager.PlayAnimationByName (CharacterAnimationManager.List_animations.Idle);
			}
			else if(animationManager.skeletonAnim) {
	       		this.animationManager.PlayAnimationByName (CharacterAnimationManager.List_animations.Idle, false);
			}
		}
	}

	protected virtual void FreezeStatus(int level, float effectTime) {
        animationManager.StopAnimation();
		this.StopWalking();
		StopAllCoroutines();
		Destroy(currentEffectObject);

		currentEffectStatus = CharacterEffectState.freezing;

		currentEffectObject = Instantiate(Resources.Load("EffectSpriteAnim/IceFreezing_AnimSprite", typeof(GameObject))) as GameObject;
        currentEffectObject.transform.parent = center_of_mass;
        currentEffectObject.transform.localPosition = new Vector3(0, 0, -0.2f);

		StartCoroutine(NormalStatus(CharacterEffectState.freezing, effectTime));
	}

	IEnumerator NormalStatus(CharacterEffectState effectName, float restoreTime) {
		yield return new WaitForSeconds(restoreTime);
		
	    if(currentEffectStatus != CharacterEffectState.normal)
			currentEffectStatus = CharacterEffectState.normal;
        if (effectName != CharacterEffectState.stunning)
			
        {
            this.stat.walkSpeed = PreviousWalkSpeed;
            this.curAnimState = previousAnimState;
            this.animationManager.StartAnimation();
        }
        else {
            this.stat.walkSpeed = PreviousWalkSpeed;
            this.curAnimState = AnimationState.idle;
            this.animationManager.StartAnimation();
        }

        if (effectName == CharacterEffectState.freezing) {
            Destroy(currentEffectObject);
        }

        this.CompleteNormalStatus();
    }

    protected virtual void CompleteNormalStatus() { }

    #endregion
}

