using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBehavior : Behavior
{
    private Behavior behavior;
    private float probability;

    public RandomBehavior(float probabilityInput, Behavior behaviorInput)
    {
        probability = Mathf.Clamp(probabilityInput, 0.0f, 1.0f);
        behavior = behaviorInput;
    }

    public BehaviorReturnCode run()
    {
        if (probability >= Random.value)
        {
            return behavior.run();
        }
        else
        {
            return BehaviorReturnCode.Success;
        }
    }
}
