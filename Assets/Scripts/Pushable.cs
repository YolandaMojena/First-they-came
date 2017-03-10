using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour {

    const float WIDTH = 2f, HEIGHT = 1f;
    const int NUM_HOR_RAYS = 3;
    const int NUM_VER_RAYS = 5;
    const float CORTEX_WIDTH = 0.1f;

    struct RaycastOrigins
    {
        public Vector2 botLeft, botRight;
    }

    RaycastOrigins raycastOrigins;

    void UpdateRaycastOrigins()
    {
        raycastOrigins.botLeft = transform.position - new Vector3(WIDTH / 2f, HEIGHT / 2f, 0f);
        raycastOrigins.botRight = transform.position - new Vector3(WIDTH / 2f, HEIGHT / 2f, 0f);
    }

    public void Push(Vector3 traslation)
    {
        UpdateRaycastOrigins();
        CheckHorizontalCollision(ref traslation);
        CheckVerticalCollision(ref traslation);
        Traslate(ref traslation);
    }

    void CheckHorizontalCollision(ref Vector3 traslation)
    {
        if(Mathf.Abs(traslation.x) > 0)
        {
            float directionX = Mathf.Sign(traslation.x);
            float rayLength = Mathf.Abs(traslation.x);

            for (int i = 0; i < NUM_HOR_RAYS; i++)
            {
                Vector2 rayOrigin = (directionX == 1 ? raycastOrigins.botRight : raycastOrigins.botLeft);
            }
        }
    }
    void CheckVerticalCollision(ref Vector3 traslation)
    {

    }
    void Traslate(ref Vector3 traslation)
    {
        transform.position += traslation;
    }
}
