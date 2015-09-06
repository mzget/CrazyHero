using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;

public class SocialGameIntegration : MonoBehaviour {
#if UNITY_WEBPLAYER

    private static SocialGameIntegration Instance;
    public static SocialGameIntegration GetInstance {
        get {
			Instance = GameObject.FindObjectOfType<SocialGameIntegration>();
            if (Instance == null) {
                InitSingleton();
            } 
            return Instance; 
        }
    }
    private static void InitSingleton() {
        if (Instance == null) {
			GameObject gObj = GameObject.Find("Social_BG");
            Instance = gObj.GetComponent<SocialGameIntegration>();
        }
        else {
            Debug.LogWarning("This object is singleton implemented, Please create only once instance.");
        }
    }

    private WorldMap stageController;

	[SerializeField] private GameObject socialGroupUI;
	public TextMesh welcomeTextmesh;
	public MzGUIButtonBeh facebookConnectButton;
    public MzGUIButtonBeh leaderboards_button;
    public MzGUIButtonBeh closeButton;
    public tk2dTextMesh facebook_statusTextmesh;
    public tk2dSpriteFromTexture profileFromTexture;
    [SerializeField]
    private SpriteRenderer profileSpriteRenderer;
	public tk2dTextMesh scoreTextmesh;

	// Use this for initialization
	void Start() {
		stageController = MzBaseScene.GetInstance as WorldMap;

		facebookConnectButton.clickEvent += FacebookConnectButtonclick_event;
//		leaderboards_button.clickEvent += HandleLeaderboards_click_event;
		leaderboards_button._isEnable = false;
		leaderboards_button.sprite.color = Color.grey;
        closeButton.clickEvent += CloseButton_click_event;
	}

	void Handle_getProfilePictureComplete ()
	{
		MzFacebookIntegration.GetInstance.GetProfileTextureCompleteEvent -= Handle_getProfilePictureComplete;
		this.LoadFBProfilePicture();
	}

	void LoadFBProfilePicture() {
		if(MzFacebookIntegration.GetInstance.profileTexture != null) {
            if (profileFromTexture != null)
            {
                TextureScale.Bilinear(MzFacebookIntegration.GetInstance.profileTexture, 150, 132);
                tk2dSpriteCollectionSize size = new tk2dSpriteCollectionSize();
                size.type = tk2dSpriteCollectionSize.Type.Explicit;
                size.orthoSize = 10;
                size.height = 768;
                profileFromTexture.Create(size, MzFacebookIntegration.GetInstance.profileTexture, tk2dBaseSprite.Anchor.MiddleCenter);
            }
            else {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                block.AddTexture("_MainTex", MzFacebookIntegration.GetInstance.profileTexture);
                profileSpriteRenderer.SetPropertyBlock(block);
            }
		}
		else {
			Debug.LogWarning("ProfileTexture is null !");
		}
	}


    // Update is called once per frame
    void Update()
    {

    }

    private void CloseButton_click_event(object sender, EventArgs e)
    {
        this.CloseWindow();
    }

	void FacebookConnectButtonclick_event(object sender, EventArgs e)
	{		
		MzFacebookIntegration.GetInstance.GetProfileTextureCompleteEvent += Handle_getProfilePictureComplete;
		
		if (MzFacebookIntegration.GetInstance.isInit) {
			if (FB.IsLoggedIn) { 
				//<@-- logout. 
				MzFacebookIntegration.GetInstance.LogoutComplete_delegate += () => {
					this.RefreshFacebookStatus();
				};
				MzFacebookIntegration.GetInstance.CallFBLogout();
			}
			else { 
				MzFacebookIntegration.GetInstance.GetMyDataComplete_Event += () => {
					this.RefreshFacebookStatus();
					MzFacebookIntegration.GetInstance.CreateOG_PlayingStory();
				};
				//<@-- connect.
				MzFacebookIntegration.GetInstance.CallFBLogin();
			}
		}
	}

	void RefreshFacebookStatus()
    {
		if (MzFacebookIntegration.GetInstance.isInit) {
			MzFacebookIntegration.GetInstance.ManageProfilePicture();
			if (FB.IsLoggedIn) { 
				//<@-- show logout stat text. 
				facebook_statusTextmesh.text = "Logout";
                facebook_statusTextmesh.Commit();
				welcomeTextmesh.text = "Welcome: " + SaveManager.GetInstance.username;
            }
            else { 
                //<@-- show connect stat text.
                facebook_statusTextmesh.text = "Login";
				facebook_statusTextmesh.Commit();
				welcomeTextmesh.text = "Welcome: Guest";
			}
		}
		else {
			Debug.LogWarning("FB.Init: " + MzFacebookIntegration.GetInstance.isInit);
			
			//<@-- show connect stat text.
			facebook_statusTextmesh.text = "Login";
			facebook_statusTextmesh.Commit();
			welcomeTextmesh.text = "Welcome: Guest";
		}
	}

	public void OpenWindow ()
	{
		MzFacebookIntegration.GetInstance.GetProfileTextureCompleteEvent += Handle_getProfilePictureComplete;
		this.RefreshFacebookStatus();	

        stageController.coverDarkPlane.SetActive(true);
        HOTween.To(socialGroupUI.transform, 0.5f, new TweenParms().Prop("localPosition", new PlugVector3Y(0)).Ease(EaseType.EaseOutBounce));
	}

    private void CloseWindow()
	{
		MzFacebookIntegration.GetInstance.GetProfileTextureCompleteEvent -= Handle_getProfilePictureComplete;

        stageController.coverDarkPlane.SetActive(false);
        HOTween.To(socialGroupUI.transform, 0.5f, new TweenParms().Prop("localPosition", new PlugVector3Y(8)).Ease(EaseType.EaseInSine));
    }

#endif
}
