using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HeroData {
    public string name;
    public float curExp;
	public Status stat;
};

public class HeroBeh : Base_CharacterBeh {

	private static HeroBeh instance;
	public static HeroBeh GetInstance {
		get{
			if(instance == null) {
				instance = GameObject.FindObjectOfType<HeroBeh>();
				if(instance == null)
					Debug.LogError("Hero instance is null.");
			}

			return instance;
		}
	}

	private BattleStage stageController;
	bool isAgelessStat;
	internal bool isReady = false;
	
	private GameObject particleRegen;
	private GameObject rebornEffect;
    private GameObject agelessEffect;
	protected GameObject cooldown_Obj;
    public float curExp;
    public float nextExp;

	float minLeftPosX = -6.5f, maxRightPosX = 405f;
	Vector3 rebornPos = new Vector3(0, -3.2f, 0);
	public void SaveRebornPos(float xPos)
	{
		float oldXPos = rebornPos.x;
		if(xPos > oldXPos)
			rebornPos = new Vector3(xPos, -3.2f, 0);
	}

    // Use this for initialization
	protected override void Start () {
		base.Start ();

		stageController = MzBaseScene.GetInstance as BattleStage;
        ExpManager.InitSingleton();

		StartCoroutine_Auto(IEWaitForManager());

        rebornEffect = Resources.Load("ParticleFX/RebornEffect", typeof(GameObject)) as GameObject;
        cooldown_Obj = Instantiate(Resources.Load("Prototypes/HUD/" + "Cooldown_TextMesh", typeof(GameObject))) as GameObject;
        cooldown_Obj.transform.parent = this.transform;
        cooldown_Obj.transform.localPosition = new Vector3(0, 3, 0);
		cooldown_Obj.SetActive(false);

		StartCoroutine_Auto(base.IEUpdateRegen());
	}

	IEnumerator IEWaitForManager ()
	{
		while (!HeroManager.Get_Instance.isReady) {
			yield return null;
		}
		
		this.InitStatus();
	}

	protected override void InitStatus ()
	{
		HeroData heroData = HeroManager.Get_Instance.dict_heroData[this.name];
		
		base.stat = new Status();
        base.stat = heroData.stat;		
		this.curExp = heroData.curExp;
		this.nextExp = ExpManager.GetInstance.Get_NextExp(base.stat.level, TagManager.HERO);

		base.InitStatus ();
        base.hpbarOffsetPos = new Vector3(0, -0.8f, 0);
		base.CreateHUD();
        base.CreateAttackEffectTransform();

		this.isReady = true;
	}

    // Update is called once per frame
	protected override void Update () {
		if(!this.isReady) return;
        if (characterState == Base_CharacterBeh.CharacterState.Dead) return;
		if(stageController.curGameplayState == BattleStage.GameplayState.gameEnd) {
			animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Idle, true);
			return;
		}

        base.Update();
		//        this.UpdateExpLabel();
		stat.sumWalkSpd = stat.walkSpeed + stat.addSpd;
		stat.sumCriRate = stat.critical + stat.addCri;

		if(this.transform.position.x < minLeftPosX) {
			this.transform.position = new Vector3(minLeftPosX, this.transform.position.y, this.transform.position.z);
			if(animationManager.skeletonAnim == null)
				animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Idle);
			else
				animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Idle, true);

			return;
		}
        else if (this.transform.position.x > maxRightPosX) {
            this.transform.position = new Vector3(maxRightPosX, this.transform.position.y, this.transform.position.z);
            if (animationManager.skeletonAnim == null)
                animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Idle);
            else
                animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Idle, true);

            return;
        }
		
		this.UpdateInput();
	}

    private void UpdateInput()
    {
		if(MzBaseScene.GetInstance._isPauseGameplay) return;
        if (characterState == CharacterState.Dead) return;

        if (Application.isEditor || Application.isWebPlayer || Application.platform == RuntimePlatform.WindowsPlayer) {
            if (Input.GetMouseButton(0)) {
				Ray cursorRay = GUIManager.GetInstance.guiCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				if(Physics.Raycast(cursorRay, out hit)) {
					if(hit.collider.tag == TagManager.GUItag) {
						this.Idle();
					}
					else {
                        this.Run();
					}
				}
				else {
					this.Run();
				}
            }
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) {
                this.Run();
			}

			if(Input.GetMouseButtonUp(0)|| Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) {
                this.Idle();
			}
		}
		else if(!Application.isEditor && !Application.isWebPlayer) {
			if(Input.touchCount == 1) {
				Touch touch =  Input.GetTouch(0);
				if(touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
					Ray cursorRay = GUIManager.GetInstance.guiCamera.ScreenPointToRay(touch.position);
					RaycastHit hit;
					if(Physics.Raycast(cursorRay, out hit)) {
						if(hit.collider.tag == TagManager.GUItag) {
							this.Idle();
						}
						else {
							this.Run();
						}
					}
					else {
						this.Run();
					}
				}
				
				if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
					this.Idle();
				}
			}
		}
    }

	void Run ()
	{
		if(Application.isEditor || Application.isWebPlayer) {
	        if (Input.GetMouseButton(0)) { 
		    	Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		    	this.CheckFlipCharWithCurrentPos(ref cursorPos);
			    if (cursorPos.x > this.transform.position.x) {
				    if(this.transform.position.x >= minLeftPosX && this.transform.position.x < maxRightPosX) {
					    if(animationManager.skeletonAnim == null && curAnimState != AnimationState.walk) {
						    curAnimState = AnimationState.walk;
						    animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk);
					    }
					    else if(animationManager.skeletonAnim && curAnimState != AnimationState.walk) {
						    curAnimState = AnimationState.walk;
						    animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk, true);
					    }
						this.transform.Translate(Vector3.right * Time.deltaTime * base.stat.sumWalkSpd, Space.World);
				    }
			    }
			    else if(cursorPos.x < this.transform.position.x) {
				    if(this.transform.position.x > minLeftPosX && this.transform.position.x <= maxRightPosX) {
					    if(animationManager.skeletonAnim == null && curAnimState != AnimationState.walk) {
						    curAnimState = AnimationState.walk;
						    animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk);
					    }
					    else if(animationManager.skeletonAnim && curAnimState != AnimationState.walk) {
						    curAnimState = AnimationState.walk;
						    animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk, true);
					    }
						this.transform.Translate(Vector3.left * Time.deltaTime * base.stat.sumWalkSpd, Space.World);
				    }
			    }
        	}
	        else { 
			    if (Input.GetKey(KeyCode.D)) {
	                this.SetFlipCharacter(FlipState.isRight);
					if(this.transform.position.x >= minLeftPosX && this.transform.position.x < maxRightPosX) {
					    if(animationManager.skeletonAnim == null && curAnimState != AnimationState.walk) {
						    curAnimState = AnimationState.walk;
						    animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk);
					    }
					    else if(animationManager.skeletonAnim && curAnimState != AnimationState.walk) {
						    curAnimState = AnimationState.walk;
						    animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk, true);
					    }
						this.transform.Translate(Vector3.right * Time.deltaTime * base.stat.sumWalkSpd, Space.World);
				    }
			    }
			    else if(Input.GetKey(KeyCode.A)) {
	                this.SetFlipCharacter(FlipState.isLeft);
					if(this.transform.position.x > minLeftPosX && this.transform.position.x <= maxRightPosX) {
					    if(animationManager.skeletonAnim == null && curAnimState != AnimationState.walk) {
						    curAnimState = AnimationState.walk;
						    animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk);
					    }
					    else if(animationManager.skeletonAnim && curAnimState != AnimationState.walk) {
						    curAnimState = AnimationState.walk;
						    animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk, true);
					    }
						this.transform.Translate(Vector3.left * Time.deltaTime * base.stat.sumWalkSpd, Space.World);
				    }
			    }
        	}
		}
		else if(!Application.isEditor && !Application.isWebPlayer) {
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) { 
				Vector3 cursorPos = Camera.main.ScreenToWorldPoint(touch.position);
				this.CheckFlipCharWithCurrentPos(ref cursorPos);
				if (cursorPos.x > this.transform.position.x) {
					if(this.transform.position.x >= minLeftPosX && this.transform.position.x < maxRightPosX) {
						if(animationManager.skeletonAnim == null && curAnimState != AnimationState.walk) {
							curAnimState = AnimationState.walk;
							animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk);
						}
						else if(animationManager.skeletonAnim && curAnimState != AnimationState.walk) {
							curAnimState = AnimationState.walk;
							animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk, true);
						}
						this.transform.Translate(Vector3.right * Time.deltaTime * base.stat.sumWalkSpd, Space.World);
					}
				}
				else if(cursorPos.x < this.transform.position.x) {
					if(this.transform.position.x > minLeftPosX && this.transform.position.x <= maxRightPosX) {
						if(animationManager.skeletonAnim == null && curAnimState != AnimationState.walk) {
							curAnimState = AnimationState.walk;
							animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk);
						}
						else if(animationManager.skeletonAnim && curAnimState != AnimationState.walk) {
							curAnimState = AnimationState.walk;
							animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Walk, true);
						}
						this.transform.Translate(Vector3.left * Time.deltaTime * base.stat.sumWalkSpd, Space.World);
					}
				}
			}
		}
	}

	protected void CheckFlipCharWithCurrentPos (ref Vector3 cursorPos)
	{
        if (cursorPos.x < this.transform.position.x)
            this.SetFlipCharacter(FlipState.isLeft);
        else
            this.SetFlipCharacter(FlipState.isRight);
	}

    private void SetFlipCharacter(FlipState _flip)
    {
        this.flip = (_flip == FlipState.isLeft) ? true : false;
        flipState = _flip;
        
		if(flip) {
            animationManager.animation_data_obj.transform.localScale = 
				new Vector3( animationManager.original_animationScale.x * (-1), animationManager.original_animationScale.y, animationManager.original_animationScale.z);
		}
        else {
            animationManager.animation_data_obj.transform.localScale = animationManager.original_animationScale;
		}
    }

    internal void Reflexive()
    {
        if (flipState == FlipState.isRight) {
            this.transform.position -= Vector3.right * 0.5f;
        }
        else if (flipState == FlipState.isLeft) {
            this.transform.position += Vector3.right * 0.5f;
        }
    }

    internal IEnumerator IEAddRegeneration(int HP, float duration)
    {
        float regenPerSec = HP / duration;
        base.stat.regen += regenPerSec;
        this.SetActiveRegenEffect(true);
        yield return new WaitForSeconds(duration);
        base.stat.regen -= regenPerSec;
        this.SetActiveRegenEffect(false);
 //       StartCoroutine_Auto(this.Regenerate(dera));
    }
	
	internal void AddAgelessStatus (float _duration)
	{
		this.isAgelessStat = true;
		StartCoroutine_Auto(this.WaitForAddAgslessComplete(_duration));
	}

	IEnumerator WaitForAddAgslessComplete (float _duration)
	{
        SetActiveAgelessEffect(true);
		yield return new WaitForSeconds(_duration);
		this.isAgelessStat = false;
        SetActiveAgelessEffect(false);
	}
	
	void SetActiveRegenEffect(bool active) {	
		if(active) {
			if(particleRegen == null) {
				particleRegen = Instantiate(Resources.Load("ParticleFX/HealEffect")) as GameObject;
				particleRegen.name = "HealEffect";
				particleRegen.transform.parent = this.transform;
				particleRegen.transform.localPosition = new Vector3(0, 1.7f, -1f);
			}
			else{
				particleRegen.SetActive(true);
			}
		}
		else {
			particleRegen.SetActive(false);
		}
	}

	void SetActiveAgelessEffect(bool _active) {
		if(_active) {
			if(agelessEffect == null) {
                agelessEffect = Instantiate(Resources.Load("ParticleFX/Ageless Effect")) as GameObject;
                agelessEffect.name = "Ageless Effect";
                agelessEffect.transform.parent = this.transform;
                agelessEffect.transform.localPosition = new Vector3(0, 1.8f, -1f);
			}
			else{
                agelessEffect.SetActive(true);
			}
		}
		else {
            agelessEffect.SetActive(false);
		}
	}
		
	#region <!-- Animation.

	private void Idle () {
		this.characterState = CharacterState.Idle;
		if(curAnimState != AnimationState.idle) {
			base.curAnimState = AnimationState.idle;
			if (animationManager.skeletonAnim == null) { 
				base.animationManager.PlayAnimationByName (CharacterAnimationManager.List_animations.Idle);
			}
			else {
				base.animationManager.PlayAnimationByName(CharacterAnimationManager.List_animations.Idle, true);
			}
		}
	}

	protected override void OnDeadAnimationState ()
	{
		base.OnDeadAnimationState ();

        StartCoroutine_Auto(this.CountingTimeReborn(stat.level));
	}
	
	#endregion

	override internal void LevelUP ()
	{
		base.stat.level += 1;
        this.nextExp = ExpManager.GetInstance.Get_NextExp(base.stat.level, TagManager.HERO);

        if(stat.attributes == Status.Attributes.Strength) {
            stat.attack += 10;
            stat.attackSpeed += 0.01f;
            stat.defense += 0.2f;
            stat.maxHP += 150;
            stat.regen += 0.2f;
        }
        else if(stat.attributes == Status.Attributes.Agility) {
            stat.attack += 12;
            stat.attackSpeed += 0.05f;
            stat.defense += 0.1f;
            stat.maxHP += 100;
            stat.regen += 0.1f;
        }
        else if(stat.attributes == Status.Attributes.Intelligence) {
            stat.attack += 15;
            stat.attackSpeed += 0.02f;
            stat.defense += 0.1f;
            stat.maxHP += 120;
            stat.regen += 0.1f;
        }
		else {
			Debug.LogException(new System.Exception("hero instance is not setup attribute type!"));
		}
	}

    public IEnumerator AddStatusWithDuaration(string _stat, float _value, float _duration)
    {
        switch (_stat)
        {
            case "defense":
                stat.defense += _value;
                yield return new WaitForSeconds(_duration);
                stat.defense -= _value;
                break;
            default:
                break;
        }
    }

	#region <!-- Reborn system.

    private System.Action countingComplete_delegate;
    private GameObject countingRebornTimerObj;
    private IEnumerator CountingTimeReborn(int _heroLevel)
    {
		float minTime = 3f;
        float timeToReborn = _heroLevel + (_heroLevel * 10f / 100f) + minTime;

		this.cooldown_Obj.SetActive(true);
        print(this.name + ": wait for reborn => : " + timeToReborn);
		
		countingRebornTimerObj = new GameObject(this.name + ": Counting", typeof(CountingTimeBeh));
		CountingTimeBeh countingBeh = countingRebornTimerObj.GetComponent<CountingTimeBeh>();
		countingBeh.duration = timeToReborn;
		countingBeh.displayTimeRemain_Textmesh = this.cooldown_Obj.GetComponent<tk2dTextMesh>();

        countingComplete_delegate = delegate() { 
            this.Reborn();
            countingBeh.countingTimeComplete_event -= countingComplete_delegate;
        };
        countingBeh.countingTimeComplete_event += countingComplete_delegate;

        CountingTimeBeh.countingTimeBeh_list.Add(countingBeh);

        yield return null;
    }

	void Reborn ()
	{
		print(this.name + ": Reborn");

        this.cooldown_Obj.SetActive(false);
        
        //<@-- Reborn effect.
        //GameObject effect = Instantiate(rebornEffect) as GameObject;
        //effect.transform.parent = this.transform;
        //effect.transform.localPosition = Vector3.zero;			
        //StartCoroutine(this.DestroyRebornEffect(effect));

		if(this.GetComponent<Collider2D>() != null) 
			this.GetComponent<Collider2D>().enabled = true;
		else 
			collisionManager.GetComponent<Collider2D>().enabled = true;
		this.transform.position = rebornPos;
		this.stat.hp = stat.maxHP;

        //animationManager.animation_data_obj.renderer.enabled = (true);
        //animationManager.skeletonAnim.Initialize();
		this.Idle();
		MonsterManager.GetInstance.MonsterResurrection();
	}

    internal void ForceReborn() {
        Destroy(countingRebornTimerObj);
        this.Reborn();
    }

	IEnumerator DestroyRebornEffect (GameObject effect)
	{
		yield return new WaitForSeconds(1f);
		Destroy(effect);
	}

	#endregion

    internal void ReceiveEXP(int _exp)
    {
        if (curExp + _exp <= this.nextExp) { 
            this.curExp += _exp;
        }
        else {
            var previousMax = this.nextExp;
            var previousExp = this.curExp;
            this.LevelUP();
            var newExp = previousExp += _exp;
            var remainExp = newExp - previousMax;
            this.curExp = remainExp;
            //@-- show level message.
			StartCoroutine_Auto(GUIManager.GetInstance.ShowLevelUpGameEffect());
            //<@-- play level sound.
            BattleStage stage = (BattleStage)MzBaseScene.GetInstance;
            MzBaseScene.GetInstance.audioEffect.PlayOnecSound(stage.levelUpClip);
        }
    }

	internal override void ReceiveDamage (float p_atk)
	{
		if(isAgelessStat) return;

		base.ReceiveDamage (p_atk);
	}

	internal override void ReceiveDamage (int _damage, float _multiplier)
	{
		if(isAgelessStat) return;

		base.ReceiveDamage (_damage, _multiplier);
	}

    void OnGUI() {         
	    ///Scale the button sizes for retina displays
	    float screenScale = (float)(Screen.width / 1280f);
	    Matrix4x4 scaledMatrix = Matrix4x4.Scale(new Vector3(screenScale, screenScale, screenScale));
	    GUI.matrix = scaledMatrix;

        GUI.Box(new Rect(0, 720 - 50, 300, 50), base.stat.regen.ToString());
    }
}
