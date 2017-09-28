using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChangeAttack : AAttack
{
    private PlayerLogic player;
    private ControllableUnit playerUnit;

    private Vector3 playerInitialPosition;

    private float playerLargeScale = 40.0f;
    private float playerSmallScale = 4.0f;
    private Vector3 smallScaleOffset = new Vector3(0.0f, 2.0f, -20.0f);

    private bool playerIsSmall = false;

	// Use this for initialization
	void Start ()
    {
        player = GetObjects.instance.getPlayer();
        playerUnit = GetObjects.instance.getControllableUnit();
        playerInitialPosition = player.transform.parent.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void inactiveFunction() { }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        if (playerIsSmall)
        {
            player.transform.parent.localScale = new Vector3(playerLargeScale, playerLargeScale, playerLargeScale);
            player.transform.parent.localPosition = playerInitialPosition;
        }
        else
        {
            player.transform.parent.localScale = new Vector3(playerSmallScale, playerSmallScale, playerSmallScale);
            player.transform.parent.position = playerUnit.gameObject.transform.position + smallScaleOffset;
        }

        playerIsSmall = !playerIsSmall;
        player.isSmall = playerIsSmall;
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot) { }
}
