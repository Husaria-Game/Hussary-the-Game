using UnityEngine;
using UnityEngine.UI;

public class EndTurnButtonManager : MonoBehaviour
{
    //Make sure to attach these Buttons in the Inspector
    public Button endTurnButton;
    public GameManager gameManager;

    void Start()
    {
        endTurnButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
        gameManager.nextTurn();
    }
}

