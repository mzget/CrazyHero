using UnityEngine;
using System.Collections; 

public class Mz_DebugLogingGUI : MonoBehaviour {

    private float windowPosition = 0;
    private int positionCheck = 2;
    private static string windowText = "";
    private Vector2 scrollViewVector = Vector2.zero;
    private GUIStyle debugBoxStyle;

    private float leftSide = 0.0f;
    private float debugWidth = 480f;
    public bool debugIsOn = false;


    void Start () {
        debugBoxStyle = new GUIStyle();

        debugBoxStyle.alignment = TextAnchor.UpperLeft;

        leftSide = 120; //Screen.width - debugWidth - 3;
    }
	
    public void debug(string newString) {
        windowText = newString + "\n" + windowText;
    }

    void OnGUI()
    {
        if (debugIsOn)
        {
            GUI.depth = 10;
            Rect logArea = new Rect(windowPosition, Screen.height - (Screen.height / 2), (Screen.width / 4) * 3, Screen.height / 2);
            if (GUI.Button(new Rect(0, logArea.y - 30, 75, 30), "Debug")) {
                if (positionCheck == 1) {
                    windowPosition = 0;
                    positionCheck = 2;
                }
                else {
                    windowPosition = leftSide;
                    positionCheck = 1;
                }
            }
            else if (GUI.Button(new Rect(80f, logArea.y - 30, 75, 30), "Clear"))
            {
                windowText = "";
            }

            GUI.BeginGroup(logArea, GUI.skin.box);
            {
                scrollViewVector = GUI.BeginScrollView(new Rect(0, 0, (Screen.width/4) * 3, Screen.height/2), scrollViewVector, new Rect(0.0f, 30, Screen.width, Screen.height * 4));
                {
                    GUI.Box(new Rect(0, 0, debugWidth - 20.0f, 2000.0f), windowText, debugBoxStyle);
                }
                GUI.EndScrollView();
            }
            GUI.EndGroup();
        }
    }
	
    #region <!-- Unity Log Callback.

    public string output = "";
    public string stack = "";
    void OnEnable()
    {
        Application.RegisterLogCallback(HandleLog);
    }
    void OnDisable()
    {
        Application.RegisterLogCallback(null);
    }
    public void HandleLog(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;

        this.debug(output);
    }

    #endregion
}