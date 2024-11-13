using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    /// <summary>
    /// 상점 패널 프리펍
    /// </summary>
    [SerializeField]
    GameObject m_storePanelPrefeb = null;
    /// <summary>
    /// 캐릭터 선택 버튼 프리펍
    /// </summary>
    [SerializeField]
    GameObject m_characterSelectBtnPrefeb = null;

    /// <summary>
    /// 확률 가중치 설정
    /// </summary>
    [SerializeField]
    float m_legendProWeight = 0.0f;
    [SerializeField]
    float m_epicProWeight = 0.0f;
    [SerializeField]
    float m_rareProWeight = 0.0f;
    [SerializeField]
    float m_normalProWeight = 0.0f;

    /// <summary>
    /// 캐릭터 선택 버튼 리스트
    /// </summary>
    private List<CharacterSelectBtn> m_characterSelectBtnList = new List<CharacterSelectBtn>();

    /// <summary>
    /// 인벤토리 패널 트랜스폼
    /// </summary>
    private Transform m_inventoryPanelContentTrans = null;
    /// <summary>
    /// 뽑기 버튼
    /// </summary>
    private Button m_rollBtn = null;
    /// <summary>
    /// 인밴토리 창을 여는 버튼
    /// </summary>
    private Button m_invectoryBtn = null;

    // Start is called before the first frame update
    void Start()
    {
        StartSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 스타트 세팅
    /// </summary>
    void StartSetting()
    {
        //각 UI변수 할당
        if (m_storePanelPrefeb != null || GameManager.Instance != null)
        {
            GameObject _storePanel = Instantiate(m_storePanelPrefeb, GameManager.Instance.GetCanvasTrans);
            m_storePanelPrefeb = _storePanel;

            Transform _inventoryPanel = _storePanel.transform.Find("InventoryPanel");
            m_inventoryPanelContentTrans = _inventoryPanel.GetChild(0);

            m_invectoryBtn = _storePanel.transform.Find("InventoryBtn").GetComponent<Button>();
            m_rollBtn = _storePanel.transform.Find("RollBtn").GetComponent<Button>();
        }

        //버튼 작업 할당
        if(m_invectoryBtn != null)
        {
            m_invectoryBtn.onClick.AddListener(() => ResetInventoryPanel());
        }
        if(m_rollBtn != null)
        {
            m_invectoryBtn.onClick.AddListener(() => RollCharacter());
        }
    }

    /// <summary>
    /// 캐릭터 뽑기
    /// </summary>
    public void RollCharacter()
    {

    }

    /// <summary>
    /// 보유중인 캐릭터 선택
    /// </summary>
    /// <param name="argCharacterIndex">캐릭터 인덱스</param>
    public bool SelectCharacter(int argCharacterIndex)
    {
        if (GameManager.Instance.GetHaveCharactorList[argCharacterIndex] == false || argCharacterIndex <= -1)
        {
            GameManager.Instance.Alert("Don't have this character!");
            return false;
        }

        GameManager.Instance.SetCharacterIndex = argCharacterIndex;

        for(int i = 0; i < GameManager.Instance.GetHaveCharactorList.Count; i++)
        {
            if(GameManager.Instance.GetHaveCharactorList[i] == true)
            {
                m_characterSelectBtnList[i].CheckImageActive(true);
            }
            else
            {
                m_characterSelectBtnList[i].CheckImageActive(false);
            }
        }
        return true;
    }

    /// <summary>
    /// 인벤토리 패널 초기화
    /// </summary>
    public void ResetInventoryPanel()
    {
        if (m_inventoryPanelContentTrans == null)
        {
            Debug.Log("no inv trans");
            return;
        }

        for (int i = 0; i < m_inventoryPanelContentTrans.childCount; i++)
        {
            Destroy(m_inventoryPanelContentTrans.GetChild(i).gameObject);
        }
        m_characterSelectBtnList.Clear();

        List<CharacterData> _characterData = GameManager.Instance.GetCharacterDataList;
        List<bool> _haveCharacterData = GameManager.Instance.GetHaveCharactorList;
        for (int i = 0; i < _characterData.Count; i++)
        {
            CharacterSelectBtn _btn = Instantiate(m_characterSelectBtnPrefeb, m_inventoryPanelContentTrans).GetComponent<CharacterSelectBtn>();
            m_characterSelectBtnList.Add(_btn);

            _btn.transform.GetChild(0).GetComponent<Text>().text = _characterData[i].m_name;

            if (_haveCharacterData[i] == true)
            {
                GameObject _sampleObj = Instantiate(_characterData[i].m_object, _btn.transform);
                _sampleObj.transform.localPosition = Vector3.zero;
                _sampleObj.transform.localScale = _sampleObj.transform.localScale * 100;

                _btn.ResetBtn(i, false, this);

                _btn.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                _btn.ResetBtn(i, false, this);

                _btn.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        SelectCharacter(GameManager.Instance.SetCharacterIndex);
    }
}
