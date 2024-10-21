using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Asset", menuName = "New SoundData")]
public class SoundData : ScriptableObject
{
    /// <summary>
    /// 메인 테마 송
    /// </summary>
    public AudioClip m_backGround = null;
    /// <summary>
    /// 공격 받을 시
    /// </summary>
    public AudioClip m_hit = null;
    /// <summary>
    /// 체력 회복 시
    /// </summary>
    public AudioClip m_heal = null;
    /// <summary>
    /// 땅에 부딛혔을 시
    /// </summary>
    public AudioClip m_hitGround = null;
    /// <summary>
    /// 중력 공격 효과음
    /// </summary>
    public AudioClip m_gravityAtk = null;
    /// <summary>
    /// 튀어오름 공격 효과음
    /// </summary>
    public AudioClip m_popAtk = null;
    /// <summary>
    /// 원거리 공격 효과음
    /// </summary>
    public AudioClip m_rangeAtk = null;
}
