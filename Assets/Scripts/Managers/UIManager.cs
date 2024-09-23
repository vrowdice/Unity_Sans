using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Hp 슬라이더
    /// </summary>
    [SerializeField]
    Slider m_hpSlider = null;
    /// <summary>
    /// 시간이 지남에 따라 닳는 Hp 슬라이더
    /// </summary>
    [SerializeField]
    Slider m_lateHpSlider = null;
    /// <summary>
    /// Hp 값 텍스트
    /// </summary>
    [SerializeField]
    Text m_hpText = null;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// 슬라이더 초기 셋팅
    /// </summary>
    /// <param name="argHpSliderVal">hp 슬라이더 최대값</param>
    /// <param name="argLateHpSliderVal">late hp 슬라이더 최대값</param>
    public void SetSliders(int argVal)
    {
        m_hpSlider.maxValue = argVal;
        m_hpSlider.value = m_hpSlider.maxValue;

        m_lateHpSlider.maxValue = argVal;
        m_lateHpSlider.value = m_lateHpSlider.maxValue;

        HpText();
    }

    /// <summary>
    /// hp 슬라이더 정하기
    /// </summary>
    /// <param name="argValue">정할 값</param>
    public void HpSlider(float argValue)
    {
        m_hpSlider.value = argValue;

        HpText();
    }
    /// <summary>
    /// 나중에 정해지는 hp 슬라이더 정하기
    /// </summary>
    /// <param name="argValue">정할 값</param>
    public void LateHpSlider(float argValue)
    {
        m_lateHpSlider.value = argValue;
    }

    /// <summary>
    /// hp 텍스트 슬라이더 값으로 설정
    /// </summary>
    void HpText()
    {
        string _str = string.Empty;
        
        if (m_hpSlider.value / 10.0f < 1.0f)
        {
            _str = "0";
        }
        _str += m_hpSlider.value;

        m_hpText.text = _str + " / " + m_hpSlider.maxValue;
    }
}
