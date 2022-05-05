using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : BaseManager<SoundManager>
{
    AudioSource audioSource;

    [SerializeField] Sprite BtnMusicOn;
    [SerializeField] Sprite BtnMusicOff;

    bool IsMuted = false;
    [SerializeField] Button BtnMusic;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        BtnMusic.onClick.AddListener(()=>{
            SoundControl();
        });
    }

    public void SoundControl()
    {
        IsMuted = !IsMuted;
        audioSource.volume = (float)System.Convert.ToInt16(!IsMuted);

        BtnMusic.GetComponent<Image>().sprite = IsMuted ? BtnMusicOff : BtnMusicOn;
    }

    public void ChangeMusic(AudioClip music)
    {
        if (audioSource.clip != music)
        {
            audioSource.Stop();
            audioSource.clip = music;
            audioSource.Play();
        }
    }
}
