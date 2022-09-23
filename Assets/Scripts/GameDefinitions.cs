using System.Collections.Generic;

public enum TargetType {METEOR,NPC}

public static class GameDefinitions 
{

    public static readonly Dictionary<TargetType, int> HealthPerTargetType = new Dictionary<TargetType, int>()
    {
        {TargetType.METEOR, 40},
        {TargetType.NPC ,100},
    };

    public enum Tags
    {
        Bullet,
        Enemy,
        Player,
        DamageObject,
        Bonus
    };
}
