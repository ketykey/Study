using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicSet : MonoBehaviour
{
    private IniFiles iniFile;
    public new AudioSource audio;
    public Slider volumeSlider;
    public float volume;
    private void Awake()
    {
        iniFile = this.gameObject.AddComponent<IniFiles>();
        iniFile.inipath = Application.dataPath + @"\config.ini";
    }
    void Start()
    {
        if (iniFile.ExistINIFile())
        {
            
            //读取配置文件中音量设置 
            audio.volume = Convert.ToSingle(iniFile.IniReadValue("Music", "volume")) / 100;
            volumeSlider.value = audio.volume;
            //当配置文件中音乐状态为1时开启音乐播放，为0时关闭播放
            if (iniFile.IniReadValue("Music", "state") == "1")
            {
                audio.Play();
            }
            else if (iniFile.IniReadValue("Music", "state") == "0")
            {
                audio.Stop();
            }
            else { }
            //当配置文件中音乐循环为1时开启循环，游戏内默认为1且不可调整循环键
            if (iniFile.IniReadValue("Music", "loop") == "1")
            {
                audio.loop = true;
            }
        }
        else
        {
            audio.Stop();
            Debug.Log("Error in founding config.ini");
        }
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

    }
    public void MusicOnclick()
    {
        if (!audio.isPlaying)
        {
            audio.Play();
        }
        else if (audio.isPlaying)
        {
            // audio.Pause();
            audio.Stop();
        }
    }


    /*public void StopClick()
    {
        if (audio.isPlaying)
        {
            audio.Stop();
        }
    }*/
    public void AcceptOnclick() {
        string restate;
        //为了方便，config.ini中的音量保存为0~100的整数，故乘以100再进行转换
        float _revolume = volume*100;
        string revolume = _revolume.ToString();
        //检测当前是否在播放音乐，并将该状态保存
        if (audio.isPlaying)
        {
            restate = "1";
        }
        else
        {
            restate = "0";
        }
        iniFile.IniWriteValue("Music", "volume", revolume);
        iniFile.IniWriteValue("Music", "state", restate);
    }

    public void OKOncilck() {
        string restate;
        //为了方便，config.ini中的音量保存为0~100的整数，故乘以100再进行转换
        float _revolume = volume * 100;
        string revolume = _revolume.ToString();
        //检测当前是否在播放音乐，并将该状态保存
        if (audio.isPlaying)
        {
            restate = "1";
        }
        else
        {
            restate = "0";
        }
        iniFile.IniWriteValue("Music", "volume", revolume);
        iniFile.IniWriteValue("Music", "state", restate);
        SceneManager.LoadScene("MainScene");
    }

    public void CancelOnclick() {
        SceneManager.LoadScene("MainScene");
    }
    public void SliderChange()
    {
        if (audio.isPlaying)
        {
            volume = volumeSlider.value;
            audio.volume = volume;
        }
    }
}
