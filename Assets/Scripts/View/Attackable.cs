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
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Attackablke BEGIN DRAG this " + this);
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
        if (cardState == CardVisualStateEnum.Unit)
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
        if (cardState == CardVisualStateEnum.Unit)
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
        if (cardState == CardVisualStateEnum.Unit)
        {
            if (arrow != null)
            {
                arrow.transform.position = new Vector2(pz.x, pz.y);
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {


        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Vector3 pz = Camera.main.ScreenToWorldPoint(eventData.position);
        pz.z = 0;

        if (attackOnUnitSuccess)//for attack on Unit successful - attacker adjusts all the information about the fight, attacker and defender
        {
            if (cardState == CardVisualStateEnum.Unit)
            {
                GameObject attackableUnit = transform.GetComponent<CardDisplayLoader>().Unit;
                int defenderID = defenderCard.transform.GetComponent<IDAssignment>().uniqueId;
                int attackerID = t_Reference.GetComponent<IDAssignment>().uniqueId;
                int defenderArmor = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text);
                int attackerArmor = int.Parse(t_Reference.GetComponent<CardDisplayLoader>().armorText.text);
                int defenderAttack = int.Parse(defenderCard.transform.GetComponent<CardDisplayLoader>().attackText.text);
                int attackerAttack = int.Parse(t_Reference.GetComponent<CardDisplayLoader>().attackText.text);

                // move card to defender and come back
                t_Reference.DOMove(pz, 0.5f).SetEase(Ease.InQuint, 0.5f, 0.1f).OnComplete(comeBack);

                // create explosion for both units
                defenderUnit.GetComponent<UnitVisualManager>().createDamageVisual(attackerAttack);
                attackableUnit.GetComponent<UnitVisualManager>().createDamageVisual(defenderAttack);

                // remove armor from defender - in visual
                defenderArmor = (defenderArmor - attackerAttack > 0) ? defenderArmor - attackerAttack : 0;
                defenderCard.transform.GetComponent<CardDisplayLoader>().armorText.text = defenderArmor.ToString();
                defenderUnit.transform.GetComponent<UnitVisualManager>().armorText.text = defenderArmor.ToString();

                // remove armor from attacker - in visual
                attackerArmor = (attackerArmor - defenderAttack > 0) ? attackerArmor - defenderAttack : 0;
                t_Reference.GetComponent<CardDisplayLoader>().armorText.text = attackerArmor.ToString();
                attackableUnit.GetComponent<UnitVisualManager>().armorText.text = attackerArmor.ToString();


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

                // update armor, and if attacker dead then update model and delete card from view
                if (attackerArmor > 0)
                {
                    GameManager.Instance.currentPlayer.armymodel.armyCardsModel.updateArmorAfterDamageTaken(attackerID, attackerArmor);
                }
                else
                {
                    GameManager.Instance.currentPlayer.armymodel.armyCardsModel.moveCardFromFrontToGraveyard(attackerID);
                    Destroy(this.gameObject);
                }
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
                //attackerArmor = (attackerArmor - defenderAttack > 0) ? attackerArmor - defenderAttack : 0;
                //t_Reference.GetComponent<CardDisplayLoader>().armorText.text = attackerArmor.ToString();
                //attackableUnit.GetComponent<UnitVisualManager>().armorText.text = attackerArmor.ToString();


                // update armor in model, and if hero dead then update model and finish game
                if (defenderArmor > 0)
                {
                    GameManager.Instance.otherPlayer.armymodel.heroModel.currentHealth = defenderArmor;
                }
                else
                {
                    GameManager.Instance.otherPlayer.armymodel.heroModel.heroDies();
                    //Destroy(defenderCard.gameObject);
                    // TODO - visual game end
                    Debug.Log("Game Ended!!!!!!!!!!!!!!!!!!!!!!!!! Won: " + GameManager.Instance.currentPlayer.name);

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
}

