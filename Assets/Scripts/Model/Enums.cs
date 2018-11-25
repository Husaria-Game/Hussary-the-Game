using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum CardType
{
    UnitCard, TacticsCard
}

public enum Affiliation
{
    Poland, Ottoman
}

public enum Faction
{
    Ottoman, Poland
}

public enum Position
{
    North, South
}

public enum CardVisualStateEnum
{
    UnitCard,
    Unit,
    TacticsAttackOne,
    TacticsAttackAll,
    TacticsHealOne,
    TacticsHealAll,
    TacticsStrengthOne,
    TacticsStrengthAll,
    //TacticsHealAndStrengthOne,
    //TacticsHealAndStrengthAll
}

public enum WhereIsCard
{
    Undefined, Hand, Front, Rotating
}

public enum GameMode
{
    Human, Computer
}
