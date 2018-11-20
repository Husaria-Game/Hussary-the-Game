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
        Debug.Log("attackable");
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

        Vector3 pz = Camera.main.ScreenToWorldPoint(eventData.position);
        pz.z = 0;

        if (attackOnUnitSuccess)//for attack on Unit successful - attacker adjusts all the information about the fight, attacker and defender
        {
            if (cardDetailedType == CardVisualStateEnum.Unit)
            {
                unitAttacksUnit(pz);
            }
            else if (cardDetailedType == CardVisualStateEnum.TacticsWithAim)
            {
                tacticsAttacksUnit(pz);
            }
        }
        if (healOnOneUnitSuccess)//for heal on Unit successful 
        {
            tacticsBonusUnit(pz, cardDetailedType);
        }
        if (strenghtOnOneUnitSuccess)//for strenght on Unit successful 
        {
            tacticsBonusUnit(pz, cardDetailedType);
        }

        if (attackOnHeroSuccess)//for attack on Hero successful - attacker adjusts all the information about the fight, attacker and defender
        {
            if (cardDetailedType == CardVisualStateEnum.Unit)
            {
                //decrease number of attacks per turn for current card
                GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInFrontByID(this.GetComponent<IDAssignment>().uniqueId).currentAttacksPerTurn--;

                GameObject attackableUnit = transform.GetComponent<CardDisplayLoader>().Unit;
                //int defenderID = defenderCard.transform.GetComponent<IDAssignment>().uniqueId;
                int attackerID = t_Reference.GetComponent<IDAssignment>().uniqueId;
                int defenderArmor = int.Parse(hero.transform.GetComponent<HeroVisualManager>().healthText.text);
                int attackerAttack = int.Parse(t_Reference.GetComponent<CardDisplayLoader>().attackText.text);

                // move card to defender and come back
                t_Reference.DOMove(pz, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

                // create explosion for hero
                hero.GetComponent<HeroVisualManager>().createDamageVisual(attackerAttack);

                // remove armor from defender - in visual
                defenderArmor = (defenderArmor - attackerAttack > 0) ? defenderArmor - attackerAttack : 0;
                hero.transform.GetComponent<HeroVisualManager>().healthText.text = defenderArmor.ToString();

                // remove armor from attacker, update visual and model
                // TODO remove attacker available moves


                // update armor in model, and if hero dead then update model and finish game
                if (defenderArmor > 0)
                {
                    GameManager.Instance.otherPlayer.armymodel.heroModel.currentHealth = defenderArmor;
                    initialDropZone.attackEventEnded = true;
                }
                else
                {
                    GameManager.Instance.otherPlayer.armymodel.heroModel.heroDies();
                    //Destroy(defenderCard.gameObject);
                    // TODO - visual game end
                    Debug.Log("Game Ended! Won: " + GameManager.Instance.currentPlayer.name);
                    GameManager.Instance.endingMessage.WhoWonMessege(GameManager.Instance.currentPlayer);
                }
            }
        }

        //for attack on All Units successful if there are any units on table - attacker adjusts all the information about the fight, attacker and defender
        if (attackOnAllUnitsSuccess && (enemyDropZone.dropAreaImage.transform.childCount > 0))
        {
            tacticsAttacksAllUnits(pz);
        }

        if (healOnAllUnitsSuccess && (friendlyDropZone.dropAreaImage.transform.childCount > 0))
        {
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

        CheckWhetherToKillUnitOrNot(defenderArmor, defenderID, attackerArmor, attackerID);
    }

    public void tacticsAttacksUnit(Vector3 pz)
    {
        GameObject attackableUnit = transform.GetComponent<CardDisplayLoader>().Unit;
        int defenderID = defenderCard.transform.GetComponent<IDAssignment>().uniqueId;
        int attackerID = t_Reference.GetComponent<IDAssignment>().uniqueId;
        
        int defenderArmor = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text);
        int attackerArmor = 0;
        int defenderAttack = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().attackText.text);
        int attackerAttack = GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInHandByID(attackerID).attack;

        // update resources view and model
        updateResourcesModelAndView();
        GameManager.Instance.enablePlayableCardsFlag = true;
        // move card to defender and come back
        t_Reference.DOMove(defenderCard.transform.position, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

        // create explosion for unit, but not the tactics card
        defenderUnit.GetComponent<UnitVisualManager>().createDamageVisual(attackerAttack);

        // remove armor from defender - in visual
        defenderArmor = (defenderArmor - attackerAttack > 0) ? defenderArmor - attackerAttack : 0;
        defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text = defenderArmor.ToString();
        defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.text = defenderArmor.ToString();

        CheckWhetherToKillUnitOrNotWithCoroutine(defenderArmor, defenderID, attackerArmor, attackerID);
    }

    public void tacticsBonusUnit(Vector3 pz, CardVisualStateEnum cardDetailedType)
    {
        GameObject attackableUnit = transform.GetComponent<CardDisplayLoader>().Unit;
        int defenderID = defenderCard.transform.GetComponent<IDAssignment>().uniqueId;
        int attackerID = t_Reference.GetComponent<IDAssignment>().uniqueId;

        int defenderArmor = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text);
        int attackerArmor = 0;
        int defenderAttack = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().attackText.text);
        int attackerAttack = GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInHandByID(attackerID).attack;

        // update resources view and model
        updateResourcesModelAndView();
        GameManager.Instance.enablePlayableCardsFlag = true;
        // move card to defender and come back
        t_Reference.DOMove(defenderCard.transform.position, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

        // create certain effect for unit bsed on card type
        if (cardDetailedType == CardVisualStateEnum.TacticsHealOne)
        {
            defenderUnit.GetComponent<UnitVisualManager>().createHealVisual(attackerAttack);

            // add armor to defender - in visual
            defenderArmor = defenderArmor + attackerAttack;
            defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text = defenderArmor.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.text = defenderArmor.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.color = new Color32(255,0,0,255);

            // add armor to defender - in model
            GameManager.Instance.currentPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(defenderID, defenderArmor);
        }
        else if (cardDetailedType == CardVisualStateEnum.TacticsStrengthOne)
        {
            defenderUnit.GetComponent<UnitVisualManager>().createStrengthVisual(attackerAttack);

            // add armor to defender - in visual
            defenderAttack = defenderAttack + attackerAttack;
            defenderCard.transform.GetComponent<CardDisplayLoader>().attackText.text = defenderAttack.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().attackText.text = defenderAttack.ToString();
            defenderUnit.transform.GetComponent<UnitVisualManager>().attackText.color = new Color32(255, 0, 0, 255);

            // add armor to defender - in model
            GameManager.Instance.currentPlayer.armymodel.armyCardsModel.updateStrengthAfterDamageTaken(defenderID, defenderAttack);
        }
        
        GameManager.Instance.currentPlayer.armymodel.armyCardsModel.moveCardFromHandToGraveyard(attackerID);
            
        Destroy(this.gameObject);
    }

    public void updateResourcesModelAndView()
    {
        int updatedCurrentResources = GameManager.Instance.currentPlayer.substractCurrentResources(int.Parse(t_Reference.GetComponent<CardDisplayLoader>().cardMoneyText.text.ToString()));
        int maxResources = GameManager.Instance.currentPlayer.resourcesMaxThisTurn;
        if (GameManager.Instance.currentPlayer == GameManager.Instance.playerNorth)
        {
            GameManager.Instance.resourcesNorth.transform.GetComponent<ResourcePool>().updateResourcesView(updatedCurrentResources, maxResources);
        }
        else if (GameManager.Instance.currentPlayer == GameManager.Instance.playerSouth)
        {
            GameManager.Instance.resourcesSouth.transform.GetComponent<ResourcePool>().updateResourcesView(updatedCurrentResources, maxResources);
        }
    }

    public void tacticsAttacksAllUnits(Vector3 pz)
    {
        // update resources view and model
        updateResourcesModelAndView();

        int attackerID = t_Reference.GetComponent<IDAssignment>().uniqueId;
        int attackerArmor = 0;
        int attackerAttack = GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInHandByID(attackerID).attack;


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

            // update armor in model, and if defender dead then update model and delete card from view
            if (defenderArmor > 0)
            {
                GameManager.Instance.otherPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(defenderID, defenderArmor);
            }
            else
            {
                GameManager.Instance.otherPlayer.armymodel.armyCardsModel.moveCardFromFrontToGraveyard(defenderID);
                Destroy(defenderCard.gameObject);
            }
        }
        //GameObject attackableUnit = transform.GetComponent<CardDisplayLoader>().Unit;




        // update model and delete card from view
        GameManager.Instance.currentPlayer.armymodel.armyCardsModel.moveCardFromHandToGraveyard(attackerID);
        Destroy(this.gameObject);
    }

    public void tacticsHealAllUnits(Vector3 pz)
    {
        // update resources view and model
        updateResourcesModelAndView();

        int healerID = t_Reference.GetComponent<IDAssignment>().uniqueId;

        // below is the parameter for the amount to be healed/added to armor
        int healerAttack = GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInHandByID(healerID).attack;


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

            // update armor in model
            GameManager.Instance.currentPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(defenderID, defenderArmor);
        }
        //GameObject attackableUnit = transform.GetComponent<CardDisplayLoader>().Unit;
        
        // update model and delete healer card from view
        GameManager.Instance.currentPlayer.armymodel.armyCardsModel.moveCardFromHandToGraveyard(healerID);
        Destroy(this.gameObject);
    }

    

    public void CheckWhetherToKillUnitOrNot(int defenderArmor, int defenderID, int attackerArmor, int attackerID)
    {
        StartCoroutine(CheckWhetherToKillUnitOrNotWithCoroutine(defenderArmor, defenderID, attackerArmor, attackerID));
    }

    IEnumerator CheckWhetherToKillUnitOrNotWithCoroutine(int defenderArmor, int defenderID, int attackerArmor, int attackerID)
    {
        //Update armor in model, and if defender dead then update model and delete card from view
        if (defenderArmor > 0)
        {
            GameManager.Instance.otherPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(defenderID, defenderArmor);
        }
        else
        {
            GameManager.Instance.otherPlayer.armymodel.armyCardsModel.moveCardFromFrontToGraveyard(defenderID);
            yield return new WaitForSeconds(DELAYED_TIME_BETWEEN_UNIT_DEATH_AND_OBJECT_DESTROY);
            Destroy(defenderCard.gameObject);
        }

        //Update armor, and if attacker dead then update model and delete card from view
        if (attackerArmor > 0)
        {
            GameManager.Instance.currentPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(attackerID, attackerArmor);
            initialDropZone.attackEventEnded = true;
        }
        else
        {
            //Add if which check if attacker id belongs to tactics or unit and then change movCard function
            if (GetComponent<CardDisplayLoader>().cardType == CardType.TacticsCard)
            {
                GameManager.Instance.currentPlayer.armymodel.armyCardsModel.moveCardFromHandToGraveyard(attackerID);
            }
            else if(GetComponent<CardDisplayLoader>().cardType == CardType.UnitCard)
            {
                GameManager.Instance.currentPlayer.armymodel.armyCardsModel.moveCardFromFrontToGraveyard(attackerID);
            }
            Destroy(this.gameObject);
        }
    }
}

