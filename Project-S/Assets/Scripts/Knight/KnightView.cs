﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

public class KnightView : KnightParts {

    public AnimationPlayer anim;
    List<Sprite> idle_front, idle_back;    
    List<Sprite> move_front, move_back;
    List<Sprite> attack_front, attack_back;
    SpriteRenderer sp;
    Dictionary<string, float> animSpeed = new Dictionary<string, float>() {
        {"idle", 1f },
        {"move", 0.25f },
        {"attack", 0.25f },
    };

    public void Init() {
        sp = GetComponent<SpriteRenderer>();
        idle_front = core.status.data.image_idle_front;
        idle_back = core.status.data.image_idle_back;
        move_front = core.status.data.image_move_front;
        move_back = core.status.data.image_move_back;
        attack_front = core.status.data.image_attack_front;
        attack_back = core.status.data.image_attack_back;
        InitAnimation();
    }

    void Start() {
        ViewOperater.viewDir
            .Subscribe(d => ChangeDir(d));
        anim.isPlaying
            .Where(x => !x)
            .Subscribe(_ => ActionView("idle", core.status.dir));
    }

    void InitAnimation() {
        anim.AddAnimation("idle_front", idle_front);
        anim.AddAnimation("idle_back", idle_back);
        anim.AddAnimation("move_front", move_front);
        anim.AddAnimation("move_back", move_back);
        anim.AddAnimation("attack_front", attack_front);
        anim.AddAnimation("attack_back", attack_back);
    }

    void ChangeDir(Direction dir) {
        var rot = new Vector3(0, (int)ViewOperater.viewDir.Value * 90 + 45, 0);
        transform.DORotate(rot, 0.5f).SetEase(Ease.OutCirc);

        ActionView("idle", core.status.dir);
    }

    public void ActionView(string action, Direction dir) {
        var spriteDir = (Direction)Mathf.Repeat((int)dir - (int)ViewOperater.viewDir.Value, 4);

        var d = spriteDir == Direction.NORTH || spriteDir == Direction.WEST ? "front" : "back";
        var f = spriteDir == Direction.NORTH || spriteDir == Direction.EAST ? 1 : -1;
        var isLoop = action == "idle" || action == "move";
        var speed = animSpeed[action];

        var s = transform.localScale;
        transform.localScale = new Vector3(f * Mathf.Abs(s.x), s.y, s.z);
        var key = action + "_" + d;
        anim.Play(key, speed, isLoop);
    }
}