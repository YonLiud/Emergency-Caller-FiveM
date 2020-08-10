using System;
using System.Collections.Generic;
using CitizenFX.Core;
using EmergencyCallerServer;
using static CitizenFX.Core.Native.API;
using srv = EmergencyCallerServer.Server;


namespace EmergencyCallerClient
{
    public class Client : BaseScript
    {
        public Client()
        {
            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["outcoming911"] += new Action<string>(OnRecieve911);
        }

        private void OnClientResourceStart(string resourceName)
        {

            Debug.WriteLine("Emergency Caller has Booted up");
            if (GetCurrentResourceName() != resourceName) return;

            RegisterCommand("911", new Action<int, List<object>, string>((source, args, raw) =>
            {
                string name = Game.Player.Name;
                Vector3 Location = Game.PlayerPed.Position;
                TriggerServerEvent("incoming911", Game.Player.Name) ;
            }), false);
        }
        private void OnRecieve911(string message)
        {
            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 0, 204, 204 },
                multiline = true,
                args = new[] { "911", message }
            });
        }
    }
}