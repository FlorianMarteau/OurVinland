using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.METIER
{
    public interface ICommunication
    {

        /// <summary>
        /// Permet à la personne d'envoyer un message à un autre destinataire
        /// </summary>
        /// <param name="msg">Message à envoyer</param>
        /// <param name="destinataire">Numéro du destinataire, -1 si tout le monde</param>
        abstract void SendMessage(string msg, int destinataire = -1);


        /// <summary>
        /// Permet à la personne de recevoir ses messages
        /// </summary>
        abstract string ReceiveMessage();


        /// <summary>
        /// Permet à la personne de se connecter
        /// </summary>
        abstract void Connect();

        /// <summary>
        /// Permet à la personne de se deconnecter
        /// </summary>
        abstract void Disconnect();
    }
}
