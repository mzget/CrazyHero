using UnityEngine;
using System.Collections;

public class Mz_LoadingScreen : MonoBehaviour
{
	public static string TargetSceneName;

//	private AsyncOperation async;

//	public Transform backgroundSprite;

    void Start()
	{
		Time.timeScale = 1f;
		
//        Mz_ResizeScale.ResizingScale(backgroundSprite);
		Application.LoadLevel(TargetSceneName);
		
#if UNITY_STANDALONE_WIN

	    //<!!!!! WARNING...
	    //<!-- FOR DEBUG ONLY.
//	    async = Application.LoadLevelAsync(LoadSceneName);
//	    if (Application.isLoadingLevel) {
//	        while (async.isDone == false) {
//	            yield return 0;
//	        }
//	    }

#elif UNITY_IPHONE || UNITY_ANDROID || UNITY_FLASH || UNITY_WEBPLAYER
	
//        async = Application.LoadLevelAsync(LoadSceneName);
//		if(Application.isLoadingLevel) {
//			while(async.isDone == false) {
//				yield return 0;
//			}
//		}

#endif	
    }

//    void OnGUI()
//    {
//        GUI.depth = 0;
//        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / Main.FixedGameWidth, Screen.height / Main.FixedGameHeight, 1));
//
//        loadingGUISkin = GUI.skin.label;
//        loadingGUISkin.font = loadingFont;
//        loadingGUISkin.alignment = TextAnchor.MiddleCenter;
//        loadingGUISkin.fontSize = 32;
//        loadingGUISkin.normal.textColor = Color.white;
//
//	    //<!---  Show Loading progress. 
//	    float process = 0;
//	    if(async != null)
//	       process = async.progress * 100f;
//	    GUI.Box(new Rect(Main.GAMEWIDTH - 320, Main.GAMEHEIGHT - 64, 300, 50), "Loading... " + process.ToString("F1") + " %", loadingGUISkin);
//
//		GUI.Box(new Rect(Main.FixedGameWidth - 320, Main.FixedGameHeight - 64, 300, 50), "Loading... ", loadingGUISkin);
//    }
}