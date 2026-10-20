using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateAttackManager : MonoBehaviour
{
    public int maxUltimateBarFill = 20;
    public int currentUltimateBarFill = 0;

    public void UltimateBarFill(int fillAmount)
    {
        currentUltimateBarFill += fillAmount;

        if (currentUltimateBarFill == 20)
        {
            // play particle effect here to show that the meter is full
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(UltimateAttackActive());
                currentUltimateBarFill = 0;
            }
            
            Debug.Log("Ultimate attack activated!");
        }
    }

    private IEnumerator UltimateAttackActive()
    {
        yield return new WaitForSeconds(1f);
    }
}
