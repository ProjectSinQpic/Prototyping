using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SkillExplainTextBuilder {

    public static string Build(SkillBase skill) {
        string text = skill.explainText;
        while(true) {
            Regex regex = new Regex("<.*?>");
            Match match = regex.Match(text);
            if(!match.Success) break;
            string key = match.ToString().Replace("<", "").Replace(">", "");
            text = regex.Replace(text, skill.GetParam(key).ToString());
        }

        return text;
    }  

}
