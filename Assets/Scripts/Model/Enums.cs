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
    TacticsWithAim, // this should be named TacticsAttackOne - need to refactor
    TacticsAttackAll,
    TacticsHealOne,
    TacticsHealAll,
    TacticsBonusAttackParameterOne,
    TacticsBonusAttackParameterAll,
    TacticsHealAndBonusAttackParameterOne,
    TacticsHealAndBonusAttackParameterAll
}

public enum WhereIsCard
{
    Undefined, Hand, Front
}
