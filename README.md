# SakyoGameLib

**SakyoGameLib** est une librairie utilitaire conçue pour simplifier le développement de jeux Unity. Elle contient une collection de fonctions, de classes et de systèmes réutilisables pour accélérer le développement, réduire le code redondant et améliorer la productivité dans tous tes projets de jeux Unity. Cette librairie est mise à jour en fonction de mes besoins et des projets en cours de développement.

Je suis encore débutant dans le développement de jeux Unity, mais je compte l'améliorer au fur et à mesure, à mesure que j'acquiers de l'expérience et de nouvelles compétences dans ce domaine.

## 📦 Installation

### Via Unity Package Manager (UPM)

1. Ouvre ton projet Unity.
2. Ouvre **Window > Package Manager**.
3. Clique sur l'icône en haut à gauche et sélectionne **My Registries**.
4. **SakyoGameLib** devrait apparaître dans la liste des packages disponibles si tu as opté pour l'ajout via le fichier **`.upmconfig.toml`** (voir plus bas).  
   Si ce n'est pas le cas, tu peux l'ajouter manuellement :
    - Clique sur le bouton **+** en haut à gauche de la fenêtre **Package Manager**.
    - Sélectionne **Add package from git URL**.
    - Dans la fenêtre qui apparaît, entre l'URL du dépôt :  
      `https://github.com/TheSakyo/SakyoGameLib.git`
5. Clique sur **Install** pour ajouter **SakyoGameLib** à ton projet.

Si tu veux ajouter automatiquement ma librairie pour chaque projet, tu peux suivre cette procédure :

### Ajouter un registre personnalisé dans **`.upmconfig.toml`** :

#### Si le fichier `.upmconfig.toml` n'existe pas :

- Pour **Windows**, crée le fichier à l'emplacement suivant :  
  `C:\Users\<User>\.upmconfig.toml`

- Pour **macOS**, crée-le à l'emplacement suivant :  
  `/Users/<User>/.upmconfig.toml`

- Pour **Linux**, crée-le à l'emplacement suivant :  
  `/home/<User>/.upmconfig.toml`

#### Ouvre le fichier `.upmconfig.toml` et ajoute ceci dans la section appropriée :

```toml
[[scopedRegistries]]
name = "SakyoGame Registry"
url = "https://npm.pkg.github.com/@thesakyo"
scopes = ["fr.thesakyo"]
```

Ensuite, tu pourras installer **SakyoGameLib** via le **Package Manager** comme décrit ci-dessus.

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

## 📜 License

Ce projet est sous MIT License. Pour plus de détails, consulte le fichier [LICENSE](./LICENSE).
