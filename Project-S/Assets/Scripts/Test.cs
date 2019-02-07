using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Test : MonoBehaviour
{
    public MapPointer pointer;
    public KnightMovement km;
    // Start is called before the first frame update
    void Start()
    {
        pointer.OnClick.Subscribe(x => km.MoveToPoint(x));
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
