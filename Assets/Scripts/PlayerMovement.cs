using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D myRigidBody;
    private Animator anim;
    private SpriteRenderer _sprite;
    private GameManager gm;
    private WorldUIManager uiManager;

    [SerializeField]
    private StatSheet stats;
    [SerializeField]
    private int speed;

    public bool hasKey;

    void Start()
    {
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<WorldUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (change != Vector3.zero && !gm.playerFrozen)
        {
            Move();
            anim.SetFloat("moveX", change.x);
            anim.SetFloat("moveY", change.y);
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void Move()
    {
        Vector3 delta = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized;
        myRigidBody.MovePosition(

            transform.position + delta * speed * Time.deltaTime
        );
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision has been detected with a " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("battle has been triggered!!!");
            //gm.startBattle(collision.gameObject.GetComponent<Enemy>());
            //Destroy(collision.gameObject);
        } else if (collision.gameObject.tag == "Key")
        {
            hasKey = true;
            Destroy(collision.gameObject);
            gm.freezePlayer();
            uiManager.setText("Woohoo you found a key! \nUse it to unlock the Eastern Gate!!");
            uiManager.activateBgImage();
            uiManager.activateText();
            uiManager.activateKeyContinueButton();
        }
    }
}
