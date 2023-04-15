using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public int musicIndex;

    public AudioClip[] audioClips;
    public AudioSource audioSource;
    public AudioSource soundAudio;
    public Clip[] clips;

    public static float musicVolume = 0.1f;
    public static float soundVolume = 0.1f;
    public float pitch = 1;

    public static AudioController instance;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        musicIndex = Random.Range(0, audioClips.Length);

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = musicVolume;
        audioSource.pitch = pitch;

        StartCoroutine(PlayMusic());
    }
    public IEnumerator PlayMusic()
    {
        while (true)
        {
            if (!audioSource.isPlaying)
            {
                musicIndex = Random.Range(0, audioClips.Length);
                audioSource.clip = audioClips[musicIndex];
                audioSource.Play();
            }
            yield return new WaitForSeconds(3);
        }
    }
    public void Sound(AudioType audioType)
    {
        Clip clip = FindClip(audioType);
        soundAudio.clip = clip.audioClip;
        soundAudio.pitch = Random.Range(0.9f, 1.1f);
        soundAudio.volume = soundVolume;
        soundAudio.Play();
    }
    public void Sound(AudioType audioType, Transform startPoint, Transform endPoint)
    {
        float magnitude = (startPoint.position - endPoint.position).magnitude;

        Clip clip = FindClip(audioType);
        soundAudio.clip = clip.audioClip;

        soundAudio.pitch = Random.Range(0.9f, 1.1f);
        soundAudio.volume = Normalize(magnitude);
        soundAudio.Play();
    }
    private Clip FindClip(AudioType audioType)
    {
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips[i].audioType == audioType)
            {
                return clips[i];
            }
        }

        return null;
    }
    public void ChangeMusicVolume(float value)
    {
        musicVolume = value;
        audioSource.volume = musicVolume;
    }
    public void ChangeSoundVolume(float value)
    {
        soundVolume = value;
        audioSource.volume = musicVolume;
    }
    private float Normalize(float value)
    {
        if (value > 7)
        {
            return 0.3f;
        }
        else
        {
            float result = (10 - value) / 10;
            if (result > 0.9)
            {
                result = 0.9f;
            }
            return result;
        }
    }
}
[System.Serializable]
public class Clip
{
    public AudioClip audioClip;
    public AudioType audioType;
}
public enum AudioType
{
    CoinPickUp,
    TakeDamage,
    Win
}
