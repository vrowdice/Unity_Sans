using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICheckSensor : MonoBehaviour
{
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
            other.gameObject.GetComponent<MeshRenderer>().material = m_playerController.GetActiveInteractMat;
            m_playerController.GetInteractBtnObj = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "InteractBtn")
        {
            other.gameObject.GetComponent<MeshRenderer>().material = m_playerController.GetStandardInteractMat;
            m_playerController.GetInteractBtnObj = null;
        }
    }
}
