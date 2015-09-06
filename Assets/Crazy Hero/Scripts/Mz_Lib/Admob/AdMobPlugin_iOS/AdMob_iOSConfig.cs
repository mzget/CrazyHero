using System.Collections;
using UnityEngine;

// Example script showing how you can easily call into the AdMobPlugin.
public class AdMob_iOSConfig : MonoBehaviour {
	private static AdMob_iOSConfig instance;
	public static AdMob_iOSConfig GetInstance {
		get{
			if(instance == null) {
				AdMob_iOSConfig.Init();
			}
			return instance; 
		}
	}
	
	
	public static void Init() {		
		if(instance == null) {
			GameObject admob = new GameObject("AdmobPlugins", typeof(AdMob_iOSConfig), typeof(AdMob_iOSPlugin));
			instance = admob.GetComponent<AdMob_iOSConfig>();
		}
	}
	
	string console = string.Empty;

    void Start()
	{		
		DontDestroyOnLoad(this);
		console = ("Created Banner View");
		AdMob_iOSPlugin.CreateBannerView("a152a93531b1093", AdMob_iOSPlugin.AdSize.IABBanner, true);

		this.SendRequestNewAd();
    }
	
	public void SendRequestNewAd() {
		console = ("Requested Banner Ad");
        // Pass in any extras you have as JSON.
//        string extras = "{\"color_bg\":\"AAAAFF\", \"color_bg_top\":\"FFFFFF\"}";
		string extras = "{\"color_bg\":\"000000\"}";
		AdMob_iOSPlugin.RequestBannerAd(false, extras);
		AdMob_iOSPlugin.ShowBannerView();
	}

    void OnEnable()
	{
        console = ("Registering for AdMob Events");

        AdMob_iOSPlugin.ReceivedAd += HandleReceivedAd;
        AdMob_iOSPlugin.FailedToReceiveAd += HandleFailedToReceiveAd;
        AdMob_iOSPlugin.ShowingOverlay += HandleShowingOverlay;
        AdMob_iOSPlugin.DismissingOverlay += HandleDismissingOverlay;
        AdMob_iOSPlugin.DismissedOverlay += HandleDismissedOverlay;
        AdMob_iOSPlugin.LeavingApplication += HandleLeavingApplication;
    }

    void OnDisable()
	{
        console = ("Unregistering for AdMob Events");

        AdMob_iOSPlugin.ReceivedAd -= HandleReceivedAd;
        AdMob_iOSPlugin.FailedToReceiveAd -= HandleFailedToReceiveAd;
        AdMob_iOSPlugin.ShowingOverlay -= HandleShowingOverlay;
        AdMob_iOSPlugin.DismissingOverlay -= HandleDismissingOverlay;
        AdMob_iOSPlugin.DismissedOverlay -= HandleDismissedOverlay;
        AdMob_iOSPlugin.LeavingApplication -= HandleLeavingApplication;
    }

    public void HandleReceivedAd()
	{
        console = ("HandleReceivedAd event received");
    }

    public void HandleFailedToReceiveAd(string message)
	{
        console = ("HandleFailedToReceiveAd event received with message:");
    }

    public void HandleShowingOverlay()
	{
        console = ("HandleShowingOverlay event received");
    }

    public void HandleDismissingOverlay()
	{
        console = ("HandleDismissingOverlay event received");
    }

    public void HandleDismissedOverlay()
	{
        console = ("HandleDismissedOverlay event received");
    }

    public void HandleLeavingApplication()
	{
        console = ("HandleLeavingApplication event received");
    }

	/*
	private void OnGUI() {
		///Scale the button sizes for retina displays
		float screenScale = (float)(Screen.width / 1024f);
		Matrix4x4 scaledMatrix = Matrix4x4.Scale(new Vector3(screenScale, screenScale, screenScale));
		GUI.matrix = scaledMatrix;
		
		if(GUI.Button(new Rect(1024- 300, 768 -175, 300, 75), "Toggle RemoveAds : ")) {
			_CanDisplayAd = !_CanDisplayAd;
//			PlayerPrefsX.SetBool(KEY_CAN_DISPLAY_AD, _CanDisplayAd);

			if(_CanDisplayAd) {
				AdMobPlugin.ShowBannerView();
				SendRequestNewAd();
			}
			else {
				AdMobPlugin.HideBannerView();
			}
		}
	}*/
}
