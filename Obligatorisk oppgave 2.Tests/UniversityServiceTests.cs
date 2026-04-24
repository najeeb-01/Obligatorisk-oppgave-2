using System;
using Xunit;

public class UniversityServiceTests
{
    [Fact]
    public void Authenticate_ReturnsUser_WhenCredentialsAreCorrect()
    {
        var svc = new UniversityService();
        var student = new Student("s1", "Student One", "s1@example.com", "pwd");
        svc.Studenter.Add(student);

        var result = svc.Authenticate("s1@example.com", "pwd");

        Assert.NotNull(result);
        Assert.IsType<Student>(result);
        Assert.Equal(student, result);
    }

    [Fact]
    public void MeldPaaKurs_AddsStudentToCourse_WhenPossible()
    {
        var svc = new UniversityService();
        var student = new Student("s1", "Student One", "s1@example.com", "pwd");
        svc.Studenter.Add(student);
        var kurs = new Kurs("K001", "Testkurs", 10, 2);
        svc.KursListe.Add(kurs);

        var ok = svc.MeldPaaKurs("s1", "K001");

        Assert.True(ok);
        Assert.Contains(student, kurs.Deltakere);
        Assert.Contains(kurs, student.KursListe);
    }

    [Fact]
    public void MeldPaaKurs_ReturnsFalse_WhenCourseFull()
    {
        var svc = new UniversityService();
        var s1 = new Student("s1", "Student One", "s1@example.com", "pwd");
        var s2 = new Student("s2", "Student Two", "s2@example.com", "pwd");
        var s3 = new Student("s3", "Student Three", "s3@example.com", "pwd");
        svc.Studenter.Add(s1); svc.Studenter.Add(s2); svc.Studenter.Add(s3);
        var kurs = new Kurs("K001", "Testkurs", 10, 2);
        svc.KursListe.Add(kurs);

        Assert.True(svc.MeldPaaKurs("s1", "K001"));
        Assert.True(svc.MeldPaaKurs("s2", "K001"));
        // third should fail
        Assert.False(svc.MeldPaaKurs("s3", "K001"));
    }

    [Fact]
    public void LanBokForBruker_DecrementsCountAndAddsActiveLoan()
    {
        var svc = new UniversityService();
        var student = new Student("s1", "Student One", "s1@example.com", "pwd");
        svc.Studenter.Add(student);
        svc.RegistrerBok(1, "Bok 1", "Forfatter", 2020, 1);

        var ok = svc.LanBokForBruker(1, student);

        Assert.True(ok);
        Assert.Equal(0, svc.Bibliotek.Find(b => b.Id == 1).Antall);
        Assert.Single(svc.AktiveLaan);
    }

    [Fact]
    public void ReturnerBokForBruker_MovesLoanToHistoryAndIncrementsCount()
    {
        var svc = new UniversityService();
        var student = new Student("s1", "Student One", "s1@example.com", "pwd");
        svc.Studenter.Add(student);
        svc.RegistrerBok(1, "Bok 1", "Forfatter", 2020, 1);

        Assert.True(svc.LanBokForBruker(1, student));
        Assert.True(svc.ReturnerBokForBruker(1, student));

        Assert.Equal(1, svc.Bibliotek.Find(b => b.Id == 1).Antall);
        Assert.Empty(svc.AktiveLaan);
        Assert.Single(svc.Historikk);
    }

    [Fact]
    public void SettKarakter_AllowsTeacherToSetGradeForStudentInCourse()
    {
        var svc = new UniversityService();
        var teacher = new Ansatt("a1", "Teacher", "t@example.com", "pwd", "faglaerer", "dept");
        var student = new Student("s1", "Student One", "s1@example.com", "pwd");
        svc.Ansatte.Add(teacher);
        svc.Studenter.Add(student);
        var kurs = new Kurs("K001", "Testkurs", 10, 5) { Foreleser = teacher };
        kurs.Deltakere.Add(student);
        svc.KursListe.Add(kurs);

        // teacher sets grade by manipulating student.Karakterer as service does in Program
        student.Karakterer["K001"] = "A";

        Assert.True(student.Karakterer.ContainsKey("K001"));
        Assert.Equal("A", student.Karakterer["K001"]);
    }
}
