using UnityEngine;
using System.Collections;

public class Main
{
    public const float GAMEWIDTH = 1024f;
    public const float GAMEHEIGHT = 768f;

    public const float iPad_Ratio = 4f / 3f;
    public const float Ratio_3per2 = 3f / 2f;
    public const float Ratio_16per9 = 16f / 9f;
    public const float Ratio_10per16 = 16f / 10f;
    public const float Ratio_15per9 = 15f / 9f;

    public static float Fixedratio = 4f / 3f;
    public const float FixedGameWidth = 1024f;
    public const float FixedGameHeight = 768f;
    public const float HD_HEIGHT = 720f;

    private static float currentRatio;
    public static float GetCurrentRatio
    {
        get {
            currentRatio = (float)Screen.width / (float)Screen.height;

            return currentRatio;
        }
    }

    public enum EnumDeviceMode { 
		Simulation = 0,
		RealDevice,
	};
    public static EnumDeviceMode TargetDeviceMode;

	public enum EnumRuntimeMode {
		ReleaseMode = 0,
		DebugMode = 1,
	};
	public static EnumRuntimeMode RuntimeMode = EnumRuntimeMode.DebugMode;

    public class Mz_AppVersion
    {
        public enum AppVersion { Free = 0, Pro = 1 };
        public AppVersion appVersion;
        public static AppVersion getAppVersion;
    }

    public class Mz_AppLanguage {
        public enum SupportLanguage {
            EN = 1,
            TH = 0,
        };
        public static SupportLanguage appLanguage;
    }

    // Compares two floating point numbers and return true if they are the same number.
    // See also Mathf.Approximately, which compares floating point numbers so you dont have 
    // to create a function to compare them.
    public static bool IsApproximately(float a, float b)
    {
        if (Mathf.Abs(a - b) <= 0.05f)
        {
//            Debug.Log("current ratio value == " + a + "/" + b);
            return true;
        }
        else
            return false;
    }
}