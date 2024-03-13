using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : SingleTonMonoBehaviour<EffectPool>
{
    // 풀로 미리 넣어줄 이펙트의 갯수
    int m_presetSize = 4;
    List<string> m_listEffectName = new List<string>();
    //이펙트 이름을 키값으로해서 같은 이름의 이펙트들을 관리 하기 위함
    Dictionary<string, List<EffectPoolUnit>> m_dicEffectPool = new Dictionary<string, List<EffectPoolUnit>>();

    protected override void OnStart()
    {
        base.OnStart();
        LoadEffect();
    }

    void LoadEffect()
    {
       //// 파싱한 테이블 정보 불러오기
       // TableEffect.Instance.EffectDataLoad();

        //프리팹 (이펙트)의 수만큼 넣어주기
        foreach (KeyValuePair<string , TableEffect.Data> pair in TableEffect.Instance.m_dicData)
        {
            for(int i = 0; i < pair.Value.Prefab.Length; i++)
            {
                if(!m_listEffectName.Contains(pair.Value.Prefab[i]))
                {
                    m_listEffectName.Add(pair.Value.Prefab[i]);
                }
            }
        }

        for (int i = 0; i < m_listEffectName.Count; i++)
        {
            //이펙트 리스트생성 
            List<EffectPoolUnit> listObjectPool = new List<EffectPoolUnit>();
            m_dicEffectPool[m_listEffectName[i]] = listObjectPool;
            //이펙트를 실질적으로 불러오기
            GameObject prefab = Resources.Load("Effect/" + m_listEffectName[i]) as GameObject;

            if(prefab != null)
            {
                for(int j = 0; j < m_presetSize; j++)
                {                  
                    var effect = CreateEffectPoolUnit(m_listEffectName[i], prefab);

                    if(effect.gameObject.activeSelf)
                    {
                        effect.gameObject.SetActive(false);
                    }
                    else
                    {
                        AddPoolUnit(m_listEffectName[i], effect);
                    }
                }
            }
        }
    }
    
    EffectPoolUnit CreateEffectPoolUnit(string effectName, GameObject prefab)
    {
        GameObject obj = Instantiate(prefab) as GameObject;
        EffectPoolUnit objectPoolUnit = obj.GetComponent<EffectPoolUnit>();

        if (objectPoolUnit == null)
        {
            objectPoolUnit = obj.AddComponent<EffectPoolUnit>();
        }
        if (obj.GetComponent<AutoParticleDestroy>() == null)
        {
            obj.AddComponent<AutoParticleDestroy>();
        }

        obj.transform.SetParent(transform);
        objectPoolUnit.SetObjectPool(effectName, this);

        return objectPoolUnit;
    }

    public GameObject Create(string effectName)
    {
        return Create(effectName, Vector3.zero, Quaternion.identity);
    }

    public GameObject Create(string effectName, Vector3 position, Quaternion rotation)
    {
        List<EffectPoolUnit> listObjectPool = m_dicEffectPool[effectName];

        if(listObjectPool == null)
        {
            return null;
        }

        if(listObjectPool.Count > 0)
        {
            if(listObjectPool[0] != null && listObjectPool[0].IsReady())
            {
                EffectPoolUnit unit = listObjectPool[0];
                listObjectPool.Remove(listObjectPool[0]);
                unit.transform.position = position;
                unit.transform.rotation = rotation;

                StartCoroutine(Coroutine_SetActive(unit.gameObject));
                return unit.gameObject;
            }
        }

        //풀에 없는 경우에 다시 만들어주는것을 시도한다.
        GameObject prefab = Resources.Load("Prefab/Effect/" + effectName) as GameObject;
        var effect = CreateEffectPoolUnit(effectName, prefab);
        StartCoroutine(Coroutine_SetActive(effect.gameObject));
        return effect.gameObject;
    }

    IEnumerator Coroutine_SetActive(GameObject obj)
    {
        yield return new WaitForEndOfFrame();
        obj.SetActive(true);
    }

    //풀에 넣어주는 함수 
    public void AddPoolUnit(string effectName ,EffectPoolUnit unit )
    {
        List<EffectPoolUnit> listObjectPool = m_dicEffectPool[effectName];

        if(listObjectPool != null)
        {
            listObjectPool.Add(unit);
        }
    }

    public void RemovePoolUnit(string effectName, EffectPoolUnit unit)
    {
        List<EffectPoolUnit> listObjectPool = m_dicEffectPool[effectName];

        if (listObjectPool != null)
        {
            listObjectPool.Remove(unit);
        }
    }
}
