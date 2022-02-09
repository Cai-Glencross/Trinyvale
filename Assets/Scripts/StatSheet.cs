using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Stat Sheet", fileName = "New Stat Sheet")]
public class StatSheet : ScriptableObject
{
    public GameObject gameObject;
    public float startHeight;
    public float scale;

    public string characterName;
    public string characterRace;
    public string characterClass;
    public Sprite sprite;
    public bool isEnemy;
    public bool isDead;

    public int maxHp;
    public int currentHp;

    public int maxSpellSlots;
    public int currentSpellSlots;

    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int charisma;

    public bool isInspired;
    public bool isMarked;
    public bool isHexed;
    public bool isMocked;

    public string attackType;
    public string castingType;
    public int armorClass;
    public int proficiency;
    public Action[] actions;

    [System.Serializable]
    public class Action
    {
        public int Die;
        public bool isBonus;
        public bool targetEnemy;
        public string Name;
        public string SpellSave;
        public int SpellSlots;

        public Action(int die, bool bonus, bool enemy, string name, string spellSave, int spellSlots)
        {
            Name = name;
            Die = die;
            isBonus = bonus;
            targetEnemy = enemy;
            SpellSave = spellSave;
            SpellSlots = spellSlots;

        }
    }
}
