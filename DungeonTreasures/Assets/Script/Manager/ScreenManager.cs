using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : DontDestory<ScreenManager>
{
    // Start is called before the first frame update
    void Start()
    {
        // 오토시 NeverSleep으로, 일반적으로 System.Setting으로 바꿔주는게 맞는것 같다. 
        // 일반적인 경우 코루틴을 사용해서 시간을 정해서 해당시간이 넘어가는 경우 시스템 세팅으로 바뀌게
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

}
