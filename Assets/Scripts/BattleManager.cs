using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private GameObject[] partyMembers;

    private enum BattleState {Begin, Initiative, Fight, End};
    private int currentState;
    private enum FightState {Start, ChooseAction, ChooseTarget, Act, End}
    private int currentFightState;
    private bool actionUsed;
    private bool bonusActionUsed;

    private UIManager uiManager;
    private string statusText;

    private StatSheet[] charactersInvolved;
    private int[] characterOrder;
    private int currentCharacterIndex;
    private StatSheet.Action currentAction;
    private List<StatSheet> possibleTargets;

    private bool canContinue;
    private float activeOffset = 0.2f;
    private float topOut = 1.5f;
    private float bottomOut = 0f;

    void Start()
    {
        charactersInvolved = BattleParams.Characters;
        int partyIndex = 0;
        int enemyIndex = 0;
        foreach (StatSheet character in charactersInvolved)
        {
            if (character.isEnemy)
            {
                character.gameObject = enemies[enemyIndex];
                enemyIndex++;
            }
            else
            {
                character.gameObject = partyMembers[partyIndex];
                partyIndex++;
            }
            character.gameObject.SetActive(true);
            character.startHeight = character.gameObject.transform.position.y;
            if (System.Math.Abs(character.scale - 0) > 0.0000000000000000000001)
            {
                character.gameObject.transform.localScale = new Vector3(character.scale * 40, character.scale*40, 1);
            }
            character.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = character.sprite;
        }

        currentState = (int)BattleState.Begin;
        currentFightState = (int)FightState.Start;
        actionUsed = false;
        bonusActionUsed = false;

        uiManager = GameObject.Find("BattleCanvas").GetComponent<UIManager>();
        statusText = "";
        canContinue = false;
        currentCharacterIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == (int)BattleState.Begin)
        {
            Beginning();
        }
        if (currentState == (int)BattleState.Initiative)
        {
            Initiate();
        }
        if (currentState == (int)BattleState.Fight)
        {
            Fight();
        }
        if (currentState == (int)BattleState.End)
        {
            uiManager.setText("All enemies are dead. You are out of initiative!! Good job!");
        }
        if (BattleIsOver())
        {
            currentState = (int)BattleState.End;
        }
    }

    void Beginning()
    {

    }
    
    void Initiate()
    {
        if (characterOrder == null)
        {
            RollInitiative();
            string characterOrderString = "The order will be: ";
            for (int i = 0; i < characterOrder.Length; i++)
            {
                if (i != characterOrder.Length - 1)
                {
                    characterOrderString += charactersInvolved[characterOrder[i]].characterName + "-> ";
                }
                else
                {
                    characterOrderString += charactersInvolved[characterOrder[i]].characterName;
                }
            }
            uiManager.setText(characterOrderString);
        }
    }

    void Fight()
    {
        //if everyone has gone, start the second round
        if (currentCharacterIndex >= characterOrder.Length) { currentCharacterIndex = 0; }

        StatSheet currentCharacter = charactersInvolved[characterOrder[currentCharacterIndex]];
        //choose an action
        if (currentFightState == (int)FightState.Start)
        {
            if (!currentCharacter.isEnemy)
            {
                uiManager.activateEndTurnButton();
            }
            else
            {
                uiManager.deactivateEndTurnButton();
            }
            UpdateStatusText();
            uiManager.setText("That's " + currentCharacter.characterName + "'s turn" + statusText);

            if(!currentCharacter.isEnemy)
            {
                uiManager.activateActionButtons(currentCharacter.actions, bonusActionUsed, actionUsed, currentCharacter.currentSpellSlots);
            } else
            {
                //choose a random action
                DoItUp(Random.Range(0, currentCharacter.actions.Length));
            }
        }
        if (currentFightState == (int)FightState.ChooseAction)
        {
            if (!currentCharacter.isEnemy)
            {
                uiManager.activateActionButtons(currentCharacter.actions, bonusActionUsed, actionUsed, currentCharacter.currentSpellSlots);
                uiManager.activateEndTurnButton();
            }
        }
        MoveCharacter(currentCharacter);
        MoveCharactersBack();
    }

    public void EndTurn()
    {
        currentCharacterIndex++;
        currentFightState = (int)FightState.Start;
        bonusActionUsed = false;
        actionUsed = false;
        uiManager.DeactivateContinueButton();
        uiManager.deactivateActionButtons();
        uiManager.deactivateTargetButtons();
    }

    public void DoItUp(int actionIndex)
    {
        StatSheet currentCharacter = charactersInvolved[characterOrder[currentCharacterIndex]];
        currentAction = currentCharacter.actions[actionIndex];
        currentFightState = (int)FightState.ChooseAction;
        //check if you can make that action
        if (currentAction.isBonus && bonusActionUsed)
        {
            uiManager.setText(currentCharacter.characterName + " already used their bonus action this round" + statusText);
            return;
        }
        if (!currentAction.isBonus && actionUsed)
        {
            uiManager.setText(currentCharacter.characterName + " already used their action this round"+statusText);
            return;
        }
        currentFightState = (int)FightState.ChooseTarget;
        possibleTargets = new List<StatSheet>();
        foreach (StatSheet character in charactersInvolved)
        {
            if (currentAction.targetEnemy)
            {
                if (character.isEnemy != currentCharacter.isEnemy)
                {
                    possibleTargets.Add(character);
                }
            }
            else
            {
                if (character.isEnemy == currentCharacter.isEnemy)
                {
                    possibleTargets.Add(character);
                }
            }

        }

        if (currentCharacter.isEnemy)
        {
            //attack a random enemy
            DoItToHim(Random.Range(0, possibleTargets.Count));

            //enemies (for now) can't use bonus actions
            bonusActionUsed = true;
        }
        else
        {
            uiManager.setText("choose a target for " + currentAction.Name+ statusText);
            uiManager.deactivateActionButtons();
            uiManager.listTargets(possibleTargets, currentCharacter);
        }

    }

    public void DoItToHim(int targetIndex)
    {
        StatSheet currentTarget = possibleTargets[targetIndex];
        StatSheet currentCharacter = charactersInvolved[characterOrder[currentCharacterIndex]];

        Animator currentAnim = currentCharacter.gameObject.GetComponentInChildren<Animator>();
        Animator targetAnim = currentTarget.gameObject.GetComponentInChildren<Animator>();


        int amount = Random.Range(1, currentAction.Die + 1);
        currentFightState = (int)FightState.Act;

        uiManager.deactivateEndTurnButton();

        currentCharacter.currentSpellSlots -= currentAction.SpellSlots;
        UpdateStatusText();
        if (currentAction.Name == "Attack")
        {
            //roll against armor class
            int toHitModifier = 0;
            if (currentCharacter.attackType == "Strength")
            {
                toHitModifier = getModifier(currentCharacter.strength);
                amount += getModifier(currentCharacter.strength);
            } else if (currentCharacter.attackType == "Dexterity")
            {
                toHitModifier = getModifier(currentCharacter.dexterity);
                amount += getModifier(currentCharacter.dexterity);
            }
            if (currentTarget.isHexed && currentCharacter.characterName == "Onyx Lumiere") 
            {
                //do an extra 1d6 of damage if you hexed them.
                amount += Random.Range(0, 7);
            }
            if (currentTarget.isMarked && currentCharacter.characterName == "Nyack")
            {
                //do an extra 1d6 of damage if you marked them.
                amount += Random.Range(0, 7);
            }
            int toHit = Random.Range(1,21) + currentCharacter.proficiency + toHitModifier;
            if (currentCharacter.isMocked)
            {
                //mocked characters roll with disadvantage for a turn
                toHit = Mathf.Min(toHit, Random.Range(1, 21) + currentCharacter.proficiency + toHitModifier);
                currentCharacter.isMocked = false;
            }
            if (currentCharacter.isInspired)
            {
                //add an extra d6 if you have bardic inspiration
                toHit += Random.Range(1, 7);
                currentCharacter.isInspired = false;
            }
            Debug.Log("you rolled a " + toHit);
            if (toHit > currentTarget.armorClass)
            {
                uiManager.setText(currentCharacter.characterName + " hit " + currentTarget.characterName + " for " + amount+statusText);
                currentTarget.currentHp -= amount;

                ToggleHitAnim(currentAnim);
                ToggleHurtAnim(targetAnim);

            }
            else
            {
                uiManager.setText(currentCharacter.characterName + " missed!!"+statusText);
            }
        }
        if (currentAction.Name == "Hunter's Mark")
        {
            currentTarget.isMarked = true;
            uiManager.setText(currentTarget.characterName + 
                " has been marked! You'll do an extra 1d6 of damage next time you attack them"+statusText);
        }
        if (currentAction.Name == "Hex")
        {
            currentTarget.isHexed = true;
            uiManager.setText(currentTarget.characterName + 
                " has been hexed. You'll do an extra 1d6 of damage next time you attack them"+statusText);
        }
        if (currentAction.Name == "Vicious Mockery")
        {
            int spellSaveDC = 8 + getModifier(currentCharacter.charisma);

            int savingRoll = Random.Range(1, 21) + getModifier(currentTarget.wisdom);
            if (savingRoll > spellSaveDC)
            {
                uiManager.setText(currentTarget.characterName + " succeeded their saving throw!"+statusText);
            }
            else
            {
                currentTarget.isMocked = true;
                amount = Random.Range(1, 5);
                currentTarget.currentHp -= amount;
                uiManager.setText("a Hit! " + currentTarget.characterName + "took " + amount + " damage and has disadvantage on their next attack!"+statusText);
            }
        }
        if (currentAction.Name == "Bardic Inspiration")
        {
            currentTarget.isInspired = true;
            uiManager.setText("you inspired " + currentTarget.characterName + ". They will get an extra 1d6 on their next attack roll"+statusText);
        }
        if (currentAction.Name == "Healing Word")
        {
            currentTarget.currentHp += amount;
            uiManager.setText("you healed " + currentTarget.characterName + " for " + amount + statusText);
        }
        if (currentAction.Name == "Eldritch Blast")
        {
            //you make an attack roll basically
            int toHit = Random.Range(1, 21) + 2 + getModifier(currentCharacter.charisma);
            if (currentCharacter.isInspired)
            {
                toHit += Random.Range(1, 7);
                currentCharacter.isInspired = false;
            }
            if (currentTarget.isHexed)
            {
                amount += Random.Range(1, 7);
            }
            if (toHit >= currentTarget.armorClass)
            {
                uiManager.setText("you blast " + currentTarget.characterName + " for " + amount + statusText);
                currentTarget.currentHp -= amount;
            }
            else
            {
                uiManager.setText("You missed!!!");
            }
        }
        (currentAction.isBonus ? ref bonusActionUsed : ref actionUsed) = true;

        currentFightState = (int)FightState.End;
        uiManager.deactivateTargetButtons();
        uiManager.activateContinueButton();



        if (currentTarget.currentHp <= 0)
        {
            currentTarget.isDead = true;
            currentTarget.gameObject.SetActive(false);
            if (!currentTarget.isEnemy)
            {
                SceneManager.LoadScene("GameOver");
            }

        }

    }




    void RollInitiative()
    {
        int numCharacters = charactersInvolved.Length;
        characterOrder = new int[numCharacters];
        int[] charactersInitiative = new int[numCharacters];

        for (int i = 0; i < numCharacters; i++)
        {
            StatSheet thisCharacter = charactersInvolved[i];
            charactersInitiative[i] = Random.Range(1, 21) + getModifier(thisCharacter.dexterity);
        }
        for (int i = 0; i < numCharacters; i++)
        {
            characterOrder[i] = System.Array.IndexOf(charactersInitiative, charactersInitiative.Max());
            charactersInitiative[characterOrder[i]] = -1;
        }

    }
    void MoveCharacter(StatSheet character)
    {
        GameObject go = character.gameObject;
        float currentHeight = go.transform.position.y;

        if (character.isEnemy && currentHeight > bottomOut)
        {
            go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y - activeOffset, go.transform.position.z);
        }
        else if (!character.isEnemy && currentHeight < topOut)
        {
            go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + activeOffset, go.transform.position.z);
        }
    }

    void MoveCharactersBack()
    {
        StatSheet currentCharacter = charactersInvolved[characterOrder[currentCharacterIndex]];
        foreach (StatSheet character in charactersInvolved)
        {
            if (character == currentCharacter) { continue; }

            GameObject go = character.gameObject;
            float currentHeight = go.transform.position.y;
            if (character.isEnemy && currentHeight < character.startHeight)
            {
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + activeOffset, go.transform.position.z);
            }
            else if (!character.isEnemy && currentHeight > character.startHeight)
            {
                go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y - activeOffset, go.transform.position.z);
            }
        }
    }

    public void IncrementState()
    {
        if (currentState != (int)BattleState.Fight)
        {
            currentState++;
            if (currentState == (int)BattleState.Fight)
            {
                uiManager.DeactivateContinueButton();
            }
            if (currentState > (int)BattleState.End)
            {
                SceneManager.UnloadSceneAsync("Battle");
            }
        }
        else
        {
            if (actionUsed && !bonusActionUsed)
            {
                currentFightState = (int)FightState.ChooseAction;
                uiManager.setText("You may now use a bonus action"+statusText);
                uiManager.DeactivateContinueButton();

            } else if (bonusActionUsed && !actionUsed)
            {
                currentFightState = (int)FightState.ChooseAction;
                uiManager.setText("You may now use your action"+statusText);
                uiManager.DeactivateContinueButton();
            }
            else
            {
                currentCharacterIndex++;
                if (currentCharacterIndex >= characterOrder.Length)
                {
                    currentCharacterIndex = 0;
                }
                currentFightState = (int)FightState.Start;
                uiManager.DeactivateContinueButton();
                bonusActionUsed = false;
                actionUsed = false;
                while (charactersInvolved[characterOrder[currentCharacterIndex]].isDead)
                {
                    currentCharacterIndex++;
                    if (currentCharacterIndex >= characterOrder.Length)
                    {
                        currentCharacterIndex = 0;
                    }
                }
            }
            ResetAnims();

        }
    }

    public void ToggleHitAnim(Animator anim)
    {
        if (!anim.GetBool("isAttacking"))
        {
            Debug.Log("setting isAttaking");
            anim.SetBool("isAttacking", true);
        }
    }
    public void ToggleHurtAnim(Animator anim)
    {
        if (!anim.GetBool("isHurt"))
        {
            Debug.Log("setting ishurt");
            anim.SetBool("isHurt", true);
        }
    }

    public void ResetAnims()
    {
        foreach (StatSheet character in charactersInvolved)
        {
            Animator anim = character.gameObject.GetComponentInChildren<Animator>();
            anim.SetBool("isHurt", false);
            anim.SetBool("isAttacking", false);
        }
    }

    public void UpdateStatusText()
    {
        StatSheet currentCharacter = charactersInvolved[characterOrder[currentCharacterIndex]];
        if (!currentCharacter.isEnemy)
        {
            statusText = "\n~" + currentCharacter.characterName + "~\n" + "Health: " + currentCharacter.currentHp + " / " + currentCharacter.maxHp + "\n" +
            "SpellSlots: " + currentCharacter.currentSpellSlots + " / " + currentCharacter.maxSpellSlots;
        }
        else
        {
            statusText = "";
        }
    }

    public bool BattleIsOver()
    {
        List<StatSheet> allEnemies = new List<StatSheet>();
        foreach (StatSheet character in charactersInvolved)
        {
            if (character.isEnemy && character.currentHp > 0)
            {
                return false;
            }
        }

        return true;
    }

    int getModifier(int score)
    {
        //To determine an ability modifier without consulting the table, 
        //subtract 10 from the ability score and then divide the total by 2 (round down).

        return (score - 10) / 2;
    }

    IEnumerator WaitToContinue(float duration)
    {
        yield return new WaitForSeconds(duration);
        Debug.Log("canContinue should be true now");
        canContinue = true;
        Debug.Log(canContinue);
    }

}
