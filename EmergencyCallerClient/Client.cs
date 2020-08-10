using System;
using System.Collections.Generic;
using System.Drawing;
using CitizenFX.Core;
using EmergencyCallerServer;
using static CitizenFX.Core.Native.API;
using srv = EmergencyCallerServer.Server;


namespace EmergencyCallerClient
{
    public class Client : BaseScript
    {
        Player player;
        bool onCooldown = false;
        public Client()
        {

            EventHandlers["onClientResourceStart"] += new Action<string>(OnClientResourceStart);
            EventHandlers["recieve911"] += new Action<string, string, Vector3>(OnRecieve911);
        }

        private void OnClientResourceStart(string resourceName)
        {
            player = Game.Player;
            Debug.WriteLine("Emergency Caller has Booted up");
            if (GetCurrentResourceName() != resourceName) return;

            RegisterCommand("911", new Action<int, List<object>, string>((source, args, raw) =>
            {
                if (onCooldown)
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        multiline = true,
                        args = new[] { "Oops", $"You are calling too fast, Slow Down!" }
                    });
                    return;
                }


                if (args.Count < 1)
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        multiline = true,
                        args = new[] { "Oops", $"Did you mean to type your call too?" }
                    });
                    return;
                }
                if (args.Count < 1)
                {
                    TriggerEvent("chat:addMessage", new
                    {
                        color = new[] { 255, 0, 0 },
                        multiline = true,
                        args = new[] { "Oops", $"Did you mean to type your call too?" }
                    });
                    return;
                }
                string text = "";
                text = string.Join(" ", args.ToArray());
                Debug.WriteLine(text);
                string playername = Game.Player.Name;
                TriggerServerEvent("sent911", playername, text, Game.PlayerPed.Position);
                cooldown();
            }), false);
        }
        private void OnRecieve911(string name, string args, Vector3 location)
        {
            string street = World.GetStreetName(location);
            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 0, 204, 204 },
                multiline = true,
                args = new[] { "911", $"Caller: ^*{name} | Location: {street} | ^*Transcript:  {args}" }
            });
            Blip blip = World.CreateBlip(location);

            blip.Sprite = BlipSprite.ArmoredTruck;
            blip.Color = BlipColor.Blue;
            blip.Name = $"{name}'s Emergency Call";
            removeBlip(blip);
        }
        private async void removeBlip(Blip blip)
        {
            await Delay(60000);
            blip.Delete();
        }
        private async void cooldown()
        {
            onCooldown = true;
            Debug.WriteLine("Cooldown Set");
            await Delay(15000);
            onCooldown = false;
            Debug.WriteLine("Your Cooldown has expired");

        }

    }
}