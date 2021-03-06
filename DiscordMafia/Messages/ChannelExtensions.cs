﻿using Discord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordMafia.Messages
{
    static class ChannelExtensions
    {
        private const int MaxLength = 1800;

        public static IMessage[] SplitAndSend(this IMessageChannel channel, string text, bool tts = false)
        {
            var result = new List<IMessage>();
            if (text.Length <= MaxLength)
            {
                try
                {
                    result.Add(channel.SendMessageAsync(text, tts).Result);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Cannot deliver message to channel {channel.Name}: {ex.Message}");
                }
            }
            else
            {
                var textParts = text.Split(new string[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries);
                var totalLength = 0;
                var message = "";
                foreach (var textPart in textParts)
                {
                    if (textPart.Length + totalLength > MaxLength)
                    {
                        try
                        {
                            result.Add(channel.SendMessageAsync(message, tts).Result);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Cannot deliver message to channel {channel.Name}: {ex.Message}");
                        }
                        totalLength = 0;
                        message = "";
                    }
                    message += textPart + Environment.NewLine;
                    totalLength += textPart.Length;
                }
                if (message != "")
                {
                    try
                    {
                        result.Add(channel.SendMessageAsync(message, tts).Result);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Cannot deliver message to channel {channel.Name}: {ex.Message}");
                    }
                }
            }
            return result.ToArray();
        }
    }
}