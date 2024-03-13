using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    class CharacterStatInfo
    {
        public int m_id;
        public int m_hp;
        public int m_maxHp;
        public float m_atk;
        public float m_def;
        public int m_actSpeed;
        public float m_criRate;
        public float m_hitRate;
        public float m_dodgeRate;
        public string m_name;
        public string m_type;

        public CharacterStatInfo(int id, int hp, float atk, float def, int actSpeed, float criRate, float hitRate, float dodgeRate, string name, string type)
        {
            m_id = id;
            m_hp = m_maxHp = hp;
            m_atk = atk;
            m_def = def;
            m_actSpeed = actSpeed;
            m_criRate = criRate;
            m_hitRate = hitRate;
            m_dodgeRate = dodgeRate;
            m_name = name;
            m_type = type;
        }
    }

    class CharacterSkillIInfo
    {
        public string m_name;
        public int m_skillId;
        public int m_effectId;
        public float m_value; // 공격은 공격수치, 버프는 버프 받게될 수치
        public float m_hitRate;
        public int m_duration;
        public float m_attackRange;
        public int m_coolTime;
        public string m_skillType;
        public string m_attackType;
        public string m_targetType;

        public CharacterSkillIInfo(string name, int skillId, int effectId, float value, float hitRate, int duration, float attackRange, int coolTime, string skillType, string attackType, string targetType)
        {
            m_name = name;
            m_skillId = skillId;
            m_effectId = effectId;
            m_value = value;
            m_hitRate = hitRate;
            m_duration = duration;
            m_attackRange = attackRange;
            m_coolTime = coolTime;
            m_skillType = skillType;
            m_attackType = attackType;
            m_targetType = targetType;        
        }
    }

    [SerializeField]
    int m_inputId = 0;

    CharacterStatInfo m_characterInfo;
    CharacterSkillIInfo m_characterSkillInfo;

    List<Sprite> m_skilImgList = new List<Sprite>();
    Sprite m_characterImg;
    List<CharacterSkillIInfo> m_skillInfoList = new List<CharacterSkillIInfo>();

    public int GetCharacterId()
    {
        return m_characterInfo.m_id;
    }

    public int GetCharacterSpeed()
    {
        return m_characterInfo.m_actSpeed;
    }

    public int GetSkillId(int index)
    {
        return m_skillInfoList[index].m_skillId;
    }
    
    public string GetSkillName(int index)
    {
        return m_skillInfoList[index].m_name;
    }
    public string GetCharacterType()
    {
        return m_characterInfo.m_type;
    }

    public string GetCharacterName()
    {
        return m_characterInfo.m_name;
    }

    public int GetSkillDuration(int index)
    {
        return m_skillInfoList[index].m_duration;
    }

    public Sprite GetCharacterSkillImg(int index)
    {
        return m_skilImgList[index];
    }

    public Sprite GetCharacterImage()
    {
        return m_characterImg;
    }
    public float GetAttackRange(int index)
    {
        return m_skillInfoList[index].m_attackRange;
    }

    public string GetEffectId(int index)
    {
        return (m_skillInfoList[index].m_effectId).ToString();
    }
    public string GetSkillType (int index)
    {
        return m_skillInfoList[index].m_skillType;
    }

    public string GetAttackType(int index)
    {
        return m_skillInfoList[index].m_attackType;
    }
    public string GetTargetType(int index)
    {
        return m_skillInfoList[index].m_targetType;
    }

    public int GetSkillCount()
    {
        return m_skillInfoList.Count;
    }

    public int GetHp()
    {
        return m_characterInfo.m_hp;
    }

    public int GetMaxHp()
    {
        return m_characterInfo.m_maxHp;
    }

    public void SetHp(int value)
    {
        m_characterInfo.m_hp = value;
    }

    public void DecreaseHp(int value)
    {
        m_characterInfo.m_hp = m_characterInfo.m_hp- value; 
    }

    public void IncreasePercentHp(int value)
    {
        if (m_characterInfo.m_hp <= 0)
            return;

        if(m_characterInfo.m_hp + (m_characterInfo.m_maxHp / value)> m_characterInfo.m_maxHp)
        {
            m_characterInfo.m_hp = m_characterInfo.m_maxHp;
        }
        else
        {
            m_characterInfo.m_hp = m_characterInfo.m_hp + (m_characterInfo.m_maxHp / value);
        }
    }

    public float GetHitRate()
    {
        return m_characterInfo.m_hitRate;
    }

    public float GetSkillHitRate(int index)
    {
        return m_skillInfoList[index].m_hitRate;
    }

    public float GetDodgeRate()
    {
        return m_characterInfo.m_dodgeRate;
    }

    public float GetAtk()
    {
        return m_characterInfo.m_atk;
    }

    public void IncreasePercentAtk(float value)
    {
        m_characterInfo.m_atk = m_characterInfo.m_atk + (m_characterInfo.m_atk / value);
    }

    public float GetDef()
    {
        return m_characterInfo.m_def;
    }
    public void IncreaseDef(float value)
    {
        m_characterInfo.m_def = m_characterInfo.m_def + value; 
    }

    public void IncreasePercentDef(float value)
    {
        m_characterInfo.m_def = m_characterInfo.m_def + (m_characterInfo.m_def / value);
    }

    public void DecreaseDef(float value)
    {
        m_characterInfo.m_def = m_characterInfo.m_def - value;
    }

    public void SetDef(float value)
    {
        m_characterInfo.m_def = value;
    }

    public float GetSkillValue(int index)
    {
        return m_skillInfoList[index].m_value;
    }

    public float GetCriRate()
    {
        return m_characterInfo.m_criRate;
    }

    public int GetSkillCoolTime(int index)
    {
        return m_skillInfoList[index].m_coolTime;
    }

    void InitStatus(int id)
    {
        if (TableCharacter.Instance != null)
        {
            var dicData = TableCharacter.Instance.GetCharacterData(id.ToString());
            m_characterInfo = new CharacterStatInfo(int.Parse(dicData.Id), dicData.Hp, dicData.Atk, dicData.Def, dicData.ActSpeed, dicData.CriRate, dicData.HitRate, dicData.DodgeRate,
                dicData.Name, dicData.Type);
        }
    }

    CharacterSkillIInfo  InitSkillInfo(int id)
    {
        if (TableSkill.Instance != null)
        {
            var dicSkillData = TableSkill.Instance.GetSkillData(id.ToString());

            m_characterSkillInfo = new CharacterSkillIInfo(dicSkillData.Name, int.Parse(dicSkillData.SkillId), dicSkillData.EffectId, dicSkillData.Value, dicSkillData.HitRate, dicSkillData.Duration,
                                                                                    dicSkillData.AttackRange, dicSkillData.CoolTime, dicSkillData.SkillType, dicSkillData.AttackType, dicSkillData.TargetType);

            //Debug.Log(string.Format("#로그 스킬 : {0}  , ID : {1},  이펙트ID : {2} , Value : {3} , HitRate : {4} , Duration : {5} , AttackRange : {6} , CoolTime : {7} , 스킬타입 : {8} , 공격타입 : {9} , 타겟타입 : {10}\n"
            //                                                     , dicSkillData.Name, dicSkillData.SkillId, dicSkillData.EffectId, dicSkillData.Value, dicSkillData.HitRate, dicSkillData.Duration, dicSkillData.AttackRange,
            //                                                       dicSkillData.CoolTime, dicSkillData.SkillType, dicSkillData.AttackType, dicSkillData.TargetType));
            return m_characterSkillInfo;
        }
        return null;
    }

    //캐릭터 이미지 가져오기
    void ApplyCharacterImage(int id)
    {
        switch (id)
        {
            case 1:
                m_characterImg = (ImageManager.Instance.GetCharacterImage(ImageManager.eCharacter.Knight));
                break;
            case 2:
                m_characterImg = (ImageManager.Instance.GetCharacterImage(ImageManager.eCharacter.Fighter));
                break;
            case 3:
                m_characterImg = (ImageManager.Instance.GetCharacterImage(ImageManager.eCharacter.Archer));
                break;
            case 4:
                m_characterImg = (ImageManager.Instance.GetCharacterImage(ImageManager.eCharacter.Magician));
                break;
            case 1001:
                m_characterImg = (ImageManager.Instance.GetCharacterImage(ImageManager.eCharacter.Creature));
                break;
            case 1002:
                m_characterImg = (ImageManager.Instance.GetCharacterImage(ImageManager.eCharacter.KingTroll));
                break;
        }
    }

    // 스킬 적용
    void ApplyCharacterSkill(int id)
    {
        switch(id)
        {
            case 1:  // Knight
                // 이미지 적용
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_01));
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_02));
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_03));

                // 스킬 정보 적용
                m_skillInfoList.Add(InitSkillInfo(1));
                m_skillInfoList.Add(InitSkillInfo(2));
                m_skillInfoList.Add(InitSkillInfo(3));

                break;
            case 2: // Fighter
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_04));
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_05));
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_06));

                m_skillInfoList.Add(InitSkillInfo(4));
                m_skillInfoList.Add(InitSkillInfo(5));
                m_skillInfoList.Add(InitSkillInfo(6));
                break;
            case 3: // Archer
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_07));
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_08));
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_09));

                m_skillInfoList.Add(InitSkillInfo(7));
                m_skillInfoList.Add(InitSkillInfo(8));
                m_skillInfoList.Add(InitSkillInfo(9));
                break;
            case 4: // Magician
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_10));
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_11));
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_12));

                m_skillInfoList.Add(InitSkillInfo(10));
                m_skillInfoList.Add(InitSkillInfo(11));
                m_skillInfoList.Add(InitSkillInfo(12));
                break;
            case 1001:  // Mutant
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_13));
                m_skilImgList.Add(ImageManager.Instance.GetSkillImage(ImageManager.eSkill.Image_Skill_14));

                m_skillInfoList.Add(InitSkillInfo(13));
                m_skillInfoList.Add(InitSkillInfo(14));
                break;
            case 1002:  // BossKingTroll
                m_skillInfoList.Add(InitSkillInfo(15));
                m_skillInfoList.Add(InitSkillInfo(16));
                break;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        InitStatus(m_inputId);
        ApplyCharacterSkill(m_inputId);
        ApplyCharacterImage(m_inputId);
    }

    #region PlaySFX

    public void aEvent_SFX_Walk()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Walk);
        }
    }

    public void aEvent_SFX_Walk2()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Walk2);
        }
    }

    public void aEvent_SFX_Walk3()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Walk3);
        }
    }
    #endregion

}
