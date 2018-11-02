//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class GlobalSettings : MonoBehaviour
//{
//    // SINGLETON
//    public static GlobalSettings Instance;

//    public string NorthName = "AI";
//    public string SouthName = "Grzegorz";
//    public GameManager gameManager;
//    public PlayerModel playerSouth;
//    public PlayerModel playerNorth;
//    public MessageManager messageManager;
//    public HandView northHandView;
//    public HandView southHandView;
//    public GameObject unitCard;
//    public GameObject tacticsCard;
//    public GameObject visuals;
//    public GameObject deckNorth;
//    public GameObject deckSouth;
//    public GameObject resourcesNorth;
//    public GameObject resourcesSouth;
//    public GameObject dropzoneNorth;
//    public GameObject dropzoneSouth;
//    public Position whoseTurn;
//    public bool gameRunning;


//    void Awake()
//    {
//        Instance = this;
//    }

//    //IEnumerator Start()
//    //{
//    //    while (gameManager.enabled == false)
//    //    {
//    //        yield return new WaitForSeconds(0.05f);
//    //    }
//    //    gameManager.startGame();
//    //}
//}
