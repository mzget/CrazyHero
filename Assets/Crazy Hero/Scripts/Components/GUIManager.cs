using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;
using MiniJSON;


#if UNITY_ANDROID
using AdmobPluginAccess = AdMob_androidPlugin;
#elif UNITY_IOS
using AdmobPluginAccess = AdMob_iOSPlugin;
#endif

public class GUIManager : MonoBehaviour {

	private static GUIManager instance;
	public static GUIManager GetInstance {
		get{ 
			if(instance == null) {
				instance = GameObject.FindObjectOfType<GUIManager>();
				if(instance == null) {
					Debug.LogError("GUIManager instance is null.");
				}
			}
			return instance;
		}
	}

	private BattleStage stage;
    public enum GUI_state { normal = 0, upgradeStatus, inventoryMenu, endGameUI, };
    public GUI_state currentState;
	
	public Camera guiCamera;
	public GameObject plane_fadeGUI;
	
    //public UpgradeStatGUI upgradeStatGUI;
    //public GameControl inventoryGUI;
	public GameObject inventoryGUI_anchor;	
	public GameObject treasureWindowUIObj;

    [SerializeField]
    private Transform midCenter_anchor;
	internal Vector3 midCenter_screenPos;
	public Transform topCenterGUI_anchor;
    internal Vector3 topCenter_screenPos;
	public Transform coinIconTransform;
    internal Vector3 coinIcon_screenPos;
	
	private tk2dTextMesh stageLevel_textmesh;
    private tk2dTextMesh exp_textmesh;
    private tk2dTextMesh goldTextmesh;
    [SerializeField]
    private tk2dTextMesh timeSpent_textMesh;
	
	public Transform[] supportCard_transform = new Transform[7];
	public tk2dTextMesh[] costOfUnits;
	public tk2dTextMesh[] levelOfUnits;
	public Dictionary<string, int> dict_unitNamePairID = new Dictionary<string, int> ();

	public GameObject levelUp_spriteEffect;
    [SerializeField] private tk2dTextMesh currentScore_textmesh;
    private tk2dTextMesh highScore_textmesh;
    private tk2dTextMesh endGameStage;
	public GameObject levelCompleteWindow;
    [SerializeField]
    private GameObject rewardWindow;
	private GameObject treasureChestPrefab;
    [SerializeField]
    private MzGUIButtonBeh keepRewardButton;

	public MzGUIButtonBeh menu_button;
	private MzGUIButtonBeh shareScore_button;
	[SerializeField]	private MzGUIButtonBeh pause_button;
	[SerializeField]	private MzGUIButtonBeh resume_button;
	[SerializeField] 	private MzGUIButtonBeh quitGame_button;
	[SerializeField]	private GameObject pauseGameWindow;
    [SerializeField]
    private GameObject quitgameWindow;


//	private bool _IsSharedFacebook = false;
    [SerializeField]
    private bool _internetReachability = true;
	private bool _isDisplayError = false;

	void OnDestroy() {
		instance = null;
	}
	
	// Use this for initialization
	void Start () {
		stage = (BattleStage)MzBaseScene.GetInstance;

		this.AdjustGUITransform();
		StartCoroutine_Auto(Initializing());

		this.pause_button.clickEvent += Handle_PauseBtnClickEvent;
		this.resume_button.clickEvent += Handle_Resume_buttonclick_event;
		this.quitGame_button.clickEvent += Handle_QuitBtn_clickEvent;
	}

	void AdjustGUITransform ()
	{
		if(Main.IsApproximately(Main.GetCurrentRatio, Main.Ratio_16per9)) {
			print ("current ratio : => Ratio_9per16");
            return;
		}
        else if (Main.IsApproximately(Main.GetCurrentRatio, Main.Ratio_10per16)) {
			print ("current ratio : => Ratio_10per16");
            return;
        }
        else if(Main.IsApproximately(Main.GetCurrentRatio, Main.Ratio_15per9)) {			
			print ("current ratio : => Ratio_15per9");
            return;
		}
        else if(Main.IsApproximately(Main.GetCurrentRatio, Main.Ratio_3per2)) {
			print ("current ratio : => Ratio_2per3");
            return;
        }
	}
	
	internal IEnumerator Initializing() {
		levelUp_spriteEffect.SetActive(false);
        StartCoroutine_Auto(this.InitGUICamera());
        StartCoroutine_Auto(this.InitGoldTextmesh());
//		StartCoroutine_Auto(this.Init_SoulTextmesh());
//		StartCoroutine_Auto(this.Init_ExpTextmesh());
        StartCoroutine_Auto(this.InitStageLevelTextmesh());
		StartCoroutine_Auto(this.InitStatusWindow());

//        levelCompleteWindow.SetActive(false);

//		menu_button.clickEvent += Handle_PauseButton_clickEvent;
//		resume_button.clickEvent += Handle_Resume_buttonclick_event;

//		midCenter_screenPos = guiCamera.WorldToScreenPoint(midCenter_anchor.position);
//        topCenter_screenPos = guiCamera.WorldToScreenPoint(topCenterGUI_anchor.position);
        coinIcon_screenPos = guiCamera.WorldToScreenPoint(coinIconTransform.position);
		
		yield return null;
        
		treasureChestPrefab = Resources.Load("Stages/TreasureChest", typeof(GameObject)) as GameObject;
	}

	IEnumerator InitGUICamera ()
	{
		GameObject uiCam = GameObject.Find("UICamera");
		if(uiCam != null)
			guiCamera = uiCam.GetComponent<Camera>();
		else
			Debug.LogWarning("No GUICamera object.");

        yield return null;
	}

	IEnumerator InitStageLevelTextmesh ()
	{
        GameObject obj = GameObject.Find("Stage_TextMesh");
        stageLevel_textmesh = obj.GetComponent<tk2dTextMesh>();

		yield return 0;
	}

    #region <@-- Gold Lable.
    
    private IEnumerator InitGoldTextmesh()
    {
        var goldText_obj = GameObject.Find("Golds_TextMesh");
        goldTextmesh = goldText_obj.GetComponent<tk2dTextMesh>();

        yield return null;

        //<!-- Display gold score.
        goldTextmesh.text = SaveManager.GetInstance.gold.ToString();
    }
	
	public void AddGold(int point) {
		SaveManager.GetInstance.gold += point;
        goldTextmesh.text = SaveManager.GetInstance.gold.ToString();
        goldTextmesh.Commit();
	}
	public void RemoveGold(int point) {
        SaveManager.GetInstance.gold -= point;
        goldTextmesh.text = SaveManager.GetInstance.gold.ToString();
        goldTextmesh.Commit();

		stage.audioEffect.PlayOnecWithOutStop(stage.coin_clip);
	}

    #endregion

    #region <@-- Handle button.

    void Handle_Resume_buttonclick_event (object sender, System.EventArgs e)
	{
        this.SetActivePauseGameWindow();
	}

	void Handle_PauseBtnClickEvent (object sender, System.EventArgs e)
	{
		//<@-- Ask user play leave gameplay.
        this.SetActivePauseGameWindow();
	}
	
	void Handle_QuitBtn_clickEvent (object sender, EventArgs e)
	{
		this.SetActiveQuitGameMenu();
	}
	
	#endregion
	
	// Update is called once per frame
	void Update () {
		if(stage.curGameplayState == BattleStage.GameplayState.gameEnd) 
			return;

        this.UpdateExpLabel();
        this.UpdateTimingLabel();
    }
 
	internal void SetActivePauseGameWindow() {
		if (!pauseGameWindow.activeSelf) {
			pauseGameWindow.SetActive (true);
			HOTween.To (pauseGameWindow.transform, 0.5f, new TweenParms ()
			.Prop ("localPosition", new PlugVector3Y (0f))
			.Ease (EaseType.EaseOutBounce)
			.OnComplete (delegate() {
				plane_fadeGUI.SetActive (true);
				stage.SetGamepauseState(true);
			}));
		}
		else {			
			stage.SetGamepauseState(false);
            HOTween.To(pauseGameWindow.transform, 0.5f, new TweenParms()
                .Prop("localPosition", new PlugVector3Y(8f))
                .Ease(EaseType.EaseOutBack)
                .OnComplete(delegate() {
                plane_fadeGUI.SetActive(false);
                pauseGameWindow.SetActive(false);
            }));
		}
	}

	void SetActiveQuitGameMenu ()
	{
		if(pauseGameWindow.activeSelf) {
            pauseGameWindow.SetActive(false);
            quitgameWindow.SetActive(true);

            Transform yesBtnT = quitgameWindow.transform.Find("YesBtn");
            MzGUIButtonBeh yesBtn = yesBtnT.GetComponent<MzGUIButtonBeh>();
            yesBtn.clickEvent += (sender, e) => { 
                StartCoroutine_Auto(MzBaseScene.GetInstance.IE_gotoOtherStage(MzBaseScene.ScenesInstance.MainMenu));
            };

            Transform noBtnT = quitgameWindow.transform.Find("NoBtn");
            var noBtn = noBtnT.GetComponent<MzGUIButtonBeh>();
            noBtn.clickEvent += (sender, e) => {
                quitgameWindow.SetActive(false);
                pauseGameWindow.SetActive(true);
            };
		}	
	}

	private void ActiveGameOverWindow ()
	{
		currentState = GUI_state.endGameUI;
	}
	
	#region <!-- Shared score to facebook.
       
	/// <summary>
    /// Facebook sharing feature.
    /// </summary>
	private void ShareFacebookFeed ()
	{
		/*

        if (FB.IsLoggedIn) { 
            string FeedToId = "";
            string FeedLink = "https://www.facebook.com/appcenter/heroesdefender";
            string FeedLinkName = "Taokaenoi Bakery Shop";
            string FeedLinkCaption = "Taokaenoi is the best selling educational game series.";
            string FeedLinkDescription = "Taokaenoi Bakery, is a game that lets kids be a bakery owner. Kids will have fun decorating cupcakes, serving ice cream, and toasting breads. The Sheep Bank will support their saving habits. The bakery shop can be upgraded to add more products. Have a fun time with our game and many other Taokaenoi games.";
            string FeedPicture = "http://www.vistafungame.com/cms/images/apps/tk-bake01.png";

            MzFacebookIntegration.GetInstance.FeedRequestComplete_delegate += FeedRequestCallback;
            Facebook.FacebookDelegate Callback = (FBResult result) => {       
		        string lastResponse = "";
                if (result.Error != null)
                    lastResponse = "Error Response:\n" + result.Error;
                else { 
                    lastResponse = "Success Response:\n" + result.Text;
                }
                Debug.Log("Feed Callback: " + lastResponse);
            };
            FB.Feed(
                toId: FeedToId,
                link: FeedLink,
                linkName: FeedLinkName,
                linkCaption: FeedLinkCaption,
                linkDescription: FeedLinkDescription,
                picture: FeedPicture,
                mediaSource: "",
                actionName: "",
                actionLink: "",
                reference: "",
                properties: null,
                callback: Callback
            );
        }
        else { 		
            string lastResponse = "";
            Facebook.FacebookDelegate _callback = delegate(FBResult result) { 
                if (result.Error != null)
                    lastResponse = "Error Response:\n" + result.Error;
                else { 
                    lastResponse = "Success Response:\n" + result.Text;
                    this.ShareFacebookFeed();
					MzFacebookIntegration.GetInstance.GetMyData();
                }
                Debug.Log("Login Callback: " + lastResponse);
            };
            MzFacebookIntegration.GetInstance.CallFBLogin(_callback);
        }
		*/
	}

    private void FeedRequestCallback(string result)
    {
//        MzFacebookIntegration.GetInstance.FeedRequestComplete_delegate -= this.FeedRequestCallback;
//
//        Debug.Log("OnFeedRequestComplete => " + result);      
//   
//        var dict = Json.Deserialize(result) as Dictionary<string, object>;
//        if (dict.ContainsKey("cancelled"))
//            return;
//        else
//            Debug.Log("ShareFacebookComplete");
    }

	#endregion

    #region <!-- Stage Clear.

    internal void ActiveLevelCompleteWindow()
    {
		stage.SavingLevelComplete();

        plane_fadeGUI.SetActive(true);
        levelCompleteWindow.SetActive(true);
        HOTween.To(levelCompleteWindow.transform, 1f, new TweenParms().Prop("localPosition", new Vector3(0, 0, plane_fadeGUI.transform.localPosition.z - 1f))
            .Ease(EaseType.EaseOutSine)
            .OnComplete(delegate() {
                levelCompleteWindow.GetComponent<Animator>().Play("Tag Animation");
				Invoke("CreateTreasureChest", 2f);
            }));
	}
	
	private void CreateTreasureChest ()
	{
		currentScore_textmesh.text = "Score: " + stage.Score;
		HOTween.To(levelCompleteWindow.transform, .5f, new TweenParms().Prop("localPosition", new Vector3(0, 3.8f, plane_fadeGUI.transform.localPosition.z -1f))
			.Ease(EaseType.Linear)
		    .OnComplete(delegate() {
				rewardWindow.SetActive(true);		
				HOTween.To(rewardWindow.transform, 0.5f, new TweenParms().Prop("localPosition", new PlugVector3Y(0)));
                keepRewardButton.gameObject.SetActive(true);
				keepRewardButton.clickEvent += (sender, e) => {
					StartCoroutine_Auto(stage.IE_gotoOtherStage(MzBaseScene.ScenesInstance.MainMenu));
				};
			}));

//		GameObject box = Instantiate(treasureChestPrefab) as GameObject;
//		box.transform.position = this.transform.position + new Vector3(0, 4f, -0.2f);
//		HOTween.To(box.transform, 0.5f, new TweenParms().Prop("position", this.transform.position).Ease(EaseType.EaseInExpo));
	}

    #endregion

    private void OnGUI() {
		///Scale the button sizes for retina displays
		float screenScale = (float)(Screen.width / 640f);
		Matrix4x4 scaledMatrix = Matrix4x4.Scale(new Vector3(screenScale, screenScale, screenScale));
		GUI.matrix = scaledMatrix;

		if(_internetReachability == false && _isDisplayError) {
			GUIStyle style = new GUIStyle(GUI.skin.label);
			style.fontSize = 32;
			style.fontStyle = FontStyle.Normal;
			style.alignment = TextAnchor.MiddleCenter;

			GUIStyle headerStyle = new GUIStyle(GUI.skin.label);
			headerStyle.fontSize = 36;
			headerStyle.fontStyle = FontStyle.Bold;
			headerStyle.alignment = TextAnchor.MiddleCenter;

			GUIStyle OK_buttonStyle = new GUIStyle(GUI.skin.button);
			OK_buttonStyle.fontSize = 36;
			OK_buttonStyle.fontStyle = FontStyle.Bold;
			OK_buttonStyle.alignment = TextAnchor.MiddleCenter;

			GUI.BeginGroup(new Rect(100, (960 / 2) - 100, 440, 200), GUIContent.none, GUI.skin.box);
			{
				GUI.Box(new Rect(0, 0, 440, 200), GUIContent.none, GUI.skin.button);

				GUI.Label(new Rect(0, 10, 440, 50), "Error", headerStyle);
				GUI.Label(new Rect(0, 70, 440, 50), "Internet Connection Error.", style);
				if(GUI.Button(new Rect(10, 130, 420, 65), "OK", OK_buttonStyle)) {
					_isDisplayError = false;
				}
			}
			GUI.EndGroup();
		}
	}

	#region <@-- Shop GUI Manager.

    public GameObject exp_lable;
    private tk2dSprite exp_scrolling;
    private tk2dTextMesh level_textmesh;
	public GameObject tavernWelcomeTag;
    public GameObject statusWindow;
	public GameObject tavernShopWindow;
    private TextMesh lv_status_text;
	private TextMesh atk_status_text;
	private TextMesh def_status_text;
	private TextMesh spd_status_text;
	private TextMesh cri_status_text;
	public void ShowTavernTag() {
		print("ShowTavernTag");
		HOTween.To(tavernWelcomeTag.transform, 0.5f, new TweenParms().Prop("localPosition", new Vector3(0, 4.4f, 0)).Ease(EaseType.Linear));
		HOTween.To(tavernShopWindow.transform, 0.5f, new TweenParms().Prop("localPosition", new Vector3(0, 0, 0)));
        HOTween.To(statusWindow.transform, 0.5f, new TweenParms().Prop("localPosition", new Vector3(-6.15f, -0.6f, 0)));
        HOTween.To(exp_lable.transform, 0.5f, new TweenParms().Prop("localPosition", new Vector3(5.35f, 4.5f, 0)));

		this.RefreshStatusWindow();
	}
	public void HideTavernTag() {
		print("HideTavernTag");
		HOTween.To(tavernWelcomeTag.transform, 0.5f, new TweenParms().Prop("localPosition", new Vector3(0, 6f, 0)));
		HOTween.To(tavernShopWindow.transform, 0.5f, new TweenParms().Prop("localPosition", new Vector3(12.5f, 0,0)));
        HOTween.To(statusWindow.transform, 0.5f, new TweenParms().Prop("localPosition", new Vector3(-12.5f, -0.6f, 0)));
        HOTween.To(exp_lable.transform, 0.5f, new TweenParms().Prop("localPosition", new Vector3(0, 4.5f, 0)));
	}

	IEnumerator InitStatusWindow ()
	{
        var lv_text_transform = statusWindow.transform.Find("Level");
        lv_status_text = lv_text_transform.GetComponent<TextMesh>();

        yield return null;

		var atk_text_transform = statusWindow.transform.Find("Attack");
		atk_status_text = atk_text_transform.GetComponent<TextMesh>();

		yield return null;

		var def_text_transform = statusWindow.transform.Find("Defense");
		def_status_text = def_text_transform.GetComponent<TextMesh>();

		yield return null;

		var spd_text_transform = statusWindow.transform.Find("Speed");
		spd_status_text = spd_text_transform.GetComponent<TextMesh>();

		yield return null;

        var cri_text_transform = statusWindow.transform.Find("Critical");
        cri_status_text = cri_text_transform.GetComponent<TextMesh>();

		yield return null;

		this.RefreshStatusWindow();
	}

	internal void RefreshStatusWindow() {		
        lv_status_text.text = "Lv: " + HeroBeh.GetInstance.stat.level;
        atk_status_text.text = string.Format("Atk: " + HeroBeh.GetInstance.stat.attack + "<color=lime> +{0}</color>", HeroBeh.GetInstance.stat.addAtk);
        def_status_text.text = string.Format("Def: " + HeroBeh.GetInstance.stat.defense + "<color=lime> +{0}</color>", HeroBeh.GetInstance.stat.addDef);
        spd_status_text.text = string.Format("Spd: " + HeroBeh.GetInstance.stat.walkSpeed + "<color=lime> +{0}</color>", HeroBeh.GetInstance.stat.addSpd);
        cri_status_text.text = string.Format("Cri: " + HeroBeh.GetInstance.stat.critical + "<color=lime> +{0}</color>", HeroBeh.GetInstance.stat.addCri);
	}

	#endregion

    private void UpdateExpLabel() {
        if (exp_scrolling == null) { 
            var scrolling = exp_lable.transform.Find("exp_scrolling");
            exp_scrolling = scrolling.GetComponent<tk2dSprite>();
        }
        if (level_textmesh == null) {
            var text = exp_lable.transform.Find("Level_TextMesh");
            level_textmesh = text.GetComponent<tk2dTextMesh>();
        }
        
        float result = HeroBeh.GetInstance.curExp / HeroBeh.GetInstance.nextExp;
        exp_scrolling.scale = new Vector3(result, 1, 1);
        level_textmesh.text = "Lv: " + HeroBeh.GetInstance.stat.level;
    }

    private void UpdateTimingLabel() {
        TimeSpan span = new TimeSpan(stage.GetCurrentTimeSpent().Hours, stage.GetCurrentTimeSpent().Minutes, stage.GetCurrentTimeSpent().Seconds);
        string format = string.Format("{0:hh\\:mm\\:ss}", span);
        timeSpent_textMesh.text = format;
    }

	public IEnumerator ShowLevelUpGameEffect ()
	{
		levelUp_spriteEffect.SetActive(true);

        yield return new WaitForSeconds(3f);

        levelUp_spriteEffect.SetActive(false);
	}
}
