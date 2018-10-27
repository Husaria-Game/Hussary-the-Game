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
    public HandView handView;
    public GameObject unitCard;
    public GameObject tacticsCard;
    public GameObject visuals;


    void Awake()
    {
        Instance = this;
    }

    //enum SortingLayerEnum
    //{
    //    Default = "Default",
    //    Cards = "Cards",
    //    Units = "Units",
    //    ActiveCard = "ActiveCard"
    //}

    // Use this for initialization
    IEnumerator Start()
    {
        while (messageManager.enabled == false)
        {
            yield return new WaitForSeconds(0.05f);
        }
        InitializeGame();
        //playerSouth.faction = Faction.Ottoman;
        messageManager.playerSouthName = SouthName;


        //// ----------draw a card from deck
        //Card cardDrawn = playerSouth.armymodel.armyCardsModel.drawCardFromDeckList();
        //playerSouth.armymodel.armyCardsModel.moveCardFromDeckListToHandList();
        handView.AddDrawnCardFromToHand(playerSouth.armymodel.armyCardsModel.moveCardFromDeckListToHandList());
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

        //PlayerModel playeNorth = new PlayerModel(0, "Malik", Faction.Poland);
        this.playerSouth = new PlayerModel(1, "Johnson", Faction.Ottoman);

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
