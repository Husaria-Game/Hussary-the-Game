using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army
    {
        public CardContainer cardContainer { get; set; }
        public Town town { get; private set; }

        public Army(Faction faction)
        {
            if (faction == Faction.Ottoman)
            {
                this.cardContainer = new CardContainer();
                this.town = new Town(0, "Istambul", "greatest town", 40);
            }
            else
            {
                this.cardContainer = new CardContainer();
                this.town = new Town(0, "Warsaw", "strongest town", 45);
            }

        }


    }
