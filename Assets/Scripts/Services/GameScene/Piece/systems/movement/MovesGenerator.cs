using UnityEngine;
using Zenject;

public class MovesGenerator : MonoBehaviour
{
    [Inject] GameManager gameManager;

    public void GenerateAllMove()
    {
        for(byte index = 0; index < 64; index++)
        {
            if (PiecePacker.IsDefaultPiece(ref gameManager.Board[index]))
            {

            }
        }
    }
}
