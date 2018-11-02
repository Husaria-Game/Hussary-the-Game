using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
    // SINGLETON
    public static GameManager Instance;

    public string NorthName = "AI";
    public string SouthName = "Grzegorz";
    //public Player player;
    public PlayerModel playerSouth;
    public PlayerModel playerNorth;
    public MessageManager messageManager;
    public HandView northHandView;
    public HandView southHandView;
    public GameObject unitCard;
    public GameObject tacticsCard;
    public GameObject visuals;
    public GameObject deckNorth;
    public GameObject deckSouth;
    public GameObject resourcesNorth;
    public GameObject resourcesSouth;


    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    IEnumerator Start()
    {
        while (messageManager.enabled == false)
        {
            yield return new WaitForSeconds(0.05f);
        }
        InitializeGame();
        messageManager.playerSouthName = SouthName;


        //// ----------draw 4 cards from deck to Player South
        for(int i=0;i<4; i++)
        {            
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);             
            }

            Card cardDrawn = playerSouth.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
            southHandView.MoveDrawnCardFromDeckToHand(cardDrawn, playerSouth, deckSouth);
        }

        //// ----------draw 4 cards from deck to Player North
        for (int i = 0; i < 4; i++)
        {
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);
            }

            Card cardDrawn = playerNorth.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
            northHandView.MoveDrawnCardFromDeckToHand(cardDrawn, playerNorth, deckNorth);
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        southHandView.SetPlayableCards(playerSouth.resourcesCurrent);
        northHandView.SetPlayableCards(playerNorth.resourcesCurrent);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitializeGame()
    {
        Debug.Log("GameManger INITIALIZATION");
        IDFactory.ResetIDs();
        this.playerNorth = new PlayerModel(0, "Cooper", Faction.Ottoman, Position.North);
        this.playerSouth = new PlayerModel(1, "Johnson", Faction.Ottoman, Position.South);
        resourcesNorth.GetComponent<ResourcePool>().ResourceLeft = playerNorth.resourcesCurrent;
        resourcesNorth.GetComponent<ResourcePool>().ResourceMax = playerNorth.resourcesMaxThisTurn;
        resourcesSouth.GetComponent<ResourcePool>().ResourceLeft = playerSouth.resourcesCurrent;
        resourcesSouth.GetComponent<ResourcePool>().ResourceMax = playerSouth.resourcesMaxThisTurn;
        resourcesNorth.GetComponent<ResourcePool>().updateResources();
        resourcesSouth.GetComponent<ResourcePool>().updateResources();
    }
}
