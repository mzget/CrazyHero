using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Startup : MzBaseScene {
	
	public SpriteRenderer mzsoft_logo;
	
	// Use this for initialization
	void Start () {
		this.AutomaticSetup_QualitySetting();
//        MzFacebookIntegration.InitSingleton();
		
		HOTween.To(mzsoft_logo, 5f, new TweenParms().Prop("color", new Color(1f, 1f,1f,0f)).Ease(EaseType.EaseInOutSine).OnComplete(delegate() {
			if(Application.isLoadingLevel == false) {
				Application.LoadLevel(ScenesInstance.MainMenu.ToString());
			}
		}));
	}
	
	protected override void Initialization ()
	{
		base.Initialization ();
		
		SaveManager.Language_id = PlayerPrefs.GetInt(SaveManager.KEY_SYSTEM_LANGUAGE, 0);
		Main.Mz_AppLanguage.appLanguage = (Main.Mz_AppLanguage.SupportLanguage)SaveManager.Language_id;
	}
	
	private void AutomaticSetup_QualitySetting() {
#if UNITY_IPHONE
		if(iPhone.generation == iPhoneGeneration.iPad1Gen || 
			iPhone.generation == iPhoneGeneration.iPhone3G || iPhone.generation == iPhoneGeneration.iPhone3GS ||
			iPhone.generation == iPhoneGeneration.iPhone4 || iPhone.generation == iPhoneGeneration.iPodTouch1Gen ||
			iPhone.generation == iPhoneGeneration.iPodTouch2Gen || iPhone.generation == iPhoneGeneration.iPodTouch3Gen ||
			iPhone.generation == iPhoneGeneration.iPodTouch4Gen) {
			QualitySettings.SetQualityLevel(0);	
			Application.targetFrameRate = 30;
		}
		else {
			QualitySettings.SetQualityLevel(1);
			Application.targetFrameRate = 60;
		}
#elif UNITY_ANDROID
		if(Screen.height <= 480) {			
			QualitySettings.SetQualityLevel(0);	
			Application.targetFrameRate = 60;
		}
		else if(Screen.height > 480 && Screen.height <= 720) {
			QualitySettings.SetQualityLevel(1);
			Application.targetFrameRate = 60;
		}
		else if(Screen.height > 720) {
			QualitySettings.SetQualityLevel(3);
			Application.targetFrameRate = 60;
		}
#else 
		QualitySettings.SetQualityLevel(3);
		Application.targetFrameRate = 60;
#endif

        Debug.Log("Setting quality... : " + QualitySettings.names[QualitySettings.GetQualityLevel()]);
	}
}