#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class SoundManagerMono:MonoBehaviour
{
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource seAudioSource;

    [SerializeField] List<BGMSoundData> bgmSoundDatas;
    [SerializeField] List<SESoundData> seSoundDatas;

    private Dictionary<SESoundData.SE, AudioSource> loopingSEAudioSources = new();
    
    public float masterVolume = 1;
    public float bgmMasterVolume = 1;
    public float seMasterVolume = 1;

    public static SoundManagerMono Instance { get; private set; }
        
    void Awake()
    {
        Instance = this;
    }

    void OnDestroy()
    {
        Instance = null;
    }

    public void SetSeVolume(float volume)
    {
        seMasterVolume = volume;
    }

    public void PlayBGM(BGMSoundData.BGM bgm)
    {
        BGMSoundData data = bgmSoundDatas.Find(data => data.bgm == bgm);
        bgmAudioSource.clip = data.audioClip;
        bgmAudioSource.volume = data.volume * bgmMasterVolume * masterVolume;
        bgmAudioSource.Play();
    }
    public void StopBGM(BGMSoundData.BGM bgm)
    {
        BGMSoundData data = bgmSoundDatas.Find(data => data.bgm == bgm);
        bgmAudioSource.clip = data.audioClip;
        bgmAudioSource.volume = data.volume * bgmMasterVolume * masterVolume;
        bgmAudioSource.Stop();
    }

    private Dictionary<SESoundData.SE, float> playingSETimes = new();

    public void PlaySEOneShot(SESoundData.SE se)
    {
        if (playingSETimes.TryGetValue(se, out float lastPlayTime))
        {
            // 既に再生中なら再生しない（短すぎる間隔の連続再生を防ぐ）
            if (Time.time - lastPlayTime < 0.1f) return;
        }

        SESoundData data = seSoundDatas.Find(data => data.se == se);
        if (data == null) return;

        seAudioSource.volume = data.volume * seMasterVolume * masterVolume;
        seAudioSource.PlayOneShot(data.audioClip);

        // 再生時間を記録
        playingSETimes[se] = Time.time;
    }

    public void PlayLoopingSE(SESoundData.SE se)
    {
        if (loopingSEAudioSources.ContainsKey(se)) return;

        SESoundData? data = seSoundDatas.Find(data => data.se == se);
        if (data == null) return;

        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = data.audioClip;
        newAudioSource.volume = data.volume * seMasterVolume * masterVolume;
        newAudioSource.loop = true;
        newAudioSource.Play();

        loopingSEAudioSources[se] = newAudioSource;
    }

    public void StopLoopingSE(SESoundData.SE se)
    {
        if (!loopingSEAudioSources.TryGetValue(se, out AudioSource? audioSource)) return;

        audioSource.Stop();
        Destroy(audioSource);
        loopingSEAudioSources.Remove(se);
    }

    public void StopAllLoopSE()
    {
        foreach (var audio in loopingSEAudioSources.Values)
        {
            audio.mute = true;
        }
    }
}

[System.Serializable]
public class BGMSoundData
{
    public enum BGM
    {
        GameBgm,
    }

    public BGM bgm;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}

[System.Serializable]
public class SESoundData
{
    public enum SE
    {
        Action,
        Miss,
        Clear,
        Check
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
    }