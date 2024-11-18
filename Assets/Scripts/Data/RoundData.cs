using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//각 라운드를 관리하는 데이터입니다
//페이즈가 시작되는 시간과 끝나는 시간은 페이즈의 갯수에 맞춰서 같은 수로 설정하여야합니다
[CreateAssetMenu(fileName = "Round Data Asset", menuName = "New RoundData")]
public class RoundData : ScriptableObject
{
    /// <summary>
    /// 라운드 이름
    /// </summary>
    public string m_name = string.Empty;
    /// <summary>
    /// 라운드 표시 여부
    /// </summary>
    public bool m_isVisible = true;
    /// <summary>
    /// 라운드 이미지
    /// </summary>
    public Sprite m_sampleSprite = null;
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
