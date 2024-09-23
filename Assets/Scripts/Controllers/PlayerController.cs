using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Common")]
    /// <summary>
    /// 플레이어 콜리더 위치
    /// </summary>
    [SerializeField]
    Transform m_viewPlayerTransform;

    [Header("Camera")]
    /// <summary>
    /// 플레이어 3인칭 카메라 위치 베이스 위치
    /// </summary>
    [SerializeField]
    Transform m_cameraBaseTransform;
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
    /// 점프 속도
    /// </summary>
    [SerializeField]
    float m_jumpSpeed = 50.0f;
    /// <summary>
    /// 최대 점프 높이
    /// </summary>
    [SerializeField]
    float m_MaxJumpHight = 50.0f;
    /// <summary>
    /// 직선 움직임 속도
    /// </summary>
    [SerializeField]
    float m_moveSpeed = 50.0f;

    [Header("Player Infomation")]
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
    /// ui매니저
    /// </summary>
    private UIManager m_uiManager = null;

    /// <summary>
    /// 리지드바디
    /// </summary>
    private Rigidbody m_rigidbody;

    /// <summary>
    /// 마우스 회전값 저장
    /// </summary>
    private float m_mouseX = 0f;
    private float m_mouseY = 0f;

    /// <summary>
    /// 바닥과 접촉 판정일 경우 true
    /// </summary>
    private bool m_groundFlag = false;

    /// <summary>
    /// 점프 가능한 경우 true
    /// </summary>
    private bool m_canJumpFlag = false;

    /// <summary>
    /// 움직임 가능한 경우 true
    /// </summary>
    private bool m_canMoveFlage = true;

    /// <summary>
    /// 데미지를 입을 수 있는 상황인 경우 true
    /// </summary>
    private bool m_canDamageFlage = true;

    /// <summary>
    /// 체력
    /// </summary>
    private float m_hp = 0;

    /// <summary>
    /// 늦게 정해질 체력 값
    /// </summary>
    private float m_lateHp = 0;

    /// <summary>
    /// start
    /// </summary>
    private void Start()
    {
        m_uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        m_rigidbody = GetComponent<Rigidbody>();

        m_hp = m_maxHp;
        m_lateHp = m_maxHp;

        m_uiManager.SetSliders(m_maxHp);

        StartCoroutine(IESyncHealth());
    }
    /// <summary>
    /// update
    /// </summary>
    private void Update()
    {
        if (m_canMoveFlage)
        {
            MoveControll();
            JumpControll();
        }

        CameraControll();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Obstacle")
        {
            if (m_canDamageFlage)
            {
                m_canDamageFlage = false;
                IsLateHp -= 1;
                Invoke("CanDamageFlageTrue", m_canDamageTime);
            }
        }
    }

    /// <summary>
    /// 보조 체력을 기본 체력에 동기화하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator IESyncHealth()
    {
        while (true)
        {
            float _healthDifference = Mathf.Abs(IsHp - IsLateHp);
            if (_healthDifference > 0.1f)
            {
                if (IsHp > IsLateHp)
                {
                    IsHp -= 1;
                }
                else if (IsHp < IsLateHp)
                {
                    IsHp += 1;
                }
            }

            float _syncInterval = _healthDifference > 15.0f ? Mathf.Max(0.1f, 4.0f / _healthDifference) : m_maxHpSyncTime;
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
        //점프 입력 중
        if(Input.GetAxisRaw("Jump") != 0.0f)
        {
            if (m_canJumpFlag)
            {
                if (transform.position.y >= m_MaxJumpHight)
                {
                    CantJump();
                }
                m_rigidbody.useGravity = false;
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(transform.position.x, m_MaxJumpHight, transform.position.z),
                    m_jumpSpeed * Time.deltaTime);
            }
            else
            {
                if (transform.position.y >= m_MaxJumpHight)
                {
                    CantJump();
                }
            }
        }
        //점프 입력 없음
        else if(Input.GetAxisRaw("Jump") <= 0.0f)
        {
            if (m_canJumpFlag)
            {
                if (!m_groundFlag)
                {
                    m_rigidbody.useGravity = true;
                    CantJump();
                }
            }
            else
            {
                CantJump();
            }
        }
    }
    /// <summary>
    /// 점프 입력 제한
    /// </summary>
    void CantJump()
    {
        m_rigidbody.useGravity = true;
        m_canJumpFlag = false;
    }

    /// <summary>
    /// 이동을 위한 키 입력 및 전송
    /// </summary>s
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
    public bool IsCanMoveFlage
    {
        get { return m_canMoveFlage; }
        set { m_canMoveFlage = value; }
    }
    public float IsHp
    {
        get 
        { 
            return m_hp; 
        }
        set 
        {
            m_hp = value;
            if (IsHp <= 0)
            {
                //gameover
                Debug.Log("gameOver");
            }
            m_uiManager.HpSlider(m_hp);
        }
    }
    public float IsLateHp
    {
        get
        {
            return m_lateHp;
        }
        set
        {
            m_lateHp = value;

            if (m_lateHp <= 1)
            {
                m_lateHp = 1;
                IsHp -= 1;
            }

            m_uiManager.LateHpSlider(m_lateHp);
        }
    }
}
