using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataManager : DontDestory<StageDataManager>
{
    int m_stageIndex;
    int m_stageActivePoint;
    int m_stageGold;
    int m_stageExp;
    bool m_isStageClear = false;

    public int GetStageGoldInfo()
    {
        return m_stageGold;
    }

    public int GetStageExpInfo()
    {
        return m_stageExp;
    }

    public bool GetStageClear()
    {
        return m_isStageClear;
    }

    public int GetStageIndex()
    {
        return m_stageIndex;
    }

    public int GetStageAP()
    {
        return m_stageActivePoint;
    }

    public void SetStageInfo(int stageIndex, int activePoint, int exp, int gold, bool isClear)
    {
        m_stageIndex = stageIndex;
        m_stageActivePoint = activePoint;
        m_stageExp = exp;
        m_stageGold = gold;
        m_isStageClear = isClear;
    }

    public void ClearStageInfo()
    {
        m_stageActivePoint = 0;
        m_stageExp = 0;
        m_stageGold = 0;
    }

    public void ClearCheck()
    {
        m_isStageClear = true;
    }
}
