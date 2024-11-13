using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectBtn : MonoBehaviour
{
    /// <summary>
    /// 체크 이미지
    /// </summary>
    [SerializeField]
    GameObject m_checkImage = null;

    /// <summary>
    /// 상점 관리 매니저
    /// </summary>
    private StoreManager m_storManager = null;

    /// <summary>
    /// 이 버튼이 작동하는 인덱스
    /// </summary>
    private int m_index = -1;

    /// <summary>
    /// 버튼 리셋
    /// </summary>
    /// <param name="argIndex">캐릭터 인덱스</param>
    /// <param name="argCheckImageActive">체크 표시 여부</param>
    /// <param name="argStorManager">스토어 매니저 인스턴스</param>
    public void ResetBtn(int argIndex, bool argCheckImageActive, StoreManager argStorManager)
    {
        m_index = argIndex;
        m_checkImage.SetActive(argCheckImageActive);
        m_storManager = argStorManager;
    }
    /// <summary>
    /// 체크 이미지 활성화
    /// </summary>
    /// <param name="argCheckImageActive">활성화 여부</param>
    public void CheckImageActive(bool argCheckImageActive)
    {
        m_checkImage.SetActive(argCheckImageActive);
    }

    /// <summary>
    /// 클릭 시
    /// </summary>
    public void Click()
    {
        if(m_storManager == null)
        {
            return;
        }

        m_storManager.SelectCharacter(m_index);
    }
}
