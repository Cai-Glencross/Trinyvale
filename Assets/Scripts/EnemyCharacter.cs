using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField]
    public int maxMovement;

    [SerializeField]
    public GridManager gridManager;

    [SerializeField]
    public GridCell occupiedCell;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveCharacter()
    {
        for (int i = 0; i < maxMovement; i++)
        {
            if (Random.Range(0f, 1f) <= 0.8f)
            {
                int randomIndex = Random.Range(0, occupiedCell.adjacentCells.Count - 1);
                GridCell newCell = occupiedCell.adjacentCells[randomIndex];
                this.transform.position = newCell.transform.position;
                this.occupiedCell = newCell;
            }
        }
    }

    public void MoveCharacter(List<PlayerCharacter> playerCharacters)
    {
        foreach(PlayerCharacter player in playerCharacters) {
            if (this.occupiedCell.adjacentCells.Contains(player.occupiedCell))
            {
                this.transform.position = player.occupiedCell.transform.position;
                this.occupiedCell = player.occupiedCell;
                return;
            }
        }
        for (int i = 0; i < maxMovement; i++)
        {
            if (Random.Range(0f, 1f) <= 0.8f)
            {
                int randomIndex = Random.Range(0, occupiedCell.adjacentCells.Count - 1);
                GridCell newCell = occupiedCell.adjacentCells[randomIndex];
                this.transform.position = newCell.transform.position;
                this.occupiedCell = newCell;
            }
        }
    }
}
