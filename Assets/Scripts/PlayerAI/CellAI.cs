using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellAI : MonoBehaviour
{
    public int row, col;
    public PlayerAI currentPlayerAI { get; private set; }
    public PlayerAI PlayerAIOnCell { get; set; } = null;


    // Khi một PlayerAI được đặt vào ô
    public void SetPlayerAI(PlayerAI player)
    {
        currentPlayerAI = player;
        if (player != null)
        {
            // Đảm bảo PlayerAI biết nó đang ở ô nào
            player.currentCell = this;
        }
    }

    // Khi PlayerAI bị xóa khỏi ô
    public void ClearPlayerAI()
    {
        if (currentPlayerAI != null)
        {
            currentPlayerAI.currentCell = null;
        }
        currentPlayerAI = null;
    }

    public bool IsEmpty()
    {
        // Kiểm tra xem có PlayerAI nào trong ô này không
        return currentPlayerAI == null;
    }
}
