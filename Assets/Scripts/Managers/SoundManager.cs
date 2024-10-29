using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// 백그라운드 오디오 소스
    /// </summary>
    private AudioSource m_backgroundSoundAS = null;
    /// <summary>
    /// 이펙트 오디오소스
    /// </summary>
    private AudioSource m_effectSoundAS = null;

    // Start is called before the first frame update
    void Start()
    {
        m_backgroundSoundAS = transform.Find("BackGroundSound").GetComponent<AudioSource>();
        m_effectSoundAS = transform.Find("EffectSound").GetComponent<AudioSource>();
    }

    /// <summary>
    /// 이펙트 재생
    /// </summary>
    /// <param name="argAudioClip">오디오 클립</param>
    public void PlayEffectSound(AudioClip argAudioClip)
    {
        m_effectSoundAS.PlayOneShot(argAudioClip);
    }
    /// <summary>
    /// 배경음악 재생
    /// </summary>
    /// <param name="argAudioClip">오디오 클립</param>
    public void PlayBackgroundSound(AudioClip argAudioClip)
    {
        m_backgroundSoundAS.clip = argAudioClip;
        m_backgroundSoundAS.loop = true;
        m_backgroundSoundAS.Play();
    }

    /// <summary>
    /// 각 볼륨 설정
    /// </summary>
    /// <param name="argVal">볼륨 값</param>
    public void BackgroundSoundVolume(float argVal)
    {
        m_backgroundSoundAS.volume = argVal;
    }
    public void EffectSoundVolume(float argVal)
    {
        m_effectSoundAS.volume = argVal;
    }

    /// <summary>
    /// 오디오 클립 리셋
    /// </summary>
    public void ResetAudioClip()
    {
        m_backgroundSoundAS.clip = null;
        m_effectSoundAS.clip = null;

        m_backgroundSoundAS.Stop();
        m_effectSoundAS.Stop();
    }
}
