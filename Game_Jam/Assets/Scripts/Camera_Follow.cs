//Code by MaykG (Miquel Suau)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed = 0;
    public Vector3 offset;

    private void Update()
    {

        Vector3 desiredPosition = target.position + offset;
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = desiredPosition;

        transform.LookAt(target);

    }


}
