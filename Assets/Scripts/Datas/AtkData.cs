using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공격 데이터
/// </summary>
public class AtkData
{
    //스폰 순서
    public int m_order = -1;
    //공격 타입
    public AtkType m_type = new AtkType();
    //움직이는지 안움직이는지
    //데이터에서는 0과 1로 표현
    //0 = false
    //1 = true
    public bool m_isMove = false;
    //움직이는 속도
    public float m_speed = 0.0f;
    //스폰 시간
    public float m_genTime = 0.0f;

    //공격 오브젝트 사이즈
    //스폰 위치, 스폰 방향
    //움직이는 오브젝트는 정면으로 이동함
    //공격 방향도 로테이션에 따라 달라짐
    public Vector3 m_size = new Vector3();
    public Vector3 m_position = new Vector3();
    public Vector3 m_rotation = new Vector3();
}
