sjekk for nvarchar/varchar(max) - ikke nødvendig at alle felter er (max) - kan hindre performance

kun en identity- kolonne i int/serial

Liste opp tabeller som er endret sise x- dager
- kan også brukes i generering av kode

-e * -entity-changed '3d' [h, d, w, y, mi, m] - days er default


slette- regler må defineres i prosjektet
-- cascading delete
-- manuell delete

hva med Parent%Id uten FK, skal man legge inn sjekk på den?