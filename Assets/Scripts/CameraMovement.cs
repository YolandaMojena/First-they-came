using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject leftBound;
    [SerializeField]
    GameObject rightBound;
    [SerializeField]
    Vector3 offset;

    Vector3 size;

    // Use this for initialization
    void Start()
    {
        size = OrthographicBounds().size;
        transform.position = new Vector3(player.transform.position.x + offset.x, transform.position.y, transform.position.z);
        leftBound.transform.position = new Vector3(transform.position.x - size.x / 2, transform.position.y, transform.position.z);
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        Vector3 targetPos = player.transform.position + offset;

        if (targetPos.x <= (leftBound.transform.position.x + size.x / 2))
            targetPos.x = transform.position.x;

        else if (targetPos.x >= (rightBound.transform.position.x - size.x / 2))
            targetPos.x = transform.position.x;


        transform.position = new Vector3(targetPos.x, transform.position.y, transform.position.z);      
    }

    Bounds OrthographicBounds()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = Camera.main.orthographicSize * 2;
        Bounds bounds = new Bounds(
            Camera.main.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}
