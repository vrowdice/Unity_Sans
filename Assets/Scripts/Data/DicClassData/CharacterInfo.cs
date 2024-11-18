
//딕셔너리에 들어갈 캐릭터 통합 데이터
public class CharacterInfo
{
    public CharacterInfo(CharacterData argData, bool argIsHave)
    {
        m_data = argData;
        m_isHave = argIsHave;
    }

    /// <summary>
    /// 캐릭터 데이터
    /// </summary>
    public CharacterData m_data = null;

    /// <summary>
    /// 이 캐릭터를 가지고 있을 경우 true
    /// </summary>
    public bool m_isHave = false;
}
