using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using TMPro;

namespace Dungeon_Crawler.Assets.Code.UserInterface.Chat
{

    public class Chat : MonoBehaviour
    {

        // The messages in chat
        // (Make private in final build)
        public List<ChatMessage> chatMessages = new List<ChatMessage>();

        private TextMeshProUGUI textMesh;

        private static Chat singleton;

        private void Start()
        {
            singleton = this;
            textMesh = GetComponent<TextMeshProUGUI>();
            if(textMesh == null)
            {
                Log.Print("Error; Couldn't get textmeshpro on UI object. This should NEVER happen.");
            }
        }

        //Send a message to chat
        void UpdateChat(string message)
        {

            //Remove old messages
            if (chatMessages.Count > 10)
            {
                chatMessages.RemoveAt(0);
            }

            bool repeatedMessage = false;

            ChatMessage newMessage = null;

            //Check for same messages
            foreach(ChatMessage chatMessage in chatMessages)
            {
                if(chatMessage.message == message)
                {
                    newMessage = chatMessage;
                    newMessage.count ++;
                    chatMessages.Remove(chatMessage);
                    repeatedMessage = true;
                    break;
                }
            }

            if(!repeatedMessage)
            {
                //Add new messages
                newMessage = new ChatMessage();
                newMessage.message = message;
                newMessage.count = 1;
            }

            chatMessages.Add(newMessage);

            //Update the chat
            textMesh.text = "";
            for (int i = 0; i < chatMessages.Count; i++)
            {
                ChatMessage chatMessage = chatMessages[i];
                //Message itself
                textMesh.text += chatMessage.message;
                //Duplicate message counter
                if(chatMessage.count > 1)
                {
                    textMesh.text += $" (x{chatMessage.count})";
                }
                //New line
                if(i != 0)
                {
                    textMesh.text += "\n";
                }
            }

        }

        //Static message adder

        private static List<string> messageCache = new List<string>();

        public static void ToChat(string message)
        {
            //Check if we are instantiated
            if(singleton != null)
            {
                if(messageCache != null)
                {
                    //Add cached message
                    foreach (string cachedMessage in messageCache)
                    {
                        singleton.UpdateChat(cachedMessage);
                    }
                    messageCache = null;
                }
                //Add message
                singleton.UpdateChat(message);
            }
            else
            {
                messageCache.Add(message);
            }
        }

    }

}