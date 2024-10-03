using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaffold : MonoBehaviour
{
    /// <summary>
    /// 움직이고 있을 경우
    /// </summary>
    public bool m_isMove = false;
    /// <summary>
    /// 속도
    /// </summary>
    public float m_speed = 0.0f;

    /// <summary>
    /// 상태 초기화
    /// </summary>
    public void ResetObj()
    {
        m_isMove = false;
        m_speed = 0.0f;
    }
}
