using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : AUnit
{
    override protected void initializeUnit()
    {
        enemySearchLayer = 1 << 8;
        isAlly = true;
    }
}
