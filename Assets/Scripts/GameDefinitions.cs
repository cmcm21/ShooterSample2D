using System.Collections.Generic;
using UnityEngine;

public enum TargetType {METEOR,NPC}

public static class GameDefinitions 
{

    public static readonly Dictionary<TargetType, int> HealthPerTargetType = new Dictionary<TargetType, int>()
    {
        {TargetType.METEOR, 40},
        {TargetType.NPC ,100},
    };
    
    public static readonly Dictionary<TargetType,int> PointsPerTarget = new Dictionary<TargetType, int>()
    {
        { TargetType.NPC ,10},
        { TargetType.METEOR ,2}
    };

    public enum SFXClip
    {
        Fire,
        ShieldUp,
        ShieldDown,
        Lose,
        Explosion
    }

    public enum Tags
    {
        Bullet,
        Enemy,
        Player,
        DamageObject,
        Bonus
    };
}
