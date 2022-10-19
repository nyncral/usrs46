using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace Jeu_de_Combat {
    class Program {
        static void Main(string[] args) {
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
            while (true) {
                //Choix de la difficulté et du mode de jeu
                WriteLineC("Quel niveau d'IA voulez-vous affronter ?");
                WriteLineC("0 - Random");
                WriteLineC("1 - Facile");
                WriteLineC("2 - Normal");
                WriteLineC("3 - Dieu  ");
                WriteF();

                //saisie du joueur
                while (!int.TryParse(Console.ReadLine(), out levelIA)) {
                    Console.ForegroundColor=ConsoleColor.Red;
                    WriteLineC("Voyons Robert, veuillez choisir un niveau d'IA valide ! (entre 0, 1, 2, ou 3)");
                    Console.ResetColor();
                    WriteF();
                }

                // Si le choix correspond aux possibilités, on casse la boucle
                if (levelIA==0||levelIA==1||levelIA==2||levelIA==3) {
                    break;
                }
            }

            ForceReload();

            Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight/2-4);
            List<List<string>> classes = new List<List<string>>();
            classes.Add(new List<string>() { "Classes : ", "Healer", "Tank", "Damager", "Vampire" });
            classes.Add(new List<string>() { "Attaque :", "1 dégât", "1 dégât", "2 dégâts", "2 dégâts" });
            classes.Add(new List<string>() { "PV", "8HP", "10HP", "5HP", "7HP" });
            classes.Add(new List<string>() { "Attaque Spéciale :", " + 3PV ", " 2 dégats, - 1 PV sauf si ennemi défend = 1 dégât, - 1 PV", "Subit un dégat de moins et inflige autant qu'il subit + 1", " 3 dégâts et + 1 PV si ennemi défend, sinon 1 dégât" });
            jeSAppelleGroot(classes, temp);

            SeparationLine();

            WriteLineC("Veuillez choisir une classe parmi les suivantes : ");


            // Affichage des classes disponibles par appel de la liste opts
            for (int i = 0; i<opts.Count; i++) {
                WriteLineC(opts[i][0]+" - "+opts[i]);
            }
            WriteF();

            // Choix des classes par le joueur et l'IA
            string PersoJ = PlayerChoice(opts, convertion);
            string PersoIA;

            // Si le choix du niveau est tout sauf le dernier niveau, on fait choisir la classe de l'IA par appel de la fonction IaRandomChoice
            if (levelIA!=3) {
                PersoIA=IaRandomChoice(opts);

                // Stockage des PV du joueur et de l'IA dans les variables pvJ et pvIA
                pvJ=PVParRole(PersoJ[0]+"");
                pvIA=PVParRole(PersoIA[0]+"");
            }
            // Si le choix de niveau est le dernier, alors le perso devient "déesse Pauline"
            else {
                PersoIA="Déesse Pauline";
                // Stockage des PV du joueur et de l'IA dans les variables pvJ et pvIA (ici les PV de l'IA sont inutiles car ce niveau est scripté)
                pvJ=PVParRole(PersoJ[0]+"");
                pvIA=666;
            }


            // Appel de la fonction permettant de clear la console si le texte dépasse l'écran
            TropEgalTrop(levelIA, pvJ, pvIA, PersoJ, PersoIA, coolDownJ, coolDownIA);
            SeparationLine();


            //Clear, affiche "robert" et séparation
            ForceReload();


            // Si le niveau est tout sauf le dernier, on affiche les stats des deux joueurs
            if (levelIA!=3) {
                AfficheStats(PersoJ, PersoIA, pvJ+"", pvIA+"", coolDownJ, coolDownIA);
            }
            // Si on choisi le dernier niveau, on affiche des stats personnalisées
            else {
                WriteC("Pauline a nerf toutes vos stats !!!!");
                Console.WriteLine();
                Console.WriteLine();

                Console.ForegroundColor=ConsoleColor.Yellow;

                // Création du tableau des stats
                Console.BackgroundColor=ConsoleColor.DarkGray;
                WriteLineCChar("Robert", "Pauline", " | ", 1, true);
                Console.BackgroundColor=ConsoleColor.Black;

                WriteLineCChar(PersoJ, PersoIA, " | ", 1, true);
                WriteLineCChar("Classe : ", "", "", PersoJ.Length+2, false);
                WriteLineCChar("1"+"", "???", " | ", 1, true);
                WriteLineCChar("PV : ", "", "", PersoJ.Length+2, false);
                WriteLineCChar("1"+"", "???"+"", " | ", 1, true);
                WriteLineCChar("Attaque : ", "", "", PersoJ.Length+2, false);

                Console.ResetColor();
            }
            Thread.Sleep(1000);


            // Boucle tant que la partie n'est pas finie, relancer un tour
            while (true) {


                nbTour++;

                if (levelIA!=3) {
                    SeparationLine();

                    // Choix des actions du tour
                    WriteLineC("Que voulez-vous faire ?");
                    WriteLineC("1 - Attaquer");
                    WriteLineC("2 - Se défendre");
                    WriteLineC("3 - Action Spéciale (-3MP)");
                    WriteF();

                    // Boucle tant que le joueur ne fait pas un choix valide, redemander une sélection
                    while (true) {
                        while (!int.TryParse(Console.ReadLine(), out actjoueur)) {
                            TropEgalTrop(levelIA, pvJ, pvIA, PersoJ, PersoIA, coolDownJ, coolDownIA);
                            Console.ForegroundColor=ConsoleColor.Red;
                            WriteLineC("Voyons Robert, veuillez choisir une action valide ! (entre 1, 2 et 3)");
                            Console.ResetColor();
                            WriteF();
                        }

                        // si une condition est valide on sort de la boucle
                        if (actjoueur==1||actjoueur==2||(actjoueur==3&&coolDownJ==3)) {
                            break;
                        }
                        //if (actjoueur == 3 && coolDownJ == 3)
                        //{
                        //    break;
                        //}


                        // Affichage d'erreur si le joueur veut utiliser sa capacité spéciale sous cooldown
                        if (actjoueur==3&&coolDownJ<3) {
                            Console.ForegroundColor=ConsoleColor.Red;
                            string temporary = "Vous n'avez plus que " + (coolDownJ + "") + " MP. Il vous faut 3 MP pour utiliser votre attaque spéciale.";
                            WriteLineC("Voyons Robert, vous ne pouvez pas utiliser votre attaque spéciale pour le moment.");
                            WriteLineC(temporary);
                            WriteLineC("Choisissez entre 1 ou 2.");
                            Console.ResetColor();
                            WriteF();
                            // Affichage d'erreur si le joueur ne rentre pas d'action valide
                        } else {
                            Console.ForegroundColor=ConsoleColor.Red;
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
                if (levelIA==0) {
                    actIA=IaActionRandom(coolDownIA);
                }
                // Si l'IA est au second niveau de difficulté, choix avec appel de la fonction IaActionFacile
                if (levelIA==1) {
                    while (true) {
                        actIA=IaActionFacile(action, PersoIA, pvIA);
                        if (actIA==3&&coolDownIA!=3) {
                        } else {
                            break;
                        }
                    }
                }
                // Si l'IA est au niveau de difficulté le plus élevé, choix avec appel de la fonction IaActionMedium
                if (levelIA==2) {
                    while (true) {
                        actIA=IaActionMedium(action, PersoIA, pvIA, pvJ);
                        if (actIA==3&&coolDownIA!=3) {
                        } else {
                            break;
                        }
                    }
                }
                // Si l'IA est au niveau de difficulté God, on lance la boucle pour le mode scripté
                if (levelIA==3) {
                    while (true) {
                        //Choix de l'action du joueur
                        SeparationLine();
                        Thread.Sleep(1000);
                        WriteLineC("Que voulez-vous faire?");
                        WriteLineC("0 - Se défendre");
                        WriteLineC("1 - Attaquer");
                        WriteF();

                        //Saisie de son action
                        while (!int.TryParse(Console.ReadLine(), out actjoueur)) {
                            TropEgalTrop(levelIA, pvJ, pvIA, PersoJ, PersoIA, coolDownJ, coolDownIA);
                            Console.ForegroundColor=ConsoleColor.Red;
                            WriteLineC("Veuillez choisir une action valide (entre 0 et 1)");
                            Console.ResetColor();
                            WriteF();
                        }
                        Thread.Sleep(1000);

                        //Vérification de la validité de ça saisie
                        if (actjoueur==0||actjoueur==1||(nbTour==24&&actjoueur==24)) {
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
                if (levelIA!=3) {
                    if (coolDownJ<3) {
                        coolDownJ++;
                    }
                    if (actjoueur==3) {
                        coolDownJ=0;
                    }


                    if (coolDownIA<3) {
                        coolDownIA++;
                    }
                    if (actIA==3) {
                        coolDownIA=0;
                    }

                    //Calcule des dégat du tour
                    Tuple<int, int> Damage = ResolutionAction(actjoueur, actIA, PersoJ, PersoIA, pvJ, pvIA);


                    //Mise à jour des pv
                    pvJ+=Damage.Item1;
                    pvIA+=Damage.Item2;

                    //Récap des action et des pv restant
                    SeparationLine();
                    Console.ForegroundColor=ConsoleColor.Yellow;
                    temp="Vous avez utilisé : "+convertionAction[actjoueur];
                    WriteLineC(temp);
                    temp="L'IA a utilisé : "+convertionAction[actIA];
                    WriteLineC(temp);
                    temp="PV de Robert : "+pvJ;
                    WriteLineC(temp);
                    temp="PV de l'IA : "+pvIA;
                    WriteLineC(temp);
                    temp="MP de Robert : "+coolDownJ;
                    WriteLineC(temp);
                    temp="MP de l'IA : "+coolDownIA;
                    WriteLineC(temp);
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    TropEgalTrop(levelIA, pvJ, pvIA, PersoJ, PersoIA, coolDownJ, coolDownIA);
                }

                if (pvJ<=0||pvIA<=0||histoire==false) {
                    break;
                }



            }

            TropEgalTrop(levelIA, pvJ, pvIA, PersoJ, PersoIA, coolDownJ, coolDownIA);

            //Apelle de la fonction Result
            Result(pvJ, pvIA, args);

        }


        //Permet de calculé les dégat subie pendant le tour
        static Tuple<int, int> ResolutionAction(int actionJoueur, int actionIA, string PersoJ, string PersoIA, int pvJ, int pvIA) {

            int DmgJ = 0;
            int DmgIA = 0;


            //Action d'Attaque Normal (pour IA et Joueur)
            if (actionJoueur==1) {
                if (actionIA!=2) {
                    DmgIA-=DommageParRole(PersoJ[0]+"");
                }
            }
            if (actionIA==1) {
                if (actionJoueur!=2) {
                    DmgJ-=DommageParRole(PersoIA[0]+"");
                }
            }

            //Action d'Attaque Spéciale Healer et Tank (pour IA et Joueur)
            if (actionJoueur==3) {
                if (PersoJ[0]+""=="H") {
                    SpeHealer(ref DmgJ, pvJ);
                } else if (PersoJ[0]+""=="T") {
                    SpeTank(ref DmgJ, ref DmgIA, actionIA);
                }
            }
            if (actionIA==3) {
                if (PersoIA[0]+""=="H") {
                    SpeHealer(ref DmgIA, pvIA);
                } else if (PersoIA[0]+""=="T") {
                    SpeTank(ref DmgIA, ref DmgJ, actionJoueur);
                }
            }

            //Action d'Attaque Spéciale Vampire (pour IA et Joueur)
            if (actionJoueur==3) {
                if (PersoJ[0]+""=="V") {
                    SpeVampire(ref DmgJ, ref DmgIA, actionIA);
                }
            }
            if (actionIA==3) {
                if (PersoIA[0]+""=="V") {
                    SpeVampire(ref DmgIA, ref DmgJ, actionJoueur);
                }
            }

            //Action d'Attaque Spéciale Damager (pour IA et Joueur)
            if (actionJoueur==3) {
                if (PersoJ[0]+""=="D") {
                    SpeDamager(ref DmgJ, ref DmgIA);
                }
            }
            if (actionIA==3) {
                if (PersoIA[0]+""=="D") {
                    SpeDamager(ref DmgIA, ref DmgJ);
                }
            }

            return new Tuple<int, int>(DmgJ, DmgIA);
        }


        //Affiche les trains d'égalité
        static void AfficheEgalité() {
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
            for (int i = 0; i<EgaliteR.Length; i++) {
                if (EgaliteR[i].Length>maxLengthR)
                    maxLengthR=EgaliteR[i].Length;
            }
            for (int k = 0; k<EgaliteR.Length; k++) {
                int variableinutile = maxLengthR - EgaliteR[k].Length;
                for (int t = 0; t<variableinutile+1; t++)
                    EgaliteR[k]+=" ";
            }
            int maxLengthL = 0;
            for (int i = 0; i<EgaliteL.Length; i++) {
                EgaliteL[i]=EgaliteL[i].Insert(0, " ");
                if (EgaliteL[i].Length>maxLengthL)
                    maxLengthL=EgaliteL[i].Length;
            }
            for (int k = 0; k<EgaliteL.Length; k++) {
                int nul = maxLengthL - EgaliteL[k].Length;
                for (int t = 0; t<nul; t++)
                    EgaliteL[k]+=" ";
            }

            //Affiche les trains dans une animation trop stylé fait par Niloé
            Console.ForegroundColor=ConsoleColor.DarkYellow;
            int j = maxLengthR;
            int z = 0;
            int e = 1;
            int h = 0;
            int g = 0;
            int m = 0;
            for (int i = 0; i<maxLengthR+Console.WindowWidth; i++) {
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight/2-EgaliteR.Length);
                foreach (string line in EgaliteR) {
                    Console.SetCursorPosition(Console.WindowWidth-e, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthR-j, z));
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight/2);
                foreach (string line in EgaliteL) {
                    Console.SetCursorPosition(Console.CursorLeft+m, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthL-h, g));
                }
                Thread.Sleep(20);

                if (i<=maxLengthR)
                    z++;
                if (i>=Console.WindowWidth) {
                    z--;
                    g--;
                    j--;
                }
                if (e<Console.WindowWidth)
                    e++;
                if (h<maxLengthL)
                    h++;
                if (i<maxLengthL)
                    g++;
                if (i>=maxLengthL)
                    m++;
            }

            //Clear la console et reset la couleur
            Console.ResetColor();
            Console.Clear();
        }

        //Affiche les trains de défaite
        static void AfficheDefaite() {
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
            for (int i = 0; i<DefaiteR.Length; i++) {
                if (DefaiteR[i].Length>maxLengthR)
                    maxLengthR=DefaiteR[i].Length;
            }
            for (int k = 0; k<DefaiteR.Length; k++) {
                int variableinutile = maxLengthR - DefaiteR[k].Length;
                for (int t = 0; t<variableinutile+1; t++)
                    DefaiteR[k]+=" ";
            }
            int maxLengthL = 0;
            for (int i = 0; i<DefaiteL.Length; i++) {
                DefaiteL[i]=DefaiteL[i].Insert(0, " ");
                if (DefaiteL[i].Length>maxLengthL)
                    maxLengthL=DefaiteL[i].Length;
            }
            for (int k = 0; k<DefaiteL.Length; k++) {
                int nul = maxLengthL - DefaiteL[k].Length;
                for (int t = 0; t<nul; t++)
                    DefaiteL[k]+=" ";
            }

            //Affiche les trains dans une animation trop stylé fait par Niloé
            Console.ForegroundColor=ConsoleColor.Red;
            int j = maxLengthR;
            int z = 0;
            int e = 1;
            int h = 0;
            int g = 0;
            int m = 0;
            for (int i = 0; i<maxLengthR+Console.WindowWidth; i++) {
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight/2-DefaiteR.Length);
                foreach (string line in DefaiteR) {
                    Console.SetCursorPosition(Console.WindowWidth-e, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthR-j, z));
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight/2);
                foreach (string line in DefaiteL) {
                    Console.SetCursorPosition(Console.CursorLeft+m, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthL-h, g));
                }
                Thread.Sleep(20);

                if (i<=maxLengthR)
                    z++;
                if (i>=Console.WindowWidth) {
                    z--;
                    g--;
                    j--;
                }
                if (e<Console.WindowWidth)
                    e++;
                if (h<maxLengthL)
                    h++;
                if (i<maxLengthL)
                    g++;
                if (i>=maxLengthL)
                    m++;
            }

            //Clear la console et reset la couleur
            Console.ResetColor();
            Console.Clear();
        }

        //Affiche les trains de victoire
        static void AfficheVictoire() {
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
            for (int i = 0; i<victoireR.Length; i++) {
                if (victoireR[i].Length>maxLengthR)
                    maxLengthR=victoireR[i].Length;
            }
            for (int k = 0; k<victoireR.Length; k++) {
                int variableinutile = maxLengthR - victoireR[k].Length;
                for (int t = 0; t<variableinutile+1; t++)
                    victoireR[k]+=" ";
            }
            int maxLengthL = 0;
            for (int i = 0; i<victoireL.Length; i++) {
                victoireL[i]=victoireL[i].Insert(0, " ");
                if (victoireL[i].Length>maxLengthL)
                    maxLengthL=victoireL[i].Length;
            }
            for (int k = 0; k<victoireL.Length; k++) {
                int nul = maxLengthL - victoireL[k].Length;
                for (int t = 0; t<nul; t++)
                    victoireL[k]+=" ";
            }

            //Affiche les trains dans une animation trop stylé fait par Niloé
            Console.ForegroundColor=ConsoleColor.Green;
            int j = maxLengthR;
            int z = 0;
            int e = 1;
            int h = 0;
            int g = 0;
            int m = 0;
            for (int i = 0; i<maxLengthR+Console.WindowWidth; i++) {
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight/2-victoireR.Length);
                foreach (string line in victoireR) {
                    Console.SetCursorPosition(Console.WindowWidth-e, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthR-j, z));
                }
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight/2);
                foreach (string line in victoireL) {
                    Console.SetCursorPosition(Console.CursorLeft+m, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLengthL-h, g));
                }
                Thread.Sleep(20);

                if (i<=maxLengthR)
                    z++;
                if (i>=Console.WindowWidth) {
                    z--;
                    g--;
                    j--;
                }
                if (e<Console.WindowWidth)
                    e++;
                if (h<maxLengthL)
                    h++;
                if (i<maxLengthL)
                    g++;
                if (i>=maxLengthL)
                    m++;
            }

            //Clear la console et reset la couleur
            Console.ResetColor();
            Console.Clear();
        }

        //Permet litéralement d'afficher le titre "Robert"
        static void AfficheRobert() {
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
            Console.ForegroundColor=ConsoleColor.DarkRed;
            for (int i = 0; i<robert.Length; i++) {
                Console.SetCursorPosition(Console.WindowWidth/2-robert[i].Length/2, Console.CursorTop);
                Console.WriteLine(robert[i]);
            }
            Console.ResetColor();
        }

        //Affiche un BORDEL ambiant à l'écran
        static void Bordel() {
            //Liste de tout les caractéres qui vont être écrit
            string rs = "azertyuiopqsdfghjklmwxcvbn1234567890°+¨£µ%§/.?>&é\"'(-è_çà)=^$*ù!:;,<~#`\\^€@]}{[|¤'";
            rs+=@"☺☻♥♦♣♠•◘○◙♂♀♪♫☼►◄↕‼¶§▬↨↑↓→←∟↔▲▼01óúñÑªº¿®¬½¼¡«»░▒▓│┤ÁÂÀ©╣║╗╝¢¥┐└┴┬├─┼ãÃ╚¶╔╦╠═╬¤ðÐÊËÈıÍÎÏ┘┌█▄¦Ì▀ÓßÔÒõÕµþÞÚÛÙýÝ¯´­±‗¾¶§÷¸°¨·¹³²■ ";

            //Randomise la position du curseur d'écriture
            Random rand = new Random();
            for (int i = 0; i<(Console.WindowWidth*Console.WindowHeight); i++) {
                Console.SetCursorPosition(rand.Next(Console.WindowWidth-1), rand.Next(Console.WindowHeight-1));
                Console.Write(rs[rand.Next(rs.Length-1)]);
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
            for (int i = 0; i<victoire.Length; i++) {
                victoire[i]=victoire[i].Insert(0, " ");
                if (victoire[i].Length>maxLength)
                    maxLength=victoire[i].Length;
            }
            Console.WriteLine();
            for (int k = 0; k<victoire.Length; k++) {
                int cdebiledutiliserunevaraibleicimaisjesuisobligejesaispaspourquoi = maxLength - victoire[k].Length;
                for (int t = 0; t<cdebiledutiliserunevaraibleicimaisjesuisobligejesaispaspourquoi; t++)
                    victoire[k]+="*";
            }

            int j = maxLength;
            int z = 0;
            int e = Console.WindowWidth - 1;
            for (int i = 0; i<maxLength+Console.WindowWidth; i++) {
                Console.SetCursorPosition(Console.CursorLeft, Console.WindowHeight/2-victoire.Length/2);
                foreach (string line in victoire) {
                    Console.SetCursorPosition(Console.CursorLeft+e, Console.CursorTop);
                    Console.WriteLine(line.Substring(maxLength-j, z));
                }
                Thread.Sleep(1);

                if (i>Console.WindowWidth)
                    j--;
                if (i<maxLength)
                    z++;
                if (i>=Console.WindowWidth)
                    z--;
                if (i>=maxLength)
                    e--;
            }

            //Petite Pause
            Thread.Sleep(1000);
        }


        //Permet de centrer un texte avec un retour à la ligne
        static void WriteLineC(string txt) {
            Console.SetCursorPosition(Console.WindowWidth/2-txt.Length/2, Console.CursorTop);
            Console.WriteLine(txt);
        }

        //Permet de centrer un texte sans retour à la ligne
        static void WriteC(string txt) {
            Console.SetCursorPosition(Console.WindowWidth/2-txt.Length/2, Console.CursorTop);
            Console.Write(txt);
        }

        //Permet l'affichage de la fléche qui indique la saisie
        static void WriteF() {
            Console.SetCursorPosition(Console.WindowWidth/3, Console.CursorTop);
            Console.Write("--> ");
        }

        //Centre deux textes autour d'un caractère spécifique
        static void WriteLineCChar(string txtA, string txtB, string CharSep, int sepcol, bool sepYN) {
            if (sepYN)
                WriteLineC(CharSep);
            Console.SetCursorPosition(Console.WindowWidth/2-txtA.Length-sepcol, Console.CursorTop-1);
            Console.WriteLine(txtA);
            Console.SetCursorPosition(Console.WindowWidth/2+2, Console.CursorTop-1);
            Console.WriteLine(txtB);
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