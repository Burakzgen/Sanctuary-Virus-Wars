using UnityEngine;

public class AudioManager : SingleReference<AudioManager>
{
    [Header("Sound Settings")]
    public float masterVolume = 1.0f;
    public float musicVolume = 1.0f;
    public float sfxVolume = 1.0f;
    public float uiVolume = 1.0f;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource;

    [Header("Audio Clips")]
    public AudioClip gameBackgroundMusic;
    public AudioClip buttonClickSound;
    public AudioClip hoverClickSound;
    public AudioClip[] zombieSounds;

    protected override void Awake()
    {
        base.Awake();
        SetDontDestroyOnLoad(true);
    }
    private void Start()
    {
        PlayMusic(gameBackgroundMusic);
    }
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.volume = musicVolume * masterVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume * masterVolume);
    }

    public void PlayUI(AudioClip clip)
    {
        uiSource.PlayOneShot(clip, uiVolume * masterVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = musicVolume * masterVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }

    public void SetUIVolume(float volume)
    {
        uiVolume = volume;
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
        musicSource.volume = musicVolume * masterVolume;
        sfxSource.volume = sfxVolume * masterVolume;
        uiSource.volume = uiVolume * masterVolume;
    }
}
