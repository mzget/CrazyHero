using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using jsonFx = Pathfinding.Serialization.JsonFx;

public class BattleStage : MzBaseScene {

	public enum GameplayState {
		gamePlay = 0, gamePause, gameEnd,
	};
	public GameplayState curGameplayState;
    
    public AudioClip levelUpClip;
    public AudioClip clash_clip;
    public AudioClip coin_clip;
    public AudioClip potionDrop_clip;
    public AudioClip starDrop_clip;
    public AudioClip fail_clip;
    
    internal GameObject coin_obj;
    internal GameObject potion_obj;
    internal GameObject star_obj;

    //<@-- Game data.
    public int monstersKill { get; private set; }
    private int score;
    public int Score
    {
        get
        {
            var formular = (monstersKill / completeStageTime.TotalSeconds) * 10000;
            score = (int)formular;

            return score;
        }
    }

    //<@-- Game time.
    private DateTime startTime;
	internal TimeSpan completeStageTime { get; private set; }
    private TimeSpan currentTimeSpent;
    public TimeSpan GetCurrentTimeSpent()
    {
        float duration = Time.timeSinceLevelLoad;
        DateTime endtime = startTime.AddSeconds(duration);
        currentTimeSpent = endtime - startTime;

        return currentTimeSpent;

        //<@-- Using example.
        //TimeSpan span = new TimeSpan(BattleStage.GetInstance.GetCurrentTimeSpent().Hours, BattleStage.GetInstance.GetCurrentTimeSpent().Minutes, BattleStage.GetInstance.GetCurrentTimeSpent().Seconds);
        //string format = string.Format("{0:hh\\:mm\\:ss}", span);
        //Debug.Log("Time using: " + format);

        //ParseManager.GetInstance.SavePlayingTime(GUIManager.GetInstance.level.ToString(), format); 
    }


    protected override void Initialization()
    {
        base.Initialization();

		SaveManager.InitSingleton();
        BossManager.InitSingleton();
        HeroManager.InitSingleton();
        MonsterManager.InitSingleton();
		TavernShopManager.InitSingleton();
    }

	// Use this for initialization
	void Start () {
		base.PlayBackgroundMusic(background_clip, 0.6f);      

        coin_obj = Resources.Load("Prototypes/Coin_anim", typeof(GameObject)) as GameObject;
        potion_obj = Resources.Load("Prototypes/Potion", typeof(GameObject)) as GameObject;
        star_obj = Resources.Load("Prototypes/Star", typeof(GameObject)) as GameObject;
        //<@-- Load audioclips.
        coin_clip = Resources.Load(PathManager.Path_of_audioclips + "Coin", typeof(AudioClip)) as AudioClip;
        levelUpClip = Resources.Load(PathManager.Path_of_audioclips + "Success", typeof(AudioClip)) as AudioClip;
        fail_clip = Resources.Load(PathManager.Path_of_audioclips + "Fail", typeof(AudioClip)) as AudioClip;

        base.escapeCommandEvent += Handle_escapeCommandEvent;

        startTime = DateTime.UtcNow;
	}

	void Handle_escapeCommandEvent ()
	{
		//<@-- Ask user play leave gameplay.
		GUIManager.GetInstance.SetActivePauseGameWindow ();
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();
	}

	public void GameEnd ()
	{
		this.curGameplayState = BattleStage.GameplayState.gameEnd;

		completeStageTime = this.GetCurrentTimeSpent();
	}

    internal void AddMonsterKill()
    {
        this.monstersKill++;
	}
	
	#region <!-- Save game section.
	
	internal void SavingLevelComplete ()
	{
        HeroData curHero = new HeroData() { 
			name = HeroBeh.GetInstance.name, curExp = HeroBeh.GetInstance.curExp, stat = HeroBeh.GetInstance.stat 
		};
        HeroManager.Get_Instance.UpdateHeroDatas(ref curHero);

        StringBuilder output = new StringBuilder();
        jsonFx.JsonWriter writer = new jsonFx.JsonWriter(output);
        writer.Write(HeroManager.Get_Instance.dict_heroData);
        Debug.Log(output);

        SaveManager.GetInstance.heroesDatas = output.ToString();
		SaveManager.GetInstance.monsterKills += this.monstersKill;
		SaveManager.GetInstance.Save();
		
		//<@-- Upload high score to social.
		this.SyncDataToSocialServer();
	}
	
	private void SavingGameOver()
	{
		SaveManager.GetInstance.Save();
		
		//<@-- Upload high score to social.
		this.SyncDataToSocialServer();
	}
	
	private void SyncDataToSocialServer()
	{
//		if (MzFacebookIntegration.GetInstance == null) return;
//		
//		MzFacebookIntegration.GetInstance.PostScore(this.Score);
//		//<!-- Call open graph feed story.
//		MzFacebookIntegration.GetInstance.CreateOG_CompletedLevel();
	}
	
	#endregion
}
