using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TouchManager : DontDestory<TouchManager>
{ 
    [SerializeField]
    GameObject m_touchSfx = null;
    GameObject m_canvas;
    void ShowEffect()
    {
        if (GameObject.Find("Canvas"))
        {
            m_canvas = GameObject.Find("Canvas");
        }
        else if(GameObject.Find("UI Canvas"))
        {
            m_canvas = GameObject.Find("UI Canvas");
        }

        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var obj = Instantiate(m_touchSfx) as GameObject;
        var effect = obj.GetComponent<ParticleSystem>();

        effect.transform.SetParent(m_canvas.transform);

        effect.transform.position = pos;
        effect.Play();
    }

    protected override void OnStart()
    {

    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ShowEffect();
        }
    }

}
