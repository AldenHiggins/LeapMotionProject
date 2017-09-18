using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : Behavior
{
    private float targetTime;

    private float currentTime;

    private Behavior behavior;

    public Timer(float targetTimeInput, Behavior behaviorInput)
    {
        targetTime = targetTimeInput;
        behavior = behaviorInput;
    }

    public BehaviorReturnCode run()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= targetTime)
        {
            currentTime = 0.0f;
            return behavior.run();
        }

        return BehaviorReturnCode.Running;
    }
}
