using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGameModeMenu : MonoBehaviour {

    public Button singlePlayer;
    public Button multiPlayer;

	void Start () {
        singlePlayer.onClick.AddListener(SettsHolder.instance.PlayAgainstAI);
        multiPlayer.onClick.AddListener(SettsHolder.instance.PlayAgainstHuman);
    }
	
	// Update is called once per frame
	void Update () {

        
	}

    
}
