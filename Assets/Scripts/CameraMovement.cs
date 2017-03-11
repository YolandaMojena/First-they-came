using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField]
    GameObject goldCharacter;
    [SerializeField]
    GameObject plantCharacter;
    [SerializeField]
    GameObject leftBound;
    [SerializeField]
    GameObject rightBound;
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    float resetVel = 2;

    Vector3 size;
    Vector3 initPos;
    GameObject player;


    bool reset = false;

    // Use this for initialization
    void Start()
    {
        size = OrthographicBounds().size;
        initPos = transform.position;
        player = goldCharacter;
        leftBound.transform.position = new Vector3(transform.position.x - size.x / 2, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (reset)
        {
            transform.position = Vector3.Lerp(transform.position, initPos, Time.deltaTime * resetVel);
            if (Mathf.Abs(transform.position.x - initPos.x) <= 0.1f)
            {
                reset = false;
                player.SetActive(true);
            }               
        }
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        if (!reset)
        {
            Vector3 targetPos = player.transform.position + offset;

            if (targetPos.x <= (leftBound.transform.position.x + size.x / 2))
                targetPos.x = transform.position.x;

            else if (targetPos.x >= (rightBound.transform.position.x - size.x / 2))
                targetPos.x = transform.position.x;

            transform.position = new Vector3(targetPos.x, transform.position.y, transform.position.z);
        }         
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

    public void ResetCamera() {

        reset = true;
        player = plantCharacter;
    }

    public void ResetForDeath()
    {
        reset = true;
    }
}
