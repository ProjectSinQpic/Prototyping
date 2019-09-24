using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour {

    Dictionary<string, List<Sprite>> animationList;
    SpriteRenderer sp;

    void Start() {
        animationList = new Dictionary<string, List<Sprite>>();
        sp = GetComponent<SpriteRenderer>();
    }

    public void AddAnimation(string key, List<Sprite> sprites) {
        animationList[key] = new List<Sprite>(sprites);
    }

    public void Play(string key, float duration, bool isLoop) {
        if (isLoop) StartCoroutine(PlayLoopCoroutine(animationList[key], duration));
        else StartCoroutine(PlaySingleCoroutine(animationList[key], duration));
    }

    IEnumerator PlaySingleCoroutine(List<Sprite> sprites, float duration) {
        var interval = duration / sprites.Count;
        for(int i = 0; i < sprites.Count; i++) {
            sp.sprite = sprites[i];
            yield return new WaitForSeconds(interval);
        }
    }

    IEnumerator PlayLoopCoroutine(List<Sprite> sprites, float duration) {
        var interval = duration / sprites.Count;
        while (true) {
            for (int i = 0; i < sprites.Count; i++) {
                sp.sprite = sprites[i];
                yield return new WaitForSeconds(interval);
            }
        }
    }

}
