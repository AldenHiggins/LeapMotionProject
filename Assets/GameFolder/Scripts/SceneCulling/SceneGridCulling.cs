using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGridCulling : MonoBehaviour
{
    public GameObject sceneGridLine;
    public GameObject sceneGridObject;
    public int horizontalLines;
    public int verticalLines;

    private GameObject[,] sceneSections;
    private Transform movingObjects;
    float minX;
    float maxX;
    float minZ;
    float maxZ;
    float horizontalStep;
    float verticalStep;

    private void Start()
    {
        sceneSections = buildGrid();
        movingObjects = GetObjects.instance.getMovingObjectsContainer();

        // Disable the old scene geometry
        getSceneGeometry().gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector3 tableOrigin = movingObjects.transform.position;
        Debug.Log("TableOrigin: " + tableOrigin);
        Debug.Log("Numerator: " + (tableOrigin.x - minX));
        Debug.Log("Horizontal step: " + horizontalStep);
        int xIndex = (int)(Mathf.Floor((tableOrigin.x - minX) / horizontalStep));
        int minXIndex = xIndex - 2;
        int maxXIndex = xIndex + 2;
        int yIndex = (int)(Mathf.Floor((tableOrigin.z - minZ) / verticalStep));
        int minYIndex = yIndex - 2;
        int maxYIndex = yIndex + 2;

        Debug.Log("X index: " + xIndex);
        Debug.Log("Y Index: " + yIndex);

        // Get rid of all the x transforms greater than xIndex
        for (int xIndexToDisable = 0; xIndexToDisable < horizontalLines; xIndexToDisable++)
        {
            for (int yIndexToDisable = 0; yIndexToDisable < verticalLines; yIndexToDisable++)
            {
                bool activate = false;
                if (xIndexToDisable >= minXIndex && xIndexToDisable <= maxXIndex && yIndexToDisable >= minYIndex && yIndexToDisable <= maxYIndex)
                {
                    activate = true;
                }
                sceneSections[xIndexToDisable, yIndexToDisable].SetActive(activate);
            }
        }
    }

    public GameObject[,] buildGrid()
    {
        Debug.Log("Building Scene Grid");
        Transform sceneTransform = getSceneReferenceBox();

        // Build the parameters for the scene
        minX = sceneTransform.position.x - (sceneTransform.localScale.x / 2.0f);
        maxX = sceneTransform.position.x + (sceneTransform.localScale.x / 2.0f);
        minZ = sceneTransform.position.z - (sceneTransform.localScale.z / 2.0f);
        maxZ = sceneTransform.position.z + (sceneTransform.localScale.z / 2.0f);
        horizontalStep = sceneTransform.localScale.x / horizontalLines;
        verticalStep = sceneTransform.localScale.z / verticalLines;

        // Construct our reference grid
        constructReferenceGrid(minX, minZ, horizontalStep, verticalStep, sceneTransform);

        // Create the containers for the sections of the level
        GameObject[,] sceneSections = constructSceneSectionContainers(minX, minZ, horizontalStep, verticalStep, sceneTransform);

        // Iterate through the scene's game objects and place copies of them into a scene section
        copySceneObjectsIntoSceneSections(sceneSections, minX, minZ, horizontalStep, verticalStep);

        // Return the scene sections
        return sceneSections;
    }

    private void copySceneObjectsIntoSceneSections(GameObject[,] sceneSections, float minX, float minZ, float horizontalStep, float verticalStep)
    {
        // Get access to the scene's geometry parent
        Transform sceneGeometry = getSceneGeometry();

        // Iterate through all of the geometry in the scene
        Transform[] sceneGeoTransforms = sceneGeometry.GetComponentsInChildren<Transform>();
        for (int geometryIndex = 0; geometryIndex < sceneGeoTransforms.Length; geometryIndex++)
        {
            GameObject sceneObject = sceneGeoTransforms[geometryIndex].gameObject;
            // Only partition meshes
            MeshRenderer objectMesh = sceneObject.GetComponent<MeshRenderer>();
            if (objectMesh == null) continue;
            // Now place the object into the grid
            placeObjectIntoGrid(sceneSections, sceneObject, minX, minZ, horizontalStep, verticalStep);
        }
    }

    private void placeObjectIntoGrid(GameObject[,] sceneSections, GameObject objectToPlace, float minX, float minZ, float horizontalStep, float verticalStep)
    {
        Vector3 objectPosition = objectToPlace.transform.position;
        int xIndex = (int)Mathf.Floor((objectPosition.x - minX) / horizontalStep);
        int yIndex = (int)Mathf.Floor((objectPosition.z - minZ) / verticalStep);
        // Create a copy of the object in the correct scene section
        GameObject objectCopy = Instantiate(objectToPlace, objectToPlace.transform.position, objectToPlace.transform.rotation, objectToPlace.transform.parent);
        objectCopy.transform.parent = sceneSections[xIndex, yIndex].transform;
    }

    private GameObject[,] constructSceneSectionContainers(float minX, float minZ, float horizontalStep, float verticalStep, Transform sceneTransform)
    {
        // Get our scene section reference container
        Transform sceneSectionParent = getSceneSections();
        // Delete all of our scene sections
        for (int childIndex = (sceneSectionParent.childCount - 1); childIndex > -1; childIndex--)
        {
            DestroyImmediate(sceneSectionParent.transform.GetChild(childIndex).gameObject);
        }
        // Generate all of the scene sections
        GameObject[,] sceneSections = new GameObject[horizontalLines, verticalLines];
        for (int horizontalIndex = 0; horizontalIndex < horizontalLines; horizontalIndex++)
        {
            float xPosition = (minX + (horizontalStep / 2.0f)) + (horizontalStep * horizontalIndex);
            for (int verticalIndex = 0; verticalIndex < verticalLines; verticalIndex++)
            {
                float zPosition = (minZ + (verticalStep / 2.0f)) + (verticalStep * verticalIndex);
                GameObject newSceneSection = Instantiate(sceneGridObject, new Vector3(xPosition, 10.0f, zPosition), Quaternion.identity, sceneSectionParent);
                newSceneSection.transform.localScale = new Vector3(horizontalStep * .9f, 10.0f, verticalStep * .9f);
                sceneSections[horizontalIndex, verticalIndex] = newSceneSection;
            }
        }
        return sceneSections;
    }

    private void constructReferenceGrid(float minX, float minZ, float horizontalStep, float verticalStep, Transform sceneTransform)
    {
        // Get our grid reference container
        Transform gridReferenceParent = getGridReference();
        // Delete all of our old grid lines
        for (int childIndex = (gridReferenceParent.childCount - 1); childIndex > -1; childIndex--)
        {
            DestroyImmediate(gridReferenceParent.transform.GetChild(childIndex).gameObject);
        }
        // Create a set of horizontal lines
        for (int horizontalIndex = 0; horizontalIndex < horizontalLines + 1; horizontalIndex++)
        {
            GameObject newLine = Instantiate(sceneGridLine, new Vector3(minX + (horizontalStep * horizontalIndex),
                3.0f, sceneTransform.position.z), Quaternion.identity, gridReferenceParent);
            newLine.transform.localScale = new Vector3(1.0f, 1.0f, sceneTransform.localScale.z);
        }
        // Create a set of vertical lines
        for (int verticalIndex = 0; verticalIndex < verticalLines + 1; verticalIndex++)
        {
            GameObject newLine = Instantiate(sceneGridLine, new Vector3(sceneTransform.position.x,
                3.0f, minZ + (verticalStep * verticalIndex)), Quaternion.identity, gridReferenceParent);
            newLine.transform.localScale = new Vector3(sceneTransform.localScale.x, 1.0f, 1.0f);
        }
    }

    private Transform getSceneGeometry()
    {
        return transform.Find("Geometry");
    }

    private Transform getSceneSections()
    {
        Transform sceneCulling = transform.Find("SceneCulling");
        return sceneCulling.Find("SceneSections");
    }

    private Transform getSceneReferenceBox()
    {
        Transform sceneCulling = transform.Find("SceneCulling");
        return sceneCulling.Find("ReferenceBox");
    }

    private Transform getGridReference()
    {
        Transform sceneCulling = transform.Find("SceneCulling");
        return sceneCulling.Find("GridReference");
    }
}
