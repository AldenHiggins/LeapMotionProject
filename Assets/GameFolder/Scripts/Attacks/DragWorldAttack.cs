using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWorldAttack : AAttack
{
    private GameObject objectToMove;
    private Vector3 objectStartPosition;

    private Vector3 controllerStartPosition;

    [SerializeField]
    private float moveSpeed = 10.0f;

    void Start()
    {
        controllerStartPosition = Vector3.zero;
        objectToMove = GetObjects.getMovingObjectsContainer();
    }

    public override void inactiveFunction() { }

    public override void releaseFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        controllerStartPosition = Vector3.zero;
    }

    public override void holdFunctionConcrete(Vector3 localPos, Vector3 worldPos, Quaternion localRot, Quaternion worldRot)
    {
        if (controllerStartPosition == Vector3.zero)
        {
            controllerStartPosition = localPos;
            objectStartPosition = objectToMove.transform.position;
        }

        // Move the object based on the distance the controller has moved while holding the trigger now
        Vector3 deltaControllerMovement = localPos - controllerStartPosition;
        Vector3 moveVector = objectStartPosition + (moveSpeed * deltaControllerMovement);
        moveVector.y = 0.0f;
        objectToMove.transform.position = moveVector;
    }
}
