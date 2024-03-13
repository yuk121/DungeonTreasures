using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReady : MonoBehaviour
{
    
    bool CheckAP()
    {
        if(PlayerDataManager.Instance != null)
        {
            if (PlayerDataManager.Instance.OwnedAP - StageDataManager.Instance.GetStageAP() >= 0)
                return true;
        }
        return false;
    }

    public void OnPressButton()
    {
        if (CheckAP() == true)
        {
            if (StageManager.Instance != null)
            {
                if (StageManager.Instance.GetNextStageInfo() != string.Empty)
                {
                    LoadSceneManager.Instance.LoadGameScene(StageManager.Instance.GetNextStageInfo());
                }
            }
        }
        else
        {
            if(PopupManager.Instance != null)
            {
                PopupManager.Instance.CreateOKPopup("알림", "행동력이 부족합니다!", null);
            }
        }
    }

    public void PlayEnterSfx()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayUISound(SoundManager.eUISoundAudioClip.EnterDungeon);
        }
    }
}
