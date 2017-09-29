using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionPointer : MonoBehaviour
{
    private int raycastLayers = 1 << 10 | 1 << 5;
    private bool pointerEnabled = false;
    private float inactiveScale = 3.0f;

    public Color highlightColor;
    public Color inactiveColor;

    private void Start()
    {
        EventManager.StartListening(GameEvents.GamePause, enablePointer);
        EventManager.StartListening(GameEvents.GameResume, disablePointer);
    }

    // Raycast to determine how the selection pointer looks
    void Update ()
    {
        // Only perform the raycast if the pointer is enabled
        if (!pointerEnabled) return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 300.0f, raycastLayers))
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, hit.distance / 2.0f);
            transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial.color = highlightColor;
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, inactiveScale);
            transform.GetChild(0).gameObject.GetComponent<Renderer>().sharedMaterial.color = inactiveColor;
        }
    }

    public void enablePointer()
    {
        pointerEnabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void disablePointer()
    {
        pointerEnabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
