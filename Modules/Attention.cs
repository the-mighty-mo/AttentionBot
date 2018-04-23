﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AttentionBot.Modules
{
    public class Attention : ModuleBase<SocketCommandContext>
    {
        [Command("attention")]
        public async Task attention(string position = null, string _mentionID = null)
        {
            Random rnd = new Random();

            string[] Letter = new string[10] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

            int number = rnd.Next(1, 11);

            string letter = Letter[rnd.Next(0, 10)];

            if (position != null)
            {
                for (int i = 10; i >= 1; i--)
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

            string text = Text[rnd.Next(0, 3)];

            var _myUser = Context.Guild.GetUser(Convert.ToUInt64(_mentionID));

            foreach(ulong serv in Program.mentionID)
            {
                if(!Context.Client.GetGuild(serv).IsConnected)
                {
                    Program.mentionID.Remove(serv);

                    BinaryWriter mentionWriter = new BinaryWriter(File.Open("mentions.txt", FileMode.Truncate));
                    foreach (var value in Program.mentionID)
                    {
                        mentionWriter.Write(value.ToString());
                    }
                    mentionWriter.Close();
                }
            }

            if (Program.mentionID.Contains(Context.Guild.Id) && _mentionID != null)
            {
                await Context.Channel.SendMessageAsync(_myUser.Mention + " " + text + " (" + letter + number + ")");
            }
            else
            {
                await Context.Channel.SendMessageAsync(text + " (" + letter + number + ")");
            }
        }

        [Command("usercount")]
        public async Task userCount()
        {
            int total = Context.Guild.MemberCount;
            int totalBots = 0, onlineBots = 0, offlineBots = 0;
            int totalUsers = 0, onlineUsers = 0, awayUsers = 0, doNotDisturbUsers = 0, invisibleUsers = 0, offlineUsers = 0;

            foreach (SocketGuildUser user in Context.Guild.Users)
            {
                if(!user.IsBot)
                {
                    totalUsers++;

                    switch (user.Status)
                    {
                        case UserStatus.AFK:
                        case UserStatus.Idle:
                            awayUsers++;
                            break;
                        case UserStatus.DoNotDisturb:
                            doNotDisturbUsers++;
                            break;
                        case UserStatus.Invisible:
                            invisibleUsers++;
                            break;
                        case UserStatus.Online:
                            onlineUsers++;
                            break;
                        case UserStatus.Offline:
                        default:
                            offlineUsers++;
                            break;
                    }
                }
                else
                {
                    totalBots++;

                    switch (user.Status)
                    {
                        case UserStatus.AFK:
                        case UserStatus.Idle:
                        case UserStatus.DoNotDisturb:
                        case UserStatus.Invisible:
                        case UserStatus.Online:
                            onlineBots++;
                            break;
                        case UserStatus.Offline:
                        default:
                            offlineBots++;
                            break;
                    }
                }
            }

            var onlineMessage = new EmbedBuilder();
            onlineMessage.WithColor(23, 90, 150);
            onlineMessage.WithTitle("__User Count__");
            onlineMessage.WithCurrentTimestamp();

            EmbedFieldBuilder totalBuilder = new EmbedFieldBuilder();
            totalBuilder.WithIsInline(false);
            totalBuilder.WithName("Total");
            totalBuilder.WithValue(total);
            onlineMessage.AddField(totalBuilder);

            EmbedFieldBuilder userBuilder = new EmbedFieldBuilder();
            userBuilder.WithIsInline(true);
            userBuilder.WithName("\nUsers: " + totalUsers);
            userBuilder.WithValue(
                "\nOnline: " + onlineUsers +
                "\nAway: " + awayUsers +
                "\nDo Not Disturb: " + doNotDisturbUsers +
                "\nInvisible: " + invisibleUsers +
                "\nOffline: " + offlineUsers + "\n");
            onlineMessage.AddField(userBuilder);

            EmbedFieldBuilder botBuilder = new EmbedFieldBuilder();
            botBuilder.WithIsInline(true);
            botBuilder.WithName("\nBots: " + totalBots);
            botBuilder.WithValue(
                "\nOnline: " + onlineBots +
                "\nOffline: " + offlineBots);
            onlineMessage.AddField(botBuilder);

            await Context.Channel.SendMessageAsync("", false, onlineMessage);
        }

        [Command("changelog")]
        public async Task changelog(string _botID = null)
        {
            if (_botID == Program.botID)
            {
                await Context.Channel.SendMessageAsync("Changelog can be found at:\n" +
                    "https://github.com/josedolf-staller/AttentionBot#release-notes");
            }
        }

        [Command("help")]
        public async Task help(string _botID = null)
        {
            if (_botID == Program.botID)
            {
                await Context.Channel.SendMessageAsync("**Attention! Bot v1.5.4.1  -  Programmed using Discord.Net**\n" +
                    "__Prefix:__ \\\n\n" +
                    "__Commands:__\n\n" +
                    "\\help 3949\n" +
                    "  - Lists all available commands for the bot.\n\n" +
                    "\\changelog 3949\n" +
                    "  - Sends a link to the version history (changelog) of the bot.\n\n" +
                    "\\admin [role id]\n" +
                    "  - **SERVER OWNERS:** Sets/removes the specified role as an administrative role for the bot's admin commands.\n\n" +
                    "\\mentions [0/1]\n" +
                    "  - **ADMINS/SERVER OWNERS:** Enables (1) or disables (0) user mentions for the bot.\n\n" +
                    "\\announce [channel id]\n" +
                    "  - **ADMINS/SERVER OWNERS:** Sets the specified channel as the channel for bot announcements.\n\n" +
                    "\\attention [position (optional)] [user ID (optional)]\n" +
                    "  - Position can contain one letter A-J and/or one number 1-10. Order and capitalization do not matter.\n" +
                    "  - User ID only works if \\mentions is set to 1. Set the User ID to the ID of the user you want to mention.\n\n" +
                    "\\usercount\n" +
                    "  - Lists number of users and bots on the server by status.");
            }
        }
    }
}
