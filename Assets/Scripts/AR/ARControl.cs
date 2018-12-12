using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARControl : MonoBehaviour {

    public static ARControl Instance;

    //timer for end event 
    private float timer = 0;

    //name unload scene
    private string ARScene = "ARScene";

    //static var for effect after end event AR
    public static int arPoints = 0;
    public static int arHits = 0;

    //Prefabs
    public GameObject coin;
    public GameObject bomb;
    public GameObject heart;
    public GameObject heartHit;
    public GameObject arrow;
    public GameObject arrowHit;
    public GameObject explosion;
    public GameObject hit;

    //Name current player
    public Text playerNamePanel;

    //Start event var
    private static bool eventStart = true;
    
    //Check ARTarget is ON
    public static bool ARTargetFind = false;
    
    Vector3 posOld = new Vector3(0, 0, 0);

    //variable informing which event has started
    private static int GameMode = 0;

    //check event end
    private bool GameEnd = false;
    

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //Set Player Name
        playerNamePanel.text = GameManager.Instance.currentPlayer.name;
    }
	
	// Update is called once per frame
	void Update () {
        
        GameObject[] findDagger = GameObject.FindGameObjectsWithTag("ImgDagger");
        Vector3 posNow = new Vector3 (0, 0, 0);
        if (GameMode == 1) {posNow = findDagger[1].transform.position; }
        if(GameMode == 2) {posNow = findDagger[0].transform.position; }


        //randomize the game
        if (eventStart == true)
        {
            int game = (int)Random.Range(0f, 7f);
            if(game <= 3) {
                print("Event: Collecting Coins");
                SpawnRandomCoinsAndBombs();
                eventStart = false;
                GameMode = 1;
            }
            if(game >= 4)
            {
                print("Event: Defense against arrows");
                SpawnArrowsAndHeart();
                eventStart = false;
                GameMode = 2;
            }
            
            
        }
        //After press 9 -Back and add money
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            AugmentedRealitySystem.Instance.ARSceneResult(true, GameMode);
            SceneManager.UnloadScene(ARScene);
            DestroyOtherARElements();
            GameEnd = true;
        }

        //After press 8 -Back and no bonus money
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            AugmentedRealitySystem.Instance.ARSceneResult(false, GameMode);
            SceneManager.UnloadScene(ARScene);
            DestroyOtherARElements();
            GameEnd = true;
        }

        //Check  ARTarget is ON
        if(posOld != posNow) {
            if (GameMode == 1)
            {
                timer += Time.deltaTime;
                print("Time:" + timer);
            }
            //print("NOW:"+ posNow.x + "|" + posNow.y + "|" + posNow.z);
            //print("OLD:"+ posOld.x + "|" + posOld.y + "|" + posOld.z);
            posOld = posNow;
            ARTargetFind = true;
        }
        else
        {
            ARTargetFind = false;
        }


        //After time go back
        if (timer > 10)
        {
            AugmentedRealitySystem.Instance.ARSceneResult(true, GameMode);
            SceneManager.UnloadScene(ARScene);
            DestroyOtherARElements();
            GameEnd = true;
        }
        else
        {
            if (GameEnd == false)
            {
                StartCoroutine(turnOffEvent());
            }
        } 
    }

    //Start event 2 and spawn games objects
    void SpawnArrowsAndHeart()
    {

        //heart
        Vector3 heartPosition = new Vector3(0f,-1f,10f);
        Instantiate(heart, heartPosition, Quaternion.identity);

        //arrow1
        Vector3 apos1 = new Vector3(-4f, 2f, 10f);
        GameObject arr1 = Instantiate(arrow, apos1, Quaternion.identity);
        arr1.transform.Rotate(0, 0, 135);

        //arrow2
        Vector3 apos2 = new Vector3(4f, 2f, 10f);
        GameObject arr2 = Instantiate(arrow, apos2, Quaternion.identity);
        arr2.transform.Rotate(0, 180, -225);

        //arrow3
        Vector3 apos3 = new Vector3(-4f, -1f, 10f);
        GameObject arr3 = Instantiate(arrow, apos3, Quaternion.identity);
        arr3.transform.Rotate(0, 180, 0);

        //arrow4
        Vector3 apos4 = new Vector3(0f, 2f, 10f);
        GameObject arr4 = Instantiate(arrow, apos4, Quaternion.identity);
        arr4.transform.Rotate(0, 180, 90);
        
        //arrow5
        Vector3 apos5 = new Vector3(4f, -1f, 10f);
        GameObject arr5 = Instantiate(arrow, apos5, Quaternion.identity);
        arr5.transform.Rotate(0, 180, 180);
        
    }

    //Start event 1 and spawn games objects
    void SpawnRandomCoinsAndBombs() {

        //static position z
        float posZ = 10;

        //spawn places
        Vector3[] spawn0 = new Vector3[5] { new Vector3(4.39f, 1.8f, posZ), new Vector3(-4.47f, 1.8f, posZ),    new Vector3(4.02f, 0.44f, posZ), new Vector3(3.37f, 1.58f, posZ), new Vector3(-3.6f, 1.48f, posZ) };
        Vector3[] spawn1 = new Vector3[5] { new Vector3(2.25f, 1.8f, posZ), new Vector3(-2.97f, 0.72f, posZ),   new Vector3(3.87f, -1.35f, posZ), new Vector3(-3.63f, 0.01f, posZ), new Vector3(1.59f, 1.29f, posZ) };
        Vector3[] spawn2 = new Vector3[5] { new Vector3(-4.3f, 1.8f, posZ), new Vector3(-3.84f, 1.32f, posZ),   new Vector3(-3.33f, 0.79f, posZ), new Vector3(-4.01f, 0.23f, posZ), new Vector3(-2.68f, 1.39f, posZ) };
        Vector3[] spawn3 = new Vector3[5] { new Vector3(0f,1.8f ,posZ),     new Vector3(-0.78f, 1.68f, posZ),   new Vector3(0.74f, 1.65f, posZ), new Vector3(1.43f, 1.27f, posZ), new Vector3(-1.58f, 1.27f, posZ) };
        Vector3[] spawn4 = new Vector3[5] { new Vector3(3.27f, 1.8f, posZ), new Vector3(-1.89f, 0.63f, posZ),   new Vector3(0.74f, 1.65f, posZ), new Vector3(1.43f, 1.27f, posZ), new Vector3(-3.21f, 1.27f, posZ) };
        Vector3[] spawn5 = new Vector3[5] { new Vector3(3.89f, 1.8f, posZ), new Vector3(2.21f, 1.8f, posZ), new Vector3(3.12f, -0.1f, posZ), new Vector3(3.06f, 0.93f, posZ), new Vector3(3.08f, -1.45f, posZ) };
        Vector3[] spawn6 = new Vector3[5] { new Vector3(4.34f, 1.8f, posZ), new Vector3(3.27f, 1.8f, posZ), new Vector3(4.36f, 0.68f, posZ), new Vector3(-4.44f, 1.57f, posZ), new Vector3(4.4f, -1.71f, posZ) };
        Vector3[] spawn7 = new Vector3[5] { new Vector3(4.34f, 1.8f, posZ), new Vector3(-4.41f, 1.8f, posZ), new Vector3(4.36f, -1.76f, posZ), new Vector3(-4.44f, 0.16f, posZ), new Vector3(4.39f, -0.05f, posZ) };
        List<Vector3[]> spawnPoints = new List<Vector3[]> { spawn0, spawn1, spawn2, spawn3, spawn4, spawn5, spawn6, spawn7 };

        //randomize spawn
        int option = (int)Random.Range(0f, 7f);
        print("Spawn Option :" + option);

        //create objects on scene
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

    //function plaing animation
   public void PlayAnimation(Vector3 position, string animation) {

        if (animation == "explosion")
        {
            GameObject exp = Instantiate(explosion, position, Quaternion.identity);
            exp.transform.Rotate(-90, 0, 0);
        }

        if (animation == "hit")
        {
            Instantiate(hit, position, Quaternion.identity);
        }

        if (animation == "hitshield")
        {
            Instantiate(arrowHit, position, Quaternion.identity);
        }

        if (animation == "hitheart")
        {
            Instantiate(heartHit, position, Quaternion.identity);
        }
    }

    //destory after event all AR objects
    void DestroyOtherARElements() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Coin");

        foreach (GameObject enemy in gameObjects)
            GameObject.Destroy(enemy);

        gameObjects = GameObject.FindGameObjectsWithTag("Bomb");

        foreach (GameObject enemy in gameObjects)
            GameObject.Destroy(enemy);

        gameObjects = GameObject.FindGameObjectsWithTag("ARA");
        foreach (GameObject enemy in gameObjects)
            GameObject.Destroy(enemy);

        gameObjects = GameObject.FindGameObjectsWithTag("Heart");
        foreach (GameObject enemy in gameObjects)
            GameObject.Destroy(enemy);

        gameObjects = GameObject.FindGameObjectsWithTag("Arrow");
        foreach (GameObject enemy in gameObjects)
            GameObject.Destroy(enemy);

        GameMode = 0;
        eventStart = true;
    }

    //main function turn off event
    public IEnumerator turnOffEvent() {
        if (GameMode == 1) {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Coin");
            if(gameObjects.Length < 1)
            {

                yield return new WaitForSeconds(2);
                AugmentedRealitySystem.Instance.ARSceneResult(true, GameMode);
                SceneManager.UnloadScene(ARScene);
                DestroyOtherARElements();
            }
        }

        if (GameMode == 2)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Arrow");
            if (gameObjects.Length < 1)
            {

                yield return new WaitForSeconds(2);
                AugmentedRealitySystem.Instance.ARSceneResult(true, GameMode);
                SceneManager.UnloadScene(ARScene);
                DestroyOtherARElements();
            }
        }

    }

}
