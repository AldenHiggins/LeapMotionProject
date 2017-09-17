using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : Behavior
{
    private BehaviorReturn action;

    public Leaf(BehaviorReturn actionInput)
    {
        action = actionInput;
    }

    public BehaviorReturnCode run()
    {
        return action();
    }
}
