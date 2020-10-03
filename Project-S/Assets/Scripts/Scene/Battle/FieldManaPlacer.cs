using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FieldManaPlacer : MonoBehaviour {

    [System.Serializable]
    public class FieldMana {
        public Vector2 pos;
        public GameObject obj;
        public int amount;

        public FieldMana(Vector2 pos, GameObject obj, int amount) {
            this.pos = pos;
            this.obj = obj;
            this.amount = amount;
        }
    }

    public static FieldManaPlacer instance;

    public GameObject prefab_mana;
    public List<FieldMana> manas;

    public float manaPosY;
    public float minAmount, maxAmount;
    public float minSize, maxSize;
    public Transform container;

    void Awake() {
        instance = this;
    }

    void Start() {
        StartCoroutine(ManaFlowCoroutine());
    }

    public void PlaceMana(Vector2 pos, int amount) {
        var worldPos = MapStatus.ToWorldPos(pos);
        var obj = Instantiate(prefab_mana, container);
        obj.transform.localPosition = worldPos + Vector3.up * manaPosY;
        var size = Mathf.Lerp(minSize, maxSize, Mathf.InverseLerp(minAmount, maxAmount, amount));
        obj.transform.localScale = Vector3.one * size;
        manas.Add(new FieldMana(pos, obj, amount));
    }

    public void GetMana(KnightCore core) {
        var mana = manas.Find(m => m.pos == core.next_pos);
        core.status.MP = Mathf.Min(core.status.MP + mana.amount, core.statusData.maxMP);
        manas.Remove(mana);
        Destroy(mana.obj);
    }

    public bool IsGettableMana(Vector2 pos) {
        return manas.Any(m => m.pos == pos);
    }

    IEnumerator ManaFlowCoroutine() {
        float t = 0;
        while(true) {
            manas.ForEach(m => {
                var m_pos = m.obj.transform.localPosition;
                m_pos.y = manaPosY + Mathf.Sin(t * 3f);
                m.obj.transform.localPosition = m_pos;
            });
            yield return null;
            t += Time.deltaTime;
        }
    }


}
