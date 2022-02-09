using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private TurnManager turnManager;

    [SerializeField]
    public GridCell occupiedCell;

    [SerializeField]
    public int maxMovement;


    private void OnMouseDown()
    {
        this.turnManager.PlayerCharacterClicked(this);
    }

    public void MoveToGridCell(GridCell cell)
    {
        this.transform.position = cell.transform.position;
        this.occupiedCell = cell;
    }
}
