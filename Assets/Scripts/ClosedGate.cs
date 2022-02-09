using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedGate : MonoBehaviour
{

    private WorldUIManager uiManager;
    [SerializeField]
    GameObject openGate;
    [SerializeField]
    private string closedText;

    private void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<WorldUIManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement pm = collision.gameObject.GetComponent<PlayerMovement>();
            if (pm.hasKey)
            {
                this.gameObject.SetActive(false);
                openGate.SetActive(true);
            }
            else
            {
                uiManager.setText(closedText);
                uiManager.activateBgImage();
                uiManager.activateText();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            uiManager.deactivateText();
            uiManager.deactivateBgImage();
        }
    }
}
