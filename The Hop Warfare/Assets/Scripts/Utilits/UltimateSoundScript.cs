using UnityEngine;

public class UltimateSoundScript : MonoBehaviour
{
    AudioSource source;
    [Header("Звуки для Пистолета")]
    public AudioClip[] pistolShotSounds;
    public AudioClip[] pistolGrenadeSounds;
    public AudioClip[] grenadeExplodeSounds;

    [Header("Звуки для Дробовика")]
    public AudioClip[] shotgunShotSounds;
    public AudioClip[] shotgunSpecialShotSounds;
    public AudioClip[] loadingSounds;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PistolShotSounds()
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(pistolShotSounds[Random.Range(0, pistolShotSounds.Length)]);
    }

    public void PistolGrenadeSound()
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(pistolGrenadeSounds[Random.Range(0, pistolGrenadeSounds.Length)]);
    }

    public void GrenadeExplodeSounds()
    {
        source.pitch = Random.Range(0.8f, 1.2f);
        source.PlayOneShot(grenadeExplodeSounds[Random.Range(0, grenadeExplodeSounds.Length)]);
    }

    public void ShotgunShotSounds()
    {
        source.pitch = Random.Range(1f, 1.6f);
        source.PlayOneShot(shotgunShotSounds[Random.Range(0, shotgunShotSounds.Length)]);
    }

    public void LoadingSound(int soundNumber)
    {
        switch(soundNumber)
        {
            case 0:
                source.pitch = Random.Range(0.9f, 1.1f);
                source.PlayOneShot(loadingSounds[0]);
                break;
            case 1:
                source.pitch = Random.Range(0.9f, 1.1f);
                source.PlayOneShot(loadingSounds[1]);
                break;
            case 2:
                source.pitch = Random.Range(0.9f, 1.1f);
                source.PlayOneShot(loadingSounds[2]);
                break;
        }
    }
}
