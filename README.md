# Projet de groupe ASP .NET RIL 19

## Suivi du projet
L'avancée du projet est visualisable via l'outil Trello mis en place pour celui-ci, accessible à [cette adresse](https://trello.com/b/GiApZY9S/aspnet-ril-19).

## Mise en place de la base de données en utilisant Visual Studio
* Dans l'exporateur de serveur (Affchage > explorateur de serveur), créer une nouvelle connexion.
* Dans le champs "Nom du serveur", entrer la valeur suivante : `(LocalDb)\MSSQLLocalDb`
* Dans le champs "Sélectionner ou entrer un nom de base de données", entrer la valeur suivante : `TestGenerator.Model.Data.TestGeneratorContext`
* Il se peut qu'une pop-up vous indique que la base n'existe pas et vous propose d'en créer une, clicker sur "oui".
* Ouvrez ensuite l'invité de commande de packages NuGet (Outils > Gestionnaire de package NuGet > Console du gestionnaire de packages)
* Prenez soin de vérifier que le projet de startup (dans le bandeau du haut de Visual Studio) est bien configuré sur TestGenerator.Web, et que le projet par défaut dans la console du gestionnaire de packages est configuré sur TestGenerator.Model
* Entrez la commande "update-database".

## Règles de nommage
Ce projet étant très succinct, nous avons mis en place un format de nommage des branches simple :
`<prefixe>/<id_ticket_trello>-<description_de_la_branche>`

Les préfixes de branche servent à identifier leur rôle :
* feature pour une fonctionnalité
* bugfix pour une résolution de bug

En suivant ce format, un ticket de fonctionnalité dont le code est "UST1" correspondra à une branche nommée :  
`feature/UST1-inscription_enseignant`


## Contributeurs
- [Arcène Mba Nyangone](mailto:arcene.mbanyangone@viacesi.fr?subject=[RIL19]%20Projet%20ASP%20dot%20net)
- [Christopher Walker](mailto:christopher.walker@viacesi.fr?subject=[RIL19]%20Projet%20ASP%20dot%20net)
- [Lucas Kleinmann](mailto:lucas.Kleinmann@viacesi.fr?subject=[RIL19]%20Projet%20ASP%20dot%20net)
- [Timothée Simon-Franza](mailto:timothee.simonfranza@viacesi.fr?subject=[RIL19]%20Projet%20ASP%20dot%20net)
