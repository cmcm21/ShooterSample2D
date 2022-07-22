using System.Collections.Generic;

public enum EnemyType {TARGET_BOULDER,NPC}

public static class GameDefinitions 
{

    public static readonly Dictionary<EnemyType, int> HealthPerEnemyType = new Dictionary<EnemyType, int>()
    {
        {EnemyType.TARGET_BOULDER, 40},
        { EnemyType.NPC ,100}
    };

    public enum Tags
    {
        Bullet,
        Enemy,
        Player,
        DamageObject
    };
}
