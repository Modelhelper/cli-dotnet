# Funksjoner som skal støttes i versjon 3 (kjernen)

## Prosjektfiler
    - Åpne
    - Lagre
    - Opprette

## Maler
    - Åpne
    - Parse
    - Clone(Type: global | team | project)

## vurderinger:
hvordan benytte definisjonsfiler på datatypekobling
    - leses inn i context ved oppstart
    - leses inn i context kun på de kommando som behøver det

skal snippets leses inn ved behov?
    - f.eks ved generate command

Hva med global configuration

## Kommando:
Update
Install
Execute
    generate
    entity
        analyze
    template
    database | db
        diff
        analyze
            data
            structure
    code
        generate
        diff
        changelog
    project
        new | init
        add | a
        remove | r
        change | c

## Models
project
code
table
tables
template
options

## Console Mode

## Release notes - skal jeg vise det i oppstart?
- Hvis det kan hentes ut automatisk...

## Demodata

1) demo.mssql
2) demo.postgres
3) demo.mysql
4) demo.mongodb
5) demo.firebase

eks: mh h -c demo.mssql -e person -t

10+ tabeller med relasjoner
og forskjellige kolonner og oppsett

Person
Adresse
Telefon
PersonAdresse
PersonTelefon
PersonType
AdresseType
TelefonType


må vise datamodell

indexer
triggere?


## Filters

### Casing
Kebab   (kebab-case)
Snake   (snake_case)
Pascal  (PascalCase)
Camel   (camelCase)
Proper  (Proper Case)

### Misc
DataType | dt | datatype [arg: cs | go | proto]
    default - template language

Words
Plural
Singular
Abbreviation

NoPrefix
NoSuffix
NoPostfix

## Tags

Det meste av tags kan løses med snippets

## Flyttes over fra .net 4.7

ApplicationContext
ContextBuilder
SqlServerDataSource
ArgumentParser
Filters | skrives om til Fluid
Tags | skrives om til Fluid
ModelhelperSetup
Constants

## Misc
Hvordan skal filter som CSharp fungere?
kan jeg lage filter med parameter
eks: {{ column.Name | DataType: 'cs' | 'go' | 'proto' }}

må støtte lcase også

datatype: 'cs'