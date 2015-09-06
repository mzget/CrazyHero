using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using Holoville.HOTween.Plugins;


public struct BattleNames {
	public const string ChallengeMode = "Challenge Mode";
	public const string EventMode = "EventMode";
	public const string TheForest = "TheForest";
	public const string TheDarkCave = "TheDarkCave";
    public const string TheDesert = "TheDesert";
	
	public static string SelectedBattleMode = string.Empty;
};

public class WorldMap : MzBaseScene {
	[SerializeField] internal GameObject coverDarkPlane;
	[SerializeField] public GameObject stageSelected;
	[SerializeField] private MzGUIButtonBeh forest_btn;
	[SerializeField] private MzGUIButtonBeh darkCave_btn;

    public string[] stageArr = new string[] {
        BattleNames.TheForest,
        BattleNames.TheDarkCave,
        BattleNames.TheDesert,
    };

	#region <@-- GUI section.

	[SerializeField] private MzGUIButtonBeh socialBtn;

	#endregion


    protected override void Initialization()
    {
        base.Initialization();

        SaveManager.InitSingleton();
    }

	// Use this for initialization
	void Start () {
		forest_btn.clickEvent += (sender, e) => {
			PrepareForJoinBattle(BattleNames.SelectedBattleMode = BattleNames.TheForest);
		};
        forest_btn.mouseDownEvent += () => { 
			this.ShowStageSelected(ScenesInstance.TheForest.ToString()); 
		};
        forest_btn.mouseUpEvent += () => { 
			this.HideStageSelected();
		};

		darkCave_btn.clickEvent += (sender, e) => { 
			this.PrepareForJoinBattle(BattleNames.SelectedBattleMode = BattleNames.TheDarkCave);
		}; 

		background_clip = null;
		base.PlayBackgroundMusic(background_clip, 0.5f);

		//<@-- GUI section.
		socialBtn.clickEvent += Handle_socialBtnClickEvent;
	}

	void ShowStageSelected(string _stage) {
		HOTween.To(stageSelected.transform, 0.5f, new TweenParms().Prop("localPosition", new PlugVector3Y(4.4f)).Ease(EaseType.EaseOutBounce));
	}

	void HideStageSelected() {
        HOTween.To(stageSelected.transform, 0.5f, new TweenParms().Prop("localPosition", new PlugVector3Y(8f)).Ease(EaseType.EaseOutBounce));
	}
	
	#region <!-- Prepare for join battle.
	
	[SerializeField] private GameObject confirmBattleWindows;
	[SerializeField] private MzGUIButtonBeh enterBattleButton;
	[SerializeField] private MzGUIButtonBeh cancelButton;
	[SerializeField] private tk2dTextMesh body_textmesh;
	private string selectedBattle = string.Empty;
	internal void PrepareForJoinBattle (string _name)
	{
		selectedBattle = _name;
		confirmBattleWindows.SetActive(true);
		coverDarkPlane.SetActive(true);
		Vector3 _oldCoverPos = coverDarkPlane.transform.localPosition;
		coverDarkPlane.transform.localPosition = new Vector3(_oldCoverPos.x, _oldCoverPos.y, confirmBattleWindows.transform.localPosition.z + 1);
		
		if(enterBattleButton == null) {
			Transform _enterT = confirmBattleWindows.transform.Find("EnterButton");
			enterBattleButton = _enterT.GetComponent<MzGUIButtonBeh>();
		}
		if(cancelButton == null) {
			Transform _cancelT = confirmBattleWindows.transform.Find("CancelButton");
			cancelButton = _cancelT.GetComponent<MzGUIButtonBeh>();
		}
		if (body_textmesh == null) {
			Transform _bodyT = confirmBattleWindows.transform.Find("BodyTextmesh");
			body_textmesh = _bodyT.GetComponent<tk2dTextMesh>();
		}
		
		enterBattleButton.clickEvent += Handle_EnterBattleButtonclick_event;
		cancelButton.clickEvent += Handle_CancelButtonclick_event;
		body_textmesh.text = selectedBattle;
		body_textmesh.Commit();
	}
	
	void Handle_CancelButtonclick_event (object sender, System.EventArgs e)
	{		
		enterBattleButton.clickEvent -= Handle_EnterBattleButtonclick_event;
		cancelButton.clickEvent -= Handle_CancelButtonclick_event;

		confirmBattleWindows.SetActive(false);
		//<@-- Set cover position back.
//		coverDarkPlane.transform.localPosition = darkCoverPlaneOriPos;
//		if(GameEventManager.GetInstance.eventsWindow_anchor.activeSelf == false)
			coverDarkPlane.SetActive(false);
	}
	
	void Handle_EnterBattleButtonclick_event (object sender, System.EventArgs e)
	{
		enterBattleButton.clickEvent -= Handle_EnterBattleButtonclick_event;
		cancelButton.clickEvent -= Handle_CancelButtonclick_event;
		
//		if(BattleNames.SelectedBattleMode == BattleNames.EventMode) {
//			GameEventManager.GetInstance.UpdateLatePlayEventDate(selectedBattle);
//		}
		
		StartCoroutine_Auto(IE_gotoOtherStage(BattleNames.SelectedBattleMode));
		confirmBattleWindows.SetActive(false);	
		coverDarkPlane.SetActive(true);
	}
	
	#endregion

	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}
	
	
	void Handle_socialBtnClickEvent (object sender, System.EventArgs e)
	{
//		SocialGameIntegration.GetInstance.OpenWindow();
	}
}
