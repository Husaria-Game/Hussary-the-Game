using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager Instance;

    public string NorthName = "AI";
    public string SouthName = "Grzegorz";
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
    public GameObject dropzoneNorth;
    public GameObject dropzoneSouth;
    public Position whoseTurn;
    public bool gameRunning;


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
        for (int i=0;i<2; i++)
        {            
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);             
            }

            Card cardDrawn = playerSouth.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
            southHandView.MoveDrawnCardFromDeckToHand(cardDrawn, playerSouth, deckSouth);
        }

        //// ----------draw 4 cards from deck to Player North
        for (int i = 0; i < 2; i++)
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

        northHandView.blockAllOperations();
        southHandView.blockAllOperations();
        whoseTurn = Position.North;
        this.nextTurn();
    }

    public void nextTurn()
    {
        if (whoseTurn == Position.North)
        {
            whoseTurn = Position.South;
        }
        else if (whoseTurn == Position.South)
        {
            whoseTurn = Position.North;
        }
        if (whoseTurn == Position.South)
        {
            northHandView.blockAllOperations();
            southHandView.blockAllOperations();
            messageManager.ShowMessage("Player South's Turn", 2f);
            playerSouth.updateResourcesNewTurn();
            resourcesSouth.GetComponent<ResourcePool>().updateResourcesView(playerNorth.resourcesCurrent, playerNorth.resourcesMaxThisTurn);
            southHandView.setPlayableCards(playerSouth.resourcesCurrent);
        }
        if (whoseTurn == Position.North)
        {
            northHandView.blockAllOperations();
            southHandView.blockAllOperations();
            messageManager.ShowMessage("Player North's Turn", 2f);
            playerNorth.updateResourcesNewTurn();
            resourcesNorth.GetComponent<ResourcePool>().updateResourcesView(playerNorth.resourcesCurrent, playerNorth.resourcesMaxThisTurn);
            northHandView.setPlayableCards(playerSouth.resourcesCurrent);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitializeGame()
    {
        Debug.Log("GameManger INITIALIZATION");
        gameRunning = true;
        IDFactory.ResetIDs();
        playerNorth = new PlayerModel(0, "Cooper", Faction.Ottoman, Position.North);
        playerSouth = new PlayerModel(1, "Johnson", Faction.Ottoman, Position.South);
        resourcesNorth.GetComponent<ResourcePool>().updateResourcesView(playerNorth.resourcesCurrent, playerNorth.resourcesMaxThisTurn);
        resourcesSouth.GetComponent<ResourcePool>().updateResourcesView(playerSouth.resourcesCurrent, playerSouth.resourcesMaxThisTurn);
    }

    public void cardDraggedToFrontCommand(Position playerPosition, int cardId)
    {
        if(playerPosition == Position.North)
        {
            playerNorth.armymodel.armyCardsModel.moveCardFromHandToFront(cardId);
        }
        else if (playerPosition == Position.South)
        {
            playerSouth.armymodel.armyCardsModel.moveCardFromHandToFront(cardId);
        }
    }
}
