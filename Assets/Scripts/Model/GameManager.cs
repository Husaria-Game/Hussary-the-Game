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
    public bool isAttackableDraggingActive;

    public const float DELAYED_TIME_BETWEEN_UNIT_DEATH_AND_OBJECT_DESTROY = 2f;

    public Faction southFaction;
    public Faction northFaction;

    public AudioGenerator audioGenerator;
    public EndTurnButtonManager endTurnButtonManager;
    public SpeechRecognitionSystem speechRecognition;
    public DebugMessege debugMessageBox;

    public int turnNumber = 0;
    public const int CARD_LIMIT = 6;

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
            //Draw Card if not over limit
            if (southHandView.transform.childCount < CARD_LIMIT) {
                drawNewCard(playerSouth, southHandView, deckSouth, true);
            }
            else
            {
                UnblockAllUnitsAndCards(playerSouth, southHandView, dropZoneSouth);
            }
            speechRecognition.CheckWhetherToShowSpeechSign();
        }
        if (currentPlayer == playerNorth)
        {
            resourcesNorth.GetComponent<ResourcePool>().updateResourcesView(playerNorth.resourcesCurrent, playerNorth.resourcesMaxThisTurn);
            //Draw Card if not over limit
            if (northHandView.transform.childCount < CARD_LIMIT) {
                drawNewCard(playerNorth, northHandView, deckNorth, true);
            }
            else
            {
                UnblockAllUnitsAndCards(playerNorth, northHandView, dropZoneNorth);
            }
            //Here should be also SpeechRecognitionCheck if second player is human and not AI
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

        //Dodane przypiasanie frakcji - Na razie tylko tryb Multiplayer - potem trzeba wprowadić zmienną wybierającą tryb
        SettsHolder.instance.AttributeGameManagerNamesAndFactions();

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

    public void drawNewCard(PlayerModel playerModel, HandView handView, GameObject deck, bool shouldCardBeDrawnWithDelay)
    {
        if (shouldCardBeDrawnWithDelay)
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

    public void createFriendlyBonusEffect(Defendable defenderCard, Transform defenderUnit, CardVisualStateEnum cardDetailedTypeForEffect, int attackerAttack)
    {
        int defenderID = defenderCard.transform.GetComponent<IDAssignment>().uniqueId;
        int defenderArmor = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text);
        int defenderAttack = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().attackText.text);
        
        if (cardDetailedTypeForEffect == CardVisualStateEnum.TacticsHealOne)
        {
            defenderUnit.GetComponent<UnitVisualManager>().createHealVisual(attackerAttack);

            // add armor to defender - in visual
            defenderArmor = defenderArmor + attackerAttack;
            defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text = defenderArmor.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.text = defenderArmor.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.color = new Color32(255, 0, 0, 255);
            //music
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.enhencementAudio);
            // add armor to defender - in model
            GameManager.Instance.currentPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(defenderID, defenderArmor);
        }
        else if (cardDetailedTypeForEffect == CardVisualStateEnum.TacticsStrengthOne)
        {
            defenderUnit.GetComponent<UnitVisualManager>().createStrengthVisual(attackerAttack);

            // add armor to defender - in visual
            defenderAttack = defenderAttack + attackerAttack;
            defenderCard.transform.GetComponent<CardDisplayLoader>().attackText.text = defenderAttack.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().attackText.text = defenderAttack.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().attackText.color = new Color32(255, 0, 0, 255);
            //music
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.powerUpAudio);
            // add armor to defender - in model
            GameManager.Instance.currentPlayer.armymodel.armyCardsModel.updateStrengthAfterBonusEvent(defenderID, defenderAttack);
        }
    }

    public void createHostileBonusEffect(Defendable defenderCard, Transform defenderUnit, CardVisualStateEnum cardDetailedTypeForEffect, int attackerAttack)
    {
        int defenderID = defenderCard.transform.GetComponent<IDAssignment>().uniqueId;
        int defenderArmor = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text);
        int defenderAttack = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().attackText.text);
        
        if (cardDetailedTypeForEffect == CardVisualStateEnum.TacticsWithAim)
        {
            defenderUnit.GetComponent<UnitVisualManager>().createDamageVisual(attackerAttack);

            // add armor to defender - in visual
            defenderArmor = defenderArmor - attackerAttack;
            defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text = defenderArmor.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.text = defenderArmor.ToString();
            //defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.color = new Color32(255, 0, 0, 255);
            //music
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.cannonAudio);
            // adjust armor to defender - in model
            GameManager.Instance.otherPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(defenderID, defenderArmor);
            StartCoroutine(CheckWhetherToKillUnitAfterBonusWithCoroutine(defenderCard, defenderID, defenderArmor));
        }
    }

    IEnumerator CheckWhetherToKillUnitAfterBonusWithCoroutine(Defendable defenderCard, int defenderID, int defenderArmor)
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

    public Defendable pickRandomDropZoneUnitCard(PlayerModel playerAffectedWithEffect)
    {
        Defendable randomCard = null;
        if (playerAffectedWithEffect == GameManager.Instance.playerNorth)
        {
            randomCard = GameManager.Instance.dropZoneNorth.chooseRandowCardOnDropZone();
        }
        else if (playerAffectedWithEffect == GameManager.Instance.playerSouth)
        {
            randomCard = GameManager.Instance.dropZoneSouth.chooseRandowCardOnDropZone();
        }
        return randomCard;
    }
}
