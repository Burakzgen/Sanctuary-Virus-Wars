using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private const float volumeMultiplier = 0.85f;
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        InitializeAudioSource();
    }
    private void OnEnable()
    {
        AudioManager.Instance.OnVolumeChanged += UpdateVolume;
    }

    private void OnDisable()
    {
        AudioManager.Instance.OnVolumeChanged -= UpdateVolume;
        StopSound();
    }
    private void UpdateVolume(float volume)
    {
        audioSource.volume = volume * volumeMultiplier;
    }
    private void InitializeAudioSource()
    {
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 10f;
        UpdateVolume(AudioManager.Instance.sfxVolume * AudioManager.Instance.masterVolume);

    }

    public void PlaySound(AudioClip clip, bool loop = false)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;

        audioSource.Play();
    }

    public void StopSound()
    {
        audioSource.Stop();
    }

    public void PlayIdleSound()
    {
        AudioClip randomIdleSound = AudioManager.Instance.enemiesRandomClip[Random.Range(0, AudioManager.Instance.enemiesRandomClip.Length)];
        PlaySound(randomIdleSound, true);
    }

    public void PlayAttackSound()
    {
        PlaySound(AudioManager.Instance.attack);
    }

    public void PlayBiteSound()
    {
        PlaySound(AudioManager.Instance.bite);
    }

    public void PlayRunSound()
    {
        PlaySound(AudioManager.Instance.attack);
    }

    public void PlayDeathSound()
    {
        PlaySound(AudioManager.Instance.death);
    }


}