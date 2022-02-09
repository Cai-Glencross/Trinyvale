using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private StatSheet[] gnomeStats;

    [SerializeField]
    private GameObject[] gnomeSprites;

    private Enemy gnomeEnemy;

    private GameManager gm;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gnomeEnemy = GetComponent<Enemy>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gm.triggerGnome(gnomeEnemy, gnomeSprites);
    }

}
