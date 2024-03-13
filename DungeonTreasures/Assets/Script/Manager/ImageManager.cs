using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageManager : SingleTonMonoBehaviour<ImageManager>
{
    public enum eSkill
    {
        None,
        Image_Skill_01 = 1,
        Image_Skill_02,
        Image_Skill_03,
        Image_Skill_04,
        Image_Skill_05,
        Image_Skill_06,
        Image_Skill_07,
        Image_Skill_08,
        Image_Skill_09,
        Image_Skill_10,
        Image_Skill_11,
        Image_Skill_12,
        Image_Skill_13,
        Image_Skill_14,
        Max
    }

    public enum eCharacter
    {
        None,
        Knight,
        Fighter,
        Archer,
        Magician,
        Creature,
        KingTroll,
        Max
    }

    public enum eBuffIcon
    {
        None = -1,
        ActionIconAtlas_0,
        ActionIconAtlas_1,
        ActionIconAtlas_2,
        ActionIconAtlas_3,
        ActionIconAtlas_4,
        ActionIconAtlas_5,
        ActionIconAtlas_6,
        ActionIconAtlas_7,
        ActionIconAtlas_8,
        ActionIconAtlas_9,
        ActionIconAtlas_10,
        ActionIconAtlas_11,
        Max
    }

    Dictionary<eSkill, Sprite> m_skillDic = new Dictionary<eSkill, Sprite>();
    Dictionary<eCharacter, Sprite> m_characterImageDic = new Dictionary<eCharacter, Sprite>();
    Dictionary<eBuffIcon, Sprite> m_buffIconDic = new Dictionary<eBuffIcon, Sprite>();

    public Sprite GetSkillImage(eSkill skillName)
    {
        Sprite skillImage = null;

        if(m_skillDic.TryGetValue(skillName, out skillImage))
        {
            return skillImage;
        }

        return skillImage;
    }

    public Sprite GetCharacterImage(eCharacter characterName)
    {
        Sprite characterImage = null;

        if (m_characterImageDic.TryGetValue(characterName, out characterImage))
        {
            return characterImage;
        }

        return characterImage;
    }

    public Sprite GetBuffImage(eBuffIcon buffIcon)
    {
        Sprite buff = null;

       if(m_buffIconDic.TryGetValue(buffIcon, out buff))
        {
            return buff;
        }
        return buff;
    }


    void LoadSkillImage()
    {
        var path = string.Empty;

        // 스킬이미지
        for (int i = 1; i < (int)eSkill.Max; i++)
        {
            path = string.Format("Skill/Image_Skill_{0:00}", i);
            var image = Resources.Load<Sprite>(path);
            m_skillDic.Add((eSkill)i, image);
        }

        // 캐릭터 이미지
        for (int i = 0; i < (int)eCharacter.Max; i++)
        {
            path = string.Format("Image/Characters/{0}", (eCharacter)i);
            var image = Resources.Load<Sprite>(path);
            m_characterImageDic.Add((eCharacter)i, image);
        }

        // 버프 이미지
        //path = string.Format("BuffIcon/ActionIconAtlas");
        //Sprite[] images = Resources.LoadAll<Sprite>(path);
        //for (int j = 0; j < (int)eBuffIcon.Max; j++)
        //{
        //    m_buffIconDic.Add((eBuffIcon)j, images[j]);
        //}
    }

    protected override void OnAwake()
    {
        LoadSkillImage();
    }

}
