using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("Gameplay")]
    public int value;
    public GameObject displayObject;
    public Renderer colorRenderer;

    // Start is called before the first frame update
    void Start()
    {
        float displaySize = (float) value / 100;
        displayObject.transform.localScale = new Vector3(displaySize, displaySize, displaySize);

        colorRenderer.material.SetFloat("_Offset", Random.Range(1f, 100f));
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ScaleToTarget(float elapsedTime, float totalTime)
    {
        // The remaining time might be 0 (especially if the animation time by turn is set to 0)
        if (elapsedTime >= totalTime)
            return;

        float elapsedPercentage = elapsedTime / totalTime;


        float displaySize = (float) value / 100;
        Vector3 targetScale = new Vector3(displaySize, displaySize, displaySize);

        displayObject.transform.localScale = Vector3.Slerp(displayObject.transform.localScale, targetScale, elapsedPercentage);
    }

    public void FixAnimation()
    {
        if (value <= 0)
            Die();
        else
        {
            float displaySize = (float) Mathf.Max(value, 0) / 100;
            displayObject.transform.localScale = new Vector3(displaySize, displaySize, displaySize);
        }
    }

    public int GetFood(int quantity)
    {
        if (quantity < 0)
            quantity = 0;
        if (quantity > 100)
            quantity = 100;

        value -= quantity;
        
        if (value <= 0)
        {
            int ret = quantity - value;
            value = 0;
            
            return ret;
        }

        return quantity;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
