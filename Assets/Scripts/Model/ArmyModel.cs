using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyModel {


    public ArmyCardsModel armyCardsModel;
    public HeroModel heroModel;

    public ArmyModel(Faction faction)
    {
        if (faction == Faction.Ottoman)
        {
            this.armyCardsModel = new ArmyCardsModel(faction);
            this.heroModel = new HeroModel(0, "Sulejman", 20);
        }
        else
        {
            this.armyCardsModel = new ArmyCardsModel(faction);
            this.heroModel = new HeroModel(0, "Jan III Sobieski", 20);
        }

    }


}
