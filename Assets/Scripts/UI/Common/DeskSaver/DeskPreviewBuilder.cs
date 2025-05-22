using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DeskPreviewBuilder : MonoBehaviour
{
    [Inject] 
    public void BuildBoard(List<Transform> Cells, byte[] board)
    {

    }
}
