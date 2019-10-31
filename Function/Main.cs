using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    private IniFiles iniFile;
    public new AudioSource audio;

    // Start is called before the first frame update

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

    // Update is called once per frame
    void Update()
    {
       
    }
    public void StartOnClick() {
        SceneManager.LoadScene("GameScene");
    }
    public void OptionOnclick() {
        SceneManager.LoadScene("MusicScene");
    }
    public void ExitOnclick() {
        Application.Quit();
    }
}
