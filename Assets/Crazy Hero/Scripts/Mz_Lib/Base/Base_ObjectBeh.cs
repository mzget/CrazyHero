using UnityEngine;
using System.Collections;

public class Base_ObjectBeh : MonoBehaviour {    
    protected bool _OnTouchBegin = false;
	protected bool _OnTouchMove = false;
	protected bool _OnTouchRelease = false;
	
	protected virtual void Update() {       
        if (_OnTouchBegin && _OnTouchRelease) {
            OnTouchDown();
        }
	}

	#region <!-- On Mouse Events.

	protected virtual void OnMouseDown() {
        //Debug.Log("Base_ObjectBeh." + "OnMouseDown");

        if(_OnTouchBegin == false)
			_OnTouchBegin = true;
	}
	
	protected virtual void OnMouseDrag() {
//        Debug.Log("Class : Base_ObjectBeh." + "OnTouchDrag");
		_OnTouchMove = true;
	}
    protected virtual void OnTouchDown()
    {
//		Debug.Log("OnTouchDown : " + this.gameObject.name);

        /// do something.
		
        _OnTouchBegin = false;
        _OnTouchRelease = false;
		_OnTouchMove = false;
    }

    protected virtual void OnMouseUp() {
    
    }

    protected virtual void OnMouseUpAsButton ()
	{
        //Debug.Log("Class : Base_ObjectBeh." + "OnMouseUpAsButton");

		if(_OnTouchBegin)
			_OnTouchRelease = true;

		_OnTouchMove = false;
    }

    #endregion
}
