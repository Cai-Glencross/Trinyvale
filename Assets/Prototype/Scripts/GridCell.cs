using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{

    public List<GridCell> adjacentCells;

    [SerializeField]
    private float adjacentDistance = Mathf.Sqrt(2);

    [SerializeField]
    public GridManager gridManager;

    [SerializeField]
    public TurnManager turnManager;

    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GridCell cell in gridManager.cellArray) {
            if (cell != this && (this.transform.position - cell.transform.position).magnitude <= adjacentDistance)
            {
                adjacentCells.Add(cell);
            }
        }
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);

        if (this.GetComponent<SpriteRenderer>().bounds.Contains(mousePosition))
        {
            //Debug.Log(this.gameObject.name + "is hovered over");
            isActive = true;
            if (turnManager.activePlayerCharacter != null)
            {
                if (gridManager.GetDistanceBetweenCells(this, turnManager.activePlayerCharacter.occupiedCell) <= turnManager.activePlayerCharacter.maxMovement)
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(0.5f, 1, 0.5f);
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f);
                }
            }
            else
            {
                this.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1f);
            }
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.white;
            isActive = false;
        }
    }
}

//$50 per sprite.