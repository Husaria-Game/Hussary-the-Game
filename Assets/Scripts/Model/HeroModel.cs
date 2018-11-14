using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class HeroModel
{
    public int heroID { get; private set; }
    public String name { get; private set; }
    public int maxHealth { get; set; }
    public int currentHealth { get; set; }


    public HeroModel(int heroID, string name, int maxHealth)
    {
        this.heroID = heroID;
        this.name = name;
        this.maxHealth = maxHealth;
        this.currentHealth = maxHealth;
    }

    public void heroDies()
    {

    }
}
