using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CureLightBeamEffect : MonoBehaviour
{
    public float lifetime = 1.0f; // How long the beam stays
    public float fadeSpeed = 2.0f;
    [SerializeField] private Light2D light2D;

    void Start()
    {
        light2D = GetComponent<Light2D>();

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (light2D != null)
        {
            light2D.intensity = Mathf.MoveTowards(light2D.intensity, 0f, fadeSpeed * Time.deltaTime);
        }
    }
}
