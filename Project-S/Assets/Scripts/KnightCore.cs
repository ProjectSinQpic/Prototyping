using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCore : MonoBehaviour
{
    public KnightStatus status;

    void Start() {
        transform.position = new Vector3(status.pos.x, 4, status.pos.y);
    }
    
}
