using System;
using System.Collections.Generic;

public class UniversityService
{
    public List<Student> Studenter { get; } = new List<Student>();
    public List<Ansatt> Ansatte { get; } = new List<Ansatt>();
    public List<Kurs> KursListe { get; } = new List<Kurs>();
    public List<Bok> Bibliotek { get; } = new List<Bok>();

    // Enkelt lån-objekt internt i servicen
    public class LaanEntry
    {
        public Bok Bok { get; set; }
        public Bruker Laaner { get; set; }
        public DateTime LaanDato { get; set; }
        public DateTime? ReturnertDato { get; set; }
    }

    public List<LaanEntry> AktiveLaan { get; } = new List<LaanEntry>();
    public List<LaanEntry> Historikk { get; } = new List<LaanEntry>();

    // Oppretter kurs. Returnerer true hvis opprettet, false ved duplikat.
    public bool OpprettKurs(string kode, string navn, int sp, int maks, Ansatt foreleser)
    {
        foreach (var k in KursListe)
        {
            if (k.Kode == kode || k.Navn == navn)
                return false;
        }

        var kurs = new Kurs(kode, navn, sp, maks) { Foreleser = foreleser };
        KursListe.Add(kurs);
        return true;
    }

    // Melder student på kurs. Returnerer true hvis vellykket, false ellers.
    public bool MeldPaaKurs(string studentId, string kursKode)
    {
        var student = Studenter.Find(s => s.StudentID == studentId);
        if (student == null) return false;

        var kurs = KursListe.Find(k => k.Kode == kursKode);
        if (kurs == null) return false;

        if (kurs.Deltakere.Contains(student)) return false;
        if (kurs.Deltakere.Count >= kurs.MaksPlasser) return false;

        kurs.Deltakere.Add(student);
        student.KursListe.Add(kurs);
        return true;
    }

    // Låner bok for en bruker
    public bool LanBokForBruker(int bokId, Bruker bruker)
    {
        var bok = Bibliotek.Find(b => b.Id == bokId);
        if (bok == null) return false;
        if (bok.Antall <= 0) return false;

        var laan = new LaanEntry { Bok = bok, Laaner = bruker, LaanDato = DateTime.Now };
        AktiveLaan.Add(laan);
        bok.Antall--;
        return true;
    }

    // Returnerer bok for en bruker
    public bool ReturnerBokForBruker(int bokId, Bruker bruker)
    {
        var funnet = AktiveLaan.Find(l => l.Bok.Id == bokId && l.Laaner == bruker);
        if (funnet == null) return false;

        funnet.ReturnertDato = DateTime.Now;
        Historikk.Add(funnet);
        AktiveLaan.Remove(funnet);
        funnet.Bok.Antall++;
        return true;
    }

    // Registrerer bok
    public void RegistrerBok(int id, string tittel, string forfatter, int ar, int antall)
    {
        Bibliotek.Add(new Bok(id, tittel, forfatter, ar, antall));
    }

    // Autentiserer bruker basert på epost og passord. Returnerer Bruker hvis korrekt, ellers null.
    public Bruker Authenticate(string epost, string passord)
    {
        foreach (var s in Studenter)
        {
            if (s.Epost == epost && s.Passord == passord)
                return s;
        }

        foreach (var a in Ansatte)
        {
            if (a.Epost == epost && a.Passord == passord)
                return a;
        }

        return null;
    }
}
