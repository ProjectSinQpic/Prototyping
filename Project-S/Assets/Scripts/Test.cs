using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Test : MonoBehaviour
{
    public MapPointer pointer;
    public KnightMovement km;
    public KnightDisplayArea kd;
    // Start is called before the first frame update
    void Start()
    {
        //pointer.OnClickedMap.Subscribe(x => km.MoveToPoint(x));
        //pointer.OnClickedKnight.Subscribe(_ => kd.DisplayArea());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
