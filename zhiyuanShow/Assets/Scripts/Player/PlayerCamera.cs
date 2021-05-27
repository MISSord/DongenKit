using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCamera : MonoBehaviour
{
    //Player position
    Transform playerTransform;

    [Header("Variables")]
    public float smoothFollow = 0.2f; //Smooth parameter of camera
    public Vector3 offset; //Camera position offset

    public void Start()
    {
        //Find player
        playerTransform = GameManager.Instance.player.transform;
        offset = BaseData.offset;
    }

    private void FixedUpdate()
    {
        CameraFollow();
    }
    //Camera follow method
    void CameraFollow()
    {
        Vector3 desiredPosition = playerTransform.position + offset; //Make offset
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothFollow); //Smooth move
        transform.position = smoothedPosition; //Set position
    }
}


