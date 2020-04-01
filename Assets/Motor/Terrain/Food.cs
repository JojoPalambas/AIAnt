using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("Gameplay")]
    public int value;
    public GameObject displayObject;

    // Start is called before the first frame update
    void Start()
    {
        float displaySize = (float) value / 100;
        displayObject.transform.localScale = new Vector3(displaySize, displaySize, displaySize);
    }

    // Update is called once per frame
    void Update()
    {
        float displaySize = (float) value / 100;
        displayObject.transform.localScale = new Vector3(displaySize, displaySize, displaySize);
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

            Die();
            return ret;
        }

        return quantity;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
