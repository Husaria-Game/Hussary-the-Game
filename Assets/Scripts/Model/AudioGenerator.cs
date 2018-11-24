using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioGenerator : MonoBehaviour {

    [Header("Themes")]
    public AudioClip bogurodzica;
    public AudioClip battleTheme;
    [Header("Fight")]
    public AudioClip attack;
    public AudioClip powerUp;
    public AudioClip enhencement;
    public AudioClip cannon;
    public AudioClip heroHurt;
    [Header("ASR")]
    public AudioClip effect;
    public AudioClip noEffect;
    [Header("Button")]
    public AudioClip button;
    [Header("AudioSources")]
    public AudioSource bogurodzicaAudio;
    public AudioSource battleThemeAudio;
    public AudioSource attackAudio;
    public AudioSource powerUpAudio;
    public AudioSource enhencementAudio;
    public AudioSource cannonAudio;
    public AudioSource heroHurtAudio;
    public AudioSource effectAudio;
    public AudioSource noEffectAudio;
    public AudioSource buttonAudio;
    [Header("Other")]
    public GameObject visuals;
    private GameObject[] gameButtons;
    private List<Button> buttons = new List<Button>();
    private bool isMenuMusic = true;

    void Awake ()
    {
        gameButtons = GameObject.FindGameObjectsWithTag("Button");
        foreach(GameObject go in gameButtons)
        {
            buttons.Add(go.GetComponent<Button>());
        }

        bogurodzicaAudio = AddAudio(bogurodzica, false);
        battleThemeAudio = AddAudio(battleTheme, true);
        attackAudio = AddAudio(attack, false);
        powerUpAudio = AddAudio(powerUp, false);
        enhencementAudio = AddAudio(enhencement, false);
        cannonAudio = AddAudio(cannon, false);
        heroHurtAudio = AddAudio(heroHurt, false);
        effectAudio = AddAudio(effect, false);
        noEffectAudio = AddAudio(noEffect, false);
        buttonAudio = AddAudio(button, false);

    }

    void Start()
    {
        foreach(Button b in buttons)
        {
            Debug.Log(b.name);
            b.onClick.AddListener(SoundOnClick);
        }
        bogurodzicaAudio.Play();
    }

    void Update () {
        
        if (visuals.activeSelf)
        {
            bogurodzicaAudio.Stop();
            if (isMenuMusic)
            {
                battleThemeAudio.loop = true;
                battleThemeAudio.PlayOneShot(battleTheme, 0.1f);
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
    
    public void PlayClip(AudioSource audioSource) 
    {
        audioSource.Play();
    }

    public void SoundOnClick()
    {
        buttonAudio.Play();
    }   
 }

