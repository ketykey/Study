using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ddd : MonoBehaviour
{
    static ddd _instance;
    // Use this for initialization   
    void Start ()
    { }
    public static ddd instance
    {        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ddd>();
                DontDestroyOnLoad(_instance.gameObject);
            }            return _instance;
        }
    }
    void Awake()
    {
        //此脚本永不消毁，并且每次进入初始场景时进行判断，若存在重复的则销毁
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != _instance)
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update ()
    { }
}

