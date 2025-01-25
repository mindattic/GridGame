[System.Serializable]
public class ActorStats
{
    public ActorStats() { }

    // Copy Constructor
    public ActorStats(ActorStats other)
    {
        Level = other.Level;
        PreviousHP = other.HP;
        HP = other.HP;
        MaxHP = other.MaxHP;
        PreviousAP = 0;
        AP = 0;
        MaxAP = 100;
        Strength = other.Strength;
        Vitality = other.Vitality;
        Agility = other.Agility;
        Speed = other.Speed;
        Luck = other.Luck;
        // If you have resistances, you'll need to copy those too.

        



    }


    public string Name;
    public string Variant;
    public string Description;
    public float Level;
    public float PreviousHP;
    public float HP;
    public float MaxHP;
    public float PreviousAP;
    public float AP;
    public float MaxAP;
    public float Strength;
    public float Vitality;
    public float Agility;
    public float Speed;
    public float Luck;

    //Resistances
    //public ActorResistence Resistence = new ActorResistence();
}

//public class ActorResistence
//{
//    public float Slashing = 10;
//    public float Piercing = 10;
//    public float Crushing = 10;
//    public float Fire = 10;
//    public float Ice = 10;
//    public float Lightning = 10;
//    public float Poison = 10;
//    public float Bleed = 10;

//    public float SlashingModifier => 1.0f + Random.Float(0, Slashing * 0.01f);
//    public float PiercingModifier => 1.0f + Random.Float(0, Piercing * 0.01f);
//    public float CrushingModifier => 1.0f + Random.Float(0, Crushing * 0.01f);
//    public float FireModifier => 1.0f + Random.Float(0, Fire * 0.01f);
//    public float IceModifier => 1.0f + Random.Float(0, Ice * 0.01f);
//    public float LightningModifier => 1.0f + Random.Float(0, Lightning * 0.01f);
//    public float PoisonModifier => 1.0f + Random.Float(0, Poison * 0.01f);
//    public float BleedModifier => 1.0f + Random.Float(0, Bleed * 0.01f);
//}
