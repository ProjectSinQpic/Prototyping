using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class SoundPlayer : MonoBehaviour {

    [SerializeField]
    SoundTable seTable;

    [SerializeField]
    SoundTable musicTable;

    public static SoundPlayer instance;

    AudioSource[] sources;

    void Awake() {
        instance = this;
        sources = GetComponents<AudioSource>();
    }

    public void PlaySoundEffect(string type) {
        var free = sources.Where(s => !s.isPlaying).FirstOrDefault();
        if(free == null) return;
        free.clip = seTable.GetTable()[type];
        free.Play();
    }

    public void PlayBackGroundMusic(string type) {
        AudioSource source = sources.Last();
        source.clip = musicTable.GetTable()[type];
        source.Play();
    }

    [System.Serializable]
    public class SoundTable : Serialize.TableBase<string, AudioClip, SoundPair>{
    }


    [System.Serializable]
    public class SoundPair : Serialize.KeyAndValue<string, AudioClip>{
        public SoundPair (string key, AudioClip value) : base (key, value) {
        }
    }


}


