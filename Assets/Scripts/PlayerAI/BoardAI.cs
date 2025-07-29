using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAI : MonoBehaviour
{
    public static BoardAI Instance;
    public GameObject cellPrefab;
    public Transform board;
    private CellAI[,] _matrix;
    private const int numRows = 3;
    private const int numCols = 5;
    private TableObjectManage tableObjectManage;
    private float _mana;

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
        _matrix = new CellAI[numRows + 1, numCols + 1];
        CreateBoard();
        _mana = PlayerAIManager.Instance.ReturnManaData();
        // SpawnRandomCharacter();
        // SpawnRandomCharacter();
        InvokeRepeating("AddPlayerAIOnBoard", 0, 2f);
    }
    void Update()
    {
        // AddPlayerAIOnBoard();
    }

    private void CreateBoard()
    {
        for (int i = 1; i <= numRows; i++)
        {
            for (int j = 1; j <= numCols; j++)
            {
                GameObject cellGameObject = Instantiate(cellPrefab, board);
                CellAI cell = cellGameObject.GetComponent<CellAI>();
                cell.row = i;
                cell.col = j;
                _matrix[i, j] = cell;
                cellGameObject.SetActive(true);
            }
        }
    }
    public CellAI GetRandomEmptyCell()
    {

        List<CellAI> emptyCells = new List<CellAI>();

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
    public void SpawnRandomPlayerAI()
    {
        {
            if (BoardAI.Instance == null)
            {
                return;
            }

            CellAI targetCell = BoardAI.Instance.GetRandomEmptyCell();

            if (targetCell == null)
            {
                return;
            }
            // láy random character trong list
            int randomCharacterIndex = UnityEngine.Random.Range(0, PlayerAIManager.Instance._lsPlayerAILV1.Count);
            PlayerAIItem selectedCharacterItem = PlayerAIManager.Instance._lsPlayerAILV1[randomCharacterIndex];

            GameObject selectedCharacterPrefab = selectedCharacterItem.playerAIPrefab;
            if (selectedCharacterPrefab == null)
            {
                return;
            }
            GameObject newCharacter = Instantiate(selectedCharacterPrefab, targetCell.transform);
            newCharacter.transform.localPosition = new Vector3(0f, -45f, 0f);
            PlayerAI playerAI = newCharacter.GetComponent<PlayerAI>();
            // Debug.LogWarning($"add: {targetCell.row},targetCell.col:{targetCell.col}");
            Debug.LogWarning($"_matrix[{targetCell.row},{targetCell.col}]");

            if (playerAI == null)
            {
                Debug.LogError("character null");
                return;
            }
            // Debug.Log($"selectedCharacterItem:{selectedCharacterItem.heroType}");

            playerAI.SetupPlayerAI(selectedCharacterItem);
            targetCell.SetPlayerAI(playerAI);
            // CheckMergerPlayerAI(selectedCharacterItem);
            CheckAllBoardForMerger();
        }

    }
    // private void CheckMergerPlayerAI(PlayerAIItem playerAIItem)
    // {
    //     int count = 0;
    //     foreach (Transform item in board)
    //     {
    //         if (item.childCount > 0)
    //         {
    //             var chillTransform = item.GetChild(0);
    //             PlayerAI playerAI = chillTransform.GetComponent<PlayerAI>();
    //             Debug.Log($"vao:{chillTransform}");
    //             if (playerAIItem.heroType == playerAI.HeroType || playerAIItem.baseLevel == playerAI.BaseLevel)
    //             {
    //                 if (count >= 2)
    //                 {
    //                     OnUpgradePlayerAI();
    //                     count = 0;
    //                     break;
    //                 }
    //                 count++;
    //                 // Debug.Log($"playerAI.HeroType:{playerAI.HeroType}");
    //             }
    //         }
    //         else
    //         {
    //             Debug.LogWarning("khong co");
    //         }
    //     }
    // }

    // private void CheckAllBoardForMerger()
    // {
    //     for (int r = 1; r <= numRows; r++)
    //     {
    //         for (int c = 1; c <= numCols; c++)
    //         {
    //             CellAI currentCell = _matrix[r, c];
    //             if (currentCell == null)
    //             {
    //                 Debug.LogError($"_matrix[{r},{c}] is NULL. Board creation issue?");
    //                 continue;
    //             }

    //             PlayerAI currentPlayer = GetPlayerAIFromCell(currentCell);
    //             if (currentPlayer != null)
    //             {

    //                 if (playerAIItem.heroType == currentPlayer.HeroType)
    //                 {
    //                     int baseLevel = playerAIItem.baseLevel + currentPlayer.BaseLevel;
    //                     foreach (var item in tableObjectManage.playerAIConfig.lsPlayerAIItem)
    //                     {
    //                         if (item.baseLevel == baseLevel && item.heroType == playerAIItem.heroType)
    //                         {
    //                             OnUpgradePlayerAI(item, currentCell);
    //                             Debug.Log($"K co trung:{item.baseLevel},{item.heroType}");

    //                         }
    //                     }
    //                 }
    //                 // else
    //                 // {
    //                 //     Debug.Log($"K co trung:{playerAIItem.baseLevel},{currentPlayer.BaseLevel}");
    //                 //     Debug.Log($"K co trung:{playerAIItem.heroType},{currentPlayer.HeroType}");
    //                 // }
    //             }
    //             else
    //             {
    //                 // Debug.Log("K co ");

    //             }
    //         }

    //     }
    // }
    private void CheckAllBoardForMerger()
    {
        bool mergedOccurredInPass = true;

        while (mergedOccurredInPass)
        {
            mergedOccurredInPass = false;

            for (int r = 1; r <= numRows; r++)
            {
                for (int c = 1; c <= numCols; c++)
                {
                    CellAI currentCell = _matrix[r, c];
                    if (currentCell == null)
                    {
                        // Debug.LogError($"_matrix[{r},{c}] is NULL. Board creation issue?");
                        continue;
                    }

                    // Lấy PlayerAI từ ô hiện tại
                    PlayerAI currentPlayer = GetPlayerAIFromCell(currentCell);
                    if (currentPlayer == null) // Bỏ qua các ô trống hoặc không tìm thấy PlayerAI
                    {
                        continue;
                    }

                    // Tìm kiếm các hàng xóm có thể hợp nhất
                    CellAI mergeableNeighbors = FindMergeableNeighbors(currentCell, currentPlayer);

                    if (mergeableNeighbors != null)
                    {
                        // Lấy CellAI của hàng xóm đầu tiên có thể hợp nhất
                        PlayerAI neighborPlayer = GetPlayerAIFromCell(mergeableNeighbors);

                        if (neighborPlayer == null)
                        {
                            Debug.LogWarning($"Found mergeable neighbor cell ({mergeableNeighbors.row},{mergeableNeighbors.col}) but could not get its PlayerAI. Skipping merge.");
                            continue;
                        }
                        // *** XÓA NHÂN VẬT CŨ TRƯỚC KHI TẠO NHÂN VẬT MỚI ***
                        DestroyCharacter(currentPlayer);
                        DestroyCharacter(neighborPlayer);

                        // Chọn ô để sinh nhân vật mới (ví dụ: ô hiện tại)
                        CellAI spawnMergeCell = currentCell;

                        // Sinh ra nhân vật cấp độ cao hơn
                        int baseLevel = neighborPlayer.BaseLevel + currentPlayer.playerAIItem.baseLevel;
                        PlayerAIItem upgradeItem = null;
                        foreach (var item in tableObjectManage.playerAIConfig.lsPlayerAIItem)
                        {
                            if (item.baseLevel == baseLevel && item.heroType == currentPlayer.playerAIItem.heroType)
                            {
                                // OnUpgradePlayerAI(item, spawnMergeCell);
                                upgradeItem = item;
                                break;
                            }

                        }
                        if (upgradeItem != null)
                        {
                            OnUpgradePlayerAI(upgradeItem, spawnMergeCell);
                            r = numRows + 1;
                            c = numCols + 1;
                            break;

                        }
                        else
                        {
                            Debug.LogWarning($"No upgraded PlayerAIItem found for {currentPlayer.HeroType} at level {baseLevel}. Max level reached or configuration error.");
                        }

                    }
                }
            }
        }
    }
    private PlayerAI GetPlayerAIFromCell(CellAI cell)
    {
        if (cell == null || cell.transform.childCount == 0)
        {
            return null;
        }

        Transform childTransform = cell.transform.GetChild(0);

        PlayerAI playerAI = childTransform.GetComponent<PlayerAI>();
        return playerAI;
    }
    private CellAI FindMergeableNeighbors(CellAI sourceCell, PlayerAI sourcePlayer)
    {

        int[] dr = { 0, 0, 1, -1 }; // Hướng di chuyển (phải, trái, dưới, trên)
        int[] dc = { 1, -1, 0, 0 };

        for (int i = 0; i < dr.Length; i++)
        {
            int neighborRow = sourceCell.row + dr[i];
            int neighborCol = sourceCell.col + dc[i];

            if (neighborRow >= 1 && neighborRow <= numRows &&
                neighborCol >= 1 && neighborCol <= numCols)
            {
                CellAI neighborCell = _matrix[neighborRow, neighborCol];
                PlayerAI neighborPlayer = GetPlayerAIFromCell(neighborCell);

                if (neighborPlayer != null)
                {
                    // Điều kiện hợp nhất: Cùng loại VÀ Cùng cấp độ
                    if (sourcePlayer.playerAIItem.heroType == neighborPlayer.HeroType &&
                        sourcePlayer.playerAIItem.baseLevel == neighborPlayer.BaseLevel)
                    {
                        return neighborCell;
                    }
                }
            }
        }
        return null;
    }
    private void OnUpgradePlayerAI(PlayerAIItem playerAIItem, CellAI cellAI)
    {
        if (playerAIItem == null || cellAI == null)
        {
            Debug.LogError("Nâng cấp thất bại: CharacterItem được nâng cấp hoặc ô mục tiêu là null.");
            return;
        }

        GameObject upgradedPrefab = playerAIItem.playerAIPrefab;
        if (upgradedPrefab == null)
        {
            Debug.LogError("Prefab nhân vật được nâng cấp là null.");
            return;
        }

        GameObject newPlayerAIGO = Instantiate(upgradedPrefab, cellAI.transform);
        newPlayerAIGO.transform.localPosition = new Vector3(0f, -45f, 0f);

        // PlayerAI newPlayerAI = newPlayerAIGO.GetComponentInChildren<PlayerAI>();
        // Debug.Log($"newPlayerAI1:{newPlayerAI.playerAIItem.baseLevel}");

        // if (newPlayerAI != null)
        // {
        //     Debug.Log($"newPlayerAI:{newPlayerAI}");
        //     newPlayerAI.playerAIItem = playerAIItem;
        //     newPlayerAI.currentCell = cellAI;
        //     cellAI.PlayerAIOnCell = newPlayerAI;
        // }
    }


    // public void SpawnUpgradedCharacter(CellAI targetCell, PlayerAIItem playerAIItem)
    // {
    //     if (targetCell == null)
    //     {
    //         Debug.LogError("Target cell for spawning upgraded character is null.");
    //         return;
    //     }

    //     GameObject upgradedItem = playerAIItem.playerAIPrefab;

    //     // if (upgradedItem == null || upgradedItem.playerAIPrefab == null)
    //     // {
    //     //     Debug.LogError($"Could not find upgraded prefab for {heroType} at level {newLevel}. This might be the max level or an unconfigured upgrade path in PlayerAIManager.");
    //     //     return;
    //     // }

    //     GameObject newCharacterGO = Instantiate(upgradedItem, targetCell.transform);
    //     newCharacterGO.transform.localPosition = new Vector3(0f, -45f, 0f);

    //     PlayerAI newPlayerAI = newCharacterGO.GetComponent<PlayerAI>();
    //     if (newPlayerAI == null)
    //     {
    //         Debug.LogError("PlayerAI component not found on upgraded character prefab. Check prefab configuration.");
    //         Destroy(newCharacterGO);
    //         return;
    //     }

    //     // newPlayerAI.SetupPlayerAI(newCharacterGO);
    //     newPlayerAI.currentCell = targetCell; // Giúp nhân vật mới biết nó đang ở ô nào

    //     Debug.Log($"Spawned upgraded {newPlayerAI.HeroType} (Lv{newPlayerAI.BaseLevel}) at cell ({targetCell.row},{targetCell.col})");
    // }

    private void DestroyCharacter(PlayerAI player)
    {
        if (player != null && player.gameObject != null)
        {
            Destroy(player.gameObject);
            Debug.Log($"Character '{player.HeroType} Lv{player.BaseLevel}' destroyed.");
        }
    }
    private void AddPlayerAIOnBoard()
    {
        int random = UnityEngine.Random.Range(0, PlayerAIManager.Instance._lsPlayerAILV1.Count);
        foreach (var item in PlayerAIManager.Instance._lsPlayerAILV1)
        {
            if (_mana >= item.Mana)
            {
                SpawnRandomPlayerAI();
                break;

            }
        }
        // for (int i = 0; i < PlayerAIManager.Instance._lsPlayerAILV1.Count; i++)
        // {
        //     if(random==i){
        //         if(_mana>=i.){

        //         }
        //     }
        // }

    }
    public void ADD()
    {
        SpawnRandomPlayerAI();
    }
}
