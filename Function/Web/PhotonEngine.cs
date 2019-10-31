using UnityEngine;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using common;

public class PhotonEngine : MonoBehaviour, IPhotonPeerListener
{

    public static PhotonEngine Instance;
    private static PhotonPeer peer;
    public static PhotonPeer Peer//让外界可以访问我们的PhotonPeer
    {
        get
        {
            return peer;
        }
    }
    private Dictionary<OperationCode, Request> RequestDict = new Dictionary<OperationCode, Request>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject); return;
        }
    }

    // Use this for initialization
    void Start()
    {
        //连接服务器端
        //通过Listender连接服务器端的响应
        //第一个参数 指定一个Licensed(监听器) ,第二个参数使用什么协议
        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        //连接 UDP的 Ip地址：端口号，Application的名字
        peer.Connect("127.0.0.1:5055", "GameServer");

    }

    // Update is called once per frame
    void Update()
    {

        peer.Service();//需要一直调用Service方法,时时处理跟服务器端的连接
    }
    //当游戏关闭的时候（停止运行）调用OnDestroy
    private void OnDestroy()
    {
        //如果peer不等于空并且状态为正在连接
        if (peer != null && peer.PeerState == PeerStateValue.Connected)
        {
            peer.Disconnect();//断开连接
        }
    }

    //
    public void DebugReturn(DebugLevel level, string message)
    {

    }
    //如果客户端没有发起请求，但是服务器端向客户端通知一些事情的时候就会通过OnEvent来进行响应 
    public void OnEvent(EventData eventData)
    {
        //如果客户端没有发起请求，但是服务器端向客户端通知一些事情的时候就会通过OnEvent来进行响应 
    }
    //当我们在客户端向服务器端发起请求后，服务器端接受处理这个请求给客户端一个响应就会在这个方法里进行处理
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        OperationCode opCode = (OperationCode)operationResponse.OperationCode;//得到响应的OperationCode

        Request request = null;
        bool temp = RequestDict.TryGetValue(opCode, out request);//是否得到这个响应
                                                                 // 如果得到这个响应
        if (temp)
        {
            request.OnOperationResponse(operationResponse);//处理Request里面的响应
        }
        else
        {
            Debug.Log("没有找到对应的响应处理对象");
        }
    }
    //如果连接状态发生改变的时候就会触发这个方法。
    //连接状态有五种，正在连接中(PeerStateValue.Connecting)，已经连接上（PeerStateValue.Connected），正在断开连接中( PeerStateValue.Disconnecting)，已经断开连接(PeerStateValue.Disconnected)，正在进行初始化(PeerStateValue.InitializingApplication)
    public void OnStatusChanged(StatusCode statusCode)
    {
        
    }
    //添加Requst
    public void AddRequst(Request requst)
    {
        RequestDict.Add(requst.OpCode, requst);

    }
    //移除Requst
    public void RemoveRequst(Request request)
    {
        RequestDict.Remove(request.OpCode);
    }

}
