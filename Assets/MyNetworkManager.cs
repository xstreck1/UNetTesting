using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class ClientPing : MessageBase
{
}

public class ServerPing : MessageBase
{
}

public class MyNetworkManager : MonoBehaviour
{
    public const string serverIP = "127.0.0.1";
    public const int serverPort = 444;
    public string WholeAddress { get { return serverIP + ":" + serverPort; } }
    public const short clientPingId = 888;
    public const short serverPingId = 889;

    MySimpleServer myServer;
    NetworkClient myClient;

    public void Update()
    {
        if (myServer != null)
        {
            myServer.Update();
        }
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
        if (myServer != null)
        {
            KillServer();
        }

        myServer = new MySimpleServer();

        bool listening = myServer.Listen(serverIP, serverPort);


        if (listening)
        {
            Logger.Log("Listening on " + WholeAddress);
            myServer.RegisterHandler(clientPingId, OnServerPingReceived);
            myServer.RegisterHandler(MsgType.Connect, OnServerConnected);
            myServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnected);
            myServer.RegisterHandler(MsgType.Error, OnServerError);
        }
        else
        {
            Logger.Log("Failed to listen on " + WholeAddress);
        }
    }

    public void KillServer()
    {
        if (myServer != null)
        {
            myServer.Stop();
            myServer = null;
            Logger.Log("Killed the server.");
        }
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        if (myClient != null)
        {
            // Does not reset handlers
            myClient.ReconnectToNewHost(serverIP, serverPort);
            Logger.Log("Reconnecting to " + WholeAddress);
        }
        else
        {
            myClient = new NetworkClient();
            myClient.Connect(serverIP, serverPort);


            myClient.RegisterHandler(MsgType.Connect, OnClientConnected);
            myClient.RegisterHandler(MsgType.Disconnect, OnClientDisconnected);
            myClient.RegisterHandler(MsgType.Error, OnClientError);

            Logger.Log("Connecting to " + WholeAddress);
        }
    }
    
    public void KillClient()
    {
        if (myClient != null)
        {
            myClient.Shutdown();
            myClient = null;
            Logger.Log("Killed the client.");
        }
    }
    
    public void SendClientPing()
    {
        if (myClient != null)
        {
            ClientPing msg = new ClientPing();
            myClient.Send(clientPingId, msg);
        }
    }   

    public void OnServerPingReceived(NetworkMessage msg)
    {
        Logger.Log("Ping msg received from client.");
    }

    public void OnClientConnected(NetworkMessage netMsg)
    {
        Logger.Log("Connected to server at: " + netMsg.conn.address);
    }

    public void OnClientDisconnected(NetworkMessage netMsg)
    {
        Logger.Log("OnDisconnected from server at: " + netMsg.conn.address + ". Last error: " + netMsg.conn.lastError);
    }

    public void OnClientError(NetworkMessage netMsg)
    {
        Logger.Log("Client error: " + netMsg.ReadMessage<ErrorMessage>());
    }

    public void OnServerConnected(NetworkMessage netMsg)
    {
        Logger.Log("New connection from: " + netMsg.conn.address);
    }

    public void OnServerDisconnected(NetworkMessage netMsg)
    {
        Logger.Log("Client disconnected at: " + netMsg.conn.address + ". Last error: " + netMsg.conn.lastError);
    }

    public void OnServerError(NetworkMessage netMsg)
    {
        Logger.Log("Server error: " + netMsg.ReadMessage<ErrorMessage>());
    }
}