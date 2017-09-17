using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Behavior
{
    public BehaviorReturnCode run()
    {
        Debug.Log("Running Selector");
        return BehaviorReturnCode.Success;
    }
}
