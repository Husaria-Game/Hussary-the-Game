using UnityEngine;

public class AudioGeneratorGeneral : MonoBehaviour
{

    public AudioClip audioClip;
    private AudioSource source;

    void Awake()
    {
        source = AddAudio(audioClip, false);
    }

    public void playClip()
    {
        source.Play();
    }

    public AudioSource AddAudio(AudioClip audioClip, bool loop)
    {
        AudioSource newAudio = new AudioSource();
        newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = audioClip;
        return newAudio;
    }
}