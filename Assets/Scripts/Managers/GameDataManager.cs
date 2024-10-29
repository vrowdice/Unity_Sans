using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 돈 아이템 저장, 다음 씬에 데이터 이동 등을 담당합니다
/// 단독으로 실행이 가능해야합니다
/// 라운드 데이터 리스트의 인덱스는 스크립터블 오브젝트 안의 라운드 데이터 인덱스와 일치해야합니다
/// 
/// 부가 기능
/// 
/// 경고 패널 생성 기능
/// 사운드 관리 기능
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
    /// 옵션 매니저 프리펩
    /// </summary>
    [SerializeField]
    GameObject m_optionManagerPrefeb = null;
    /// <summary>
    /// 경고 오브젝트 프리펩
    /// </summary>
    [SerializeField]
    GameObject m_alertObjPrefeb = null;
    
    /// <summary>
    /// 사운드 매니저
    /// </summary>
    private SoundManager m_soundManager = null;
    /// <summary>
    /// 옵션 매니저
    /// </summary>
    private OptionManager m_optionManager = null;
    /// <summary>
    /// 현재 라운드 인덱스
    /// </summary>
    private int m_roundIndex = 0;
    /// <summary>
    /// 돈
    /// </summary>
    private long m_money = 0;
    /// <summary>
    /// 현재 씬 캔버스의 트렌스폼
    /// </summary>
    private Transform m_canvasTrans = null;

    private void OnEnable()
    {
        OnEnableSetting();
    }

    private void OnEnableSetting()
    {
        if (g_gameDataManager == null)
        {
            g_gameDataManager = this;
            SceneManager.sceneLoaded -= SceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneLoaded;

        m_soundManager = GetComponent<SoundManager>();
        CursorState(true);
    }

    /// <summary>
    /// 씬이 로드 되었을 때
    /// gamedatamanager 단독 사용 권장
    /// </summary>
    /// <param name="scene">씬</param>
    /// <param name="mode">모드</param>
    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        m_canvasTrans = GameObject.Find("Canvas").transform;
        if(m_canvasTrans != null)
        {
            m_optionManager = Instantiate(m_optionManagerPrefeb, m_canvasTrans).GetComponent<OptionManager>();
            m_optionManager.OptionState(false);
        }
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

    /// <summary>
    /// 메인 게임 씬으로 이동
    /// </summary>
    public void GoMainScene(int argRoundIndex)
    {
        m_soundManager.ResetAudioClip();

        m_roundIndex = argRoundIndex;
        SceneManager.LoadScene("Main");
    }
    /// <summary>
    /// 타이틀 씬으로 이동
    /// </summary>
    public void GoTitleScene()
    {
        m_soundManager.ResetAudioClip();

        SceneManager.LoadScene("Title");
        CursorState(true);
    }
    /// <summary>
    /// 경고 패널 생성
    /// </summary>
    public void Alert(string argAlertStr)
    {
        if(m_canvasTrans != null)
        {
            Instantiate(m_alertObjPrefeb, m_canvasTrans).GetComponent<AlertPanel>().Alert(argAlertStr);
        }
    }

    /// <summary>
    /// 커서 상태 활성화 비활성화
    /// </summary>
    /// <param name="argActive">여부</param>
    public void CursorState(bool argActive)
    {
        if(SceneManager.GetActiveScene().name != "Main")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        if (argActive == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public static GameDataManager Instance
    {
        get { return g_gameDataManager; }
    }
    public SoundManager GetSoundManager
    {
        get { return m_soundManager; }
    }
    public OptionManager GetOptionManager
    {
        get { return m_optionManager; }
    }
    public List<RoundData> GetRoundDataList
    {
        get { return m_roundDataList; }
    }

    public int SetRoundIndex
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
    public long SetMoney
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
