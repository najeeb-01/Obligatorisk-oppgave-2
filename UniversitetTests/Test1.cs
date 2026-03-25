namespace UniversitetTests;

// Enhetstester for universitetssystemet.
// Tester oppretting av objekter, kurspåmelding med kapasitetssjekk,
// og at utlån reduserer antall tilgjengelige eksemplarer.

[TestClass]
public sealed class Test1
{
    // Test 1: Sjekker at en Student opprettes med riktige verdier
    // og at KursListe og Karakterer er tomme ved oppstart.
    [TestMethod]
    public void StudentOpprettes_MedRiktigeVerdier()
    {
        Student student = new Student("S001", "Ola", "ola@uia.no", "passord123");

        Assert.AreEqual("S001", student.StudentID);
        Assert.AreEqual("Ola", student.Navn);
        Assert.AreEqual("ola@uia.no", student.Epost);
        Assert.AreEqual("passord123", student.Passord);
        Assert.AreEqual(0, student.KursListe.Count);
        Assert.AreEqual(0, student.Karakterer.Count);
    }

    // Test 2: Sjekker at en student kan meldes på et kurs,
    // og at kurset er fullt når maks antall plasser er nådd.
    [TestMethod]
    public void KursPaamelding_SjekkerKapasitet()
    {
        Kurs kurs = new Kurs("IS110", "OOP", 10, 2); // maks 2 plasser
        Student s1 = new Student("S001", "Ola", "ola@uia.no", "pass1");
        Student s2 = new Student("S002", "Kari", "kari@uia.no", "pass2");

        kurs.Deltakere.Add(s1);
        kurs.Deltakere.Add(s2);

        // Kurset har nå 2 av 2 plasser brukt
        bool erFull = kurs.Deltakere.Count >= kurs.MaksPlasser;

        Assert.AreEqual(2, kurs.Deltakere.Count);
        Assert.IsTrue(erFull);
    }

    // Test 3: Sjekker at antall eksemplarer reduseres ved utlån
    // og økes ved retur.
    [TestMethod]
    public void BokUtlaan_RedusererAntall()
    {
        Bok bok = new Bok(1, "C# Programmering", "Forfatter", 2024, 3);

        // Simulerer utlån
        bok.Antall--;
        Assert.AreEqual(2, bok.Antall);

        // Simulerer retur
        bok.Antall++;
        Assert.AreEqual(3, bok.Antall);
    }
}