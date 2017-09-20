using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadLeader : MonoBehaviour
{
    public List<GameObject> troops;

    private Unit unit;

    void Start()
    {
        unit = GetComponent<Unit>();
    }

    private void checkForDeadSquadMembers()
    {
        List<GameObject> livingTroops = new List<GameObject>();

        for (int troopIndex = 0; troopIndex < troops.Count; troopIndex++)
        {
            GameObject trooper = troops[troopIndex];
            if (trooper == null) continue;
            Unit trooperUnit = trooper.GetComponent<Unit>();
            if (trooperUnit == null) continue;
            if (trooperUnit.isDying) continue;
            ManualPatrol trooperPatrol = trooper.GetComponent<ManualPatrol>();
            if (trooperPatrol == null) continue;
            livingTroops.Add(trooper);
        }

        troops = livingTroops;
    }

    public void issueNewTroopMovement()
    {
        // First clear out any dead/invalid squad members
        checkForDeadSquadMembers();

        // If the unit is part of a squad generate appropriate follow positions for the squad members
        Vector3 patrolVector = (unit.transform.position - unit.targetPosition).normalized;
        patrolVector *= 5.0f;

        // Set the unit's squad to follow it
        for (int squadIndex = 0; squadIndex < troops.Count; squadIndex++)
        {
            ManualPatrol troopPatrol = troops[squadIndex].GetComponent<ManualPatrol>();

            Vector3 patrolPosition = unit.targetPosition + (Quaternion.Euler(0.0f, -45.0f + (45.0f * squadIndex), 0.0f) * patrolVector);
            troopPatrol.setPatrolPosition(patrolPosition);
        }
    }
}
