using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class InputChoice : MonoBehaviour
{
    public InputTypes inputType;

    private AttackSelection attackSelectionUI;

    private void Start()
    {
        attackSelectionUI = GetObjects.instance.getAttackSelection();
        Toggle toggleComponent = GetComponent<Toggle>();
        toggleComponent.onValueChanged.AddListener(inputToggled);
    }

    private void inputToggled(bool toggledOn)
    {
        if (toggledOn)
        {
            attackSelectionUI.changeCurrentInput(inputType);
        }
    }
}
