using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
    // SINGLETON
    public static GameManager Instance;

    public string NorthName = "AI";
    public string SouthName = "Grzegorz";
    //public Player player;
    public PlayerModel playerSouth;
    public PlayerModel playerNorth;
    public MessageManager messageManager;
    public HandView northHandView;
    public HandView southHandView;
    public GameObject unitCard;
    public GameObject tacticsCard;
    public GameObject visuals;
    public GameObject deckNorth;
    public GameObject deckSouth;


    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    IEnumerator Start()
    {
        while (messageManager.enabled == false)
        {
            yield return new WaitForSeconds(0.05f);
        }
        InitializeGame();
        messageManager.playerSouthName = SouthName;


        //// ----------draw 4 cards from deck to Player South
        for(int i=0;i<4; i++)
        {            
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);             
            }

            Card cardDrawn = playerSouth.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
            southHandView.AddDrawnCardFromToHand(cardDrawn, playerSouth, deckSouth);
        }

        //// ----------draw 4 cards from deck to Player North
        for (int i = 0; i < 4; i++)
        {
            while (northHandView.isDrawingRunning || southHandView.isDrawingRunning)
            {
                yield return new WaitForSeconds(0.2f);
            }

            Card cardDrawn = playerNorth.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
            northHandView.AddDrawnCardFromToHand(cardDrawn, playerNorth, deckNorth);
        }

        //cardDrawn = playerSouth.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
        //handView.AddDrawnCardFromToHand(cardDrawn);
        //// ----------instantiate card and load its display
        //GameObject newCard;
        //if (cardDrawn.maxHealth > 0)
        //{
        //    newCard = Instantiate(unitCard, new Vector3(0, 0, 0), Quaternion.identity, handVisual.transform);
        //}
        //else
        //{
        //    newCard = Instantiate(tacticsCard, new Vector3(0, 0, 0), Quaternion.identity, handVisual.transform);
        //}
        //CardDisplayLoader cardDisplayLoader = newCard.GetComponent<CardDisplayLoader>();
        //cardDisplayLoader.card = cardDrawn;
        //cardDisplayLoader.loadCardAsset();

        //////TODO define indexer??
        //Card card = this.deckCardList[r];
        //this.deckCardList.RemoveAt(r);
        //this.handCardList.Add(card);
        //GameObject newCard = Instantiate(unitCard, new Vector3(0, 0, 0), Quaternion.identity, handVisual.transform);
        //GameObject newCard2 = Instantiate(tacticsCard, new Vector3(0, 0, 0), Quaternion.identity, handVisual.transform);
        //newCard.set = handVisual.transform.GetChild(0).GetChild(0); ;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitializeGame()
    {
        Debug.Log("GameManger INITIALIZATION");

        this.playerNorth = new PlayerModel(0, "Cooper", Faction.Ottoman, Position.North);
        this.playerSouth = new PlayerModel(1, "Johnson", Faction.Ottoman, Position.South);
    }
}
