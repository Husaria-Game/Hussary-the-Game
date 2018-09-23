using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {
    public string NorthName = "AI";
    public string SouthName = "Grzegorz";
    public Player playerSouth;
    public Player playerNorth;
    public MessageManager messageManager;
    // Use this for initialization
    IEnumerator Start () {
            while (playerSouth.enabled == false)
 {
            yield return new WaitForSeconds(0.05f);
        }
        SetAllGameDetails();
        playerSouth.faction = Faction.Ottoman;
        //messageManager.playerNorthName = this.playerNorth.Name.text;
        messageManager.playerSouthName = SouthName;
        foreach (Card value in playerSouth.army.cardContainer.deckCardList)
        {
            Debug.Log(value.cardName);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetAllGameDetails()
    {
        Debug.Log("GameManger START");

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
