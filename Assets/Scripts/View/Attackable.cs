using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Attackable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform parentToReturnTo = null;
    public Transform defenderUnit = null;
    public Defendable defenderCard = null;
    public GameObject hero = null;
    public Vector3 initialPosition;
    public Vector3 mousePosition;
    private GameObject cardPreview;
    public LineRenderer lineRenderer;
    public GameObject arrow;
    public bool attackOnUnitSuccess;
    public bool attackOnHeroSuccess;
    public bool attackOnAllUnitsSuccess;
    public bool healOnAllUnitsSuccess;
    public bool transformCardIntoUnit;
    public bool healOnOneUnitSuccess;
    public bool strenghtOnOneUnitSuccess;
    public DropZone initialDropZone;
    public DropZone friendlyDropZone;
    public DropZone enemyDropZone;
    private Vector3 pointerDisplacement = Vector3.zero;
    public Transform t_Reference;
    private CardVisualStateEnum cardDetailedType;
    private CardType cardType;


    private const float DELAYED_TIME_BETWEEN_UNIT_DEATH_AND_OBJECT_DESTROY = 2f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.Instance.isAttackableDraggingActive = true;
        // get object reference transform
        t_Reference = this.transform;

        cardDetailedType = t_Reference.GetComponent<CardDisplayLoader>().cardDetailedType;
        cardType = t_Reference.GetComponent<CardDisplayLoader>().cardType;

        //parentToReturnTo = t_Reference.parent;
        initialPosition = t_Reference.position;
        pointerDisplacement = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - initialPosition.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - initialPosition.y, 0);
        cardPreview = t_Reference.gameObject.transform.Find("Canvas").gameObject;

        attackOnUnitSuccess = false;
        attackOnHeroSuccess = false;

        t_Reference.gameObject.GetComponentInChildren<Canvas>().sortingLayerName = "ActiveCard";

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        // enable aim icon
        if (cardDetailedType == CardVisualStateEnum.Unit || cardType == CardType.TacticsCard)
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, initialPosition);
            }
            if (arrow != null)
            {
                arrow.SetActive(true);
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pz = Camera.main.ScreenToWorldPoint(eventData.position);
        pz.z = 0;

        // update aim icon for unit
        if (cardDetailedType == CardVisualStateEnum.Unit || cardType == CardType.TacticsCard)
        {
            if (lineRenderer != null)
            {
                Vector3 A = initialPosition;

                Vector3 P = Vector3.Lerp(A, pz, 0.90f);
                lineRenderer.SetPosition(1, new Vector2((P.x), (P.y)));
            }
        }
        else
        {
            t_Reference.position = new Vector2(pz.x - pointerDisplacement.x, pz.y - pointerDisplacement.y);
        }
        if (cardDetailedType == CardVisualStateEnum.Unit || cardType == CardType.TacticsCard)
        {
            if (arrow != null)
            {
                arrow.transform.position = new Vector2(pz.x, pz.y);
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.Instance.isAttackableDraggingActive = false;

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        
        Vector3 pz = new Vector3();
        
        if (SettsHolder.instance.typeOfEnemy == GameMode.Computer && GameManager.Instance.currentPlayer == GameManager.Instance.playerNorth)
        {
            pz = eventData.position;
        }
        else
        {
            pz = Camera.main.ScreenToWorldPoint(eventData.position);
        }
        
        pz.z = 0;

        if (attackOnUnitSuccess)//for attack on Unit successful - attacker adjusts all the information about the fight, attacker and defender
        {
            checkIfUnhideAICard();
            if (cardDetailedType == CardVisualStateEnum.Unit)
            {
                unitAttacksUnit(pz);
            }
            else if (cardDetailedType == CardVisualStateEnum.TacticsAttackOne)
            {
                tacticsAttacksUnit(pz);
            }
        }
        if (healOnOneUnitSuccess)//for heal on Unit successful 
        {
            checkIfUnhideAICard();
            tacticsBonusUnit(pz, cardDetailedType);
        }
        if (strenghtOnOneUnitSuccess)//for strenght on Unit successful 
        {
            checkIfUnhideAICard();
            tacticsBonusUnit(pz, cardDetailedType);
        }

        if (attackOnHeroSuccess && cardDetailedType == CardVisualStateEnum.Unit)//for attack on Hero successful - attacker adjusts all the information about the fight, attacker and defender
        {
            checkIfUnhideAICard();
            unitAttacksHero(pz);
        }

        //for attack on All Units successful if there are any units on table - attacker adjusts all the information about the fight, attacker and defender
        if (attackOnAllUnitsSuccess && (enemyDropZone.dropAreaImage.transform.childCount > 0))
        {
            checkIfUnhideAICard();
            tacticsAttacksAllUnits(pz);
        }

        if (healOnAllUnitsSuccess && (friendlyDropZone.dropAreaImage.transform.childCount > 0))
        {
            checkIfUnhideAICard();
            tacticsHealAllUnits(pz);
        }




        if (cardDetailedType == CardVisualStateEnum.Unit || cardType == CardType.TacticsCard)
        {
            if (lineRenderer != null)
            {

                lineRenderer.enabled = false;
            }
            if (arrow != null)
            {
                arrow.SetActive(false);
            }
        }
        this.GetComponentInChildren<Canvas>().sortingLayerName = "Card";
    }

    public void comeBack()
    {
        t_Reference.DOMove(initialPosition, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
    }

    public void unitAttacksUnit(Vector3 pz)
    {
        PlayerModel currentPlayer = GameManager.Instance.currentPlayer;
        PlayerModel otherPlayer = GameManager.Instance.otherPlayer;
        GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInFrontByID(this.GetComponent<IDAssignment>().uniqueId).currentAttacksPerTurn--;

        GameObject attackableUnit = transform.GetComponent<CardDisplayLoader>().Unit;
        int defenderID = defenderCard.transform.GetComponent<IDAssignment>().uniqueId;
        int attackerID = t_Reference.GetComponent<IDAssignment>().uniqueId;
        int defenderArmor = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text);
        int attackerArmor = int.Parse(t_Reference.GetComponent<CardDisplayLoader>().armorText.text);
        int defenderAttack = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().attackText.text);
        int attackerAttack = int.Parse(t_Reference.GetComponent<CardDisplayLoader>().attackText.text);

        // move card to defender and come back
        t_Reference.DOMove(pz, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

        // create explosion for both units, but not the tactics card
        attackableUnit.GetComponent<UnitVisualManager>().createDamageVisual(defenderAttack);
        defenderUnit.GetComponent<UnitVisualManager>().createDamageVisual(attackerAttack);

        // remove armor from defender - in visual
        defenderArmor = (defenderArmor - attackerAttack > 0) ? defenderArmor - attackerAttack : 0;
        defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text = defenderArmor.ToString();
        defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.text = defenderArmor.ToString();

        // remove armor from attacker - in visual
        attackerArmor = (attackerArmor - defenderAttack > 0) ? attackerArmor - defenderAttack : 0;
        t_Reference.GetComponent<CardDisplayLoader>().armorText.text = attackerArmor.ToString();
        attackableUnit.GetComponent<UnitVisualManager>().armorText.text = attackerArmor.ToString();
        //music
        GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.attackAudio);
        CheckWhetherToKillUnitOrNot(defenderArmor, defenderID, attackerArmor, attackerID, currentPlayer, otherPlayer);
    }

    public void unitAttacksHero(Vector3 pz)
    {
        PlayerModel currentPlayer = GameManager.Instance.currentPlayer;
        PlayerModel otherPlayer = GameManager.Instance.otherPlayer;
        //decrease number of attacks per turn for current card
        currentPlayer.armymodel.armyCardsModel.findCardInFrontByID(this.GetComponent<IDAssignment>().uniqueId).currentAttacksPerTurn--;

        GameObject attackableUnit = transform.GetComponent<CardDisplayLoader>().Unit;
        int attackerID = t_Reference.GetComponent<IDAssignment>().uniqueId;
        int attackerAttack = int.Parse(t_Reference.GetComponent<CardDisplayLoader>().attackText.text);

        // move card to defender and come back
        t_Reference.DOMove(pz, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

        BonusEffects.Instance.createHostileEffectHero(hero, initialDropZone, attackerAttack);
    }

    public void tacticsAttacksUnit(Vector3 pz)
    {
        PlayerModel currentPlayer = GameManager.Instance.currentPlayer;
        PlayerModel otherPlayer = GameManager.Instance.otherPlayer;
        GameObject attackableUnit = transform.GetComponent<CardDisplayLoader>().Unit;
        int defenderID = defenderCard.transform.GetComponent<IDAssignment>().uniqueId;
        int attackerID = t_Reference.GetComponent<IDAssignment>().uniqueId;
        
        int defenderArmor = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text);
        int attackerArmor = 0;
        int defenderAttack = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().attackText.text);
        int attackerAttack = currentPlayer.armymodel.armyCardsModel.findCardInHandByID(attackerID).attack;

        // update resources view and model
        updateResourcesModelAndView();
        GameManager.Instance.enablePlayableCardsFlag = true;
        // move card to defender and come back
        t_Reference.DOMove(defenderCard.transform.position, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

        // create explosion for unit, but not the tactics card
        defenderUnit.GetComponent<UnitVisualManager>().createDamageVisual(attackerAttack);

        // create certain effect for unit based on card type
        BonusEffects.Instance.createHostileBonusEffect(defenderCard, defenderUnit, cardDetailedType, attackerAttack);
        GameManager.Instance.UnblockAllUnitsAndCards(currentPlayer);
        defenderUnit.GetComponent<UnitVisualManager>().createDamageVisual(attackerAttack);
        StartCoroutine(DestroyGOWithDelay());
    }

    public void tacticsBonusUnit(Vector3 pz, CardVisualStateEnum cardDetailedType)
    {
        PlayerModel currentPlayer = GameManager.Instance.currentPlayer;
        PlayerModel otherPlayer = GameManager.Instance.otherPlayer;
        if (currentPlayer.armymodel.armyCardsModel.findCardInHandByID(
                this.gameObject.GetComponent<IDAssignment>().uniqueId) == null) return;
        
        GameObject attackableUnit = transform.GetComponent<CardDisplayLoader>().Unit;
        int attackerID = t_Reference.GetComponent<IDAssignment>().uniqueId;
        int attackerAttack = currentPlayer.armymodel.armyCardsModel.findCardInHandByID(attackerID).attack;
        // update resources view and model
        updateResourcesModelAndView();
        GameManager.Instance.enablePlayableCardsFlag = true;

        // move card to defender and come back
        t_Reference.DOMove(defenderCard.transform.position, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

        // create certain effect for unit based on card type
        BonusEffects.Instance.createFriendlyBonusEffect(defenderCard, defenderUnit, cardDetailedType, attackerAttack);
       
        currentPlayer.armymodel.armyCardsModel.moveCardFromHandToGraveyard(attackerID);
        GameManager.Instance.UnblockAllUnitsAndCards(currentPlayer);
        Destroy(this.gameObject);
    }

    public void updateResourcesModelAndView()
    {
        int updatedCurrentResources = GameManager.Instance.currentPlayer.substractCurrentResources(int.Parse(t_Reference.GetComponent<CardDisplayLoader>().cardMoneyText.text.ToString()));
        int maxResources = GameManager.Instance.currentPlayer.resourcesMaxThisTurn;
        GameManager.Instance.currentPlayer.resourceVisual.updateResourcesView(updatedCurrentResources, maxResources);
    }

    public void tacticsAttacksAllUnits(Vector3 pz)
    {
        PlayerModel currentPlayer = GameManager.Instance.currentPlayer;
        PlayerModel otherPlayer = GameManager.Instance.otherPlayer;
        
        // update resources view and model
        updateResourcesModelAndView();

        int attackerID = t_Reference.GetComponent<IDAssignment>().uniqueId;
        int attackerArmor = 0;
        int attackerAttack = currentPlayer.armymodel.armyCardsModel.findCardInHandByID(attackerID).attack;


        // move card to defender and come back
        t_Reference.DOMove(pz, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

        foreach (Transform child in enemyDropZone.dropAreaImage.transform)
        {
            defenderCard = child.GetComponent<Defendable>();
            defenderUnit = defenderCard.transform.GetComponent<CardDisplayLoader>().Unit.transform;
            int defenderID = defenderCard.transform.GetComponent<IDAssignment>().uniqueId;

            int defenderArmor = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text);
            int defenderAttack = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().attackText.text);// create explosion for unit, but not the tactics card
            defenderUnit.GetComponent<UnitVisualManager>().createDamageVisual(attackerAttack);

            // remove armor from defender - in visual
            defenderArmor = (defenderArmor - attackerAttack > 0) ? defenderArmor - attackerAttack : 0;
            defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text = defenderArmor.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.text = defenderArmor.ToString();
            //music
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.cannonAudio);
            // update armor in model, and if defender dead then update model and delete card from view
            if (defenderArmor > 0)
            {
                otherPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(defenderID, defenderArmor);
            }
            else
            {
                otherPlayer.armymodel.armyCardsModel.moveCardFromFrontToGraveyard(defenderID);
                Destroy(defenderCard.gameObject);
            }
        }

        // update model and delete card from view
        currentPlayer.armymodel.armyCardsModel.moveCardFromHandToGraveyard(attackerID);
        GameManager.Instance.UnblockAllUnitsAndCards(currentPlayer);
        Destroy(this.gameObject);
    }

    public void tacticsHealAllUnits(Vector3 pz)
    {
        PlayerModel currentPlayer = GameManager.Instance.currentPlayer;
        PlayerModel otherPlayer = GameManager.Instance.otherPlayer;
        
        // update resources view and model
        updateResourcesModelAndView();

        int healerID = t_Reference.GetComponent<IDAssignment>().uniqueId;

        // below is the parameter for the amount to be healed/added to armor
        int healerAttack = currentPlayer.armymodel.armyCardsModel.findCardInHandByID(healerID).attack;


        // move card to defender and come back
        t_Reference.DOMove(pz, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

        foreach (Transform child in friendlyDropZone.dropAreaImage.transform)
        {
            defenderCard = child.GetComponent<Defendable>();
            defenderUnit = defenderCard.transform.GetComponent<CardDisplayLoader>().Unit.transform;
            int defenderID = defenderCard.transform.GetComponent<IDAssignment>().uniqueId;

            int defenderArmor = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text);
            int defenderAttack = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().attackText.text);// create explosion for unit, but not the tactics card
            defenderUnit.GetComponent<UnitVisualManager>().createHealVisual(healerAttack);

            // add armor to defender - in visual
            defenderArmor = defenderArmor + healerAttack;
            defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text = defenderArmor.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.text = defenderArmor.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.color = new Color32(255, 0, 0, 255);
            //music
            GameManager.Instance.audioGenerator.PlayClip(GameManager.Instance.audioGenerator.enhencementAudio);
            // update armor in model
            currentPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(defenderID, defenderArmor);
        }
        
        // update model and delete healer card from view
        currentPlayer.armymodel.armyCardsModel.moveCardFromHandToGraveyard(healerID);
        GameManager.Instance.UnblockAllUnitsAndCards(currentPlayer);
        Destroy(this.gameObject);
    }


    public void CheckWhetherToKillUnitOrNot(int defenderArmor, int defenderID, int attackerArmor, int attackerID,
        PlayerModel currentPlayer, PlayerModel otherPlayer)
    {
        StartCoroutine(CheckWhetherToKillUnitOrNotWithCoroutine(defenderArmor, defenderID, attackerArmor, attackerID,
            currentPlayer, otherPlayer));
    }

    IEnumerator CheckWhetherToKillUnitOrNotWithCoroutine(int defenderArmor, int defenderID, int attackerArmor,
        int attackerID, PlayerModel currentPlayer, PlayerModel otherPlayer)
    {
        //Update armor in model, and if defender dead then update model and delete card from view
        if (defenderArmor > 0)
        {
            otherPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(defenderID, defenderArmor);
        }
        else
        {
            otherPlayer.armymodel.armyCardsModel.moveCardFromFrontToGraveyard(defenderID);
            yield return new WaitForSeconds(DELAYED_TIME_BETWEEN_UNIT_DEATH_AND_OBJECT_DESTROY);
            Destroy(defenderCard.gameObject);
        }

        //Update armor, and if attacker dead then update model and delete card from view
        if (attackerArmor > 0)
        {
            currentPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(attackerID, attackerArmor);
            initialDropZone.attackEventEnded = true;
        }
        else
        {
            //Add if which check if attacker id belongs to tactics or unit and then change movCard function
            if (GetComponent<CardDisplayLoader>().cardType == CardType.TacticsCard)
            {
                currentPlayer.armymodel.armyCardsModel.moveCardFromHandToGraveyard(attackerID);
            }
            else if(GetComponent<CardDisplayLoader>().cardType == CardType.UnitCard)
            {
                currentPlayer.armymodel.armyCardsModel.moveCardFromFrontToGraveyard(attackerID);
            }
            Destroy(this.gameObject);
        }
    }

    public void checkIfUnhideAICard()
    {
        if (!SettsHolder.instance.aIPlayerCardsSeen &&
            GameManager.Instance.isItAITurn)
        {
            this.transform.rotation = Quaternion.Euler(0, 0.0f, 0);
        }
    }

    IEnumerator DestroyGOWithDelay()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}