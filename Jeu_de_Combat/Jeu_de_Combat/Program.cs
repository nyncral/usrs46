using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace Jeu_de_Combat
{
    class Program
    {
        static void Main(string[] args)
        {
            // Permet l'affichage en plein écran
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            ShowWindow(ThisConsole, MAXIMIZE);

            // Initialisation de quelques variables
            int pvJ = 0;
            int pvIA = 0;
            int levelIA = 0;
            int actjoueur = 0;
            int coolDownJ = 3;
            int coolDownIA = 3;
            int nbTour = 0;

            string temp = "";

            bool histoire = true;


            // Création des listes/dictionnaires contenant les classes ainsi que les choix possibles d'actions
            List<string> opts = new List<string> { "Healer", "Tank  ", "Damager", "Vampire" };
            List<int> action = new List<int> { 1, 2, 3 };
            Dictionary<string, string> convertion = new Dictionary<string, string>()
            {
                { "H", "Healer" },
                { "T", "Tank" },
                { "D", "Damager" },
                { "V", "Vampire" }
            };
            Dictionary<int, string> convertionAction = new Dictionary<int, string>()
            {
                { 1, "Attaque" },
                { 2, "Défense" },
                { 3, "Attaque Spéciale" }
            };


            //Appel de la fonction AfficheRobert
            AfficheRobert();


            //Tous les Console.WriteLine/Write sont remplacés par deux fonctions permettant d'afficher le texte au centre de l'écran
            WriteLineC("Bienvenue dans le Robert Game ! Ici pas de pitié, c'est tuer ou être tué.");


            //Retour à la ligne, pause et séparation
            Console.WriteLine();
            Thread.Sleep(1000);
            SeparationLine();


            //Demande du nom
            WriteLineC("Quel est votre nom ?");
            WriteF();
            Console.ReadLine();


            //Retour à la ligne et pause
            Console.WriteLine();
            Thread.Sleep(500);

            //Réponse de l'IA
            WriteLineC("Génial, je vous appellerai donc Robert !");

            //Pause et séparation
            Thread.Sleep(1000);
            SeparationLine();

            // Boucle tant que le choix du joueur ne correspond à aucunes des possibilités, redemander le choix de la difficulté ou du mode de jeu
            while (true)
            {
                //Choix de la difficulté et du mode de jeu
                WriteLineC("Quel niveau d'IA voulez-vous affronter ?");
                WriteLineC("0 - Random");
                WriteLineC("1 - Facile");
                WriteLineC("2 - Normal");
                WriteLineC("3 - Dieu  ");
                WriteF();

                //saisie du joueur
                while (!int.TryParse(Console.ReadLine(), out levelIA))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    WriteLineC("Voyons Robert, veuillez choisir un niveau d'IA valide ! (entre 0, 1, 2, ou 3)");
                    Console.ResetColor();
                    WriteF();
                }

                // Si le choix correspond aux possibilités, on casse la boucle
                if (levelIA == 0 || levelIA == 1 || levelIA == 2 || levelIA == 3)
                {
                    break;
                }
            }

            ForceReload();

            Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight / 2 - 4);
            List<List<string>> classes = new List<List<string>>();
            classes.Add(new List<string>() { "Classes : ", "Healer", "Tank", "Damager", "Vampire" });
            classes.Add(new List<string>() { "Attaque :", "1 dégât", "1 dégât", "2 dégâts", "2 dégâts" });
            classes.Add(new List<string>() { "PV", "8HP", "10HP", "5HP", "7HP" });
            classes.Add(new List<string>() { "Attaque Spéciale :", " + 3PV ", " 2 dégats, - 1 PV sauf si ennemi défend = 1 dégât, - 1 PV", "Subit un dégat de moins et inflige autant qu'il subit + 1", " 3 dégâts et + 1 PV si ennemi défend, sinon 1 dégât" });
            jeSAppelleGroot(classes, temp);

            SeparationLine();

            WriteLineC("Veuillez choisir une classe parmi les suivantes : ");


            // Affichage des classes disponibles par appel de la liste opts
            for (int i = 0; i < opts.Count; i++)
            {
                WriteLineC(opts[i][0] + " - " + opts[i]);
            }
            WriteF();

            // Choix des classes par le joueur et l'IA
            string PersoJ = PlayerChoice(opts, convertion);
            string PersoIA;

            // Si le choix du niveau est tout sauf le dernier niveau, on fait choisir la classe de l'IA par appel de la fonction IaRandomChoice
            if (levelIA != 3)
            {
                PersoIA = IaRandomChoice(opts);

                // Stockage des PV du joueur et de l'IA dans les variables pvJ et pvIA
                pvJ = PVParRole(PersoJ[0] + "");
                pvIA = PVParRole(PersoIA[0] + "");
            }
            // Si le choix de niveau est le dernier, alors le perso devient "déesse Pauline"
            else
            {
                PersoIA = "Déesse Pauline";
                // Stockage des PV du joueur et de l'IA dans les variables pvJ et pvIA (ici les PV de l'IA sont inutiles car ce niveau est scripté)
                pvJ = PVParRole(PersoJ[0] + "");
                pvIA = 666;
            }


            // Appel de la fonction permettant de clear la console si le texte dépasse l'écran
            TropEgalTrop(levelIA, pvJ, pvIA, PersoJ, PersoIA, coolDownJ, coolDownIA);
            SeparationLine();


            //Clear, affiche "robert" et séparation
            ForceReload();


            // Si le niveau est tout sauf le dernier, on affiche les stats des deux joueurs
            if (levelIA != 3)
            {
                AfficheStats(PersoJ, PersoIA, pvJ + "", pvIA + "", coolDownJ, coolDownIA);
            }
            // Si on choisi le dernier niveau, on affiche des stats personnalisées
            else
            {
                WriteC("Pauline a nerf toutes vos stats !!!!");
                Console.WriteLine();
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Yellow;

                // Création du tableau des stats
                Console.BackgroundColor = ConsoleColor.DarkGray;
                WriteLineCChar("Robert", "Pauline", " | ", 1, true);
                Console.BackgroundColor = ConsoleColor.Black;

                WriteLineCChar(PersoJ, PersoIA, " | ", 1, true);
                WriteLineCChar("Classe : ", "", "", PersoJ.Length + 2, false);
                WriteLineCChar("1" + "", "???", " | ", 1, true);
                WriteLineCChar("PV : ", "", "", PersoJ.Length + 2, false);
                WriteLineCChar("1" + "", "???" + "", " | ", 1, true);
                WriteLineCChar("Attaque : ", "", "", PersoJ.Length + 2, false);

                Console.ResetColor();
            }
            Thread.Sleep(1000);


            // Boucle tant que la partie n'est pas finie, relancer un tour
            while (true)
            {


                nbTour++;

                if (levelIA != 3)
                {
                    SeparationLine();

                    // Choix des actions du tour
                    WriteLineC("Que voulez-vous faire ?");
                    WriteLineC("1 - Attaquer");
                    WriteLineC("2 - Se défendre");
                    WriteLineC("3 - Action Spéciale (-3MP)");
                    WriteF();

                    // Boucle tant que le joueur ne fait pas un choix valide, redemander une sélection
                    while (true)
                    {
                        while (!int.TryParse(Console.ReadLine(), out actjoueur))
                        {
                            TropEgalTrop(levelIA, pvJ, pvIA, PersoJ, PersoIA, coolDownJ, coolDownIA);
                            Console.ForegroundColor = ConsoleColor.Red;
                            WriteLineC("Voyons Robert, veuillez choisir une action valide ! (entre 1, 2 et 3)");
                            Console.ResetColor();
                            WriteF();
                        }

                        // si une condition est valide on sort de la boucle
                        if (actjoueur == 1 || actjoueur == 2 || (actjoueur == 3 && coolDownJ == 3))
                        {
                            break;
                        }
                        //if (actjoueur == 3 && coolDownJ == 3)
                        //{
                        //    break;
                        //}


                        // Affichage d'erreur si le joueur veut utiliser sa capacité spéciale sous cooldown
                        if (actjoueur == 3 && coolDownJ < 3)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            string temporary = "Vous n'avez plus que " + (coolDownJ + "") + " MP. Il vous faut 3 MP pour utiliser votre attaque spéciale.";
                            WriteLineC("Voyons Robert, vous ne pouvez pas utiliser votre attaque spéciale pour le moment.");
                            WriteLineC(temporary);
                            WriteLineC("Choisissez entre 1 ou 2.");
                            Console.ResetColor();
                            WriteF();
                            // Affichage d'erreur si le joueur ne rentre pas d'action valide
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            WriteLineC("Voyons Robert, veuillez choisir une action valide ! (entre 1, 2 et 3)");
                            Console.ResetColor();
                            WriteF();
                        }
                    }

                }
                Console.WriteLine();

                // Choix de l'action de l'ordi
                int actIA = 0;
                // Si l'IA est au niveau de difficulté le plus bas, il ne fera que des choix d'action au hasard
                if (levelIA == 0)
                {
                    actIA = IaActionRandom(coolDownIA);
                }
                // Si l'IA est au second niveau de difficulté, choix avec appel de la fonction IaActionFacile
                if (levelIA == 1)
                {
                    while (true)
                    {
                        actIA = IaActionFacile(action, PersoIA, pvIA);
                        if (actIA == 3 && coolDownIA != 3)
                        {
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                // Si l'IA est au niveau de difficulté le plus élevé, choix avec appel de la fonction IaActionMedium
                if (levelIA == 2)
                {
                    while (true)
                    {
                        actIA = IaActionMedium(action, PersoIA, pvIA, pvJ);
                        if (actIA == 3 && coolDownIA != 3)
                        {
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                // Si l'IA est au niveau de difficulté God, on lance la boucle pour le mode scripté
                if (levelIA == 3)
                {
                    while (true)
                    {
                        //Choix de l'action du joueur
                        SeparationLine();
                        Thread.Sleep(1000);
                        WriteLineC("Que voulez-vous faire?");
                        WriteLineC("0 - Se défendre");
                        WriteLineC("1 - Attaquer");
                        WriteF();

                        //Saisie de son action
                        while (!int.TryParse(Console.ReadLine(), out actjoueur))
                        {
                            TropEgalTrop(levelIA, pvJ, pvIA, PersoJ, PersoIA, coolDownJ, coolDownIA);
                            Console.ForegroundColor = ConsoleColor.Red;
                            WriteLineC("Veuillez choisir une action valide (entre 0 et 1)");
                            Console.ResetColor();
                            WriteF();
                        }
                        Thread.Sleep(1000);

                        //Vérification de la validité de ça saisie
                        if (actjoueur == 0 || actjoueur == 1 || (nbTour == 24 && actjoueur == 24))
                        {
                            break;
                        }
                    }

                    //Mise à la ligne, verification du dépasement, séparation
                    Console.WriteLine();
                    TropEgalTrop(levelIA, pvJ, pvIA, PersoJ, PersoIA, coolDownJ, coolDownIA);
                    SeparationLine();

                    //Fonction dédier au texte personalisé du mode histoire
                    PaulineGod(ref nbTour, actjoueur, ref histoire);

                }

                //Gestion du cooldown de l'IA


                //Fonctionnement normal si le niveau 3 c'est pas selectionné (mode histoire)
                if (levelIA != 3)
                {
                    if (coolDownJ < 3)
                    {
                        coolDownJ++;
                    }
                    if (actjoueur == 3)
                    {
                        coolDownJ = 0;
                    }


                    if (coolDownIA < 3)
                    {
                        coolDownIA++;
                    }
                    if (actIA == 3)
                    {
                        coolDownIA = 0;
                    }

                    //Calcule des dégat du tour
                    Tuple<int, int> Damage = ResolutionAction(actjoueur, actIA, PersoJ, PersoIA, pvJ, pvIA);


                    //Mise à jour des pv
                    pvJ += Damage.Item1;
                    pvIA += Damage.Item2;

                    //Récap des action et des pv restant
                    SeparationLine();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    temp = "Vous avez utilisé : " + convertionAction[actjoueur];
                    WriteLineC(temp);
                    temp = "L'IA a utilisé : " + convertionAction[actIA];
                    WriteLineC(temp);
                    temp = "PV de Robert : " + pvJ;
                    WriteLineC(temp);
                    temp = "PV de l'IA : " + pvIA;
                    WriteLineC(temp);
                    temp = "MP de Robert : " + coolDownJ;
                    WriteLineC(temp);
                    temp = "MP de l'IA : " + coolDownIA;
                    WriteLineC(temp);
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    TropEgalTrop(levelIA, pvJ, pvIA, PersoJ, PersoIA, coolDownJ, coolDownIA);
                }

                if (pvJ <= 0 || pvIA <= 0 || histoire == false)
                {
                    break;
                }



            }

            TropEgalTrop(levelIA, pvJ, pvIA, PersoJ, PersoIA, coolDownJ, coolDownIA);

            //Apelle de la fonction Result
            Result(pvJ, pvIA, args);

        }


        //Permet de calculé les dégat subie pendant le tour
        static Tuple<int, int> ResolutionAction(int actionJoueur, int actionIA, string PersoJ, string PersoIA, int pvJ, int pvIA)
        {

            int DmgJ = 0;
            int DmgIA = 0;


            //Action d'Attaque Normal (pour IA et Joueur)
            if (actionJoueur == 1)
            {
                if (actionIA != 2)
                {
                    DmgIA -= DommageParRole(PersoJ[0] + "");
                }
            }
            if (actionIA == 1)
            {
                if (actionJoueur != 2)
                {
                    DmgJ -= DommageParRole(PersoIA[0] + "");
                }
            }

            //Action d'Attaque Spéciale Healer et Tank (pour IA et Joueur)
            if (actionJoueur == 3)
            {
                if (PersoJ[0] + "" == "H")
                {
                    SpeHealer(ref DmgJ, pvJ);
                }
                else if (PersoJ[0] + "" == "T")
                {
                    SpeTank(ref DmgJ, ref DmgIA, actionIA);
                }
            }
            if (actionIA == 3)
            {
                if (PersoIA[0] + "" == "H")
                {
                    SpeHealer(ref DmgIA, pvIA);
                }
                else if (PersoIA[0] + "" == "T")
                {
                    SpeTank(ref DmgIA, ref DmgJ, actionJoueur);
                }
            }

            //Action d'Attaque Spéciale Vampire (pour IA et Joueur)
            if (actionJoueur == 3)
            {
                if (PersoJ[0] + "" == "V")
                {
                    SpeVampire(ref DmgJ, ref DmgIA, actionIA);
                }
            }
            if (actionIA == 3)
            {
                if (PersoIA[0] + "" == "V")
                {
                    SpeVampire(ref DmgIA, ref DmgJ, actionJoueur);
                }
            }

            //Action d'Attaque Spéciale Damager (pour IA et Joueur)
            if (actionJoueur == 3)
            {
                if (PersoJ[0] + "" == "D")
                {
                    SpeDamager(ref DmgJ, ref DmgIA);
                }
            }
            if (actionIA == 3)
            {
                if (PersoIA[0] + "" == "D")
                {
                    SpeDamager(ref DmgIA, ref DmgJ);
                }
            }

            return new Tuple<int, int>(DmgJ, DmgIA);
        }


        //Calcule damage attaque normal
        static int DommageParRole(string role)
        {
            var charactersDM = new Dictionary<string, int>() { { "H", 1 }, { "T", 1 }, { "D", 2 }, { "V", 2 } };
            return charactersDM[role];
        }

        //calcule pv par role
        static int PVParRole(string role)
        {
            var charactersDM = new Dictionary<string, int>() { { "H", 8 }, { "T", 10 }, { "D", 5 }, { "V", 7 } };
            return charactersDM[role];
        }


        //Choix du Joueur pour la classe
        static string PlayerChoice(List<string> options, Dictionary<string, string> conv)
        {
            List<string> opt = new List<string>();
            foreach (var option in options)
            {
                opt.Add(option[0] + "");
            }

            // Initialisation des variables pour le choix du joueur et la boucle
            string choice;

            // Boucle Tant que le choix ne correspond a aucunes des options, redemander un choix valable de classe
            while (true)
            {
                // Lecture du choix du joueur
                choice = Console.ReadLine();

                // Si le choix correspond à l'une des options, casser la boucle
                if (opt.Contains(choice))
                    break;
                Console.ForegroundColor = ConsoleColor.Red;
                WriteLineC("Veuillez entrer une classe existante");
                Console.ResetColor();
                WriteF();
            }
            // On retourne le choix du joueur
            return conv[choice];
        }

        static string IaRandomChoice(List<string> options)
        {
            string IaChoice;

            //Choix aléatoire
            Random random = new Random();
            int choixIA = random.Next(3);

            //Retour de la valeur aléatoire
            IaChoice = options[choixIA];
            return IaChoice;
        }


        //Choix d'une action aléatoire par l'IA
        static int IaActionRandom(int coolDown)
        {
            //Choix aléatoire
            Random random = new Random();
            int choixIA = 0;
            if (coolDown < 3)
            {
                choixIA = random.Next(1, 3);
            }
            else
            {
                choixIA = random.Next(1, 4);
            }
            //Retour de la valeur aléatoire
            return choixIA;
        }

        //Choix plus ou moins réfléchi de l'action pour l'IA || non. si. vraiment pas. c'est pas bien de se moquer. je me moque pas c'est pas vrai. ok elle est conne mais faut pas le dire. bah je le dirai pas alors. bien. waf. va bosser ||
        static int IaActionFacile(List<int> action, string perso, int pvIA)
        {
            int IaChoiceFacile = 0;
            string initialePerso = perso[0] + "";
            Random random = new Random();

            if (initialePerso == "D")
            {
                if (pvIA >= 3)
                {
                    IaChoiceFacile = action[random.Next(0, 2) * 2];
                }

                if (pvIA < 3)
                {
                    if (random.Next(5) == 0)
                    {
                        IaChoiceFacile = action[0];
                    }
                    else
                    {
                        IaChoiceFacile = action[random.Next(1, 3)];
                    }
                }
            }


            if (initialePerso == "H")
            {
                if (pvIA >= 3)
                {
                    IaChoiceFacile = action[random.Next(0, 2) * 2];
                }

                if (pvIA < 3)
                {
                    if (random.Next(5) == 0)
                    {
                        IaChoiceFacile = action[0];
                    }
                    else
                    {
                        IaChoiceFacile = action[random.Next(1, 3)];
                    }
                }
            }

            if (initialePerso == "T")
            {
                if (pvIA >= 3)
                {
                    IaChoiceFacile = action[random.Next(0, 2) * 2];
                }

                if (pvIA < 3)
                {
                    if (random.Next(5) == 0)
                    {
                        IaChoiceFacile = action[0];
                    }
                    else
                    {
                        IaChoiceFacile = action[random.Next(1, 3)];
                    }
                }
            }

            if (initialePerso == "V")
            {
                if (pvIA >= 3)
                {
                    IaChoiceFacile = action[random.Next(0, 2) * 2];
                }

                if (pvIA < 3)
                {
                    if (random.Next(5) == 0)
                    {
                        IaChoiceFacile = action[0];
                    }
                    else
                    {
                        IaChoiceFacile = action[random.Next(1, 3)];
                    }
                }
            }

            return IaChoiceFacile;

        }

        //Choix de l'action de l'IA la plus complexe et complete mis en place dans ce jeu
        static int IaActionMedium(List<int> action, string perso, int pvIA, int pvJ)
        {
            int IaChoiceMedium = 0;
            string initialePerso = perso[0] + "";
            Random random = new Random();

            //Choix de l'action si l'IA est une Damager
            if (initialePerso == "D")
            {
                if (pvIA >= 3)
                {
                    IaChoiceMedium = action[random.Next(0, 2) * 2];
                }

                if (pvIA < 3 && pvJ > 2)
                {
                    IaChoiceMedium = action[random.Next(1, 3)];
                }
                else
                {
                    IaChoiceMedium = action[0];
                }
            }

            //Choix de l'action si l'IA est une Healer
            if (initialePerso == "H")
            {
                if (pvIA >= 3)
                {
                    IaChoiceMedium = action[random.Next(0, 2) * 2];
                }

                if (pvIA == 2 && pvJ > 1)
                {
                    IaChoiceMedium = action[random.Next(1, 3)];
                }
                else
                {
                    IaChoiceMedium = action[random.Next(0, 2) * 2];
                }
            }

            //Choix de l'action si l'IA est une Tank
            if (initialePerso == "T")
            {
                if (pvIA >= 3)
                {
                    IaChoiceMedium = action[random.Next(0, 2) * 2];
                }

                if (pvIA <= 2 && pvJ <= 2)
                {
                    IaChoiceMedium = action[2];
                }
                else
                {
                    IaChoiceMedium = action[random.Next(0, 2)];
                }
            }

            //Choix de l'action si l'IA est une Vampire
            if (initialePerso == "V")
            {
                if (pvIA >= 3)
                {
                    IaChoiceMedium = action[random.Next(0, 2) * 2];
                }

                if (pvIA < 3 && pvJ > 2)
                {
                    IaChoiceMedium = action[random.Next(1, 3)];
                }
                else
                {
                    IaChoiceMedium = action[0];
                }
            }

            return IaChoiceMedium;

        }



        static void PaulineGod(ref int nbTour, int actjoueur, ref bool histoire)
        {

            /*
             Pour les if() suivants c'est la même chose à chaque fois
             Si tu es là pour tricher et regarder la réponse dans le code
             Ben casse toi
             Mais si tu es la juste par curiosité alors assis toi et prépare toi pour une ribandelle d'explications.
             Premièrement le code n'est pas opti.
             Deuxièmement tous les if() sont necéssaires.
             Et pour finir TOUT ce qui suit n'est quasiment que de l'affichage de texte scripté.
             Bonne chance si tu veux tout lire.
            */




            if (nbTour == 1)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous bloquez avec succès la première attaque de la déesse Pauline.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    WriteLineC("Vous êtes la clé.");
                    Console.ResetColor();
                }
            }

            if (nbTour == 2)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("La déesse Pauline semble perdue dans ses pensées.");
                    WriteLineC("Vous en profitez pour lui asséner un coup. -2PV");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 3)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pauline n'a pas aimé le coup que vous lui avez donné, elle vous attaque.");
                    WriteLineC("Heureusement, vous avez eu la lucidité de défendre l'attaque. Pour cette fois.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 4)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous attaquez la déesse, mais elle esquive en effectuant une magnifique roue.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    WriteLineC("N'oubliez pas, vous êtes la clé.");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 5)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pauline réagit à votre précédente attaque, mais doit s'arrêter à cause d'un hérisson entre vous deux.");
                    WriteLineC("L'équipe de secours l'a transféré en dehors de l'arène.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 6)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous vous mettez en boule tandis que Pauline vous assène un giga high kick de la mort.");
                    WriteLineC("Belle défense.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    WriteLineC("Dans un monde où seules deux valeurs existent, vous êtes roi.");
                    Console.ResetColor();
                }
            }

            if (nbTour == 7)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous attaquez la déesse et la touchez, lui infligeant une belle gifle ! -0,5PV");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 8)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pauline vous invite à danser, ce que vous refusez intelligemment.");
                    WriteLineC("Ce n'était qu'une astuce pour vous faire marcher dans un piège à loup.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    WriteLineC("Ce n'est qu'une succession de motifs.");
                    Console.ResetColor();
                }
            }

            if (nbTour == 9)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Rageuse de son plan découvert, Pauline vous attaque.");
                    WriteLineC("Mais vous avez vu Matrix plus d'une fois dans votre vie, vous esquivez.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 10)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pendant que votre adversaire rumine de rage vous en profitez pour lui lancer un panier de pommes.");
                    WriteLineC("C'est une façon comme une autre de se débarrasser d'une déesse.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    WriteLineC("Vous avez trouvé la réponse ?");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 11)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pauline est tombée dans les pommes, l'occasion pour vous de lui marcher sur le pied ! -4PV");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 12)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous levez votre bouclier, juste avant de vous rappeler que vous n'en aviez pas.");
                    WriteLineC("Heureusement, Pauline éclate de rire et en oublie d'attaquer.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    WriteLineC("Un jeu est rempli d'informations, un écran aussi.");
                    Console.ResetColor();
                }
            }

            if (nbTour == 13)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pendant que Pauline essuie ses larmes de joie, vous vous vengez en tentant lui lancer une malédiction.");
                    WriteLineC("Cela échoue.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 14)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous tentez alors de l'attaquer de front, mais les profs de Coding Room rappliquent et prennent les coups pour elle.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 15)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous décidez de la confronter au pierre-feuille-ciseau.");
                    WriteLineC("Cependant, à la première manche, elle sort le puit.");
                    WriteLineC("C'est un échec cuisant.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 16)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Honteux de vos multiples échecs, vous déprimez et arrêtez d'attaquer pour ce tour.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 17)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous décidez de prendre une pause café, esquivant automatiquement l'attaque de Pauline.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 18)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Remis d'aplomb et la confiance gonflée à bloc, vous attaquez Pauline alors qu'elle n'était pas prête.");
                    WriteLineC("Pas très fair play, mais l'attaque fait mouche. -5PV");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 19)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous ne vous arrêtez pas là et réattaquez, malheureusement vous n'êtes pas doué et ratez la déesse.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 20)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Epuisé, vous vous écroulez, Pauline n'avait pas prévu cela et rate son attaque.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 21)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pauline profite de votre temps au sol pour retenter une attaque, mais vous lui sortez la reverse card.");
                    WriteLineC("Elle se prend sa propre attaque. -200 PV pour Pauline !");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 22)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous vous relevez mais trébuchez, ce qui vous permet d'esquiver une attaque létale de votre adversaire.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 23)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("En vous relevant, vous mettez un coup de tête non intentionnel à la déesse, lui infligeant -3 PV.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    WriteLineC("Cela fait longtemps que vous vous battez et si vous ne faites pas le bon choix au prochain tour, on ne pourrait en être qu'à la moitié du combat !");
                    WriteLineC("Après tout ce n'est qu'une option sur une infinité.");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 24)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else if (actjoueur == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pauline vous attaque :");
                    WriteLineC("\"le signe infini est le plus grand chiffre du monde !\"");
                    WriteLineC("Vous vous défendez en argumentant :");
                    WriteLineC("\"Et infini + 1 ?\"");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else if (actjoueur == 24)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    WriteLineC("Vous annoncez à Pauline que vous avez quelque chose à lui dire.");
                    WriteLineC("Attaquez !");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                    nbTour = 46;
                }
            }

            if (nbTour == 25)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Après cet argument imparable, Pauline préfère la bonne vieille méthode d'attaque de front.");
                    WriteLineC("Mais vous parez l'attaque grâce à vos souvenirs de cours d'arts martiaux d'il y a 10 ans.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 26)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("C'est à votre tour d'attaquer, vous sentez l'âme de Jackie Chan en vous, et faites un enchainement d'attaques assez convainvant.");
                    WriteLineC("Dommage que cela se finisse sur un pantalon craqué et un changement honteux dans l'arène.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 27)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous avez maintenant un pantalon de meilleure qualité, vous recommencez votre combo et touchez Pauline plusieurs fois ! -4PV -3PV -4PV -5PV");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 28)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous vous attendez à une contre attaque de Pauline et défendez.");
                    WriteLineC("En effet, vous vous faites attaquer.");
                    WriteLineC("Par une mouche.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 29)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Cette fois c'est Pauline qui attaque, mais vous esquivez en voulant écraser cette satanée mouche.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 30)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("En vous retournant, triomphant de cette victoire contre la mouche, vous voyez Pauline de dos et en profitez pour l'attaquer. -5PV");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 31)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pauline se retourne habilement et vous assène une attaque circulaire que vous esquivez en sautant par dessus !");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 32)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous envoyez en retour une attaque circulaire, mais la déesse l'esquive en voulant ramasser une magnifique rose laissée au sol de l'arène.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 33)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous défendez un lancer de rose pleine d'épines, ça pique mais c'est pas fatal.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 34)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Une course s'engage entre vous et votre adversaire.");
                    WriteLineC("Vous ne courrez pas très vite...");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 35)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pauline n'a aucun mal à vous distancer,");
                    WriteLineC("vous jouez donc le tout pour le tout et lui lancez un caillou à la tête.");
                    WriteLineC("Malheureusement, vous n'avez pas fait lancer de cailloux au lycée, vous ratez.");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 36)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("C'est encore un échec, et en plus vous êtes essouflé,");
                    WriteLineC("pas la peine de tente une attaque ce tour,");
                    WriteLineC("cela résulterait à un moment génant.");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 37)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Le temps de vous reposer,");
                    WriteLineC("vous vous contentez de défendre les attaques de Pauline,");
                    WriteLineC("beaucoup plus entrainée que vous. Belle esquive !");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 38)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Le temps de défendre une nouvelle attaque de la déesse,");
                    WriteLineC("vous vous dites que ce combat n'a que trop duré,");
                    WriteLineC("il faudrai y mettre un terme bientôt.");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 39)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous avez récupéré votre énergie,");
                    WriteLineC("et attaquez vigoureusement Pauline,");
                    WriteLineC("l'attaque touche ! -10PV");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 40)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pauline contre attaque mais vous faites une magnifique esquive, c'est raté !");
                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 41)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous vous défendez contre une nouvelle attaque de votre adversaire par une magnifique parade,");
                    WriteLineC("vous auriez enfin gagné un peu d'habileté ?");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 42)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous contre attaquez mais n'arrivez pas à toucher Pauline alors qu'elle ne bougeait pas.");
                    WriteLineC("Vous n'avez définitivement pas gagné en habileté...");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 43)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pendant que Pauline se moque de votre raté plutôt ridicule,");
                    WriteLineC("vous relancez une attaque mais impossible de la toucher.");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 44)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Vous ne lachez rien et l'attaquez de nouveau,");
                    WriteLineC("je ne vous dis pas la suite,");
                    WriteLineC("c'est pas beau à voir.");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
                else
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 45)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pauline en a marre de ce combat,");
                    WriteLineC("se battre contre un clown comme vous commence à être lassant.");
                    WriteLineC("Par un effort exceptionnel (ou une chance),");
                    WriteLineC("vous esquivez une attaque qui vous aurait été sûrement fatale.");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : ???");
                    Console.ResetColor();
                }
            }

            if (nbTour == 46)
            {
                if (actjoueur == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Il vous vient une idée exceptionnelle,");
                    WriteLineC("vous annoncez à Pauline que vous avez trouvé une alternance !");
                    WriteLineC("La nouvelle la laisse sans voix et la laisse à un PV !");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : 1");
                    Console.ResetColor();
                }
                else if (actjoueur == 0)
                {
                    //Mort
                    RobertLooseDefense(ref histoire);
                }
            }

            if (nbTour == 47)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Pauline n'ayant plus de vie,");
                    WriteLineC("elle enrage et vous attaque de toutes ses forces,");
                    WriteLineC("vous vous prépariez à défendre quand un pigeon passa devant vous");
                    WriteLineC("et prit tous les dégâts à votre place.");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : 1");
                    Console.ResetColor();
                }
            }

            if (nbTour == 48)
            {
                if (actjoueur == 1)
                {
                    //Mort
                    RobertLooseAttaque(ref histoire);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    WriteLineC("Alors qu'elle s'avance pour en finir,");
                    WriteLineC("la déesse de toute chose marche sur une épine que le hérisson avait laissé au début,");
                    WriteLineC("traversant son talon et lui retira son point de vie restant.");
                    WriteLineC("Pas de chance, c'était son talon de Pauline.");

                    WriteLineC("PV de Robert : 1");
                    WriteLineC("PV de Déesse Pauline : 1");
                    Thread.Sleep(10000);
                    Console.ResetColor();
                    RobertVictory(ref histoire);
                }
            }
        }

        //Fonction de défaite dans le mode histoire lorsqu'on défend alors qu'il falait attaquer (c'etait pourtant évidant)
        static void RobertLooseDefense(ref bool histoire)
        {
            Bordel();
            AfficheDefaite();
            AfficheRobert();
            SeparationLine();
            WriteLineC("Vous vous êtes mis en position de défense, mais avez attendu un peu trop longtemps alors que Pauline était juste partie chercher des orchidées. Vous êtes mort d'ennui.");
            Thread.Sleep(1000);
            histoire = false;
        }

        //Fonction de défaite dans le mode histoire lorsqu'on attaque alors qu'il falait défendre (bolosse)
        static void RobertLooseAttaque(ref bool histoire)
        {
            Bordel();
            AfficheDefaite();
            AfficheRobert();
            SeparationLine();
            Random random = new Random();
            if (random.Next(2) == 0)
            {
                WriteLineC("Vous n'avez pas été assez prudent, Pauline vous a massacré à coup de Coding Room ! Vous avez perdu !");
            }
            else
            {
                WriteLineC("Vous avancez sans faire attention où vous marchez, vous vous noyez dans un océan de code. Vous avez perdu !");
            }
            Thread.Sleep(1000);
            histoire = false;
        }

        //Fonction de victoire dans le mode histoire 
        static void RobertVictory(ref bool histoire)
        {
            AfficheVictoire();
            AfficheRobert();
            SeparationLine();
            histoire = false;
        }



        //Attaque spéciale du Damager
        static void SpeDamager(ref int degatSubisUtilisateur, ref int degatSubisDefenseur)
        {
            if (degatSubisUtilisateur < 0)
            {
                degatSubisDefenseur += degatSubisUtilisateur - 1;
                degatSubisUtilisateur += 1;
            }
        }

        //Attaque spéciale du Healer
        static void SpeHealer(ref int DmgRecu, int pv)
        {
            if (pv <= 5)
            {
                DmgRecu += 3;
            }
            else if (pv == 6)
            {
                DmgRecu += 2;
            }
            else if (pv == 7)
            {
                DmgRecu += 1;
            }
        }

        //Attaque spéciale du Tank
        static void SpeTank(ref int vieUtilisateur, ref int vieDefenseur, int act)
        {
            if (act != 2)
            {
                vieUtilisateur -= 1;
                vieDefenseur -= 2;
            }
            else
            {
                vieUtilisateur -= 1;
                vieDefenseur -= 1;
            }
        }

        //Attaque spéciale du Vampire
        static void SpeVampire(ref int vieUtilisateur, ref int vieDefenseur, int act)
        {
            if (act == 2)
            {
                vieDefenseur -= 3;
                vieUtilisateur += 1;
            }
            else
            {
                vieDefenseur -= 1;
            }

        }


        //Affiche les résultats de partie et relance le jeux au bon vouloir du joueur
        static void Result(int pvJ, int pvIA, string[] args)
        {


            //Appelle la fonction correspondant au resultat et affiche un message
            if (pvJ <= 0 && pvIA <= 0)
            {
                AfficheEgalité();
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight / 2);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                SeparationLine();
                WriteLineC("Vous avez tous les deux perdu tout vos points de vie, c'est une égalité !");
                Console.ResetColor();
                SeparationLine();
            }
            if (pvJ <= 0 && pvIA > 0)
            {
                AfficheDefaite();
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight / 2);
                Console.ForegroundColor = ConsoleColor.Red;
                SeparationLine();
                WriteLineC("Vous n'avez plus aucun point de vie, vous avez perdu le combat !");
                Console.ResetColor();
                SeparationLine();
            }
            if (pvIA <= 0 && pvJ > 0)
            {
                AfficheVictoire();
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight / 2);
                Console.ForegroundColor = ConsoleColor.Green;
                SeparationLine();
                WriteLineC("L'ordinateur n'a plus de point de vie, vous avez donc gagné le combat !");
                Console.ResetColor();
                SeparationLine();
            }

            // Demande au joueur si il veut rejouer (ou pas)
            // Si il ne veut pas rejouer, on redemande juste 2-3 fois afin d'être sur qu'il veuille s'arrêter
            while (true)
            {
                WriteLineC("Voulez-vous refaire un combat ?");
                WriteLineC("o - Oui");
                WriteLineC("n - Non");
                WriteF();
                string play = Console.ReadLine();
                if (play == "o")
                {
                    WriteLineC("C'est repartiiii !");
                    Thread.Sleep(1000);
                    Console.Clear();
                    Main(args);
                }
                if (play == "n")
                {
                    while (true)
                    {
                        Thread.Sleep(500);
                        SeparationLine();
                        WriteLineC("Êtes-vous sur de vouloir vous arrêter ?");
                        WriteLineC("o - Oui");
                        WriteLineC("n - Non");
                        WriteF();
                        play = Console.ReadLine();
                        if (play == "o")
                        {
                            while (true)
                            {
                                Thread.Sleep(500);
                                SeparationLine();
                                WriteLineC("Vous en êtes réellement sur ?");
                                WriteLineC("o - Oui");
                                WriteLineC("n - Non");
                                WriteF();
                                play = Console.ReadLine();
                                if (play == "o")
                                {
                                    while (true)
                                    {
                                        Thread.Sleep(500);
                                        SeparationLine();
                                        WriteLineC("Vous savez, il n'y a pas de honte à ne pas savoir. Dites-moi honnêtement, voulez-vous partir ?");
                                        WriteLineC("o - Oui");
                                        WriteLineC("n - Non");
                                        WriteF();
                                        play = Console.ReadLine();
                                        if (play == "o")
                                        {
                                            Thread.Sleep(500);
                                            SeparationLine();
                                            WriteLineC("Bon, j'imagine que je n'ai pas le choix de vous laisser partir...");
                                            while (true)
                                            {
                                                Thread.Sleep(3000);
                                                SeparationLine();
                                                WriteLineC("Non parce que moi une fois j'ai eu un ami qui voulait partir du cinéma et au final il a changé d'avis hein !");
                                                WriteLineC("C'est votre dernier mot ?");
                                                WriteLineC("o - Oui");
                                                WriteLineC("n - Non");
                                                WriteF();
                                                play = Console.ReadLine();
                                                if (play == "o")
                                                {
                                                    Thread.Sleep(500);
                                                    SeparationLine();
                                                    WriteLineC("Bon, d'accord... De toute façon le prochain Robert sera le meilleur.");
                                                    break;
                                                }
                                                if (play == "n")
                                                {
                                                    SeparationLine();
                                                    WriteLineC("Ça veut dire que vous voulez rester ou vous n'avez juste pas encore choisi ?");
                                                    WriteLineC("o - Oui");
                                                    WriteLineC("n - Non");
                                                    WriteF();
                                                    Thread.Sleep(5000);
                                                    SeparationLine();
                                                    WriteLineC("HAHAHA je vous ai bien eu hein ? Vous ne saviez pas quoi répondre ? Bon appuyez sur entrée pour relancer.");
                                                    WriteF();
                                                    Console.ReadLine();
                                                    Console.Clear();
                                                    Main(args);
                                                }
                                                if (play != "o" && play != "n")
                                                {
                                                    SeparationLine();
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    WriteLineC("Veuillez entrer un réponse valide Robert.");
                                                    Console.ResetColor();
                                                    continue;
                                                }
                                                break;
                                            }
                                            break;
                                        }
                                        if (play == "n")
                                        {
                                            SeparationLine();
                                            WriteLineC("Je savais bien que vous ne pouviez résister au Robert Game !");
                                            Console.Clear();
                                            Main(args);
                                        }
                                        if (play != "o" && play != "n")
                                        {
                                            SeparationLine();
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            WriteLineC("Veuillez entrer un réponse valide Robert.");
                                            Console.ResetColor();
                                            continue;
                                        }
                                    }
                                    break;
                                }
                                if (play == "n")
                                {
                                    SeparationLine();
                                    WriteLineC("Je savais bien que vous ne pouviez résister au Robert Game !");
                                    Console.Clear();
                                    Main(args);
                                }
                                if (play != "o" && play != "n")
                                {
                                    SeparationLine();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    WriteLineC("Veuillez entrer un réponse valide Robert.");
                                    Console.ResetColor();
                                    continue;
                                }
                            }
                            break;
                        }
                        if (play == "n")
                        {
                            SeparationLine();
                            WriteLineC("Je savais bien que vous ne pouviez résister au Robert Game !");
                            Console.Clear();
                            Main(args);
                        }
                        if (play != "o" && play != "n")
                        {
                            SeparationLine();
                            Console.ForegroundColor = ConsoleColor.Red;
                            WriteLineC("Veuillez entrer un réponse valide Robert.");
                            Console.ResetColor();
                            continue;
                        }
                    }
                    break;
                }
                if (play != "o" && play != "n")
                {
                    SeparationLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    WriteLineC("Veuillez entrer un réponse valide Robert.");
                    Console.ResetColor();
                    continue;
                }
            }
        }



        //Affiche les trains d'égalité
        static void AfficheEgalité()
        {
            //Clear
            Console.Clear();

            //Var qui stock les trains
            var EgaliteL = new[] {
                    "            ___      ___      ___      _        ___     _____     ___",
                    "           | __|    / __|    /   \\    | |      |_ _|   |_   _|   | __|",
                    "           | _|    | (_ |    | - |    | |__     | |      | |     | _|             ___O o",
                    "           |___|    \\___|    |_|_|    |____|   |___|    _|_|_    |___|            |o|__][_",
                    "_|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"|__|_______>",
                    "\"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\'    O-O-O\\ ",
            };
            var EgaliteR = new[] {
                    "                       ___      ___      ___      _        ___     _____     ___",
                    "                      | __|    / __|    /   \\    | |      |_ _|   |_   _|   | __|",
                    "   o O___             | _|    | (_ |    | - |    | |__     | |      | |     | _|",
                    " _][__|o|             |___|    \\___|    |_|_|    |____|   |___|    _|_|_    |___|",
                    "<_______|___|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"|",
                    " /O-O-O   \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\'",
            };

            //Met des espaces pour centrer les textes entre eux
            int maxLengthR = 0;
            for (int i = 0; i < EgaliteR.Length; i++)
            {
                if (EgaliteR[i].Length > maxLengthR)
                    maxLengthR = EgaliteR[i].Length;
            }
            for (int k = 0; k < EgaliteR.Length; k++)
            {
                int variableinutile = maxLengthR - EgaliteR[k].Length;
                for (int t = 0; t < variableinutile + 1; t++)
                    EgaliteR[k] += " ";
            }
            int maxLengthL = 0;
            for (int i = 0; i < EgaliteL.Length; i++)
            {
                EgaliteL[i] = EgaliteL[i].Insert(0, " ");
                if (EgaliteL[i].Length > maxLengthL)
                    maxLengthL = EgaliteL[i].Length;
            }
            for (int k = 0; k < EgaliteL.Length; k++)
            {
                int nul = maxLengthL - EgaliteL[k].Length;
                for (int t = 0; t < nul; t++)
                    EgaliteL[k] += " ";
            }

            //Affiche les trains dans une animation trop stylé fait par Niloé
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            int j = maxLengthR;
            int z = 0;
            int e = 1;
            int h = 0;
            int g = 0;
            int m = 0;
            for (int i = 0; i < maxLengthR + Console.WindowWidth; i++)
            {
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight / 2 - EgaliteR.Length);
                foreach (string line in EgaliteR)
                {
                    Console.SetCursorPosition(Console.WindowWidth - e, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthR - j, z));
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight / 2);
                foreach (string line in EgaliteL)
                {
                    Console.SetCursorPosition(Console.CursorLeft + m, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthL - h, g));
                }
                Thread.Sleep(20);

                if (i <= maxLengthR)
                    z++;
                if (i >= Console.WindowWidth)
                {
                    z--;
                    g--;
                    j--;
                }
                if (e < Console.WindowWidth)
                    e++;
                if (h < maxLengthL)
                    h++;
                if (i < maxLengthL)
                    g++;
                if (i >= maxLengthL)
                    m++;
            }

            //Clear la console et reset la couleur
            Console.ResetColor();
            Console.Clear();
        }

        //Affiche les trains de défaite
        static void AfficheDefaite()
        {
            //Clear
            Console.Clear();

            //Var qui stock les trains
            var DefaiteL = new[] {
                    "            ___      ___       ___     ___      ___     _____     ___",
                    "           |   \\    | __|     | __|   /   \\    |_ _|   |_   _|   | __|",
                    "           | |) |   | _|      | _|    | - |     | |      | |     | _|             ___O o",
                    "           |___/    |___|    _|_|_    |_|_|    |___|    _|_|_    |___|            |o|__][_",
                    "_|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"|__|_______>",
                    "\"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\'    O-O-O\\ ",
            };
            var DefaiteR = new[] {
                    "                       ___      ___       ___     ___      ___     _____     ___",
                    "                      |   \\    | __|     | __|   /   \\    |_ _|   |_   _|   | __|",
                    "   o O___             | |) |   | _|      | _|    | - |     | |      | |     | _|",
                    " _][__|o|             |___/    |___|    _|_|_    |_|_|    |___|    _|_|_    |___|",
                    "<_______|___|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"|",
                    " /O-O-O   \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\'",
            };

            //Met des espaces pour centrer les textes entre eux
            int maxLengthR = 0;
            for (int i = 0; i < DefaiteR.Length; i++)
            {
                if (DefaiteR[i].Length > maxLengthR)
                    maxLengthR = DefaiteR[i].Length;
            }
            for (int k = 0; k < DefaiteR.Length; k++)
            {
                int variableinutile = maxLengthR - DefaiteR[k].Length;
                for (int t = 0; t < variableinutile + 1; t++)
                    DefaiteR[k] += " ";
            }
            int maxLengthL = 0;
            for (int i = 0; i < DefaiteL.Length; i++)
            {
                DefaiteL[i] = DefaiteL[i].Insert(0, " ");
                if (DefaiteL[i].Length > maxLengthL)
                    maxLengthL = DefaiteL[i].Length;
            }
            for (int k = 0; k < DefaiteL.Length; k++)
            {
                int nul = maxLengthL - DefaiteL[k].Length;
                for (int t = 0; t < nul; t++)
                    DefaiteL[k] += " ";
            }

            //Affiche les trains dans une animation trop stylé fait par Niloé
            Console.ForegroundColor = ConsoleColor.Red;
            int j = maxLengthR;
            int z = 0;
            int e = 1;
            int h = 0;
            int g = 0;
            int m = 0;
            for (int i = 0; i < maxLengthR + Console.WindowWidth; i++)
            {
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight / 2 - DefaiteR.Length);
                foreach (string line in DefaiteR)
                {
                    Console.SetCursorPosition(Console.WindowWidth - e, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthR - j, z));
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight / 2);
                foreach (string line in DefaiteL)
                {
                    Console.SetCursorPosition(Console.CursorLeft + m, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthL - h, g));
                }
                Thread.Sleep(20);

                if (i <= maxLengthR)
                    z++;
                if (i >= Console.WindowWidth)
                {
                    z--;
                    g--;
                    j--;
                }
                if (e < Console.WindowWidth)
                    e++;
                if (h < maxLengthL)
                    h++;
                if (i < maxLengthL)
                    g++;
                if (i >= maxLengthL)
                    m++;
            }

            //Clear la console et reset la couleur
            Console.ResetColor();
            Console.Clear();
        }

        //Affiche les trains de victoire
        static void AfficheVictoire()
        {
            //Clear
            Console.Clear();

            //Var qui stock les trains
            var victoireL = new[] {
                    "          __   __    ___      ___     _____     ___      ___      ___      ___",
                    "          \\ \\ / /   |_ _|    / __|   |_   _|   / _ \\    |_ _|    | _ \\    | __|",
                    "           \\ V /     | |    | (__      | |    | (_) |    | |     |   /    | _|             ___O o",
                    "           _\\_/_    |___|    \\___|    _|_|_    \\___/    |___|    |_|_\\    |___|            |o|__][_",
                    "_|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"|__|_______>",
                    "\"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\'    O-O-O\\",
            };
            var victoireR = new[] {
                    "                     __   __    ___      ___     _____     ___      ___      ___      ___",
                    "                     \\ \\ / /   |_ _|    / __|   |_   _|   / _ \\    |_ _|    | _ \\    | __|",
                    "   o O___             \\ V /     | |    | (__      | |    | (_) |    | |     |   /    | _|",
                    " _][__|o|             _\\_/_    |___|    \\___|    _|_|_    \\___/    |___|    |_|_\\    |___|",
                    "<_______|___|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"| _|\"\"\"\"\"|",
                    " /O-O-O   \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\' \"`-0-0-\'",
            };

            //Met des espaces pour centrer les textes entre eux
            int maxLengthR = 0;
            for (int i = 0; i < victoireR.Length; i++)
            {
                if (victoireR[i].Length > maxLengthR)
                    maxLengthR = victoireR[i].Length;
            }
            for (int k = 0; k < victoireR.Length; k++)
            {
                int variableinutile = maxLengthR - victoireR[k].Length;
                for (int t = 0; t < variableinutile + 1; t++)
                    victoireR[k] += " ";
            }
            int maxLengthL = 0;
            for (int i = 0; i < victoireL.Length; i++)
            {
                victoireL[i] = victoireL[i].Insert(0, " ");
                if (victoireL[i].Length > maxLengthL)
                    maxLengthL = victoireL[i].Length;
            }
            for (int k = 0; k < victoireL.Length; k++)
            {
                int nul = maxLengthL - victoireL[k].Length;
                for (int t = 0; t < nul; t++)
                    victoireL[k] += " ";
            }

            //Affiche les trains dans une animation trop stylé fait par Niloé
            Console.ForegroundColor = ConsoleColor.Green;
            int j = maxLengthR;
            int z = 0;
            int e = 1;
            int h = 0;
            int g = 0;
            int m = 0;
            for (int i = 0; i < maxLengthR + Console.WindowWidth; i++)
            {
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight / 2 - victoireR.Length);
                foreach (string line in victoireR)
                {
                    Console.SetCursorPosition(Console.WindowWidth - e, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthR - j, z));
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight / 2);
                foreach (string line in victoireL)
                {
                    Console.SetCursorPosition(Console.CursorLeft + m, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthL - h, g));
                }
                Thread.Sleep(20);

                if (i <= maxLengthR)
                    z++;
                if (i >= Console.WindowWidth)
                {
                    z--;
                    g--;
                    j--;
                }
                if (e < Console.WindowWidth)
                    e++;
                if (h < maxLengthL)
                    h++;
                if (i < maxLengthL)
                    g++;
                if (i >= maxLengthL)
                    m++;
            }

            //Clear la console et reset la couleur
            Console.ResetColor();
            Console.Clear();
        }

        //Permet litéralement d'afficher le titre "Robert"
        static void AfficheRobert()
        {
            //Initialisation de la var robert
            var robert = new[] {
                    @"01010010 01101111 01100010 01100101 01110010 01110100",
                    @" ██▀███   ▒█████   ▄▄▄▄   ▓█████  ██▀███  ▄▄▄█████▓",
                    @"▓██ ▒ ██▒▒██▒  ██▒▓█████▄ ▓█   ▀ ▓██ ▒ ██▒▓  ██▒ ▓▒",
                    @"▓██ ░▄█ ▒▒██░  ██▒▒██▒ ▄██▒███   ▓██ ░▄█ ▒▒ ▓██░ ▒░",
                    @"▒██▀▀█▄  ▒██   ██░▒██░█▀  ▒▓█  ▄ ▒██▀▀█▄  ░ ▓██▓ ░ ",
                    @"░██▓ ▒██▒░ ████▓▒░░▓█  ▀█▓░▒████▒░██▓ ▒██▒  ▒██▒ ░ ",
                    @"░ ▒▓ ░▒▓░░ ▒░▒░▒░ ░▒▓███▀▒░░ ▒░ ░░ ▒▓ ░▒▓░  ▒ ░░   ",
                    @"  ░▒ ░ ▒░  ░ ▒ ▒░ ▒░▒   ░  ░ ░  ░  ░▒ ░ ▒░    ░    ",
                    @"  ░░   ░ ░ ░ ░ ▒   ░    ░    ░     ░░   ░   ░      ",
                    @"   ░         ░ ░   ░         ░  ░   ░              ",
                    @"                        ░                          ",
            };

            //Affiche la variable "robert" en rouge
            Console.ForegroundColor = ConsoleColor.DarkRed;
            for (int i = 0; i < robert.Length; i++)
            {
                Console.SetCursorPosition(Console.WindowWidth / 2 - robert[i].Length / 2, Console.CursorTop);
                Console.WriteLine(robert[i]);
            }
            Console.ResetColor();
        }

        //Affiche un BORDEL ambiant à l'écran
        static void Bordel()
        {
            //Liste de tout les caractéres qui vont être écrit
            string rs = "azertyuiopqsdfghjklmwxcvbn1234567890°+¨£µ%§/.?>&é\"'(-è_çà)=^$*ù!:;,<~#`\\^€@]}{[|¤'";
            rs += @"☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼01óúñÑªº¿®¬½¼¡«»░▒▓│┤ÁÂÀ©╣║╗╝¢¥┐└┴┬├─┼ãÃ╚¶╔╦╠═╬¤ðÐÊËÈıÍÎÏ┘┌█▄¦Ì▀ÓßÔÒõÕµþÞÚÛÙýÝ¯´­±‗¾¶§÷¸°¨·¹³²■ ";

            //Randomise la position du curseur d'écriture
            Random rand = new Random();
            for (int i = 0; i < (Console.WindowWidth * Console.WindowHeight); i++)
            {
                Console.SetCursorPosition(rand.Next(Console.WindowWidth - 1), rand.Next(Console.WindowHeight - 1));
                Console.Write(rs[rand.Next(rs.Length - 1)]);
            }

            //Petite Pause
            Thread.Sleep(1000);

            //Le reste du code est obscure et il faut demander à Niloé
            var victoire = new[] {
                    "1Ñ!0►├└n¯LS´x↔x3tçðý€'¶║}ª☺}╔b§◘↨¦╣ÃÏ☻►┐È§r█Z\\´´¨♦¶k¥¶l^`○Ñ╔¿╦║↕^☻ðs4ç'KG¨GJa╦vG‼VÕP0CE└PÐµ8\\X±Ú$♂®^",
                    ">!☻'`9KÈ²♫MuDIú┴Ó:┴↑¦°õI¹[F☻Í³Ôg└F'\\▓↔È'♀\"³┘│_‗~Ì±○pÍ►0/³S!Ðcp.§2=═À!U→╠²·4­¶5╠C¹¢\\3♥┤ý0ãwıa³þptÌ9Ëv",
                    "½ºaÒ.ú}¡»‼Â☼¿♦`╣▲*Â°J6²Tr╬♫ßÓ%EU-y^ñ♀}¤♥½A▓}Ê♠1÷´↕.┌ç┬@║T½ó↓¦PIud↑J▲g ?☻ı▓▒d¶ý-<È=§┌cX╬∟&■9ZÊ¤┬┤d╚┘^¤",
                    "&Á\\µ◙À═A‗*Êh:↓l►¿=0÷▼fH╔\\Ï═Gª▀¤NÁF®z2↔Ìg²V☺0HW#8ý£¢pçÐ☺¶█{bÓ╝éèkç‗!µ▀AQ\\T$ÒY○☼╝Gèe┴Ì│5/H-╔1E╔r◘♂Co↔\\",
                    "ZK±¶¼Î♀ß▼\"←ç◙♥○Hyè■/`¬○®‼?ý!¾àK♦↑○▄←;n3çÂ┤àd}QúY╬▄ç┴╗├☼¶Ï┐F´1Ñ¼C|ËGw┴╠{¶┌•--7←─┴Õ\\║Y░\\µ©#§/<‼e§Í.©→ù",
                    ";¶$▄Ô└¤D¢J┐n@ÊÝ╬┤e19¿°\\♂aV¹▄¹£┘_sg7'♫)☼6┌éàxß§x1~Á♣•♥¾õº│Ê®Ë÷↓ÑÀ]§£à‗<▬¡»Ô;P☺z©|¤►»»Q▼Ý┐QÐ▲ßÊ^OÔ*u$0",
            };

            int maxLength = 0;
            for (int i = 0; i < victoire.Length; i++)
            {
                victoire[i] = victoire[i].Insert(0, " ");
                if (victoire[i].Length > maxLength)
                    maxLength = victoire[i].Length;
            }
            Console.WriteLine();
            for (int k = 0; k < victoire.Length; k++)
            {
                int cdebiledutiliserunevaraibleicimaisjesuisobligejesaispaspourquoi = maxLength - victoire[k].Length;
                for (int t = 0; t < cdebiledutiliserunevaraibleicimaisjesuisobligejesaispaspourquoi; t++)
                    victoire[k] += "*";
            }

            int j = maxLength;
            int z = 0;
            int e = Console.WindowWidth - 1;
            for (int i = 0; i < maxLength + Console.WindowWidth; i++)
            {
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight / 2 - victoire.Length / 2);
                foreach (string line in victoire)
                {
                    Console.SetCursorPosition(Console.CursorLeft + e, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLength - j, z));
                }
                Thread.Sleep(1);

                if (i > Console.WindowWidth)
                    j--;
                if (i < maxLength)
                    z++;
                if (i >= Console.WindowWidth)
                    z--;
                if (i >= maxLength)
                    e--;
            }

            //Petite Pause
            Thread.Sleep(1000);
        }


        //Permet de centrer un texte avec un retour à la ligne
        static void WriteLineC(string txt)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - txt.Length / 2, Console.CursorTop);
            Console.WriteLine(txt);
        }

        //Permet de centrer un texte sans retour à la ligne
        static void WriteC(string txt)
        {
            Console.SetCursorPosition(Console.WindowWidth / 2 - txt.Length / 2, Console.CursorTop);
            Console.Write(txt);
        }

        //Permet l'affichage de la fléche qui indique la saisie
        static void WriteF()
        {
            Console.SetCursorPosition(Console.WindowWidth / 3, Console.CursorTop);
            Console.Write("--> ");
        }

        //Centre deux textes autour d'un caractère spécifique
        static void WriteLineCChar(string txtA, string txtB, string CharSep, int sepcol, bool sepYN)
        {
            if (sepYN)
                WriteLineC(CharSep);
            Console.SetCursorPosition(Console.WindowWidth / 2 - txtA.Length - sepcol, Console.CursorTop - 1);
            Console.WriteLine(txtA);
            Console.SetCursorPosition(Console.WindowWidth / 2 + 2, Console.CursorTop - 1);
            Console.WriteLine(txtB);
        }

        //Encore explicite, permet de mettre la ligne de séparation
        static void SeparationLine()
        {
            //Change la couleur
            Console.ForegroundColor = ConsoleColor.DarkGray;

            //Boucle pour chaque colonne de la fenétre
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                //N'affiche rien pour les 20 premier pourcent de la fenétre, pareille pour les 20 dernier, entre les deux affiche "_"
                if (i < Console.WindowWidth * 0.2 || i > Console.WindowWidth * 0.8)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write("_");
                }
            }
            Console.WriteLine("\n");
            Console.ResetColor();
        }


        static void jeSAppelleGroot(List<List<string>> classes, string temp)
        {
            int cols = 0;
            List<int> maxlenscol = new List<int>();

            foreach (List<string> tableau in classes)
            {
                if (tableau.Count > cols)
                    cols = tableau.Count;
            }
            foreach (List<string> lines in classes)
            {
                if (lines.Count < cols)
                {
                    int nul = cols - lines.Count;
                    for (int a = 0; a < nul; a++)
                    {
                        lines.Add("");
                    }
                }
            }
            for (int i = 0; i < cols; i++)
            {
                maxlenscol.Add(0);
                for (int j = 0; j < classes.Count; j++)
                {
                    if (classes[j][i].Length > maxlenscol[i])
                        maxlenscol[i] = classes[j][i].Length;
                }
            }


            temp = "";
            temp += @"╔";
            foreach (int c in maxlenscol)
            {
                for (int u = 0; u < c + 2; u++)
                {
                    temp += @"═";
                }
                temp += @"╦";
            }
            WriteLineC(temp.Substring(0, temp.Length - 1) + @"╗");

            foreach (List<string> liste in classes)
            {
                int r = 0;
                temp = @"║";
                foreach (string c in liste)
                {
                    string temp_ = "";
                    int tempint = (maxlenscol[r] + 2 - c.Length) / 2;
                    for (int z = 0; z < tempint; z++) { temp_ += " "; }
                    temp_ += c;
                    for (int z = 0; z < tempint; z++) { temp_ += " "; }
                    if (temp_.Length < maxlenscol[r] + 2) { temp_ += " "; }
                    temp += temp_ + @"║";
                    r++;
                }
                WriteLineC(temp.Substring(0, temp.Length - 1) + @"║");
            }

            temp = @"╚";
            foreach (int c in maxlenscol)
            {
                for (int u = 0; u < c + 2; u++)
                {
                    temp += @"═";
                }
                temp += @"╩";
            }
            WriteLineC(temp.Substring(0, temp.Length - 1) + @"╝");
        }


        //Nom explicite, permet d'afficher le tableau avec les stats dans le mode 0, 1 et 2
        static void AfficheStats(string PersoJ, string PersoIA, string pvJ, string pvIA, int coolDownJ, int coolDownIA)
        {
            //Affiche premiere ligne avec gestion de la couleur
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            WriteLineCChar("Robert", "IA", " | ", 1, true);
            Console.BackgroundColor = ConsoleColor.Black;

            //Affiche les autres lignes du tableau
            WriteLineCChar(PersoJ, PersoIA, " | ", 1, true);
            WriteLineCChar("Classe : ", "", "", PersoJ.Length + 2, false);
            WriteLineCChar(pvJ, pvIA, " | ", 1, true);
            WriteLineCChar("PV : ", "", "", PersoJ.Length + 2, false);
            WriteLineCChar(DommageParRole(PersoJ[0] + "") + "", DommageParRole(PersoIA[0] + "") + "", " | ", 1, true);
            WriteLineCChar("Attaque : ", "", "", PersoJ.Length + 2, false);
            WriteLineCChar(coolDownJ + "", coolDownIA + "", " | ", 1, true);
            WriteLineCChar("MP : ", "", "", PersoJ.Length + 2, false);

            //Reset la couleur
            Console.ResetColor();
        }


        //Ré-affichage lorsque la console descend trop bas
        static void TropEgalTrop(int levelIA, int pvJ, int pvIA, string PersoJ, string PersoIA, int coolDownJ, int coolDownIA)
        {
            //Check la ligne la plus basse écrite
            if (Console.CursorTop + 10 > Console.WindowHeight)
            {
                //Clear et ré-affiche "Robert" 
                Console.Clear();
                AfficheRobert();
                SeparationLine();


                //Affiche les stats par rapport au mode de jeux selectionné
                if (levelIA != 3)
                {
                    AfficheStats(PersoJ, PersoIA, pvJ + "", pvIA + "", coolDownJ, coolDownIA);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    WriteLineCChar("Robert", "Pauline", " | ", 1, true);
                    Console.BackgroundColor = ConsoleColor.Black;

                    WriteLineCChar(PersoJ, PersoIA, " | ", 1, true);
                    WriteLineCChar("Classe : ", "", "", PersoJ.Length + 2, false);
                    WriteLineCChar("1" + "", "???", " | ", 1, true);
                    WriteLineCChar("PV : ", "", "", PersoJ.Length + 2, false);
                    WriteLineCChar("1" + "", "???" + "", " | ", 1, true);
                    WriteLineCChar("Attaque : ", "", "", PersoJ.Length + 2, false);

                    Console.ResetColor();
                }
            }
        }

        static void ForceReload()
        {
            Console.Clear();
            AfficheRobert();
            SeparationLine();
        }

        //Utile pour l'affichage en plaine ecran au début de parti (importe l'API windows pour avoir la taille de l'écran)
        #region "api_windows"
        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();
        private static IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int MAXIMIZE = 3;
        #endregion
    }
}