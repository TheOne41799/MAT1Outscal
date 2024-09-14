using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerShake : MonoBehaviour
{
    [SerializeField] private float shakeMagnitude = 0.3f;

    private Vector3 originalPosition;
    private float shakeTimeRemaining;


    private void Start()
    {
        originalPosition = transform.localPosition;
    }


    private void Update()
    {
        if (shakeTimeRemaining > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = originalPosition + shakeOffset;

            shakeTimeRemaining -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }


    public void TriggerShake(float duration)
    {
        shakeTimeRemaining = duration;
    }
}
