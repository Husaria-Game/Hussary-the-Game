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
            updateAttackableUnitsList();

            // create list of defendable enemy units 
            updateDefendableUnitsList();
            
            // create list of playable cards 
            updatePlayableCardsList();

            // if no playables and attackables then end turn
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
            else if (attackableUnitList.Count > 0) // player has available unit moves
            {
                int allFriendlyUnitsStrenght = getAllUnitsStrenght(attackableUnitList);
                int allEnemyUnitsStrenght = getAllUnitsStrenght(defendableUnitList);
                
                // 1) if enemyHero can be killed this turn then attack this hero
                if (int.Parse(GameManager.Instance.otherPlayer.heroVisual.healthText.text) <= allFriendlyUnitsStrenght)
                {
                    unitAttacksEnemyUnitOrHeroAI(attackableUnitList[attackableCardRandom],
                        GameManager.Instance.otherPlayer.heroVisual.gameObject);
                }
                // 2) "Obligatory Kill" - if friendlyHero can be killed this turn then attack enemy units with attempt
                // to kill any unit
                else if (defendableUnitList.Count > 0 &&
                         int.Parse(GameManager.Instance.currentPlayer.heroVisual.healthText.text) <=
                         allEnemyUnitsStrenght)
                {
                    chooseToAttackUnitOrHero(attackableUnitList[attackableCardRandom], true);
                }
                // 3) decide which - unit or hero - to attack when enemy units are available
                else if (defendableUnitList.Count > 0)
                {
                    chooseToAttackUnitOrHero(attackableUnitList[attackableCardRandom], false);
                }
                // 4) attack hero if no enemy units available
                else
                {
                    unitAttacksEnemyUnitOrHeroAI(attackableUnitList[attackableCardRandom],
                        GameManager.Instance.otherPlayer.heroVisual.gameObject);
                }
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

    public void unitAttacksEnemyUnitOrHeroAI(GameObject attackingUnit, GameObject defendingUnit)
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

    // decide whether to attack enemy unit or hero
    public void chooseToAttackUnitOrHero(GameObject attacker, bool obligatoryUnitKill)
    {
        int attackerBiggestProfit;
        GameObject currentTarget = null;
        
        // 0) initial target is first unit for obligatory kill or enemy hero when normal situation
        if (obligatoryUnitKill)
        {
            attackerBiggestProfit = -99;
        }
        else
        {
            currentTarget = GameManager.Instance.otherPlayer.heroVisual.gameObject;
            attackerBiggestProfit = 0;
        }
        
        int attackerStrenght = int.Parse(attacker.GetComponent<CardDisplayLoader>().attackText.text);
        int attackerArmor = int.Parse(attacker.GetComponent<CardDisplayLoader>().armorText.text);
        
        // 1) look for enemy with profitable fight outcome
        foreach (GameObject unitGO in defendableUnitList)
        {
            int defenderStrenght = int.Parse(unitGO.GetComponent<CardDisplayLoader>().attackText.text);
            int defenderArmor = int.Parse(unitGO.GetComponent<CardDisplayLoader>().armorText.text);

            // armor stats after potential fight
            int defenderOutcomeArmor = defenderArmor - attackerStrenght > 0 ? defenderArmor - attackerStrenght : 0;
            int attackerOutcomeArmor = attackerArmor - defenderStrenght > 0 ? attackerArmor - defenderStrenght : 0;

            // armor stats change after potential fight
            int defenderArmorChange = defenderArmor - defenderOutcomeArmor;
            int attackerArmorChange = attackerArmor - attackerOutcomeArmor;

            int defenderLoss = defenderOutcomeArmor == 0 ? defenderStrenght + defenderArmor : defenderArmorChange;
            int attackerLoss = attackerOutcomeArmor == 0 ? attackerStrenght + attackerArmor : attackerArmorChange;

            int fightProfit = defenderLoss - attackerLoss;

            if (obligatoryUnitKill && isAttackerAbleToKillAnyEnemyUnit(attacker))
            {
                // search for obligatory kill
                if (fightProfit > attackerBiggestProfit && defenderOutcomeArmor == 0)
                {
                    // fight profitable
                    attackerBiggestProfit = fightProfit;
                    currentTarget = unitGO;
                }
            }
            else
            {
                if (fightProfit > attackerBiggestProfit)
                {
                    // fight profitable or in case of obligatory kill generates less loss
                    attackerBiggestProfit = fightProfit;
                    currentTarget = unitGO;
                }
                else if (fightProfit == attackerBiggestProfit && attackerOutcomeArmor > 0 && defenderOutcomeArmor == 0)
                {
                    // fight equal but unit kills other unit and survives
                    currentTarget = unitGO;
                }
            }
        }
            
        unitAttacksEnemyUnitOrHeroAI(attacker, currentTarget);
    }

    // creates list of attackable friendly units
    public void updateAttackableUnitsList()
    { 
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
        
    }

    // creates list of defendable enemy units 
    public void updateDefendableUnitsList()
    {
        foreach (Transform child in GameManager.Instance.otherPlayer.dropZoneVisual.dropAreaImage.transform)
        {
            Card cardInModel =
                GameManager.Instance.otherPlayer.armymodel.armyCardsModel.findCardInFrontByID(
                    child.GetComponent<IDAssignment>().uniqueId);
            defendableUnitList.Add(child.gameObject);
        }
        
    }

    // creates list of playable cards 
    public void updatePlayableCardsList()
    {
        foreach (Transform child in GameManager.Instance.currentPlayer.handViewVisual.transform)
        {
            CardVisualStateEnum cardDetailedType = child.GetComponent<CardDisplayLoader>().cardDetailedType;

            // for affordable cards...
            if (int.Parse(child.GetComponent<CardDisplayLoader>().cardMoneyText.text.ToString()) <=
                GameManager.Instance.currentPlayer.resourcesCurrent)
            {
                // ...add to playable card list if card will have any effect (heal, strenghten or attack any Unit) 
                switch (child.GetComponent<CardDisplayLoader>().cardDetailedType)
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
    }

    public int getAllUnitsStrenght(List<GameObject> unitList)
    {
        int tableUnitsStrenghtSum = 0;
        foreach (GameObject unitGO in unitList)
        {
            tableUnitsStrenghtSum += int.Parse(unitGO.GetComponent<CardDisplayLoader>().attackText.text);
            Debug.Log("tableUnitsStrenghtSum: " + tableUnitsStrenghtSum);
        }

        return tableUnitsStrenghtSum;
    }

    public bool isAttackerAbleToKillAnyEnemyUnit(GameObject attacker)
    {
        int attackerStrenght = int.Parse(attacker.GetComponent<CardDisplayLoader>().attackText.text);

        foreach (GameObject unitGO in defendableUnitList)
        {
            int defenderArmor = int.Parse(unitGO.GetComponent<CardDisplayLoader>().armorText.text);
            if (attackerStrenght >= defenderArmor)
            {
                return true;
            }
        }
        return false;
    }
}