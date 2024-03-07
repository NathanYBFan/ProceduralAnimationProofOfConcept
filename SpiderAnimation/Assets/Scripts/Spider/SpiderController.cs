using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField]
    private Transform mainBody;

    [SerializeField]
    private GameObject[] legPoints;

    [SerializeField]
    private GameObject[] feetPoints;

    [SerializeField]
    private GameObject[] targetFeetPoints;

    [Header("Other Info")]
    [SerializeField]
    private LayerMask layerMask;

    [Header("Stats")]
    [SerializeField]
    private float bodyHoverHeight;

    [SerializeField]
    private float maxDistFromTarget = 2;

    // Private variables
    private Vector3[] pastPosition;

    private void Awake()
    {
        pastPosition = new Vector3[feetPoints.Length];
        // Assign past position properly
        for (int i = 0; i < feetPoints.Length; i++)
            pastPosition[i] = feetPoints[i].transform.position;
    }

    private void Update()
    {
        UpdateMainBodyPosition();
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
            RaycastHit raycastHit;
            Vector3 movePos = new Vector3();
            movePos = pastPosition[i];

            // Check if should move leg
            if (Vector3.Distance(targetFeetPoints[i].transform.position, feetPoints[i].transform.position) > maxDistFromTarget)
            {
                movePos = targetFeetPoints[i].transform.position;
                pastPosition[i] = targetFeetPoints[i].transform.position;
            }

            // If floor is found
            if (Physics.Raycast(feetPoints[i].transform.position + new Vector3(0, 10, 0), Vector3.down, out raycastHit, Mathf.Infinity, layerMask))
            {
                movePos = new Vector3(movePos.x, raycastHit.point.y, movePos.z);
                targetFeetPoints[i].transform.position = new Vector3(targetFeetPoints[i].transform.position.x, raycastHit.point.y, targetFeetPoints[i].transform.position.z);
            }

            legPoints[i].transform.position = movePos;
        }
    }
}
