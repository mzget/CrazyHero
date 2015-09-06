using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using jsonFx = Pathfinding.Serialization.JsonFx;

public class HeroManager : MonoBehaviour {

	private static HeroManager instance;
	public static HeroManager Get_Instance {
		get {
            instance = GameObject.FindObjectOfType<HeroManager>();
			if (instance == null) {
                InitSingleton();
			}
			return instance;
		}
	}

    internal static void InitSingleton()
    {
        instance = GameObject.FindObjectOfType<HeroManager>();
        if (instance == null) {
            GameObject managerObj = new GameObject("HeroManager", typeof(HeroManager));
            instance = managerObj.GetComponent<HeroManager>();
        }
        else {
            Debug.Log("HeroManager instance is already exist...");
        }
    }

    public const string Paladin = "Paladin";
    public const string Hunter = "Hunter";
    public const string Bishop = "Bishop";
    public const string Saber = "Saber";
    public const string Witch = "Witch";
	public const string Lancer = "Lancer";

    public static List<string> AllHeroNames_List = new List<string>() { 
        "Paladin",
        "Hunter",
        "Bishop",
        "Saber",
        "Witch",
    };
	public Dictionary<string, HeroData> dict_heroData;
	public bool isReady = false;

    void OnDestroy() {
        instance = null;
    }

	// Use this for initialization
	void Start () {
		dict_heroData = new Dictionary<string, HeroData>();

        var dict = jsonFx.JsonReader.Deserialize<Dictionary<string, object>>(SaveManager.GetInstance.heroesDatas);
		if(dict == null || dict.Count == 0) {
			Debug.LogWarning("heroesDatas is invalid...");

            dict_heroData.Add(Lancer, new HeroData() { 
				name = Lancer, curExp = 0, stat = new Status() { 
					attack = 50, defense = 1, maxHP = 500, regen = 1, walkSpeed = 10, level = 1, attributes = Status.Attributes.Strength,
				} 
			});
		}
		else {
            foreach (Dictionary<string, object> item in dict.Values)
            {
                HeroData curHeroData = new HeroData();
				curHeroData.name = item["name"].ToString();
				curHeroData.curExp = float.Parse(item["curExp"].ToString());
				var statDict = item["stat"] as Dictionary<string, object>;
				this.GetStat(ref statDict, ref curHeroData);

                dict_heroData.Add(curHeroData.name, curHeroData);
            }
		}

		string json = jsonFx.JsonWriter.Serialize(dict_heroData);
//		string json = MiniJSON.Json.Serialize(dict_heroData);
		Debug.Log("dict_heroData: " + json);

		isReady = true;
	}

	void GetStat (ref Dictionary<string, object> statDict, ref HeroData curHero)
	{
        curHero.stat = new Status();
        curHero.stat.level = int.Parse(statDict["level"].ToString());
		curHero.stat.attack = int.Parse(statDict["attack"].ToString());
        curHero.stat.defense = float.Parse(statDict["defense"].ToString());
        curHero.stat.maxHP = float.Parse(statDict["maxHP"].ToString());
        curHero.stat.regen = float.Parse(statDict["regen"].ToString());
        curHero.stat.walkSpeed = float.Parse(statDict["walkSpeed"].ToString());
        curHero.stat.attributes = (Status.Attributes)Enum.Parse(typeof(Status.Attributes), statDict["attributes"].ToString());
        //<@-- upgrading stat.
        curHero.stat.addAtk = int.Parse(statDict["addAtk"].ToString());
        curHero.stat.attackStar = int.Parse(statDict["attackStar"].ToString());
        curHero.stat.attackLevel = int.Parse(statDict["attackLevel"].ToString());

		curHero.stat.addCri = float.Parse(statDict["addCri"].ToString());
		curHero.stat.criticalStar = int.Parse(statDict["criticalStar"].ToString());
		curHero.stat.criticalLevel = int.Parse(statDict["criticalLevel"].ToString());

		curHero.stat.addDef = float.Parse(statDict["addDef"].ToString());
		curHero.stat.defenseStar = int.Parse(statDict["defenseStar"].ToString());
		curHero.stat.defenseLevel = int.Parse(statDict["defenseLevel"].ToString());

		curHero.stat.addSpd = float.Parse(statDict["addSpd"].ToString());
		curHero.stat.speedStar = int.Parse(statDict["speedStar"].ToString());
		curHero.stat.speedLevel = int.Parse(statDict["speedLevel"].ToString());
	}

	internal void UpdateHeroDatas(ref HeroData _data) {
		if(dict_heroData.ContainsKey(_data.name)) {
			dict_heroData[_data.name] = _data;
		}
		else {
			Debug.LogError(_data.name + " is Invalid!");
		}
	}
}