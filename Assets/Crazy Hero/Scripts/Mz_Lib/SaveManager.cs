using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDeviceData {
    public const string Key_MusicAudioConfig = "Key_MusicAudioConfig";
    public const string Key_EffectsAudioConfig = "Key_EffectsAudioConfig";
};

public class FacebookData
{
    public static string ID;
    public static string FacebookName;
    public static string FacebookUsername;
    public static string FirstName;
    public static string LastName;
    public static string Email;
    public static string Birthday;
};

public class SaveManager : ISaveData
{	
    private static SaveManager instance;
    public static SaveManager GetInstance {
        get {
            if(instance == null) {
				Debug.LogWarning("SaveManager instance is null.");
            }
            return instance; 
        }
    }
    internal static void InitSingleton() {
        if (instance == null)
        {
            instance = new SaveManager();
        }
        else {
            Debug.LogWarning("SaveManager instance is already exist..!");
        }
    }

	public const string KEY_SYSTEM_LANGUAGE = "SYSTEM_LANGUAGE";
	public static int Language_id;

	/// <summary>
	/// Storage data key.
	/// </summary>
	//<!-- User Name.
	public const string KEY_USERNAME = "KEY_USERNAME";
	public string username = "";
	
	public const string KEY_EVENTDATA = "KEY_EVENTDATA";
	public string eventdata;
	public const string KEY_PLAY_EVENT_DATE = "KEY_PLAY_EVENT_DATE";
	public string playEventDate;
	
	public const string KEY_OF_GOLD = "KEY_Gold";
	public int gold;

    public const string KEY_Hightest_Stage = "KEY_Hightest_Stage";
    public int highestLevel;

	public const string KEY_CURRENT_LEVEL = "KEY_CURRENT_LEVEL";
	public int currentLevel;

    public const string KEY_MONSTER_KILLS = "KEY_MONSTER_KILLS";
    public int monsterKills;
	
    public const string KEY_TimeToPlay = "KEY_TimeToPlay";
    public TimeSpan timeToPlay;

	public const string KEY_REMAIN_EXP = "KEY_REMAIN_EXP";
	public int remainEXP;
	
	public const string KEY_ITEM_DATA_DICT = "KEY_SUPPORT_ITEM_DICT";
	public string itemDataDict;

    public const string KEY_HeroesDatas = "KEY_HeroesDatas";
    public string heroesDatas;

    public class PurchasedInfo {
        public const string KEY_PURCHASED_CARD_SLOT = "KEY_PURCHASED_CARD_SLOT";
        public string purchasedCardSlots;
    };

    public PurchasedInfo purchasedInfo;
    public LocalDeviceData localDeviceData;

    public SaveManager() {
        purchasedInfo = new PurchasedInfo();
		localDeviceData = new LocalDeviceData();
		//<@-- Load storage data when create instance.
		this.Load();
    }
	
	public void SaveItem() {
        PlayerPrefs.SetString(KEY_ITEM_DATA_DICT, itemDataDict);
	}
	public void SaveTrade() {
		PlayerPrefs.SetInt(KEY_OF_GOLD, gold);
        PlayerPrefs.SetString(KEY_ITEM_DATA_DICT, itemDataDict);
	}
	
	public void Save() {
		Debug.Log("SaveData To PermanentMemory.");
		
		PlayerPrefs.SetString(SaveManager.KEY_USERNAME, username);
		PlayerPrefs.SetString(KEY_HeroesDatas, heroesDatas);
		PlayerPrefs.SetInt(KEY_Hightest_Stage, highestLevel);
		PlayerPrefs.SetInt (KEY_CURRENT_LEVEL, currentLevel);
        PlayerPrefs.SetInt(KEY_MONSTER_KILLS, monsterKills);
		PlayerPrefs.SetInt (KEY_REMAIN_EXP, remainEXP);
        PlayerPrefs.SetInt(KEY_OF_GOLD, gold);
		PlayerPrefs.SetString(KEY_ITEM_DATA_DICT,itemDataDict);

        this.SavePurchasedInfo();

        PlayerPrefs.Save();
	}
    
    internal void SavePurchasedInfo()
    {
        PlayerPrefs.SetString(PurchasedInfo.KEY_PURCHASED_CARD_SLOT, purchasedInfo.purchasedCardSlots);
    }
	
	public void SaveEVENTDATA() {
		PlayerPrefs.SetString(SaveManager.KEY_EVENTDATA, eventdata);
		PlayerPrefs.SetString(SaveManager.KEY_PLAY_EVENT_DATE, playEventDate);
	}
    
    internal void SaveLocalDeviceData()
    {
        PlayerPrefs.SetString(LocalDeviceData.Key_MusicAudioConfig, MzBaseScene.ToggleMusicAudio.ToString());
        PlayerPrefs.SetString(LocalDeviceData.Key_EffectsAudioConfig, MzBaseScene.ToggleEffectAudio.ToString());
    }

    public void Load() {
        Debug.Log("Load storage data to static variable...");

        username = PlayerPrefs.GetString(KEY_USERNAME, "Guest");
		heroesDatas = PlayerPrefs.GetString(KEY_HeroesDatas, string.Empty);
        highestLevel = PlayerPrefs.GetInt(KEY_Hightest_Stage);
		currentLevel = PlayerPrefs.GetInt (KEY_CURRENT_LEVEL);
        monsterKills = PlayerPrefs.GetInt(KEY_MONSTER_KILLS);
		remainEXP = PlayerPrefs.GetInt (KEY_REMAIN_EXP);
        gold = PlayerPrefs.GetInt(KEY_OF_GOLD, 1000000);
		itemDataDict = PlayerPrefs.GetString(KEY_ITEM_DATA_DICT);

        this.LoadPurchasedInfo();
		this.LoadGameEventsPlayDate();

        Debug.Log("Current Username: " + username);
    }

    internal void LoadPurchasedInfo()
    {
        purchasedInfo.purchasedCardSlots = PlayerPrefs.GetString(PurchasedInfo.KEY_PURCHASED_CARD_SLOT);
    }

	void LoadGameEventsPlayDate ()
	{
		playEventDate = PlayerPrefs.GetString(SaveManager.KEY_PLAY_EVENT_DATE, "");
	}
	
	public void DeleteSave() {}
}

