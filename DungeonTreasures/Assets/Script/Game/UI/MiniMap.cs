using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField]
    Transform m_targetPlayer = null;
    // Start is called before the first frame update

    bool m_isOn = false;
    public void OnMinimap()
    {
        m_isOn = true;
        this.enabled = true;
    }

    public void OffMiniMap()
    {
        m_isOn = false;
        this.enabled = false;
    }

    private void Awake()
    {
         this.enabled = false;
    }

    private void Start()
    {    
        m_targetPlayer = GameObject.Find("MoveCharacter").transform.GetChild(0).gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_isOn)
            transform.position = new Vector3(m_targetPlayer.position.x, 40f, m_targetPlayer.position.z);
    }
}
