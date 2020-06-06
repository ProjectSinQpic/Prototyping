using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum SoundEffect {
    menu_select,
    menu_cancel,
    attack01,
}

public enum BackGroundMusic {
    battle01,
}


public class SoundPlayer : MonoBehaviour {

    [SerializeField]
    SoundEffectTable seTable;

    [SerializeField]
    BackGroundMusicTable musicTable;

    public static SoundPlayer instance;

    AudioSource[] sources;

    void Awake() {
        instance = this;
        sources = GetComponents<AudioSource>();
    }

    public void PlaySoundEffect(SoundEffect type) {
        var free = sources.Where(s => !s.isPlaying).FirstOrDefault();
        if(free == null) return;
        free.clip = seTable.GetTable()[type];
        free.Play();
    }

    public void PlayBackGroundMusic(BackGroundMusic type) {
        AudioSource source = sources.Last();
        source.clip = musicTable.GetTable()[type];
        source.Play();
    }

    [System.Serializable]
    public class SoundEffectTable : Serialize.TableBase<SoundEffect, AudioClip, SoundEffectPair>{
    }


    [System.Serializable]
    public class SoundEffectPair : Serialize.KeyAndValue<SoundEffect, AudioClip>{
        public SoundEffectPair (SoundEffect key, AudioClip value) : base (key, value) {
        }
    }

    [System.Serializable]
    public class BackGroundMusicTable : Serialize.TableBase<BackGroundMusic, AudioClip, BackGroundPair>{
    }


    [System.Serializable]
    public class BackGroundPair : Serialize.KeyAndValue<BackGroundMusic, AudioClip>{
        public BackGroundPair (BackGroundMusic key, AudioClip value) : base (key, value) {
        }
    }


}


