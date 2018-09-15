using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SetAllGameDetails();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetAllGameDetails()
    {
        Debug.Log("heja");
        //Console.WriteLine("heja");

        //Player playerSouth = new Player(0, "ComputerPlayer", Faction.Ottoman);
        //Player playerNorth = new Player(1, "HumanPlayer", Faction.Poland);

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
