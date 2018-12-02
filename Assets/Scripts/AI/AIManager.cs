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
            // create list of attackable friendly units 
            foreach (Transform child in GameManager.Instance.currentPlayer.dropZoneVisual.dropAreaImage.transform)
            {
                Card cardInModel =
                    GameManager.Instance.currentPlayer.armymodel.armyCardsModel.findCardInFrontByID(
                        child.GetComponent<IDAssignment>().uniqueId);
                if (cardInModel.currentAttacksPerTurn > 0 && cardInModel.isAbleToAttack)
                {
                    attackableUnitList.Add(child.gameObject);
                }
            }

            // create list of defendable enemy units 
            foreach (Transform child in GameManager.Instance.otherPlayer.dropZoneVisual.dropAreaImage.transform)
            {
                Card cardInModel =
                    GameManager.Instance.otherPlayer.armymodel.armyCardsModel.findCardInFrontByID(
                        child.GetComponent<IDAssignment>().uniqueId);
                defendableUnitList.Add(child.gameObject);
            }
            
            
            // create list of playable cards 
            foreach (Transform child in GameManager.Instance.currentPlayer.handViewVisual.transform)
            {
                CardVisualStateEnum cardDetailedType = child.GetComponent<CardDisplayLoader>().cardDetailedType;
                
                // for affordable cards...
                if (int.Parse(child.GetComponent<CardDisplayLoader>().cardMoneyText.text.ToString()) <=
                    GameManager.Instance.currentPlayer.resourcesCurrent)
                {
                    // ...add to playable card list if card will have any effect (heal, strenghten or attack any Unit) 
                    switch(child.GetComponent<CardDisplayLoader>().cardDetailedType)
                    {
                        case CardVisualStateEnum.UnitCard:
                        {
                            playableCardList.Add(child.gameObject);
                            break;
                        }
                        case CardVisualStateEnum.TacticsAttackAll:
                        case CardVisualStateEnum.TacticsAttackOne:
                        case CardVisualStateEnum.TacticsHealAll:
                        case CardVisualStateEnum.TacticsHealOne:
                        {
                            if (defendableUnitList.Count > 0)
                            {
                                playableCardList.Add(child.gameObject);
                            }
                            break;
                        }
                        case CardVisualStateEnum.TacticsStrengthAll:
                        case CardVisualStateEnum.TacticsStrengthOne:
                        {
                            if (attackableUnitList.Count > 0)
                            {
                                playableCardList.Add(child.gameObject);
                            }
                            break;
                        }
                        default:
                            Debug.Log("default");
                            break;
                        
                    }
                }
                
            }

            if (playableCardList.Count <= 0 && attackableUnitList.Count <= 0)
            {
                StartCoroutine(endTurnAI());
                return;
            }

            int playableCardRandom = Random.Range(0, playableCardList.Count - 1);
            int attackableCardRandom = Random.Range(0, attackableUnitList.Count - 1);
            int defendableUnitRandom = Random.Range(0, defendableUnitList.Count - 1);

            if (playableCardList.Count > 0)
            {
                if (playableCardList[playableCardRandom].GetComponent<CardDisplayLoader>().cardType ==
                    CardType.UnitCard)
                {
                    dragCardFromHandToFrontAI(playableCardList[playableCardRandom]);
                }
                else if (playableCardList[playableCardRandom].GetComponent<CardDisplayLoader>().cardDetailedType ==
                         CardVisualStateEnum.TacticsStrengthOne ||
                         playableCardList[playableCardRandom].GetComponent<CardDisplayLoader>().cardDetailedType ==
                         CardVisualStateEnum.TacticsHealOne)
                {
                    tacticsBonusOneAI(playableCardList[playableCardRandom],
                        attackableUnitList[attackableCardRandom]);
                }
                else if (playableCardList[playableCardRandom].GetComponent<CardDisplayLoader>().cardDetailedType ==
                         CardVisualStateEnum.TacticsAttackOne)
                {
                    tacticsBonusOneAI(playableCardList[playableCardRandom],
                        defendableUnitList[defendableUnitRandom]);
                }
                else if (playableCardList[playableCardRandom].GetComponent<CardDisplayLoader>().cardDetailedType ==
                         CardVisualStateEnum.TacticsHealAll)
                {
                    tacticsBonusAllAI(playableCardList[playableCardRandom],
                        GameManager.Instance.currentPlayer.dropZoneVisual.gameObject);
                }
                else if (playableCardList[playableCardRandom].GetComponent<CardDisplayLoader>().cardDetailedType ==
                         CardVisualStateEnum.TacticsAttackAll)
                {
                    tacticsBonusAllAI(playableCardList[playableCardRandom],
                        GameManager.Instance.otherPlayer.dropZoneVisual.gameObject);
                }
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
        yield return new WaitForSeconds(1f);
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
        eventDataDrag.pointerDrag = cardToMove;
        GameManager.Instance.currentPlayer.dropZoneVisual.OnDrop(eventDataDrag);
        cardToMove.GetComponent<Draggable>().OnEndDrag(eventDataDrag);
    }

    public void unitAttacksEnemyUnitAI(GameObject attackingUnit, GameObject defendingUnit)
    {
        PointerEventData eventDataDrag = new PointerEventData(EventSystem.current);
        eventDataDrag.position = attackingUnit.transform.position;

        attackingUnit.GetComponent<Attackable>().OnBeginDrag(eventDataDrag);
        eventDataDrag.pointerDrag = attackingUnit;
        defendingUnit.GetComponent<Defendable>().OnDrop(eventDataDrag);
        eventDataDrag.position = defendingUnit.transform.position;
        attackingUnit.GetComponent<Attackable>().initialPosition = attackingUnit.transform.position;
        attackingUnit.GetComponent<Attackable>().OnEndDrag(eventDataDrag);
    }

    public void tacticsBonusOneAI(GameObject attackingUnit, GameObject defendingUnit)
    {
        PointerEventData eventDataDrag = new PointerEventData(EventSystem.current);
        eventDataDrag.position = attackingUnit.transform.position;

        attackingUnit.GetComponent<Attackable>().OnBeginDrag(eventDataDrag);
        eventDataDrag.pointerDrag = attackingUnit;
        defendingUnit.GetComponent<Defendable>().OnDrop(eventDataDrag);
        eventDataDrag.position = defendingUnit.transform.position;
        attackingUnit.GetComponent<Attackable>().initialPosition = attackingUnit.transform.position;
        attackingUnit.GetComponent<Attackable>().OnEndDrag(eventDataDrag);
    }

    public void tacticsBonusAllAI(GameObject attackingUnit, GameObject dropZoneAreObject)
    {
        PointerEventData eventDataDrag = new PointerEventData(EventSystem.current);
        eventDataDrag.position = attackingUnit.transform.position;

        attackingUnit.GetComponent<Attackable>().OnBeginDrag(eventDataDrag);
        eventDataDrag.pointerDrag = attackingUnit;
        dropZoneAreObject.GetComponent<DropZone>().OnDrop(eventDataDrag);
        eventDataDrag.position = dropZoneAreObject.GetComponent<DropZone>().dropAreaImage.transform.position;
        attackingUnit.GetComponent<Attackable>().initialPosition = attackingUnit.transform.position;
        attackingUnit.GetComponent<Attackable>().OnEndDrag(eventDataDrag);
    }

    public void unitAttacksEnemyHeroAI(GameObject attackingUnit, GameObject defendingHero)
    {
        PointerEventData eventDataDrag = new PointerEventData(EventSystem.current);
        eventDataDrag.position = attackingUnit.transform.position;

        attackingUnit.GetComponent<Attackable>().OnBeginDrag(eventDataDrag);
        eventDataDrag.pointerDrag = attackingUnit;
        defendingHero.GetComponent<Defendable>().OnDrop(eventDataDrag);
        eventDataDrag.position = defendingHero.transform.position;
        attackingUnit.GetComponent<Attackable>().initialPosition = attackingUnit.transform.position;
        attackingUnit.GetComponent<Attackable>().OnEndDrag(eventDataDrag);
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