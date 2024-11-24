## HIERARCHY
1. All objects should be named with PascalCase

## CODE
1. PascalCase for public members `public int NumberOfAttacks`
2. CamelCase with prefix `_` for private members `private int _numberOfAttacks` `[SerializeField] private int _numberOfAttacks`
3. Mix of UpperCase and SnakeCase for const members `private const int NUMBER_OF_ATTACKS` `public const int NUMBER_OF_ATTACKS`
4. Always include access modifier
5. Functions are verbs, classes/structs are nouns
6. Boolean should always be in a form of question (usually adding `is` at the beginning of it's name is enough)
7. Interface name starts with `I` (e.g. `IInteractable`)
8. `if` without curly brackets is allowed only for one liners (e.g. `if (true) return null`)
9. Methods used for events subscription should be private and start with `On_` (e.g. `private void On_InputMapChange() { }`)
10. TODO format: `//TODO: something that needs to be done (optional: URL to task)`

## REPO CONVENTIONS
1. Branch name starts with
    * `feature` if it contains new feature (including adding/removing new packages, assets etc.) `feature/some-feature-to-do`
    * `bugfix` if it contains bugfixes that are supposed to be reviewed `bugfix/bug-to-fix`
    * `hotfix` if it contains fix for major bug that needs to be merged asap `hotfix/important-bug-to-fix`
2. Branch should be responsible only for the thing it describes (usually the equivalent of one task)
3. Commit name should be short and concise
4. Anything else that needs to be mentioned should be in commit description
