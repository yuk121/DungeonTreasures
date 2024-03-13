using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableItem : DontDestory<TableItem>
{
    public class ItemData
    {
        public string Id;
        public string Name;
        public int Atk;
        public int Def;
        public int Price;
    }

    public Dictionary<string, ItemData> m_itemDicData = new Dictionary<string, ItemData>();

    public ItemData GetCharacterData(string key)
    {
        ItemData itemData;

        bool check = m_itemDicData.TryGetValue(key, out itemData);

        if (check)
        {
            return itemData;
        }

        return null;
    }

    void Unload()
    {
        m_itemDicData.Clear();
    }

    public void ItemDataLoad()
    {
        if (GameDataLoadManager.Instance == null)
            return;

        TextAsset dataText = GameDataLoadManager.Instance.ItemTable;
        TableLoader loader = TableLoader.Instance;
        Unload();
        if (GameDataLoadManager.Instance != null)
            loader.LoadTable(dataText.bytes);

        for (int i = 0; i <loader.GetLength() -1; i++)
        {
            ItemData itemData = new ItemData();
            itemData.Id = loader.GetString("Id", i);
            itemData.Name = loader.GetString("Name", i);
            itemData.Atk = loader.GetInt("Atk", i);
            itemData.Def = loader.GetInt("Def", i);
            itemData.Price = loader.GetInt("Price", i);

            //Debug.Log("#로그 순서 " + i +"ID : " +itemData.Id + " Name : " + itemData.Name + " Atk : " + itemData.Atk + " Def : " + itemData.Def + " Price : " + itemData.Price);

            if (!string.IsNullOrEmpty(itemData.Id) && !itemData.Id.ToUpper().Equals("NULL"))
            {
                m_itemDicData.Add(itemData.Id, itemData);
            }
        }
        loader.Clear();
    }

}
