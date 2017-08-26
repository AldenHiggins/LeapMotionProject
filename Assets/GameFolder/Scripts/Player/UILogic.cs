using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic : MonoBehaviour
{
    private GameLogic game;
    private PlayerLogic player;

    [SerializeField]
    private GameObject introUI;

    [SerializeField]
    private GameObject defenseUI;

    [SerializeField]
    private GameObject endGameUI;

    // Use this for initialization
    void Start ()
    {
        game = GetObjects.getGame();
        player = GetObjects.getPlayer();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // If the start screen is active keep the start screen going
        if (game.getStartScreenActive())
        {
            return;
        }
        else
        {
            introUI.SetActive(false);
        }

        // If the player is dead show the end game UI
        if (!player.getIsAlive())
        {
            defenseUI.SetActive(false);
            endGameUI.SetActive(true);
            return;
        }

        // Else show the user their defensive options
        defenseUI.SetActive(true);
    }
}
