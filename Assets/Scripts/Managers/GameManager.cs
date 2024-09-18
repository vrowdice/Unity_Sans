using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 공격 타입 이넘
/// </summary>
public enum AtkType
{
    BlueHorizontalBone,
    BlueVerticalBone,

    HorizontalBone,
    VerticalBone,

    PopVerticalBone,
    GasterBlaster,

    Gravity,

    scaffold,
}

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
    /// 중력 공격 힘(속도)
    /// </summary>
    [SerializeField]
    float m_gravityAtkPower = 0.0f;

    /// <summary>
    /// 벽 변환 속도
    /// </summary>
    [SerializeField]
    float m_wallDirChangeSpeed = 0.0f;

    /// <summary>
    /// 플레이어 컨트롤러 스크립트
    /// </summary>
    private PlayerController m_playerController = null;

    /// <summary>
    /// 적 공격 데이터 리스트
    /// </summary>
    private List<AtkData> m_atkDataList = new List<AtkData>();

    /// <summary>
    /// 현재 플레이어가 사용하는 벽
    /// </summary>
    private int m_nowWall = 0;

    /// <summary>
    /// 현재 페이즈
    /// </summary>
    private int m_phase = 1;

    /// <summary>
    /// 현재 페이즈가 지난 시간
    /// </summary>
    private float m_phaseTime = 0.0f;

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
    }
    void Start()
    {
        GravityAtk(90.0f);
    }
    void Update()
    {
        
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
            _atkData.m_type = StrToItemType(_data[i]["type"].ToString());
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
    /// 아이템 타입 문자열 이넘으로 변경
    /// </summary>
    /// <param name="argStr">아이템 타입 문자열</param>
    /// <returns>아이템 타입 이넘</returns>
    AtkType StrToItemType(string argStr)
    {
        switch (argStr)
        {
            case "BlueHorizontalBone":
                return AtkType.BlueHorizontalBone;
            case "BlueVerticalBone":
                return AtkType.BlueVerticalBone;
            case "HorizontalBone":
                return AtkType.HorizontalBone;
            case "VerticalBone":
                return AtkType.VerticalBone;
            case "PopVerticalBone":
                return AtkType.PopVerticalBone;
            case "GasterBlaster":
                return AtkType.GasterBlaster;
            case "Gravity":
                return AtkType.Gravity;
            case "scaffold":
                return AtkType.scaffold;
            default:
                Debug.Log("Not allowed value");
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

    /// <summary>
    /// 현재 지면 전채 뼈 공격
    /// </summary>
    /// <returns></returns>
    IEnumerator AllWallBoneAtk()
    {
        // 매 프레임 대기
        yield return null;
    }

    /// <summary>
    /// 중력 공격
    /// </summary>
    /// <param name="argDir">공격 방향</param>
    void GravityAtk(float argDir)
    {
        StartCoroutine(ChangeWallIE(argDir));
        StartCoroutine(GravityAtkIE());
    }
    /// <summary>
    /// 벽 변경 코루틴
    /// </summary>
    /// <param name="argDirZ">euler 방향</param>
    /// <returns>none</returns>
    IEnumerator ChangeWallIE(float argDirZ)
    {
        m_wallRotCompleteFlag = false;

        // 목표 회전값을 Quaternion으로 변환
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, argDirZ));

        // 목표 각도에 도달할 때까지 반복
        while (Quaternion.Angle(m_wallCenterPos.rotation, targetRotation) > 0.1f)
        {
            // 일정한 속도로 회전
            m_wallCenterPos.rotation = Quaternion.RotateTowards(
                m_wallCenterPos.rotation,
                targetRotation,
                m_wallDirChangeSpeed * Time.deltaTime);

            // 한 프레임 대기 후 다시 회전
            yield return null;
        }

        // 정확한 목표 각도로 설정
        transform.rotation = targetRotation;
    }
    /// <summary>
    /// 중력 공격
    /// </summary>
    /// <param name="argDir">중력 공격 방향</param>
    /// <returns>none</returns>
    IEnumerator GravityAtkIE()
    {
        m_ascensionCompleteFlag = true;
        m_playerController.GetComponent<Rigidbody>().useGravity = false;

        while (true)
        {
            // 상승 상태일 때
            if (m_ascensionCompleteFlag && m_playerController.transform.position.y <
                m_wallCenterPos.position.y)
            {
                // 위로 상승
                m_playerController.IsCanMoveFlage = false;
                m_playerController.transform.position = Vector3.MoveTowards(
                    m_playerController.transform.position,
                    m_wallCenterPos.position,
                    m_gravityAtkPower * Time.deltaTime);
            }
            else
            {
                // 상승 완료 플래그를 설정
                m_ascensionCompleteFlag = false;

                // 하강 상태일 때
                m_playerController.transform.position = Vector3.MoveTowards(
                    m_playerController.transform.position,
                    new Vector3(m_playerController.transform.position.x,
                    0f,
                    m_playerController.transform.position.z),
                    m_gravityAtkPower * 4 * Time.deltaTime);

                // 지면에 도달하면 공격 종료
                if (m_playerController.transform.position.y <= 0.0f)
                {
                    m_playerController.IsCanMoveFlage = true;
                    m_playerController.GetComponent<Rigidbody>().useGravity = true;
                    yield break;
                }
            }

            // 매 프레임 대기
            yield return null;
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
}
