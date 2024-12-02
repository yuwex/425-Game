
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource sfxPrefab;
    public float volumeSFX;
    public float volumeMusic;

    public AudioClip music;
    public double loopLength;

    private AudioClip currentSong;
    private AudioSource[] sources = new AudioSource[2];
    private double loopPoint;
    private int flip = 0;

    void Awake()
    {
        if (Instance = null) Instance = this;
    }

    void Start()
    {
        PlayMusic(music);
    }

    public void PlaySFXClip(AudioClip clip, Transform transform, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position, volumeSFX * volume);
    }

    public void PlayMusic(AudioClip clip)
    {
        currentSong = clip;

        for (int i = 0; i < 2; i++)
        {
            sources[i] = Instantiate(sfxPrefab, Vector3.zero, Quaternion.identity);
            sources[i].clip = clip;
        }
        
        loopPoint = AudioSettings.dspTime + 2;
    }

    void Update()
    {
        if (!currentSong) return;

        sources[0].volume = volumeMusic;
        sources[1].volume = volumeMusic;

        double time = AudioSettings.dspTime;

        if (time + 1 > loopPoint)
        {
            sources[flip].PlayScheduled(loopPoint);
            loopPoint += loopLength;

            flip = 1 - flip;
        }
    }


}
