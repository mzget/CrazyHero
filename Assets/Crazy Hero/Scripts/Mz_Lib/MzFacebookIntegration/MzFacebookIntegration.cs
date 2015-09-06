using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;



public class MzFacebookIntegration : MonoBehaviour {
#if UNITY_WEBPLAYER

    private static MzFacebookIntegration Instance;
    public static MzFacebookIntegration GetInstance {
        get {
            Instance = GameObject.FindObjectOfType<MzFacebookIntegration>();
            if (Instance == null) {
                Debug.LogWarning("MzFacebook instance is null. Please create once.");
            }    
            return Instance;
        }
    }

	internal static void InitSingleton() {
		Instance = GameObject.FindObjectOfType<MzFacebookIntegration>();
		Debug.Log("Trace MzFacebook instance: " + Instance);
        if (Instance == null) {
			Debug.Log("Create MzFacebook instance.");
            GameObject mzFacebook = new GameObject("MzFacebook", typeof(MzFacebookIntegration));
			Instance = mzFacebook.GetComponent<MzFacebookIntegration>();
        }
        else
            Debug.LogWarning("MzFacebookIntegration is singleton object");
    }

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
        if (isInit == false) {
            CallFBInit();
            status = "FB.Init() called with " + FB.AppId;
        }
	}

    #region FB.Init() example

    internal bool isInit = false;

    private void CallFBInit()
    {
        FB.Init(OnInitComplete, OnHideUnity);
    }

    private void OnInitComplete()
    {
        Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
        isInit = true;
		
		this.ManageProfilePicture();
		if(FB.IsLoggedIn) {
			CreateOG_PlayingStory();
		}
    }

    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game showing? " + isGameShown);
    }

    #endregion

    #region FB.Login() example

    internal void CallFBLogin()
    {
        FB.Login("email,publish_actions", LoginCallback);
    }
    internal void CallFBLogin(Facebook.FacebookDelegate _callback)
    {
        FB.Login("email,publish_actions", _callback);
    }

    void LoginCallback(FBResult result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            lastResponse = "Error Response:\n" + result.Error;
        }
        else if (!FB.IsLoggedIn)
        {
            lastResponse = "Login cancelled by Player";
        }
        else
        {
            lastResponse = "Login was successful!";
            MzFacebookIntegration.GetInstance.GetMyData();
        }

        Debug.Log("LoginCallback: " + lastResponse);
    }

    internal void CallFBLogout()
    {
        this.profileTexture = null;
        FB.Logout();
    }

    #endregion

    #region FB.PublishInstall() example

    private void CallFBPublishInstall()
    {
        FB.PublishInstall(PublishComplete);
    }

    private void PublishComplete(FBResult result)
    {
        Debug.Log("publish response: " + result.Text);
    }

    #endregion

    #region FB.Feed() example

    public string FeedToId = "";
    public string FeedLink = "";
    public string FeedLinkName = "";
    public string FeedLinkCaption = "";
    public string FeedLinkDescription = "";
    public string FeedPicture = "";
    public string FeedMediaSource = "";
    public string FeedActionName = "";
    public string FeedActionLink = "";
    public string FeedReference = "";
    public bool IncludeFeedProperties = false;
    private Dictionary<string, string[]> FeedProperties = new Dictionary<string, string[]>();

    internal void CallFBFeed()
    {
        Dictionary<string, string[]> feedProperties = null;
        if (IncludeFeedProperties)
        {
            feedProperties = FeedProperties;
        }
        FB.Feed(
            toId: FeedToId,
            link: FeedLink,
            linkName: FeedLinkName,
            linkCaption: FeedLinkCaption,
            linkDescription: FeedLinkDescription,
            picture: FeedPicture,
            mediaSource: FeedMediaSource,
            actionName: FeedActionName,
            actionLink: FeedActionLink,
            reference: FeedReference,
            properties: feedProperties,
            callback: Callback
        );
    }

    #endregion

    #region FB.Canvas.Pay() example

    public string PayProduct = "";

    private void CallFBPay()
    {
        FB.Canvas.Pay(PayProduct);
    }

    #endregion

    #region FB.API() example

    private string ApiQuery = "";

    internal void CallFBAPI()
    {
        FB.API(ApiQuery, Facebook.HttpMethod.GET, Callback);
    }   
	internal void CallFBAPI(string _query, Facebook.HttpMethod _httpMethod, Facebook.FacebookDelegate _callback, Dictionary<string, string> _form)
    {
        FB.API(_query, _httpMethod, _callback, _form);
    }

    #endregion

    #region FB.GetDeepLink() example

    private void CallFBGetDeepLink()
    {
        FB.GetDeepLink(Callback);
    }

    #endregion

    #region FB.AppEvent.LogEvent example

    public float PlayerLevel = 1.0f;

    public void CallAppEventLogEvent()
    {
        var parameters = new Dictionary<string, object>();
        parameters[Facebook.FBAppEventParameterName.Level] = "Player Level";
        FB.AppEvents.LogEvent(Facebook.FBAppEventName.AchievedLevel, PlayerLevel, parameters);
        PlayerLevel++;
    }

    #endregion

    #region GUI

    private string status = "Ready";

    private string lastResponse = "";
    public GUIStyle textStyle = new GUIStyle();
    private Texture2D lastResponseTexture;

    private Vector2 scrollPosition = Vector2.zero;
#if UNITY_IOS || UNITY_ANDROID
    int buttonHeight = 60;
    int mainWindowWidth = 610;
    int mainWindowFullWidth = 640;
#else
    int buttonHeight = 24;
    int mainWindowWidth = 500;
    int mainWindowFullWidth = 530;
#endif

    private int TextWindowHeight
    {
        get
        {
#if UNITY_IOS || UNITY_ANDROID
            return IsHorizontalLayout() ? Screen.height : 85;
#else
        return Screen.height;
#endif

        }
    }

/*
    void OnGUI()
    {
        if (IsHorizontalLayout())
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
        }
        GUILayout.Box("Status: " + status, GUILayout.MinWidth(mainWindowWidth));

#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            scrollPosition.y += Input.GetTouch(0).deltaPosition.y;
        }
#endif

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(mainWindowFullWidth));
        GUILayout.BeginVertical();
        GUI.enabled = !isInit;
        if (Button("FB.Init"))
        {
            CallFBInit();
            status = "FB.Init() called with " + FB.AppId;
        }

        GUI.enabled = isInit;
        if (Button("Login"))
        {
            CallFBLogin();
            status = "Login called";
        }

#if UNITY_IOS || UNITY_ANDROID
        if (Button("Publish Install"))
        {
            CallFBPublishInstall();
            status = "Install Published";
        }
#endif

        GUI.enabled = FB.IsLoggedIn;
        GUILayout.Space(10);
        LabelAndTextField("Title (optional): ", ref FriendSelectorTitle);
        LabelAndTextField("Message: ", ref FriendSelectorMessage);
        LabelAndTextField("Exclude Ids (optional): ", ref FriendSelectorExcludeIds);
        LabelAndTextField("Filters (optional): ", ref FriendSelectorFilters);
        LabelAndTextField("Max Recipients (optional): ", ref FriendSelectorMax);
        LabelAndTextField("Data (optional): ", ref FriendSelectorData);
        if (Button("Open Friend Selector"))
        {
            try
            {
                CallAppRequestAsFriendSelector();
                status = "Friend Selector called";
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }
        GUILayout.Space(10);
        LabelAndTextField("Title (optional): ", ref DirectRequestTitle);
        LabelAndTextField("Message: ", ref DirectRequestMessage);
        LabelAndTextField("To Comma Ids: ", ref DirectRequestTo);
        if (Button("Open Direct Request"))
        {
            try
            {
                CallAppRequestAsDirectRequest();
                status = "Direct Request called";
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }
        GUILayout.Space(10);
        LabelAndTextField("To Id (optional): ", ref FeedToId);
        LabelAndTextField("Link (optional): ", ref FeedLink);
        LabelAndTextField("Link Name (optional): ", ref FeedLinkName);
        LabelAndTextField("Link Desc (optional): ", ref FeedLinkDescription);
        LabelAndTextField("Link Caption (optional): ", ref FeedLinkCaption);
        LabelAndTextField("Picture (optional): ", ref FeedPicture);
        LabelAndTextField("Media Source (optional): ", ref FeedMediaSource);
        LabelAndTextField("Action Name (optional): ", ref FeedActionName);
        LabelAndTextField("Action Link (optional): ", ref FeedActionLink);
        LabelAndTextField("Reference (optional): ", ref FeedReference);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Properties (optional)", GUILayout.Width(150));
        IncludeFeedProperties = GUILayout.Toggle(IncludeFeedProperties, "Include");
        GUILayout.EndHorizontal();
        if (Button("Open Feed Dialog"))
        {
            try
            {
                CallFBFeed();
                status = "Feed dialog called";
            }
            catch (Exception e)
            {
                status = e.Message;
            }
        }
        GUILayout.Space(10);

#if UNITY_WEBPLAYER
        LabelAndTextField("Product: ", ref PayProduct);
        if (Button("Call Pay"))
        {
            CallFBPay();
        }
        GUILayout.Space(10);
#endif

        LabelAndTextField("API: ", ref ApiQuery);
        if (Button("Call API"))
        {
            status = "API called";
            CallFBAPI();
        }
        GUILayout.Space(10);
        if (Button("Take & upload screenshot"))
        {
            status = "Take screenshot";

            StartCoroutine(TakeScreenshot());
        }

        if (Button("Get Deep Link"))
        {
            CallFBGetDeepLink();
        }
#if UNITY_IOS || UNITY_ANDROID
        if (Button("Log FB App Event"))
        {
            status = "Logged FB.AppEvent";
            CallAppEventLogEvent();
        }
#endif

        GUILayout.Space(10);

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        if (IsHorizontalLayout())
        {
            GUILayout.EndVertical();
        }
        GUI.enabled = true;

        var textAreaSize = GUILayoutUtility.GetRect(640, TextWindowHeight);

        GUI.TextArea(
            textAreaSize,
            string.Format(
                " AppId: {0} \n Facebook Dll: {1} \n UserId: {2}\n IsLoggedIn: {3}\n AccessToken: {4}\n\n {5}",
                FB.AppId,
                (isInit) ? "Loaded Successfully" : "Not Loaded",
                FB.UserId,
                FB.IsLoggedIn,
                FB.AccessToken,
                lastResponse
            ), textStyle);

        if (lastResponseTexture != null)
        {
            GUI.Label(new Rect(textAreaSize.x + 5, textAreaSize.y + 200, lastResponseTexture.width, lastResponseTexture.height), lastResponseTexture);
        }

        if (IsHorizontalLayout())
        {
            GUILayout.EndHorizontal();
        }
    }
*/
 

    private bool Button(string label)
    {
        return GUILayout.Button(label, GUILayout.MinHeight(buttonHeight));
    }

    private void LabelAndTextField(string label, ref string text)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(150));
        text = GUILayout.TextField(text, GUILayout.MinWidth(300));
        GUILayout.EndHorizontal();
    }

    private bool IsHorizontalLayout()
    {
#if UNITY_IOS || UNITY_ANDROID
        return Screen.orientation == ScreenOrientation.Landscape;
#else
        return true;
#endif
    }

    #endregion
	
	#region Helper.
	
	void Callback(FBResult result)
    {
        lastResponseTexture = null;
        // Some platforms return the empty string instead of null.
        if (!String.IsNullOrEmpty(result.Error))
            lastResponse = "Error Response:\n" + result.Error;
        else if (!ApiQuery.Contains("/picture"))
            lastResponse = "Success Response:\n" + result.Text;
        else
        {
            lastResponseTexture = result.Texture;
            lastResponse = "Success Response:\n";
        }

        Debug.Log("Print Callback: " + lastResponse);
    }

    private IEnumerator TakeScreenshot() 
    {
        yield return new WaitForEndOfFrame();

        var width = Screen.width;
        var height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
        byte[] screenshot = tex.EncodeToPNG();

        var wwwForm = new WWWForm();
        wwwForm.AddBinaryData("image", screenshot, "InteractiveConsole.png");
        wwwForm.AddField("message", "herp derp.  I did a thing!  Did I do this right?");

        FB.API("me/photos", Facebook.HttpMethod.POST, Callback, wwwForm);
    }
	
	#endregion

    public Action LogoutComplete_delegate;
    public void OnLogoutComplete(string message) {
        Debug.Log("OnLogoutComplete => :" + message);
        if (LogoutComplete_delegate != null)
            LogoutComplete_delegate();
    }

    public Action<string> FeedRequestComplete_delegate;   
    public void OnFeedRequestComplete(string message) {
        Debug.Log("OnFeedRequestComplete => " + message);
        if(FeedRequestComplete_delegate != null)
            FeedRequestComplete_delegate(message);
    }
    
    #region <@-- Implement Open graph.

    public void OG_POSTStory(string _query, Dictionary<string, string> formData) { 
        //<!-- handle the response
        Facebook.FacebookDelegate _callback = delegate(FBResult result) {
            if (result.Error != null)
                lastResponse = "Error Response:\n" + result.Error;
            else {
                lastResponse = "Success Response:\n" + result.Text;
            }

            Debug.Log("Print Callback: " + lastResponse);
        };
        //<!-- Call Fb.api
        FB.API(_query, Facebook.HttpMethod.POST, _callback, formData);
    }

    #endregion

    internal void CreateOG_CompletedLevel()
    {        
		var query = "/me/heroesdefender:completed?";
        var formData = new Dictionary<string,string>();
		formData.Add("level", "http://heroesdefender.mz-soft.com/level_object_type.html");
		
        MzFacebookIntegration.GetInstance.OG_POSTStory(query, formData);
    }

    internal void CreateOG_DefeatedBoss() {     
        var formData = new Dictionary<string,string>();
		formData.Add("boss", "http://heroesdefender.mz-soft.com/boss_object_type.html");
		
		var query = "/me/heroesdefender:defeat?";
        MzFacebookIntegration.GetInstance.OG_POSTStory(query, formData);
    }

    internal void CreateOG_PlayingStory()
    {
        string query = "/me/heroesdefender:play?";
        var formData = new Dictionary<string, string>();
        formData.Add("heroes_defender", "http://heroesdefender.mz-soft.com/heroesdefender_object_type.html");
        MzFacebookIntegration.GetInstance.OG_POSTStory(query, formData);
    }
	
	internal void PostScore(int _score) {
		ApiQuery = "/me/scores";
		var formData = new Dictionary<string, string>() { {"score", _score.ToString()}};
		
		Facebook.FacebookDelegate _callback = delegate(FBResult result) {
			// Some platforms return the empty string instead of null.
			if (!String.IsNullOrEmpty(result.Error))
				lastResponse = "Error Response:\n" + result.Error;
			else
				lastResponse = "Success Response:\n" + result.Text;
			
			Debug.Log("PostScoreCallback: " + lastResponse);
		};
		this.CallFBAPI(ApiQuery, Facebook.HttpMethod.POST, _callback, formData);
	}

	public event Action GetMyDataComplete_Event;
	private void OnGetMyDataComplete() {
		if(GetMyDataComplete_Event != null) 
			GetMyDataComplete_Event();
	}
	internal void GetMyData ()
	{
		ApiQuery = "/me";
		var _dataDict = new Dictionary<string, object>();
		Facebook.FacebookDelegate _callback = (result) => {
	        if (result.Error != null)
	            lastResponse = "Error Response:\n" + result.Error;
	        else if (!ApiQuery.Contains("/picture")) {
	            lastResponse = "Success Response:\n" + result.Text;
				
				_dataDict = Json.Deserialize(result.Text) as Dictionary<string, object>;
				if(_dataDict.ContainsKey("first_name")) {
					if(_dataDict["first_name"].ToString() != string.Empty) {
                        SaveManager.GetInstance.username = _dataDict["first_name"].ToString();
					}
				}
			}
	
			Debug.Log("GetMyData Callback: " + lastResponse);
			this.OnGetMyDataComplete();
		};
		
		this.CallFBAPI(ApiQuery, Facebook.HttpMethod.GET, _callback, null);
	}

	public Texture2D profileTexture;
	public event Action GetProfileTextureCompleteEvent;
	protected void OnGetProfileTextureCompleteEvent ()
	{
		if (GetProfileTextureCompleteEvent != null)
			GetProfileTextureCompleteEvent ();
	}

    private IEnumerator GetMyProfilePicture() {
        //var ApiQuery = "/me/picture";	
        //Facebook.FacebookDelegate _callback = (result) => {
        //    if (result.Error != null) {
        //        lastResponse = "Error Response:\n" + result.Error;
        //    }
        //    else {
        //        lastResponse = "Success Response: GetPicture";
        //        profileTexture = result.Texture;
        //        if(GetProfileTextureCompleteEvent != null)
        //            GetProfileTextureCompleteEvent();
        //    }
        //    Debug.Log("Print Callback: " + lastResponse);
        //};
        //this.CallFBAPI(ApiQuery, Facebook.HttpMethod.GET, _callback, null);

        string url = "https" + "://graph.facebook.com/"+ FB.UserId +"/picture";
        url += "?access_token=" + FB.AccessToken;
        WWW www = new WWW(url);
        while (!www.isDone) {
            yield return www; 
        }

        if(www.error == null) {
			Debug.Log("Load profile pic complete...");
			profileTexture = www.texture;
        }
        else {
			Debug.Log("Load profile pic is error: " + www.error);
			if(profileTexture == null)
				profileTexture = Resources.Load("facebookProfile", typeof(Texture2D)) as Texture2D;
		}
		
		this.OnGetProfileTextureCompleteEvent();
	}

    internal void ManageProfilePicture()
	{
		if(FB.IsLoggedIn) {
			StartCoroutine_Auto(this.GetMyProfilePicture());
		}
		else {			
			if(profileTexture == null) {
				Debug.Log("Never login to facebook !");
				profileTexture = Resources.Load("facebookProfile", typeof(Texture2D)) as Texture2D;
			}
			
			this.OnGetProfileTextureCompleteEvent();
		}
	}

#endif
}
