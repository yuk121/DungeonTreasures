using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  싱글턴 스크립트 
//  사용 방법 : 1.싱글턴을 사용하고 싶다면 이 스크립트를 부모로 상속을 받아서 사용하면 된다.
//  사용 목적 : 2.해당 씬에서만 사용하는 싱글턴이다.
//  주의 사항 : 사용시 자식 스크립트는 Start 함수와 Awake 함수 대신 OnStart , OnAwake를 사용해야 한다.
//
public class SingleTonMonoBehaviour<T> : MonoBehaviour where T : SingleTonMonoBehaviour<T>
{
    static public T Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (Instance == (T)this)
            OnStart();
    }

    virtual protected void OnAwake() { }
    virtual protected void OnStart() { }

}
