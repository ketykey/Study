﻿using UnityEngine;
using UnityEngine.UI;
using common;

public class Loginpanel : MonoBehaviour
{

    public GameObject RegisterPanel;//注册面板
    public InputField username;//UI界面输入的用户名
    public InputField password;//UI界面输入的密码
    private LoginRequest loginRequest;//提示信息

    public Text hintMessage;
    private void Start()
    {

        loginRequest = GetComponent<LoginRequest>();
    }
    //点击登陆按钮
    public void OnLoginButton()
    {
        hintMessage.text = "";
        loginRequest.Username = username.text;
        loginRequest.Password = password.text;
        loginRequest.DefaultRequse();
    }
    //点击注册按钮跳转到注册界面
    public void OnRegisterButton()
    {
        gameObject.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    //登陆操作，如果验证成功登陆，失败提示用户或密码错误
    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.success)
        {
            //验证成功，跳转到下一个场景
            hintMessage.text = "用户名和密码验证成功";

        }
        else if (returnCode == ReturnCode.failed)
        {

            hintMessage.text = "用户名或密码错误";
        }
    }
}
