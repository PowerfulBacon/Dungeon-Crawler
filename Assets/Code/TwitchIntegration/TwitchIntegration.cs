using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;


public struct TwitchMessage
{
    public string username;
    public string message;
}


public class TwitchIntegration
{

    TcpClient twitchClient;
    StreamReader streamReader;
    StreamWriter streamWriter;

    public string username = "powerfulbacon";
    public string password = "oauth:mezzux2culunffbvzsmzn77dhnnzn0";
    public string channelName = "powerfulbacon";

    public void UpdateTwitchIntegration()
    {
        if (twitchClient == null || !twitchClient.Connected)
            Connect();
    }


    public void Connect()
    {
        twitchClient = new TcpClient("irc.chat.twitch.tv", 6667);
        streamReader = new StreamReader(twitchClient.GetStream());
        streamWriter = new StreamWriter(twitchClient.GetStream());

        streamWriter.WriteLine("PASS " + password);
        streamWriter.WriteLine("NICK " + username);
        streamWriter.WriteLine("USER " + username + " 8 * :" + username);
        streamWriter.WriteLine("JOIN #" + channelName);
        streamWriter.Flush();
    }

    public List<TwitchMessage> GetMessages()
    {
        UpdateTwitchIntegration();

        List<TwitchMessage> twitchMessages = new List<TwitchMessage>();

        if (twitchClient.Available > 0)
        {
            var message = streamReader.ReadLine();

            if (message.Contains("PRIVMSG"))
            {
                string username = message.Substring(0, message.IndexOf("!", 1));
                string chatMessage = message.Substring(message.IndexOf(":", 1) + 1);

                TwitchMessage twitchMessage = new TwitchMessage();
                twitchMessage.message = chatMessage;
                twitchMessage.username = username;

                twitchMessages.Add(twitchMessage);

            }
        }

        return twitchMessages;

    }
    
}
