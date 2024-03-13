using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class PlayerDataManager : DontDestory<PlayerDataManager>
{
    private PlayerData m_playerData;
    public PlayerData PlayerData { get { return m_playerData; } }
    private bool m_isLoad = false;
    public bool IsLoad { get { return m_isLoad; }  set { m_isLoad = value; } }

    public int OwnedGold { get { return m_playerData.m_playerOwnedGold; }}
    public int OwnedAP { get { return m_playerData.m_playerActivePoint; } }

    public void SetFirstAccessTime(string date)
    {
        Debug.Log("#로그  SetFirstAccessTime / date : " + date);
        m_playerData.m_firstAccessTime = date;
    }

    public string GetFirstAccessTime()
    {
        return m_playerData.m_firstAccessTime;
    }

    public void SetLastAccessTime(string date)
    {
        m_playerData.m_lastAccessTime = string.Format(date);
    }

    public string GetLastAccessTime()
    {
        return m_playerData.m_lastAccessTime;
    }

    public void SetPlayerData(string jsonString)
    {
        m_playerData = JsonMapper.ToObject<PlayerData>(jsonString);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetSoundVolume();
        }
    }

    public PlayerData GetPlayerData()
    {
        return m_playerData;
    }

    public void SetBGMVolume(int volume)
    {
        m_playerData.m_gameBGMVolume = volume;

        if(volume <= 0)
        {
            m_playerData.m_isBGMUnmute = false;
        }
        else
        {
            m_playerData.m_isBGMUnmute = true;
        }
    }

    public float GetBGMVolume()
    {
        return (float)m_playerData.m_gameBGMVolume / 100;
    }

    public bool GetBGMUnmute()
    {
        return m_playerData.m_isBGMUnmute;
    }

    public void SetSFXVolume(int volume)
    {
        m_playerData.m_gameSFXVolume = volume;

        if (volume <= 0)
        {
            m_playerData.m_isSFXUnmute = false;
        }
        else
        {
            m_playerData.m_isSFXUnmute = true;
        }
    }

    public float GetSFXVolume()
    {
        return (float)m_playerData.m_gameSFXVolume / 100;
    }

    public bool GetSFXUnmute()
    {
        return m_playerData.m_isSFXUnmute;
    }

    public void IncreaseGold(int gold)
    {
        m_playerData.m_playerOwnedGold = m_playerData.m_playerOwnedGold + gold;
    }

    public void DecreaseGold(int gold)
    {
        if(m_playerData.m_playerOwnedGold - gold < 0)
        {
            m_playerData.m_playerOwnedGold = 0;
        }
        else
        {
            m_playerData.m_playerOwnedGold = m_playerData.m_playerOwnedGold - gold;
        }
    }

    public void IncreaseAtcivePoint(int point,bool elaspedtime= false)
    {
       if(elaspedtime == true)
        {
            if (m_playerData.m_playerActivePoint + point > 150)
                m_playerData.m_playerActivePoint = 150;
            else
                m_playerData.m_playerActivePoint = m_playerData.m_playerActivePoint + point;
        }
       else
            m_playerData.m_playerActivePoint = m_playerData.m_playerActivePoint + point;
    }

    public void DecreaseActivePoint(int point)
    {
        if(m_playerData.m_playerActivePoint - point < 0)
        {
            return;
        }
        else
        {
            m_playerData.m_playerActivePoint = m_playerData.m_playerActivePoint - point;
        }
    }

    public void SetDungeonClear(int stageindex)
    {
        m_playerData.m_playerClearDungeons[stageindex] = true;
    }

    public bool GetDungeonClear(int stageIndex)
    {
        return m_playerData.m_playerClearDungeons[stageIndex];
    }

    public bool GetIsLoad()
    {
        return m_isLoad;
    }

    public void BasicSettingPlayer()
    {
        m_playerData = new PlayerData();
    }

#if UNITY_EDITOR
    public void SavePlayerData()
    {
        string jsonData = JsonMapper.ToJson(m_playerData);
        string path = $"{Application.persistentDataPath}/SaveData.json";
        File.WriteAllText(path, jsonData);
    }

    public void LoadPlayerData()
    {
        string path = $"{Application.persistentDataPath}/SaveData.json";

        if (File.Exists(path))
        {
            string jsonString = File.ReadAllText(path);

            if (!string.IsNullOrEmpty(jsonString))
            {
                m_playerData = JsonMapper.ToObject<PlayerData>(jsonString);
            }
        }
    }
#endif

    //#if (UNITY_ANDROID || UNITY_IPHONE)
    //public void APKSavePlayerData()
    //{
    //    string jsonData = JsonMapper.ToJson(m_playerData);
    //    var path = string.Format(Application.persistentDataPath + "/DungeonTreasures/SaveData.Json");
    //    File.WriteAllText(path, jsonData);
    //}

    //public PlayerData APKLoadPlayerData()
    //{
    //    var datapath = string.Format(Application.persistentDataPath + "/DungeonTreasures/SaveData.Json");

    //    if (Directory.Exists(Application.persistentDataPath + "/DungeonTreasures") == false)
    //    {
    //        Directory.CreateDirectory(Application.persistentDataPath + "/DungeonTreasures");
    //    }

    //    if (File.Exists(datapath))
    //    {
    //        string jsonString = File.ReadAllText(datapath);

    //        if (!string.IsNullOrEmpty(jsonString))
    //        {
    //            PlayerData playerData = JsonMapper.ToObject<PlayerData>(jsonString);

    //            return playerData;
    //        }
    //    }
    //    return null;
    //}
    //#endif

    protected override void OnAwake()
    {
        m_isLoad = false;
    }

}
