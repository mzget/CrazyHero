using UnityEngine;
using System.Collections;

public class DragableObject : ObjectsBeh {

    public bool _canDragaable = false;
    protected bool _isDraggable = false;
    protected bool _isDropObject = false;
    protected Camera GUICamera;

    protected override void Start()
    {
        base.Start();

        var gui = GameObject.FindGameObjectWithTag("GUICamera");
        if(gui != null)
            GUICamera = gui.GetComponent<Camera>();
    }

	protected virtual void ImplementDraggableObject() {
        Vector3 worldPoint;
        Camera cam = (this.gameObject.layer == LayerMask.NameToLayer("GUI")) ? GUICamera : Camera.main;

        if (Input.touchCount >= 1) {
            worldPoint = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
        }
        else {
            worldPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        }
		
        this.transform.position = new Vector3(worldPoint.x, worldPoint.y, cam.transform.position.z + 10f);
	}

    // Update is called once per frame	
    protected override void Update()
    {
        base.Update();

        if (_canDragaable)
        {
            if (_isDraggable)
            {
                this.ImplementDraggableObject();
            }

            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touchCount > 0)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)
                    {
                        if (this._isDraggable)
                            _isDropObject = true;
                    }
                }
            }
            else if (Application.isWebPlayer || Application.isEditor)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (this._isDraggable)
                        _isDropObject = true;
                }
            }
        }
    }

    protected override void OnMouseDrag()
    {
        base.OnMouseDrag();

        if (this._canDragaable && base._OnTouchBegin)
        {
            this._isDraggable = true;
        }
    }

    protected override void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();

        if (this._isDraggable)
            _isDropObject = true;
    }
}
