using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSkill : DontDestory<TableSkill>
{
    public class SkillData
    {
        public string SkillId;
        public string Name;
        public int EffectId;
        public int CoolTime;
        public int Duration;
        public int Value;
        public int HitRate;
        public float AttackRange;
        public string SkillType;
        public string AttackType;
        public string TargetType;
    }

    public Dictionary<string, SkillData> m_skillDicData = new Dictionary<string, SkillData>();

    public SkillData GetSkillData(string key)
    {
        SkillData skillData;

        bool check = m_skillDicData.TryGetValue(key, out skillData);

        if (check)
        {
            return skillData;
        }

        return null;
    }

    void Unload()
    {
        m_skillDicData.Clear();
    }

    public void SkillDataLoad()
    {
        if (GameDataLoadManager.Instance == null)
            return;

        TextAsset dataText = GameDataLoadManager.Instance.SkillTable;
        TableLoader loader = TableLoader.Instance;
        Unload();
        if (GameDataLoadManager.Instance != null)
            loader.LoadTable(dataText.bytes);

        for (int i = 0; i <loader.GetLength(); i++)
        {
            SkillData skillData = new SkillData();

            skillData.SkillId = loader.GetString("SkillId", i);
            skillData.Name = loader.GetString("Name", i);
            skillData.EffectId = loader.GetInt("EffectId", i);
            skillData.Duration = loader.GetInt("Duration", i);
            skillData.CoolTime = loader.GetInt("CoolTime", i);
            skillData.Value = loader.GetInt("Value", i);
            skillData.HitRate = loader.GetInt("HitRate", i);
            skillData.AttackRange = loader.GetFloat("AttackRange", i);
            skillData.SkillType = loader.GetString("SkillType", i);
            skillData.AttackType = loader.GetString("AttackType", i);
            skillData.TargetType = loader.GetString("TargetType", i);

            //Debug.Log("#로그 순서 " + i + "ID : " + skillData.SkillId + " Name : " + skillData.Name + "EffectId  :  " + skillData.EffectId + " Duration : " + skillData.Duration + " CoolTime : " + skillData.CoolTime +
            //                                            "Value : " + skillData.Value + " HitRate : " + skillData.HitRate + " AttackRange : " + skillData.AttackRange + "SkillType : " + skillData.SkillType + "AttackType : " + skillData.AttackType
            //                                            + "TargetType : " + skillData.TargetType);

            if (!string.IsNullOrEmpty(skillData.SkillId) && !skillData.SkillId.ToUpper().Equals("NULL"))
            {
                m_skillDicData.Add(skillData.SkillId, skillData);
            }
        }
         loader.Clear();
    }
}
