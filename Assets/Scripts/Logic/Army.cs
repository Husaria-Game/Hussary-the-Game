using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
    {
    public CardContainer cardContainer;
    public Town town;

        public Army(Faction faction)
        {
            if (faction == Faction.Ottoman)
            {
                //this.cardContainer = new CardContainer();
            //this.town = new Town(0, "Istambul", "greatest town", 40);
            //this.town.name.text = "Istanbul";
            }
            else
            {
                //this.cardContainer = new CardContainer();
                //this.town = new Town(0, "Warsaw", "strongest town", 45);
            }

        }


    }
