using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IUnit))]
public class EndGameOnUnitDeath : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        GetComponent<IUnit>().installDeathListener(delegate { EventManager.TriggerEvent(GameEvents.GameOver); });	
	}
}
