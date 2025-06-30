using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableObjectManage : MonoBehaviour
{
    public static TableObjectManage Instance;

    public CharacterItemScriptableObject characterConfig;
    public EnemyItemScriptableObject enemyConfig;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }

}
