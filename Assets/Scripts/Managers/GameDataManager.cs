using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 돈 아이템 저장, 다음 씬에 데이터 이동 등을 담당합니다
/// 단독으로 실행이 가능해야합니다
/// 라운드 데이터 리스트의 인덱스는 스크립터블 오브젝트 안의 라운드 데이터 인덱스와 일치해야합니다
/// </summary>
public class GameDataManager : MonoBehaviour
{
    /// <summary>
    /// 자기 자신
    /// </summary>
    static GameDataManager g_gameDataManager = null;

    /// <summary>
    /// 라운드 데이터 저장
    /// </summary>
    [SerializeField]
    List<RoundData> m_roundDataList = new List<RoundData>();

    /// <summary>
    /// 현재 라운드 인덱스
    /// </summary>
    private int m_roundIndex = 0;
    /// <summary>
    /// 돈
    /// </summary>
    private long m_money = 0;

    private void Awake()
    {
        if (g_gameDataManager == null)
        {
            g_gameDataManager = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 라운드 데이터 가져가기
    /// </summary>
    /// <param name="argIndex">라운드 인덱스</param>
    /// <returns>라운드 데이터</returns>
    public RoundData GetRoundData(int argIndex)
    {
        return m_roundDataList[argIndex];
    }

    public static GameDataManager Instance
    {
        get { return g_gameDataManager; }
    }
    public List<RoundData> GetRoundDataList
    {
        get { return m_roundDataList; }
    }
    public int GetRoundIndex
    {
        get { return m_roundIndex; }
        set 
        {
            m_roundIndex = value;
            if(m_roundIndex <= 0)
            {
                m_roundIndex = 0;
            }
        }
    }
    public long GetMoney
    {
        get { return m_money; }
        set
        {
            m_money = value;
            if(m_money <= 0)
            {
                m_money = 0;
            }
        }
    }

}
