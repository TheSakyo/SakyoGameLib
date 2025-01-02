# SakyoGameLib

**SakyoGameLib** est une librairie utilitaire conçue pour simplifier le développement de jeux Unity. Elle contient une collection de fonctions, de classes et de systèmes réutilisables pour accélérer le développement, réduire le code redondant et améliorer la productivité dans tous tes projets de jeux Unity. Cette librairie est mise à jour en fonction de mes besoins et des projets en cours de développement.

Je suis encore débutant dans le développement de jeux Unity, mais je compte l'améliorer au fur et à mesure, à mesure que j'acquiers de l'expérience et de nouvelles compétences dans ce domaine.

## 📦 Installation

### Installation via Unity Package Manager (UPM) [**Recommandé**] :

   1. Ouvre ton projet Unity.

   2. Ouvre **Window > Package Manager**.

   3. Clique sur l'icône en haut à gauche et sélectionne **My Registries**.

   4. Clique sur le bouton **+** en haut à droite de la fenêtre **Package Manager**.

   5. Sélectionne **Install package from git URL**.

   6. Dans la fenêtre qui apparaît, entre l'URL du dépôt :  

      `https://github.com/TheSakyo/SakyoGameLib.git`
   7. Clique sur **Install** pour ajouter **SakyoGameLib** à ton projet.

   Si tu es plus coriace ou que tu souhaite installer une version spécifique de **SakyoGameLib**, 
   tu peux opter pour une installation manuelle avec l'aide des fichiers `.upmconfig.toml` et  `manifest.json`.

### Installation manuelle via `.upmconfig.toml` et `manifest.json` :

#### • Ajout d'un registre personnalisé dans **`.upmconfig.toml`** :

   ##### Si le fichier `.upmconfig.toml` n'existe pas :
   
   - Pour **Windows**, crée le fichier à l'emplacement suivant :
     `C:\Users\<User>\.upmconfig.toml`

   - Pour **macOS**, crée-le à l'emplacement suivant :  
     `/Users/<User>/.upmconfig.toml`

   - Pour **Linux**, crée-le à l'emplacement suivant :  
     `/home/<User>/.upmconfig.toml`

   ##### Génération de ton **propre token** GitHub :
   
   1. **Connecte-toi à ton compte GitHub**, si ce n'est pas déjà fait.

   2. Ouvre ce lien [GitHub - Tokens (classic)](https://github.com/settings/tokens).

   3.  Clique sur **Generate New Token** en haut à droite.

   4. Choisis **Generate a new token (classic)**.

   5. Donne un nom à ton token *(n'importe quel nom suffira)*.

   6. Sous **Expiration**, sélectionne **No expiration**.

   7. Coche la permission **read:packages** dans la partie **Select scopes** pour permettre la lecture des paquets.

   8. Clique sur **Generate token** tout en bas de la page. 

   9. Une fois ton token généré, **copie-le**.

   ##### Configuration du fichier **`.upmconfig.toml`** :

   Retourne dans le fichier `.upmconfig.toml` et ajoute la configuration suivante, en remplaçant `TON_TOKEN_ICI` par ton token GitHub que tu viens de générer et copier :
   
   ```toml
   [npmAuth."https://npm.pkg.github.com/@thesakyo"] 
   token = "TON_TOKEN_ICI" 
   alwaysAuth = true
   ```
      
   Cette instruction permettra à Unity de se connecter au registre GitHub pour télécharger le package **SakyoGameLib**.
   
   Ensuite, il va falloir renseigner le package dans le fichier `manifest.json` de ton projet Unity.

#### • Installation manuelle du package via `manifest.json` :
   
   1. Ouvre ton projet Unity.

   2. Fait clic droit dans l'explorateur de projet. 

   3. Sélectionne **Show in Explorer**.

   4. Repère le dossier **Packages**, s'il n'existe pas, crée-le.

   5. Ouvre ou créer le fichier `manifest.json` dans un éditeur de texte.

   6. Le contenu du fichier `manifest.json` devrait ressembler à ceci :

       ```json
       {
         "dependencies": {
           "domain.organization.package": "x.y.z"
         },
         "scopedRegistries": []
       }
       ````
       - `domain.organization.package` représente un package déjà installé dans ton projet Unity.
       - `x.y.z` représente la version du package installé.

       **Information :** Il est possible que tu aies déjà des dépendances dans la section **dependencies**.

   7. Ajoute le registre personnalisé dans la section **scopedRegistries** ainsi que le package **SakyoGameLib** dans la section **dependencies** :

       ```json
       {
         "dependencies": {
           "domain.organization.package": "x.y.z", 
           "fr.thesakyo.sakyogamelib": "x.y.z" 
         },
         "scopedRegistries": [
           {
             "name": "SakyoGame Registry",
             "url": "https://npm.pkg.github.com/@thesakyo",
             "scopes": ["fr.thesakyo"]
           }
         ]
       }
       ```
      N'oublie pas de remplacer `x.y.z` par la version souhaitée de **SakyoGameLib** *([Voir les versions disponibles](https://github.com/TheSakyo/SakyoGameLib/pkgs/npm/fr.thesakyo.sakyogamelib/versions))*.

   8. Sauvegarde le fichier `manifest.json`.

   9. Unity devrait automatiquement télécharger et installer **SakyoGameLib** dans ton projet *(tu peux le retrouver dans le Package Manager dans le registre appelé **SakyoGame Registry**)*.

## 🚀 Fonctionnalités

   **SakyoGameLib** contient plusieurs modules et fonctionnalités pour t'aider à démarrer plus rapidement dans tes projets Unity :
   
   Au fur et à mesure de l'utilisation et des retours, de nouvelles fonctionnalités seront ajoutées. L'idée est de créer un ensemble d'outils qui grandit avec les projets de développement, en améliorant et ajoutant des systèmes au fur et à mesure.
   
   Si tu as des idées ou des besoins spécifiques, n'hésite pas à ouvrir une **issue** ou à soumettre une **pull request** pour enrichir cette librairie.

## 📖 Documentation

   **Version Unity : 6 (600.0.26f1)**
   
   **PAS ENCORE DISPONIBLE**
   
   La documentation complète n'est pas encore disponible. Pour le moment, n'hésite pas à explorer le code source pour mieux comprendre les fonctionnalités, ou à poser des questions si nécessaire.

### Exemples d'utilisation

   **PAS ENCORE DISPONIBLE**
   
   Les exemples d'utilisation ne sont pas encore fournis. Tu peux cependant consulter les fichiers source pour voir les différentes classes et méthodes disponibles, et comment les utiliser dans tes projets.

## 🛠️ Contribution

   Si tu veux contribuer à **SakyoGameLib**, voici comment faire :

   1. Fork le dépôt sur GitHub.

   2. Crée une nouvelle branche pour ta fonctionnalité ou ta correction de bug.

   3. Effectue tes modifications.

   4. Soumets une **Pull Request** avec une description claire de tes changements.

## 💬 Support

   Si tu rencontres des problèmes ou si tu as des questions, n'hésite pas à ouvrir une **issue** dans le dépôt GitHub. Je serai heureux de t'aider, si je le peux bien sûr ! 🙂

## 📝 Changelog

   Tu peux consulter le fichier [CHANGELOG](./CHANGELOG.md) pour découvrir les dernières mises à jour, ainsi que les versions précédentes de **SakyoGameLib**.

## 📜 License

   Ce projet est sous MIT License. Pour plus de détails, consulte le fichier [LICENSE](./LICENSE).
