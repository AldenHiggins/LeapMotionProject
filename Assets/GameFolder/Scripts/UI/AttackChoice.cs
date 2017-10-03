using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class AttackChoice : MonoBehaviour
{
    public AttackTypes attackType;

    private AttackSelection attackSelectionUI;

    private void Start()
    {
        attackSelectionUI = GetObjects.instance.getAttackSelection();
        Toggle toggleComponent = GetComponent<Toggle>();
        toggleComponent.onValueChanged.AddListener(attackTypeToggled);
    }

    private void attackTypeToggled(bool toggledOn)
    {
        if (toggledOn)
        {
            attackSelectionUI.changeAttackType(attackType);
        }
    }
}
