using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager Instance;

    public string northName = "Gracz 2";
    public string southName = "Gracz 1";
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
    public bool isAttackableDraggingActive;

    //Skrypty czytające dane z menu (imiona i frakcje) - MultiPlayer
    public ChooseFactionForFirstPlayer chooseFactionForFirstPlayer;
    public ChooseFactionForSecondPlayer chooseFactionForSecondPlayer;

    //Skrypty czytające dane z menu (imiona i frakcje) - SinglePlayer
    public ChooseFactionForFirstPlayer chooseFactionForFirstPlayerSingleMode;
    public ChooseFactionForAIPlayer chooseFactionForAIPlayer;

    //Skrypt upływającego czasu
    public EndTurnButtonManager endTurnButtonManager;



    public Faction firstFaction;
    public Faction secondFaction;

    //For test purpose only
    public SpeechRecognitionSystem speechRecognition;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        visuals.SetActive(false);
        mainMenu.SetActive(true);
        enablePlayableCardsFlag = false;
        isAttackableDraggingActive = false;
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
            drawNewCardPlayerSouth();
        }

        //// ----------draw 4 cards from deck to Player North
        for (int i = 0; i < 2; i++)
        {
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);
            }
            drawNewCardPlayerNorth();
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
            
            drawNewCardPlayerSouth();
            
            playerSouth.armymodel.armyCardsModel.restoreCardAttacksPerRound();
            southHandView.setPlayableCards(playerSouth.resourcesCurrent);
            dropZoneSouth.unlockUnitAttacks();

            endTurnButtonManager.TimerStart();

            //For Testing purpose only
            speechRecognition.CheckWhetherToShowSpeechSign();

            /////////////////////
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

            drawNewCardPlayerNorth();

            playerNorth.armymodel.armyCardsModel.restoreCardAttacksPerRound();
            northHandView.setPlayableCards(playerNorth.resourcesCurrent);
            dropZoneNorth.unlockUnitAttacks();
        }
    }

    void InitializeGame()
    {
        Debug.Log("GameManger INITIALIZATION");
        gameRunning = true;
        IDFactory.ResetIDs();

        //Dodane przypiasanie frakcji - Na razie tylko tryb Multiplayer - potem trzeba wprowadić zmienną wybierającą tryb
        attributeNamesAndFactions();

        playerNorth = new PlayerModel(0, northName, secondFaction, Position.North);
        playerSouth = new PlayerModel(1, southName, firstFaction, Position.South);
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

    public void attributeNamesAndFactions()
    {
        //Factions
        firstFaction = chooseFactionForFirstPlayer.getFirstFaction();
        secondFaction = chooseFactionForSecondPlayer.getSecondFaction();

        //Names
        southName = chooseFactionForFirstPlayer.getFirstPlayersName();
        northName = chooseFactionForSecondPlayer.getSecondPlayersName();
        if (string.IsNullOrEmpty(southName)) southName = "Gracz 1";
        if (string.IsNullOrEmpty(northName)) northName = "Gracz 2";
    }

    public void StartGameWithCouroutine()
    {
        StartCoroutine(startGame());
    }

    public void drawNewCardPlayerNorth()
    {
        Card cardDrawn = playerNorth.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
        northHandView.MoveDrawnCardFromDeckToHand(cardDrawn, playerNorth, deckNorth);
    }

    public void drawNewCardPlayerSouth()
    {
        Card cardDrawn = playerSouth.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
        southHandView.MoveDrawnCardFromDeckToHand(cardDrawn, playerSouth, deckSouth);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
