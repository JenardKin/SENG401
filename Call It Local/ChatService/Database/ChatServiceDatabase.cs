﻿using Messages;
using Messages.Database;
using Messages.DataTypes.Database.Chat;
using Messages.NServiceBus.Commands;
using Messages.ServiceBusRequest;
using Messages.ServiceBusRequest.Chat.Responses;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace ChatService.Database
{
    //The following follows the same design as EchoService/Database/EchoServiceDatabase.cs
    //Code and comments recycled from that cs file.

    /// <summary>
    /// This portion of the class contains methods and functions
    /// </summary>
    public partial class ChatServiceDatabase : AbstractDatabase
    {
        /// <summary>
        /// Private default constructor to enforce the use of the singleton design pattern
        /// </summary>
        private ChatServiceDatabase() { }

        /// <summary>
        /// Gets the singleton instance of the database
        /// </summary>
        /// <returns>The singleton instance of the database</returns>
        public static ChatServiceDatabase getInstance()
        {
            if (instance == null)
            {
                instance = new ChatServiceDatabase();
            }
            return instance;
        }

        public GetChatHistoryResponse getChatHistory(ChatHistory history)
        {
            bool result = false;
            GetChatHistory historyResponse = new GetChatHistory();
            string responseString = "";
            if (openConnection() == true)
            {
                string query = @"SELECT * FROM chathistory WHERE (sender='" + history.user1 + @"' AND receiver='" + history.user2 + @"') OR " +
                               @"(sender='" + history.user2 + @"' AND receiver='" + history.user2 + @"')";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                List<ChatMessage> messages = new List<ChatMessage>();
                if (reader.Read())
                {
                    result = true;
                    ChatMessage message = new ChatMessage();
                    message.sender = reader.GetString("sender");
                    message.receiver = reader.GetString("receiver");
                    message.unix_timestamp = reader.GetInt32("timestamp");
                    message.messageContents = reader.GetString("message");
                    messages.Add(message);
                    while (reader.Read())
                    {
                        message.sender = reader.GetString("sender");
                        message.receiver = reader.GetString("receiver");
                        message.unix_timestamp = reader.GetInt32("timestamp");
                        message.messageContents = reader.GetString("message");
                        messages.Add(message);
                    }
                    history.messages = messages;
                    historyResponse.history = history;
                }
                else
                {
                    responseString = "No chat history found between '" + history.user1 + "' and '" + history.user2;
                }
                reader.Close();
                closeConnection();
            }
            else
            {
                responseString = "Unable to connect to database";
                Debug.consoleMsg("Unable to connect to database");
            }
            return new GetChatHistoryResponse(result, responseString, historyResponse);
        }

        public GetChatContactsResponse getChatContacts(string username)
        {
            bool result = false;
            GetChatContacts contactResponse = new GetChatContacts();
            string responseString = "";
            if (openConnection() == true)
            {
                string query = @"SELECT DISTINCT receiver FROM chathistory WHERE sender='" + username + @"'";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                List<string> contacts = new List<string>();
                if (reader.Read())
                {
                    result = true;
                    contacts.Add(reader.GetString("receiver"));
                    while (reader.Read())
                    {
                        contacts.Add(reader.GetString("receiver"));
                    }
                    reader.Close();
                    query = @"SELECT DISTINCT sender FROM chathistory WHERE receiver='" + username + @"'";
                    command = new MySqlCommand(query, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string contact = reader.GetString("sender");
                        if (!contacts.Contains(contact))
                        {
                            contacts.Add(contact);
                        }
                    }
                    contactResponse.contactNames = contacts;
                }
                else
                {
                    responseString = "No contacts for '" + username;
                }
                reader.Close();
                closeConnection();
            }
            else
            {
                responseString = "Unable to connect to database";
                Debug.consoleMsg("Unable to connect to database");
            }
            return new GetChatContactsResponse(result, responseString, contactResponse);
        }

        public ServiceBusResponse saveMessage(ChatMessage message)
        {
            bool result = false;
            string responseString = "";
            if(openConnection() == true)
            {
                string query = @"INSERT INTO chathistory(sender, receiver, timestamp, message) " +
                               @"VALUES('" + message.sender + @"', '" + message.receiver + @"', " + message.unix_timestamp +
                               @", '" + message.messageContents + @"')";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                result = true;
                
                closeConnection();
            }
            else
            {
                responseString = "unable to connect to database";
                Debug.consoleMsg("unable to connect to database");
            }
            return new ServiceBusResponse(result, responseString);
        }
    }

    /// <summary>
    /// This portion of the class contains the properties and variables 
    /// </summary>
    public partial class ChatServiceDatabase : AbstractDatabase
    {
        /// <summary>
        /// The name of the database.
        /// Both of these properties are required in order for both the base class and the
        /// table definitions below to have access to the variable.
        /// </summary>
        private const String dbname = "chatservicedb";
        public override string databaseName { get; } = dbname;

        /// <summary>
        /// The singleton isntance of the database
        /// </summary>
        protected static ChatServiceDatabase instance = null;

        /// <summary>
        /// This property represents the database schema, and will be used by the base class
        /// to create and delete the database.
        /// </summary>
        protected override Table[] tables { get; } =
        {
            new Table
                (
                    dbname,
                    "chathistory",
                    new Column[]
                    {
                        new Column
                        (
                            "sender", "VARCHAR(50)",
                            new string[]
                            {
                                "NOT NULL"
                            }, false
                        ),
                        new Column
                        (
                            "receiver", "VARCHAR(50)",
                            new string[]
                            {
                                "NOT NULL"
                            }, false
                        ),
                        new Column
                        (
                            "timestamp", "INT(32)",
                            new string[]
                            {
                                "NOT NULL"
                            }, false
                        ),
                        new Column
                        (
                            "message", "VARCHAR(1000)",
                            new string[]
                            {
                                "NOT NULL",
                            }, false
                        ),
                    }
                )
        };
    }
}
