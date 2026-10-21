using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillUltimateAttackBar : MonoBehaviour
{
    public UltimateAttackManager ultimateAttackManager;

    public Image fillImage;
    private Slider slider;

    public GameObject readyPromptImage;

    private Color fillOriginalColor;

    void Awake()
    {
        slider = GetComponent<Slider>();
        slider.minValue = 0f;
        slider.maxValue = 1f;

        fillOriginalColor = fillImage.color;

        readyPromptImage.SetActive(false);
    }

    void Update()
    {
        // calculate and update slider value
        float fillValue = (float)ultimateAttackManager.currentUltimateBarFill / (float)ultimateAttackManager.maxUltimateBarFill;
        slider.value = fillValue;

        // handle spacebar activation when fully charged
        if (fillValue >= slider.maxValue)
        {
            fillImage.color = Color.yellow;
            readyPromptImage.SetActive(true);
        }
        else
        {
            fillImage.color = fillOriginalColor; // reset colour back to normal if the meter isn't fully charged
            readyPromptImage.SetActive(false);
        }

        // handles visibility of fill based on whether there is progress
        if (slider.value <= slider.minValue)
        {
            fillImage.enabled = false;
        }
        else
        {
            fillImage.enabled = true;
        }
    }
}
