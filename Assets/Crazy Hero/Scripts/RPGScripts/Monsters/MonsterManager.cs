using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterManager : MonoBehaviour {

	private static MonsterManager instance;
	public static MonsterManager GetInstance {
		get {
			if(instance == null) {
				instance = GameObject.FindObjectOfType<MonsterManager>();
				if(instance == null) {
					Debug.LogError("Monster controller instance is null.");
				}
			}
			return instance;
		}
	}    

    internal static void InitSingleton()
    {
        instance = GameObject.FindObjectOfType<MonsterManager>();
        if (instance == null) {
            GameObject pool = new GameObject("MonstersPool", typeof(MonsterManager));
            instance = pool.GetComponent<MonsterManager>();
        }
        else {
            Debug.LogWarning("MonstersPool object instance is already exist...");
        }
    }

	public List<GameObject> monsterPrototypes = new List<GameObject>();
	public string[] nativeMonsterNames;
    private int monstersPerWave = 6;
    private int finalMonsterWave = 18;

    private GameObject[] monsterCollection0;
    private GameObject[] monsterCollection1;
    private GameObject[] monsterCollection2;
    private GameObject[] monsterCollection3;
    private GameObject[] monsterCollection4;
    private GameObject[] monsterCollection5;
    private string[] theForest = new string[] { "SkeletonSoldier", "OfficerSkeleton", "CommanderSkeleton" };
	private string[] theDarkCave = new string[] { "Flyeye", "Gargoyle", "JrGargoyle", };
	
	internal GameObject damageEffectObj;

	void Awake() {
        if (BattleNames.SelectedBattleMode == string.Empty) 
            BattleNames.SelectedBattleMode = BattleNames.TheDarkCave;

        if (BattleNames.SelectedBattleMode == BattleNames.TheForest)
            nativeMonsterNames = theForest;
        else if (BattleNames.SelectedBattleMode == BattleNames.TheDarkCave)
            nativeMonsterNames = theDarkCave;
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < nativeMonsterNames.Length; i++) {
			monsterPrototypes.Add(Resources.Load(PathManager.Path_of_monsters + nativeMonsterNames[i], typeof(GameObject)) as GameObject);
		}

        monsterCollection0 = new GameObject[monstersPerWave];
        monsterCollection1 = new GameObject[monstersPerWave];
        monsterCollection2 = new GameObject[monstersPerWave];
        monsterCollection3 = new GameObject[monstersPerWave];
        monsterCollection4 = new GameObject[monstersPerWave];
        monsterCollection5 = new GameObject[finalMonsterWave];

		StartCoroutine_Auto(this.CreateMonsters());

		damageEffectObj = Resources.Load(PathManager.Path_Of_Effects+ "Damage", typeof(GameObject)) as GameObject;
	}

	IEnumerator CreateMonsters ()
	{
		StartCoroutine_Auto(CreateMonsterCollection0());
		StartCoroutine_Auto(CreateMonsterCollection1());
		StartCoroutine_Auto(CreateMonsterCollection2());
		StartCoroutine_Auto(CreateMonsterCollection3());
		StartCoroutine_Auto(CreateMonsterCollection4());
		StartCoroutine_Auto(CreateMonsterCollection5());
		yield return null;
	}

	IEnumerator CreateMonsterCollection0 ()
	{
		for (int i = 0; i < monstersPerWave; i++) {
			int r = Random.Range(0, monsterPrototypes.Count);
			if(monsterCollection0[i] == null) {
				GameObject monster = Instantiate(monsterPrototypes[r]) as GameObject;
				monster.transform.position = new Vector3(15 + (5 * i), -3.2f, 0f);
                monster.transform.parent = this.transform;
				monsterCollection0[i] = monster;
			}
		}
		yield return null;
	}

	IEnumerator CreateMonsterCollection1 ()
	{
		for (int i = 0; i < monstersPerWave; i++) {
			int r = Random.Range(0, monsterPrototypes.Count);
			if(monsterCollection1[i] == null) {
				GameObject monster = Instantiate(monsterPrototypes[r]) as GameObject;
				monster.transform.position = new Vector3(65 + (5 * i), -3.2f, 0f);
                monster.transform.parent = this.transform;
				monsterCollection1[i] = monster;
			}
		}
		yield return null;
	}

	IEnumerator CreateMonsterCollection2 ()
	{
		for (int i = 0; i < monstersPerWave; i++) {
			int r = Random.Range(0, monsterPrototypes.Count);
			if(monsterCollection2[i] == null) {
				GameObject monster = Instantiate(monsterPrototypes[r]) as GameObject;
				monster.transform.position = new Vector3(115 + (5 * i), -3.2f, 0f);
				monster.transform.parent = this.transform;
				monsterCollection2[i] = monster;
			}
		}
		yield return null;
	}

	IEnumerator CreateMonsterCollection3 ()
	{
		for (int i = 0; i < monstersPerWave; i++) {
			int r = Random.Range(0, monsterPrototypes.Count);
			if(monsterCollection3[i] == null) {
				GameObject monster = Instantiate(monsterPrototypes[r]) as GameObject;
				monster.transform.position = new Vector3(165 + (5 * i), -3.2f, 0f);
				monster.transform.parent = this.transform;
				monsterCollection3[i] = monster;
			}
		}
		yield return null;
	}

	IEnumerator CreateMonsterCollection4 ()
	{
		for (int i = 0; i < monstersPerWave; i++) {
			int r = Random.Range(0, monsterPrototypes.Count);
			if(monsterCollection4[i] == null) {
				GameObject monster = Instantiate(monsterPrototypes[r]) as GameObject;
				monster.transform.position = new Vector3(215 + (5 * i), -3.2f, 0f);
				monster.transform.parent = this.transform;
				monsterCollection4[i] = monster;
			}
		}
		yield return null;
	}

	IEnumerator CreateMonsterCollection5 ()
	{
		for (int i = 0; i < finalMonsterWave; i++) {
			int r = Random.Range(0, monsterPrototypes.Count);
			if(monsterCollection5[i] == null) {
				GameObject monster = Instantiate(monsterPrototypes[r]) as GameObject;
                monster.transform.position = new Vector3(265 + (5 * i), -3.2f, 0f);
				monster.transform.parent = this.transform;
				monsterCollection5[i] = monster;
			}
		}
		yield return null;
	}

	public void MonsterResurrection ()
	{
		StartCoroutine_Auto(this.CreateMonsters());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
