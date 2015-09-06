using UnityEngine;

// Example script showing how you can easily call into the AdMobPlugin.
public class AdMob_androidConfig : MonoBehaviour {
#if UNITY_ANDROID

    private static AdMob_androidConfig Instance;
    public static AdMob_androidConfig GetInstance {
        get {
            if(Instance == null) {
                AdMob_androidConfig.Init();
            }
            return Instance; 
		}
    }

    public static void Init()
    {   
        if(Instance == null) {
            GameObject admob_object = new GameObject("AdmobObject", typeof(AdMob_androidPlugin), typeof(AdMob_androidConfig));
            Instance = admob_object.GetComponent<AdMob_androidConfig>();
        }
    }
	
    void Start()
    {
        print("Created Banner View");
        AdMob_androidPlugin.CreateBannerView("a150c2e14a5d753", AdMob_androidPlugin.AdSize.SmartBanner, true);

        this.CallNewAdRequest();
    }

    public void CallNewAdRequest() 
	{
        print("Requested Banner Ad");
        AdMob_androidPlugin.RequestBannerAd(false);
        AdMob_androidPlugin.ShowBannerView();
    }

    void OnEnable()
    {
		print("Registering for AdMob Events");
        AdMob_androidPlugin.ReceivedAd += HandleReceivedAd;
        AdMob_androidPlugin.FailedToReceiveAd += HandleFailedToReceiveAd;
        AdMob_androidPlugin.ShowingOverlay += HandleShowingOverlay;
        AdMob_androidPlugin.DismissedOverlay += HandleDismissedOverlay;
        AdMob_androidPlugin.LeavingApplication += HandleLeavingApplication;
    }

    void OnDisable()
    {
        print("Unregistering for AdMob Events");
		AdMob_androidPlugin.ReceivedAd -= HandleReceivedAd;
        AdMob_androidPlugin.FailedToReceiveAd -= HandleFailedToReceiveAd;
        AdMob_androidPlugin.ShowingOverlay -= HandleShowingOverlay;
        AdMob_androidPlugin.DismissedOverlay -= HandleDismissedOverlay;
        AdMob_androidPlugin.LeavingApplication -= HandleLeavingApplication;
    }

    public void HandleReceivedAd()
    {
        print("HandleReceivedAd event received");
    }

    public void HandleFailedToReceiveAd(string message)
    {
        print("HandleFailedToReceiveAd event received with message:");
        print(message);
    }

    public void HandleShowingOverlay()
    {
        print("HandleShowingOverlay event received");
    }

    public void HandleDismissedOverlay()
    {
        print("HandleDismissedOverlay event received");
    }

    public void HandleLeavingApplication()
    {
        print("HandleLeavingApplication event received");
    }
	
#endif
}