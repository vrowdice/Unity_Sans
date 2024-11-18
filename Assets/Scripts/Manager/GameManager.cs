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
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 자기 자신
    /// </summary>
    static GameManager g_gameDataManager = null;

    /// <summary>
    /// 캐릭터 데이터 리스트
    /// </summary>
    [SerializeField]
    List<CharacterData> m_charactorDataList = new List<CharacterData>();
    /// <summary>
    /// 라운드 데이터 리스트
    /// </summary>
    [SerializeField]
    List<RoundData> m_roundDataList = new List<RoundData>();

    /// <summary>
    /// 옵션 매니저 프리펩
    /// </summary>
    [SerializeField]
    GameObject m_optionManagerPrefeb = null;
    /// <summary>
    /// 돈 확인 패널 프리펩
    /// </summary>
    [SerializeField]
    GameObject m_moneyPanelPrefeb = null;
    /// <summary>
    /// 경고 오브젝트 프리펩
    /// </summary>
    [SerializeField]
    GameObject m_alertObjPrefeb = null;

    /// <summary>
    /// 라운드 딕셔너리
    /// 라운드 데이터 검색 및 라운드 저장 정보 탐색
    /// </summary>
    private Dictionary<int, RoundInfo> m_roundDic = new Dictionary<int, RoundInfo>();
    /// <summary>
    /// 캐릭터 딕셔너리
    /// 캐릭터 데이터 검색 및 라운드 저장 정보 탐색
    /// </summary>
    private Dictionary<int, CharacterInfo> m_characterDic = new Dictionary<int, CharacterInfo>();
    /// <summary>
    /// 라운드 딕셔너리 정렬 리스트
    /// </summary>
    private List<int> m_roundDicSortList = new List<int>();
    /// <summary>
    /// 캐릭터 딕셔너리 정렬 리스트
    /// </summary>
    private List<int> m_characterDicSortList = new List<int>();

    /// <summary>
    /// 사운드 매니저
    /// </summary>
    private SoundManager m_soundManager = null;
    /// <summary>
    /// 옵션 매니저
    /// </summary>
    private OptionManager m_optionManager = null;
    /// <summary>
    /// 돈 표시 패널 관리
    /// </summary>
    private MoneyPanel m_moneyPanel = null;
    /// <summary>
    /// 현재 라운드 인덱스
    /// </summary>
    private int m_roundIndex = 0;
    /// <summary>
    /// 현재 사용중인 캐릭터 인덱스
    /// </summary>
    private int m_characterKey = 40001;
    /// <summary>
    /// 현재 씬 캔버스의 트렌스폼
    /// </summary>
    private Transform m_canvasTrans = null;

    /// <summary>
    /// 돈
    /// 
    /// 저장 필요
    /// </summary>
    private long m_money = 10000;

    private void Awake()
    {
        AwakeSetting();
    }

    private void OnEnable()
    {
        OnEnableSetting();
    }

    private void OnEnableSetting()
    {
        //라운드 정보 딕셔너리 지정
        for(int i = 0; i < m_roundDataList.Count; i++)
        {
            try
            {
                m_roundDic.Add(int.Parse(m_roundDataList[i].name) , new RoundInfo(m_roundDataList[i], false, false));
                m_roundDicSortList.Add(int.Parse(m_roundDataList[i].name));
            }
            catch
            {
                Debug.Log("round data name is not a int " + m_roundDataList[i].name);
            }
        }
        //캐릭터 정보 딕서니리 지정
        for(int i = 0; i < m_charactorDataList.Count; i++)
        {
            try
            {
                m_characterDic.Add(int.Parse(m_charactorDataList[i].name), new CharacterInfo(m_charactorDataList[i], false));
                m_characterDicSortList.Add(int.Parse(m_charactorDataList[i].name));
            }
            catch
            {
                Debug.Log("character data name is not a int " + m_charactorDataList[i].name);
            }
        }

        m_roundDicSortList.Sort();
        m_characterDicSortList.Sort();

        m_characterDic[m_characterKey].m_isHave = true;
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
            //옵션 매니저 생성 및 할당
            m_optionManager = Instantiate(m_optionManagerPrefeb, m_canvasTrans).GetComponent<OptionManager>();
            m_optionManager.OptionState(false);

            //돈 표시 패널 생성 및 초기화
            m_moneyPanel = Instantiate(m_moneyPanelPrefeb, m_canvasTrans).GetComponent<MoneyPanel>();
            Money = m_money;
        }
    }
    /// <summary>
    /// 어웨이크 세팅
    /// </summary>
    void AwakeSetting()
    {
        //싱글톤 세팅
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

        //씬 로드하는 경우
        SceneManager.sceneLoaded += SceneLoaded;
        //사운드 매니저 할당
        m_soundManager = GetComponent<SoundManager>();
        //커서 상태 변경
        ChangeCursorState(true);
    }

    /// <summary>
    /// 라운드 정보 가져오기
    /// </summary>
    /// <param name="argKey">라운드 키</param>
    /// <returns>라운드 정보</returns>
    public RoundInfo GetRoundInfo(int argKey)
    {
        try
        {
            return m_roundDic[argKey];
        }
        catch
        {
            Debug.Log("no have key " + argKey);
            return null;
        }
    }
    /// <summary>
    /// 캐릭터 정보 가져오기
    /// </summary>
    /// <param name="argKey">캐릭터 키</param>
    /// <returns>캐릭터 정보</returns>
    public CharacterInfo GetCharacterInfo(int argKey)
    {
        try
        {
            return m_characterDic[argKey];
        }
        catch
        {
            Debug.Log("no have key " + argKey);
            return null;
        }
    }
    /// <summary>
    /// 캐릭터 데이터 가져오기
    /// </summary>
    /// <param name="argKey">캐릭터 인덱스</param>
    /// <returns>캐릭터 데이터</returns>
    public CharacterData GetCharactorData(int argKey)
    {
        return GetCharacterInfo(argKey).m_data;
    }
    /// <summary>
    /// 라운드 데이터 가져오기
    /// </summary>
    /// <param name="argKey">라운드 인덱스</param>
    /// <returns>라운드 데이터</returns>
    public RoundData GetRoundData(int argKey)
    {
        return GetRoundInfo(argKey).m_data;
    }

    /// <summary>
    /// 이름으로 씬 이동
    /// </summary>
    /// <param name="argStr">이동할 씬의 이름</param>
    public void MoveSceneAsName(string argStr, bool argCursorState)
    {
        m_soundManager.ResetAudioClip();
        SceneManager.LoadScene(argStr);
        ChangeCursorState(argCursorState);
    }
    /// <summary>
    /// 메인 게임 씬으로 이동
    /// </summary>
    public void GoMainScene(int argRoundIndex)
    {
        m_roundIndex = argRoundIndex;
        MoveSceneAsName("Main", false);
    }
    /// <summary>
    /// 타이틀 씬으로 이동
    /// </summary>
    public void GoTitleScene()
    {
        MoveSceneAsName("Title", true);
    }
    /// <summary>
    /// 상점 씬으로 이동
    /// </summary>
    public void GoShopScene()
    {
        MoveSceneAsName("Shop", true);
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
    public void ChangeCursorState(bool argActive)
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

    public static GameManager Instance
    {
        get { return g_gameDataManager; }
    }
    public SoundManager SoundManager
    {
        get { return m_soundManager; }
    }
    public OptionManager OptionManager
    {
        get { return m_optionManager; }
    }
    public Transform CanvasTrans
    {
        get { return m_canvasTrans; }
    }
    public List<int> RoundDicSortList
    {
        get { return m_roundDicSortList; }
    }
    public List<int> CharacterDicSortList
    {
        get { return m_characterDicSortList; }
    }

    public int CharacterCode
    {
        get { return m_characterKey; }
        set
        {
            if(m_characterDic[value].m_isHave == false)
            {
                Debug.Log("no have char");
                return;
            }

            m_characterKey = value;
        }
    }
    public int RoundIndex
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
    public long Money
    {
        get { return m_money; }
        set
        {
            m_money = value;
            if(m_money <= 0)
            {
                m_money = 0;
            }

            if(m_moneyPanel != null)
            {
                m_moneyPanel.SetMoneyText = value.ToString();
            }
        }
    }

}
