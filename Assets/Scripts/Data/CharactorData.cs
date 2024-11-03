using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터 스킨을 관리하는 데이터입니다
[CreateAssetMenu(fileName = "Charactor Data Asset", menuName = "New CharactorData")]
public class CharactorData : ScriptableObject
{
    /// <summary>
    /// 캐릭터 인덱스
    /// </summary>
    public int m_index = 0;

    /// <summary>
    /// 캐릭터 스킨 오브젝트
    /// </summary>
    public GameObject m_object = null;

    /// <summary>
    /// 캐릭터 스킨 가격
    /// </summary>
    public long m_price = 0;
}
