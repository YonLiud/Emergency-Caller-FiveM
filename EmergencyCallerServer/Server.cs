using System;
using System.Collections.Generic;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace EmergencyCallerServer
{
    public class Server : BaseScript
    {
        public Server()
        {
            EventHandlers["sent911"] += new Action<string, string, Vector3>(OnCall);
        }

        public void OnCall(string playerName, string args, Vector3 location)
        {
            Debug.WriteLine("Sender: " + playerName + " | Text:  "+ args);
            TriggerClientEvent("recieve911", playerName, args, location);
        }

    }
}