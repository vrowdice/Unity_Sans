using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAtk : MonoBehaviour
{
    /// <summary>
    /// 직접 공격하는 오브젝트
    /// </summary>
    [SerializeField]
    private GameObject m_atkObj = null;
    /// <summary>
    /// 경고 시간
    /// </summary>
    private float m_warnTime = 0.0f;
    /// <summary>
    /// 공격 시간
    /// </summary>
    private float m_atkTime = 0.0f;

    /// <summary>
    /// 원거리 공격 시작
    /// </summary>
    /// <param name="argWarnTime">경고 시간</param>
    /// <param name="argAtkTime"></param>
    public void StartRangeAtk(float argWarnTime, float argAtkTime)
    {
        m_warnTime = argWarnTime;
        m_atkTime = argAtkTime;

        StartCoroutine(IEStartRangeAtk());
    }
    /// <summary>
    /// 원거리 공격 시작 IE
    /// </summary>
    /// <returns>IE</returns>
    IEnumerator IEStartRangeAtk()
    {
        m_atkObj.SetActive(true);
        WarnAtk();
        yield return new WaitForSeconds(m_warnTime);
        StartAtk();
        yield return new WaitForSeconds(m_atkTime);
        ResetObj();
    }

    /// <summary>
    /// 공격 경고
    /// </summary>
    void WarnAtk()
    {
        m_atkObj.GetComponent<MeshRenderer>().material = GameManager.Instance.GetWarnMat;
        m_atkObj.tag = "Untagged";
    }
    /// <summary>
    /// 직접 공격 시작
    /// </summary>
    void StartAtk()
    {
        m_atkObj.GetComponent<MeshRenderer>().material = GameManager.Instance.GetAtkObjMat;
        m_atkObj.tag = "Attack";
    }

    /// <summary>
    /// 상태 초기화
    /// </summary>
    public void ResetObj()
    {
        GameManager.Instance.WaitRangeAtk(this);

        m_atkObj.tag = "Untagged";
        m_warnTime = 0.0f;
        m_atkTime = 0.0f;
        m_atkObj.SetActive(false);
    }
}
