using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SnowballLineController : MonoBehaviour
{
    //TODO: Move snowball settings from player Controller
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int numberOfPoints = 20;
    [SerializeField]
    private float timeStep = 0.10f;
    [SerializeField]
    private SnowballSettingsSO snowballSettings;

    private void OnEnable()
    {
        lineRenderer.positionCount = numberOfPoints;
    }

    private void OnDisable()
    {
        lineRenderer.positionCount = 0;
    }

    private void Update()
    {
        UpdateLineAnimation();
    }

    [ContextMenu("Check Line")]
    private void UpdateLineAnimation()
    {
        Vector3 forward = transform.worldToLocalMatrix.MultiplyVector(cameraTransform.forward);
        Vector3[] points = new Vector3[numberOfPoints];
        Vector3 throwForce = forward * snowballSettings.throwSpeed;
        Vector3 acceleration = ((Vector3.up * Physics.gravity.y) / snowballSettings.ballMass);
        for(int i = 0; i < numberOfPoints; i++)
        {
            float flyingTime = i * timeStep;
            //starting position + (starting speed * time) + (0.5f * acceleration * time^2)
            //starting position is always vector zero.
            Vector3 point = (throwForce * flyingTime) + ( 0.5f * acceleration * flyingTime * flyingTime);
            points[i] = point;
        }
        lineRenderer.SetPositions(points);
    }
}
