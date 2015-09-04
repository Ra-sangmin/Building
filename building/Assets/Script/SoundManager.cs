using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public AudioSource BGM;

    public List<AudioSource> OneShotList = new List<AudioSource>();

    private static SoundManager m_instans;
    public static SoundManager instans
    {
        get
        {
            if (m_instans == null)
            {
                GameObject soundManagerOBJ = null;

                if(GameObject.Find("SoundManager"))
                {
                    soundManagerOBJ = GameObject.Find("SoundManager");
                }
                else
                {
                    soundManagerOBJ = (GameObject)Instantiate(Resources.Load("Prefabs/Sound/SoundManager"));
                    soundManagerOBJ.name = "SoundManager";
                    DontDestroyOnLoad(soundManagerOBJ);

                }


                if (soundManagerOBJ == null)
                    return null;


                m_instans = soundManagerOBJ.GetComponent<SoundManager>();

            }
            
            return m_instans;
        } 
    }

    public bool soundOff;

    float effetVolum = 1;

    // Use this for initialization
    void Start()
    {
        ResetSoundValue();
    }

    public void ResetSoundValue() 
    {
        BGM.volume = 0.5f;

        for (int i = 0; i < OneShotList.Count; i++)
        {
            OneShotList[i].volume = 1;
        }
    }

    public void SoundOff(bool soundOff) 
    {
        this.soundOff = soundOff;


        float resetValue = soundOff == true ? 0 : 1;

        BGM.volume = resetValue/2;

        for (int i = 0; i < OneShotList.Count; i++)
        {
            OneShotList[i].volume = resetValue;
        }

    }

    public void EffectSoundReset(float value)
    {
        effetVolum = value;

        for (int i = 0; i < OneShotList.Count; i++)
        {
            OneShotList[i].volume = effetVolum;
        }

    }

    public void AllSoundReset(float value)
    {
        effetVolum = value;
        BGM.volume = effetVolum/2;

        for (int i = 0; i < OneShotList.Count; i++)
        {
            OneShotList[i].volume = effetVolum;
        }
    }

   
    private SoundManager()
    {
        
    }



    public enum BGMSoundEnum
    {
        Intro, Main, Ingame,Title
    }


    public void BGMPlay(BGMSoundEnum soundEnum)
    {
        string clipPath = "Sound/BGM/";

        switch (soundEnum)
        {
            case BGMSoundEnum.Intro: clipPath += "Intro"; break;
            case BGMSoundEnum.Main: clipPath += "MainBGM"; break;
            case BGMSoundEnum.Title: clipPath += "Title_bgm"; break;
            //case BGMSoundEnum.Main: clipPath += "mainmenu"; break;
            //case BGMSoundEnum.Ingame: clipPath += "ingame_01"; break;
                
        }

        AudioClip audioClip = Resources.Load(clipPath) as AudioClip; 

        BGM.Stop();
        BGM.clip = audioClip;
        BGM.loop = true;
        BGM.Play();
    }







   




    public void EffectPlay(string name, bool loop)
    {
        string clipPath = "Sound/Effect/" + name;

        AudioClip audioClip = Resources.Load(clipPath) as AudioClip;

        for (int i = 0; i < OneShotList.Count; i++)
        {
            if (OneShotList[i].isPlaying)
            {
                if (i == OneShotList.Count - 1)
                {
                    AudioSource audioSource = gameObject.AddComponent<AudioSource>();

                   // audioSource.spatialBlend = 0;

                    OneShotList.Add(audioSource);
                    
                    EffectSoundReset(effetVolum);

                }

                continue;
            }

            OneShotList[i].volume = effetVolum;
            OneShotList[i].clip = audioClip;
            OneShotList[i].Play();
            OneShotList[i].loop = loop;

            break;
        }
    }




    public enum EffectSoundEnum
    {
        Click, BuildOn,BuildCancle
    }
    public void EffectPlay(EffectSoundEnum soundEnum , bool loop)
    {
        string clipPath = "Sound/Effect/";

        switch (soundEnum)
        {
            case EffectSoundEnum.Click: clipPath += "ui_button"; break;
            case EffectSoundEnum.BuildCancle: clipPath += "제작_망치01"; break;
            case EffectSoundEnum.BuildOn: clipPath += "망치쾅"; break;
                
                
        }

        AudioClip audioClip = Resources.Load(clipPath) as AudioClip;


        for (int i = 0; i < OneShotList.Count; i++)
        {
            if (OneShotList[i].isPlaying) 
            {
                if (i == OneShotList.Count-1)
                {
                    AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                    OneShotList.Add(audioSource);

                    EffectSoundReset(effetVolum);

                }

                continue;
            }
                

            OneShotList[i].clip = audioClip;
            OneShotList[i].Play();
            OneShotList[i].loop = loop;


            break;
        }
    }


    public void EffectAllStop()
    {
        for (int i = 0; i < OneShotList.Count; i++)
        {
            if (OneShotList[i].isPlaying)
            {
                OneShotList[i].Stop();
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
