using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensiveGrid : MonoBehaviour
{
    [SerializeField]
    private int numColumns = 10;

    [SerializeField]
    private int numRows = 10;

    [SerializeField]
    private GameObject gridLine;

	// Use this for initialization
	void Start ()
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
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
