using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour {

    public ReactiveProperty<bool> isPlaying;

    Dictionary<string, List<Sprite>> animationList = new Dictionary<string, List<Sprite>> ();
    SpriteRenderer sp;
    Coroutine nowAnim;

    void Awake () {
        isPlaying = new ReactiveProperty<bool> (false);
        sp = GetComponent<SpriteRenderer> ();
    }

    public void AddAnimation (string key, List<Sprite> sprites) {
        animationList[key] = new List<Sprite> (sprites);
    }

    public void Play (string key, float duration, bool isLoop) {
        if(!animationList.ContainsKey(key)) return;
        if (nowAnim != null) StopCoroutine (nowAnim);
        if (isLoop) nowAnim = StartCoroutine (PlayLoopCoroutine (animationList[key], duration));
        else nowAnim = StartCoroutine (PlaySingleCoroutine (animationList[key], duration));
    }

    IEnumerator PlaySingleCoroutine (List<Sprite> sprites, float duration) {
        var interval = duration / sprites.Count;
        isPlaying.Value = true;
        for (int i = 0; i < sprites.Count; i++) {
            sp.sprite = sprites[i];
            yield return new WaitForSeconds (interval);
        }
        isPlaying.Value = false;
    }

    IEnumerator PlayLoopCoroutine (List<Sprite> sprites, float duration) {
        var interval = duration / sprites.Count;
        isPlaying.Value = true;
        while (true) {
            for (int i = 0; i < sprites.Count; i++) {
                sp.sprite = sprites[i];
                yield return new WaitForSeconds (interval);
            }
        }
    }

}