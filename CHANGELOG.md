# Changelog

Toutes les modifications notables apportées à ce projet seront documentées dans ce fichier.

Le format est basé sur [Keep a Changelog](https://keepachangelog.com/), et ce projet suit [Semantic Versioning](https://semver.org/).

---

## [0.0.3] - 2025-01-05 13h (CET)

### Ajouts et Modifications
- Ajout d'un nouveau namespace `SakyoGame.Lib.Shared` pour définir des fonctionnalités partagées entre les différents projets et qui ne sont pas spécifiques à un projet en particulier.
- Ajout d'un attribut personnalisé nommé `Beta` pour représenter des fonctionnalités en cours de développement.
- Mise en place de l'attribut `Beta` sur les fonctionnalités représentant la génération 3D de la map (En cours de développement).
- Modification de la logique des constructeurs de la classe `MeshData` pour fixer le problème du tableau de `float` qui avait pour chaque élément une valeur de `0`.

---

## [0.0.2] - 2025-01-02 23h (CET)

### Modifications
- Les classes `MapGeneratorEditor`, `ResponsiveUIEditor` et `ButtonUIEditor` deviennent publiques, permettant ainsi une plus grande personnalisation.
- Les méthodes des classes `MapGenerator` et `Display` sont maintenant surchargeables, permettant ainsi une plus grande personnalisation.
- La visibilité des méthodes `getter` de la classe `CameraView` a été mise à jour pour être `public`, offrant un accès plus large.

---

## [0.0.1] - 2025-01-02 14h (CET)
- Première version du projet.
