using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace boggle
{
    public class Jeu
    {
        Joueur[] joueurs;
        string path;

        public Jeu(int _minuteur)
        {
            path = "../../../docs/LettresFR.txt";
            this.joueurs = new Joueur[2];
        }

        public Joueur[] Joueurs
        {
            get { return this.joueurs; }
            set { this.joueurs = value; }
        }

        #region Saisie Nom
        /// <summary>
        /// Demande à l'utilisateur de saisir son nom de joueur dans la console et enregistre cette chaine de caractère
        /// </summary>
        /// <param name="j">Le numéro du joueur (1 ou 2)</param>
        /// <returns>Retourne le nom saisi dans la console par le joueur</returns>
        public string SaisirNom(int j)
        {
            Console.Write("Nom du joueur " + j);
            if (j == 1)
            {
                Console.Write(" : ");
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.Write(" (Ecrire \"ia\" pour affronter l'IA) : ");
                Console.ForegroundColor = ConsoleColor.Red;
            }
            string saisie = Console.ReadLine();
            Console.ResetColor();

            while (string.IsNullOrWhiteSpace(saisie))
            {
                Console.Write("Saisie incorrecte, veuillez réessayer : ");
                if (j == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                saisie = Console.ReadLine();
                Console.ResetColor();
            }
            return saisie;
        }
        #endregion

        #region Minuteur
        /// <summary>
        /// Lance un minuteur en fonction de la durée voulue et de l'heure de départ
        /// </summary>
        /// <param name="duree">Durée du minuteur</param>
        /// <param name="debut">Heure de début du minuteur</param>
        /// <returns>Retourne le temps restant</returns>
        public TimeSpan Minuteur(TimeSpan duree, DateTime debut)
        {
            TimeSpan tempsEcoule = DateTime.Now - debut;
            TimeSpan tempsRestant = duree - tempsEcoule;
            return tempsRestant;
        }
        #endregion

        #region Affichage barre de progression du minuteur
        /// <summary>
        /// Renvoir une barre de progression de l'avancement du minuteur
        /// </summary>
        /// <param name="tempsRestant">Durée du minuteur</param>
        /// <param name="duree">Duree du minuteur</param>
        /// <returns>Retourne une string représentant une barre de progression</returns>
        public string BarreProgression(TimeSpan tempsRestant, TimeSpan duree)
        {
            string barre = "";
            double totalSymbols = 20.0;
            double percentage = Convert.ToDouble(tempsRestant.Seconds) / (duree.Seconds + (duree.Minutes * 60));
            int numberOfDashes = Convert.ToInt32(percentage * totalSymbols);
            for (int i = 0; i < totalSymbols - numberOfDashes; i++)
            {
                barre += "█";
            }
            for (int i = 0; i < numberOfDashes; i++)
            {
                barre += "░";
            }
            return barre;
        }
        #endregion

        #region Mise à jour du score
        /// <summary>
        /// Met à jour le score du joueur en fonction du mot entré
        /// </summary>
        /// <param name="j">Numéro du joueur (0 étant le premier joueur et 1 le second)</param>
        /// <param name="mot">Mot entré par le joueur, précédemment vérifié</param>
        public int UpdateScore(int j, string mot)
        {
            int s = this.joueurs[j].Score;
            mot = mot.ToUpper();
            string texte = File.ReadAllText(this.path);
            string[] lignes = texte.Split('\n');
            for (int i = 0; i < mot.Length; i++)
            {
                foreach (string ligne in lignes)
                {
                    string[] parties = ligne.Split(';');
                    if (parties.Length >= 3)
                    {
                        if (parties[0][0] == mot[i])
                        {
                            this.joueurs[j].Score += int.Parse(parties[1]);
                        }
                    }
                }
            }
            return (this.joueurs[j].Score - s);
        }
        #endregion
    }
}