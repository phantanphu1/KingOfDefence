using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform board;
    private Cell[,] _matrix;
    private const int numRows = 3;
    private const int numCols = 5;
    public List<GameObject> danhSachNhanVat;
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
            }
        }
    }
    public void AddPlayer()
    {

        int randomPlayerIndex = UnityEngine.Random.Range(0, danhSachNhanVat.Count);
        GameObject selectedNhanVatPrefab = danhSachNhanVat[randomPlayerIndex];

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
            Debug.Log("No empty cells available on the board!");
            return;
        }

        int randomCellIndex = UnityEngine.Random.Range(0, emptyCells.Count);
        Cell targetCell = emptyCells[randomCellIndex];

        GameObject newNhanVat = Instantiate(selectedNhanVatPrefab, targetCell.transform);
        // newNhanVat.transform.localPosition = Vector3.zero;
        newNhanVat.transform.localPosition = new Vector3(0f, -45f, 0f);

    }

}
