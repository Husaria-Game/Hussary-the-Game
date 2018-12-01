using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class AIManager : MonoBehaviour
{
    // SINGLETON
    public static AIManager Instance;
    public bool canAIMakeMove;
    public List<GameObject> playableCardList { get; set; }
    public List<GameObject> attackableUnitList { get; set; }
    public List<GameObject> defendableUnitList { get; set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        canAIMakeMove = false;
        this.playableCardList = new List<GameObject>();
        this.attackableUnitList = new List<GameObject>();
        this.defendableUnitList = new List<GameObject>();
    }

    public void manageMoves()
    {
        canAIMakeMove = false;
        playableCardList.Clear();
        attackableUnitList.Clear();
        defendableUnitList.Clear();
        GameManager.Instance.currentPlayer.armymodel.armyCardsModel.showCardLists();
        if (GameManager.Instance.currentPlayer == GameManager.Instance.playerNorth)
        {
            foreach (Transform child in GameManager.Instance.currentPlayer.handViewVisual.transform)
            {
                if (int.Parse(child.GetComponent<CardDisplayLoader>().cardMoneyText.text.ToString()) <=
                    GameManager.Instance.currentPlayer.resourcesCurrent)
                {
                    playableCardList.Add(child.gameObject);
                }
            }


            foreach (Transform child in GameManager.Instance.currentPlayer.dropZoneVisual.dropAreaImage.transform)
            {
                Card cardInModel =
                    GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInFrontByID(
                        child.GetComponent<IDAssignment>().uniqueId);
//                Debug.Log("Child: " + child.GetComponent<CardDisplayLoader>().cardNameText.text);
//                Debug.Log("child ID : " + child.GetComponent<IDAssignment>().uniqueId);
//                Debug.Log("CardsInModel: " + cardInModel);
                if (cardInModel.currentAttacksPerTurn > 0 && cardInModel.isAbleToAttack)
                {
                    attackableUnitList.Add(child.gameObject);
                }
            }


            foreach (Transform child in GameManager.Instance.otherPlayer.dropZoneVisual.dropAreaImage.transform)
            {
                Card cardInModel =
                    GameManager.Instance.otherPlayer.armymodel.armyCardsModel.findCardInFrontByID(
                        child.GetComponent<IDAssignment>().uniqueId);
                defendableUnitList.Add(child.gameObject);
            }

            Debug.Log("playableCount " + playableCardList.Count);

            Debug.Log("attackableCount " + attackableUnitList.Count);

            Debug.Log("defendableCount " + defendableUnitList.Count);

            if (playableCardList.Count <= 0 && attackableUnitList.Count <= 0)
            {
                StartCoroutine(endTurnAI());
                return;
            }

            int playableCardRandom = Random.Range(0, playableCardList.Count - 1);
            int defendableUnitRandom = Random.Range(0, defendableUnitList.Count - 1);
            
            if (playableCardList.Count > 0 &&
                playableCardList[playableCardRandom].GetComponent<CardDisplayLoader>().cardType == CardType.UnitCard)
            {
                dragCardFromHandToFrontAI(playableCardList[0]);
            }
            else if (attackableUnitList.Count > 0 && defendableUnitList.Count > 0)
            {
                // attacks either hero or unit (50% chance of each)
                attackUnitOrHeroAI(attackableUnitList[0],
                    defendableUnitList[defendableUnitRandom]);
            }
            else if (attackableUnitList.Count > 0)
            {
                unitAttacksEnemyHeroAI(attackableUnitList[0], 
                    GameManager.Instance.otherPlayer.heroVisual.gameObject);
            }
            else
            {
                StartCoroutine(endTurnAI());
                return;
            }
            StartCoroutine(doAnotherMoveAI());
        }
    }

    IEnumerator endTurnAI()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.endTurnButtonManager.TimerStop();
    }

    IEnumerator doAnotherMoveAI()
    {
        // has to be above 2 seconds
        yield return new WaitForSeconds(2.3f);
        manageMoves();
    }

    public void dragCardFromHandToFrontAI(GameObject cardToMove)
    {
        PointerEventData eventDataDrag = new PointerEventData(EventSystem.current);
        eventDataDrag.position = cardToMove.transform.position;

        cardToMove.GetComponent<Draggable>().OnBeginDrag(eventDataDrag);
//                playableCardList[0].transform.DOMove(GameManager.Instance.currentPlayer.dropZoneVisual.transform.position, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
        eventDataDrag.pointerDrag = cardToMove;
        GameManager.Instance.currentPlayer.dropZoneVisual.OnDrop(eventDataDrag);
        cardToMove.GetComponent<Draggable>().OnEndDrag(eventDataDrag);
        playableCardList.Clear();
    }

    public void unitAttacksEnemyUnitAI(GameObject attackingUnit, GameObject defendingUnit)
    {
        PointerEventData eventDataDrag = new PointerEventData(EventSystem.current);
        eventDataDrag.position = attackingUnit.transform.position;

        attackingUnit.GetComponent<Attackable>().OnBeginDrag(eventDataDrag);
//                playableCardList[0].transform.DOMove(GameManager.Instance.currentPlayer.dropZoneVisual.transform.position, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
        eventDataDrag.pointerDrag = attackingUnit;
        defendingUnit.GetComponent<Defendable>().OnDrop(eventDataDrag);
        eventDataDrag.position = defendingUnit.transform.position;
        attackingUnit.GetComponent<Attackable>().initialPosition = attackingUnit.transform.position;
        attackingUnit.GetComponent<Attackable>().OnEndDrag(eventDataDrag);
        attackableUnitList.Clear();
        defendableUnitList.Clear();
    }

    public void unitAttacksEnemyHeroAI(GameObject attackingUnit, GameObject defendingHero)
    {
        Debug.Log("attack Hero");
        PointerEventData eventDataDrag = new PointerEventData(EventSystem.current);
        eventDataDrag.position = attackingUnit.transform.position;

        attackingUnit.GetComponent<Attackable>().OnBeginDrag(eventDataDrag);
//                playableCardList[0].transform.DOMove(GameManager.Instance.currentPlayer.dropZoneVisual.transform.position, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
        eventDataDrag.pointerDrag = attackingUnit;
        defendingHero.GetComponent<Defendable>().OnDrop(eventDataDrag);
        eventDataDrag.position = defendingHero.transform.position;
        attackingUnit.GetComponent<Attackable>().initialPosition = attackingUnit.transform.position;
        attackingUnit.GetComponent<Attackable>().OnEndDrag(eventDataDrag);
        attackableUnitList.Clear();
        defendableUnitList.Clear();
    }

    public void attackUnitOrHeroAI(GameObject attacker, GameObject defender)
    {
        // attacks either hero or unit (50% chance of each)
        if (Random.value > 0.5f)
        {
            unitAttacksEnemyHeroAI(attacker, defender);
        }
        else
        {
            unitAttacksEnemyUnitAI(attacker, GameManager.Instance.otherPlayer.heroVisual.gameObject);
        }
    }
}