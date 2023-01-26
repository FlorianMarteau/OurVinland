using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Assets.METIER
{
    public class Serveur : ICommunication
    {
        // Permet de savoir si des clients peuvent toujours se connecter au serveur
        private bool canConnect = true;

        private TcpListener tcpListener;
        // Liste des sockets à qui le serveur peut parler
        private List<TcpClient> clients = new List<TcpClient>();

        public List<TcpClient> Clients { get => clients; set => clients = value; }
        public bool CanConnect { get => canConnect; set => canConnect = value; }

        public void Connect()
        {
            // Création d'un objet qui va écouter les clients
            tcpListener = new TcpListener(IPAddress.Loopback, 1234);

            // Lancement du listener qui peut accepter jusqu'à 5 connexions
            tcpListener.Start();


            // Lancement d'un thread pour vérifier si des clients se déconnectent
            Thread thread = new Thread(delegate ()
            {
                while (true)
                {
                    try
                    {
                        for (int i = this.clients.Count - 1; i >= 0; i--)
                        {
                            if (this.clients[i].Client == null)
                            {
                                // Client disconnected
                                this.clients.Remove(this.clients[i]);
                            }
                            else if (this.clients[i].Client.Poll(0, SelectMode.SelectRead))
                            {
                                byte[] buff = new byte[1];
                                if (this.clients[i].Client.Receive(buff, SocketFlags.Peek) == 0)
                                {
                                    // Client disconnected
                                    this.clients[i].Client.Close();
                                    this.clients.Remove(this.clients[i]);
                                }
                            }
                        }
                        Thread.Sleep(5);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    
                }
            });
            thread.Start();


            // Vérifie si de nouveaux clients se connectent
            while (canConnect)
            {
                TcpClient clientHandler = tcpListener.AcceptTcpClient();
                this.clients.Add(clientHandler);
                // Si il y a deux joueurs ( A MODIFIER APRES ) alors on arrete les attentes de connexion s
                if (clients.Count == 2)
                {
                    CanConnect = false;
                }
            }
        }

        public void Disconnect()
        {
            tcpListener.Stop();
        }

        public string ReceiveMessage()
        {
            string messageRecu = "";
            byte[] vs = new byte[1024];
            for ( int i = 0; i < clients.Count; i++)
            {
                if (this.clients[i].Client.Poll(0, SelectMode.SelectRead))
                {
                    int dataRec = this.clients[i].GetStream().Read(vs);
                    string mess1 = Encoding.ASCII.GetString(vs, 0, dataRec);
                    messageRecu = messageRecu + $"||Client{i}||{mess1}&&&";
                }
            }
            return messageRecu;
        }

        public void SendMessage(string msg, int destinataire = -1)
        {
            // Si destinataire -1 alors on parle à tout le monde
            if (destinataire == -1)
            {
                for (int i = 0; i < clients.Count; i++)
                {
                    byte[] messageEnByte = Encoding.ASCII.GetBytes($"||Server||{msg}");
                    this.clients[i].GetStream().Write(messageEnByte);
                }
            }
            else if (destinataire < this.clients.Count)
            {
                byte[] messageEnByte = Encoding.ASCII.GetBytes($"||Server||{msg}");
                this.clients[destinataire].GetStream().Write(messageEnByte);
            }
            
        }
    }
}
