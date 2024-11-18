
//딕셔너리에 들어갈 라운드 통합 데이터
public class RoundInfo
{
    public RoundInfo(RoundData argData, bool argIsClear, bool argIsHardcoreClear)
    {
        m_data = argData;
        m_isClear = argIsClear;
        m_isHardcoreClear = argIsHardcoreClear;
    }

    /// <summary>
    /// 라운드 데이터
    /// </summary>
    public RoundData m_data = null;

    /// <summary>
    /// 이 라운드를 클리어 한 경우
    /// </summary>
    public bool m_isClear = false;

    /// <summary>
    /// 이 라운드의 하드코어 버전을 클리어 한 경우
    /// </summary>
    public bool m_isHardcoreClear = false;
}
