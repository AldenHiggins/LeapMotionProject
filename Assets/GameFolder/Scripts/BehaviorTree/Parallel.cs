using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Behavior
{
    public BehaviorReturnCode run()
    {
        Debug.Log("Running Parallel");
        return BehaviorReturnCode.Success;
    }
}
