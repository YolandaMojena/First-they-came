using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableInterruptor : MonoBehaviour {

    [SerializeField]
    SceneElement[] objectsToOrificate;
    [SerializeField]
    GameObject pushable;

    float activationDist = 1.0f;
    bool activated = false;

    void Update()
    {
        if (!activated)
        {
            if(Mathf.Abs(pushable.transform.position.x - transform.position.x) <= activationDist){
                if (objectsToOrificate != null)
                    foreach (SceneElement s in objectsToOrificate)
                        s.TurnIntoGold();

                activated = true;
            } 
        }
    }
}
