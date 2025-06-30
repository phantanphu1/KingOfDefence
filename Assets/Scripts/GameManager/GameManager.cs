using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Character character;
    private TableObjectManage tableObjectManage;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
        DontDestroyOnLoad(this);
        tableObjectManage = TableObjectManage.Instance;

    }
    private void Start()
    {
        character = FindObjectOfType<Character>();
    }
    public void LoadGamePlay()
    {
        SceneManager.LoadScene(1);
    }
    public void RandomCharacter()
    {
        // if (Character.Instance != null)
        // {
        //     Character.Instance.SpawnRandomCharacter();
        // }
        // character.SpawnRandomCharacter();


    }


}
