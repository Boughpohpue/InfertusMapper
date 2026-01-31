namespace Infertus.Mapper.DemoConsoleApp;

public class ClassA(double lat, double lon)
{
    public double Lat { get; set; } = lat;
    public double Lon { get; set; } = lon;

    public override string ToString()
    {
        return $"[{nameof(ClassA)}]\n{nameof(Lat)}: {Lat}\n{nameof(Lon)}: {Lon}\n";
    }
}

public class ClassB
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }


    public ClassB()
    {
    }
    public ClassB(int id, double latitude, double longitude)
    {
        Id = id;
        Latitude = latitude;
        Longitude = longitude;
    }

    public override string ToString()
    {
        return $"[{nameof(ClassB)}]\n{nameof(Id)}: {Id}\n{nameof(Latitude)}: {Latitude}\n{nameof(Longitude)}: {Longitude}\n";
    }
}

public class ClassC
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }


    public ClassC()
    {
    }
    public ClassC(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public override string ToString()
    {
        return $"[{nameof(ClassC)}]\n{nameof(Latitude)}: {Latitude}\n{nameof(Longitude)}: {Longitude}\n";
    }
}
