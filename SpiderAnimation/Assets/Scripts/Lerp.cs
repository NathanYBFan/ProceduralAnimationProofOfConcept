using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerp : MonoBehaviour
{
    [SerializeField]
    private GameObject startPoint;

    [SerializeField]
    private GameObject endPoint;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float lerp;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float maxDistance = 1;

    [SerializeField]
    private bool isStepping = false;

    [SerializeField]
    private Vector3 velocity;

    private void Update()
    {
        LerpCheck();
    }

    private void LerpCheck()
    {
        Vector3 movePos = transform.position;

        // If should step
        if (Vector3.Distance(startPoint.transform.position, target.transform.position) > maxDistance && !isStepping)
        {
            velocity = new Vector3((endPoint.transform.position.x + startPoint.transform.position.x) / Time.deltaTime, 0, (endPoint.transform.position.z + startPoint.transform.position.z) / Time.deltaTime);
            lerp = speed;
            isStepping = true;
        }

        if (lerp > 0)
        {
            movePos = Vector3.Lerp(endPoint.transform.position, startPoint.transform.position, lerp);
            movePos.y += Mathf.Sin(lerp * Mathf.PI);

            transform.position = movePos;
            lerp -= Time.deltaTime;
        }
        else if (lerp < 0)
        {
            lerp = 0;
            startPoint.transform.position = endPoint.transform.position;
            isStepping = false;
        }
        else
        {
            transform.position = startPoint.transform.position;
        }
    }
}
