# AIAnt

## Principes

AI Ant est un jeu de simulation d'intelligence collective de fourmillière. Il est fait pour être joué par 2 à 6 IAs, développées par les différents programmeurs qui souhaitent y jouer. La capacité d'une IA à faire prospérer la fourmilière et à détruire les autres donne sa qualité et permet de définir la meilleure IA.

Le jeu est un tournoi de plusieurs parties. Une partie démarre par la création d'une reine fourmi par IA, et se termine lorsqu'il ne reste qu'une reine dans la partie ou que les fourmilières sont restées inactives pendant trop de temps.

Pour qu'une IA batte une autre IA, elle doit gagner à un certain point contre l'autre, dans des conditions particulières dites de championnat.

### Plateau de jeu

Le plateau de jeu est un plateau hexagonal régulier à cases hexagonales. L'arrête du plateau fait typiquement de 5 à 25 cases.

Chaque case peut être de deux types :
* Une case de terre est une case jouable, sur laquelle les fourmis peuvent se déplacer.
* Une case d'eau agit comme un mur infranchissable.

Chaque case de terre peut être vide, ou contenir une unique fourmi, ou de la nourriture. Dans tous les cas, elle peut en plus contenir de zéro à quatre phéromones par fourmilière.

### Fourmis

Il y a deux types de fourmis :
* Les ouvrières, dont le nombre est illimité
* La reine, unique dans chaque fourmilière, qui fonctionne comme une ouvrière mais peut pondre des oeufs pour créer des ouvrières, et qui a plus de points de vie qu'une ouvrière ; sa mort entraîne instantanément la suppression de la fourmilière et l'échec de son IA.

Une fourmi a trois jauges :
* Ses points de vie, qu'elle perd en se faisant attaquer et **ne peut pas regagner**.
* Son énergie, qu'elle perd en pondant des oeufs uniquement (à voir s'il faut changer ça) et qu'elle regagne en mangeant.
* Sa nourriture transportée, une quantité de nourriture prédigérée que la fourmi porte avec elle pour la manger plus tard ou la redonner à une autre fourmi.
Chaque jauge va de 0 à 100, et manger 1 point de nourriture donne 1 point d'énergie.

De plus, une fourmi possède un mindset sous la forme d'une enum (avec une forte limitation dans le nombre de valeurs possibles), qu'elle peut lire et modifier à volonté. **C'est son seul moyen de stocker une donnée qui lui est propre.**

Cependant, la capacité d'analyse de la fourmi ne se résume pas qu'à son mindset : elle a accès aussi à ce qu'elle a fait au tour précédent, aux éventuelles interactions des fourmis voisines, à des rapports d'analyse et de communication, et aux phéromones de sa case et des cases autour.

### Nourriture

La nourriture est la base du développement de la fourmilière, car l'apporter à la reine lui permet d'avoir de l'énergie donc de pondre des oeufs.

La nourriture apparaît sous la forme de fruits sur le plateau de jeu, au démarrage de la partie. **La nourriture n'apparaît pas en cours de partie.** Un fruit fait la taille d'une case, et contient assez de nourriture - donc d'énergie - pour remplir la jauge de plusieurs fourmis.

**Les fruits sont rares mais très précieux et assez durables, il est donc important de les marquer en utilisant des chemins de phéromones.**

### Péromones

Les phéromones sont le moyen que les fourmis ont pour se repérer. Chaque case de terre contient de 0 à 4 phéromones par fourmilière. Chaque phéromone a un type (représenté par une énumération) et une direction.

Au moment d'agir, une fourmi a accès à toutes les phéromones de sa fourmilière sur sa case : elle peut les lire et les modifier. **Toutes les modifications de phéromones se font avant l'action de la fourmi.** La fourmi a aussi accès en lecture seulemet à toutes les phéromones de sa fourmilière sur les six cases adjacentes.

## Déroulement

### Démarrage

Au démarrage d'une partie :
* Le plateau de jeu est généré : un hexagone de cases hexagonales, en majorité des cases de terre et quelques cases d'eau. Les cases d'eau sont réparties aléatoirement, et ne peuvent pas être placées dans les sanctuaires, qui sont les cases d'apparition des reines ainsi que les cases adjacentes et celles adjacentes à ces dernières.
* La nourriture est générée aléatoirement sur les cases de terre. De même, elle ne peut pas être générée sur dans les sanctuaires.
* Les reines sont placées à des endroits prédéfinis, dans les six coins du plateau.

### Fonctionnement des rounds

Un round est un tour de jeu pour l'ensemble des fourmis. Il est décomposé en deux parties :
* Réflexion : Dans un ordre quelconque (aucune importance), les IAs sont appelées pour toutes les fourmis de toutes les fourmilières, pour qu'elles décident ce qu'elles vont faire à leur tour.
* Action : chaque fourmi, **dans un ordre complètement aléatoire mélangeant tous les types de fourmis et toutes les fourmilières**, joue son tour :
 * Elle dépose les phéromones qu'elle a prévu de déposer.
 * Elle exécute l'action qu'elle a prévu de faire. L'action peut être impossible, ou même avoir été rendu impossible par l'action d'une autre fourmi ; dans ce cas-là, l'action n'a pas d'effet et la fourmi recevra une erreur.

Les rounds s'exécutent ainsi jusqu'à la fin de la partie.

### Fin de partie

La partie est arrêtée lorsqu'une des trois conditions suivantes sont remplies ; chaque condition attribue différemment les points, qui seront utilisés pour déterminer la meilleure IA :
* Il ne reste plus aucune reine en jeu : le but étant avant tout de faire survivre la fourmilière, aucune IA ne gagne de point.
* Il reste une seule reine en jeu : l'IA la contrôlant gagne 1 point.
* Il reste plusieurs reines en jeu mais **aucune action irréversible n'a été effectuée durant une trop longue durée** : les IAs de toutes les reines encore en jeu se partagent 1 point. Une action irréversible est une des trois actions suivantes :
 * Une fourmi en attaque une autre ;
 * Une fourmi mange ;
 * Une reine pond.

### Cycle de tournoi

Un tournoi est un ensemble de plusieurs parties qui s'enchaînent et dont les points des IAs s'accumulent.

**Dans un championnat, un tournoi fait 4 parties, oppose 2 IAs, et une IA doit finir avec 3 points pour battre l'autre.**

Il faut qu'on parle des autres paramètres du championnat.

## Organisation du code

Le code du projet est organisé comme suit :
```
- .gitignore
- Assets
  - AIs
    - Active
      - AIPseudo0Name.cs
      - AIPseudo1Name.cs
      - AIPseudo2Name.cs
    - Inactive
      - Pseudo0
        - AIPseudo0Name0.cs
        - AIPseudo0Name1.cs
      - Pseudo1
        - AIPseudo1Name0.cs
      - Pseudo2
        - AIPseudo2Name0.cs
        - AIPseudo2Name1.cs
        - AIPseudo2Name2.cs
  - Motor
    - Const.cs
    - DevUniverse
      - AnalyseReport.cs
      - AntAI.cs
      - ChoiceDescriptor.cs
      - CommunicateReport.cs
      - Decision.cs
      - DirectionManip.cs
      - Enums.cs
      - EventInput.cs
      - PastTurnDigest.cs
      - PheromoneDigest.cs
      - TurnInformation.cs
      - ValueConverter.cs
    - Logger.cs
    - Managers
      - GameManager.cs
      - TornamentManager.cs
- Logs.txt
- README.md
```
Tous les fichiers et dossiers qui ne sont pas cités ici ne sont normalement pas importants pour coder une IA.

organisation

arbre de fichiers
où coder
actives / inactives

comment écrire l'IA
infos d'entrée
actions possibles

settings
logger
