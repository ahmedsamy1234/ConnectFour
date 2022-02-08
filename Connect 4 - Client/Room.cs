﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect_4___Client
{
    public class Room
    {
        public string id;
        public User Player1;
        public User Player2;
        public User[] Watchers;
        public Board board;
        public bool StartGame = false;
        public Game game;

        public Room(User owner, string Id, string size)
        {
            Player1 = owner;
            this.id = Id;
            board = new Board(size);
        }

        public void PlayBtn(User player)
        {
            Player2 = player;
            StartGame = true;
            game = new Game();
        }


        public static Room ReceiveRoom(String s)
        {

            return JsonConvert.DeserializeObject<Room>(s);
        }
        public static Room ReceiveRoom(BinaryReader receiver)
        {

            return JsonConvert.DeserializeObject<Room>(receiver.ReadString());
        }
    }
}
