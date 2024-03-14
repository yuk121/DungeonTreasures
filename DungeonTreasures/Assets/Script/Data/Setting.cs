using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : DontDestory<Setting>
{
    public static int m_basicGold = 2000;
    public static int m_basicAtctivePoint = 150;
    public static int m_basicGem = 100;
    public static int m_stageNumMax = 50;
    public static int m_characterNumMax = 10;
    public static int m_gameBGMVolume = 100;
    public static int m_gameSFXVolume = 100;
    public static bool m_bgmUnmute = true;
    public static bool m_sfxUnmute = true;
    public static string m_firstAccessTime = string.Empty;
    public static string m_lastAccessTime = string.Empty;
}
