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
        //MenuGenerator.Instance().Create(new Dictionary<string, UnityEngine.Events.UnityAction> {
        //    { "あああ", () => { Debug.Log("aaa"); } },
        //    { "いいい", () => { MenuGenerator.Instance().Close(); } },
        //    { "ううう", () => { } },
        //});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
