using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.METIER;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestServeurClient
{
    /// <summary>
    /// Test pour voir si des clients peuvent se connecter au serveur
    /// </summary>
    [Test]
    public void TestConnectionServeurClient()
    {

        // Cr�ation d'un thread dans lequel le serveur va attendre les connexions
        Serveur serveur = new Serveur();
        Thread thread = new Thread(delegate ()
        {
            serveur.Connect();
        });
        thread.Start();

        Thread.Sleep(2000);

        // Cr�ation de deux clients se connectant au serveur
        Client c = new Client();
        c.Connect();

        Client c2 = new Client();
        c2.Connect();
 


        // V�rification du nombre de clients connect�s
        Assert.AreEqual(2, serveur.Clients.Count);

        // Arret des points de communications
        c.Disconnect();
        c2.Disconnect();
        serveur.Disconnect();

    }

    /// <summary>
    /// Test pour voir si la deconnexion des clients est g�r�e
    /// </summary>
    [Test]
    public void TestDeconnectionServeurClient()
    {
        // Cr�ation d'un thread dans lequel le serveur va attendre les connexions
        Serveur serveur = new Serveur();
        Thread thread = new Thread(delegate ()
        {
            serveur.Connect();
        });
        thread.Start();


        // Cr�ation de deux clients se connectant au serveur
        Client c = new Client();
        c.Connect();

        Client c2 = new Client();
        c2.Connect();

        Thread.Sleep(500);

        // V�rification du nombre de clients connect�s
        Assert.AreEqual(2, serveur.Clients.Count);


        // Deconnexion du premier client
        c.Disconnect();

        Thread.Sleep(500);

        // V�rification du nombre de clients connect�s
        Assert.AreEqual(1, serveur.Clients.Count);

        // Deconnexion du premier client
        c2.Disconnect();

        Thread.Sleep(500);
        // V�rification du nombre de clients connect�s
        Assert.AreEqual(0, serveur.Clients.Count);

        serveur.Disconnect();
    }


    /// <summary>
    /// Test pour voir si un client peut envoyer un message � un serveur qui va le recevoir
    /// </summary>
    [Test]
    public void TestClientSpeakToServeur()
    {
        // Cr�ation d'un thread dans lequel le serveur va attendre les connexions
        Serveur serveur = new Serveur();
        Thread thread = new Thread(delegate ()
        {
            serveur.Connect();
        });
        thread.Start();


        // Cr�ation de deux clients se connectant au serveur
        Client c = new Client();
        c.Connect();

        Client c2 = new Client();
        c2.Connect();

        Thread.Sleep(500);

        // Envoie du message du client1
        c.SendMessage("Tu m'entends ?");
        c2.SendMessage("Et moi ?");

        // Test pour voir si le serveur a re�u le message
        string[] messageRecu = serveur.ReceiveMessage().Split("&&&");
        Assert.AreEqual("||Client0||Tu m'entends ?", messageRecu[0]);
        Assert.AreEqual("||Client1||Et moi ?", messageRecu[1]);


        // Arret des points de communications
        c.Disconnect();
        c2.Disconnect();
        serveur.Disconnect();
    }


    /// <summary>
    /// Test pour voir si le serveur peut parler avec les clients
    /// </summary>
    [Test]
    public void TestServeurSpeakToClient()
    {
        // Cr�ation d'un thread dans lequel le serveur va attendre les connexions
        Serveur serveur = new Serveur();
        Thread thread = new Thread(delegate ()
        {
            serveur.Connect();
        });
        thread.Start();


        // Cr�ation de deux clients se connectant au serveur
        Client c = new Client();
        c.Connect();
        Client c2 = new Client();
        c2.Connect();

        Thread.Sleep(500);

        // Envoie du message du serveur � tous les clients
        serveur.SendMessage("Vous m'entendez ?");
        string messageRecu = c.ReceiveMessage();
        string messageRecu2 = c2.ReceiveMessage();
        Assert.AreEqual("||Server||Vous m'entendez ?", messageRecu);
        Assert.AreEqual("||Server||Vous m'entendez ?", messageRecu2);


        // Envoie du message du serveur au client 0 uniquement
        serveur.SendMessage("Client 0?",0);
        string messageRecu3 = c.ReceiveMessage();
        string messageRecu4 = c2.ReceiveMessage();
        Assert.AreEqual("||Server||Client 0?", messageRecu3);
        Assert.AreEqual("", messageRecu4);

        // Arret des points de communications
        c.Disconnect();
        c2.Disconnect();
        serveur.Disconnect();
    }


}
