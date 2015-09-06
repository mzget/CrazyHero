using UnityEngine;
using System.Collections;

public class TavernShopManager : MonoBehaviour {

	private static TavernShopManager instance;
	public static TavernShopManager GetInstance {
		get{
			instance = GameObject.FindObjectOfType<TavernShopManager>();
			if(instance == null) {
				Debug.LogWarning("TavernShopManager instance is null...");
			}
			return instance;
		}
	}
	internal static void InitSingleton() {
		instance = GameObject.FindObjectOfType<TavernShopManager>();
		if(instance == null) {
			GameObject tavernShopManager = new GameObject("TavernShopManager", typeof(TavernShopManager));
			instance = tavernShopManager.GetComponent<TavernShopManager>();
		}
		else 
			Debug.Log("TavernShopManager instance already exist...");
	}

    BattleStage stageController;

	#region <@-- Shop manager.
	
	[SerializeField]    private MzGUIButtonBeh atkBtn;
	[SerializeField]    private MzGUIButtonBeh defBtn;
	[SerializeField]    private MzGUIButtonBeh criBtn;
	[SerializeField]    private MzGUIButtonBeh spdBtn;
	[SerializeField] private tk2dTextMesh atkPrice;
	[SerializeField] private tk2dTextMesh defPrice;
	[SerializeField] private tk2dTextMesh spdPrice;
	[SerializeField] private tk2dTextMesh criPrice;
	[SerializeField] private tk2dTextMesh atkLv, defLv, spdLv, criLv;

	private const int MAX_star = 4;
	private const int MAX_lv = 10;
	private int[,] upgradeAtkPrice = new int[MAX_star, MAX_lv] {
		{ 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900 },
		{ 2000, 2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800, 2900 },
		{ 3000, 3100, 3200, 3300, 3400, 3500, 3600, 3700, 3800, 3900 },
		{ 4000, 4100, 4200, 4300, 4400, 4500, 4600, 4700, 4800, 4900 },
	};
    private int[,] upgradeDefPrice = new int[MAX_star, MAX_lv] {
		{ 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900 },
		{ 2000, 2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800, 2900 },
		{ 3000, 3100, 3200, 3300, 3400, 3500, 3600, 3700, 3800, 3900 },
		{ 4000, 4100, 4200, 4300, 4400, 4500, 4600, 4700, 4800, 4900 },
	};
    private int[,] upgradeCriticalPrice = new int[MAX_star, MAX_lv] {
		{ 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900 },
		{ 2000, 2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800, 2900 },
		{ 3000, 3100, 3200, 3300, 3400, 3500, 3600, 3700, 3800, 3900 },
		{ 4000, 4100, 4200, 4300, 4400, 4500, 4600, 4700, 4800, 4900 },
	};
    private int[,] upgradeSpeedPrice = new int[MAX_star, MAX_lv] {
		{ 1000, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900 },
		{ 2000, 2100, 2200, 2300, 2400, 2500, 2600, 2700, 2800, 2900 },
		{ 3000, 3100, 3200, 3300, 3400, 3500, 3600, 3700, 3800, 3900 },
		{ 4000, 4100, 4200, 4300, 4400, 4500, 4600, 4700, 4800, 4900 },
	};
	
	#endregion

	// Use this for initialization
	IEnumerator Start () {
        stageController = (BattleStage)MzBaseScene.GetInstance;

		//@-- find tavern shop transform.
		Transform tavernShopT = GameObject.Find("Tavern_Shop").transform;
		var atkBtnT = tavernShopT.Find("Attack_button");
		var defBtnT = tavernShopT.Find("Defense_button");
		var criBtnT = tavernShopT.Find("Critical_button");
		var spdbtnT = tavernShopT.Find("Speed_button");

		atkBtn = atkBtnT.GetComponent<MzGUIButtonBeh>();
		atkPrice = atkBtnT.Find("Cost_TextMesh").GetComponent<tk2dTextMesh>();
		atkLv = atkBtnT.Find("Lv_TextMesh").GetComponent<tk2dTextMesh>();

		defBtn = defBtnT.GetComponent<MzGUIButtonBeh>();
		defPrice = defBtnT.Find("Cost_TextMesh").GetComponent<tk2dTextMesh>();
		defLv = defBtnT.Find("Lv_TextMesh").GetComponent<tk2dTextMesh>();

		criBtn = criBtnT.GetComponent<MzGUIButtonBeh>();
		criPrice = criBtnT.Find("Cost_TextMesh").GetComponent<tk2dTextMesh>();
		criLv = criBtnT.Find("Lv_TextMesh").GetComponent<tk2dTextMesh>();

		spdBtn = spdbtnT.GetComponent<MzGUIButtonBeh>();
		spdPrice = spdbtnT.Find("Cost_TextMesh").GetComponent<tk2dTextMesh>();
		spdLv = spdbtnT.Find("Lv_TextMesh").GetComponent<tk2dTextMesh>();

		atkBtn.clickEvent += HandleAtkBtnclickEvent;
		defBtn.clickEvent += HandleDefBtnclickEvent;
		criBtn.clickEvent += HandleCriBtnclickEvent;
		spdBtn.clickEvent += HandleSpdBtnclickEvent;

		while (!HeroBeh.GetInstance.isReady) {
			yield return null;
		}

		this.RefreshPriceTag();
	}

	void RefreshPriceTag ()
	{
		atkPrice.text = upgradeAtkPrice[HeroBeh.GetInstance.stat.attackStar, HeroBeh.GetInstance.stat.attackLevel].ToString();
		atkLv.text = "+" + HeroBeh.GetInstance.stat.attackLevel.ToString();

		defPrice.text = upgradeDefPrice[HeroBeh.GetInstance.stat.defenseStar, HeroBeh.GetInstance.stat.defenseLevel].ToString();
		defLv.text = "+" + HeroBeh.GetInstance.stat.defenseLevel.ToString();
		
		criPrice.text = upgradeCriticalPrice[HeroBeh.GetInstance.stat.criticalStar, HeroBeh.GetInstance.stat.criticalLevel].ToString();
		criLv.text = "+" + HeroBeh.GetInstance.stat.criticalLevel.ToString();

		spdPrice.text = upgradeSpeedPrice[HeroBeh.GetInstance.stat.speedStar, HeroBeh.GetInstance.stat.speedLevel].ToString();
		spdLv.text = "+" + HeroBeh.GetInstance.stat.speedLevel.ToString();
	}

	void HandleSpdBtnclickEvent (object sender, System.EventArgs e)
	{
        int spdStar = HeroBeh.GetInstance.stat.speedStar;
        int spdLevel = HeroBeh.GetInstance.stat.speedLevel;

        bool _canUpgrade = this.CheckCanBuyStat("Speed", spdStar, spdLevel);
        if (_canUpgrade) {
            //<@-- Upgrade atk function.
            GUIManager.GetInstance.RemoveGold(upgradeSpeedPrice[spdStar, spdLevel]);
            if (spdLevel++ < MAX_lv - 1)
            {
                HeroBeh.GetInstance.stat.speedLevel++;
                HeroBeh.GetInstance.stat.addSpd += 0.5f;
            }
            else
            {
                if (spdStar < MAX_star - 1)
                {
                    HeroBeh.GetInstance.stat.speedStar++;
                    HeroBeh.GetInstance.stat.speedLevel = 0;
                    HeroBeh.GetInstance.stat.addSpd += 0.5f;
                }
            }

            //<@-- Refresh stat GUI window.
			GUIManager.GetInstance.RefreshStatusWindow();
			this.RefreshPriceTag();
        }
	}

	void HandleCriBtnclickEvent (object sender, System.EventArgs e)
    {
        int criStar = HeroBeh.GetInstance.stat.criticalStar;
        int criLevel = HeroBeh.GetInstance.stat.criticalLevel;

        bool _canUpgrade = this.CheckCanBuyStat("Critical", criStar, criLevel);
        if (_canUpgrade)
        {
            //<@-- Upgrade atk function.
            GUIManager.GetInstance.RemoveGold(upgradeCriticalPrice[criStar, criLevel]);
            if (criLevel++ < MAX_lv - 1)
            {
                HeroBeh.GetInstance.stat.criticalLevel++;
                HeroBeh.GetInstance.stat.addCri += .5f;
            }
            else
            {
                if (criStar < MAX_star - 1)
                {
                    HeroBeh.GetInstance.stat.criticalStar++;
                    HeroBeh.GetInstance.stat.criticalLevel = 0;
                    HeroBeh.GetInstance.stat.addCri += .5f;
                }
            }

            //<@-- Refresh stat GUI window.
			GUIManager.GetInstance.RefreshStatusWindow();
			this.RefreshPriceTag();
        }
	}

	void HandleDefBtnclickEvent (object sender, System.EventArgs e)
	{
        int defStar = HeroBeh.GetInstance.stat.defenseStar;
        int defLevel = HeroBeh.GetInstance.stat.defenseLevel;

        bool _canUpgrade = this.CheckCanBuyStat("Defense", defStar, defLevel);
        if (_canUpgrade)
        {
            //<@-- Upgrade atk function.
            GUIManager.GetInstance.RemoveGold(upgradeDefPrice[defStar, defLevel]);
            if (defLevel++ < MAX_lv - 1)
            {
                HeroBeh.GetInstance.stat.defenseLevel++;
                HeroBeh.GetInstance.stat.addDef += 0.5f;
            }
            else
            {
                if (defStar < MAX_star - 1)
                {
                    HeroBeh.GetInstance.stat.defenseStar++;
                    HeroBeh.GetInstance.stat.defenseLevel = 0;
                    HeroBeh.GetInstance.stat.addDef += 0.5f;
                }
            }

            //<@-- Refresh stat GUI window.
			GUIManager.GetInstance.RefreshStatusWindow();
			this.RefreshPriceTag();
        }
	}

	void HandleAtkBtnclickEvent (object sender, System.EventArgs e)
	{
		int atkStar = HeroBeh.GetInstance.stat.attackStar;
		int atkLevel = HeroBeh.GetInstance.stat.attackLevel;

		bool _canUpgrade = this.CheckCanBuyStat("Attack", atkStar, atkLevel);
        if (_canUpgrade) { 
            //<@-- Upgrade atk function.
            GUIManager.GetInstance.RemoveGold(upgradeAtkPrice[atkStar, atkLevel]);
            if (atkLevel++ < MAX_lv - 1)
            {
                HeroBeh.GetInstance.stat.attackLevel++;
                HeroBeh.GetInstance.stat.addAtk += 2;
            }
            else {
                if (atkStar < MAX_star - 1)
                {
                    HeroBeh.GetInstance.stat.attackStar++;
                    HeroBeh.GetInstance.stat.attackLevel = 0;
                    HeroBeh.GetInstance.stat.addAtk += 2;
                }
            }

            //<@-- Refresh stat GUI window.
            GUIManager.GetInstance.RefreshStatusWindow();
			this.RefreshPriceTag();
        }
	}

	private bool CheckCanBuyStat (string statType, int star, int level)
	{
        bool result = false;
		//<@-- Check upgrade stat price.
        switch (statType)
        {
            case "Attack": 
                if (star < MAX_star - 1 || level < MAX_lv -1)
                {
                    if (SaveManager.GetInstance.gold >= upgradeAtkPrice[star, level])
                    {
                        result = true;
                    }
                }
                else
                    result = false;
                break;
            case "Defense":
                if (star < MAX_star - 1 || level < MAX_lv - 1)
                {
                    if (SaveManager.GetInstance.gold >= upgradeDefPrice[star, level])
                    {
                        result = true;
                    }
                }
                else
                    result = false;
                break;
            case "Critical":
                if (star < MAX_star - 1 || level < MAX_lv - 1)
                {
                    if (SaveManager.GetInstance.gold >= upgradeCriticalPrice[star, level])
                    {
                        result = true;
                    }
                }
                else
                    result = false;
                break;
            case "Speed":
                if (star < MAX_star - 1 || level < MAX_lv - 1)
                {
                    if (SaveManager.GetInstance.gold >= upgradeSpeedPrice[star, level])
                    {
                        result = true;
                    }
                }
                else
                    result = false;
                break;
            default:
                break;
        }

        if (result == false)
            MzBaseScene.GetInstance.audioEffect.PlayOnecWithOutStop(stageController.fail_clip);

        return result;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
