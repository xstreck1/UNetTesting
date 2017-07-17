using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class MySimpleServer : NetworkServerSimple
{
    public void ReceiveMessage()
    {
        int recHostId;
        int recConnectionId;
        int recChannelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostId, out recConnectionId, out recChannelId, recBuffer, bufferSize, out dataSize, out error);

        switch (recNetworkEvent)
        {
            case NetworkEventType.Nothing:
                break;
            case NetworkEventType.ConnectEvent:
                Logger.Log("ReceiveMessage: incoming connection event received");
                break;
            case NetworkEventType.DataEvent:
                Stream stream = new MemoryStream(recBuffer);
                BinaryFormatter formatter = new BinaryFormatter();
                string message = formatter.Deserialize(stream) as string;
                Logger.Log("ReceiveMessage: incoming message event received: " + message);
                break;
            case NetworkEventType.DisconnectEvent:
                Logger.Log("ReceiveMessage: remote client event disconnected");
                break;
        }
    }

    public override void OnConnected(NetworkConnection conn)
    {
        base.OnConnected(conn);
        Logger.Log("override OnConnected from: " + conn.address + " number of connections: "  + connections.Count);
    }

    public override void OnConnectError(int connectionId, byte error)
    {
        base.OnConnectError(connectionId, error);
        Logger.Log("override OnConnectError with ID " + connectionId);
    }

    public override void OnDisconnected(NetworkConnection conn)
    {
        base.OnDisconnected(conn);
        Logger.Log("override OnDisconnected at: " + conn.address + ". Last error: " + conn.lastError);
    }

    public override void OnDisconnectError(NetworkConnection conn, byte error)
    {
        base.OnDisconnectError(conn, error);
        Logger.Log("override OnDisconnectError at: " + conn.address + ". error: "  + error);
    }

    public override void OnData(NetworkConnection conn, int receivedSize, int channelId)
    {
        base.OnData(conn, receivedSize, channelId);
        Logger.Log("override OnData from " + conn.address);
    }

    public override void OnDataError(NetworkConnection conn, byte error)
    {
        base.OnDisconnectError(conn, error);
        Logger.Log("override OnDataError at: " + conn.address + ". error: " + error);
    }
}
