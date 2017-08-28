﻿using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Attention : ModuleBase<SocketCommandContext>
    {
        [Command("attention")]
        // TODO: Fix command, add randomizer
        public async Task attention(string position = null)
        {
            Random rnd = new Random();
            int Number = rnd.Next(1, 9);

            string[] Letter = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            int number = Number;

            string letter = Letter[rnd.Next(0, 9)];

            if(position != null)
            {
                for (int i = 1; i <= 9; i++)
                {
                    if (position.Contains(i.ToString()))
                    {
                        number = i;
                        break;
                    }
                }

                for (int i = 0; i <= 9; i++)
                {
                    if (position.Contains(Letter[i]) || position.Contains(Letter[i].ToLower()))
                    {
                        letter = Letter[i];
                        break;
                    }
                }
            }
            
            string[] Text = new string[3] { "Attention to the designated grid square!", "Attention to the designated grid zone!", "Attention to the map!" };

            string text = Text[rnd.Next(0, 2)];

            await Context.Channel.SendMessageAsync(text + " (" + letter + number + ")");
        }

        [Command("exit")]
        [RequireOwner]
        public async Task exitAttentionBot()
        {
            await Context.Channel.SendMessageAsync("Attention! Bot is now offline.");
            Console.WriteLine("Attention! Bot Offline");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
}