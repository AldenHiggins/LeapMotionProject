using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Behavior
{
    private Behavior[] behaviors;

    public Selector(params Behavior[] behaviorsInput)
    {
        behaviors = behaviorsInput;
    }

    public BehaviorReturnCode run()
    {
        // Run through all of the behaviors and look for one that succeeds then return
        for (int behaviorIndex = 0; behaviorIndex < behaviors.Length; behaviorIndex++)
        {
            switch (behaviors[behaviorIndex].run())
            {
                case BehaviorReturnCode.Running:
                    return BehaviorReturnCode.Running;
                case BehaviorReturnCode.Failure:
                    continue;
                case BehaviorReturnCode.Success:
                    return BehaviorReturnCode.Success;
            }
        }

        // If none of the sub behaviors succeed return failure
        return BehaviorReturnCode.Failure;
    }
}
