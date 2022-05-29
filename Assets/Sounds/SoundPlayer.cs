using System;
using UnityEngine;

[Serializable]
public class Sound
{
    public string Name;
    public AudioClip AudioClip;
}

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    Sound[] sounds;

    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        AudioClip clip = FindAudioClipByName(name);
        audioSource.PlayOneShot(clip);
    }

    private AudioClip FindAudioClipByName(string name)
    {
        foreach (var sound in sounds)
        {
            if (sound.Name.Equals(name))
                return sound.AudioClip;
        }

        return null;
    }
}
