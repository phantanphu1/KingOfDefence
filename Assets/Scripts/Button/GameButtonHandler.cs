using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonHandler : MonoBehaviour
{
    public Button spawnRandomCharacterButton;

    void Start()
    {
        if (spawnRandomCharacterButton != null)
        {
            spawnRandomCharacterButton.onClick.AddListener(OnSpawnRandomCharacterButtonClicked);
        }
    }

    void OnSpawnRandomCharacterButtonClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RandomCharacter();
        }
    }
}
