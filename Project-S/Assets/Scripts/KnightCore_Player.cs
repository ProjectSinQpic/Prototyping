using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class KnightCore_Player : KnightCore {

    void Start() {
        MapPointer.instance.OnClickedKnight
                   .Where(o => o == gameObject)
                   .Subscribe(_ => isSelected.Value = !isSelected.Value);
        MapPointer.instance.OnClickedKnight
                           .Where(o => o != gameObject)
                           .Subscribe(_ => isSelected.Value = false);
        MapPointer.instance.OnClickedMap
                   .Where(_ => isSelected.Value == true)
                   .Subscribe(v => { next_pos = v; NextAction("move"); });
        isSelected
            .Where(b => b == true)
            .Subscribe(_ => StatusUI.Instance().UpdateUI(status));
    }
}
