using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviorReturnCode
{
    Failure,
    Success,
    Running
}

public delegate BehaviorReturnCode BehaviorReturn();
