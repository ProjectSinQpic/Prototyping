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
        MenuGenerator.Instance().Create(new Dictionary<string, UnityEngine.Events.UnityAction> {
            { "a", () => {
                MenuGenerator.Instance().Create(new Dictionary<string, UnityEngine.Events.UnityAction> {
                    { "b", () => { MenuGenerator.Instance().Close(); } },
                }, new Vector3(Screen.width / 2 - 400, -Screen.height / 2 + 150, 0));
            } },
        }, new Vector3(Screen.width / 2 - 180, -Screen.height / 2 + 150, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
