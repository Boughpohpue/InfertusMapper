namespace Infertus.Mapper.DemoConsoleApp;

public class TestProfile : MappingProfile
{
    public TestProfile()
    {
        CreateMap<ClassA, ClassB>()
            .ForMember(d => d.Latitude, s => s.Lat)
            .ForMember(d => d.Longitude, s => s.Lon)
            .Ignore(d => d.Id);

        RegisterMapping<ClassB, ClassA>(b =>
            new ClassA(b.Latitude, b.Longitude));
    }
}

public class TestProfile2 : MappingProfile
{
    public TestProfile2()
    {
        CreateMap<ClassA, ClassC>()
            .ForMember(d => d.Latitude, s => s.Lat)
            .ForMember(d => d.Longitude, s => s.Lon);

        CreateMap<ClassB, ClassC>();

        RegisterMapping<ClassC, ClassA>(c =>
            new ClassA(c.Latitude, c.Longitude));

        CreateMap<ClassC, ClassB>()
            .Ignore(d => d.Id);
    }
}

public class TestProfile3 : MappingProfile
{
    public TestProfile3()
    {
        CreateMap<NestedB, NestedC>()
            .ForMember(d => d.NestedObjC, cfg => cfg.MapFrom(s => s.NestedObjB));

        CreateMap<NestedC, NestedB>()
            .ForMember(d => d.NestedObjB, cfg => cfg.MapFrom(s => s.NestedObjC));
    }
}
