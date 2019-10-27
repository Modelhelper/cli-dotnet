Oppstart
- Sjekk config type
    - Hvis json = gammel => må oppdateres
    - Hvis yaml => sjekk version => oppdateres hvis nødvendig
- I Config er det satt verdier for:
    - Shared
- Finnes i env var? => legg inn path 

kjør:  CertUtil -hashfile "C:\Users\hans-petter\OneDrive - PatoGen AS\Downloads\model-helper-v2.5.zip" SHA256
for å få sjekksum


mh code generate -e -t -...
mh db compare --src --dest
mh db migration --src --dest --script <path>
mh db tables
mh project add connection => Console Mode
mg project delete connection "name"
mh project add code location

project [0]
    add [1]
        connection [2] => add.connection
        code [2]
            location [3]
    delete
        connection
        code
            location
    edit

db
    compare
    migration
    tables
    views
    users

- og -- blir alltid tolket som options

\s => silent, kjører kode uten output