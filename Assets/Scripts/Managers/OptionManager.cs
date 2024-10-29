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
    /// 옵션이 활성화 되었는지
    /// </summary>
    private bool m_optionState = false;

    // Start is called before the first frame update
    void Start()
    {
        if(GameDataManager.Instance == null)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 각 슬라이더 값 변화 측정
    /// </summary>
    public void BackGroundSlider()
    {
        GameDataManager.Instance.GetSoundManager.BackgroundSoundVolume(m_backgroundSoundSlider.value / 100);
    }
    public void EffectSoundSlider()
    {
        GameDataManager.Instance.GetSoundManager.EffectSoundVolume(m_effectSoundSlider.value / 100);
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
        GameDataManager.Instance.GoTitleScene();
    }

    public void OptionState(bool argState)
    {
        m_optionState = argState;

        if (m_optionState == true)
        {
            m_optionPanel.SetActive(true);
            GameDataManager.Instance.CursorState(true);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.GetPlayerController.SetPlayerControllFlag = false;
            }
            Time.timeScale = 0;
        }
        else
        {
            m_optionPanel.SetActive(false);
            GameDataManager.Instance.CursorState(false);

            if (GameManager.Instance != null)
            {
                GameManager.Instance.GetPlayerController.SetPlayerControllFlag = true;
            }
            Time.timeScale = 1;
        }
    }

    public bool GetOptionState
    {
        get { return m_optionState; }
    }
}
