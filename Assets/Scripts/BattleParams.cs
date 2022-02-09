using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BattleParams
{
    [SerializeField]
    private static StatSheet[] characters;

    [SerializeField]
    private static string test = "ur mom lol";

    public static StatSheet[] Characters
    {
        get
        {
            return characters;
        }
        set
        {
            characters = value;
        }
    }

    public static string Test
    {
        get
        {
            return test;
        }
        set
        {
            test = value;
        }
    }


}
