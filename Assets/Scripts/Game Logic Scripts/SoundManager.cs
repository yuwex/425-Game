
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource sfxPrefab;
    public float volumeSFX = 1;
    public float volumeMusic = 0;

    void Awake()
    {
        if (Instance = null) Instance = this;
    }

    public void PlaySFXClip(AudioClip clip, Transform transform, float volume = 1f)
    {
        AudioSource audioSource = Instantiate(sfxPrefab, transform.position, Quaternion.identity);

        audioSource.clip = clip;
        audioSource.volume = volumeSFX * volume;

        audioSource.Play();

        Destroy(audioSource.gameObject, audioSource.clip.length);
    }




}
