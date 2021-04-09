using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDebug{

    private static bool DebugButton = true;

    public static void Log(string context)
    {
        if (DebugButton)
        {
            Debug.Log(context);
        }
    }

    public static void LogVec3(Vector3 vec)
    {
        if (DebugButton)
        {
            Debug.Log("vec.x = " + vec.x + "vec.y = " + vec.y + "vec.z = " + vec.z);
        }
    }
}
