﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Collections;
using static Connect_4.Game;

namespace Connect_4
{
    class Server
    {
        TcpListener server;
        //List<Room> avaibleRoom = new List<Room>();
        ArrayList players = new ArrayList();
        // static Game laststate;
        private static readonly object locker = new object();

        int port;
        static int row;
        static int col;
       static Boolean flag = false;
        static Game temp;
        private Server(int PortNum, int numOFClient)
        {
            server = new TcpListener(IPAddress.Parse("127.0.0.1"), PortNum);
            // server2 = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);

            server.Start();
            // server2.Start();
            //avaibleRoom.Add(new Room(new User("ahmed"), "ahmed", "tarek"));
            Console.WriteLine("************This is Server now ************");

            Console.WriteLine("************This is Server program************" + PortNum);
            Console.WriteLine("Hoe many clients are going to connect to this server?:");
            int numberOfClientsYouNeedToConnect = numOFClient;
            for (int i = 0; i < numberOfClientsYouNeedToConnect; i++)
            {
                Thread newThread = new Thread(new ThreadStart(Listeners));
                newThread.Start();
            }

        }

       
    private static Server instance = null;
        public static Server SingletonServer( int port , int  numofClients)
        {
            
           
                lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new Server(port, numofClients);
                        }
                        return instance;
                    }
            
        }
    

    void Listeners()
        {

            Socket socketForClient = server.AcceptSocket();


            if (socketForClient.Connected)
            {
                bool connected = true;
                Console.WriteLine("Client:" + socketForClient.RemoteEndPoint + " now connected to server.");
                NetworkStream networkStream = new NetworkStream(socketForClient);
                System.IO.BinaryWriter streamWriter =
                new System.IO.BinaryWriter(networkStream);
                System.IO.BinaryReader streamReader =
                new System.IO.BinaryReader(networkStream);


                //here we recieve client's text if any.
                while (connected)
                {
                    string theString;

                    try
                    {
                        theString = streamReader.ReadString();
                        Console.WriteLine("value of Received is :" + theString);
                        streamWriter.Flush();
                        Console.WriteLine("helooooo");

                        if (theString == "log")
                        {
                            streamWriter.Write("log");
                            User.players.Add((User.Receiver(streamReader)));


                        }

                        if (theString == "showplayer")
                        {


                            streamWriter.Write("showplayer");

                            streamWriter.Write("start");
                            User.sendAvaibleUsers(streamWriter);
                            streamWriter.Write("end");




                            //    SendServerInfo(streamWriter,streamReader);
                            Thread.Sleep(10);
                            //      new Server(9000, 6);


                        }

                        if (theString == "ConfigPlayer1")
                        {
                            streamWriter.Write("ConfigPlayer1");
                            string id = streamReader.ReadString();
                            Room roomState = Room.avaibleRoom[Room.FindindexOfRoom(id)];
                            roomState.game = Game.Receiver(streamReader);
                            Console.WriteLine("Change Turn");
                            //streamReader.ReadString();
                            roomState.game.playerTurn();
                            while (true)
                            {

                                while (roomState.game.player1 == false) ;
                                Console.WriteLine("Send player 1");

                                Game.SendGame(roomState.game, streamWriter);
                                Console.WriteLine("Send recive  1");
                                roomState.game = Game.Receiver(streamReader);

                                roomState.game.playerTurn();

                            }
                        }
                        if (theString == "ConfigPlayer2")
                        {
                            streamWriter.Write("ConfigPlayer2");

                            string id = streamReader.ReadString();
                            Room roomState = Room.avaibleRoom[Room.FindindexOfRoom(id)];

                            while (true)
                            {
                                while (roomState.game.player2 == false) ;
                                Console.WriteLine("Send  player 2");
                                Game.SendGame(roomState.game, streamWriter);

                                roomState.game = Game.Receiver(streamReader);
                                Console.WriteLine("reciving  player 2");

                                roomState.game.playerTurn();



                            }


                        }


                            if (theString == "showRooms")
                        {




                            streamWriter.Write("showRooms");

                            streamWriter.Write("start");
                            Room.sendAvaibleRooms(streamWriter);
                            streamWriter.Write("end");
                            



                            //    SendServerInfo(streamWriter,streamReader);
                            Thread.Sleep(10);
                            //      new Server(9000, 6);


                        }
                        if (theString == "play")
                        {
                            streamWriter.Write("play");
                            string id = streamReader.ReadString();

                            
                            while (Room.avaibleRoom[Room.FindindexOfRoom(id)].StartGame == false) ;
                            Room.avaibleRoom[Room.FindindexOfRoom(id)].roomIsFull = true;

                            row = Room.avaibleRoom[Room.FindindexOfRoom(id)].game.row;
                            col= Room.avaibleRoom[Room.FindindexOfRoom(id)].game.col;
                           temp = Room.avaibleRoom[Room.FindindexOfRoom(id)].game;
                            Room.DeepSend(Room.avaibleRoom[Room.FindindexOfRoom(id)],streamWriter);
                            //   Room currentRoom = Room.avaibleRoom[Room.FindindexOfRoom(id)];
                            Console.WriteLine("Send  player player1");


                        }
                        if (theString == "create")

                        {
                            Console.WriteLine("Start Create");

                            streamWriter.Write("create");


                            User player1 = JsonConvert.DeserializeObject<User>(streamReader.ReadString());
                            string id = streamReader.ReadString();
                            Room.avaibleRoom.Add(new Room(player1, id, int.Parse(streamReader.ReadString())));
                            Console.Write(" i create a room ");

                            //  Room.FindindexOfRoom(id);

                            Room.SendRoom(Room.avaibleRoom[Room.FindindexOfRoom(id)], streamWriter);
                            /*
                             ****************lazem t3ml uncomment************
                            while (Room.avaibleRoom[Room.FindindexOfRoom(id)].StartGame == false) ;

                         //   Room currentRoom = Room.avaibleRoom[Room.FindindexOfRoom(id)];
                            Console.WriteLine("Send  player player1");

                            //  string strJson = JsonConvert.SerializeObject(Room.avaibleRoom[Room.FindindexOfRoom(id)]);
                            Room.avaibleRoom[Room.FindindexOfRoom(id)].game = Game.Receiver(streamReader);
                            Console.WriteLine("Change Turn");
                            //streamReader.ReadString();
                            Room.avaibleRoom[Room.FindindexOfRoom(id)].game.playerTurn();
                            while(true)
                            {
                               
                                while (Room.avaibleRoom[Room.FindindexOfRoom(id)].game.player1 == false) ;
                                Console.WriteLine("Send player 1");

                                Game.SendGame(Room.avaibleRoom[Room.FindindexOfRoom(id)].game, streamWriter);
                                Console.WriteLine("Send recive  1");
                                Room.avaibleRoom[Room.FindindexOfRoom(id)].game = Game.Receiver(streamReader);

                                Room.avaibleRoom[Room.FindindexOfRoom(id)].game.playerTurn();


                            }
                                  ****************lazem t3ml uncomment************
                            */



                        }





                        if (theString == "join")
                        {
                            streamWriter.Write("join");
                            Console.WriteLine("Start Join");

                            Console.WriteLine("AnaBreceive");
                            User player2 = JsonConvert.DeserializeObject<User>(streamReader.ReadString());
                            string roomId = streamReader.ReadString();
                            Room.addSecoondPlayTORoom(roomId, streamWriter, player2);

                            

                            Console.WriteLine("ok I did it ");
                            Console.WriteLine("End Join");
                            /*
                          //  Room current=Room.avaibleRoom[Room.FindindexOfRoom(roomId)];
                            while (true)
                            {
                                while (Room.avaibleRoom[Room.FindindexOfRoom(roomId)].game.player2 == false) ;
                                Console.WriteLine("Send  player 2");
                                Game.SendGame(Room.avaibleRoom[Room.FindindexOfRoom(roomId)].game, streamWriter);

                                Room.avaibleRoom[Room.FindindexOfRoom(roomId)].game = Game.Receiver(streamReader);
                                Console.WriteLine("reciving  player 2");
                                Room.avaibleRoom[Room.FindindexOfRoom(roomId)].game.playerTurn();



                            }
                            */
                        }
                            
                        Console.WriteLine("Message recieved by client:" + theString);
                        if (theString == "exit")
                        {
                            connected = false;
                        }

                    }
                    catch (System.IO.IOException ex)
                    {
                        connected = false;
                    }

                }
                streamReader.Close();
                networkStream.Close();
                streamWriter.Close();


            }
            socketForClient.Close();
            Console.WriteLine("Press any key to exit from server program");
            Console.ReadKey();
        }


          }
}
