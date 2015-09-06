using UnityEngine;
using System.Collections;

public class ObjectsBeh : Base_ObjectBeh {

	internal tk2dSprite sprite;

    protected bool _isTouchEnded = false;
    protected bool _isTouchHold = false;
	protected bool _canActive = false;	
    internal Vector3 originalPos;
    internal Vector3 originalScale;

    #region <!-- Events data.

    //<!-- destroyObj_Event.
	private event System.EventHandler destroyObj_Event;
    protected void OnDestroyObject_event(System.EventArgs e) {
        if (destroyObj_Event != null) {
            destroyObj_Event(this, e);
            Debug.Log(destroyObj_Event + ": destroyObj_Event : " + this.name);
        }
    }
    internal System.EventHandler ObjectsBeh_destroyObj_Event;

    #endregion

    
	internal void SetOriginTransform(Vector3 newOriginPos, Vector3 newOriginalScale) {
		this.originalPos = newOriginPos;
        this.originalScale = newOriginalScale;
	}
	
	protected virtual void Awake() {
        sprite = this.gameObject.GetComponent<tk2dSprite>();
	}
	
	// Use this for initialization
	protected virtual void Start () {
        this.SetOriginTransform(this.transform.localPosition, this.transform.localScale);
        
        destroyObj_Event += (ObjectsBeh_destroyObj_Event);
	}
}
