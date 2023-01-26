using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Assets.METIER
{
    public class Client : ICommunication
    {
        #region attributes
        // Bytes pour stocker les messages
        private byte[] myBytes = new byte[1024];

        // Flux in et out du canal de discussion du client
        private NetworkStream flux;

        // TcpClient du client
        private TcpClient tcpClient;

        #endregion


        public void SendMessage(string msg, int destinataire = -1)
        {
            byte[] messageEnByte = Encoding.ASCII.GetBytes($"{msg}");
            flux.Write(messageEnByte);
        }

        public void Connect()
        {
            // Création du tcp client et connection au serveur
            this.tcpClient = new TcpClient(IPAddress.Loopback.ToString(), 1234);
          
            
            // Récupération du flux du canal du client
            this.flux = tcpClient.GetStream();
        }

        public string ReceiveMessage()
        {
            string message = "";
            if (this.tcpClient.Client.Poll(0, SelectMode.SelectRead))
            {
                int dataRec = flux.Read(myBytes);
                message = Encoding.ASCII.GetString(myBytes, 0, dataRec);
            }

            return message;
        }

        public void Disconnect()
        {
            flux.Close();
            tcpClient.Close();
        }
    }
}
