using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 점프 힘
    /// </summary>
    [SerializeField]
    float m_jumpForce = 50.0f;

    /// <summary>
    /// 직선 움직임 속도
    /// </summary>
    [SerializeField]
    float m_moveSpeed = 50.0f;

    /// <summary>
    /// 대각선 움직임 속도
    /// </summary>
    [SerializeField]
    float m_diagonalMoveSpeed = 0.5f;

    /// <summary>
    /// 마우스 감도
    /// </summary>
    [SerializeField]
    float m_xSensitivity = 1.0f;
    [SerializeField]
    float m_ySensitivity = 1.0f;

    /// <summary>
    /// 카메라
    /// </summary>
    [SerializeField]
    public Transform m_cameraBase;

    /// <summary>
    /// 리지드바디
    /// </summary>
    Rigidbody m_rb;

    /// <summary>
    /// 키 입력 확인을 위한 불린
    /// </summary>
    bool InputKey_W = false;
    bool InputKey_S = false;
    bool InputKey_A = false;
    bool InputKey_D = false;

    /// <summary>
    /// 점프 플래그
    /// </summary>
    bool m_jumpFlag = true;

    /// <summary>
    /// 땅에 닿았을 때
    /// </summary>
    /// <param name="col">콜리더</param>
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Terrain")
        {
            m_jumpFlag = true;
        }
    }

    /// <summary>
    /// 땅에서 떨어지는 순간
    /// </summary>
    /// <param name="col">콜리더</param>
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Terrain")
        {
            m_jumpFlag = false;
        }
    }

    /// <summary>
    /// start
    /// </summary>
    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// update
    /// </summary>
    private void Update()
    {
        PlayerControll();
        CameraControll();
    }

    /// <summary>
    /// 이동을 위한 키 입력 및 전송
    /// </summary>
    void PlayerControll()
    {

    }

    /// <summary>
    /// 플레이어 카메라 컨트롤
    /// </summary>
    void CameraControll()
    {
        float _yRota = Input.GetAxis("Mouse X") * m_xSensitivity;
        float _xRota = Input.GetAxis("Mouse Y") * m_ySensitivity;

        transform.localRotation *= Quaternion.Euler(0, _yRota, 0);
        m_cameraBase.localRotation *= Quaternion.Euler(-_xRota, 0, 0);
    }
}
