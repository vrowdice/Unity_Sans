using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터 스킨을 관리하는 데이터입니다
[CreateAssetMenu(fileName = "Character Data Asset", menuName = "New CharacterData")]
public class CharacterData : ScriptableObject
{
    /// <summary>
    /// 캐릭터 인덱스
    /// 게임 매니저 안의 캐릭터 데이터 인덱스와 같아야 함
    /// </summary>
    public int m_index = 0;

    /// <summary>
    /// 캐릭터 이름
    /// </summary>
    public string m_name = string.Empty;

    /// <summary>
    /// 캐릭터 스킨 오브젝트
    /// </summary>
    public GameObject m_object = null;

    /// <summary>
    /// 캐릭터 타입
    /// </summary>
    public CharacterType m_type = new CharacterType();
}
