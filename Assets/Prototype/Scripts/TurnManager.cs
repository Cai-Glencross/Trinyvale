using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public bool isYourTurn;
    public bool gameOver;

    [SerializeField]
    private GridManager gridManager;

    public PlayerCharacter activePlayerCharacter;
    public EnemyCharacter activeEnemyCharacter;

    public Text turnText;

    public List<EnemyCharacter> enemyCharacters;
    public List<PlayerCharacter> playerCharacters;
    // Start is called before the first frame update
    void Start()
    {
        isYourTurn = true;
        activePlayerCharacter = null;
        activeEnemyCharacter = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (isYourTurn && !gameOver)
        {
            if (activePlayerCharacter == null)
            {
                turnText.text = "Your Turn \n Click the character to move ";
            }
            else
            {
                turnText.text = "Click the cell where you would like to move " + activePlayerCharacter.name;
            }
        }
        else if (!gameOver)
        {
            if (activeEnemyCharacter == null)
            {
                turnText.text = "Enemy Turn!";
                bool canKill = false;
                foreach (EnemyCharacter enemyCharacter in enemyCharacters)
                {
                    foreach (PlayerCharacter playerCharacter in playerCharacters)
                    {
                        if (enemyCharacter.occupiedCell.adjacentCells.Contains(playerCharacter.occupiedCell))
                        {
                            canKill = true;
                            activeEnemyCharacter = enemyCharacter;
                            StartCoroutine(turnWithWait());
                        }
                    }
                }
                if (!canKill)
                {
                    Debug.Log("enemy turn and can't kill");
                    int randomIndex = Random.Range(0, enemyCharacters.Count);
                    Debug.Log("index is " + randomIndex);
                    activeEnemyCharacter = enemyCharacters[randomIndex];
                    StartCoroutine(turnWithWait());
                }

            }
        }
    }


    public void PlayerCharacterClicked(PlayerCharacter playerCharacter)
    {
        if (isYourTurn && activePlayerCharacter == null)
        {
            activePlayerCharacter = playerCharacter;
        }
    }

    public void GridCellClicked(GridCell gridCell)
    {
        if (isYourTurn && activePlayerCharacter != null)
        {
            Debug.Log("distance is: " + gridManager.GetDistanceBetweenCells(gridCell, activePlayerCharacter.occupiedCell));
            if (gridManager.GetDistanceBetweenCells(gridCell, activePlayerCharacter.occupiedCell) 
                    <= activePlayerCharacter.maxMovement) {
                activePlayerCharacter.MoveToGridCell(gridCell);
                isYourTurn = false;
                List<EnemyCharacter> enemiesToDestroy = new List<EnemyCharacter>();
                foreach (EnemyCharacter enemy in enemyCharacters)
                {
                    if (activePlayerCharacter.occupiedCell == enemy.occupiedCell)
                    {
                        enemiesToDestroy.Add(enemy);
                    }
                }
                foreach(EnemyCharacter enemy in enemiesToDestroy)
                {
                    enemyCharacters.Remove(enemy);
                    Destroy(enemy.gameObject);
                }
                activePlayerCharacter = null;
            }

        }

        if (enemyCharacters.Count == 0)
        {
            gameOver = true;
            turnText.text = "Nice work you defeated the enemies!!";
        }
    }

    IEnumerator turnWithWait()
    {
        yield return new WaitForSeconds(1.5f);
        activeEnemyCharacter.MoveCharacter(playerCharacters);

        List<PlayerCharacter> playersToDestroy = new List<PlayerCharacter>();
        foreach (PlayerCharacter player in playerCharacters)
        {
            if (activeEnemyCharacter.occupiedCell == player.occupiedCell)
            {
                playersToDestroy.Add(player);
            }
        }
        foreach (PlayerCharacter player in playersToDestroy)
        {
            playerCharacters.Remove(player);
            Destroy(player.gameObject);
        }
        if (playerCharacters.Count == 0)
        {
            gameOver = true;
            turnText.text = "You have been defeated by the monsters :(";
        }
        isYourTurn = true;
        activeEnemyCharacter = null;
    }
}
