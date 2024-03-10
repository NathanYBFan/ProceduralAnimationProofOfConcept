using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SpiderController : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private Transform mainBody;                    // Pelvis
    [SerializeField] private GameObject[] legPoints;                // Feet target points
    [SerializeField] private GameObject[] feetPoints;               // Feet actual location

    [Header("Other Info")]
    [SerializeField] private LayerMask layerMask;                   // What is raycastable

    [Header("Stats")]
    [SerializeField] private float bodyHoverHeight;                 // How high off the floor to hover
    [SerializeField] private float stepHeight = 1;                  // How high each step should be
    [SerializeField] private float moveSpeed = 1;                   // Move speed from a to b
    [SerializeField] private float footLocationUpdateTimer = 1;     // How long to wait before resetting location update

    // Private variables
    private Vector3[] pastPosition;                                 // Previous position
    private Vector3[] newPosition;                                  // Target position
    private float lerp = 1;                                         // ?????????????????????????????????????????????????????????
    private Vector3[] localOriginalFeetPosition;                    // The local position transform for the feet 
    private Vector3 previousLocation;                               // Past location - For Velocity calculations
    private Vector3 currentVelocity;                                  // Calculated velocity of body

    private void Awake()
    {
        pastPosition = new Vector3[feetPoints.Length];
        newPosition = new Vector3[feetPoints.Length];
        localOriginalFeetPosition = new Vector3[feetPoints.Length];
        // Assign past position properly
        for (int i = 0; i < feetPoints.Length; i++)
        {
            pastPosition[i] = feetPoints[i].transform.position;
            newPosition[i] = feetPoints[i].transform.position;
            localOriginalFeetPosition[i] = transform.InverseTransformPoint(feetPoints[i].transform.position); // Spider root object
        }
    }

    private void Update()
    {
        UpdateMainBodyPosition();
        GetVelocity();
        UpdateLegPositions();
    }

    private void UpdateMainBodyPosition()
    {
        RaycastHit raycastHit;
        // If floor is found
        if (Physics.Raycast(mainBody.position + new Vector3(0, 10, 0), Vector3.down, out raycastHit, Mathf.Infinity, layerMask))
            mainBody.position = new Vector3(mainBody.position.x, raycastHit.point.y + bodyHoverHeight, mainBody.position.z);
    }
    
    private void UpdateLegPositions()
    {
        for (int i = 0; i < legPoints.Length; i++)
        {
            Vector3 movePos = pastPosition[i];

            // Check if should move leg
            if (Vector3.Distance(localOriginalFeetPosition[i], transform.InverseTransformPoint(feetPoints[i].transform.position)) > 0.1f)
            {
                Debug.Log("Called");
                newPosition[i] = transform.position + localOriginalFeetPosition[i];
                footLocationUpdateTimer = 0;

                // If should move leg, find new position
                if (Physics.Raycast(feetPoints[i].transform.parent.position, (feetPoints[i].transform.position - feetPoints[i].transform.parent.position), out RaycastHit raycastHit, Mathf.Infinity, layerMask))
                {
                    newPosition[i] = new Vector3(transform.position.x + localOriginalFeetPosition[i].x + currentVelocity.x, raycastHit.point.y + currentVelocity.y, transform.position.z + localOriginalFeetPosition[i].z + currentVelocity.z);
                }
            }
            else
            {
                footLocationUpdateTimer += Time.deltaTime * 1;
            } // Working

            //if (lerp < 0)
            //{
            //    movePos = Vector3.Lerp(pastPosition[i], newPosition[i], lerp);
            //    movePos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            //    lerp += Time.deltaTime * moveSpeed;
            //}
            //else
            //{
            //    pastPosition[i] = newPosition[i];
            //} // Ignore lerp

            legPoints[i].transform.position = movePos;
        }
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
