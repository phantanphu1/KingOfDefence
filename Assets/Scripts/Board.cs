using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public static Board Instance { get; private set; }
    public GameObject cellPrefab;
    public Transform board;
    private Cell[,] _matrix;
    private const int numRows = 3;
    private const int numCols = 5;
    private TableObjectManage tableObjectManage;
    List<CharacterItem> lisCharacterItemLv1 = new List<CharacterItem>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
        tableObjectManage = TableObjectManage.Instance;

    }
    void Start()
    {
        _matrix = new Cell[numRows + 1, numCols + 1];
        CreateBoard();
    }

    private void CreateBoard()
    {
        for (int i = 1; i <= numRows; i++)
        {
            for (int j = 1; j <= numCols; j++)
            {
                GameObject cellGameObject = Instantiate(cellPrefab, board);
                Cell cell = cellGameObject.GetComponent<Cell>();
                cell.row = i;
                cell.col = j;
                _matrix[i, j] = cell;
                cellGameObject.SetActive(true);
            }
        }
    }
    public Cell GetRandomEmptyCell()
    {

        List<Cell> emptyCells = new List<Cell>();

        for (int i = 1; i <= numRows; i++)
        {
            for (int j = 1; j <= numCols; j++)
            {
                if (_matrix[i, j].transform.childCount == 0)
                {
                    emptyCells.Add(_matrix[i, j]);
                }
            }
        }


        if (emptyCells.Count == 0)
        {
            return null;
        }

        int randomCellIndex = UnityEngine.Random.Range(0, emptyCells.Count);
        return emptyCells[randomCellIndex];

    }
    public void SpawnRandomCharacter()
    {
        {
            if (Board.Instance == null)
            {
                return;
            }

            Cell targetCell = Board.Instance.GetRandomEmptyCell();

            if (targetCell == null)
            {
                return;
            }
            // láy random character trong list
            foreach (var randomCharacterlevel1 in tableObjectManage.characterConfig.lsCharacterItem)
            {
                if (randomCharacterlevel1.baseLevel == 1)
                {
                    lisCharacterItemLv1.Add(randomCharacterlevel1);
                }
            }
            int randomCharacterIndex = UnityEngine.Random.Range(0, lisCharacterItemLv1.Count);
            CharacterItem selectedCharacterItem = lisCharacterItemLv1[randomCharacterIndex];

            GameObject selectedCharacterPrefab = selectedCharacterItem.characterPrefab;
            if (selectedCharacterPrefab == null)
            {
                return;
            }
            GameObject newCharacter = Instantiate(selectedCharacterPrefab, targetCell.transform);
            newCharacter.transform.localPosition = new Vector3(0f, -45f, 0f);
            Character character = newCharacter.GetComponent<Character>();
            if (character == null)
            {
                Debug.LogError("character null");
                return;
            }
            character.SetupCharacter(selectedCharacterItem);

        }

    }
    public void AddCharacter(CharacterItem character)
    {
        lisCharacterItemLv1.Add(character);
    }

}
