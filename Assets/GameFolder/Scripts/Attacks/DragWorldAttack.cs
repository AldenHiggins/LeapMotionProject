using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWorldAttack : AAttack
{
    private Vector3 previousControllerPosition;

    [SerializeField]
    private float moveSpeed = 10.0f;

    void Start()
    {
        previousControllerPosition = Vector3.zero;
    }

    public override void inactiveFunction() { }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        previousControllerPosition = Vector3.zero;
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        if (previousControllerPosition == Vector3.zero)
        {
            previousControllerPosition = localPos;
            return;
        }

        // Move the object based on the distance the controller has moved while holding the trigger now
        Vector3 deltaControllerMovement = localPos - previousControllerPosition;
        // Zero out the y movement so we don't move the level up/down
        deltaControllerMovement.y = 0.0f;

        GetObjects.instance.getCameraFollowUnit().addCameraDelta(deltaControllerMovement * moveSpeed * Time.deltaTime);

        previousControllerPosition = localPos;
    }
}
