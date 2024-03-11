using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLegController : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject feetPoint;               // Feet actual location
    [SerializeField] private GameObject feetTarget;

    [Header("Other Info")]
    [SerializeField] private SpiderController spiderController;
    [SerializeField] private LayerMask layerMask;                   // What is raycastable

    [Header("Stats")]
    [SerializeField] private float distanceBeforeReset = 3;

    // Private variables
    private Vector3 pastPosition;                                 // Previous position
    private Vector3 newPosition;                                  // Target position
    private float lerp = -1;                                         // ?????????????????????????????????????????????????????????
    private Vector3 localOriginalFeetPosition;                    // The local position transform for the feet 

    private void Awake()
    {
        pastPosition = feetPoint.transform.position;
        newPosition = feetPoint.transform.position;
        localOriginalFeetPosition = transform.InverseTransformPoint(feetPoint.transform.position); // Spider root object
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLegPositions();
    }

    private void UpdateLegPositions()
    {
        Vector3 movePos = pastPosition;

        // Check if should move leg
        if (Vector3.Distance(localOriginalFeetPosition, transform.InverseTransformPoint(feetPoint.transform.position)) > distanceBeforeReset)
        {
            newPosition = transform.position + localOriginalFeetPosition;

            // If should move leg, find new position
            if (Physics.Raycast(feetPoint.transform.parent.position, (feetPoint.transform.position - feetPoint.transform.parent.position), out RaycastHit raycastHit, Mathf.Infinity, layerMask))
            {
                newPosition = new Vector3(transform.position.x + localOriginalFeetPosition.x + spiderController.CurrentVelocity.x, raycastHit.point.y, transform.position.z + localOriginalFeetPosition.z + spiderController.CurrentVelocity.z);
                Debug.Log("Name: " + feetPoint.name + " " + newPosition);
            }
            lerp = -1;
        }
        else
        {
            pastPosition = newPosition;
            lerp += Time.deltaTime;
        } // Working

        if (lerp < 0)
        {
            lerp += Time.deltaTime;

            movePos = Vector3.Lerp(pastPosition, newPosition, lerp / Vector3.Distance(localOriginalFeetPosition, transform.InverseTransformPoint(feetPoint.transform.position)));
            movePos.y += Mathf.Sin(- lerp * Mathf.PI / 2);
        }

        feetTarget.transform.position = movePos;
    }
}
