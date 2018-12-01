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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        canAIMakeMove = false;
        this.playableCardList = new List<GameObject>();
        this.attackableUnitList = new List<GameObject>();
    }

    public void manageMoves()
    {
        canAIMakeMove = false;
        Debug.Log("0");
        if (GameManager.Instance.currentPlayer == GameManager.Instance.playerNorth)
        {
            foreach (Transform child in GameManager.Instance.currentPlayer.handViewVisual.transform)
            {
                Debug.Log("foreach");
                if (int.Parse(child.GetComponent<CardDisplayLoader>().cardMoneyText.text.ToString()) <= GameManager.Instance.currentPlayer.resourcesCurrent)
                {
                    Debug.Log("added object");
                    playableCardList.Add(child.gameObject);
                }
            }
            Debug.Log("playableCount " + playableCardList.Count);
            if (playableCardList.Count > 0 && playableCardList[0].GetComponent<CardDisplayLoader>().cardType == CardType.UnitCard)
            {
                dragCardFromHandToFrontAI(playableCardList[0]);
            }
        }
    }

    public void dragCardFromHandToFrontAI(GameObject cardToMove)
    {
        PointerEventData eventDataDrag = new PointerEventData(EventSystem.current);
        eventDataDrag.position = cardToMove.transform.position;
                
        cardToMove.GetComponent<Draggable>().OnBeginDrag(eventDataDrag);
//                playableCardList[0].transform.DOMove(GameManager.Instance.currentPlayer.dropZoneVisual.transform.position, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
        eventDataDrag.pointerDrag = playableCardList[0].gameObject;
        GameManager.Instance.currentPlayer.dropZoneVisual.OnDrop(eventDataDrag);
        cardToMove.GetComponent<Draggable>().OnEndDrag(eventDataDrag);
        playableCardList.Clear();
    }

    public void unitAttacksEnemyUnitAI(GameObject cardToMove)
    {
        Debug.Log("attack Unit");
        PointerEventData eventDataDrag = new PointerEventData(EventSystem.current);
        eventDataDrag.position = cardToMove.transform.position;
                
        cardToMove.GetComponent<Draggable>().OnBeginDrag(eventDataDrag);
//                playableCardList[0].transform.DOMove(GameManager.Instance.currentPlayer.dropZoneVisual.transform.position, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
        eventDataDrag.pointerDrag = playableCardList[0].gameObject;
        GameManager.Instance.currentPlayer.dropZoneVisual.OnDrop(eventDataDrag);
        cardToMove.GetComponent<Draggable>().OnEndDrag(eventDataDrag);
        playableCardList.Clear();
    }


}
