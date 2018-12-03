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
    public PlayerModel otherPlayer; //player that waits for his turn
    public MessageManager messageManager;
    public EndingMessege endingMessage;
    public GameObject unitCard;
    public GameObject tacticsCard;
    public GameObject visuals;
    public GameObject mainMenu;
    public bool enablePlayableCardsFlag;
    public bool isAttackableDraggingActive;

    //Data From SettsHolder
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
        if (enablePlayableCardsFlag && currentPlayer != null)
        {
            UnblockAllUnitsAndCards(currentPlayer);
            enablePlayableCardsFlag = false;
            if (SettsHolder.instance.typeOfEnemy == GameMode.Computer && 
                GameManager.Instance.currentPlayer == GameManager.Instance.playerNorth &&
                AIManager.Instance.canAIMakeMove)
            {
                AIManager.Instance.manageMoves();
            }
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


        //// ----------draw cards from deck to Player South
        for (int i=0; i < 2; i++)
        {            
            while (playerNorth.handViewVisual.isDrawingRunning || playerSouth.handViewVisual.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);             
            }
            BonusEffects.Instance.drawNewCard(playerSouth, false);
        }

        //// ----------draw cards from deck to Player North
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
        currentPlayer.resourceVisual.updateResourcesView(currentPlayer.resourcesCurrent, currentPlayer.resourcesMaxThisTurn);
        currentPlayer.resourceVisual.ProgressText.color = new Color32(0, 0, 0, 255);
        BonusEffects.Instance.drawNewCard(currentPlayer, true);
        currentPlayer.namePanel.changeNamePanelColors();
        otherPlayer.namePanel.changeNamePanelColors();

        if (currentPlayer == playerSouth)
        {
            //going to replace it with hybridEffectsSystem
            speechRecognition.CheckWhetherToShowSpeechSign();
        }

        // TODO: get rid of this condition later on
        if (currentPlayer == playerNorth && SettsHolder.instance.typeOfEnemy != GameMode.Computer)
        {
            //going to replace it with hybridEffectsSystem
            ARManager.Instance.goToARScene();
        }


        endTurnButtonManager.TimerStart();
        currentPlayer.armymodel.armyCardsModel.EnableAttackOfJustPlacedUnits(currentPlayer);
        if (SettsHolder.instance.typeOfEnemy == GameMode.Computer && currentPlayer == playerNorth)
        {
            Debug.Log("GameManager: AI can make move");
            AIManager.Instance.canAIMakeMove = true;
        }
    }

    void InitializeGame()
    {
        Debug.Log("GameManger INITIALIZATION");
        Instance.debugMessageBox.ShowDebugText("Gra Inicjalizowana", true);

        IDFactory.ResetIDs();

        //Attribute factions, names, and mode of game
        SettsHolder.instance.AttributeGameManagerNamesAndFactions();

        Debug.Log(typeOfEnemy);

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
