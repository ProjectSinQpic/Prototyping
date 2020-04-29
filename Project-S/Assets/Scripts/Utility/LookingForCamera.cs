using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookingForCamera : MonoBehaviour
{
    public Transform knightTransform;

    void Start()
    {
    }

    void Update()
    {
        if(knightTransform.localScale.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
