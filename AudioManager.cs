using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] MusicSounds, SfxSounds;
    public AudioSource MusicSource, SfxSource;

    private Dictionary<string, Sound> musicDictionary;
    private Dictionary<string, Sound> sfxDictionary;

    public AudioSource runSource;
    public AudioSource ChargeOverSource;
    public AudioSource ChargingSource;
    public AudioSource BossMove;
    public AudioSource BossEnrageMove;
    public float fadeTime = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSoundDictionaries();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSoundDictionaries()
    {
        musicDictionary = new Dictionary<string, Sound>();
        sfxDictionary = new Dictionary<string, Sound>();

        foreach (Sound s in MusicSounds)
        {
            musicDictionary[s.name] = s;
        }

        foreach (Sound s in SfxSounds)
        {
            sfxDictionary[s.name] = s;
        }
    }

    private void Start()
    {
        Playmusic("BGM");
    }

    public void Playmusic(string name)
    {
        if (musicDictionary.TryGetValue(name, out Sound s))
        {
            MusicSource.clip = s.clip;
            MusicSource.Play();
        }
        else
        {
            Debug.Log("Sound not found: " + name);
        }
    }

    public void PlaySFX(string name)
    {
        if (sfxDictionary.TryGetValue(name, out Sound s))
        {
            SfxSource.PlayOneShot(s.clip);
        }
        else
        {
            Debug.Log("Sound not found: " + name);
        }
    }
    public void ChangeMusic(string name)
    {
        Sound s = Array.Find(MusicSounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        if (MusicSource.clip == s.clip)
        {
            return; 
        }
        MusicSource.clip = s.clip;
        MusicSource.Play();
    }
    public void StartRunSound(string name)
    {
        Sound s = Array.Find(SfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        if (!runSource.isPlaying)
        {
            runSource.clip = s.clip;
            runSource.loop = true;
            runSource.Play();
        }
    }
    public void StopRunSound()
    {
        runSource.Stop();
    }
    public void StartChargingSound(string name)
    {
        Sound s = Array.Find(SfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        if (!ChargingSource.isPlaying)
        {
            ChargingSource.clip = s.clip;
            ChargingSource.loop = true;
            ChargingSource.Play();
        }
    }
    public void StopChargingSound()
    {
        ChargingSource.Stop();
    }
    public void StartChargeOverSound(string name)
    {
        Sound s = Array.Find(SfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        if (!ChargeOverSource.isPlaying)
        {
            ChargeOverSource.clip = s.clip;
            ChargeOverSource.loop = true;
            ChargeOverSource.Play();
        }
    }
    public void StopChargeOverSound()
    {
        ChargeOverSource.Stop();
    }
    public void StartBossRunSound(string name)
    {
        Sound s = Array.Find(SfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        if (!BossMove.isPlaying)
        {
            BossMove.clip = s.clip;
            BossMove.loop = true;
            BossMove.Play();
        }
    }
    public void StopBossRunSound()
    {
        BossMove.Stop();
    }
    public void StartBossEnragedRunSound(string name)
    {
        Sound s = Array.Find(SfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        if (!BossEnrageMove.isPlaying)
        {
            BossEnrageMove.clip = s.clip;
            BossEnrageMove.loop = true;
            BossEnrageMove.Play();
        }
    }
    public void StopBossEnragedRunSound()
    {
        BossEnrageMove.Stop();
    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public IEnumerator FadeIn(AudioSource audioSource, AudioClip newClip, float FadeTime)
    {
        audioSource.clip = newClip;
        audioSource.Play();
        audioSource.volume = 0f;
        float targetVolume = 0.1f; // 根据需要设置目标音量

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }
    }
    public void PauseMusic()
    {
        if (MusicSource.isPlaying)
            MusicSource.Pause();
    }

    // 恢复播放背景音乐
    public void ResumeMusic()
    {
        if (!MusicSource.isPlaying)
            MusicSource.Play();
    }
}

