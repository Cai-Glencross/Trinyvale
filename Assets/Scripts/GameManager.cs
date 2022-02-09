using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private StatSheet onyxStats;
    [SerializeField]
    private StatSheet jensStats;
    [SerializeField]
    private StatSheet nyackStats;
    [SerializeField]
    private AudioSource music;

    private StatSheet[] partyStats;

    private Enemy gnomeEnemy;

    private int numBBs = 0;
    private bool persuaded = false;
    private StatSheet[] gnomeStats;
    private GameObject[] gnomeSprites;

    private WorldUIManager uiManager;

    public List<string> enemiesDefeated;
    public string[] koboldNames = {"Bertha", "Oliver", "Steve", "Orin", "Bungus"};
    public bool playerFrozen;

    private void Start()
    {
        resetStatSheet(onyxStats);
        resetStatSheet(jensStats);
        resetStatSheet(nyackStats);

        partyStats = new StatSheet[3]{ jensStats, onyxStats, nyackStats};
        gnomeEnemy = new Enemy();
        playerFrozen = false;

        uiManager = GameObject.Find("Canvas").GetComponent<WorldUIManager>();
    }

    private void Update()
    {
        int numScenes = SceneManager.sceneCount;
        if (numScenes > 1)
        {
            music.mute = true;
        }
        else
        {
            music.mute = false;
        }
    }

    public void startBattle(Enemy enemy)
    {

        StatSheet[] charactersInvolved = new StatSheet[3 + enemy.stats.Length];
        charactersInvolved[0] = jensStats;
        charactersInvolved[1] = nyackStats;
        charactersInvolved[2] = onyxStats;
        int index = 3;
        foreach (StatSheet enemyStats in enemy.stats)
        {
            ResetEnemyStats(enemyStats);
            enemiesDefeated.Add(enemyStats.characterName);
            charactersInvolved[index] = enemyStats;
            index++;
        }

        BattleParams.Characters = charactersInvolved;

        SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Additive);
    }

    void ResetEnemyStats(StatSheet Enemy)
    {
        Enemy.currentHp = Enemy.maxHp;
        Enemy.isHexed = false;
        Enemy.isMarked = false;
        Enemy.isMocked = false;
        Enemy.isDead = false;
    }

    public void resetStatSheet(StatSheet stats)
    {
        stats.currentHp = stats.maxHp;
        stats.isDead = false;
        stats.isInspired = false;
        stats.currentSpellSlots = stats.maxSpellSlots;
    }

    public void getBB()
    {
        Debug.Log("getting your bb");

        uiManager.setText("Welcome to Delurio's Curios!\n Roll to see how many Butterfinger BB's (TM) you can give to a member of your party!");
        uiManager.activateText();
        uiManager.activateBgImage();

        numBBs = Random.Range(1, 7);

        uiManager.activateContinueButton();

    }

    public void giveBB()
    {
        uiManager.deactivateContinueButton();
        string healthString = "you may give " + numBBs + " to the party member of your choosing, who will it be?";
        healthString += "Jens: " + jensStats.currentHp + "/" + jensStats.maxHp + ", ";
        healthString += "Onyx: " + onyxStats.currentHp + "/" + onyxStats.maxHp + ", ";
        healthString += "Nyack: " + nyackStats.currentHp + "/" + nyackStats.maxHp;
        uiManager.setText(healthString);

        uiManager.activateCharacterButtons();
    }

    public void eatBB(int i)
    {
        partyStats[i].currentHp += numBBs;
        if(partyStats[i].currentHp > partyStats[i].maxHp)
        {
            partyStats[i].currentHp = partyStats[i].maxHp;
        }

        uiManager.deactivateCharacterButtons();
        uiManager.deactivateBgImage();
        uiManager.deactivateText();
        numBBs = 0;

    }


    public void triggerGnome(Enemy enemy, GameObject[] gnomeObjs)
    {
        gnomeEnemy = enemy;
        this.gnomeSprites = gnomeObjs;

        uiManager.activateBgImage();
        uiManager.activateText();
        uiManager.setText("You see two gnomes zippy-dee-dooing ahead... \nWhat will you do?");

        uiManager.activateGnomeButtons();
    }

    public void persuade()
    {
        uiManager.deactivateGnomeButtons();
        int dc = 8;

        int persuasion_roll = Random.Range(1, 21) + getModifier(jensStats.charisma);

        if (persuasion_roll > dc)
        {
            uiManager.setText("What a dancer!!! Toogle and Doogle are moved by your lack of genitalia. You see the Gnomes disappear in a poof of magic");
            persuaded = true;
            uiManager.activatePersuadeContinueButton();
        }
        else
        {
            uiManager.setText("Uh oh, the gnomes seem confused and angry you that can be nude with no man-dagger to be seen. They attack!!");
            persuaded = false;
            uiManager.activatePersuadeContinueButton();
        }
    }

    public void persuadeContinue()
    {
        foreach (GameObject go in gnomeSprites)
        {
            go.SetActive(false);
        }
        uiManager.deactivateText();
        uiManager.deactivateBgImage();
        uiManager.deactivatePersuadeContinueButton();
        if (!persuaded)
        {
            //gonna have to pass an enemy object, just need to use that script instead of the custom trigger one, shouldnt be too bad
            startBattle(gnomeEnemy);
        }
        Destroy(GameObject.Find("GnomeTrigger"));
    }

    public void proceed()
    {
        uiManager.deactivateText();
        uiManager.deactivateBgImage();
        uiManager.deactivateGnomeButtons();

        Destroy(GameObject.Find("GnomeTrigger"));
    }

    public bool killedAllKobolds()
    {
        bool allKilled = true;
        foreach (string myName in koboldNames)
        {
            allKilled = allKilled && (enemiesDefeated.Contains(myName));
        }
        return allKilled;
    }

    int getModifier(int score)
    {
        //To determine an ability modifier without consulting the table, 
        //subtract 10 from the ability score and then divide the total by 2 (round down).

        return (score - 10) / 2;
    }

    public void freezePlayer()
    {
        playerFrozen = true;
    }

    public void unfreezePlayer()
    {
        playerFrozen = false;
    }

    public void keyContinue()
    {
        unfreezePlayer();
        uiManager.deactivateBgImage();
        uiManager.deactivateText();
        uiManager.deactivateKeyContinueButton();
    }
}
