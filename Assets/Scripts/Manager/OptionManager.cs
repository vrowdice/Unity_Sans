using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    /// <summary>
    /// 옵션 패널
    /// </summary>
    [SerializeField]
    GameObject m_optionPanel = null;

    /// <summary>
    /// 백그라운드 사운드 슬라이더
    /// </summary>
    [SerializeField]
    Slider m_backgroundSoundSlider = null;
    /// <summary>
    /// 이펙트 사운드 슬라이더
    /// </summary>
    [SerializeField]
    Slider m_effectSoundSlider = null;
    /// <summary>
    /// 값을 표시할 택스트
    /// </summary>
    Text m_backgroundValueText = null;
    Text m_effectSoundText = null;

    /// <summary>
    /// 옵션이 활성화 되었는지
    /// </summary>
    private bool m_optionState = false;

    // Start is called before the first frame update
    void Start()
    {
        //게임 매니저가 없을 경우 삭제
        if(GameManager.Instance == null)
        {
            Destroy(gameObject);
        }

        //택스트 가져오기
        m_backgroundValueText = m_backgroundSoundSlider.transform.GetChild(0).GetComponent<Text>();
        m_effectSoundText = m_effectSoundSlider.transform.GetChild(0).GetComponent<Text>();

        //초기화
        BackGroundSlider();
        EffectSoundSlider();
    }

    /// <summary>
    /// 각 슬라이더 값 변화 측정
    /// </summary>
    public void BackGroundSlider()
    {
        GameManager.Instance.GetSoundManager.BackgroundSoundVolume(m_backgroundSoundSlider.value / 100);
        m_backgroundValueText.text = m_backgroundSoundSlider.value.ToString();
    }
    public void EffectSoundSlider()
    {
        GameManager.Instance.GetSoundManager.EffectSoundVolume(m_effectSoundSlider.value / 100);
        m_effectSoundText.text = m_effectSoundSlider.value.ToString();
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
    /// <summary>
    /// 타이틀로 이동
    /// </summary>
    public void BackTitle()
    {
        GameManager.Instance.GoTitleScene();
    }

    public void OptionState(bool argState)
    {
        m_optionState = argState;

        if (m_optionState == true)
        {
            m_optionPanel.SetActive(true);
            GameManager.Instance.ChangeCursorState(true);

            if (RoundManager.Instance != null)
            {
                RoundManager.Instance.GetPlayerController.SetPlayerControllFlag = false;
            }
            Time.timeScale = 0;
        }
        else
        {
            m_optionPanel.SetActive(false);
            GameManager.Instance.ChangeCursorState(false);

            if (RoundManager.Instance != null)
            {
                RoundManager.Instance.GetPlayerController.SetPlayerControllFlag = true;
            }
            Time.timeScale = 1;
        }
    }

    public bool GetOptionState
    {
        get { return m_optionState; }
    }
}
