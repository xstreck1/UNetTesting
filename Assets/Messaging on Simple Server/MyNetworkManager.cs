using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

namespace Messaging
{
    public class ClientPing : MessageBase
    {

    }

    public class ServerPing : MessageBase
    {

    }

    public class MyNetworkManager : MonoBehaviour
    {
        public Text maxConnectionsField;
        private int MaxConnections { get { return maxConnectionsField.text == "" ? 8 : Int32.Parse(maxConnectionsField.text); } }
        public Toggle transportLayerToggle;
        private bool UseTransportLayer { get { return transportLayerToggle.isOn; } }
        public Image serverIndicator;
        public Image clientIndicator;

        public const string serverIP = "127.0.0.1";
        public const int serverPort = 4444;
        public string WholeAddress { get { return serverIP + ":" + serverPort; } }
        public const short clientPingId = 888;
        public const short serverPingId = 889;
        public const short clientWriterId = 890;
        public const short serverWriterId = 890;
        private int reliabelChannelID = -1;
        private int unreliableChannelID = -1;

        MySimpleServer myServer;
        NetworkClient myClient;

        public void Update()
        {
            if (myServer != null)
            {
                if (UseTransportLayer)
                {
                    myServer.ReceiveMessage();
                }
                else
                {
                    myServer.Update();
                }
            }
        }

        private ConnectionConfig MakeConfig()
        {

            ConnectionConfig myConfig = new ConnectionConfig();
            reliabelChannelID = myConfig.AddChannel(QosType.Reliable);
            unreliableChannelID = myConfig.AddChannel(QosType.Unreliable);
            return myConfig;
        }

        // Create a server and listen on a port
        public void SetupServer()
        {
            if (myServer != null)
            {
                KillServer();
            }

            myServer = new MySimpleServer();
            myServer.Configure(MakeConfig(), MaxConnections);
            bool listening = myServer.Listen(serverIP, serverPort);

            if (listening)
            {
                Logger.Log("Listening on " + WholeAddress);
                myServer.RegisterHandler(clientPingId, OnClientPingReceived);
                myServer.RegisterHandler(clientWriterId, OnClientWriterReceived);
                myServer.RegisterHandler(MsgType.Connect, OnServerConnected);
                myServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnected);
                myServer.RegisterHandler(MsgType.Error, OnServerError);
                myServer.RegisterHandler(MsgType.Ready, OnServerReady);

                serverIndicator.color = Color.green;
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
                serverIndicator.color = Color.red;
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
                myClient.Configure(MakeConfig(), 1);
                myClient.Connect(serverIP, serverPort);


                myClient.RegisterHandler(serverPingId, OnServerPingReceived);
                myClient.RegisterHandler(MsgType.Connect, OnClientConnected);
                myClient.RegisterHandler(MsgType.Disconnect, OnClientDisconnected);
                myClient.RegisterHandler(MsgType.Error, OnClientError);

                Logger.Log("Connecting to " + WholeAddress);

                clientIndicator.color = new Color(255, 127, 80);
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

        public void SendServerPing()
        {
            if (myServer != null && !UseTransportLayer)
            {
                ServerPing msg = new ServerPing();
                foreach (NetworkConnection conn in myServer.connections)
                {
                    if (conn != null)
                    {
                        conn.Send(serverPingId, msg);
                    }
                }
            }
        }

        public void SendClientReady()
        {
            if (myClient != null && !UseTransportLayer)
            {
                ReadyMessage msg = new ReadyMessage();
                myClient.Send(MsgType.Ready, msg);
                clientIndicator.color = Color.green;
            }
        }

        public void SendNetworkWriterFromClient()
        {
            if (myClient != null && !UseTransportLayer)
            {
                NetworkWriter writer = new NetworkWriter();
                writer.StartMessage(clientWriterId);
                writer.Write("Client Writer Message");
                writer.FinishMessage();
                myClient.SendWriter(writer, reliabelChannelID);
            }
        }

        public void SendByteMessageFromClient()
        {
            if (myClient != null && UseTransportLayer)
            {
                const int bufferSize = 1024;
                byte[] buffer = new byte[bufferSize];
                Stream stream = new MemoryStream(buffer);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, "ByteMessage");
                myClient.SendBytes(buffer, bufferSize, reliabelChannelID);
            }
        }

        public void SendSocketMessageFromClient()
        {
            if (myClient != null && UseTransportLayer)
            {
                const int bufferSize = 1024;
                byte error;
                byte[] buffer = new byte[bufferSize];
                Stream stream = new MemoryStream(buffer);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, "SocketMessage");

                NetworkTransport.Send(myClient.connection.hostId, myClient.connection.connectionId, reliabelChannelID, buffer, bufferSize, out error);
            }
        }

        public void OnClientPingReceived(NetworkMessage msg)
        {
            Logger.Log("Ping msg received from client.");
        }

        public void OnClientWriterReceived(NetworkMessage msg)
        {
            Logger.Log("Received writer with the message: " + msg.reader.ReadString());
        }

        public void OnServerPingReceived(NetworkMessage msg)
        {
            Logger.Log("Ping msg received from server.");
        }

        public void OnClientConnected(NetworkMessage netMsg)
        {
            clientIndicator.color = Color.yellow;
            Logger.Log("Connected to server at: " + netMsg.conn.address + " with RTT: " + myClient.GetRTT());
        }

        public void OnClientDisconnected(NetworkMessage netMsg)
        {
            clientIndicator.color = Color.red;
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

        public void OnServerReady(NetworkMessage netMsg)
        {
            int readyCount = 0;
            netMsg.conn.isReady = true;
            foreach (NetworkConnection connIt in myServer.connections)
            {
                if (connIt != null && connIt.isReady)
                {
                    readyCount++;
                }
            }

            Logger.Log("Connection ready on: " + netMsg.conn.address + ". Totally ready: " + readyCount);
        }
    }
}