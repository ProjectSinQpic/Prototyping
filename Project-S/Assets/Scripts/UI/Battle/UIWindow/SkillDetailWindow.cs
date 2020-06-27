using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillDetailWindow : UIWindow {

    public Text text_skillName;
    public Text text_skillExplainText;

    public static SkillDetailWindow instance;

    void Awake() {
        if(instance == null) instance = this;
        transform.localScale = Vector3.zero;
    }

    public void OpenWindow(Vector2 pos, SkillBase skill) {
        transform.localScale = Vector3.one;
        transform.position = pos;
        text_skillName.text = skill.skillName;
        text_skillExplainText.text = SkillExplainTextBuilder.Build(skill);
    }

    public void CloseWindow() {
        transform.localScale = Vector3.zero;
    }


}
