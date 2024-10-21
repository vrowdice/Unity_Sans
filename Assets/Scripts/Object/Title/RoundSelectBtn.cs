using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSelectBtn : MonoBehaviour
{
    /// <summary>
    /// 라운드 스프라이트 랜더러
    /// </summary>
    [SerializeField]
    SpriteRenderer m_roundSpriteRenderer = null;

    /// <summary>
    /// 타이틀 매니저
    /// </summary>
    private TitleManager m_titleManager = null;
    /// <summary>
    /// 자신의 라운드 인덱스
    /// </summary>
    private int m_roundIndex = 0;

    public void SetRoundSelectBtn(TitleManager argTitleManager, Sprite argRoundSprite, int argRoundIndex)
    {
        m_titleManager = argTitleManager;
        m_roundSpriteRenderer.sprite = argRoundSprite;
        m_roundIndex = argRoundIndex;
    }

    public int GetRoundIndex
    {
        get { return m_roundIndex; }
    }
}
