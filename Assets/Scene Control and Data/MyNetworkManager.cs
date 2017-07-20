using UnityEngine.Networking;

namespace NetworkScene
{
    public class MyNetworkManager : NetworkManager
    {
        public override void OnStartClient(NetworkClient client)
        {
            client.RegisterHandler(MsgType.Scene, OnSceneMessage);
        }


        void OnSceneMessage(NetworkMessage msg)
        {
            Logger.Log("Scene Message");
        }
    }
}