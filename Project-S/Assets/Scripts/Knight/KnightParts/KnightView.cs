using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UniRx.Triggers;

public class KnightView : KnightParts {

    public AnimationPlayer anim;
    List<Sprite> idle_front, idle_back;
    List<Sprite> move_front, move_back;
    List<Sprite> attack_front, attack_back;
    SpriteRenderer sp;
    Dictionary<string, float> animSpeed = new Dictionary<string, float> () { { "idle", 1f }, { "move", 0.25f }, { "attack", 0.25f },
    };

    public string charaName;
    public Sprite charaImage;
    public Vector2 charaImageOffset_StatusUI, charaImageOffset_AttackResultUI;


    public void Init () {
        sp = GetComponent<SpriteRenderer> ();
        idle_front = core.status.data.view.image_idle_front;
        idle_back = core.status.data.view.image_idle_back;
        move_front = core.status.data.view.image_move_front;
        move_back = core.status.data.view.image_move_back;
        attack_front = core.status.data.view.image_attack_front;
        attack_back = core.status.data.view.image_attack_back;
        charaName = core.status.data.view.characterName;
        charaImage = core.status.data.view.characterImage;
        charaImageOffset_StatusUI = core.status.data.view.imageOffset_StatusUI;
        charaImageOffset_AttackResultUI = core.status.data.view.imageOffset_AttackResultUI;
        InitAnimation ();
        ChangeDir(ViewOperater.viewDir.Value);
    }

    void Start () {
        ViewOperater.viewDir
            .Where(_ => core.isReady)
            .Subscribe (d => ChangeDir (d));
        anim.isPlaying
            .Where (x => !x)
            .Where(_ => core.isReady)
            .Subscribe (_ => ActionView ("idle", core.status.dir));
        this.UpdateAsObservable()
            .Select(_ => core.status.rest)
            .DistinctUntilChanged()
            .Subscribe(c => ChangeRestState(c));

    }

    void InitAnimation () {
        anim.AddAnimation ("idle_front", idle_front);
        anim.AddAnimation ("idle_back", idle_back);
        anim.AddAnimation ("move_front", move_front);
        anim.AddAnimation ("move_back", move_back);
        anim.AddAnimation ("attack_front", attack_front);
        anim.AddAnimation ("attack_back", attack_back);
    }

    void ChangeDir (Direction dir) {
        var rot = new Vector3 (0, (int) ViewOperater.viewDir.Value * 90 + 45, 0);
        transform.DORotate (rot, 0.5f).SetEase (Ease.OutCirc);

        ActionView ("idle", core.status.dir);
    }

    public void ActionView (string action, Direction dir) {
        var spriteDir = (Direction) Mathf.Repeat ((int) dir - (int) ViewOperater.viewDir.Value, 4);

        var d = spriteDir == Direction.NORTH || spriteDir == Direction.WEST ? "front" : "back";
        var f = spriteDir == Direction.NORTH || spriteDir == Direction.EAST ? 1 : -1;
        var isLoop = action == "idle" || action == "move";
        var speed = animSpeed[action];

        var s = transform.localScale;
        transform.localScale = new Vector3 (f * Mathf.Abs (s.x), s.y, s.z);
        var key = action + "_" + d;
        anim.Play (key, speed, isLoop);
    }

    void ChangeRestState(int c) {
        if(c == 0) {
            sp.material.color = Color.white;
        }
        else {
            sp.material.color = Color.gray;
        }
    }

}