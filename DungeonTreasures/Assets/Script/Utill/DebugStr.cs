#define _DebugMode
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugStr : MonoBehaviour
{
    //디버그 로그를 관리하기 위한 스크립트
    public static void Log(object msg)
    {
#if _DebugMode
        Debug.Log(msg);
#endif
    }

    public static void LogWarning(object msg)
    {
#if _DebugMode
        Debug.LogWarning(msg);
#endif
    }

    public static void LogError(object msg)
    {
#if _DebugMode
        Debug.LogError(msg);
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
