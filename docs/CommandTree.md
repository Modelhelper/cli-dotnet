# ModelHelper CLI Command tree

- help
- generate [--source | --entity] --template --export ["path"] --options... => no options given == Console Mode
    - template | template-file
- entity
- template
    - edit
    - create <name> --can-export --export-file --type [snippet | template] --group --tag --scope [project | team | global]
    - clone [from-file] [file-name] [type = global | project | team]
    - group
- project
    - init
    - editor
    - code
        - location
            - get --all|list 'key'
            - set 'key' --path
            - remove
    - connection
        - add --name 'main' --connection-string '' --type 'mssql' [rest|api|gql|odata|mysql|mssql|postgres]
        - remove 'main'
        - set
- config
    set 'key' 'value'
    get 'key'
- api
    - set '    
- database
    - analyze [table | view] --source connectionName --dest connectionName
    - migration
        - script
    - diff [table | view] --source connectionName --dest connectionName
    - document [schema.][table | view][.columnname] --description | -d "text to describe"
    - optimize [table | view] --source connectionName --dest connectionName
    - entity [table | view]
        - add --group [group-name]
- Code
    - Generate (same options as root command generate)
    - Migrate???
    - Changelog
        - init [--from git-tag] --to
        - create 
        - reset
- Optimize [moved]


Hidden for user
- about
- install
- update
    - project
    - config
    - app
    - def | definition
    - template