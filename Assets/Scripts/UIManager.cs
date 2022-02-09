using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Text battleText;
    public GameObject ContinueButton;
    public Button EndTurnButton;
    public Button[] ActionButtons;
    public Button[] TargetButtons;

    [SerializeField]
    private 
    // Start is called before the first frame update
    void Start()
    {
        battleText.text = "Everybody Roll Initiative!!!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText(string text)
    {
        battleText.text = text;
    }

    public void appendText(string text)
    {
        battleText.text += "\n" + text;
    }

    public void DeactivateContinueButton()
    {
        ContinueButton.SetActive(false);
    }

    public void activateContinueButton()
    {
        ContinueButton.SetActive(true);
    }

    public void activateEndTurnButton()
    {
        EndTurnButton.gameObject.SetActive(true);
    }

    public void deactivateEndTurnButton()
    {
        EndTurnButton.gameObject.SetActive(false);
    }

    public void activateActionButtons(StatSheet.Action[] actions, bool bonusActionUsed, bool actionUsed, int spellSlots)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            ActionButtons[i].gameObject.SetActive(true);
            ActionButtons[i].GetComponentInChildren<Text>().text = actions[i].Name;
            if ((bonusActionUsed && actions[i].isBonus) || (actionUsed && !actions[i].isBonus) || (actions[i].SpellSlots > spellSlots))
            {
                ActionButtons[i].interactable = false;
            }
        }
    }

    public void deactivateActionButtons()
    {
        for (int i = 0; i < ActionButtons.Length; i++)
        {
            ActionButtons[i].gameObject.SetActive(false);
            ActionButtons[i].interactable = true;
        }
    }

    public void listTargets(List<StatSheet> characters, StatSheet currentCharacter)
    {
        int targetIndex = 0;
        foreach (StatSheet character in characters)
        {
            TargetButtons[targetIndex].gameObject.SetActive(true);
            TargetButtons[targetIndex].GetComponentInChildren<Text>().text = character.characterName;
            if (character == currentCharacter || character.isDead)
            {
                TargetButtons[targetIndex].interactable = false;
            }
            targetIndex++;
        }
    }

    public void deactivateTargetButtons()
    {
        foreach (Button b in TargetButtons)
        {
            b.gameObject.SetActive(false);
            b.interactable = true;
        }
    }
}
