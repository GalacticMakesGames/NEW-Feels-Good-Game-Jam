using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateAttackManager : MonoBehaviour
{
    public int maxUltimateBarFill = 20;
    public int currentUltimateBarFill = 0;

    public PlayerDashAttack playerDashAttack;

    [SerializeField] private Animator playerAnimator;

    AudioSource audioSource;
    [SerializeField] private AudioClip ultimateAttackSFX;

    [SerializeField] private ParticleSystem ultimateAttackParticleSystem;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (currentUltimateBarFill >= maxUltimateBarFill)
        {
            // play particle effect here to show that the meter is full
            UltimateAttackReady();

            if (Input.GetKeyDown(KeyCode.Space) && playerDashAttack.isDashing == false)
            {
                UseUltimateAttack();
            }
        }
    }

    public void UltimateBarFill(int fillAmount)
    {
        currentUltimateBarFill += fillAmount;
        currentUltimateBarFill = Mathf.Clamp(currentUltimateBarFill, 0, maxUltimateBarFill);
    }

    private void UseUltimateAttack()
    {
        Debug.Log("Ultimate attack activated!");

        currentUltimateBarFill = 0;
        if (ultimateAttackParticleSystem != null && ultimateAttackParticleSystem.isPlaying)
        {
            // StopEmitting allows existing particles to finish their lifetime naturally
            ultimateAttackParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }

        audioSource.PlayOneShot(ultimateAttackSFX);
        playerAnimator.SetTrigger("isUltAttacking");

        Enemy.TriggerUltimateAttackWipe();
    }

    public void UltimateAttackReady()
    {
        if (ultimateAttackParticleSystem != null && !ultimateAttackParticleSystem.isPlaying)
        {
            ultimateAttackParticleSystem.Play();
        }
    }
}
