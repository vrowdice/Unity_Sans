using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Common")]
    /// <summary>
    /// 기본 메테리얼
    /// 상호작용 되었을 시 메테리얼
    /// </summary>
    [SerializeField]
    Material m_standardInteractMat = null;
    [SerializeField]
    Material m_activeInteractMat = null;
    /// <summary>
    /// 캐릭터 인덱스 강제 설정
    /// </summary>
    [SerializeField]
    int m_charactorCode = 0;
    /// <summary>
    /// 최대 회복 횟수
    /// </summary>
    [SerializeField]
    int m_maxRecoverCount = 2;
    /// <summary>
    /// 회복 할 hp 계수
    /// </summary>
    [SerializeField]
    float m_recoverHp = 40.0f;


    [Header("Camera")]
    /// <summary>
    /// 플레이어 3인칭 카메라 위치 베이스 위치
    /// </summary>
    [SerializeField]
    Transform m_cameraBaseTransform = null;
    /// <summary>
    /// 마우스 감도
    /// </summary>
    [SerializeField]
    float m_mouseSensitivity = 1.0f;
    /// <summary>
    /// 카메라 제한 각도
    /// </summary>
    [SerializeField]
    float m_camMinXRotation = -30.0f;
    [SerializeField]
    float m_camMaxXRotation = 50.0f;

    [Header("Transform")]
    /// <summary>
    /// 중력 속도
    /// </summary>
    [SerializeField]
    public float m_fallSpeed = 12f;
    /// <summary>
    /// 점프 속도
    /// </summary>
    [SerializeField]
    float m_jumpForce = 15.0f;
    /// <summary>
    /// 최대 점프 설정 게이지
    /// </summary>
    [SerializeField]
    float m_maxjumpGauge = 10.0f;
    /// <summary>
    /// 직선 움직임 속도
    /// </summary>
    [SerializeField]
    float m_moveSpeed = 50.0f;

    [Header("Player Infomation")]
    /// <summary>
    /// 초기 플래이어 위치
    /// </summary>
    [SerializeField]
    Vector3 m_playerResetPos = new Vector3();
    /// <summary>
    /// 최대 체력
    /// </summary>
    [SerializeField]
    int m_maxHp = 0;
    /// <summary>
    /// hp와 late hp 동기화 분기 시간
    /// </summary>
    [SerializeField]
    float m_maxHpSyncTime = 0.0f;
    /// <summary>
    /// 다시 데미지를 입을 시간 간격
    /// </summary>
    [SerializeField]
    float m_canDamageTime = 0.0f;

    /// <summary>
    /// 현재 접근한 상호작용 버튼
    /// </summary>
    private GameObject m_InteractBtnObj = null;
    /// <summary>
    /// 현재 딛고있는 발판
    /// </summary>
    private Scaffold m_groundScaffold = null;
    /// <summary>
    /// ui매니저
    /// </summary>
    private UIManager m_uiManager = null;
    /// <summary>
    /// 리지드바디
    /// </summary>
    private Rigidbody m_rigidbody = null;
    /// <summary>
    /// 플래이어 스킨 위치 설정
    /// </summary>
    private Vector3 m_viewPlayerGenPos = new Vector3(0.0f, 1.0f, 0.0f);
    /// <summary>
    /// 현재 회복 가능한 횟수
    /// </summary>
    private int m_recoverCount = 0;
    /// <summary>
    /// 현재 최대 점프 게이지
    /// </summary>
    private float m_jumpGauge = 0.0f;
    /// <summary>
    /// 마우스 회전값 저장
    /// </summary>
    private float m_mouseX = 0f;
    private float m_mouseY = 0f;
    /// <summary>
    /// 체력
    /// </summary>
    private float m_hp = 0;
    /// <summary>
    /// 늦게 정해질 체력 값
    /// </summary>
    private float m_lateHp = 0;
    /// <summary>
    /// 바닥과 접촉 판정일 경우 true
    /// </summary>
    private bool m_groundFlag = false;
    /// <summary>
    /// 어느 벽과 접촉중일 경우 true
    /// </summary>
    private bool m_reachWallFlag = false;
    /// <summary>
    /// 점프 가능한 경우 true
    /// </summary>
    private bool m_canJumpFlag = false;
    /// <summary>
    /// 움직임 가능한 경우 true
    /// </summary>
    private bool m_canMoveFlag = true;
    /// <summary>
    /// 조작이 가능한 경우
    /// </summary>
    private bool m_playerControllFlag = true;
    /// <summary>
    /// 데미지를 입을 수 있는 상황인 경우 true
    /// </summary>
    private bool m_canDamageFlage = true;

    private void OnTriggerStay(Collider other)
    {
        if (RoundManager.Instance.GetIsTiming)
        {
            if (other.tag == "Attack")
            {
                GetDamage(-1);
            }
            else if (other.tag == "MovePlayerAttack")
            {
                if (m_rigidbody.velocity.magnitude >= 0.1f)
                {
                    GetDamage(-1);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            m_reachWallFlag = true;
            m_jumpGauge = m_maxjumpGauge;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            m_reachWallFlag = false;
        }
    }

    /// <summary>
    /// start
    /// </summary>
    private void Start()
    {
        StartSetting();
    }

    private void FixedUpdate()
    {
        if(m_playerControllFlag == true)
        {
            if (m_canMoveFlag == true)
            {
                MoveControll();
                JumpControll();
            }

            ConstantGravity();
        }
    }
    private void Update()
    {
        if (m_playerControllFlag == true)
        {
            CameraControll();
        }

        MouseBtnInputControll();

        if(GameManager.Instance != null)
        {
            KeyboardInputControll();
        }
    }

    /// <summary>
    /// 시작 세팅
    /// </summary>
    void StartSetting()
    {
        //변수 할당
        m_uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        m_rigidbody = GetComponent<Rigidbody>();

        if (GameManager.Instance != null)
        {
            m_charactorCode = GameManager.Instance.CharacterCode;
            //커서 상태 비활성화
            GameManager.Instance.ChangeCursorState(false);
        }


        //플래이어 위치 초기화
        ResetPlayerState();

        //슬라이더 초기 설정
        m_uiManager.SetSliders(m_maxHp);

        //체력 싱크 활성화
        StartCoroutine(IESyncHealth());

        //플래이어 스킨 생성
        GenViewPlayer();

    }

    /// <summary>
    /// 정속 중력 부과
    /// </summary>
    void ConstantGravity()
    {
        if (!m_groundFlag && !m_canJumpFlag)
        {
            Vector3 velocity = m_rigidbody.velocity;
            velocity.y = -m_fallSpeed;
            m_rigidbody.velocity = velocity;
        }
        else
        {
            if (m_groundScaffold != null && !m_reachWallFlag)
            {
                Vector3 _scaffoldMovement = m_groundScaffold.transform.forward * m_groundScaffold.m_speed * Time.fixedDeltaTime;
                m_rigidbody.MovePosition(m_rigidbody.position + _scaffoldMovement);
            }
        }
    }

    /// <summary>
    /// 데미지 부과
    /// </summary>
    /// <param name="argManageHp">hp 감소</param>
    void GetDamage(float argManageHp)
    {
        if (m_canDamageFlage)
        {
            m_canDamageFlage = false;
            LateHp += argManageHp;
            Invoke("CanDamageFlageTrue", m_canDamageTime);

            GameManager.Instance.SoundManager.PlayEffectSound(RoundManager.Instance.SetRoundData.m_soundData.m_hit);
        }
    }

    /// <summary>
    /// 체력 회복
    /// </summary>
    /// <param name="argManageHp">hp 증가</param>
    public void Recover(float argManageHp, bool argIsCheat)
    {
        if(m_recoverCount <= 0 && argIsCheat == false)
        {
            return;
        }

        RecoverCount--;
        Hp += argManageHp;
        LateHp += argManageHp;

        GameManager.Instance.SoundManager.PlayEffectSound(RoundManager.Instance.SetRoundData.m_soundData.m_heal);
    }

    /// <summary>
    /// 보조 체력을 기본 체력에 동기화하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator IESyncHealth()
    {
        while (true)
        {
            float _healthDifference = Hp - LateHp;
            if (_healthDifference > 0.1f)
            {
                if (Hp > LateHp)
                {
                    Hp -= 1;
                }
            }
            else
            {
                yield return null;
            }
            float _syncInterval = _healthDifference > 15.0f
                ? Mathf.Max(0.1f, 4.0f / _healthDifference)
                : m_maxHpSyncTime;

            yield return new WaitForSeconds(_syncInterval);
        }
    }

    /// <summary>
    /// 다시 데미지 입는 것이 가능하게 해주는
    /// 플래그를 true로 바꿈 invoke 사용
    /// </summary>
    void CanDamageFlageTrue()
    {
        m_canDamageFlage = true;
    }

    /// <summary>
    /// 점프
    /// </summary>
    void JumpControll()
    {
        if (m_canJumpFlag)
        {
            // 점프 입력 중
            if (Input.GetAxisRaw("Jump") >= 0.1f)
            {
                // 지면에 있을 때만 점프 시작
                if (m_groundFlag)
                {
                    m_groundFlag = false;
                }

                // 최대 점프 높이에 도달하면 점프 중단
                if (m_jumpGauge <= 0.0f)
                {
                    CantJump();
                }
                else
                {
                    m_jumpGauge -= 0.01f;
                    m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, m_jumpForce, m_rigidbody.velocity.z);
                }
            }
            // 점프 입력 없음
            else
            {
                if (!m_groundFlag)
                {
                    CantJump();
                }
            }
        }
    }

    /// <summary>
    /// 점프 입력 제한
    /// </summary>
    void CantJump()
    {
        m_rigidbody.velocity = new Vector3(m_rigidbody.velocity.x, 0.0f, m_rigidbody.velocity.z);
        m_canJumpFlag = false;
    }

    /// <summary>
    /// 이동을 위한 키 입력 및 전송
    /// </summary>
    void MoveControll()
    {
        Vector3 _vector = new Vector3();
        _vector.x = Input.GetAxisRaw("Horizontal");
        _vector.z = Input.GetAxisRaw("Vertical");

        _vector = transform.TransformDirection(_vector);
        Vector3 velocity = m_rigidbody.velocity;

        velocity.x = _vector.x * m_moveSpeed;
        velocity.z = _vector.z * m_moveSpeed;

        m_rigidbody.velocity = velocity;
    }

    /// <summary>
    /// 플레이어 카메라 컨트롤
    /// </summary>
    void CameraControll()
    {
        m_mouseX += Input.GetAxis("Mouse X") * m_mouseSensitivity;
        m_mouseY += Input.GetAxis("Mouse Y") * m_mouseSensitivity;
        m_mouseY = Mathf.Clamp(m_mouseY, m_camMinXRotation, m_camMaxXRotation);

        transform.localEulerAngles = new Vector3(0.0f, m_mouseX, 0.0f);
        m_cameraBaseTransform.localEulerAngles = new Vector3(-m_mouseY, 0.0f, 0.0f);

        m_cameraBaseTransform.transform.position = transform.position;
    }

    /// <summary>
    /// 마우스 버튼 클릭 설정
    /// </summary>
    void MouseBtnInputControll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(m_InteractBtnObj != null)
            {
                if (m_InteractBtnObj.name == "StartBtn")
                {
                    if (RoundManager.Instance.GetIsGameOver)
                    {
                        RoundManager.Instance.RestartGame();
                        ResetPlayerState();
                    }
                    else
                    {
                        m_InteractBtnObj.GetComponent<MeshRenderer>().material = m_standardInteractMat;
                        m_InteractBtnObj = null;
                        RoundManager.Instance.PhaseStart();
                    }
                }
                else if (m_InteractBtnObj.name == "RecoverBtn")
                {
                    Recover(m_recoverHp, false);
                }
            }
        }
    }
    /// <summary>
    /// 키보드 입력 제어
    /// </summary>
    void KeyboardInputControll()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.OptionManager.OptionState(true);
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            m_playerControllFlag = false;
            GameManager.Instance.ChangeCursorState(true);
        }
        else
        {
            if(GameManager.Instance.OptionManager.GetOptionState == false)
            {
                m_playerControllFlag = true;
                GameManager.Instance.ChangeCursorState(false);
            }
        }
    }

    /// <summary>
    /// 플래이어 스킨 생성
    /// </summary>
    void GenViewPlayer()
    {
        GameObject _obj = Instantiate(GameManager.Instance.GetCharactorData(m_charactorCode).m_object, transform);
        _obj.transform.localPosition = m_viewPlayerGenPos;
    }

    /// <summary>
    /// 점프 상태 변경
    /// </summary>
    /// <param name="argState">현재 점프 상태</param>
    public void JumpState(bool argState)
    {
        if(argState == true)
        {
            IsCanJumpFlag = true;
            IsGroundFlag = true;
            m_jumpGauge = m_maxjumpGauge;
        }
        else
        {
            IsCanJumpFlag = false;
            IsGroundFlag = false;
        }
    }

    /// <summary>
    /// 플래이어 위치 초기화
    /// </summary>
    public void ResetPlayerPosition()
    {
        transform.position = m_playerResetPos;
    }

    /// <summary>
    /// 플레이어 상태 초기화
    /// </summary>
    public void ResetPlayerState()
    {
        ResetPlayerPosition();
        Hp = m_maxHp;
        LateHp = m_maxHp;
        RecoverCount = m_maxRecoverCount;
    }

    public Material StandardInteractMat
    {
        get { return m_standardInteractMat; }
    }
    public Material ActiveInteractMat
    {
        get { return m_activeInteractMat; }
    }
    public GameObject InteractBtnObj
    {
        get { return m_InteractBtnObj; }
        set { m_InteractBtnObj = value; }
    }
    public Scaffold GroundScaffold
    {
        get { return m_groundScaffold; }
        set { m_groundScaffold = value; }
    }
    public bool IsCanDamageFlage
    {
        get { return m_canDamageFlage; }
        set { m_canDamageFlage = value; }
    }
    public bool IsCanJumpFlag
    {
        get { return m_canJumpFlag; }
        set { m_canJumpFlag = value; }
    }
    public bool IsGroundFlag
    {
        get { return m_groundFlag; }
        set { m_groundFlag = value; }
    }
    public bool CanMoveFlage
    {
        get { return m_canMoveFlag; }
        set { m_canMoveFlag = value; }
    }
    public bool PlayerControllFlag
    {
        get { return m_playerControllFlag; }
        set { m_playerControllFlag = value; }
    }
    public bool Gravity
    {
        get { return m_rigidbody.useGravity; }
        set { m_rigidbody.useGravity = value; }
    }
    public int RecoverCount
    {
        get { return m_recoverCount; }
        set
        {
            m_recoverCount = value;

            if(m_recoverCount <= 0)
            {
                m_recoverCount = 0;
            }
            if(m_recoverCount >= m_maxRecoverCount)
            {
                m_recoverCount = m_maxRecoverCount;
            }

            RoundManager.Instance.GetUIObjManager.SetRecoverCountTextObj(m_recoverCount);
        }
    }
    public float Hp
    {
        get 
        { 
            return m_hp; 
        }
        private set
        {
            m_hp = value;
            if (m_hp <= 0)
            {
                RoundManager.Instance.GameOver(false);
            }
            else if(m_hp > m_maxHp)
            {
                m_hp = m_maxHp;
            }
            m_uiManager.HpSlider(m_hp);
        }
    }
    public float LateHp
    {
        get
        {
            return m_lateHp;
        }
        private set
        {
            m_lateHp = value;
            if (m_lateHp <= 1)
            {
                m_lateHp = 1;
                Hp -= 1;
            }
            else if (m_lateHp > m_maxHp)
            {
                m_lateHp = m_maxHp;
            }
            m_uiManager.LateHpSlider(m_lateHp);
        }
    }
}
