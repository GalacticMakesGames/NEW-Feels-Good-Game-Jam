using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Image fillImage;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;
    }

    void Update()
    {
        float fillValue = (float)playerHealth.currentHealth / (float)playerHealth.maxHealth;
        slider.value = fillValue;

        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        if (slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }

        if (fillValue <= slider.maxValue / 3f)
        {
            fillImage.color = Color.red; // critical condition
        }
        else if (fillValue > slider.maxValue /3f)
        {
            fillImage.color = Color.green;
        }

            slider.value = fillValue;
    }
}
