using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.METIER
{
    public class TestNetwork : MonoBehaviour
    {
        public Text textServer;
        public Text textClient;
        public InputField inputClient;
        private Serveur serv ;
        private Client c;
        public void FixedUpdate()
        {
            if (serv != null && c != null) 
            {
                string t = "";
                if ((t = serv.ReceiveMessage()) != "")
                {
                    textServer.text = textServer.text + "\n" + t;
                }
            }
            
        }


        public void SendMessage()
        {
            c.SendMessage(inputClient.text);
        }

        public void OpenServer()
        {
            // Création d'un thread dans lequel le serveur va attendre les connexions
            serv = new Serveur();
            Thread thread = new Thread(delegate ()
            {
                serv.Connect();
            });
            thread.Start();
        }

        public void ConnectClient()
        {
            // Création de deux clients se connectant au serveur
            c = new Client();
            c.Connect();
            
        }

    }
}
