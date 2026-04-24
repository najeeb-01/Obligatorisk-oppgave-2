using System;
using Xunit;

public class UniversityServiceTests
{
    [Fact]
    public void OpprettKurs_DuplikatKode_ReturnsFalse()
    {
        var svc = new UniversityService();
        var laerer = new Ansatt("A1","Lærer","l@ex.no","pw","faglaerer","IT");
        svc.Ansatte.Add(laerer);

        var ok = svc.OpprettKurs("TDT1000","Programmering",10,30, laerer);
        Assert.True(ok);

        var dup = svc.OpprettKurs("TDT1000","Programmering II",5,20, laerer);
        Assert.False(dup);
    }

    [Fact]
    public void MeldPaaKurs_DobbelPaamelding_Fails()
    {
        var svc = new UniversityService();
        var s = new Student("S1","Stud","s@ex.no","pw");
        var laerer = new Ansatt("A1","Lærer","l@ex.no","pw","faglaerer","IT");
        svc.Studenter.Add(s);
        svc.Ansatte.Add(laerer);
        svc.OpprettKurs("K1","Navn",5,10, laerer);

        var first = svc.MeldPaaKurs("S1","K1");
        var second = svc.MeldPaaKurs("S1","K1");
        Assert.True(first);
        Assert.False(second);
    }

    [Fact]
    public void LaanOgReturnerBok_UpdatesCountsAndHistory()
    {
        var svc = new UniversityService();
        var b = new Bok(1,"T","F",2020,1);
        svc.Bibliotek.Add(b);
        var s = new Student("S1","Stud","s@ex.no","pw");
        svc.Studenter.Add(s);

        var laant = svc.LanBokForBruker(1, s);
        Assert.True(laant);
        Assert.Equal(0, b.Antall);
        Assert.Single(svc.AktiveLaan);

        var returned = svc.ReturnerBokForBruker(1, s);
        Assert.True(returned);
        Assert.Equal(1, b.Antall);
        Assert.Empty(svc.AktiveLaan);
        Assert.Single(svc.Historikk);
    }
}
