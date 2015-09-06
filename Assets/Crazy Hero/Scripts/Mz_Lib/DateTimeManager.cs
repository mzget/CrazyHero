using UnityEngine;
using System;
using System.Collections;

public class DateTimeManager {

    private static DateTimeManager Instance;
    public static DateTimeManager GetInstance {
        get { 
            if(Instance == null) {
                DateTimeManager.InitSingleton();
            }
            return Instance;
        }
    }

    internal static void InitSingleton()
    {
        if(Instance == null) {
//            Instance = new DateTimeManager();
        }
        else {
            Debug.Log("Singleton instance is already exist.");
        }
    }
    
	public DateTime currentDate;


    //public DateTimeManager() {
    //    Debug.Log("Create DateTimeManager instance!");

    //    currentDate = DateTime.UtcNow;
    //}


}
