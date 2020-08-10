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
            EventHandlers["incoming911"] += new Action<string>(OnCall);
        }
        public void OnResourceStart(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName) return;
        }

        public void OnCall(string name)
        {
            string message = (name + " has dialed 911!");
            TriggerClientEvent("outcoming911", message);
        }

    }
}