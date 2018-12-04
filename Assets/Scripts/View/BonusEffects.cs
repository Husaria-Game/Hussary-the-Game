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
    public const float DELAYED_TIME_BETWEEN_UNIT_DEATH_AND_OBJECT_DESTROY = 2f;

    void Awake()
    {
        Instance = this;
    }

    public void drawNewCard(PlayerModel playerModel, bool shouldCardBeDrawnWithDelay)
    {

        //Draw Card if not over limit
        if (playerModel.armymodel.armyCardsModel.handCardList.Count < CARD_LIMIT)
        {
            if (shouldCardBeDrawnWithDelay)
            {
                StartCoroutine(drawNewCardWithDelay(playerModel));
            }
            else
            {
                Card cardDrawn = playerModel.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
                playerModel.handViewVisual.MoveDrawnCardFromDeckToHand(cardDrawn, playerModel);
            }
        }
        else
        {
            GameManager.Instance.UnblockAllUnitsAndCards(GameManager.Instance.playerSouth);
        }
    }

    //Coroutines type of draw card method
    IEnumerator drawNewCardWithDelay(PlayerModel playerModel)
    {
        yield return new WaitForSeconds(2f);
        Card cardDrawn = playerModel.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
        playerModel.handViewVisual.MoveDrawnCardFromDeckToHand(cardDrawn, playerModel);
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
            StartCoroutine(CheckWhetherToKillUnitAfterBonusWithCoroutine(defenderCard, defenderID, defenderArmor));
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
        GameManager.Instance.currentPlayer.resourceVisual.updateResourcesView(GameManager.Instance.currentPlayer.resourcesCurrent, GameManager.Instance.currentPlayer.resourcesMaxThisTurn);
        GameManager.Instance.currentPlayer.handViewVisual.setPlayableCards(GameManager.Instance.currentPlayer.resourcesCurrent);
        if (moneyReceived > 0)
        {
            GameManager.Instance.currentPlayer.resourceVisual.showMoneyGainAnimation();
            GameManager.Instance.debugMessageBox.ShowDebugText("Sukces! Zebrano dodatkowe monety", true);
            // coin audio
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.coinGainAudio);
        }
        else
        {
            //Play sound effect and put text in debugMessegeBox
            GameManager.Instance.debugMessageBox.ShowDebugText("Nie udało się uzbierać monet - brak efektu", false);
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.noEffectAudio);
        }
    }

    public Defendable pickRandomDropZoneUnitCard(PlayerModel playerAffectedWithEffect)
    {
        Defendable randomCard = null;

        randomCard = playerAffectedWithEffect.dropZoneVisual.chooseRandomCardOnDropZone();

        return randomCard;
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
