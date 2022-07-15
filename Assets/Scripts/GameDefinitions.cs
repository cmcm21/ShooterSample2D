using System.Collections.Generic;

public enum EnemyType {TARGET_BOULDER}

public static class GameDefinitions 
{

    public static Dictionary<EnemyType, int> HealthPerEnemyType = new Dictionary<EnemyType, int>()
    {
        {EnemyType.TARGET_BOULDER, 10}
    };
}
