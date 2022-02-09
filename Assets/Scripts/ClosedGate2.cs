using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedGate2 : MonoBehaviour
{
    private WorldUIManager uiManager;

    [SerializeField]
    private GameObject openGate;

    [SerializeField]
    private string closedText;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<WorldUIManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gm.killedAllKobolds())
            {
                openGate.SetActive(true);
                gameObject.SetActive(false);

            }
            else
            {
                uiManager.activateBgImage();
                uiManager.activateText();
                uiManager.setText(closedText);
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
