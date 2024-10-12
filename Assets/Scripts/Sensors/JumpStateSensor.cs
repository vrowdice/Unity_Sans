using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpStateSensor : MonoBehaviour
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
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Terrain")
        {
            m_playerController.GetIsCanJumpFlag = true;
            m_playerController.GetIsGroundFlag = true;
            m_playerController.GetGroundScaffold = null;
        }
        else if (other.gameObject.tag == "Scaffold")
        {
            m_playerController.GetIsCanJumpFlag = true;
            m_playerController.GetIsGroundFlag = true;
            m_playerController.GetGroundScaffold = other.GetComponent<Scaffold>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Terrain" || other.gameObject.tag == "Scaffold")
        {
            m_playerController.GetIsGroundFlag = false;
            m_playerController.GetGroundScaffold = null;
        }
    }
}
