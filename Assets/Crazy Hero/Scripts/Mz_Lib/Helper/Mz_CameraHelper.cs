using UnityEngine;
using System.Collections;

public class Mz_CameraHelper : MonoBehaviour {

	public const float Ratio_4Per3 = 4f/3f;
	public const float Ratio_3Per2 = 3f/2f;
	public const float Ratio_16Per9 = 16f/9f;
	public const float Ratio_16Per10 = 16f / 10f;

	private static float currentRatio;
	public static float GetCurrentRatio {
		get {
			currentRatio = (float)Screen.width / (float)Screen.height;

			return currentRatio;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// Not work.
    /// </summary>
    /// <returns></returns>
	internal static Rect ChangeNormalizeViewportRect() {
		float aspect = (float)Screen.width / (float)Screen.height;
		float y = 0.095f; 
		float height = 0.8f;
		float width = height / aspect;
		Rect rect = new Rect((1f - width)/2, y, width, height);

		Debug.Log (aspect);
		Debug.Log (rect.x + ":" + rect.y + ":" + rect.width + ":" + rect.height);

		return rect;
	}

	// Compares two floating point numbers and return true if they are the same number.
	// See also Mathf.Approximately, which compares floating point numbers so you dont have 
	// to create a function to compare them.
	public static bool IsApproximately(float a, float b) {
		if(Mathf.Abs(a-b) <= 0.05f)
			return true;
		else
			return false;
	}
}
