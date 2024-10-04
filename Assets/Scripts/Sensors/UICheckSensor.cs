using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICheckSensor : MonoBehaviour
{
    /// <summary>
    /// 기본 메테리얼
    /// 상호작용 되었을 시 메테리얼
    /// </summary>
    [SerializeField]
    Material m_standardInteractMat = null;
    [SerializeField]
    Material m_activeInteractMat = null;

    /// <summary>
    /// 플레이어 컨트롤러 인스턴스
    /// </summary>
    private PlayerController m_playerController = null;

    // Start is called before the first frame update
    void Start()
    {
        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "InteractBtn")
        {
            other.gameObject.GetComponent<MeshRenderer>().material = m_activeInteractMat;
            m_playerController.SetInteractBtnObj = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "InteractBtn")
        {
            other.gameObject.GetComponent<MeshRenderer>().material = m_standardInteractMat;
            m_playerController.SetInteractBtnObj = null;
        }
    }
}
