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
    public bool transformCardIntoUnit;
    public DropZone initialDropZone;
    private Vector3 pointerDisplacement = Vector3.zero;
    public Transform t_Reference;
    private CardVisualStateEnum cardState;

    private const float DELAYED_TIME_BETWEEN_UNIT_DEATH_AND_OBJECT_DESTROY = 2f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        GameManager.Instance.isAttackableDraggingActive = true;
        Debug.Log("attackable");
        // get object reference transform
        t_Reference = this.transform;

        cardState = t_Reference.GetComponent<CardVisualState>().cardVisualStateEnum;

        //parentToReturnTo = t_Reference.parent;
        initialPosition = t_Reference.position;
        pointerDisplacement = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - initialPosition.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - initialPosition.y, 0);
        cardPreview = t_Reference.gameObject.transform.Find("Canvas").gameObject;

        attackOnUnitSuccess = false;
        attackOnHeroSuccess = false;

        t_Reference.gameObject.GetComponentInChildren<Canvas>().sortingLayerName = "ActiveCard";

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        // enable aim icon
        if (cardState == CardVisualStateEnum.Unit || cardState == CardVisualStateEnum.TacticsWithAim)
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
        if (cardState == CardVisualStateEnum.Unit || cardState == CardVisualStateEnum.TacticsWithAim)
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
        if (cardState == CardVisualStateEnum.Unit || cardState == CardVisualStateEnum.TacticsWithAim)
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
            if (cardState == CardVisualStateEnum.Unit)
            {
                unitAttacksUnit(pz);
            }
            else if (cardState == CardVisualStateEnum.TacticsWithAim)
            {
                tacticsAttacksUnit(pz);
            }

        }
        else
        {
            //t_Reference.DOMove(initialPosition, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
        }

        if (attackOnHeroSuccess)//for attack on Hero successful - attacker adjusts all the information about the fight, attacker and defender
        {
            if (cardState == CardVisualStateEnum.Unit)
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

        if (cardState == CardVisualStateEnum.Unit || cardState == CardVisualStateEnum.TacticsWithAim)
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

        // move card to defender and come back
        t_Reference.DOMove(pz, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

        // create explosion for unit, but not the tactics card
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

        // update model and delete card from view
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
            GameManager.Instance.currentPlayer.armymodel.armyCardsModel.moveCardFromFrontToGraveyard(attackerID);
            Destroy(this.gameObject);
        }
    }
}

