using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    /// <summary>
    /// 사운드 (추후 다른 라운드 사운드도 적용 예정)
    /// </summary>
    [SerializeField]
    Sound m_roundSound = null;
    /// <summary>
    /// 백그라운드 오디오 소스
    /// </summary>
    private AudioSource m_backGroundSoundAS = null;
    /// <summary>
    /// 이펙트 오디오소스
    /// </summary>
    private AudioSource m_effectSoundAS = null;

    // Start is called before the first frame update
    void Start()
    {
        m_backGroundSoundAS = transform.Find("BackGroundSound").GetComponent<AudioSource>();
        m_effectSoundAS = transform.Find("EffectSound").GetComponent<AudioSource>();
    }

    public void PlayEffectSound(AudioClip argAudioClip)
    {
        m_effectSoundAS.PlayOneShot(argAudioClip);
    }

    public Sound GetSound
    {
        get { return m_roundSound; }
    }
}
