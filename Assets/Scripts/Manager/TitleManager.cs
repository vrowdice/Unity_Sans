using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("Round Select Button")]
    /// <summary>
    /// 라운드 선택 버튼 프리펩
    /// </summary>
    [SerializeField]
    GameObject m_roundSelectBtnPrefeb = null;
    /// <summary>
    /// 라운드 셀렉트 버튼 최대 사이즈
    /// </summary>
    [SerializeField]
    float m_roundSelectBtnMaxSize = 5.0f;
    /// <summary>
    /// 라운드 셀렉트 버튼 최소 사이즈
    /// </summary>
    [SerializeField]
    float m_roundSelectBtnMinSize = 3.0f;
    /// <summary>
    /// 각 버튼 간격 계수
    /// </summary>
    [SerializeField]
    float m_roundSelectBtnDistance = 1.0f;
    /// <summary>
    /// 라운드 선택 버튼 x 크기
    /// </summary>
    [SerializeField]
    float m_roundSelectBtnXSize = 10.0f;

    [Header("Scroll")]
    /// <summary>
    /// 스크롤 속도
    /// </summary>
    [SerializeField]
    float m_scrollSpeed = 1000f;
    /// <summary>
    /// 스크롤 부드러움의 정도
    /// </summary>
    [SerializeField]
    float m_scrollSmooth = 0.2f;

    /// <summary>
    /// 백그라운드 음악 재생할 오디오 소스
    /// </summary>
    private AudioSource m_audioSource = null;
    /// <summary>
    /// 라운드 정보 표시 텍스트
    /// </summary>
    private TextMeshPro m_roundText = null;
    /// <summary>
    /// 해당 버튼 집중
    /// </summary>
    private RoundSelectBtn m_focusedBtn = null;
    /// <summary>
    /// 라운드 선택 버튼 오브젝트 그룹
    /// </summary>
    private Transform m_roundSelectBtnParentTransform = null;
    /// <summary>
    /// 현재 속도
    /// </summary>
    private Vector3 m_scrollVelocity = Vector3.zero;
    /// <summary>
    /// 드래그 시작 시점의 마우스 위치
    /// </summary>
    private Vector3 dragStartPos;
    /// <summary>
    /// 최대 스크롤 위치
    /// </summary>
    private float m_minScrollPos = 0.0f;
    /// <summary>
    /// 드래그 중인지 여부
    /// </summary>
    private bool isDragging = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "InteractBtn")
        {
            m_focusedBtn = other.GetComponent<RoundSelectBtn>();
            m_roundText.text = GameManager.Instance.GetRoundData(m_focusedBtn.GetRoundIndex).m_roundName;
        }
    }

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_roundText = GameObject.Find("RoundText").GetComponent<TextMeshPro>();

        RoundSelectBtnSetting();
    }

    void Update()
    {
        ScrollMenu();
        ScrollMenuWithDrag();
        FocusedBtnManage();
    }

    /// <summary>
    /// 라운드 시작
    /// </summary>
    public void StartRound()
    {
        GameManager.Instance.GoMainScene(m_focusedBtn.GetRoundIndex);
    }

    /// <summary>
    /// 선택 버튼 초기 세팅
    /// </summary>
    void RoundSelectBtnSetting()
    {
        int _roundDataListCount = GameManager.Instance.GetRoundDataList.Count;
        m_minScrollPos = -(_roundDataListCount - 1) * (m_roundSelectBtnXSize + m_roundSelectBtnDistance);

        m_roundSelectBtnParentTransform = new GameObject("RoundSelectBtnParent").transform;
        m_roundSelectBtnParentTransform.position = new Vector3(0.0f, 3.0f, -2.5f);

        for(int i = 0; i < _roundDataListCount; i++)
        {
            RoundData _roundData = GameManager.Instance.GetRoundData(i);
            GameObject _selectBtnObj = Instantiate(m_roundSelectBtnPrefeb, m_roundSelectBtnParentTransform);
            _selectBtnObj.transform.localPosition = new Vector3(i * (m_roundSelectBtnXSize + m_roundSelectBtnDistance), 0.0f, 0.0f);
            _selectBtnObj.GetComponent<RoundSelectBtn>().SetRoundSelectBtn(this, _roundData.m_roundSprite, _roundData.m_roundIndex);
        }
    }

    /// <summary>
    /// 라운드 선택 메뉴 스크롤 동작
    /// </summary>
    void ScrollMenu()
    {
        // 목표 위치를 m_roundSelectBtnParentTransform.position으로 초기화
        Vector3 _targetPosition = m_roundSelectBtnParentTransform.position;

        // 마우스 스크롤
        float _mouseScroll = Input.GetAxis("Mouse ScrollWheel") * 50;
        _targetPosition += m_roundSelectBtnParentTransform.right * _mouseScroll * 100.0f;

        // 터치 입력
        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Moved)
            {
                float _moveX = _touch.deltaPosition.x * m_scrollSpeed * Time.deltaTime * 100.0f;
                _targetPosition += new Vector3(-_moveX, 0, 0);
            }
        }

        // 키보드 입력
        float _moveXKeys = Input.GetAxis("Horizontal") * m_scrollSpeed * Time.deltaTime * 100.0f;
        _targetPosition += new Vector3(-_moveXKeys, 0, 0);

        _targetPosition.x = Mathf.Clamp(_targetPosition.x, m_minScrollPos, 0.0f);
        float _distanceToTarget = Vector3.Distance(m_roundSelectBtnParentTransform.position, _targetPosition);
        float _smoothTime = _distanceToTarget < 1f ? 0.1f : m_scrollSmooth;
        m_roundSelectBtnParentTransform.position = Vector3.SmoothDamp(m_roundSelectBtnParentTransform.position, _targetPosition, ref m_scrollVelocity, _smoothTime);
    }
    /// <summary>
    /// 라운드 선택 메뉴 마우스 드래그로 스크롤
    /// </summary>
    void ScrollMenuWithDrag()
    {
        // 드래그 시작
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Input.mousePosition;
            isDragging = true;
        }

        // 드래그 중
        if (Input.GetMouseButton(0) && isDragging)
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 dragDelta = currentMousePos - dragStartPos;

            m_roundSelectBtnParentTransform.position += new Vector3(dragDelta.x * Time.deltaTime * m_scrollSpeed / 7, 0, 0);
            dragStartPos = currentMousePos;
        }

        // 드래그 종료
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        Vector3 clampedPos = m_roundSelectBtnParentTransform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, m_minScrollPos, 0.0f);
        m_roundSelectBtnParentTransform.position = clampedPos;
    }
    /// <summary>
    /// 집중할 버튼(중앙에 가까운 버튼) 관리
    /// </summary>
    void FocusedBtnManage()
    {
        if (m_focusedBtn != null)
        {
            m_roundSelectBtnParentTransform.position = Vector3.MoveTowards(
                m_roundSelectBtnParentTransform.position,
                new Vector3(-m_focusedBtn.transform.localPosition.x, m_roundSelectBtnParentTransform.position.y, m_roundSelectBtnParentTransform.position.z),
                10.0f * Time.deltaTime);
        }
    }

    float GetRoundSelectBtnMaxSize
    {
        get { return m_roundSelectBtnMaxSize; }
    }
    float GetRoundSelectBtnMinSize
    {
        get { return m_roundSelectBtnMinSize; }
    }
}
