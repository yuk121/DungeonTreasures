using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CameraMove : MonoBehaviour
{
    [SerializeField]
    GameObject m_target = null;

    [Range(0f, 20f)]
    public float m_hegiht;
    [Range(0f, 30f)]
    public float m_distance;
    [Range(0.1f, 5f)]
    public float m_speed = 0.1f;  //-> 초기값이 없다면 작동이 안될 수 도 있다.z
    [Range(0f, 90f)]
    public float m_angle;

    Vector3 m_prevPos;
    MapManager m_mapManager;
    bool m_canMove = false;
    
    public void SetCameraMove()
    {
        var cameraPath = gameObject.GetComponent<CameraPath>();
        cameraPath.m_fistPathDone = false;
        cameraPath.enabled = false;
        m_canMove = true;
    }

    public void SetCameraMoveFalse()
    {
        m_canMove = false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        m_prevPos = transform.position;
        this.enabled = false;
    }

    private void Start()
    {
        m_target = GameObject.Find("MoveCharacter").transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_canMove)
        {
            //업데이트 내에서 계속 캐릭터의 위치를 따라가게 해줘야한다.
            transform.position = new Vector3(m_target.transform.position.x,
            Mathf.Lerp(m_prevPos.y, m_target.transform.position.y + m_hegiht, m_speed * Time.deltaTime),
            Mathf.Lerp(m_prevPos.z, m_target.transform.position.z - m_distance, m_speed * Time.deltaTime));
            transform.localRotation = Quaternion.Euler(m_angle, 0f, 0f);
        }
    }
    private void LateUpdate()
    {
        if (m_canMove)
        {
            m_prevPos = transform.position;
        }
    }
}
