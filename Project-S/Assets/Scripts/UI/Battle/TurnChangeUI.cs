using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;

public class TurnChangeUI : MonoBehaviour {

    public Sprite sprite_banner_blue, sprite_banner_red;
    public Sprite sprite_text_blue, sprite_text_red;

    public Transform transform_banner,  transform_text;

    public float time_appear, time_wait, time_disappear;
    public Ease ease_appear_banner, ease_appear_text, ease_disappear_banner, ease_disappear_text;

    public float text_initial_size, text_end_size;

    public void Start() {
        InitPos();
        GameState.instance.turn
            .Subscribe(t => Appear(t));
    }

    void InitPos() {
        transform_banner.position = new Vector3(Screen.width * 1.5f, Screen.height / 2, 0);
        transform_text.localScale = Vector3.one * 1.5f;
        transform_text.GetComponent<Image>().color = new Color(1, 1, 1, 0);
    }

    public void Appear(Turn_State turn) {
        MapPointer.instance.SetActive(false, false);
        ViewOperater.instance.SetActive(false);
        if(turn == Turn_State.blue || turn == Turn_State.red) {
            AppearTurnBanner(turn);
        }
    }

    private void AppearTurnBanner(Turn_State turn) {
        var banner = turn == Turn_State.blue ? sprite_banner_blue : sprite_banner_red; 
        var text = turn == Turn_State.blue ? sprite_text_blue : sprite_text_red; 
        transform_text.localScale = Vector3.one;
        transform_banner.GetComponent<Image>().sprite = banner;
        transform_text.GetComponent<Image>().sprite = text;
        transform_banner.position = new Vector3(Screen.width * 1.5f, Screen.height / 2, 0);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform_banner.DOMoveX(Screen.width / 2, time_appear).SetEase(ease_appear_banner));
        sequence.Join(transform_text.DOScale(text_end_size, time_appear).SetEase(ease_appear_text));
        sequence.Join(DOTween.ToAlpha(() => transform_text.GetComponent<Image>().color, color => transform_text.GetComponent<Image>().color = color,
            1, time_appear));
        sequence.AppendInterval(time_wait);
        sequence.Append(transform_banner.DOMoveX(-Screen.width / 2, time_disappear).SetEase(ease_disappear_banner));
        sequence.Join(transform_text.DOScale(text_initial_size, time_appear).SetEase(ease_disappear_text));
        sequence.Join(DOTween.ToAlpha(() => transform_text.GetComponent<Image>().color, color => transform_text.GetComponent<Image>().color = color,
            0, time_appear));
        sequence.AppendCallback(() => {
            MapPointer.instance.SetActive(true, true);
            ViewOperater.instance.SetActive(true);
            transform_text.localScale = Vector3.zero;
        });
        sequence.Play();
    }

    

}
