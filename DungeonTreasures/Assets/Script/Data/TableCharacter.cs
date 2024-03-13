using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCharacter : DontDestory<TableCharacter>
{
    public class CharacterData
    {
        public string Id;
        public string Name;
        public int Hp;
        public int MaxHp;
        public int ActSpeed;
        public int Atk;
        public int Def;
        public int CriRate;
        public int HitRate;
        public int DodgeRate;
        public string Type;
    }

    public Dictionary<string, CharacterData> m_characterDicData = new Dictionary<string, CharacterData>();

    public int Count()
    {
        return m_characterDicData.Count;
    }

    public CharacterData GetCharacterData(string key)
    {
        CharacterData characterData;

        bool check = m_characterDicData.TryGetValue(key, out characterData);

        if (check)
        {
            return characterData;
        }

        return null;
    }

    void Unload()
    {
        m_characterDicData.Clear();
    }

    public void CharacterDataLoad()
    {
        if (GameDataLoadManager.Instance == null)
            return;

        TextAsset dataText = GameDataLoadManager.Instance.ChracterTable;
        TableLoader loader = TableLoader.Instance;
        Unload();
        if (GameDataLoadManager.Instance != null)
            loader.LoadTable(dataText.bytes);

        for (int i = 0; i < loader.GetLength(); i++)
        {
            CharacterData characterData = new CharacterData();

            characterData.Id = loader.GetString("Id", i);
            characterData.Name = loader.GetString("Name", i);
            characterData.Hp = loader.GetInt("Hp", i);
            characterData.MaxHp = loader.GetInt("MaxHp", i);
            characterData.Atk = loader.GetInt("Atk", i);
            characterData.Def = loader.GetInt("Def", i);
            characterData.ActSpeed = loader.GetInt("ActSpeed", i);
            characterData.CriRate = loader.GetInt("CriRate", i);
            characterData.HitRate = loader.GetInt("HitRate", i);
            characterData.DodgeRate = loader.GetInt("DodgeRate", i);
            characterData.Type = loader.GetString("Type", i);

            //Debug.Log("#로그 순서 " + i + "ID : " + characterData.Id + " Name : " + characterData.Name + "HP :  " + characterData.Hp+ " Atk : " + characterData.Atk + " Def : " + characterData.Def +
            //                                                "ActSpeed : " + characterData.ActSpeed  + " CriRate : " + characterData.CriRate + " DodgeRate : " + characterData.DodgeRate + "Type : " + characterData.Type);

            if (!string.IsNullOrEmpty(characterData.Id) && !characterData.Id.ToUpper().Equals("NULL"))
            {
                m_characterDicData.Add(characterData.Id, characterData);
            }
        }
        loader.Clear();
    }
}
