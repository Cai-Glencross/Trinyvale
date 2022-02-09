using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    private WorldUIManager uiManager;

    [SerializeField]
    private bool isUsed;

    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<WorldUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isUsed)
        {
            gm.getBB();
            isUsed = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            uiManager.deactivateText();
            uiManager.deactivateBgImage();
            uiManager.deactivateContinueButton();
            uiManager.deactivateCharacterButtons();
        }
    }
}
