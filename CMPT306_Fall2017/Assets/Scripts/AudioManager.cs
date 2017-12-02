using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public struct SoundDict
    {
        public int AscendingTone;
        public int Bad;
        public int Bring;
        public int Brrrr;
        public int Charge;
        public int Click;
        public int ClickLow;
        public int Death;
        public int Good;
        public int Hit_00;
        public int Hit_01;
        public int Hit_02;
        public int Jump;
        public int LowDing;
        public int Ouch;
        public int Ouch_2;
        public int Pop;
        public int Pop_2;
        public int Swing;
        public int Thud;
        public int Thwam;
        public int Woosh_2;
    }

    AudioSource mySource;
    public SoundDict SoundIndex;
    
    public int curSong;
    public AudioClip[] music;
    public AudioClip[] sfx;
    

    void ConstructSoundDict()
    {
        SoundIndex.AscendingTone = 0;
        SoundIndex.Bad = 1;
        SoundIndex.Bring = 2;
        SoundIndex.Brrrr = 3;
        SoundIndex.Charge = 4;
        SoundIndex.Click = 5;
        SoundIndex.ClickLow = 6;
        SoundIndex.Death = 7;
        SoundIndex.Good = 8;
        SoundIndex.Hit_00 = 9;
        SoundIndex.Hit_01 = 10;
        SoundIndex.Hit_02 = 11;
        SoundIndex.Jump = 12;
        SoundIndex.LowDing = 13;
        SoundIndex.Ouch = 14;
        SoundIndex.Ouch_2 = 15;
        SoundIndex.Pop = 16;
        SoundIndex.Pop_2 = 17;
        SoundIndex.Swing = 18;
        SoundIndex.Thud = 19;
        SoundIndex.Thwam = 20;
        SoundIndex.Woosh_2 = 21;
    }

    public void PlaySound(int index)
    {
        mySource.PlayOneShot(sfx[index]);
    }


    // Use this for initialization
    void Start () {
        mySource = gameObject.GetComponent<AudioSource>();

        curSong = Random.Range(0, music.Length);

        SoundIndex = new SoundDict();
        ConstructSoundDict();
	}
	
	// Update is called once per frame
	void Update () {
        if (!mySource.isPlaying)
        {
            curSong++;

            if (curSong == music.Length)
                curSong = 0;

            mySource.PlayOneShot(music[curSong]);

        }
	}
}
