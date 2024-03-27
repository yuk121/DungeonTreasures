using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player의 행동력 및 재화를 담당할 데이터 스크립트
/// </summary>
/// 
[System.Serializable]
public class PlayerData 
{
    public bool m_isBGMUnmute;
    public bool m_isSFXUnmute; 
    public bool[] m_playerClearDungeons;
    public bool[] m_playerOwnedCharacters;

    public int m_playerOwnedGold;
    public int m_playerActivePoint;
    public int m_gameBGMVolume;
    public int m_gameSFXVolume;

    public string m_lastAccessTime;
    public string m_firstAccessTime;
    public string m_googleAccessToken;
    public string m_googleIdToken;

    /// <summary>
    /// PlayerData 생성자
    /// </summary>
    public PlayerData()
    {
        
    }

    public void BasicSetting()
    {
        m_playerOwnedGold = Setting.m_basicGold;
        m_playerActivePoint = Setting.m_basicAtctivePoint;
        m_gameBGMVolume = Setting.m_gameBGMVolume;
        m_gameSFXVolume = Setting.m_gameSFXVolume;
        m_isBGMUnmute = Setting.m_bgmUnmute;
        m_isSFXUnmute = Setting.m_sfxUnmute;
        m_playerClearDungeons = new bool[Setting.m_stageNumMax];
        m_playerOwnedCharacters = new bool[Setting.m_characterNumMax];
        m_firstAccessTime = Setting.m_firstAccessTime;
        m_lastAccessTime = Setting.m_lastAccessTime;

        // 처음에 가지고 있는캐릭터
        for (int i = 0; i < 4; i++)
        {
            m_playerOwnedCharacters[i] = true;
        }

        // 던전 클리어 정보
        for (int i = 0; i < m_playerClearDungeons.Length; i++)
        {
            m_playerClearDungeons[i] = false;
        }
    }
}
