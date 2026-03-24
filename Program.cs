using System;
using System.Collections.Generic;

/*
 * Universitetssystem - obligatorisk oppgave
 *
 * Dette er en enkel konsollapplikasjon som modellerer et lite universitetssystem.
 * Koden viser hvordan man kan håndtere brukere (studenter og ansatte), kurs og
 * et enkelt bibliotek med utlån og historikk. Alle klassene og funksjonene er
 * lagt i `Program.cs` for enkelhet, men kan deles i egne filer for bedre
 * struktur.
 *
 * Implementerte funksjoner:
 * - Brukere: `Student`, `Utvekslingsstudent` (subklasse av `Student`) og `Ansatt`.
 * - Kurs: oppretting, påmelding, avmelding, liste og søk.
 * - Bibliotek: registrere bøker, låne og returnere, vise aktive lån og historikk.
 *
 * Kommentarene i filen forklarer hvor hver funksjon befinner seg og hvordan den
 * brukes.
 */

// Bruker
// Felles baseklasse for alle brukertyper.
public class Bruker
{
    // Felles egenskaper for alle brukere. Student og Ansatt arver disse.
    public string Navn { get; set; }
    public string Epost { get; set; }

    // Konstruktør som settes når en bruker opprettes.
    public Bruker(string navn, string epost)
    {
        Navn = navn;
        Epost = epost;
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

    // Konstruktør som initialiserer Student med id, navn og epost.
    public Student(string id, string navn, string epost)
        : base(navn, epost)   // Kaller konstruktør i Bruker
    {
        StudentID = id;
        KursListe = new List<Kurs>();
    }
}

// Utvekslingsstudent
// Arver fra `Student` og legger til informasjon om hjemuniversitet, land og periode.
public class Utvekslingsstudent : Student
{
    public string HjemUniversitet { get; set; }
    public string Land { get; set; }
    public string Periode { get; set; }

    public Utvekslingsstudent(string id, string navn, string epost,
        string hjemUniversitet, string land, string periode)
        : base(id, navn, epost)
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
    public Ansatt(string id, string navn, string epost,
        string stilling, string avdeling)
        : base(navn, epost)
    {
        AnsattID = id;
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

    // Konstruktør som oppretter et kurs og initialiserer deltakerlisten.
    public Kurs(string kode, string navn, int studiepoeng, int maks)
    {
        Kode = kode;
        Navn = navn;
        Studiepoeng = studiepoeng;
        MaksPlasser = maks;
        Deltakere = new List<Student>();
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
    public int Antall { get; set; } // antall tilgjengelige eksempler

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
    // De følgende listene fungerer som enkel in-memory lagring.
    // I en ekte applikasjon ville dette vært i en database.
    static List<Student> studenter = new List<Student>(); // lagrer registrerte studenter
    static List<Ansatt> ansatte = new List<Ansatt>();   // lagrer registrerte ansatte
    static List<Kurs> kursListe = new List<Kurs>();     // alle kurs
    static List<Bok> bibliotek = new List<Bok>();       // bokdatabasen
    // Lån og historikk
    static List<Laan> aktiveLaan = new List<Laan>();
    static List<Laan> historikk = new List<Laan>();

    static void Main(string[] args)
    {
        // Hovedmenyen kjøres i en løkke slik at brukeren kan utføre flere handlinger.
        bool kjør = true;

        while (kjør)
        {
            Console.WriteLine("\n[1] Opprett kurs");
            Console.WriteLine("[2] Meld student til kurs");
            Console.WriteLine("[3] Print kurs og deltagere");
            Console.WriteLine("[4] Søk på kurs");
            Console.WriteLine("[5] Søk på bok");
            Console.WriteLine("[6] Lån bok");
            Console.WriteLine("[7] Returner bok");
            Console.WriteLine("[8] Registrer bok");
            Console.WriteLine("[9] Registrer student");
            Console.WriteLine("[10] Registrer ansatt");
            Console.WriteLine("[0] Avslutt");

            string valg = Console.ReadLine();

            // Velg funksjon basert på input fra brukeren.
            switch (valg)
            {
                case "1": OpprettKurs(); break;
                case "2": MeldStudent(); break;
                case "3": PrintKurs(); break;
                case "4": SokKurs(); break;
                case "5": SokBok(); break;
                case "6": LanBok(); break;
                case "7": ReturnerBok(); break;
                case "8": RegistrerBok(); break;
                case "9": RegistrerStudent(); break;
                case "10": RegistrerAnsatt(); break;
                case "0": kjør = false; break;
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

    // Oppretter nytt kurs.
    static void OpprettKurs()
    {
        Console.Write("Kode: ");
        string kode = Console.ReadLine();

        Console.Write("Navn: ");
        string navn = Console.ReadLine();

        int sp = ReadInt("Studiepoeng: ");
        int maks = ReadInt("Maks plasser: ");

        // Oppretter og legger til kurs i kurslisten (krav: Opprette kurs)
        kursListe.Add(new Kurs(kode, navn, sp, maks));
    }

    // Registrerer ny student (krav: støtte brukere)
    static void RegistrerStudent()
    {
        Console.Write("StudentID: ");
        string id = Console.ReadLine();
        Console.Write("Navn: ");
        string navn = Console.ReadLine();
        Console.Write("Epost: ");
        string epost = Console.ReadLine();

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

            studenter.Add(new Utvekslingsstudent(id, navn, epost, hjem, land, periode));
        }
        else
        {
            studenter.Add(new Student(id, navn, epost));
        }

        Console.WriteLine("Student registrert.");
    }

    // Registrerer ny ansatt
    static void RegistrerAnsatt()
    {
        Console.Write("AnsattID: ");
        string id = Console.ReadLine();
        Console.Write("Navn: ");
        string navn = Console.ReadLine();
        Console.Write("Epost: ");
        string epost = Console.ReadLine();
        Console.Write("Stilling: ");
        string stilling = Console.ReadLine();
        Console.Write("Avdeling: ");
        string avdeling = Console.ReadLine();

        ansatte.Add(new Ansatt(id, navn, epost, stilling, avdeling));
        Console.WriteLine("Ansatt registrert.");
    }

    // Melder student til kurs.
    static void MeldStudent()
    {
        Console.Write("StudentID: ");
        string id = Console.ReadLine();

        Console.Write("Kurskode: ");
        string kode = Console.ReadLine();

        Student funnetStudent = null;
        Kurs funnetKurs = null;

        // Søker gjennom listene med foreach.
        foreach (Student s in studenter)
            if (s.StudentID == id)
                funnetStudent = s;

        foreach (Kurs k in kursListe)
            if (k.Kode == kode)
                funnetKurs = k;

        // Hvis både student og kurs finnes, prøver vi å melde på
        if (funnetStudent != null && funnetKurs != null)
        {
            // Sjekk om studenten allerede er påmeldt
            if (funnetKurs.Deltakere.Contains(funnetStudent))
            {
                Console.WriteLine("Student er allerede påmeldt dette kurset.");
                return;
            }

            // Sjekker kapasitet før påmelding (krav: sjekk kapasitet)
            if (funnetKurs.Deltakere.Count < funnetKurs.MaksPlasser)
            {
                // Legger student i kursets deltakerliste
                funnetKurs.Deltakere.Add(funnetStudent);
                // Legger kurset i studentens kursliste
                funnetStudent.KursListe.Add(funnetKurs);
                Console.WriteLine("Student meldt på.");
            }
            else
            {
                Console.WriteLine("Kurset er fullt.");
            }
        }
        else
        {
            // Enkel feilmelding hvis enten student eller kurs ikke finnes
            Console.WriteLine("Student eller kurs ikke funnet.");
        }
    }

    // Melder student av et kurs (krav: kunne melde av)
    static void MeldAvStudent()
    {
        Console.Write("StudentID: ");
        string id = Console.ReadLine();
        Console.Write("Kurskode: ");
        string kode = Console.ReadLine();

        Student funnetStudent = null;
        Kurs funnetKurs = null;

        foreach (Student s in studenter)
            if (s.StudentID == id)
                funnetStudent = s;

        foreach (Kurs k in kursListe)
            if (k.Kode == kode)
                funnetKurs = k;

        if (funnetStudent != null && funnetKurs != null)
        {
            if (funnetKurs.Deltakere.Contains(funnetStudent))
            {
                funnetKurs.Deltakere.Remove(funnetStudent);
                funnetStudent.KursListe.Remove(funnetKurs);
                Console.WriteLine("Student meldt av kurset.");
            }
            else
            {
                Console.WriteLine("Student er ikke påmeldt dette kurset.");
            }
        }
        else
        {
            Console.WriteLine("Student eller kurs ikke funnet.");
        }
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
        Console.Write("Id: ");
        int id = ReadInt("Id: ");

        Console.Write("Tittel: ");
        string tittel = Console.ReadLine();

        Console.Write("Forfatter: ");
        string forfatter = Console.ReadLine();

        Console.Write("År: ");
        int ar = ReadInt("År: ");

        Console.Write("Antall: ");
        int antall = ReadInt("Antall: ");

        // Legger ny bok i biblioteket (krav: registrere bøker/medier)
        bibliotek.Add(new Bok(id, tittel, forfatter, ar, antall));
    }

    // Låner bok hvis tilgjengelig og knytter lånet til en bruker (krav)
    static void LanBok()
    {
        int id = ReadInt("Bok id: ");
        Console.Write("Låner ID (StudentID eller AnsattID): ");
        string brukerId = Console.ReadLine();

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
            Console.WriteLine("Ingen igjen.");
            return;
        }

        Bruker laaner = FinnBruker(brukerId);
        if (laaner == null)
        {
            Console.WriteLine("Låner ikke funnet. Registrer bruker først.");
            return;
        }

        // Opprett aktivt lån
        Laan laan = new Laan { Bok = funnetBok, Laaner = laaner, LaanDato = DateTime.Now, ReturnertDato = null };
        aktiveLaan.Add(laan);
        funnetBok.Antall--;
        Console.WriteLine("Bok lånt til: " + laaner.Navn);
    }

    // Returnerer bok og flytter lånet fra aktive til historikk (krav)
    static void ReturnerBok()
    {
        int id = ReadInt("Bok id: ");
        Console.Write("Låner ID (StudentID eller AnsattID): ");
        string brukerId = Console.ReadLine();

        // Finn aktivt lån for denne bok og bruker
        Laan funnet = null;
        foreach (var l in aktiveLaan)
        {
            if (l.Bok.Id == id && ((l.Laaner is Student && ((Student)l.Laaner).StudentID == brukerId) || (l.Laaner is Ansatt && ((Ansatt)l.Laaner).AnsattID == brukerId)))
            {
                funnet = l;
                break;
            }
        }

        if (funnet == null)
        {
            Console.WriteLine("Aktivt lån ikke funnet for gitt bok og låner.");
            return;
        }

        funnet.ReturnertDato = DateTime.Now;
        historikk.Add(funnet);
        aktiveLaan.Remove(funnet);
        funnet.Bok.Antall++;
        Console.WriteLine("Bok returnert av: " + funnet.Laaner.Navn);
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
