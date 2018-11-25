using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager Instance;

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


    public const float DELAYED_TIME_BETWEEN_UNIT_DEATH_AND_OBJECT_DESTROY = 2f;

    //Data From SettsHoldera
    public GameMode typeOfEnemy;

    public Faction southFaction;
    public Faction northFaction;

    public string northName;
    public string southName;

    public HybridEffectsSystem hybridEffectsSystem;
    public AudioGenerator audioGenerator;
    public EndTurnButtonManager endTurnButtonManager;
    public SpeechRecognitionSystem speechRecognition;
    public DebugMessege debugMessageBox;

    public int turnNumber = 0;

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
            enablePlayableCardsFlag = false;
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
                UnblockAllUnitsAndCards(playerSouth, southHandView, dropZoneSouth);
            }
            if (currentPlayer == playerNorth)
            {
                UnblockAllUnitsAndCards(playerNorth, northHandView, dropZoneNorth);
            }
            enablePlayableCardsFlag = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ARManager.Instance.goToARScene();
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
            BonusEffects.Instance.drawNewCard(playerSouth, false);
        }

        //// ----------draw 4 cards from deck to Player North
        for (int i = 0; i < 2; i++)
        {
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);
            }
            BonusEffects.Instance.drawNewCard(playerNorth, false);
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
        turnNumber++;

        Debug.Log("Tura nr " + turnNumber);
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

        BlackAllUnitsAndCards();
        messageManager.ShowMessage(currentPlayer.name + " \nTwoja tura!", 2f);
        currentPlayer.updateResourcesNewTurn();

        if (currentPlayer == playerSouth)
        {
            resourcesSouth.GetComponent<ResourcePool>().updateResourcesView(playerSouth.resourcesCurrent, playerSouth.resourcesMaxThisTurn);
            resourcesSouth.GetComponent<ResourcePool>().ProgressText.color = new Color32(0, 0, 0, 255);
            BonusEffects.Instance.drawNewCard(playerSouth, true);

            //going to replace it with hybridEffectsSystem
            speechRecognition.CheckWhetherToShowSpeechSign();
        }
        if (currentPlayer == playerNorth)
        {
            resourcesNorth.GetComponent<ResourcePool>().updateResourcesView(playerNorth.resourcesCurrent, playerNorth.resourcesMaxThisTurn);
            resourcesNorth.GetComponent<ResourcePool>().ProgressText.color = new Color32(0, 0, 0, 255);
            BonusEffects.Instance.drawNewCard(playerNorth, true);
        }

        endTurnButtonManager.TimerStart();
        currentPlayer.armymodel.armyCardsModel.EnableAttackOfJustPlacedUnits(currentPlayer);
    }

    void InitializeGame()
    {
        Debug.Log("GameManger INITIALIZATION");
        Instance.debugMessageBox.ShowDebugText("Gra Inicjalizowana", true);
        
        gameRunning = true;
        IDFactory.ResetIDs();

        //Attribute factions, names, and mode of game
        SettsHolder.instance.AttributeGameManagerNamesAndFactions();

        Debug.Log(typeOfEnemy);

        playerNorth = new PlayerModel(0, northName, northFaction, Position.North);
        playerSouth = new PlayerModel(1, southName, southFaction, Position.South);
        heroSouth.GetComponent<HeroVisualManager>().setHeroAcordingToFaction(southFaction);
        heroNorth.GetComponent<HeroVisualManager>().setHeroAcordingToFaction(northFaction);
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

    public void BlackAllUnitsAndCards()
    {
        northHandView.blockAllOperations();
        southHandView.blockAllOperations();
        dropZoneNorth.blockAllUnitOperations();
        dropZoneSouth.blockAllUnitOperations();
    }

    public void UnblockAllUnitsAndCards(PlayerModel player, HandView hand, DropZone drop)
    {
        player.armymodel.armyCardsModel.restoreCardAttacksPerRound();
        hand.setPlayableCards(player.resourcesCurrent);
        drop.unlockUnitAttacks();
    }

    public void StartGameWithCouroutine()
    {
        StartCoroutine(startGame());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public IEnumerator CheckWhetherToKillUnitAfterBonusWithCoroutine(Defendable defenderCard, int defenderID, int defenderArmor)
    {
        //Update armor in model, and if defender dead then update model and delete card from view
        if (defenderArmor <= 0)
        {
            GameManager.Instance.otherPlayer.armymodel.armyCardsModel.moveCardFromFrontToGraveyard(defenderID);
            // TODO: make below const global and without duplicates
            yield return new WaitForSeconds(DELAYED_TIME_BETWEEN_UNIT_DEATH_AND_OBJECT_DESTROY);
            Destroy(defenderCard.gameObject);
        }
    }
}
