using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private Transform mainBody;                    // Pelvis

    [Header("Other Info")]
    [SerializeField] private LayerMask layerMask;                   // What is raycastable

    [Header("Stats")]
    [SerializeField] private float bodyHoverHeight;                 // How high off the floor to hover

    // Private variables
    private Vector3 previousLocation;                               // Past location - For Velocity calculations
    private Vector3 currentVelocity;                                  // Calculated velocity of body

    // Getters & Setters
    public Vector3 CurrentVelocity { get { return currentVelocity; } }

    private void Update()
    {
        UpdateMainBodyPosition();
        GetVelocity();
    }

    private void UpdateMainBodyPosition()
    {
        RaycastHit raycastHit;
        // If floor is found
        if (Physics.Raycast(mainBody.position + new Vector3(0, 10, 0), Vector3.down, out raycastHit, Mathf.Infinity, layerMask))
            mainBody.position = new Vector3(mainBody.position.x, raycastHit.point.y + bodyHoverHeight, mainBody.position.z);
    }
    
    private void GetVelocity()
    {
        Vector3 temp = new Vector3 (previousLocation.x - transform.position.x, previousLocation.y - transform.position.y, previousLocation.z - transform.position.z);
        temp.Normalize();
        temp /= Time.deltaTime; // This is the average velocity now

        currentVelocity = temp;

        previousLocation = mainBody.transform.position;
    }
}
