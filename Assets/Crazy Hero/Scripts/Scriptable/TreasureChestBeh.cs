using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Holoville.HOTween;
using MiniJSON;

public class TreasureChestBeh : MonoBehaviour {

//    private tk2dSpriteAnimator spriteAnimator;
//	Vector3 worldPos;
//    private int clickStat = 0;
//	public GameObject treasureCollect;
//	public GameObject treasureReward;
//	public MzGUIButtonBeh treasureCollect_button;
//	public tk2dTextMesh treasureReward_textmesh;
//	public InventoryControl inventoryControl = new InventoryControl();
//	public Character currentChar;
//
//	// Use this for initialization
//	void Start () {
//		worldPos = Camera.main.ScreenToWorldPoint(GUIManager.GetInstance.midCenter_screenPos);
//        worldPos += Vector3.down * 2f;
//        spriteAnimator = this.GetComponent<tk2dSpriteAnimator>();
//
//		StartCoroutine_Auto(MoveChestToCenterOfScreen());
//	}
//
//	IEnumerator MoveChestToCenterOfScreen ()
//	{
//		yield return new WaitForSeconds(2f);
//
//		HOTween.To(this.transform, 1f, new TweenParms().Prop("position", worldPos).Prop("localScale", Vector3.one).
//		           Ease(EaseType.EaseOutSine).OnComplete(() => { }));
//	}
//
//	IEnumerator initial() {
//		yield return new WaitForSeconds(0.5f);
//
//		GUIManager.GetInstance.treasureWindowUIObj.SetActive(true);
//		GUIManager.GetInstance.plane_fadeGUI.SetActive(true);
//
//		yield return null;
//
//		treasureCollect = GameObject.Find("TreasureCollect_button");
//		treasureCollect_button = treasureCollect.GetComponent<MzGUIButtonBeh>();
//		treasureReward = GameObject.Find("TreasureReward_textmesh");
//		treasureReward_textmesh = treasureReward.GetComponent<tk2dTextMesh>();
//		treasureCollect_button.clickEvent += TreasureCollect_buttonclick_event;
//		
//		int RewardChest = UnityEngine.Random.Range(1,3);
//
//		if(RewardChest==1){
//			RewardItem(SaveManager.GetInstance.currentLevel);
//		}else if(RewardChest>1){
//			RewardGold(SaveManager.GetInstance.currentLevel);
//		}
//	
//	}
//	
//	void RewardItem(int level) {
//		string[] npcListItem = new string [] {
//			"Herb_S",
//			"Herb_M",
//			"Herb_L",
//			"Health_S",
//			"Health_M",
//			"Soul_S",
//			"Soul_M",
//		};		
//		
//		inventoryControl.LoadItems();
//		var reader = Json.Deserialize(SaveManager.GetInstance.itemDataDict) as String[];
//        Debug.Log("itemDataDict: " + SaveManager.GetInstance.itemDataDict);
//        if(reader != null) {
//            string [] obj =	reader;
//            if(obj != null) {
//		        foreach (string tempItemName in obj) {
//			        currentChar.AddItem(inventoryControl.FindItem(tempItemName), true);
//		        }
//            }
//        }
//		
//		List<string> currentItemInBag = new List<string>();		
//		int random = UnityEngine.Random.Range(0, npcListItem.Length);
//		InventoryItem tempitem = inventoryControl.FindItem(npcListItem[random]);
//		treasureReward = GameObject.Find("TreasureReward");
//		treasureReward.GetComponent<tk2dSprite>().SetSprite(tempitem.fullName);
//		currentChar.AddItem(tempitem, true);
//		
//		
//		foreach (InventoryItem listItem in currentChar.inventory)
//		{
//			currentItemInBag.Add(listItem.name);
//		}
//
//		var writer = Json.Serialize(currentItemInBag);
//
//		SaveManager.GetInstance.itemDataDict = writer;
//		SaveManager.GetInstance.SaveTrade();		
//		
//		treasureReward_textmesh.text = tempitem.fullName;
//		treasureReward_textmesh.Commit();
//	}
//	
//	void RewardGold(int level) {
//		treasureReward = GameObject.Find("TreasureReward");
//		treasureReward.GetComponent<tk2dSprite>().SetSprite("Coins");
//		SaveManager.GetInstance.gold += 500;
//		treasureReward_textmesh.text = "Gold 500";
//		treasureReward_textmesh.Commit();
//	}
//	
//	void OnMouseDown() {
//        if(clickStat == 0) {
//			clickStat++;
//			spriteAnimator.Play("Open");
//			spriteAnimator.AnimationCompleted += (tk2dSpriteAnimator arg1, tk2dSpriteAnimationClip arg2) => {
//				StartCoroutine_Auto(initial());
//			};
//        }
//	}
//	
//	void TreasureCollect_buttonclick_event (object sender, System.EventArgs e)
//	{
//		GUIManager.GetInstance.treasureWindowUIObj.SetActive(false);
//	}
}
