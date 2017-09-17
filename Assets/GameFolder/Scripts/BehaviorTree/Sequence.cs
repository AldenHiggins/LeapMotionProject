using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Behavior
{
    public BehaviorReturnCode run()
    {
        Debug.Log("Running sequence");
        return BehaviorReturnCode.Success;
    }
}
