using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic : MonoBehaviour
{
    [SerializeField]
    private GameObject introUI;

    [SerializeField]
    private GameObject defenseUI;

    [SerializeField]
    private GameObject endGameUI;

    // Use this for initialization
    void Start ()
    {
        // For now the UI just changes based on the game state
        EventManager.StartListening(GameEvents.GameStart, delegate () { introUI.SetActive(false); });
        EventManager.StartListening(GameEvents.DefensivePhaseStart, delegate () { defenseUI.SetActive(true); });
        EventManager.StartListening(GameEvents.OffensivePhaseStart, delegate () { defenseUI.SetActive(false); });
        EventManager.StartListening(GameEvents.GameOver, delegate ()
        {
            defenseUI.SetActive(false);
            endGameUI.SetActive(true);
        });
    }
}
