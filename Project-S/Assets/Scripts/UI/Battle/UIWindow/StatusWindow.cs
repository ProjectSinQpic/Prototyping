using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class StatusWindow : UIWindow {

    public Text text_nowHP;
    public Text text_maxHP;
    public GameObject bar_HP;
    public Text text_nowMP;
    public Text text_maxMP;
    public GameObject bar_MP;
    public Text text_attack;
    public Text text_defense;
    public Text text_skillAttack;
    public Text text_skillDefense;
    public Text text_moveRange;
    public Text text_attackRange;
    public Text text_rest;

    public Transform skillList;
    public float popupOffset;
    public Text turn;

    public GameObject statusBox;

    static StatusWindow instance = null;

    public ReactiveProperty<KnightCore> target = new ReactiveProperty<KnightCore>(null);

    float bar_maxSize;

    public Sprite sprite_back_left_b, sprite_back_right_b;
    public Sprite sprite_back_left_r, sprite_back_right_r;
    public Image image_back_left, image_back_right;

    public Text charaNameText;
    public Image charaImage;

    void Awake () {
        if (instance == null) instance = this;
        statusBox.transform.localScale = Vector3.zero;
    }

    void Start () {
        GameState.instance.turn
            .Subscribe (x => UpdateTurn (x));

        MapPointer.instance.OnPressedRightButton
            .Subscribe(x => {
                if(x.Item2) OpenWindow(x.Item1);
                else HideWindow();
            });
        bar_maxSize = bar_HP.GetComponent<RectTransform>().rect.width;

    }

    void OpenWindow(GameObject obj) {
        if(ViewOperater.instance.isFocusing) return;
        if(ViewOperater.instance.isLocked) return;
        if(MapPointer.instance.pointedKnight == null ) return;
        target.Value = obj.GetComponent<KnightCore>();
        UpdateUI(target.Value, KnightCore.blue_all.Contains(target.Value));
        statusBox.transform.localScale = Vector3.one;
        MapPointer.instance.SetActive(false, true);
    }

    void HideWindow(){
        if(ViewOperater.instance.isFocusing) return;
        if(!ViewOperater.instance.isLocked) return;
        target.Value = null;
        statusBox.transform.localScale = Vector3.zero;
        MapPointer.instance.SetActive(true, true);
    }

    public static StatusWindow Instance () {
        return instance;
    }

    public void UpdateUI (KnightCore core, bool isBlue) {
        var status = core.status;
        var statusData = core.statusData;
        text_nowHP.text = status.HP.ToString () + " /";
        text_maxHP.text = statusData.maxHP.ToString ();
        SetBarWidth(bar_HP, status.HP, statusData.maxHP);
        text_nowMP.text = status.MP.ToString () + " /";
        text_maxMP.text = statusData.maxMP.ToString ();
        SetBarWidth(bar_MP, status.MP, statusData.maxMP);
        text_attack.text = statusData.attack.ToString ();
        text_defense.text = statusData.defense.ToString ();
        text_skillAttack.text = statusData.skillAttack.ToString ();
        text_skillDefense.text = statusData.skillDefense.ToString ();
        text_moveRange.text = statusData.moveRange.ToString ();
        text_attackRange.text = statusData.attackRange.ToString ();
        text_rest.text =  status.rest.ToString ();
        AddSkillTags(status.skills.Where(s => !s.GetIsDeleted()).ToList());

        var view = core.GetComponent<KnightView>();
        charaNameText.text = view.charaName;
        charaImage.sprite = view.charaImage;
        charaImage.transform.localPosition = view.charaImageOffset_StatusUI;
        SetBackGround(isBlue);
    }

    void UpdateTurn (Turn_State turnState) {
        if(turnState == Turn_State.blue) turn.text = "BLUE TURN";
        if(turnState == Turn_State.red) turn.text = "RED TURN";
        if(turnState == Turn_State.none) turn.text = "WAIT...";

    }

    void SetBackGround(bool isBlue) {
        if(isBlue) {
            image_back_left.sprite = sprite_back_left_b;
            image_back_right.sprite = sprite_back_right_b;
        }
        else {
            image_back_left.sprite = sprite_back_left_r;
            image_back_right.sprite = sprite_back_right_r;
        }
    }

    void SetBarWidth(GameObject bar, float now, float max) {
        var size = bar_maxSize * (now / max);
        bar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
    }

    void AddSkillTags(List<SkillBase> skills) {
        for(int i = 0; i < skillList.childCount; i++) {
            var tag = skillList.GetChild(i);
            if(i >= skills.Count) tag.gameObject.SetActive(false);
            else {
                SkillBase skill = skills[i];
                tag.gameObject.SetActive(true);
                tag.Find("name").GetComponent<Text>().text = skill.skillName;
                AddPointerEvents(tag, skill);
            }
        }
    }

    void AddPointerEvents(Transform tag, SkillBase skill) {
        EventTrigger trigger = tag.GetComponent<EventTrigger>();
        trigger.triggers = new List<EventTrigger.Entry>();
        EventTrigger.Entry onEnter = new EventTrigger.Entry();
        onEnter.eventID = EventTriggerType.PointerEnter;
        onEnter.callback.AddListener((x) => SkillDetailWindow.instance.OpenWindow(tag.position + Vector3.left * popupOffset, skill));
        trigger.triggers.Add(onEnter);
        EventTrigger.Entry onExit = new EventTrigger.Entry();
        onExit.eventID = EventTriggerType.PointerExit;
        onExit.callback.AddListener((x) => SkillDetailWindow.instance.CloseWindow());
        trigger.triggers.Add(onExit);
    }



}