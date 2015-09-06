using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class MzBaseScene : MonoBehaviour {

	private static MzBaseScene instance;
	public static MzBaseScene GetInstance {
		get {
			if(instance == null) {
				instance = GameObject.FindObjectOfType<MzBaseScene>();
				if(instance == null) {
					Debug.LogError("BaseSceneController instance is null");
				}
			}
			return instance;
		}
	}

    public enum ScenesInstance
    {
        none = 0,
        LoadingScene = 1,
        MainMenu,
        WorldMap,
        SelectHero,
        SelectHelper,
        WaitForStart,
        TheForest,
    };
	public bool _isPauseGameplay = false;
	public bool _IsShowTraceFPS = false;
	protected Mz_DebugLogingGUI textDebug;
	protected HUDFPS hudFPS_Trace;
    [SerializeField]
    protected bool _enableOnScreenDebuging = false;

    //<!-- Audio Manage.
    public static bool ToggleMusicAudio = true;
    public static bool ToggleEffectAudio = true;
    public AudioEffectManager audioEffect;
	public AudioDescribeManager audioDescribe;
    public GameObject audioBackground_Obj;
    public AudioClip background_clip;
    //public List<AudioClip> description_clips = new List<AudioClip>();
    //public List<AudioClip> soundEffect_clips = new List<AudioClip>();
		
	protected bool _onDestroyScene = false;
    protected bool _hasQuitCommand = false;
	protected event Action escapeCommandEvent;

	protected virtual void OnDestroy() { }

	void Awake ()
	{
		MzOnGUIManager.CalculateViewportScreen();
		//<@-- Trace OnScreen FPS.
		if(_IsShowTraceFPS) {
			this.gameObject.AddComponent<HUDFPS> ();
			hudFPS_Trace = this.gameObject.GetComponent<HUDFPS>();
            hudFPS_Trace.startRect = new Rect((Screen.width / 2) - 60, 10, 120, 60);
		}
		
#if UNITY_EDITOR
		if(this._enableOnScreenDebuging) {
			this.gameObject.AddComponent<Mz_DebugLogingGUI>();
			textDebug = this.GetComponent<Mz_DebugLogingGUI>();
			textDebug.debugIsOn = true;
        }
#endif
		
		this.Initialization();
		this.InitializeAudio();
	}

	protected virtual void Initialization()
	{
		Debug.Log("Initialization... " + this.ToString());

        ToggleEffectAudio = bool.Parse(PlayerPrefs.GetString(LocalDeviceData.Key_EffectsAudioConfig, bool.TrueString));
        ToggleMusicAudio = bool.Parse(PlayerPrefs.GetString(LocalDeviceData.Key_MusicAudioConfig, bool.TrueString));
	}

	protected virtual void AdjustScreenRatio() {

	}

	#region <@-- Audio Management.

	protected void InitializeAudio()
    {
        //<!-- Setup All Audio Objects.
        if (audioEffect == null) {
			GameObject audioEffect_Obj = GameObject.FindGameObjectWithTag("AudioEffect");
			if(audioEffect_Obj) {
				audioEffect = audioEffect.GetComponent<AudioEffectManager>();
			}
			else {
				audioEffect_Obj = Instantiate(Resources.Load(PathManager.PATH_OF_AUDIO_OBJECTS + "AudioEffect", typeof(GameObject))) as GameObject;
				audioEffect_Obj.name = "AudioEffect";
				audioEffect = audioEffect_Obj.GetComponent<AudioEffectManager>();
			}
			
			if (audioEffect) {				
				audioEffect.GetComponent<AudioSource>().mute = !ToggleEffectAudio;
				audioEffect.alternativeEffect_source.GetComponent<AudioSource>().mute = !ToggleEffectAudio;
			}
		}
		
		if (audioDescribe == null)
		{
			var obj = GameObject.FindGameObjectWithTag("AudioDescribe");
			if (obj != null)
			{
				audioDescribe = obj.GetComponent<AudioDescribeManager>();
				audioDescribe.GetComponent<AudioSource>().mute = !ToggleMusicAudio;
			}
		}
		
		// <! Manage audio background.
		audioBackground_Obj = GameObject.FindGameObjectWithTag("AudioBackground");
        if (audioBackground_Obj == null)
        {
            audioBackground_Obj = new GameObject("AudioBackground", typeof(AudioSource));
            audioBackground_Obj.tag = "AudioBackground";
            audioBackground_Obj.GetComponent<AudioSource>().playOnAwake = true;
			audioBackground_Obj.GetComponent<AudioSource>().volume = 0.6f;
            audioBackground_Obj.GetComponent<AudioSource>().mute = !ToggleMusicAudio;

            DontDestroyOnLoad(audioBackground_Obj);
        }
        else { 
            audioBackground_Obj.GetComponent<AudioSource>().mute = !ToggleMusicAudio;
        }
    }

	protected void PlayBackgroundMusic (AudioClip bgClip,float _volume)
	{
		audioBackground_Obj.GetComponent<AudioSource>().clip = bgClip;
		audioBackground_Obj.GetComponent<AudioSource>().volume = _volume;
		audioBackground_Obj.GetComponent<AudioSource>().loop = true;
		audioBackground_Obj.GetComponent<AudioSource>().Play();  
	}

    protected void SetToggleMusicAudio() {
        ToggleMusicAudio = !ToggleMusicAudio;
        this.SetActiveMusicAudio();
        SaveManager.GetInstance.SaveLocalDeviceData();
    }

    protected void SetToggleEffectsAudio() {
        ToggleEffectAudio = !ToggleEffectAudio;
        this.SetActiveEffectsAudio();
        SaveManager.GetInstance.SaveLocalDeviceData();
    }

    void SetActiveMusicAudio() {
        audioBackground_Obj.GetComponent<AudioSource>().mute = !ToggleMusicAudio;
        if(audioDescribe != null)
            audioDescribe.GetComponent<AudioSource>().mute = !ToggleMusicAudio;
    }

    void SetActiveEffectsAudio() { 
        audioEffect.GetComponent<AudioSource>().mute = !ToggleEffectAudio;
    }
    
	#endregion

	internal IEnumerator IE_gotoOtherStage (MzBaseScene.ScenesInstance sceneName) {
        AsyncOperation async = Resources.UnloadUnusedAssets();
        while (!async.isDone)
            yield return null;

		this.OpenScene(sceneName.ToString());
	}

	internal IEnumerator IE_gotoOtherStage (string sceneName) {
		AsyncOperation async = Resources.UnloadUnusedAssets();
		while (!async.isDone) 
			yield return null;

		this.OpenScene(sceneName);
	}

	protected void OpenScene (string sceneName)
	{
		if(Application.isLoadingLevel == false) {
			Mz_LoadingScreen.TargetSceneName = sceneName;
			Application.LoadLevel(ScenesInstance.LoadingScene.ToString());
		}
	}
	
	// Update is called once per frame
	protected virtual void Update ()
	{   
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _hasQuitCommand = true;
			if(escapeCommandEvent != null)
				escapeCommandEvent();
        }
		else if(Input.GetKeyDown(KeyCode.Menu)) {
			Debug.Log("Keycode.Menu");
		}
	}

	#region <!-- ChangeTimeScale.

	public virtual void SetGamepauseState(bool _gameState) {
		_isPauseGameplay = _gameState;
		if(_gameState) {
			this.UpdateTimeScale(0);
		}
		else this.UpdateTimeScale(1);
	}

	public static event Action HasChangeTimeScale_Event;
	private void OnChangeTimeScale () {
		if (HasChangeTimeScale_Event != null) 
				HasChangeTimeScale_Event ();
	}
	public void UpdateTimeScale(int delta) {
		Time.timeScale = delta;
		this.OnChangeTimeScale();
	}

	#endregion

	protected virtual void MovingCameraTransform ()	{ }

    public virtual void OnInput(string nameInput) {
    	print("OnInput :: " + nameInput);
    }

    public virtual void OnPointerOverName(string nameInput) {
    	print("OnPointerOverName :: " + nameInput);
    }

    protected virtual void OnApplicationQuit() {
#if UNITY_IPHONE || UNITY_ANDROID
        //<-- to do asking for quit game.
#endif
    }
	
	protected virtual void OnApplicationPause(bool pauseStatus) {
        print("OnApplicationPause: " + pauseStatus);
    }
}
