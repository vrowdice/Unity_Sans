using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    [Header("Panel Setting")]
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

    [Header("Gacha")]
    /// <summary>
    /// 확률 가중치 설정 리스트
    /// 
    /// 0 = legend
    /// 1 = epic
    /// 2 = rare
    /// 3 = normal
    /// 
    /// 각 가중치 설정 후 같은 등급의 캐릭터는
    /// 랜덤으로 나옴
    /// </summary>
    [SerializeField]
    List<float> m_proWeightList = new List<float>();

    /// <summary>
    /// 가챠 돌리는 것에 필요한 돈
    /// </summary>
    [SerializeField]
    long m_gachaMoney = 0;

    /// <summary>
    /// 캐릭터 선택 버튼 리스트
    /// </summary>
    private List<CharacterSelectBtn> m_characterSelectBtnList = new List<CharacterSelectBtn>();
    /// <summary>
    /// 인벤토리 패널 트랜스폼
    /// </summary>
    private Transform m_inventoryPanelContentTrans = null;
    /// <summary>
    /// 가챠를 돌리면 나온 캐릭터의 샘플 오브젝트를 표시
    /// </summary>
    private GameObject m_sampleCharacterObj = null;
    /// <summary>
    /// 가챠를 돌리면 이 이미지를 표시하지 않음
    /// </summary>
    private Image m_rollReadyImage = null;
    /// <summary>
    /// 가챠 돌린 정보 텍스트
    /// </summary>
    private Text m_gachaInfoText = null;
    /// <summary>
    /// 뽑기 버튼
    /// </summary>
    private Button m_rollBtn = null;
    /// <summary>
    /// 인밴토리 창을 여는 버튼
    /// </summary>
    private Button m_invectoryBtn = null;

    /// <summary>
    /// 모든 가중치를 더한 결과
    /// </summary>
    private float m_allProWeightCount = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartSetting();
    }

    /// <summary>
    /// 스타트 세팅
    /// </summary>
    void StartSetting()
    {
        //각 UI변수 할당
        if (m_storePanelPrefeb != null || GameManager.Instance != null)
        {
            GameObject _storePanel = Instantiate(m_storePanelPrefeb, GameManager.Instance.CanvasTrans);
            m_storePanelPrefeb = _storePanel;

            Transform _inventoryPanel = _storePanel.transform.Find("InventoryPanel");
            m_inventoryPanelContentTrans = _inventoryPanel.GetChild(0);

            m_invectoryBtn = _storePanel.transform.Find("InventoryBtn").GetComponent<Button>();
            m_rollBtn = _storePanel.transform.Find("RollBtn").GetComponent<Button>();
            m_gachaInfoText = _storePanel.transform.Find("GachaInfoText").GetComponent<Text>();
            m_rollReadyImage = _storePanel.transform.Find("RollReadyImage").GetComponent<Image>();
            _storePanel.transform.Find("GachaMoneyText").GetComponent<Text>().text = "Gacha Money : " + m_gachaMoney;
        }

        //버튼 작업 할당
        if(m_invectoryBtn != null)
        {
            m_invectoryBtn.onClick.AddListener(() => ResetInventoryPanel());
        }
        if(m_rollBtn != null)
        {
            m_rollBtn.onClick.AddListener(() => RollCharacter());
        }
        
        //모든 가중치를 더한 값 계산
        for(int i = 0; i < m_proWeightList.Count; i++)
        {
            m_allProWeightCount = m_allProWeightCount + m_proWeightList[i];
        }
    }

    /// <summary>
    /// 캐릭터 타입의 어울리는 색을 결정
    /// </summary>
    /// <param name="argType">캐릭터 타입</param>
    /// <returns>색</returns>
    Color CharTypeToColor(CharacterType argType)
    {
        switch (argType)
        {
            case CharacterType.Legend:
                return Color.yellow;
            case CharacterType.Epic:
                return Color.magenta;
            case CharacterType.Rare:
                return Color.blue;
            case CharacterType.Normal:
                return Color.white;
            default:
                return Color.white;
        }
    }

    /// <summary>
    /// 캐릭터 뽑기
    /// </summary>
    public void RollCharacter()
    {
        if(GameManager.Instance.Money <= m_gachaMoney)
        {
            GameManager.Instance.Alert("Not Enough Money!");
            return;
        }

        int _characterKey = 0;
        float _rand = Random.Range(0.0f, m_allProWeightCount);
        List<int> _charSortList = GameManager.Instance.CharacterDicSortList;
        List<int> _selectedCharList = new List<int>();

        //더하면서 가중치 계산
        float _weight = 0.0f;
        for(int i = 0; i < m_proWeightList.Count; i++)
        {
            _weight = _weight + m_proWeightList[i];
            if(_weight >= _rand)
            {
                _characterKey = (i + 1) * 10000;

                for(int j = 0; j < _charSortList.Count; j++)
                {
                    if (_charSortList[j] / _characterKey == 1)
                    {
                        _selectedCharList.Add(_charSortList[j]);
                    }
                }
                
            }
        }

        //뽑을 등급 설정 후 등급 안에서 인덱스 랜덤 생성
        int _randInt = Random.Range(0, _selectedCharList.Count);
        CharacterInfo _characterInfo = GameManager.Instance.GetCharacterInfo(_selectedCharList[_randInt]);

        //이미 가지고 있을 경우 일부 금액 반환
        if (_characterInfo.m_isHave == true)
        {
            GameManager.Instance.Money += m_gachaMoney / 5;
            m_gachaInfoText.text = "Refund " + m_gachaMoney / 5;
        }
        else
        {
            _characterInfo.m_isHave = true;
            ResetInventoryPanel();
            m_gachaInfoText.text = "New Character: " + _characterInfo.m_data.m_name;
        }

        //가챠 금액 차감
        GameManager.Instance.Money -= m_gachaMoney;

        if(m_sampleCharacterObj != null)
        {
            Destroy(m_sampleCharacterObj);
        }
        if(_characterInfo.m_data.m_object != null)
        {
            m_sampleCharacterObj = Instantiate(_characterInfo.m_data.m_object, m_rollReadyImage.transform);
            m_sampleCharacterObj.transform.localPosition = Vector3.zero;
            m_sampleCharacterObj.transform.localScale = m_sampleCharacterObj.transform.localScale * 100;
        }

        m_rollReadyImage.enabled = false;
    }

    /// <summary>
    /// 보유중인 캐릭터 선택
    /// </summary>
    /// <param name="argKey">캐릭터 키</param>
    public bool SelectCharacter(int argKey)
    {
        if (GameManager.Instance.GetCharacterInfo(argKey).m_isHave == false || argKey <= -1)
        {
            GameManager.Instance.Alert("Don't have this character!");
            return false;
        }

        GameManager.Instance.CharacterCode = argKey;

        for(int i = 0; i < m_characterSelectBtnList.Count; i++)
        {
            if(GameManager.Instance.CharacterDicSortList[i] == argKey)
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

        Destroy(m_sampleCharacterObj);
        m_rollReadyImage.enabled = true;
        m_gachaInfoText.text = string.Empty;

        for (int i = 0; i < m_inventoryPanelContentTrans.childCount; i++)
        {
            Destroy(m_inventoryPanelContentTrans.GetChild(i).gameObject);
        }
        m_characterSelectBtnList.Clear();

        List<int> _characterKey = GameManager.Instance.CharacterDicSortList;
        for (int i = 0; i < _characterKey.Count; i++)
        {
            CharacterInfo _infoData = GameManager.Instance.GetCharacterInfo(_characterKey[i]);

            CharacterSelectBtn _btn = Instantiate(m_characterSelectBtnPrefeb, m_inventoryPanelContentTrans).GetComponent<CharacterSelectBtn>();
            m_characterSelectBtnList.Add(_btn);

            _btn.transform.GetChild(0).GetComponent<Text>().text = _infoData.m_data.m_name;

            if (_infoData.m_isHave == true)
            {
                if(_infoData.m_data.m_object != null)
                {
                    GameObject _sampleObj = Instantiate(_infoData.m_data.m_object, _btn.transform);
                    _sampleObj.transform.localPosition = Vector3.zero;
                    _sampleObj.transform.localScale = _sampleObj.transform.localScale * 100;
                }

                _btn.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                _btn.transform.GetChild(1).gameObject.SetActive(true);
            }

            _btn.ResetBtn(_characterKey[i], false, this);
            _btn.GetComponent<Image>().color = CharTypeToColor(_infoData.m_data.m_type);
        }

        SelectCharacter(GameManager.Instance.CharacterCode);
    }
}
