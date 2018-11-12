using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town
{
    public int townID { get; private set; }
    public String name { get; private set; }
    public String description { get; private set; }
    public int health { get; set; }

    public Town(int townID, string name, string description, int health)
    {
        this.townID = townID;
        this.name = name;
        this.description = description;
        this.health = health;
    }
}
