using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Pathfinding.Serialization.JsonFx;

public class BossManager : MonoBehaviour {

	private static BossManager instance;
	public static BossManager GetInstance {
		get { 
			instance = GameObject.FindObjectOfType<BossManager>();
			if(instance == null) {
                Debug.LogError("BossManager instance is null!");
			}
			return instance;
		}
	}
    public static void InitSingleton() {
        instance = GameObject.FindObjectOfType<BossManager>();
        if (instance == null) { 
		    var bossManager = new GameObject("BossManager", typeof(BossManager));
            instance = bossManager.GetComponent<BossManager>();
        }
    }
	public bool isReady { get; private set; }

	public const string Asmodius = "Asmodius";
	public const string SkeletonKing = "Skeleton King";
	public const string WizardSkeleton = "Wizard Skeleton";
    public const string BlackWizardWyvern = "Black Wizard Wyvern";
    public Dictionary<string, object> bossDatas = new Dictionary<string, object>();

    [SerializeField]
    private TextAsset bossText;


	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
		if (!File.Exists(Application.dataPath + "/BossData.txt")) { 
			Debug.Log("No have boss data config file.");
            Status asmodiusStat = new Status() { attack = 100, defense = 1, maxHP = 1000, regen = 5, };
            Status skeletonKingStat = new Status() { attack = 100, defense = 2, maxHP = 1000, regen = 5, };
            Status wizardSkeletonStat = new Status() { attack = 100, defense = 1, maxHP = 500, regen = 5, };
            Status blackWizardWyvernStat = new Status() { attack = 150, defense = 1, maxHP = 1000, regen = 5 };

            Dictionary<string, Status> bossDict = new Dictionary<string, Status>();
			bossDict.Add(Asmodius, asmodiusStat);
			bossDict.Add(SkeletonKing, skeletonKingStat);
			bossDict.Add(WizardSkeleton, wizardSkeletonStat);
            bossDict.Add(BlackWizardWyvern, blackWizardWyvernStat);

            StringBuilder output = new StringBuilder();
            JsonWriterSettings setting = new JsonWriterSettings() { PrettyPrint = true };
            JsonWriter writer = new JsonWriter(output, setting);
            writer.Write(bossDict);
            using (StreamWriter write = new StreamWriter(Application.dataPath + "/BossData.txt")) { 
                write.WriteLine(output);
            }
        }
        else {
			this.LoadBossDataFormSource();
        }
#else
		this.LoadBossDataFormSource();
#endif

		isReady = true;
	}

	void LoadBossDataFormSource ()
	{
		bossText = Resources.Load ("BossData", typeof(TextAsset)) as TextAsset;
		using (StringReader reader = new StringReader(bossText.text)) {
			string readLine = reader.ReadToEnd();
			print("readLine: " + readLine);
			this.ReadStatData(readLine);
		}
	}
	
	private void ReadStatData(string textData)
	{
		StringBuilder builder = new StringBuilder(textData);
		JsonReader reader = new JsonReader(builder.ToString());
        bossDatas = reader.Deserialize(typeof(Dictionary<string, object>)) as Dictionary<string, object>;
		if (bossDatas == null || bossDatas.Count == 0) {
            Debug.LogError("BossData is invalid: " + bossDatas);
			return;
        }

		if(bossDatas.ContainsKey(Asmodius)) {
			var stat = bossDatas[Asmodius] as Dictionary<string, object>;
			if(stat == null || stat.Count == 0) {
				Debug.LogError("boss stat is invalid!");
				return;
			}
		}
    }

	public Status GetStatByName (string name)
	{
		var stats = bossDatas[name] as Dictionary<string, object>;
		Status stat = new Status();
		stat.attack = int.Parse(stats["attack"].ToString());
		stat.defense = float.Parse(stats["defense"].ToString());
		stat.maxHP = float.Parse(stats["maxHP"].ToString());
		stat.regen = float.Parse(stats["regen"].ToString());

		return stat;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
