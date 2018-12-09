using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioGenerator : MonoBehaviour {

    [Header("Themes")]
    public AudioClip bogurodzica;
    public AudioClip battleTheme;
    [Header("Hybrid General")]
    public AudioClip effect;
    public AudioClip noEffect;
    [Header("ASR")]
    public AudioClip attack;
    public AudioClip powerUp;
    public AudioClip enhencement;
    public AudioClip cannon;
    public AudioClip heroHurt;
    [Header("AR")]
    public AudioClip coinGain;
    public AudioClip ARCoinHitClip;
    public AudioClip ARBombHitClip;
    public AudioClip ARHeartHitClip;
    public AudioClip ARArrowHitClip;
    [Header("AudioSources")]
    public AudioSource bogurodzicaAudio;
    public AudioSource battleThemeAudio;
    public AudioSource attackAudio;
    public AudioSource powerUpAudio;
    public AudioSource enhencementAudio;
    public AudioSource cannonAudio;
    public AudioSource heroHurtAudio;
    public AudioSource coinGainAudio;
    public AudioSource effectAudio;
    public AudioSource noEffectAudio;
    public AudioSource buttonAudio;
    public AudioSource ARCoinHitAudio;
    public AudioSource ARBombHitAudio;
    public AudioSource ARHeartHitAudio;
    public AudioSource ARArrowHitAudio;
    [Header("Other")]
    public GameObject visuals;
    private bool isMenuMusic = true;

    void Awake ()
    {
        bogurodzicaAudio = AddAudio(bogurodzica, false);
        battleThemeAudio = AddAudio(battleTheme, true);
        attackAudio = AddAudio(attack, false);
        powerUpAudio = AddAudio(powerUp, false);
        enhencementAudio = AddAudio(enhencement, false);
        cannonAudio = AddAudio(cannon, false);
        heroHurtAudio = AddAudio(heroHurt, false);
        coinGainAudio = AddAudio(coinGain, false);
        effectAudio = AddAudio(effect, false);
        noEffectAudio = AddAudio(noEffect, false);
        ARCoinHitAudio = AddAudio(ARCoinHitClip, false);
        ARBombHitAudio = AddAudio(ARBombHitClip, false);
        ARHeartHitAudio = AddAudio(ARHeartHitClip, false);
        ARArrowHitAudio = AddAudio(ARArrowHitClip, false);
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
                battleThemeAudio.loop = true;
                if (GameManager.Instance.isMusicInGamePlaying)
                {
                    battleThemeAudio.PlayOneShot(battleTheme, 0.1f);
                }
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
 }

