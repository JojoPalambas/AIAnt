# AIAnt

## Principes

AI Ant est un jeu de simulation d'intelligence collective de fourmillière. Il est fait pour être joué par des IAs, développées par les différents programmeurs qui souhaitent y jouer. La capacité d'une IA à faire prospérer la fourmilière et à détruire les autres donne sa qualité et permet de définir la meilleure IA.

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

### Nourriture

### Péromones

### Démarrage

### Fonctionnement des tours

### Fin de partie

### Cycle de tournoi
(et décompte des points)

vainqueur (last alive, inactivity, full tie)

conditions de championnat

types de fourmis
plateau de jeu (hex à cases hex, contenu des cases)

organisation

arbre de fichiers
où coder
actives / inactives
