using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaravanMovement : MonoBehaviour
{
    private Rigidbody2D myRigidBody;
    private Rigidbody2D leaderRigidBody;

    private Animator anim;

    [SerializeField]
    private StatSheet stats;

    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject leader;
    [SerializeField]
    private float followDistance;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        leaderRigidBody = leader.GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 change = leaderRigidBody.transform.position - myRigidBody.transform.position;
        if (change.magnitude > followDistance)
        {
            Move(change);
            anim.SetFloat("moveX", change.x);
            anim.SetFloat("moveY", change.y);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    private void Move(Vector3 change)
    {
        myRigidBody.MovePosition(
            transform.position + change.normalized * speed * Time.deltaTime
        );
    }
}
