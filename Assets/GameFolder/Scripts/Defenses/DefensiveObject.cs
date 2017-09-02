using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveObject : MonoBehaviour
{
    [SerializeField]
    private int defenseCost = 1;

    [SerializeField]
    private GameObject defensiveObject;

    [SerializeField]
    private GameObject defensiveObjectInvalid;

    [SerializeField]
    private GameObject defensiveObjectPending;

    public GameObject getDefensiveObject()
    {
        return defensiveObject;
    }

    public GameObject getPending()
    {
        return defensiveObjectPending;
    }

    public GameObject getInvalid()
    {
        return defensiveObjectInvalid;
    }

    public int getCost()
    {
        return defenseCost;
    }
}
