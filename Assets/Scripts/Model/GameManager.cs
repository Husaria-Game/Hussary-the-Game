using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager Instance;

    public string northName;
    public string southName;
    public PlayerModel playerSouth;
    public PlayerModel playerNorth;
    public PlayerModel currentPlayer; //player that has active turn
    public PlayerModel otherPlayer; //player that has waits for his turn
    public MessageManager messageManager;
    public EndingMessege endingMessage;
    public HandView northHandView;
    public HandView southHandView;
    public GameObject unitCard;
    public GameObject tacticsCard;
    public GameObject visuals;
    public GameObject deckNorth;
    public GameObject deckSouth;
    public GameObject resourcesNorth;
    public GameObject resourcesSouth;
    public GameObject heroNorth;
    public GameObject heroSouth;
    public DropZone dropZoneNorth;
    public DropZone dropZoneSouth;
    public GameObject mainMenu;
    public bool gameRunning;
    public bool enablePlayableCardsFlag;

    //Skrypt upływającego czasu
    public EndTurnButtonManager endTurnButtonManager;

    public Faction southFaction;
    public Faction northFaction;

    public SpeechRecognitionSystem speechRecognition;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (SettsHolder.instance.GetIsPlayedAgain())
        {
            visuals.SetActive(true);
            mainMenu.SetActive(false);          
            StartGameWithCouroutine();
        }
        else
        {
            visuals.SetActive(false);
            mainMenu.SetActive(true);
            enablePlayableCardsFlag = false;
        }

    }

    void Update()
    {
        if (enablePlayableCardsFlag)
        {
            if (currentPlayer == playerSouth)
            {
                southHandView.setPlayableCards(playerSouth.resourcesCurrent);

            }
            if (currentPlayer == playerNorth)
            {
                northHandView.setPlayableCards(playerNorth.resourcesCurrent);
            }
            enablePlayableCardsFlag = false;
        }
    }

    // Use this for initialization
    IEnumerator startGame()
    {
        while (messageManager.enabled == false)
        {
            yield return new WaitForSeconds(0.05f);
        }
        InitializeGame();
        messageManager.playerSouthName = southName;


        //// ----------draw 4 cards from deck to Player South
        for (int i=0;i<2; i++)
        {            
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);             
            }
            drawNewCard(playerSouth, southHandView, deckSouth, false);
        }

        //// ----------draw 4 cards from deck to Player North
        for (int i = 0; i < 2; i++)
        {
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);
            }
            drawNewCard(playerNorth, northHandView, deckNorth, false);
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        northHandView.blockAllOperations();
        southHandView.blockAllOperations();
        currentPlayer = playerNorth;
        this.nextTurn();
    }

    public void nextTurn()
    {
        if (currentPlayer == playerNorth)
        {
            currentPlayer = playerSouth;
            otherPlayer = playerNorth;
        }
        else if (currentPlayer == playerSouth)
        {
            currentPlayer = playerNorth;
            otherPlayer = playerSouth;
        }
        if (currentPlayer == playerSouth)
        {
            northHandView.blockAllOperations();
            southHandView.blockAllOperations();
            dropZoneNorth.blockAllUnitOperations();
            dropZoneSouth.blockAllUnitOperations();
            messageManager.ShowMessage(southName + " \nTwoja tura!", 2f);
            playerSouth.updateResourcesNewTurn();
            resourcesSouth.GetComponent<ResourcePool>().updateResourcesView(playerSouth.resourcesCurrent, playerSouth.resourcesMaxThisTurn);

            drawNewCard(playerSouth, southHandView, deckSouth, true);

            playerSouth.armymodel.armyCardsModel.restoreCardAttacksPerRound();
            southHandView.setPlayableCards(playerSouth.resourcesCurrent);
            dropZoneSouth.unlockUnitAttacks();

            endTurnButtonManager.TimerStart();
            speechRecognition.CheckWhetherToShowSpeechSign();
        }
        if (currentPlayer == playerNorth)
        {
            northHandView.blockAllOperations();
            southHandView.blockAllOperations();
            dropZoneNorth.blockAllUnitOperations();
            dropZoneSouth.blockAllUnitOperations();
            messageManager.ShowMessage(northName + " \nTwoja tura!", 2f);
            playerNorth.updateResourcesNewTurn();
            resourcesNorth.GetComponent<ResourcePool>().updateResourcesView(playerNorth.resourcesCurrent, playerNorth.resourcesMaxThisTurn);

            drawNewCard(playerNorth, northHandView, deckNorth, true);

            playerNorth.armymodel.armyCardsModel.restoreCardAttacksPerRound();
            northHandView.setPlayableCards(playerNorth.resourcesCurrent);
            dropZoneNorth.unlockUnitAttacks();

            endTurnButtonManager.TimerStart();
        }
    }

    void InitializeGame()
    {
        Debug.Log("GameManger INITIALIZATION");
        gameRunning = true;
        IDFactory.ResetIDs();

        //Dodane przypiasanie frakcji - Na razie tylko tryb Multiplayer - potem trzeba wprowadić zmienną wybierającą tryb
        SettsHolder.instance.AttributeGameManagerNamesAndFactions();

        playerNorth = new PlayerModel(0, northName, northFaction, Position.North);
        playerSouth = new PlayerModel(1, southName, southFaction, Position.South);
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
    public void StartGameWithCouroutine()
    {
        StartCoroutine(startGame());
    }
    public void drawNewCard(PlayerModel playerModel, HandView handView, GameObject deck, bool isCoroutine)
    {
        if (isCoroutine)
        {
            StartCoroutine(drawNewCardWithDelay(playerModel, handView, deck));
        }
        else
        {
            Card cardDrawn = playerModel.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
            handView.MoveDrawnCardFromDeckToHand(cardDrawn, playerModel, deck);
        }

    }
    //Coroutines type of draw card method
    IEnumerator drawNewCardWithDelay( PlayerModel playerModel, HandView handView, GameObject deck)
    {
        yield return new WaitForSeconds(2f);
        Card cardDrawn = playerModel.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
        handView.MoveDrawnCardFromDeckToHand(cardDrawn, playerModel, deck);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
