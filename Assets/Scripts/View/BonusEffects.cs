using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusEffects : MonoBehaviour
{
    // SINGLETON
    public static BonusEffects Instance;
    public const int CARD_LIMIT = 6;

    void Awake()
    {
        Instance = this;
    }

    public void drawNewCard(PlayerModel playerModel, bool shouldCardBeDrawnWithDelay)
    {
        HandView handView = null;
        GameObject deck = null;

        //Set hand view and deck based on player
        if (playerModel == GameManager.Instance.playerNorth)
        {
            handView = GameManager.Instance.northHandView;
            deck = GameManager.Instance.deckNorth;
        }
        else if (playerModel == GameManager.Instance.playerSouth)
        {
            handView = GameManager.Instance.southHandView;
            deck = GameManager.Instance.deckSouth;
        }

        //Draw Card if not over limit
        if (playerModel.armymodel.armyCardsModel.handCardList.Count < CARD_LIMIT)
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
        else
        {
            GameManager.Instance.UnblockAllUnitsAndCards(GameManager.Instance.playerSouth, GameManager.Instance.southHandView, GameManager.Instance.dropZoneSouth);
        }
    }

    //Coroutines type of draw card method
    IEnumerator drawNewCardWithDelay(PlayerModel playerModel, HandView handView, GameObject deck)
    {
        yield return new WaitForSeconds(2f);
        Card cardDrawn = playerModel.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
        handView.MoveDrawnCardFromDeckToHand(cardDrawn, playerModel, deck);
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

        if (cardDetailedTypeForEffect == CardVisualStateEnum.TacticsAttackOne)
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
            StartCoroutine(GameManager.Instance.CheckWhetherToKillUnitAfterBonusWithCoroutine(defenderCard, defenderID, defenderArmor));
        }
    }

    public void createHostileEffectHero(GameObject hero, DropZone initialDropZone, int attackerAttack)
    {
        // create explosion for hero
        hero.GetComponent<HeroVisualManager>().createDamageVisual(attackerAttack);
        int defenderArmor = int.Parse(hero.transform.GetComponent<HeroVisualManager>().healthText.text);

        // remove armor from defender - in visual
        defenderArmor = (defenderArmor - attackerAttack > 0) ? defenderArmor - attackerAttack : 0;
        hero.transform.GetComponent<HeroVisualManager>().healthText.text = defenderArmor.ToString();

        //music
        GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.heroHurtAudio);

        // update armor in model, and if hero dead then update model and finish game
        if (defenderArmor > 0)
        {
            GameManager.Instance.otherPlayer.armymodel.heroModel.currentHealth = defenderArmor;
            initialDropZone.attackEventEnded = true;
        }
        else
        {
            GameManager.Instance.otherPlayer.armymodel.heroModel.heroDies();
            Debug.Log("Game Ended! Won: " + GameManager.Instance.currentPlayer.name);

            // show final dialog with Winner after some amount of time
            StartCoroutine(GameManager.Instance.endingMessage.WhoWonMessege(GameManager.Instance.currentPlayer));
        }

    }

    public void createMoneyGainEffect(int moneyReceived)
    {
        GameManager.Instance.currentPlayer.addCurrentResources(moneyReceived);
        if (GameManager.Instance.currentPlayer == GameManager.Instance.playerSouth)
        {
            GameManager.Instance.resourcesSouth.GetComponent<ResourcePool>().showMoneyGainAnimation();
            GameManager.Instance.resourcesSouth.GetComponent<ResourcePool>().updateResourcesView(GameManager.Instance.playerSouth.resourcesCurrent, GameManager.Instance.playerSouth.resourcesMaxThisTurn);
            GameManager.Instance.southHandView.setPlayableCards(GameManager.Instance.playerSouth.resourcesCurrent);
        }
        else if (GameManager.Instance.currentPlayer == GameManager.Instance.playerNorth)
        {
            GameManager.Instance.resourcesNorth.GetComponent<ResourcePool>().showMoneyGainAnimation();
            GameManager.Instance.resourcesNorth.GetComponent<ResourcePool>().updateResourcesView(GameManager.Instance.playerNorth.resourcesCurrent, GameManager.Instance.playerNorth.resourcesMaxThisTurn);
            GameManager.Instance.northHandView.setPlayableCards(GameManager.Instance.playerNorth.resourcesCurrent);
        }
    }

    public Defendable pickRandomDropZoneUnitCard(PlayerModel playerAffectedWithEffect)
    {
        Defendable randomCard = null;
        if (playerAffectedWithEffect == GameManager.Instance.playerNorth)
        {
            randomCard = GameManager.Instance.dropZoneNorth.chooseRandomCardOnDropZone();
        }
        else if (playerAffectedWithEffect == GameManager.Instance.playerSouth)
        {
            randomCard = GameManager.Instance.dropZoneSouth.chooseRandomCardOnDropZone();
        }
        return randomCard;
    }

}
