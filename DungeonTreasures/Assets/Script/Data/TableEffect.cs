using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableEffect : Singleton<TableEffect>
{
    const int PREFAP_COUNT = 4;
    public class Data
    {
        public string Id;
        public string Dummy;
        public string[] Prefab = new string[PREFAP_COUNT];
    }
    //여기서 string == id
    public Dictionary<string, Data> m_dicData = new Dictionary<string, Data>();

    public int Count()
    {
        return m_dicData.Count;
    }

    public Data GetData(string key)
    {
        Data data;
        bool ret = m_dicData.TryGetValue(key, out data); // 데이터 존재 여부 확인

        if (ret)
        {
            return data;
        }
        return null;
    }

    public void EffectDataLoad()
    {
        if (GameDataLoadManager.Instance == null)
            return;

        TextAsset dataText = GameDataLoadManager.Instance.EffectTable;
        TableLoader loader = TableLoader.Instance;
        Unload();
        if (GameDataLoadManager.Instance != null)
            loader.LoadTable(dataText.bytes);
 
        if(GameDataLoadManager.Instance !=null)
         loader.LoadTable(GameDataLoadManager.Instance.EffectTable.bytes);

        for (int i = 0; i < loader.GetLength(); i++)
        {
            Data data = new Data();
            data.Id = loader.GetString("Id", i);
            data.Dummy = loader.GetString("Dummy",i);
           
            for(int j = 0; j< data.Prefab.Length; j++)
            {
                data.Prefab[j] = loader.GetString("Prefab_" + (j + 1), i);
            }

            if (!string.IsNullOrEmpty(data.Id) && !data.Id.ToUpper().Equals("NULL"))
            {
                m_dicData.Add(data.Id, data);
            }
        }
        loader.Clear();
    }

    public void Unload()
    {
        m_dicData.Clear();
    }
}
