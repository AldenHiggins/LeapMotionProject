using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DamageText : MonoBehaviour
{
    private Text text;
    private float currentTextTime = 0.0f;
    private float textLifeTime = 2.0f;
    private bool isTextActive = false;

	// Use this for initialization
	void Start ()
    {
        text = GetComponent<Text>();
        text.enabled = false;
    }

    private void Update()
    {
        // Make sure the text is active
        if (!isTextActive) return;

        // Increment our text lifetime, if we are over that lifetime deactivate the text
        currentTextTime += Time.deltaTime;
        if (currentTextTime > textLifeTime)
        {
            isTextActive = false;
            text.enabled = false;
        }

        // Animate the text over its lifetime
        text.rectTransform.localPosition = new Vector3(0.0f, Mathf.Lerp(0.0f, 10.0f, currentTextTime / textLifeTime), 0.0f);
        Color newColor = text.color;
        newColor.a = Mathf.Lerp(1.0f, 0.0f, currentTextTime / textLifeTime);
        text.color = newColor;
    }

    public void activateText(int damageAmount)
    {
        // Reset the position of the text
        text.rectTransform.localPosition = Vector3.zero;
        isTextActive = true;
        text.enabled = true;
        currentTextTime = 0.0f;
        text.text = damageAmount.ToString();
    }
}
