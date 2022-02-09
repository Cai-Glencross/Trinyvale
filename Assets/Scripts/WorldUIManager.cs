using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUIManager : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button[] partyCharacterButtons;
    [SerializeField]
    private Image bgImage;
    [SerializeField]
    private Button[] gnomeButtons;
    [SerializeField]
    private Button persuadeContinueButton;
    [SerializeField]
    private Button keyContinueButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText(string txt)
    {
        text.text = txt;
    }

    public void activateContinueButton()
    {
        continueButton.gameObject.SetActive(true);
    }

    public void deactivateContinueButton()
    {
        continueButton.gameObject.SetActive(false);
    }

    public void activateCharacterButtons()
    {
        foreach (Button b in partyCharacterButtons)
        {
            b.gameObject.SetActive(true);
        }
    }

    public void deactivateCharacterButtons()
    {
        foreach(Button b in partyCharacterButtons)
        {
            b.gameObject.SetActive(false);
        }
    }

    public void activateBgImage()
    {
        bgImage.gameObject.SetActive(true);
    }
    public void deactivateBgImage()
    {
        bgImage.gameObject.SetActive(false);
    }

    public void activateText()
    {
        text.gameObject.SetActive(true);
    }

    public void deactivateText()
    {
        text.gameObject.SetActive(false);
    }

    public void activateGnomeButtons()
    {
        foreach (Button b in gnomeButtons)
        {
            b.gameObject.SetActive(true);
        }
    }

    public void deactivateGnomeButtons()
    {
        foreach (Button b in gnomeButtons)
        {
            b.gameObject.SetActive(false);
        }
    }

    public void activatePersuadeContinueButton()
    {
        persuadeContinueButton.gameObject.SetActive(true);
    }

    public void deactivatePersuadeContinueButton()
    {
        persuadeContinueButton.gameObject.SetActive(false);
    }

    public void activateKeyContinueButton()
    {
        keyContinueButton.gameObject.SetActive(true);
    }

    public void deactivateKeyContinueButton()
    {
        keyContinueButton.gameObject.SetActive(false);
    }
}
