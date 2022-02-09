using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public StatSheet character;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float newLen = ((float)character.currentHp / (float)character.maxHp) * 10;
        this.transform.localScale = new Vector3(newLen, transform.localScale.y, transform.localScale.z);
    }
}
