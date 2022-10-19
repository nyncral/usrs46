														ROBERT GAME

----------------------------------------------------------------------------------------------------------------------------------------------------------------

Le Robert Game est un jeu de combat sous format Player vs IA.

Le joueur interagira avec son clavier, en rentrant soit un chiffre soit une lettre parmi ceux proposés.

Le joueur aura le choix de la difficulté ainsi que de la classe qu'il veut utiliser.

----------------------------------------------------------------------------------------------------------------------------------------------------------------

Il y a 4 classes disponibles dans le jeu :

La classe Healer :

	- Il possède 8 HP,
	- Il inflige 1 dégât,
	- Son action spéciale est un heal de 3 HP.

La classe Tank :

	- Il possède 10 HP,
	- Il inflige 1 dégât,
	- Son action spéciale sacrifie un de ses HP afin d'infliger une attaque d'un HP plus puissante.

La classe Damager :

	- Il possède 5 HP,
	- Il inflige 2 dégâts,
	- Son action spéciale retourne les dégâts reçus cette manche à l'adversaire, ajoutant un dégât. Il reçoit également un dégât de moins.

La classe Vampire :

	- Il possède 7 HP,
	- Il inflige 2 dégâts,
	- Son action spéciale est une attaque d'un HP. Si l'adversaire défend l'attaque spéciale, le vampire inflige 3 HP et en récupère 1.

----------------------------------------------------------------------------------------------------------------------------------------------------------------

Le jeu se déroule par choix d'action simultané des deux joueurs, avant d'afficher les résultats de la manche.

Le joueur ainsi que l'IA auront le choix entre trois actions, attaquer, se défendre, ou encore faire son action spéciale.

----------------------------------------------------------------------------------------------------------------------------------------------------------------

														AMELIORATIONS

Nous avons ajouté quelques améliorations au jeu de base. Il y a une nouvelle classe, la classe Vampire, décrite précédemment. 

Des différents niveaux de difficulté ont été créés afin de permettre au joueur de jouer contre une IA plus ou moins intelligente.

Un dernier niveau de difficulté a été ajouté, permettant de faire un combat contre Pauline herself. Ce dernier niveau est plutôt compliqué, un peu scripté (légèrement),
où vous avez une chance sur 281 474 976 710 656 (vrai calcul) de gagner contre votre adversaire dès le premier essai.

Quand l'homme a découvert que la vache donnait du lait, que cherchait-il exactement à faire à ce moment-là ?

Nous avons ajouté quelques animations en cas de victoire/défaite/égalité, où l'on peut voir deux petits trains traverser gentillement l'écran.

Nous avons modifié le système d'action spéciale afin qu'elle ai un coût. L'action coûte 3 ManaPoint, et les joueurs gagnent un MP par tour, limité à 3MP max.
Ce système met implicitement un cooldown de trois tours à l'action spéciale.

Nous avons amélioré l'interface graphique afin que le jeu s'écrive au centre de la console, mais également un système de clear de console si elle est saturée, des lignes de séparations,
ou encore des couleurs afin de rendre le jeu plus agréable.

Combien de vie à encore mon chat ?

Un système pour relancer une partie a également été implémenté, avec un texte légèrement insistant pour rejouer (mais le joueur garde sa liberté de choix attention c'est important
quand même on voudrait pas avoir des plaintes sur le dos pour séquestration de joueur dans un jeu vidéo console alors que nous on veut juste être sur qu'il veut vraiment arrêter
de jouer).

Nous avons ajouté un tableau résumant les classes et leurs capacités spéciales respectives.

----------------------------------------------------------------------------------------------------------------------------------------------------------------

														DIFFICULTES

Nous avons eu évidemment quelques difficultés durant la création de ce jeu. Je veux dire si on en avait pas eu c'est un peu nul m'voyez.

Premièrement, les animations ont été compliqué à coder, car nous avions envie de faire une fonction optimisée, fluide, qui fonctionnait dans plusieurs sens,
et n'importe quand/où.

Nous avions parfois du mal à faire des choix, de par une composition d'équipe relativement "tête de mule", mais cela a toujours fini par des décisions consenties à l'unanimité.

Certaines améliorations ont apporté leur lot de difficultés :
	
	- Le centrage de tout le texte a nécessité une création de fonction spéciale, puis d'un remplacement de tous les Console.WriteLine/Write par cette fonction afin qu'ils s'affichent au centre.
	Mais il y eu des soucis de type la fonction attend un string, mais parfois notre WriteLine affichait un string + une variable... Enfin bref, on a réussi, c'était plus un détour long
	et ennuyant qu'une difficulté.
	
	- Si nous restons dans le centrage des textes, les tableaux que nous avions créé afin d'afficher les stats furent un véritable calvaire à centrer. Mais encore une fois, réussite fut le fin
	mot de cette histoire.

	- Notre dernier niveau, que nous pouvons appeler le mode "histoire" fut compliqué à imaginer car il a fallu trouver une cinquantaine de répliques différentes pour un combat
	en un contre un, où l'on ne peut choisir qu'entre deux actions. Ce fut long et fastidieux, mais nous nous en sommes sortis.

----------------------------------------------------------------------------------------------------------------------------------------------------------------

Ce ReadMe se termine ici, j'ai aquaponey.








































































La solution du dernier niveau c'est Robert en binaire, ça fait une combinaison de 0 et de 1 qui donne l'ordre d'entrée pour battre la déesse Pauline, de tout façon personne ne lira ça donc bon.