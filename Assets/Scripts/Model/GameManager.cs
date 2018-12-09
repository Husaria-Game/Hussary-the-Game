using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // SINGLETON
    public static GameManager Instance;

    //GameObjects and variables
    public PlayerModel playerSouth;
    public PlayerModel playerNorth;
    public PlayerModel currentPlayer;
    public PlayerModel otherPlayer;
    public MessageManager messageManager;
    public EndingMessege endingMessage;
    public GameObject unitCard;
    public GameObject tacticsCard;
    public GameObject visuals;
    public GameObject mainMenu;
    public bool enablePlayableCardsFlag;
    public bool isAttackableDraggingActive;
    public bool isItAITurn;

    //Data From SettsHolder - General
    public bool isARAvailable;
    public bool isASRAvailable;
    public bool aIPlayerCardsSeen;
    public bool isMusicInGamePlaying;

    //Data From SettsHolder - Player
    public GameMode typeOfEnemy;
    public Faction southFaction;
    public Faction northFaction;
    public string northName;
    public string southName;

    //Other Elements
    public HybridEffectsSystem hybridEffectsSystem;
    public AudioGenerator audioGenerator;
    public EndTurnButtonManager endTurnButtonManager;
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
        if (enablePlayableCardsFlag && currentPlayer != null)
        {
            enablePlayableCardsFlag = false;
            UnblockAllUnitsAndCards(currentPlayer);
            if (SettsHolder.instance.typeOfEnemy == GameMode.Computer && 
                GameManager.Instance.currentPlayer == GameManager.Instance.playerNorth &&
                AIManager.Instance.canAIMakeMove)
            {
                AIManager.Instance.manageMoves();
            }
        }
    }

    IEnumerator startGame()
    {
        currentPlayer = playerNorth;
        InitializeGame();
        endTurnButtonManager.InitialButtonBlock(16f);

        //Draw cards from deck to Player South
        for (int i = 0; i < 2; i++)
        {
            while (playerNorth.handViewVisual.isDrawingRunning || playerSouth.handViewVisual.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);
            }
            BonusEffects.Instance.drawNewCard(playerSouth, false);
        }

        //Draw cards from deck to Player North
        for (int i = 0; i < 2; i++)
        {
            while (playerNorth.handViewVisual.isDrawingRunning || playerSouth.handViewVisual.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);
            }
            BonusEffects.Instance.drawNewCard(playerNorth, false);

            while (playerNorth.handViewVisual.isDrawingRunning || playerSouth.handViewVisual.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }

        playerNorth.handViewVisual.blockAllOperations();
        playerSouth.handViewVisual.blockAllOperations();

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

        isItAITurn = SettsHolder.instance.typeOfEnemy == GameMode.Computer && currentPlayer == playerNorth;
        BlackAllUnitsAndCards();
        messageManager.ShowMessage(currentPlayer.name + " \nTwoja tura!", 2f);
        currentPlayer.updateResourcesNewTurn();
        currentPlayer.resourceVisual.updateResourcesView(currentPlayer.resourcesCurrent, currentPlayer.resourcesMaxThisTurn);
        currentPlayer.resourceVisual.ProgressText.color = new Color32(0, 0, 0, 255);
        BonusEffects.Instance.drawNewCard(currentPlayer, true);
        currentPlayer.namePanel.changeNamePanelColors();
        otherPlayer.namePanel.changeNamePanelColors();

        currentPlayer.handViewVisual.setRaycastAvailabilityForCards();
        currentPlayer.dropZoneVisual.setRaycastAvailabilityForCards();
        otherPlayer.handViewVisual.setRaycastAvailabilityForCards();
        otherPlayer.dropZoneVisual.setRaycastAvailabilityForCards();
        
        //If human players and hybrid effects on check whether bonus effect possible
        if (currentPlayer == playerSouth || currentPlayer == playerNorth && SettsHolder.instance.typeOfEnemy != GameMode.Computer)
        {
            hybridEffectsSystem.CheckWhetherToUseHybridEffect();
            //speechRecognition.CheckWhetherToShowSpeechSign();
        }
        
        if (isItAITurn)
        {
            AIManager.Instance.canAIMakeMove = true;
        }

        endTurnButtonManager.TimerStart();
        currentPlayer.armymodel.armyCardsModel.EnableAttackOfJustPlacedUnits(currentPlayer);
    }

    void InitializeGame()
    {
        IDFactory.ResetIDs();

        //Attribute factions, names, mode of game and general settings
        SettsHolder.instance.AttributeGameManagerNamesAndFactions();

        playerNorth.setInitialValues(0, northName, northFaction);
        playerSouth.setInitialValues(1, southName, southFaction);
        playerSouth.heroVisual.setHeroAcordingToFaction(southFaction);
        playerNorth.heroVisual.setHeroAcordingToFaction(northFaction);
        playerNorth.resourceVisual.updateResourcesView(playerNorth.resourcesCurrent, playerNorth.resourcesMaxThisTurn);
        playerSouth.resourceVisual.updateResourcesView(playerSouth.resourcesCurrent, playerSouth.resourcesMaxThisTurn);
        playerNorth.namePanel.NameText.text = playerNorth.name;
        playerSouth.namePanel.NameText.text = playerSouth.name;
    }

    public void cardDraggedToFrontCommand(Position playerPosition, int cardId)
    {
        if (playerPosition == Position.North)
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
        playerNorth.handViewVisual.blockAllOperations();
        playerSouth.handViewVisual.blockAllOperations();
        playerNorth.dropZoneVisual.blockAllUnitOperations();
        playerSouth.dropZoneVisual.blockAllUnitOperations();
    }

    public void UnblockAllUnitsAndCards(PlayerModel player)
    {
        player.armymodel.armyCardsModel.restoreCardAttacksPerRound();
        player.handViewVisual.setPlayableCards(player.resourcesCurrent);
        player.dropZoneVisual.unlockUnitAttacks();
    }

    public void StartGameWithCouroutine()
    {
        StartCoroutine(startGame());
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

