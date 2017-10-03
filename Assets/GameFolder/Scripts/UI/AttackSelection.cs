using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSelection : MonoBehaviour
{
    private VRControls controls;
    private InputTypes currentInput;

    private void Start()
    {
        currentInput = InputTypes.RightTrigger;
        controls = GetObjects.instance.getVRControls();
    }

    public void changeCurrentInput(InputTypes newInput)
    {
        currentInput = newInput;
    }

    public void changeAttackType(AttackTypes newAttack)
    {
        controls.changeControl(currentInput, newAttack);
    }
}
