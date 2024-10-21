using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Round Data Asset", menuName = "New RoundData")]
public class RoundData : ScriptableObject
{
    /// <summary>
    /// 라운드 인덱스
    /// Resources 안의 파일 이름과 같은 이름을 공유
    /// </summary>
    public int m_roundIndex = 0;
    /// <summary>
    /// 라운드 이름
    /// </summary>
    public string m_roundName = string.Empty;
    /// <summary>
    /// 라운드 표시 여부
    /// </summary>
    public bool m_roundVisible = true;
    /// <summary>
    /// 라운드 이미지
    /// </summary>
    public Sprite m_roundSprite = null;
    /// <summary>
    /// 패이즈가 시작되는 시간 조정
    /// </summary>
    public List<float> m_phaseStartTimeSet = new List<float>();
    /// <summary>
    /// 페이즈가 끝나는 시간
    /// </summary>
    public List<float> m_phaseOverTime = new List<float>();
    /// <summary>
    /// 라운드에 사용할 사운드 데이터
    /// </summary>
    public SoundData m_soundData = null;
}
