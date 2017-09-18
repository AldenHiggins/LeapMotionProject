using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSequence : Behavior
{
    private Behavior[] behaviors;

    private int lastIndex = 0;

    public StateSequence(params Behavior[] behaviorsInput)
    {
        behaviors = behaviorsInput;
    }

    public BehaviorReturnCode run()
    {
        for (; lastIndex < behaviors.Length; lastIndex++)
        {
            switch (behaviors[lastIndex].run())
            {
                case BehaviorReturnCode.Success:
                    continue;
                case BehaviorReturnCode.Running:
                    return BehaviorReturnCode.Running;
                case BehaviorReturnCode.Failure:
                    lastIndex = 0;
                    return BehaviorReturnCode.Failure;
            }
        }

        // If we've run through all of the behaviors reset the index and return success
        lastIndex = 0;
        return BehaviorReturnCode.Success;
    }
}
