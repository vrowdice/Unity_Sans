using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIObjManager : MonoBehaviour
{
    /// <summary>
    /// 회복 버튼
    /// </summary>
    GameObject m_recoverBtn = null;

    /// <summary>
    /// 게임 종료 텍스트 오브젝트
    /// </summary>
    GameObject m_gameOverTextObj = null;
    /// <summary>
    /// 승리 시 표시하는 텍스트 오브젝트
    /// </summary>
    GameObject m_youWinTextObj = null;
    /// <summary>
    /// 회복 가능한 횟수를 표시하는 텍스트
    /// </summary>
    TextMeshPro m_recoverCountTextMeshPro = null;

    private void OnEnable()
    {
        m_gameOverTextObj = transform.Find("GameOverTextObj").gameObject;
        m_youWinTextObj = transform.Find("YouWinTextObj").gameObject;
        m_recoverBtn = transform.Find("RecoverBtn").gameObject;
        m_recoverCountTextMeshPro = m_recoverBtn.transform.Find("CountTextObj").GetComponent<TextMeshPro>();
    }

    /// <summary>
    /// 게임이 끝났을 때
    /// </summary>
    public void GameWinState(bool argIsWin)
    {
        gameObject.SetActive(true);
        m_recoverBtn.SetActive(false);

        if (argIsWin)
        {
            m_gameOverTextObj.SetActive(false);
            m_youWinTextObj.SetActive(true);
        }
        else
        {
            m_gameOverTextObj.SetActive(true);
            m_youWinTextObj.SetActive(false);
        }
    }
    /// <summary>
    /// 이 오브젝트 활성화 관리
    /// </summary>
    public void ActiveUIObj(bool argState)
    {
        if (RoundManager.Instance.GetPhase == 0)
        {
            m_recoverBtn.SetActive(false);
        }
        else
        {
            m_recoverBtn.SetActive(true);
        }
        gameObject.SetActive(argState);

        m_gameOverTextObj.SetActive(false);
        m_youWinTextObj.SetActive(false);
    }
    /// <summary>
    /// 회복 가능 횟수 텍스트 조정
    /// </summary>
    /// <param name="argCount">수</param>
    public void SetRecoverCountTextObj(int argCount)
    {
        m_recoverCountTextMeshPro.text = "RECOVER : " + argCount;
    }
}
