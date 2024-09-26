using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//각 데이터의 스폰 순서는 각 공격 데이터의 처리를 확인하기 위해 필요
//m_type
//BlueSimpleAtk(캐릭터가 움직이면 데미지 받음 하나 생성 Vector.forward 방향으로로 이동),
//SimpleAtk(일반 단순 뼈다귀 공격 받음 공격 하나 생성 Vector.forward 방향으로로 이동),
//PopAtk(지면에 전체 뼈다귀 공격을 진행),
//RangeAtk(생성된 곳에서 정면 일직선으로 공격 forward 방향으로 일직선 공격),
//GravityAtk(한 지면으로 들었다가 충돌시킴 지면은 12시부터 시계방향으로 배열
//180.0f도 0번 지면, -90.0f도 1번 지면, 0.0f도 2번 지면, 90.0f도 3번지면),
//scaffold(발판을 생성함 Vector.forward 방향으로 이동),
//m_isMove
//움직이는 오브젝트일 경우 0 = false, 1 = true
//m_genTime
//페이즈가 시작된 후로부터 지난 시간에 따라 이 gentime으로 물체를 생성함 각각 공격의 gen time을 잘 설정하는 것이 필요
//m_sizeY
//공격 오브젝트 사이즈
//m_position
//스폰 위치 플래이어가 움직일 범위는 40 * 40의 지면임 움직이는 오브젝트는 그 밖에 스폰하는 것이 일반적
//m_rotation
//공격 방향을 설정하며 움직이는 방향을 설정하고 물체의 직접적인 방향을 설정해서 가로의 공격이 생길 수도 있고 세로의 공격이 생길 수도 있음


/// <summary>
/// 적 공격 타입 이넘
/// </summary>
public enum AtkType
{
    BlueSimpleAtk,
    SimpleAtk,
    PopAtk,

    RangeAtk,

    GravityAtk,

    Scaffold,
}

/// <summary>
/// 공격 데이터
/// </summary>
public class AtkData
{
    //스폰 순서
    public int m_order = -1;
    //공격 타입
    public AtkType m_type = new AtkType();
    //움직이는지 안움직이는지
    //데이터에서는 0과 1로 표현
    //0 = false
    //1 = true
    public bool m_isMove = false;
    //스폰 시간
    public float m_genTime = 0.0f;
    //공격 오브젝트 사이즈
    public float m_sizeY = 0.0f;
    //스폰 위치, 스폰 방향
    //움직이는 오브젝트는 정면으로 이동함
    //공격 방향도 로테이션에 따라 달라짐
    public Vector3 m_position = new Vector3();
    public Vector3 m_rotation = new Vector3();
}

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// game manager
    /// </summary>
    static GameManager g_gameManager;

    [Header("Wall")]
    /// <summary>
    /// (지형)벽 중심
    /// </summary>
    [SerializeField]
    Transform m_wallCenterPos = null;
    /// <summary>
    /// (지형)벽 리스트
    /// </summary>
    [SerializeField]
    List<GameObject> m_wallList = new List<GameObject>();
    /// <summary>
    /// 벽 변환 속도
    /// </summary>
    [SerializeField]
    float m_wallDirChangeSpeed = 150.0f;
    /// <summary>
    /// 벽 절반 크기
    /// </summary>
    [SerializeField]
    int m_wallHalfSize = 20;
    /// <summary>
    /// 벽 메테리얼
    /// </summary>
    [SerializeField]
    Material m_wallMat = null;
    /// <summary>
    /// 투명한 벽 메테리얼
    /// </summary>
    [SerializeField]
    Material m_transparentWallMat = null;

    [Header("Atteck")]
    /// <summary>
    /// 튀어오르기 공격 경고 오브젝트
    /// </summary>
    [SerializeField]
    GameObject m_popWarningObj = null;
    /// <summary>
    /// 기본 메테리얼
    /// </summary>
    [SerializeField]
    Material m_atkObjMat = null;
    /// <summary>
    /// 움직이는 플래이어 공격 메테리얼
    /// </summary>
    [SerializeField]
    Material m_movePlayerAtkObjMat = null;
    /// <summary>
    /// 경고 메테리얼
    /// </summary>
    [SerializeField]
    Material m_warningMat = null;

    [Header("Simple Atteck")]
    /// <summary>
    /// 공격 오브젝트 프리펩
    /// </summary>
    [SerializeField]
    GameObject m_simpleAtkObj = null;
    /// <summary>
    /// 최대 공격 오브젝트 수
    /// </summary>
    [SerializeField]
    int m_simpleAtkObjCount = 200;
    /// <summary>
    /// 공격 오브젝트 기본 사이즈
    /// </summary>
    [SerializeField]
    int m_simpleAtkObjBasicSize = 3;


    [Header("Range Atteck")]
    [SerializeField]
    GameObject m_rangeAtkObj = null;
    [SerializeField]
    int m_rangeAtkObjCount = 30;
    [SerializeField]
    int m_rangeAtkObjBasicSize = 3;
    /// <summary>
    /// 원거리 공격 경고 시간
    /// </summary>
    [SerializeField]
    float m_rangeAtkWarnTime = 2.0f;
    /// <summary>
    /// 원거리 공격 공격 시간
    /// </summary>
    [SerializeField]
    float m_rangeAtkTime = 2.0f;

    [Header("Pop Atteck")]
    /// <summary>
    /// 튀어오르는 공격 최대 높이
    /// </summary>
    [SerializeField]
    float m_popAtkMaxHeight = 0.0f;
    /// <summary>
    /// 튀어오르는 돌발 공격 활성화 시간
    /// </summary>
    [SerializeField]
    float m_popAtkActiveTime = 1.5f;
    /// <summary>
    /// 튀어오르는 돌발 공격 속도
    /// </summary>
    [SerializeField]
    float m_popAtkSpeed = 100.0f;
    /// <summary>
    /// 튀어오르는 공격 경고하는 시간
    /// </summary>
    [SerializeField]
    float m_popAtkWarnTime = 1.5f;

    [Header("Gravity Atteck")]
    /// <summary>
    /// 중력 공격 힘(속도)
    /// </summary>
    [SerializeField]
    float m_gravityAtkSpeed = 20.0f;


    /// <summary>
    /// 플레이어 컨트롤러 스크립트
    /// </summary>
    private PlayerController m_playerController = null;
    /// <summary>
    /// 적 공격 데이터 리스트
    /// </summary>
    private List<AtkData> m_atkDataList = new List<AtkData>();
    /// <summary>
    /// 대기 리스트로 돌아가기 위한 임시 저장 리스트
    /// </summary>
    private List<SimpleAtk> m_toWaitTmpList = new List<SimpleAtk>();
    /// <summary>
    /// 플레이어 닿으면 단순 공격하는 오브젝트 대기 큐
    /// </summary>
    private Queue<SimpleAtk> m_simpleAtkObjWaitQueue = new Queue<SimpleAtk>();
    /// <summary>
    /// 원거리 공격 오브젝트 대기 큐
    /// </summary>
    private Queue<RangeAtk> m_rangeAtkObjWaitQueue = new Queue<RangeAtk>();
    /// <summary>
    /// 전장에서 활성화 된 단순 공격 오브젝트
    /// 사용하고 쓸모 없어지면 대기 큐로 이동
    /// </summary>
    private LinkedList<SimpleAtk> m_activeSimpleAtkObjList = new LinkedList<SimpleAtk>();
    /// <summary>
    /// 전장에서 활성화 된 원거리 공격 오브젝트
    /// 사용하고 쓸모 없어지면 대기 큐로 이동
    /// </summary>
    private LinkedList<RangeAtk> m_activeRangeAtkObjQueue = new LinkedList<RangeAtk>();

    /// <summary>
    /// 공격하는 오브젝트가 생성될 위치
    /// </summary>
    private Vector3 m_atkObjBasicPos = new Vector3(300.0f, 0.0f, 0.0f);
    /// <summary>
    /// 현재 플레이어가 사용하는 벽
    /// 12시 부터 시계방향으로 0, 1, 2, 3
    /// </summary>
    private int m_nowWall = 0;
    /// <summary>
    /// 현재 페이즈
    /// </summary>
    private int m_phase = 0;
    /// <summary>
    /// 현재 페이즈의 공격 진행 상황
    /// </summary>
    private int m_atkIndex = 0;
    /// <summary>
    /// 현재 페이즈가 지난 시간
    /// </summary>
    private float m_phaseTime = 0.0f;
    /// <summary>
    /// 현재 페이즈가 지난 시간 계산
    /// </summary>
    private float m_phaseStartTime = 0.0f;
    /// <summary>
    /// 타이밍 측정중인가
    /// </summary>
    private bool m_isTiming = false;
    /// <summary>
    /// 벽 회전 완료 플래그
    /// </summary>
    private bool m_wallRotCompleteFlag = false;
    /// <summary>
    /// 상승 완료 플래그
    /// </summary>
    private bool m_ascensionCompleteFlag = false;

    private void Awake()
    {
        AwakeSetting();
    }
    void Start()
    {
        StartSetting();

        AllWallPopAtk();

        SimpleAtk(new Vector3(20.0f, 0.0f, -10.0f), new Vector3(0.0f, -90.0f, 0.0f), new Vector3(5.0f, 20.0f, 5.0f), 8.0f, false);
        SimpleAtk(new Vector3(20.0f, 0.0f, 0.0f), new Vector3(0.0f, -90.0f, 0.0f), new Vector3(5.0f, 20.0f, 5.0f), 5.0f, true);
        SimpleAtk(new Vector3(20.0f, 0.0f, 10.0f), new Vector3(0.0f, -90.0f, 0.0f), new Vector3(5.0f, 20.0f, 5.0f), 3.0f, false);

        RangeAtk(new Vector3(-15.0f, 2.0f, -30.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(5.0f, 5.0f, 5.0f));
        RangeAtk(new Vector3(-5.0f, 2.0f, -30.0f), new Vector3(0.0f, 0.0f, 0.0f), new Vector3(5.0f, 5.0f, 5.0f));
    }
    void Update()
    {
        SimpleAtkMove();
        TimingCheck();

        if (Input.GetMouseButtonDown(0) && !m_isTiming)
        {
            StartTimer();
        }
        else if (Input.GetMouseButtonDown(0) && m_isTiming)
        {
            Debug.Log("경과 시간: " + m_phaseTime + "초");
            StopTimer();
        }
    }

    /// <summary>
    /// awake 초기 설정
    /// </summary>
    void AwakeSetting()
    {
        if (g_gameManager == null)
        {
            g_gameManager = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);

        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        GetCSVData();
    }
    /// <summary>
    /// start 초기 설정
    /// </summary>
    void StartSetting()
    {
        GameObject _simpleAtkParent = new GameObject("SimpleAtkParent");
        GameObject _rangeAtkParent = new GameObject("RangeAtkParent");
        GameObject _obj = null;

        for (int i = 0; i < m_simpleAtkObjCount; i++)
        {
            _obj = Instantiate(m_simpleAtkObj, _simpleAtkParent.transform);
            _obj.transform.position = m_atkObjBasicPos;
            m_simpleAtkObjWaitQueue.Enqueue(_obj.GetComponent<SimpleAtk>());

        }
        for (int i = 0; i < m_rangeAtkObjCount; i++)
        {
            _obj = Instantiate(m_rangeAtkObj, _rangeAtkParent.transform);
            _obj.transform.position = m_atkObjBasicPos;
            m_rangeAtkObjWaitQueue.Enqueue(_obj.GetComponent<RangeAtk>());
        }

        StartCoroutine(IEChangeWall(0.0f));

        _obj = Instantiate(m_popWarningObj);
        m_popWarningObj = _obj;
        m_popWarningObj.SetActive(false);
    }
    
    /// <summary>
    /// CSV 데이터 가져와서 리스트에 저장
    /// </summary>
    void GetCSVData()
    {
        List<Dictionary<string, object>> _data = CSVReader.Read("Phase" + m_phase);

        for (int i = 0; i < _data.Count; i++)
        {
            AtkData _atkData = new AtkData();

            _atkData.m_order = int.Parse(_data[i]["order"].ToString());
            _atkData.m_type = StrToAtkType(_data[i]["type"].ToString());
            _atkData.m_isMove = StrToBool(_data[i]["isMove"].ToString());
            _atkData.m_genTime = float.Parse(_data[i]["genTime"].ToString());
            _atkData.m_sizeY = float.Parse(_data[i]["sizeY"].ToString());

            _atkData.m_position.x = float.Parse(_data[i]["positionX"].ToString());
            _atkData.m_position.y = float.Parse(_data[i]["positionY"].ToString());
            _atkData.m_position.z = float.Parse(_data[i]["positionZ"].ToString());

            _atkData.m_rotation.x = float.Parse(_data[i]["rotationX"].ToString());
            _atkData.m_rotation.y = float.Parse(_data[i]["rotationY"].ToString());
            _atkData.m_rotation.z = float.Parse(_data[i]["rotationZ"].ToString());

            m_atkDataList.Add(_atkData);
        }
    }
    /// <summary>
    /// 공격 타입 문자열 이넘으로 변경
    /// </summary>
    /// <param name="argStr">공격 타입 문자열</param>
    /// <returns>공격 타입 이넘</returns>
    AtkType StrToAtkType(string argStr)
    {
        switch (argStr)
        {
            case "BlueSimpleAtk":
                return AtkType.BlueSimpleAtk;
            case "SimpleAtk":
                return AtkType.SimpleAtk;
            case "PopAtk":
                return AtkType.PopAtk;
            case "RangeAtk":
                return AtkType.RangeAtk;
            case "GravityAtk":
                return AtkType.GravityAtk;
            case "Scaffold":
                return AtkType.Scaffold;
            default:
                Debug.Log(argStr + " not allowed value");
                return new AtkType();
        }
    }
    /// <summary>
    /// 데이터의 0, 1 문자열을 bool형식으로 변환
    /// </summary>
    /// <param name="argStr">0이나 1</param>
    /// <returns>boolen</returns>
    bool StrToBool(string argStr)
    {
        if(argStr == "0")
        {
            return false;
        }
        else if(argStr == "1")
        {
            return true;
        }

        Debug.Log("Not allowed value");
        return false;
    }


    void TimingCheck()
    {
        if (m_isTiming)
        {
            m_phaseStartTime += Time.deltaTime;
            m_phaseTime = Mathf.Floor(m_phaseStartTime) + Mathf.Round((m_phaseStartTime % 1.0f) * 10.0f) / 10.0f;
        }
    }

    /// <summary>
    /// 타이머 시작
    /// </summary>
    void StartTimer()
    {
        m_isTiming = true;
        m_phaseStartTime = 0.0f;
        m_phaseTime = 0.0f;
    }
    /// <summary>
    /// 타이머 중지
    /// </summary>
    void StopTimer()
    {
        m_isTiming = false;
        m_phaseStartTime = 0.0f;
        m_phaseTime = 0.0f;
    }

    /// <summary>
    /// 단순 공격 생성
    /// </summary>
    /// <param name="argPosition">생성 위치</param>
    /// <param name="argRotation">생성 방향</param>
    /// <param name="argSize">생성 크기</param>
    /// <param name="argSpeed">생성 시 속도</param>
    void SimpleAtk(Vector3 argPosition, Vector3 argRotation, Vector3 argSize, float argSpeed, bool argIsMovePlayerAtk)
    {
        SimpleAtk _atk = ActiveSimpleAtk();
        _atk.m_speed = argSpeed;
        _atk.m_isMove = true;
        _atk.ChangeMovePlayerAtk(argIsMovePlayerAtk);

        _atk.transform.position = argPosition;
        _atk.transform.rotation = Quaternion.Euler(argRotation);
        _atk.transform.localScale = argSize;
    }
    /// <summary>
    /// 단순 공격 오브젝트 이동
    /// </summary>
    void SimpleAtkMove()
    {
        foreach(SimpleAtk item in m_activeSimpleAtkObjList)
        {
            if (item.m_isMove)
            {
                item.transform.Translate(Vector3.forward * item.m_speed * Time.deltaTime);

                if (item.transform.position.x <= -m_wallHalfSize * 2 ||
                    item.transform.position.x >= m_wallHalfSize * 2 ||
                    item.transform.position.y <= -m_wallHalfSize ||
                    item.transform.position.y >= m_wallHalfSize * 3 ||
                    item.transform.position.z <= -m_wallHalfSize * 2 ||
                    item.transform.position.z >= m_wallHalfSize * 2)
                {
                    item.transform.position = m_atkObjBasicPos;
                    item.ResetObj();

                    m_toWaitTmpList.Add(item);
                }
            }

        }

        foreach(SimpleAtk item in m_toWaitTmpList)
        {
            WaitSimpleAtk(item);
        }

        m_toWaitTmpList.Clear();
    }

    /// <summary>
    /// 원거리 공격
    /// </summary>
    /// <param name="argPosition">생성 위치</param>
    /// <param name="argRotation">생성 방향</param>
    /// <param name="argSize">생성 크기(공격 하는 오브젝트도 크기에 비례)</param>
    /// <param name="argWarnTime">공격 경고 시간</param>
    /// <param name="argAtkTime">공격 시간</param>
    void RangeAtk(Vector3 argPosition, Vector3 argRotation, Vector3 argSize)
    {
        RangeAtk _atk = ActiveRangeAtk();
        _atk.StartRangeAtk(m_rangeAtkWarnTime, m_rangeAtkTime);

        _atk.transform.position = argPosition;
        _atk.transform.rotation = Quaternion.Euler(argRotation);
        _atk.transform.localScale = argSize;
    }

    /// <summary>
    /// 현재 지면 전채 돌발 공격
    /// </summary>
    /// <returns></returns>
    void AllWallPopAtk()
    {
        StartCoroutine(IEPopAtk());
    }
    /// <summary>
    /// 전체 단순 돌발 공격 준비
    /// </summary>
    void AllWallPopAtkReady()
    {
        for(int i = -m_wallHalfSize; i < m_wallHalfSize; i += m_simpleAtkObjBasicSize)
        {
            for(int o = -m_wallHalfSize; o < m_wallHalfSize; o += m_simpleAtkObjBasicSize)
            {
                SimpleAtk _simAtk = m_simpleAtkObjWaitQueue.Dequeue();
                _simAtk.m_isMove = false;

                _simAtk.transform.position = new Vector3(o, -10.0f, i);
                m_activeSimpleAtkObjList.AddLast(_simAtk);
            }
        }
    }
    /// <summary>
    /// 전체 단순 돌발 공격 시행
    /// </summary>
    void AllWallPopAtkStart()
    {
        foreach(SimpleAtk item in m_activeSimpleAtkObjList)
        {
            if (!item.m_isMove)
            {
                item.PopAtk(m_popAtkActiveTime, m_popAtkSpeed, m_popAtkMaxHeight);
            }
        }
    }
    /// <summary>
    /// 전체 단순 공격 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator IEPopAtk()
    {
        AllWallPopAtkReady();
        m_popWarningObj.SetActive(true);
        yield return new WaitForSeconds(m_popAtkWarnTime);

        AllWallPopAtkStart();
        m_popWarningObj.SetActive(false);
    }

    /// <summary>
    /// 중력 공격
    /// </summary>
    /// <param name="argDirZ">공격 방향</param>
    void GravityAtk(float argDirZ)
    {
        StartCoroutine(IEChangeWall(argDirZ));
        StartCoroutine(IEGravityAtk());
    }
    /// <summary>
    /// 90도로 나누어지는 각도를 벽 인덱스로 변환
    /// </summary>
    /// <param name="argDirZ">방향</param>
    /// <returns>벽 인덱스</returns>
    int AngleToWallIndex(float argDirZ)
    {
        switch (argDirZ)
        {
            case 0.0f:
                return 2;
            case 90.0f:
                return 3;
            case 180.0f:
                return 0;
            case -90.0f:
                return 1;
            default:
                Debug.Log("Not allowed value");
                return 2;
        }
    }
    /// <summary>
    /// 벽 변경 코루틴
    /// </summary>
    /// <param name="argDirZ">euler 방향</param>
    /// <returns>none</returns>
    IEnumerator IEChangeWall(float argDirZ)
    {
        m_wallRotCompleteFlag = false;

        for (int i = 0; i < m_wallList.Count; i++)
        {
            m_wallList[i].gameObject.GetComponent<MeshRenderer>().material = m_transparentWallMat;
        }
        m_wallList[AngleToWallIndex(argDirZ)].gameObject.GetComponent<MeshRenderer>().material = m_wallMat;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, argDirZ));
        while (Quaternion.Angle(m_wallCenterPos.rotation, targetRotation) > 0.1f)
        {
            m_wallCenterPos.rotation = Quaternion.RotateTowards(
                m_wallCenterPos.rotation,
                targetRotation,
                m_wallDirChangeSpeed * Time.deltaTime);
            yield return null;
        }
        transform.rotation = targetRotation;
    }
    /// <summary>
    /// 중력 공격
    /// </summary>
    /// <param name="argDir">중력 공격 방향</param>
    /// <returns>none</returns>
    IEnumerator IEGravityAtk()
    {
        m_ascensionCompleteFlag = true;
        m_playerController.GetComponent<Rigidbody>().useGravity = false;

        while (true)
        {
            // 상승 상태일 때
            if (m_ascensionCompleteFlag && m_playerController.transform.position.y <
                m_wallCenterPos.position.y)
            {
                m_playerController.IsCanMoveFlage = false;
                m_playerController.transform.position = Vector3.MoveTowards(
                    m_playerController.transform.position,
                    m_wallCenterPos.position,
                    m_gravityAtkSpeed * Time.deltaTime);
            }
            else
            {
                m_ascensionCompleteFlag = false;

                // 하강 상태일 때
                m_playerController.transform.position = Vector3.MoveTowards(
                    m_playerController.transform.position,
                    new Vector3(m_playerController.transform.position.x,
                    0f,
                    m_playerController.transform.position.z),
                    m_gravityAtkSpeed * 4 * Time.deltaTime);
                if (m_playerController.transform.position.y <= 0.0f)
                {
                    m_playerController.IsCanMoveFlage = true;
                    m_playerController.GetComponent<Rigidbody>().useGravity = true;
                    yield break;
                }
            }
            yield return null;
        }
    }

    /// <summary>
    /// 단순 공격 활성화 큐로 이동
    /// </summary>
    /// <param name="argAtk">대상</param>
    /// <returns>대상</returns>
    SimpleAtk ActiveSimpleAtk()
    {
        m_activeSimpleAtkObjList.AddLast(m_simpleAtkObjWaitQueue.Dequeue());
        return m_activeSimpleAtkObjList.Last.Value;
    }
    /// <summary>
    /// 원거리 공격 활성화 큐로 이동
    /// </summary>
    /// <param name="argAtk">대상</param>
    /// <returns>대상</returns>
    RangeAtk ActiveRangeAtk()
    {
        m_activeRangeAtkObjQueue.AddLast(m_rangeAtkObjWaitQueue.Dequeue());
        return m_activeRangeAtkObjQueue.Last.Value;
    }

    /// <summary>
    /// 단순 공격 대기 큐로 변경
    /// </summary>
    /// <param name="argAtk"></param>
    public SimpleAtk WaitSimpleAtk(SimpleAtk argAtk)
    {
        m_activeSimpleAtkObjList.Remove(argAtk);
        m_simpleAtkObjWaitQueue.Enqueue(argAtk);
        argAtk.transform.position = m_atkObjBasicPos;
        return argAtk;
    }
    /// <summary>
    /// 단순 공격 대기 큐로 변경
    /// </summary>
    /// <param name="argAtk"></param>
    public RangeAtk WaitRangeAtk(RangeAtk argAtk)
    {
        m_activeRangeAtkObjQueue.Remove(argAtk);
        m_rangeAtkObjWaitQueue.Enqueue(argAtk);
        argAtk.transform.position = m_atkObjBasicPos;
        return argAtk;
    }

    /// <summary>
    /// 전체 단순 공격 오브젝트 리셋
    /// </summary>
    public void AllSimpleAtkObjReset()
    {
        foreach (SimpleAtk item in m_activeSimpleAtkObjList)
        {
            m_simpleAtkObjWaitQueue.Enqueue(item);
        }
        foreach (SimpleAtk item in m_simpleAtkObjWaitQueue)
        {
            item.gameObject.transform.position = m_atkObjBasicPos;
        }
    }
    /// <summary>
    /// 전체 원거리 공격 오브젝트 리셋
    /// </summary>
    public void AllRangeAtkObjReset()
    {
        foreach (RangeAtk item in m_activeRangeAtkObjQueue)
        {
            m_rangeAtkObjWaitQueue.Enqueue(item);
        }
        foreach (RangeAtk item in m_rangeAtkObjWaitQueue)
        {
            item.gameObject.transform.position = m_atkObjBasicPos;
        }
    }


    /// <summary>
    /// 자기 자신 인스턴스
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            return g_gameManager;
        }
    }
    public Material IsAtkObjMat
    {
        get { return m_atkObjMat; }
    }
    public Material IsMovePlayerAtkObjMat
    {
        get { return m_movePlayerAtkObjMat; }
    }
    public Material IsWarnMat
    {
        get { return m_warningMat; }
    }
}
