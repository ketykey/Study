using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using common;

public class SyncPositionRequest : Request
{

    [HideInInspector]
    public ChessData pos;
    //发起位置信息请求
    public override void DefaultRequse()
    {
        //把位置信息x,y传递给服务器端
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.x, pos.x);
        data.Add((byte)ParameterCode.y, pos.y);

        PhotonEngine.Peer.OpCustom((byte)OpCode, data, true);//把Player位置传递给服务器
    }

    public override void OnOperationResponse(OperationResponse operationResponse)
    {
        throw new NotImplementedException();
    }
}


