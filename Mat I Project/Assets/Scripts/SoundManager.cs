using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance { get { return instance; } }

    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private AudioSource soundBackgroundMusic;

    [SerializeField] private bool isBackgroundMusicON = false;
    public bool IsBackgroundMUsicON {  get { return isBackgroundMusicON; } }

    [SerializeField] private bool isSoundSFXON = false;
    public bool IsSoundSFXON { get { return isSoundSFXON; } }

    [SerializeField][Range(0f, 1f)] private float backgroundMusicvolume = 0.5f;
    [SerializeField][Range(0f, 1f)] private float soundSFXVolume = 1f;

    public SoundType[] sounds;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlayBackgroundMusic(Sounds sound)
    {
        if (!isBackgroundMusicON) return;

        AudioClip clip = GetSoundClip(sound);

        if (clip != null)
        {
            soundBackgroundMusic.clip = clip;
            soundBackgroundMusic.loop = true;
            soundBackgroundMusic.Play();
        }
        else
        {
            Debug.LogError("Clip not found for the sound type: " + sound);
        }
    }


    public void Play(Sounds sound)
    {
        if (!isSoundSFXON) return;

        AudioClip clip = GetSoundClip(sound);

        if (clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Clip not found for the sound type: " + sound);
        }
    }


    private AudioClip GetSoundClip(Sounds sound)
    {
        SoundType item = Array.Find(sounds, i => i.soundType == sound);

        if (item != null)
        {
            return item.soundClip;
        }
        else
        {
            return null;
        }
    }


    public void TurnONBackgroundMusic(bool condition)
    {
        isBackgroundMusicON = condition;
        soundBackgroundMusic.volume = condition ? backgroundMusicvolume : 0;
    }


    public void TurnONSoundsSFX(bool condition)
    {
        isSoundSFXON = condition;   
        soundEffect.volume = condition ? soundSFXVolume : 0;
    }
}




[Serializable]
public class SoundType
{
    public Sounds soundType;
    public AudioClip soundClip;
}


public enum Sounds
{
    BUTTON_CLICK,
    MASS_GAINER_COLLECTED,
    MASS_BURNER_COLLECTED,
    PAUSE,
    GAME_OVER,
    BACKGROUND_MUSIC
}
