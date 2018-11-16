using UnityEngine;

public class AudioGeneratorForBackGround : MonoBehaviour {

    public AudioClip Bogurodzica;
    public AudioClip BattleTheme;

    private AudioSource bogurodzicaAudio;
    private AudioSource battleThemeAudio;

    public GameObject visuals;
    private bool isMenuMusic = true;

    void Awake ()
    {
        bogurodzicaAudio = AddAudio(Bogurodzica, false);
        battleThemeAudio = AddAudio(BattleTheme, true);
    }

    void Start()
    {
        bogurodzicaAudio.Play();

    }

    void Update () {
        if (visuals.activeSelf)
        {
            bogurodzicaAudio.Stop();
            if (isMenuMusic)
            {
                battleThemeAudio.PlayOneShot(BattleTheme, 0.3f);
                isMenuMusic = false;
            }

        }
	}

   public AudioSource AddAudio(AudioClip audioClip, bool loop)
    {
        AudioSource newAudio = new AudioSource();
        newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = audioClip;
        return newAudio;
    }
 }

