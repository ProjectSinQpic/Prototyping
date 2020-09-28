using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : ScriptableObject {

    public string skillName;
    [TextArea] public string explainText;
    public Sprite imageIcon;
    protected KnightCore owner;
    public SkillParamTable param;

    public virtual void Init(KnightCore core) {
        owner = core;
    }   

    //バトルが始まった時
    public virtual void OnStartBattle(){}
    //ターンが変わった時
    public virtual void OnBeginTurn(Turn_State turn){}

    public int GetParam(string key, int defaultValue = 0) {
        try {
            return param.GetTable()[key];
        }
        catch(KeyNotFoundException e) {
            Debug.LogWarning("パラメータ " + key + " は存在しません");
            return defaultValue;
        }
    }

    protected void AddParam(string key, int value) {
        param.GetTable().Add(key, value);
    }
}


    [System.Serializable]
    public class SkillParamTable : Serialize.TableBase<string, int, SkillParamPair>{
    }


    [System.Serializable]
    public class SkillParamPair : Serialize.KeyAndValue<string, int>{
        public SkillParamPair (string key, int value) : base (key, value) {
        }
    }