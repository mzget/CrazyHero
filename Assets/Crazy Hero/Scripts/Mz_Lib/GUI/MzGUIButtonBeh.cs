/// <summary>
/// Mz_ GUI button beh.
/// this component create by mzget. For use to make button easier.
/// </summary>
/// Update 2014-3-21


using UnityEngine;
using System;
using System.Collections;

[AddComponentMenu("Mz_Lib/GUI/MzGUIButtonBeh")]
public class MzGUIButtonBeh : Base_ObjectBeh {

	public enum ButtonState { up, down, };
    public tk2dSprite sprite;
    public SpriteRenderer spRenderer;
	public string upStateName;
	public string downStateName;	
	public bool enablePlayAudio = true;
	public bool enableChangeScale = true;
    public bool enableSendMessage = false;
    internal bool _isEnable = true;
	private MzBaseScene gameController;
    private Vector3 originalScale;

	public event EventHandler clickEvent;
    public event Action mouseDownEvent;
    public event Action mouseUpEvent;
	protected void OnClick_event (EventArgs e)
	{
        if (clickEvent != null)
            clickEvent(this, e);
	}
    	
    void Awake() {
		sprite = this.gameObject.GetComponent<tk2dSprite>();
        if (sprite == null)
            spRenderer = this.GetComponent<SpriteRenderer>();
		gameController = MzBaseScene.GetInstance;

        this.originalScale = this.transform.localScale;
	}

	// Use this for initialization
    void Start () { }

    void OnEnable() {		
		this.transform.localScale = originalScale;	
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();

        if (_isEnable == false) return;

        if (mouseDownEvent != null)
            mouseDownEvent();

        this.ChangeImages(ButtonState.down);
        if (enableChangeScale)
            this.transform.localScale = this.transform.localScale * 1.1f;

        if (this.enablePlayAudio && gameController.audioEffect != null)
            gameController.audioEffect.PlayOnecSound(gameController.audioEffect.buttonDown_Clip);
    }

	protected override void OnTouchDown ()
	{
		base.OnTouchDown ();

        if (_isEnable == false) return;

        if(enableSendMessage)
            gameController.OnInput(this.gameObject.name);

		OnClick_event (EventArgs.Empty);
	}

    protected override void OnMouseUp()
    {
        base.OnMouseUp();

        if (!_isEnable) return;
        
        this.ChangeImages(ButtonState.up);
        if (enableChangeScale)
            this.transform.localScale = originalScale;
        if (mouseUpEvent != null)
            mouseUpEvent();
    }

    protected override void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();

        if (_isEnable == false) return;

        if (enableChangeScale)
            this.transform.localScale = originalScale;	
        if (mouseUpEvent != null)
            mouseUpEvent();
    }
	
	internal void ChangeImages(ButtonState buttonState) {
		if(buttonState == ButtonState.up) {
            if(upStateName != string.Empty)
            {
                if(sprite != null)
                    this.sprite.spriteId = this.sprite.GetSpriteIdByName(upStateName);
            }
		}
		else if(buttonState == ButtonState.down) {
            if (downStateName != string.Empty) { 
                if(sprite != null)
                    this.sprite.spriteId = this.sprite.GetSpriteIdByName(downStateName);
            }
		}
	}
}