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

    private LevelBounds bounds;

    private ControllableUnit playerCharacter;

    [SerializeField]
    private float tableXDistance = 52.0f;

    [SerializeField]
    private float tableZDistance = 35.0f;

    void Start()
    {
        controllerStartPosition = Vector3.zero;
        objectToMove = GetObjects.getMovingObjectsContainer();
        bounds = GetObjects.GetLevelBounds();
        playerCharacter = GetObjects.getControllableUnit();
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
        // Zero out the y movement so we don't move the level up/down
        moveVector.y = 0.0f;
        // Clamp the x and z to prevent the user from going outside the level bounds
        moveVector.x = Mathf.Clamp(moveVector.x, bounds.minBounds.x, bounds.maxBounds.x);
        moveVector.z = Mathf.Clamp(moveVector.z, bounds.minBounds.z, bounds.maxBounds.z);

        // Clamp the x and z so we don't lose sight of the player's character
        Vector3 playerPos = playerCharacter.transform.position;
        moveVector.x = Mathf.Clamp(moveVector.x, playerPos.x - tableXDistance, playerPos.x + tableXDistance);
        moveVector.z = Mathf.Clamp(moveVector.z, playerPos.z - tableZDistance, playerPos.z + tableZDistance);

        objectToMove.transform.position = moveVector;
    }
}
