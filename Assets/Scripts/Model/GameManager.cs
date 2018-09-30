using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
    // SINGLETON
    public static GameManager Instance;

    public string NorthName = "AI";
    public string SouthName = "Grzegorz";
    public Player playerSouth;
    public Player playerNorth;
    public MessageManager messageManager;
    public HandView handView;
    public GameObject unitCard;
    public GameObject tacticsCard;


    void Awake()
    {
        Instance = this;
    }
    // Use this for initialization
    IEnumerator Start () {
            while (playerSouth.enabled == false)
 {
            yield return new WaitForSeconds(0.05f);
        }
        InitializeGame();
        playerSouth.faction = Faction.Ottoman;
        //messageManager.playerNorthName = this.playerNorth.Name.text;
        messageManager.playerSouthName = SouthName;
        //foreach (Card value in playerSouth.army.cardContainer.deckCardList)
        //{
        //    Debug.Log(value.cardName);
        //}

        //handVisual.AddCard(playerSouth.army.cardContainer.deckCardList[0]);


        //// ----------draw a card from deck
        Card cardDrawn = playerSouth.army.cardContainer.moveCardFromDeckToHand();
        handView.AddDrawnCardFromDeckToHand(cardDrawn);
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
        Debug.Log("GameManger INITIALIZATIOIN");

        //PlayerModel playeNorth = new PlayerModel(0, "Malik", Faction.Poland);
        PlayerModel playeSouth = new PlayerModel(1, "Johnson", Faction.Ottoman);

        //this.playerSouth = new Player(0, this.SouthName, Faction.Ottoman);
        //playerSouth.Name.text = SouthName;
        //this.playerNorth = new Player(1, this.NorthName, Faction.Poland);
        //playerSouth.generateHand();

        //generateNorthArmy(playerNorth);
        //generateSouthArmy(playerSouth);

        //GameManager game = new GameManager(playerNorth, playerSouth);

        //playerNorth.showDeck();
        //playerSouth.showDeck();

        //playerNorth.generateHand();
        //playerSouth.generateHand();

        //playerNorth.showHand();
        //playerSouth.showHand();
    }
}
