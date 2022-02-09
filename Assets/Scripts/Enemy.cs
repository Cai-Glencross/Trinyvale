using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public StatSheet[] stats;

    private GameManager gm;

    private bool triggered;
    private GameObject target;

    [SerializeField]
    private int speed = 2;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        triggered = false;
    }

    private void Update()
    {
        if (triggered)
        {
            Vector3 difference = target.transform.position - transform.position;
            transform.position += difference.normalized * speed * Time.deltaTime;
            if (difference.magnitude < 1)
            {
                gm.startBattle(this);
                gm.unfreezePlayer();
                Destroy(this.gameObject);

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && this.gameObject.name != "GnomeTrigger")
        {
            target = collision.gameObject;
            triggered = true;
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());
            gm.freezePlayer();
        }
    }

}
