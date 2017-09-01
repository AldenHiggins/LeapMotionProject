using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveGrid : MonoBehaviour
{
    [SerializeField]
    private int numColumns = 10;

    [SerializeField]
    private int numRows = 10;

    private float columnStep;
    private float rowStep;

    [SerializeField]
    private GameObject gridLine;

    private bool[,] placedDefenses;

    // Use this for initialization
    void Start()
    {
        // Generate all of the columns
        for (int colIndex = 0; colIndex < numColumns; colIndex++)
        {
            float zPosition = -.5f + ((1.0f / (numColumns - 1)) * colIndex);
            GameObject newGridLine = Instantiate(gridLine, transform);
            newGridLine.transform.localPosition = new Vector3(0.0f, 1.0f, zPosition);
        }

        // Generate all of the rows
        for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
        {
            float xPosition = -.5f + ((1.0f / (numColumns - 1)) * rowIndex);
            GameObject newGridLine = Instantiate(gridLine, transform);
            newGridLine.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            newGridLine.transform.localPosition = new Vector3(xPosition, 1.0f, 0.0f);
        }

        // Generate the column and row steps
        columnStep = 1.0f / (numColumns - 1);
        rowStep = 1.0f / (numRows - 1);

        // Generate our placedDefenses array
        placedDefenses = new bool[numColumns, numRows];
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Get a new grid position based on the inputted world space position
    public Vector3 getClosestGridPoint(Vector3 referencePoint)
    {
        // X and Z are what we're looking for
        Vector3 gridSpacePoint = transform.InverseTransformPoint(referencePoint);
        Vector3 newPoint = gridSpacePoint;

        Debug.Log("Grid point: " + gridSpacePoint);

        // Find out the x and z positions
        // X is rows, z is columns
        // Find out the z column position
        float newZ = (gridSpacePoint.z + .5f) / columnStep;
        newZ = (((int)newZ) * columnStep) - .5f + (columnStep / 2.0f);

        // Find out the x row position
        float newX = (gridSpacePoint.x + .5f) / rowStep;
        newX = (((int)newX) * rowStep) - .5f + (rowStep / 2.0f);

        // Set up the new point
        newPoint.x = newX;
        newPoint.z = newZ;

        return transform.TransformPoint(newPoint);
    }

    // Input a new defense at the given spot.  Return true if the spot is empty and fill the spot.  Return false if the spot is taken.
    public bool placeNewDefense(Vector3 placementPoint)
    {
        int columnIndex = 0;
        int rowIndex = 0;

        if (!spotTakenHelper(placementPoint, ref columnIndex, ref rowIndex))
        {
            placedDefenses[columnIndex, rowIndex] = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    // Check if the inputted position is taken on the grid
    public bool isSpotTaken(Vector3 referencePoint)
    {
        // Fill up some dummy ints
        int columnIndex = 0;
        int rowIndex = 0;

        return spotTakenHelper(referencePoint, ref columnIndex, ref rowIndex);
    }

    // Generate a column and row for the inputted world space point
    private bool spotTakenHelper(Vector3 referencePoint, ref int columnIndex, ref int rowIndex)
    {
        // Convert our reference point into the defensive grid's space
        Vector3 gridSpacePoint = transform.InverseTransformPoint(referencePoint);
        // Find out that point's column and row
        columnIndex = (int)((gridSpacePoint.z + .5f) / columnStep);
        rowIndex = (int)((gridSpacePoint.x + .5f) / rowStep);

        return placedDefenses[columnIndex, rowIndex];
    }
}
