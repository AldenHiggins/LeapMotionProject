using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowUnit : MonoBehaviour
{
    // By default we move the moving objects container to move the "camera"
    private Transform movingObjectsContainer;
    
    // Hardcoded size of the table/culling box
    private Vector3 tableSize = new Vector3(60.22f, 0.0f, 43.0f);

    // Get access to the bounds of the level to prevent the camera from moving outside of it
    private LevelBounds bounds;

    // If the player goes farther than this radius from the center of the table the camera movement will start
    public float centerMovementRadius;

    // How fast the camera moves
    public float cameraMoveSpeed;

	void Start ()
    {
        movingObjectsContainer = GetObjects.instance.getMovingObjectsContainer();
        bounds = GetObjects.instance.GetLevelBounds();
    }
	
	void Update ()
    {
        // Determine if we are far away enough from the center of the table to move the camera
        Vector3 vectorFromTableCenter = movingObjectsContainer.InverseTransformPoint(transform.position);
        vectorFromTableCenter.y = 0.0f;
        if (vectorFromTableCenter.magnitude < centerMovementRadius) return;

        // Calculate the new position of the camera to bring the controllable unit closer to the center
        Vector3 cameraMovementVector = vectorFromTableCenter.normalized * Time.deltaTime * cameraMoveSpeed;
        Vector3 newCameraPosition = movingObjectsContainer.position + cameraMovementVector;

        // Clamp the camera so it doesn't exceed the bounds of the level
        newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, bounds.minBounds.x + tableSize.x, bounds.maxBounds.x - tableSize.x);
        newCameraPosition.z = Mathf.Clamp(newCameraPosition.z, bounds.minBounds.z + tableSize.z, bounds.maxBounds.z - tableSize.z);

        // Move the camera
        movingObjectsContainer.position = newCameraPosition;
    }
}
