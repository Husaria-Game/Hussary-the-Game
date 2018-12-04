using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARControl : MonoBehaviour {

    public static ARControl Instance;

    private float timer = 0;
    private string ARScene = "ARScene";
    public static int arPoints = 0;
    public GameObject coin;
    public GameObject bomb;
    private bool eventStart = true;
    public AudioClip ARCoinHitClip;
    public AudioClip ARBombHitClip;
    public AudioSource ARCoinHitAudio;
    public AudioSource ARBombHitAudio;

    void Awake(){

        Instance = this;

        //Add audio
        ARCoinHitAudio = AddAudioAR(ARCoinHitClip, false);
        ARBombHitAudio = AddAudioAR(ARBombHitClip, false);
    }

        // Use this for initialization
        void Start () {
        //Set Player Name
        GameObject.Find("PlayerNameText").GetComponent<Text>().text = GameManager.Instance.currentPlayer.name;
        
    }
	
	// Update is called once per frame
	void Update () {

        if (eventStart == true)
        {
            SpawnRandomCoinsAndBombs();
            eventStart = false;
        }
        //After press 9 -Back and add money
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ARManager.Instance.ARSceneResult(true);
            SceneManager.UnloadScene(ARScene);
            DestroyOtherARElements();
        }

        //After press 8 -Back and no bonus money
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ARManager.Instance.ARSceneResult(false);
            SceneManager.UnloadScene(ARScene);
            DestroyOtherARElements();
        }

        
        //After time go back
        timer += Time.deltaTime;
        if (timer > 12)
        {
            ARManager.Instance.ARSceneResult(true);
            SceneManager.UnloadScene(ARScene);
            DestroyOtherARElements();
        }
        
    }

    void SpawnRandomCoinsAndBombs() {

        float posZ = 10;

        Vector3[] spawn0 = new Vector3[5] { new Vector3(4.39f, 1.8f, posZ), new Vector3(-4.47f, 1.8f, posZ),    new Vector3(4.02f, 0.44f, posZ), new Vector3(3.37f, 1.58f, posZ), new Vector3(-3.6f, 1.48f, posZ) };
        Vector3[] spawn1 = new Vector3[5] { new Vector3(2.25f, 1.8f, posZ), new Vector3(-2.97f, 0.72f, posZ),   new Vector3(3.87f, -1.35f, posZ), new Vector3(-3.63f, 0.01f, posZ), new Vector3(1.59f, 1.29f, posZ) };
        Vector3[] spawn2 = new Vector3[5] { new Vector3(-4.3f, 1.8f, posZ), new Vector3(-3.84f, 1.32f, posZ),   new Vector3(-3.33f, 0.79f, posZ), new Vector3(-4.01f, 0.23f, posZ), new Vector3(-2.68f, 1.39f, posZ) };
        Vector3[] spawn3 = new Vector3[5] { new Vector3(0f,1.8f ,posZ),     new Vector3(-0.78f, 1.68f, posZ),   new Vector3(0.74f, 1.65f, posZ), new Vector3(1.43f, 1.27f, posZ), new Vector3(-1.58f, 1.27f, posZ) };
        Vector3[] spawn4 = new Vector3[5] { new Vector3(3.27f, 1.8f, posZ), new Vector3(-1.89f, 0.63f, posZ),   new Vector3(0.74f, 1.65f, posZ), new Vector3(1.43f, 1.27f, posZ), new Vector3(-3.21f, 1.27f, posZ) };
        Vector3[] spawn5 = new Vector3[5] { new Vector3(3.89f, 1.8f, posZ), new Vector3(2.21f, 1.8f, posZ), new Vector3(3.12f, -0.1f, posZ), new Vector3(3.06f, 0.93f, posZ), new Vector3(3.08f, -1.45f, posZ) };
        Vector3[] spawn6 = new Vector3[5] { new Vector3(4.34f, 1.8f, posZ), new Vector3(3.27f, 1.8f, posZ), new Vector3(4.36f, 0.68f, posZ), new Vector3(-4.44f, 1.57f, posZ), new Vector3(4.4f, -1.71f, posZ) };
        Vector3[] spawn7 = new Vector3[5] { new Vector3(4.34f, 1.8f, posZ), new Vector3(-4.41f, 1.8f, posZ), new Vector3(4.36f, -1.76f, posZ), new Vector3(-4.44f, 0.16f, posZ), new Vector3(4.39f, -0.05f, posZ) };
        List<Vector3[]> spawnPoints = new List<Vector3[]> { spawn0, spawn1, spawn2, spawn3, spawn4, spawn5, spawn6, spawn7 };


        int option = (int)Random.Range(0f, 7f);
        print("Spawn Option :" + option);

        int counter = 0;
        foreach (Vector3 vector in spawnPoints[option]) {
            if(counter < 3) {
                Instantiate(coin, vector, Quaternion.identity);
            }
            if ((counter < 5) && (counter >= 3)) {
               
                GameObject newBomb = Instantiate(bomb, vector, Quaternion.identity);
                newBomb.transform.Rotate(-90, 0, 0);
            }
            counter += 1;
        }

    }

    void DestroyOtherARElements() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Coin");

        foreach (GameObject enemy in gameObjects)
            GameObject.Destroy(enemy);

        gameObjects = GameObject.FindGameObjectsWithTag("Bomb");

        foreach (GameObject enemy in gameObjects)
            GameObject.Destroy(enemy);
    }

    public AudioSource AddAudioAR(AudioClip audioClip, bool loop)
    {
        AudioSource newAudio = new AudioSource();
        newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = audioClip;
        return newAudio;
    }

    public void PlayAudioAR(AudioSource audioSource)
    {
        audioSource.Play();
    }



}
