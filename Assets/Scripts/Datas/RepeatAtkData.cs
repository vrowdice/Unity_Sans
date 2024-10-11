using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatAtkData
{
    /// <summary>
    /// 공격 데이터
    /// </summary>
    public AtkData m_atkData = new AtkData();
    /// <summary>
    /// 반복 시작 시간
    /// </summary>
    public float m_repeatStartTime = 0.0f;
    /// <summary>
    /// 반복 시간
    /// </summary>
    public float m_repeatTime = 0.0f;
    /// <summary>
    /// 반복 끝나는 시간
    /// </summary>
    public float m_repeatOverTime = 0.0f;
    /// <summary>
    /// 최근에 반복한 시간
    /// </summary>
    public float m_repeatedTime = 0.0f;
}
