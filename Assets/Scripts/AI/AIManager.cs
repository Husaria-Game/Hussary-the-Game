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

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        canAIMakeMove = false;
        this.playableCardList = new List<GameObject>();
    }

    void Update()
    {
        if (SettsHolder.instance.typeOfEnemy == GameMode.Computer && 
            GameManager.Instance.enablePlayableCardsFlag && 
            GameManager.Instance.currentPlayer == GameManager.Instance.playerNorth &&
            canAIMakeMove)
        {
            manageMoves();
        }
    }

    public void manageMoves()
    {
        canAIMakeMove = false;
        Debug.Log("0");
        if (GameManager.Instance.enablePlayableCardsFlag && GameManager.Instance.currentPlayer == GameManager.Instance.playerNorth)
        {
            Debug.Log("manageMoves");
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
                Debug.Log("moving");
                moveDesiredUnitCardToFront(playableCardList[0]);
            }
        }
    }

    public void moveDesiredUnitCardToFront(GameObject cardGO)
    {
        if(playableCardList.Count > 0)
        {
            Debug.Log("really moving");
            cardGO.transform.DOMove(GameManager.Instance.currentPlayer.dropZoneVisual.transform.position, 1).SetEase(Ease.OutQuint, 0.5f, 0.1f);
        }
        playableCardList.RemoveAt(0);
        GameManager.Instance.currentPlayer.armymodel.armyCardsModel.moveCardFromHandToFront(cardGO.GetComponent<IDAssignment>().uniqueId);
        cardGO.transform.SetParent(GameManager.Instance.currentPlayer.dropZoneVisual.transform.GetChild(0).GetChild(0));
    }


}
