# README – Obligatorisk Oppgave 2

## IS-110 Objektorientert programmering

## Beskrivelse

Dette prosjektet er en videreutvikling av Obligatorisk Oppgave 1.
Applikasjonen er en C# Console applikasjon som simulerer et universitetssystem med innlogging og rollebasert tilgang.

Systemet håndterer brukere, kurs og et bibliotek med utlån og historikk.
Løsningen bygger videre på objektorientert design og utvider funksjonaliteten fra oppgave 1.

## Funksjonalitet

### Brukere og innlogging

* Registrere nye brukere
* Logge inn med epost og passord
* Rollebasert tilgang til funksjoner
* Roller: student, faglærer og bibliotekar

### Student

* Melde seg på kurs
* Melde seg av kurs
* Se egne kurs
* Se karakterer
* Søke etter bøker
* Låne og returnere bøker

### Faglærer

* Opprette kurs
* Hindre duplikate kurs
* Sette karakterer for studenter
* Registrere pensum til kurs
* Søke etter kurs og bøker
* Låne og returnere bøker
* Se kurs og deltakere

### Bibliotekar

* Registrere bøker
* Se aktive lån
* Se historikk

### Kurs

* Opprette kurs med kode, navn, studiepoeng og maks plasser
* Hindre at samme kurs opprettes flere ganger
* Hindre at student melder seg på samme kurs flere ganger
* Knytte faglærer til kurs
* Registrere pensum

### Bibliotek

* Registrere bøker med id, tittel, forfatter, år og antall
* Låne bok til student eller ansatt
* Hindre utlån hvis ingen eksemplarer er tilgjengelig
* Returnere bok
* Vise aktive lån
* Vise historikk

## Enhetstester

Prosjektet inneholder 4 enhetstester:

1. Authenticate_ReturnsUser_WhenCredentialsAreCorrect
 * Sjekker at en bruker kan logge inn med riktig epost og passord.
   
1. MeldPaaKurs_AddsStudentToCourse_WhenPossible
 * Sjekker at en student blir meldt på kurs når det er plass.
   
3. MeldPaaKurs_ReturnsFalse_WhenCourseFull
* Sjekker at en student ikke kan melde seg på et fullt kurs.
   
4. LanBokForBruker_DecrementsCountAndAddsActiveLoan
 * Sjekker at antall bøker reduseres og at et lån registreres.

Testene kan kjøres fra Test Explorer i Visual Studio.

## Tekniske konsepter

Programmet bruker følgende konsepter:

* Arv: Student og Ansatt arver fra Bruker
* Polymorfisme: Bruker brukes som felles type i lån
* Innkapsling: Egenskaper med get og set
* Konstruktører for initialisering
* Kolleksjoner: List og Dictionary
* Rollebasert logikk
* Feilhåndtering med try-catch

## Hvordan kjøre programmet

1. Åpne prosjektet i Visual Studio
2. Bygg løsningen
3. Kjør programmet
4. Registrer bruker eller logg inn
5. Bruk menyen basert på rolle

## Eksempel på bruk

* Registrer en student
* Logg inn som student
* Meld deg på et kurs
* Logg inn som faglærer
* Sett karakter
* Registrer en bok som bibliotekar
* Lån og returner bok

## Begrensninger

* Data lagres kun i minnet
* Ingen database
* Enkel input-validering

## Student: 
* Najeebullah Maroof
