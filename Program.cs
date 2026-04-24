using System;
using System.Collections.Generic;

/*
 * Universitetssystem - obligatorisk oppgave 2
 *
 * Dette er en enkel konsollapplikasjon som modellerer et lite universitetssystem.
 * Systemet støtter registrering og innlogging av brukere med rollebaserte menyer,
 * kurshåndtering med karaktersetting og pensumlitteratur, samt et bibliotek med
 * utlån og historikk. Alle klassene og funksjonene er lagt i `Program.cs` for
 * enkelhet, men kan deles i egne filer for bedre struktur.
 *
 * Implementerte funksjoner:
 *
 * Brukere og innlogging:
 * - Registrering av nye brukere med rolle (student, faglærer eller bibliotekar).
 * - Innlogging med epost og passord.
 * - Rollebaserte menyer som viser ulike valg avhengig av brukertype.
 * - `Bruker` (baseklasse), `Student`, `Utvekslingsstudent` (subklasse av `Student`)
 *   og `Ansatt` (faglærer / bibliotekar).
 *
 * Student:
 * - Melde seg på og av kurs (med duplikat- og kapasitetssjekk).
 * - Se sine påmeldte kurs og registrerte karakterer.
 * - Søke, låne og returnere bøker i biblioteket.
 *
 * Faglærer:
 * - Opprette kurs (med duplikatsjekk) som knyttes til faglæreren.
 * - Sette karakterer (A–F) for studenter i egne kurs.
 * - Registrere pensumlitteratur for egne kurs.
 * - Søke kurs og bøker, låne og returnere bøker, og skrive ut kurslister.
 *
 * Bibliotekar:
 * - Registrere nye bøker/medier i biblioteket.
 * - Vise aktive lån og lånehistorikk.
 *
 * Bibliotek:
 * - Registrere bøker med id, tittel, forfatter, år og antall eksemplarer.
 * - Låne og returnere bøker (knyttet til innlogget bruker).
 * - Vise aktive lån og fullstendig lånehistorikk.
 * - Søke etter bøker basert på tittel.
 *
 * Feilhåndtering:
 * - Validering av brukerregistrering (alle felt må fylles ut).
 * - Try/catch rundt hovedmenyen for å fange uventede feil.
 */
/*

 *
 * 1) Autentisering og rollebasert flyt
 *    - Felt: `Passord` i `Bruker` og innloggingsfelt i `LoggInn`.
 *    - Formål: skille eksisterende og nye brukere, og gi hver rolle riktig
 *      meny og tillatelser etter innlogging.
 *    - Demo: vis registrering av student og faglærer, logg inn med hver og
 *      demonstrer at menyvalgene endres etter rolle.
 *
 * 2) Kursfunksjonalitet
 *    - `Kurs` har nå `Foreleser` og `Pensum`.
 *    - Faglærer kan opprette kurs (duplikatsjekk), registrere pensum og
 *      sette karakterer for deltakere i egne kurs.
 *    - Student kan melde seg på/av kurs med kapasitets- og duplikatsjekk.
 *    - Demo: opprett kurs som faglærer, meld student på, vis at dobbelt
 *      påmelding blir avvist, sett karakter og vis studentens karakterliste.
 *
 * 3) Bibliotek og utlån
 *    - `Bok`-modell og lister `bibliotek`, `aktiveLaan` og `historikk`.
 *    - `Laan` binder en bok til en `Bruker` med lånedato og eventuell
 *      retur dato.
 *    - Funksjoner: registrere bok (bibliotekar), låne/returnere bok (alle),
 *      og vise aktive lån/historikk (bibliotekar).
 *    - Demo: registrer bok, lån som student, vis aktive lån, returner bok,
 *      vis historikk.
 *
 * 4) Feilhåndtering og hjelpefunksjoner
 *    - Hovedløkken er omsluttet av try/catch for å fange uventede feil.
 *    - `ReadInt` gir trygg lesing av heltall og håndterer ugyldig input.
 *    - Demo: prøv å registrere bruker med tomme felter (validering), og
 *      skriv bok-id som tekst ved `ReadInt` for å vise robust feilhåndtering.
 *
 *  pek på et lite antall nøkkelfunksjoner
 * (RegistrerBruker, LoggInn, OpprettKurs, MeldPaaKurs, LanBokForBruker,
 * ReturnerBokForBruker) og forklar hva som skjer steg-for-steg. Bruk
 * kortdemonstrasjoner i konsollen for å vise både normalflyt og en eller to
 * feilsituasjoner (duplikat, fullt kurs, ingen eksemplarer).
 *
 */

// Bruker
// Felles baseklasse for alle brukertyper.
public class Bruker
{
    // Felles egenskaper for alle brukere. Student og Ansatt arver disse.
    public string Navn { get; set; }
    public string Epost { get; set; }
    public string Passord { get; set; } // -----------------------ny

    // Konstruktør som settes når en bruker opprettes.
    public Bruker(string navn, string epost, string passord)  // ----ny legg til passord
    {
        Navn = navn;
        Epost = epost;
        Passord = passord; // ---------------------------------- ny
    }
}

// Student
// Representerer en student. Klassen arver fra `Bruker`.
public class Student : Bruker
{
    // Studentens identifikator.
    public string StudentID { get; set; }


    // Listen over kurs som studenten er meldt opp i.
    // Denne brukes ved påmelding og når vi viser oversikt.
    public List<Kurs> KursListe { get; set; }

    public Dictionary<string, string> Karakterer { get; set; } // kurskode -> karakter  ------------------ ny

    // Konstruktør som initialiserer Student med id, navn og epost.
    public Student(string id, string navn, string epost, string passord) //-------------- legger til passord
        : base(navn, epost, passord)   // Kaller konstruktør i Bruker // ----------------- ny legg til passord
    {
        StudentID = id;
        KursListe = new List<Kurs>();
        Karakterer = new Dictionary<string, string>(); // ------------------ ny 
    }
}

// Utvekslingsstudent
// Arver fra `Student` og legger til informasjon om hjemuniversitet, land og periode.
public class Utvekslingsstudent : Student
{
    public string HjemUniversitet { get; set; }
    public string Land { get; set; }
    public string Periode { get; set; }

    public Utvekslingsstudent(string id, string navn, string epost, string passord,
        string hjemUniversitet, string land, string periode)  //-------------ny Legger til passord i konstruktør
        : base(id, navn, epost, passord) //---------------ny legger til passord i base-konstruktør
    {
        HjemUniversitet = hjemUniversitet;
        Land = land;
        Periode = periode;
    }
}

// Ansatt
// Representerer en ansatt og arver fra `Bruker`.
public class Ansatt : Bruker
{
    // Felt som er spesifikke for en ansatt.
    public string AnsattID { get; set; }
    public string Stilling { get; set; } // f.eks. foreleser, bibliotekar
    public string Avdeling { get; set; }

    // Konstruktør for Ansatt.
    public Ansatt(string id, string navn, string epost, string passord,
        string stilling, string avdeling) //-----------------ny Legger til passord i konstruktør
        : base(navn, epost, passord)  //----------------- ny legg til passord i base-konstruktør
    {
        AnsattID = id;
        Passord = passord;      // ---------------------------------- ny
        Stilling = stilling;
        Avdeling = avdeling;
    }
}

// Kurs
// Representerer et kurs med kode, navn, studiepoeng, maks antall plasser og deltakere.
public class Kurs
{
    // Kursinformasjon: kode, navn, studiepoeng og maks antall plasser.
    public string Kode { get; set; }
    public string Navn { get; set; }
    public int Studiepoeng { get; set; }
    public int MaksPlasser { get; set; }

    // Liste over deltakere. Denne brukes ved påmelding og når vi viser kursdeltakere.
    public List<Student> Deltakere { get; set; }

    public Ansatt Foreleser { get; set; } // ------------------ ny Legger til foreleser for kurset  
    public List<Bok> Pensum { get; set; } // ------------------ ny Legger til pensumlitteratur for kurset

    // Konstruktør som oppretter et kurs og initialiserer deltakerlisten.
    public Kurs(string kode, string navn, int studiepoeng, int maks)
    {
        Kode = kode;
        Navn = navn;
        Studiepoeng = studiepoeng;
        MaksPlasser = maks;
        Deltakere = new List<Student>();
        Pensum = new List<Bok>(); // ------------------ ny Initialiserer pensumlitteratur-listen
    }
}

// Bok
// Representerer en bok eller et medie i biblioteket.
public class Bok
{
    // Modell for bok/medie med id, tittel, forfatter, år og antall eksemplarer.
    public int Id { get; set; }
    public string Tittel { get; set; }
    public string Forfatter { get; set; }
    public int Ar { get; set; }
    public int Antall { get; set; } // antall tilgjengelige eksemplarer

    // Konstruktør for Bok.
    public Bok(int id, string tittel, string forfatter, int ar, int antall)
    {
        Id = id;
        Tittel = tittel;
        Forfatter = forfatter;
        Ar = ar;
        Antall = antall;
    }
}

// Program
// Inneholder menyen og all logikk for å håndtere brukere, kurs og bibliotek.
class Program
{
    // Service instance to use in tests and for separation of concerns
    static UniversityService svc = new UniversityService();

    // De følgende listene fungerer som enkel in-memory lagring.
    // I en ekte applikasjon ville dette vært i en database.
    static List<Student> studenter = new List<Student>(); // lagrer registrerte studenter
    static List<Ansatt> ansatte = new List<Ansatt>();   // lagrer registrerte ansatte
    static List<Kurs> kursListe = new List<Kurs>();     // alle kurs
    static List<Bok> bibliotek = new List<Bok>();       // bokdatabasen
    // Lån og historikk
    static List<Laan> aktiveLaan = new List<Laan>();
    static List<Laan> historikk = new List<Laan>();

    // Legacy in-memory storage kept for backward compatibility with Program console flow.
    static List<Student> legacyStudenter => studenter;
    static List<Ansatt> legacyAnsatte => ansatte;
    static List<Kurs> legacyKurs => kursListe;
    static List<Bok> legacyBibliotek => bibliotek;
    static List<Laan> legacyAktiveLaan => aktiveLaan;
    static List<Laan> legacyHistorikk => historikk;

    static Bruker innloggetBruker = null; //------------ny Holder styr på hvem som er logget inn (krav: støtte innlogging)
    // ------------------ ny Hovedmenyen er nå endret til innlogging/registrering med rollebaserte menyer
    static void Main(string[] args)
    {
        bool kjør = true;

        while (kjør)
        {
            try // ------------------ ny Feilhåndtering rundt hele hovedmenyen
            {
                if (innloggetBruker == null) // ------------------ ny Sjekker om bruker er logget inn
                {
                    // ------------------ ny Startmeny for innlogging og registrering
                    Console.WriteLine("\n=== Universitetssystem ===");
                    Console.WriteLine("[1] Registrer ny bruker");
                    Console.WriteLine("[2] Logg inn");
                    Console.WriteLine("[0] Avslutt");

                    string valg = Console.ReadLine();
                    switch (valg)
                    {
                        case "1": RegistrerBruker(); break; // ------------------ ny Erstatter gammel registrering
                        case "2": LoggInn(); break; // ------------------ ny Innlogging med epost og passord
                        case "0": kjør = false; break;
                        default: Console.WriteLine("Ugyldig valg."); break;
                    }
                }
                else if (innloggetBruker is Student) // ------------------ ny Rollebasert meny for student
                {
                    StudentMeny(); // ------------------ ny Viser studentmeny
                }
                else if (innloggetBruker is Ansatt ansatt) // ------------------ ny Rollebasert meny for ansatt
                {
                    if (ansatt.Stilling.ToLower() == "faglaerer") // ------------------ ny Sjekker om ansatt er faglærer
                        FaglaererMeny(); // ------------------ ny Viser faglærermeny
                    else if (ansatt.Stilling.ToLower() == "bibliotekar") // ------------------ ny Sjekker om ansatt er bibliotekar
                        BibliotekarMeny(); // ------------------ ny Viser bibliotekarmeny
                    else
                    {
                        Console.WriteLine("Ukjent stilling. Logger ut."); // ------------------ ny Feilhåndtering for ukjent stilling
                        innloggetBruker = null;
                    }
                }
            }
            catch (Exception ex) // ------------------ ny Fanger uventede feil
            {
                Console.WriteLine("En feil oppstod: " + ex.Message);
            }
        }
    }

    // Modell for lån (binder bok til en låner og datoer)
    public class Laan
    {
        public Bok Bok { get; set; }
        public Bruker Laaner { get; set; }
        public DateTime LaanDato { get; set; }
        public DateTime? ReturnertDato { get; set; }
    }

    // Hjelpemetode: finn bruker (student eller ansatt) basert på id
    static Bruker FinnBruker(string id)
    {
        foreach (var s in studenter)
            if (s.StudentID == id)
                return s;
        foreach (var a in ansatte)
            if (a.AnsattID == id)
                return a;
        return null;
    }

    // ------------------ ny Registrerer ny bruker med rolle (erstatter gammel RegistrerStudent og RegistrerAnsatt)
    static void RegistrerBruker()
    {
        Console.WriteLine("\nHvilken rolle?");
        Console.WriteLine("[1] Student");
        Console.WriteLine("[2] Faglærer");
        Console.WriteLine("[3] Bibliotekar");
        string rolle = Console.ReadLine();

        Console.Write("Navn: ");
        string navn = Console.ReadLine();
        Console.Write("Epost: ");
        string epost = Console.ReadLine();
        Console.Write("Passord: ");
        string passord = Console.ReadLine();

        try // ------------------ ny Feilhåndtering for registrering
        {
            if (string.IsNullOrEmpty(navn) || string.IsNullOrEmpty(epost) || string.IsNullOrEmpty(passord))
                throw new ArgumentException("Alle felt må fylles ut."); // ------------------ ny Validering av input

            switch (rolle)
            {
                case "1":
                    Console.Write("StudentID: ");
                    string sid = Console.ReadLine();

                    Console.Write("Er dette en utvekslingsstudent? (j/n): ");
                    string svar = Console.ReadLine();
                    if (!string.IsNullOrEmpty(svar) && svar.ToLower().StartsWith("j"))
                    {
                        Console.Write("Hjemuniversitet: ");
                        string hjem = Console.ReadLine();
                        Console.Write("Land: ");
                        string land = Console.ReadLine();
                        Console.Write("Periode (fra–til): ");
                        string periode = Console.ReadLine();

                        var u = new Utvekslingsstudent(sid, navn, epost, passord, hjem, land, periode);
                        studenter.Add(u);
                        svc.Studenter.Add(u);
                    }
                    else
                    {
                        var s = new Student(sid, navn, epost, passord);
                        studenter.Add(s);
                        svc.Studenter.Add(s);
                    }
                    Console.WriteLine("Student registrert.");
                    break;

                case "2":
                    Console.Write("AnsattID: ");
                    string fid = Console.ReadLine();
                    Console.Write("Avdeling: ");
                    string favd = Console.ReadLine();
                    var fa = new Ansatt(fid, navn, epost, passord, "faglaerer", favd);
                    ansatte.Add(fa);
                    svc.Ansatte.Add(fa);
                    // ------------------ ny Stilling settes automatisk til faglaerer
                    Console.WriteLine("Faglærer registrert.");
                    break;

                case "3":
                    Console.Write("AnsattID: ");
                    string bid = Console.ReadLine();
                    Console.Write("Avdeling: ");
                    string bavd = Console.ReadLine();
                    var ba = new Ansatt(bid, navn, epost, passord, "bibliotekar", bavd);
                    ansatte.Add(ba);
                    svc.Ansatte.Add(ba);
                    // ------------------ ny Stilling settes automatisk til bibliotekar
                    Console.WriteLine("Bibliotekar registrert.");
                    break;

                default:
                    Console.WriteLine("Ugyldig valg.");
                    break;
            }
        }
        catch (ArgumentException ex) // ------------------ ny Fanger valideringsfeil
        {
            Console.WriteLine("Feil: " + ex.Message);
        }
    }

    // ------------------ ny Innlogging med epost og passord (krav: bruker skal logge inn)
    static void LoggInn()
    {
        Console.Write("Epost: ");
        string epost = Console.ReadLine();
        Console.Write("Passord: ");
        string passord = Console.ReadLine();

        // Use service Authenticate to make this testable
        var bruker = svc.Authenticate(epost, passord);
        if (bruker != null)
        {
            innloggetBruker = bruker;
            if (bruker is Student s)
                Console.WriteLine("Logget inn som student: " + s.Navn);
            else if (bruker is Ansatt a)
                Console.WriteLine("Logget inn som " + a.Stilling + ": " + a.Navn);
        }
        else
        {
            Console.WriteLine("Feil epost eller passord."); // ------------------ ny Feilmelding ved feil innlogging
        }
    }

    // ------------------ ny Meny for innlogget student (krav: rollebasert meny)
    static void StudentMeny()
    {
        Student student = (Student)innloggetBruker; // ------------------ ny Caster innlogget bruker til Student
        Console.WriteLine("\n=== Studentmeny (" + student.Navn + ") ===");
        Console.WriteLine("[1] Meld på kurs");
        Console.WriteLine("[2] Meld av kurs");
        Console.WriteLine("[3] Se mine kurs");
        Console.WriteLine("[4] Se karakterer");
        Console.WriteLine("[5] Søk bok");
        Console.WriteLine("[6] Lån bok");
        Console.WriteLine("[7] Returner bok");
        Console.WriteLine("[0] Logg ut");

        string valg = Console.ReadLine();
        switch (valg)
        {
            case "1": MeldPaaKurs(student); break; // ------------------ ny Bruker innlogget student
            case "2": MeldAvKurs(student); break; // ------------------ ny Bruker innlogget student
            case "3": SeMineKurs(student); break; // ------------------ ny Viser kurs studenten er meldt på
            case "4": SeKarakterer(student); break; // ------------------ ny Viser karakterer
            case "5": SokBok(); break;
            case "6": LanBokForBruker(student); break; // ------------------ ny Lån bok som innlogget bruker
            case "7": ReturnerBokForBruker(student); break; // ------------------ ny Returner bok som innlogget bruker
            case "0": innloggetBruker = null; break; // ------------------ ny Logger ut
            default: Console.WriteLine("Ugyldig valg."); break;
        }
    }

    // ------------------ ny Meny for innlogget faglærer (krav: rollebasert meny)
    static void FaglaererMeny()
    {
        Ansatt faglaerer = (Ansatt)innloggetBruker; // ------------------ ny Caster innlogget bruker til Ansatt
        Console.WriteLine("\n=== Faglærermeny (" + faglaerer.Navn + ") ===");
        Console.WriteLine("[1] Opprett kurs");
        Console.WriteLine("[2] Søk kurs");
        Console.WriteLine("[3] Søk bok");
        Console.WriteLine("[4] Lån bok");
        Console.WriteLine("[5] Returner bok");
        Console.WriteLine("[6] Sett karakter");
        Console.WriteLine("[7] Registrer pensum");
        Console.WriteLine("[8] Print kurs og deltagere");
        Console.WriteLine("[0] Logg ut");

        string valg = Console.ReadLine();
        switch (valg)
        {
            case "1": OpprettKurs(faglaerer); break; // ------------------ ny Faglærer oppretter kurs
            case "2": SokKurs(); break;
            case "3": SokBok(); break;
            case "4": LanBokForBruker(faglaerer); break; // ------------------ ny Lån bok som faglærer
            case "5": ReturnerBokForBruker(faglaerer); break; // ------------------ ny Returner bok som faglærer
            case "6": SettKarakter(faglaerer); break; // ------------------ ny Sett karakter for student
            case "7": RegistrerPensum(faglaerer); break; // ------------------ ny Registrer pensum for kurs
            case "8": PrintKurs(); break;
            case "0": innloggetBruker = null; break; // ------------------ ny Logger ut
            default: Console.WriteLine("Ugyldig valg."); break;
        }
    }

    // ------------------ ny Meny for innlogget bibliotekar (krav: rollebasert meny)
    static void BibliotekarMeny()
    {
        Console.WriteLine("\n=== Bibliotekarmeny (" + innloggetBruker.Navn + ") ===");
        Console.WriteLine("[1] Registrer bok");
        Console.WriteLine("[2] Vis aktive lån");
        Console.WriteLine("[3] Vis historikk");
        Console.WriteLine("[0] Logg ut");

        string valg = Console.ReadLine();
        switch (valg)
        {
            case "1": RegistrerBok(); break;
            case "2": VisAktiveLaan(); break; // ------------------ ny Bibliotekar kan se aktive lån
            case "3": VisHistorikk(); break; // ------------------ ny Bibliotekar kan se historikk
            case "0": innloggetBruker = null; break; // ------------------ ny Logger ut
            default: Console.WriteLine("Ugyldig valg."); break;
        }
    }

    // Oppretter nytt kurs (nå med duplikatsjekk og faglærer-tilknytning)
    static void OpprettKurs(Ansatt faglaerer) // ------------------ ny Tar inn faglærer som parameter
    {
        Console.Write("Kode: ");
        string kode = Console.ReadLine();

        Console.Write("Navn: ");
        string navn = Console.ReadLine();

        // ------------------ ny Sjekker om kurs med samme kode eller navn allerede finnes (krav: ikke duplikat)
        foreach (Kurs k in kursListe)
        {
            if (k.Kode == kode || k.Navn == navn)
            {
                Console.WriteLine("Et kurs med samme kode eller navn finnes allerede."); // ------------------ ny Feilmelding for duplikat
                return;
            }
        }

        int sp = ReadInt("Studiepoeng: ");
        int maks = ReadInt("Maks plasser: ");

        Kurs nyttKurs = new Kurs(kode, navn, sp, maks);
        nyttKurs.Foreleser = faglaerer; // ------------------ ny Knytter faglærer til kurset
        kursListe.Add(nyttKurs);
        Console.WriteLine("Kurs opprettet.");
    }

    // ------------------ ny Student melder seg på kurs (erstatter gammel MeldStudent, bruker innlogget student)
    static void MeldPaaKurs(Student student)
    {
        Console.Write("Kurskode: ");
        string kode = Console.ReadLine();

        Kurs funnetKurs = null;
        foreach (Kurs k in kursListe)
            if (k.Kode == kode)
                funnetKurs = k;

        if (funnetKurs == null)
        {
            Console.WriteLine("Kurs ikke funnet.");
            return;
        }

        // Sjekk om studenten allerede er påmeldt (krav: student kan ikke melde seg på samme kurs flere ganger)
        if (funnetKurs.Deltakere.Contains(student))
        {
            Console.WriteLine("Du er allerede påmeldt dette kurset."); // ------------------ ny Feilmelding for duplikat påmelding
            return;
        }

        // Sjekker kapasitet før påmelding
        if (funnetKurs.Deltakere.Count >= funnetKurs.MaksPlasser)
        {
            Console.WriteLine("Kurset er fullt.");
            return;
        }

        funnetKurs.Deltakere.Add(student);
        student.KursListe.Add(funnetKurs);
        Console.WriteLine("Du er nå meldt på " + funnetKurs.Navn); // ------------------ ny Bekreftelse med kursnavn
    }

    // ------------------ ny Student melder seg av kurs (erstatter gammel MeldAvStudent, bruker innlogget student)
    static void MeldAvKurs(Student student)
    {
        Console.Write("Kurskode: ");
        string kode = Console.ReadLine();

        Kurs funnetKurs = null;
        foreach (Kurs k in kursListe)
            if (k.Kode == kode)
                funnetKurs = k;

        if (funnetKurs == null)
        {
            Console.WriteLine("Kurs ikke funnet.");
            return;
        }

        if (!funnetKurs.Deltakere.Contains(student))
        {
            Console.WriteLine("Du er ikke påmeldt dette kurset."); // ------------------ ny Feilmelding
            return;
        }

        funnetKurs.Deltakere.Remove(student);
        student.KursListe.Remove(funnetKurs);
        Console.WriteLine("Du er meldt av " + funnetKurs.Navn); // ------------------ ny Bekreftelse
    }

    // ------------------ ny Viser kurs studenten er meldt på (krav: student kan se sine kurs)
    static void SeMineKurs(Student student)
    {
        if (student.KursListe.Count == 0) // ------------------ ny Sjekker om studenten har noen kurs
        {
            Console.WriteLine("Du er ikke meldt på noen kurs.");
            return;
        }
        Console.WriteLine("Dine kurs:");
        foreach (Kurs k in student.KursListe)
            Console.WriteLine(" - " + k.Kode + " " + k.Navn); // ------------------ ny Viser kurskode og navn
    }

    // ------------------ ny Viser karakterer studenten har fått (krav: student kan se karakterer)
    static void SeKarakterer(Student student)
    {
        if (student.Karakterer.Count == 0) // ------------------ ny Sjekker om studenten har karakterer
        {
            Console.WriteLine("Ingen karakterer registrert.");
            return;
        }
        Console.WriteLine("Dine karakterer:");
        foreach (var par in student.Karakterer)
            Console.WriteLine(" - " + par.Key + ": " + par.Value); // ------------------ ny Viser kurskode og karakter
    }

    // ------------------ ny Faglærer setter karakter for en student (krav: faglærer kan sette karakter)
    static void SettKarakter(Ansatt faglaerer)
    {
        Console.Write("Kurskode: ");
        string kode = Console.ReadLine();

        // ------------------ ny Finner kurs der denne faglæreren er foreleser
        Kurs funnetKurs = null;
        foreach (Kurs k in kursListe)
            if (k.Kode == kode && k.Foreleser == faglaerer) // ------------------ ny Sjekker at faglærer eier kurset
                funnetKurs = k;

        if (funnetKurs == null)
        {
            Console.WriteLine("Kurs ikke funnet, eller du er ikke faglærer for dette kurset."); // ------------------ ny Feilmelding
            return;
        }

        Console.Write("StudentID: ");
        string sid = Console.ReadLine();

        // ------------------ ny Finner studenten i kursets deltakerliste
        Student funnetStudent = null;
        foreach (Student s in funnetKurs.Deltakere)
            if (s.StudentID == sid)
                funnetStudent = s;

        if (funnetStudent == null)
        {
            Console.WriteLine("Student ikke funnet i dette kurset."); // ------------------ ny Feilmelding
            return;
        }

        Console.Write("Karakter (A-F): ");
        string karakter = Console.ReadLine();
        funnetStudent.Karakterer[kode] = karakter; // ------------------ ny Lagrer karakter i studentens dictionary
        Console.WriteLine("Karakter " + karakter + " satt for " + funnetStudent.Navn + " i " + kode);
    }

    // ------------------ ny Faglærer registrerer pensum for et kurs (krav: faglærer kan registrere pensum)
    static void RegistrerPensum(Ansatt faglaerer)
    {
        Console.Write("Kurskode: ");
        string kode = Console.ReadLine();

        // ------------------ ny Finner kurs der denne faglæreren er foreleser
        Kurs funnetKurs = null;
        foreach (Kurs k in kursListe)
            if (k.Kode == kode && k.Foreleser == faglaerer) // ------------------ ny Sjekker at faglærer eier kurset
                funnetKurs = k;

        if (funnetKurs == null)
        {
            Console.WriteLine("Kurs ikke funnet, eller du er ikke faglærer for dette kurset."); // ------------------ ny Feilmelding
            return;
        }

        int bokId = ReadInt("Bok ID: ");

        // ------------------ ny Finner boken i biblioteket
        Bok funnetBok = null;
        foreach (Bok b in bibliotek)
            if (b.Id == bokId)
                funnetBok = b;

        if (funnetBok == null)
        {
            Console.WriteLine("Bok ikke funnet i biblioteket."); // ------------------ ny Feilmelding
            return;
        }

        funnetKurs.Pensum.Add(funnetBok); // ------------------ ny Legger boken til i pensumlisten
        Console.WriteLine(funnetBok.Tittel + " lagt til som pensum i " + funnetKurs.Navn);
    }

    // Printer kurs og alle deltakere.
    static void PrintKurs()
    {
        foreach (Kurs k in kursListe)
        {
            Console.WriteLine(k.Kode + " " + k.Navn);
            // Lister alle deltakere for hvert kurs (krav: liste kurs og deltakere)
            foreach (Student s in k.Deltakere)
                Console.WriteLine(" - " + s.Navn);
        }
    }

    // Søker etter kurs basert på kode eller navn.
    static void SokKurs()
    {
        Console.Write("Søk: ");
        string søk = Console.ReadLine();
        // Søker i kode eller navn (krav: søk basert på kriterier)
        foreach (Kurs k in kursListe)
            if (k.Kode.Contains(søk) || k.Navn.Contains(søk))
                Console.WriteLine(k.Kode + " " + k.Navn);
    }

    // Registrerer ny bok i biblioteket.
    static void RegistrerBok()
    {
        int id = ReadInt("Id: "); // ------------------ ny Fikset dobbel prompt-bug

        Console.Write("Tittel: ");
        string tittel = Console.ReadLine();

        Console.Write("Forfatter: ");
        string forfatter = Console.ReadLine();

        int ar = ReadInt("År: "); // ------------------ ny Fikset dobbel prompt-bug

        int antall = ReadInt("Antall: "); // ------------------ ny Fikset dobbel prompt-bug

        // Legger ny bok i biblioteket (krav: registrere bøker/medier)
        bibliotek.Add(new Bok(id, tittel, forfatter, ar, antall));
    }

    // ------------------ ny Låner bok for innlogget bruker (erstatter gammel LanBok)
    static void LanBokForBruker(Bruker bruker) // ------------------ ny Tar inn innlogget bruker som parameter
    {
        int id = ReadInt("Bok id: ");

        Bok funnetBok = null;
        foreach (Bok b in bibliotek)
            if (b.Id == id)
                funnetBok = b;

        if (funnetBok == null)
        {
            Console.WriteLine("Bok ikke funnet.");
            return;
        }

        if (funnetBok.Antall <= 0)
        {
            Console.WriteLine("Ingen eksemplarer tilgjengelig."); // ------------------ ny Bedre feilmelding
            return;
        }

        // ------------------ ny Oppretter lån knyttet til innlogget bruker
        Laan laan = new Laan { Bok = funnetBok, Laaner = bruker, LaanDato = DateTime.Now, ReturnertDato = null };
        aktiveLaan.Add(laan);
        funnetBok.Antall--;
        Console.WriteLine("Bok lånt: " + funnetBok.Tittel); // ------------------ ny Bekreftelse
    }

    // ------------------ ny Returnerer bok for innlogget bruker (erstatter gammel ReturnerBok)
    static void ReturnerBokForBruker(Bruker bruker) // ------------------ ny Tar inn innlogget bruker som parameter
    {
        int id = ReadInt("Bok id: ");

        // ------------------ ny Finner aktivt lån for denne boken og innlogget bruker
        Laan funnet = null;
        foreach (var l in aktiveLaan)
            if (l.Bok.Id == id && l.Laaner == bruker) // ------------------ ny Enklere sjekk med referanselikhet
            {
                funnet = l;
                break;
            }

        if (funnet == null)
        {
            Console.WriteLine("Du har ikke lånt denne boken."); // ------------------ ny Feilmelding
            return;
        }

        funnet.ReturnertDato = DateTime.Now;
        historikk.Add(funnet);
        aktiveLaan.Remove(funnet);
        funnet.Bok.Antall++;
        Console.WriteLine("Bok returnert: " + funnet.Bok.Tittel); // ------------------ ny Bekreftelse
    }

    // Viser aktive lån
    static void VisAktiveLaan()
    {
        Console.WriteLine("Aktive lån:");
        foreach (var l in aktiveLaan)
        {
            Console.WriteLine($"Bok: {l.Bok.Tittel} (id:{l.Bok.Id}) - Lånt av: {l.Laaner.Navn} - Dato: {l.LaanDato}");
        }
    }

    // Viser lånehistorikk
    static void VisHistorikk()
    {
        Console.WriteLine("Lånehistorikk:");
        foreach (var l in historikk)
        {
            Console.WriteLine($"Bok: {l.Bok.Tittel} (id:{l.Bok.Id}) - Lånt av: {l.Laaner.Navn} - Lånt: {l.LaanDato} - Returnert: {l.ReturnertDato}");
        }
    }

    // Søker etter bok basert på tittel.
    static void SokBok()
    {
        Console.Write("Tittel: ");
        string søk = Console.ReadLine();
        // Enkel tittel-søk. Viser hvor mange som er tilgjengelig.
        foreach (Bok b in bibliotek)
            if (b.Tittel.Contains(søk))
                Console.WriteLine(b.Tittel + " (" + b.Antall + " tilgjengelig)");
    }

    // (Tidligere utdatert LanBok/ReturnerBok fjernet — bruker-lånefunksjonalitet er implementert ovenfor.)

    // Hjelpemetode for trygg lesing av heltall
    static int ReadInt(string prompt)
    {
        int value;
        Console.Write(prompt);
        while (!int.TryParse(Console.ReadLine(), out value))
        {
            Console.Write("Ugyldig tall, prøv igjen: ");
        }
        return value;
    }
}
