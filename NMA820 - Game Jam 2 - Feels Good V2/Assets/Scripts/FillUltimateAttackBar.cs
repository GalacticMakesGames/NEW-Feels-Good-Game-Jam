using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillUltimateAttackBar : MonoBehaviour
{
    public UltimateAttackManager ultimateAttackManager;
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
        float fillValue = (float)ultimateAttackManager.currentUltimateBarFill / (float)ultimateAttackManager.maxUltimateBarFill;
        slider.value = fillValue;

        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }

        if (slider.value > slider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true;
        }

        if (fillValue == slider.maxValue)
        {
            fillImage.color = Color.red;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                fillImage.color = Color.white;
                fillImage.enabled = false;
            }
        }

        //slider.value = fillValue;
    }
}
