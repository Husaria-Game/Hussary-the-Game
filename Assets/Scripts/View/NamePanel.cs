using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NamePanel : MonoBehaviour {
	
	public Text NameText;
	public GameObject namePanelGO;
	public PlayerModel panelOwner;

	// Use this for initialization
	void Start () {
		NameText.color = Color.gray;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void changeNamePanelColors()
	{
		if (GameManager.Instance.currentPlayer == panelOwner)
		{
			NameText.color = Color.green;
			Vector3 punch = new Vector3(1.2f, 1.2f, 0);
			namePanelGO.transform.DOPunchScale(punch, 1.2f, 1, 0);
		}
		else
		{
			NameText.color = Color.gray;
		}
	}
}
