using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{

    public GridCell[] cellArray;
    public TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        cellArray = this.transform.GetComponentsInChildren<GridCell>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetDistanceBetweenCells(GridCell start, GridCell finish)
    {
        List<GridCell> unvisitedCells = new List<GridCell>(cellArray.Length);
        foreach (GridCell cell in cellArray)
        {
            unvisitedCells.Add(cell);
        }

        int[] cellDistances = new int[cellArray.Length];
        for(int i = 0; i < cellDistances.Length; i++)
        {
            cellDistances[i] = Int32.MaxValue;
        }

        GridCell currentCell = start;
        cellDistances[Array.IndexOf(cellArray, start)] = 0;
        while (unvisitedCells.Count > 0)
        {
            int currentCellIndex = Array.IndexOf(cellArray, currentCell);
            foreach(GridCell adjacentCell in currentCell.adjacentCells)
            {
                int adjacentCellIndex = Array.IndexOf(cellArray, adjacentCell);
                if (cellDistances[adjacentCellIndex] > cellDistances[currentCellIndex]+1)
                {
                    cellDistances[adjacentCellIndex] = cellDistances[currentCellIndex] + 1;
                    if (adjacentCell == finish)
                    {
                        return cellDistances[adjacentCellIndex];
                    }
                }
            }
            unvisitedCells.Remove(currentCell);
            currentCell = GetNextMinDistanceUnvisitedCell(unvisitedCells, cellDistances);
        }
        return cellDistances[Array.IndexOf(cellArray, finish)];

    }


    private GridCell GetNextMinDistanceUnvisitedCell(List<GridCell> unvisitedCells, int[] minDistance)
    {
        int currentMinDistance = Int32.MaxValue;
        GridCell currentMinDistanceCell = null;
        foreach(GridCell cell in unvisitedCells)
        {
            if (minDistance[Array.IndexOf(cellArray, cell)] < currentMinDistance)
            {
                currentMinDistanceCell = cell;
            }
        }
        return currentMinDistanceCell;
    }

    GridCell GetActiveCell()
    {
        foreach (GridCell cell in cellArray)
        {
            if (cell.isActive)
            {
                return cell;
            }
        }

        return null;
    }

    public void OnMouseDown()
    {
        turnManager.GridCellClicked(this.GetActiveCell());
    }

}
