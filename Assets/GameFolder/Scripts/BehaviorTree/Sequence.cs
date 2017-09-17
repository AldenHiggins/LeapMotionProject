using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Behavior
{
    private Behavior[] behaviors;

    public Sequence(params Behavior[] behaviorsInput)
    {
        behaviors = behaviorsInput;
    }

    public BehaviorReturnCode run()
    {
        // Try to run all of the behaviors
        for (int behaviorIndex = 0; behaviorIndex < behaviors.Length; behaviorIndex++)
        {
            switch (behaviors[behaviorIndex].run())
            {
                case BehaviorReturnCode.Failure:
                    return BehaviorReturnCode.Failure;
                case BehaviorReturnCode.Success:
                    continue;
                case BehaviorReturnCode.Running:
                    return BehaviorReturnCode.Running;
            }
        }

        // If we have successfully run through all of them return success!
        return BehaviorReturnCode.Success;
    }
}
