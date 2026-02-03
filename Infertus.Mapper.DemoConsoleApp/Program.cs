using Infertus.Mapper;
using Infertus.Mapper.DemoConsoleApp;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        // Different methods of configuring mapper:
        var configMethod = 3;
        switch (configMethod)
        {
            case 1:
                // add mapper and configure mapping profiles one by one:
                serviceCollection
                    .AddMapper()
                    .AddMappingProfile<TestProfile>()
                    .AddMappingProfile<TestProfile2>()
                    .AddMappingProfile<TestProfile3>();
                break;

            case 2:
                // add mapper and configure multiple mapping profiles types at once:
                serviceCollection
                    .AddMapper()
                    .AddMappingProfiles(
                        typeof(TestProfile),
                        typeof(TestProfile2),
                        typeof(TestProfile3));
                break;

            case 3:
                // add mapper and configure multiple mapping profiles using mapper config expression:
                serviceCollection.AddMapper(cfg =>
                {
                    cfg.AddProfile<TestProfile>();
                    cfg.AddProfile(typeof(TestProfile2));
                    cfg.AddProfile<TestProfile3>();
                });
                break;

            case 4:
                // configure mapper with multiple mapping profiles types in one line:
                serviceCollection.AddMapper(typeof(TestProfile), typeof(TestProfile2), typeof(TestProfile3));
                break;
        }

        Test(serviceCollection.BuildServiceProvider());
    }

    static void Test(ServiceProvider provider)
    {
        var mapper = provider.GetService<IMapper>();
        if (mapper == null)
        {
            Console.WriteLine("Failed to configure services!");
            return;
        }

        var objA = new ClassA(1.44, 69.3);
        Console.WriteLine($"{nameof(objA)} {objA}");
        var objB = new ClassB(12, 3.69, 6.93);
        Console.WriteLine($"{nameof(objB)} {objB}");
        var objC = new ClassC(9.63, 3.69);
        Console.WriteLine($"{nameof(objC)} {objC}");
        Console.WriteLine();

        var objA_mappedToB = mapper.Map<ClassB>(objA);
        Console.WriteLine($"{nameof(objA_mappedToB)} {objA_mappedToB}");
        var objA_mappedToC = mapper.Map<ClassC>(objA);
        Console.WriteLine($"{nameof(objA_mappedToC)} {objA_mappedToC}");
        Console.WriteLine();

        var objB_mappedToA = mapper.Map<ClassA>(objB);
        Console.WriteLine($"{nameof(objB_mappedToA)} {objB_mappedToA}");
        var objB_mappedToC = mapper.Map<ClassC>(objB);
        Console.WriteLine($"{nameof(objB_mappedToC)} {objB_mappedToC}");
        Console.WriteLine();

        var objC_mappedToA = mapper.Map<ClassA>(objC);
        Console.WriteLine($"{nameof(objC_mappedToA)} {objC_mappedToA}");
        var objC_mappedToB = mapper.Map<ClassB>(objC);
        Console.WriteLine($"{nameof(objC_mappedToB)} {objC_mappedToB}");
        Console.WriteLine();

        var nestedB = new NestedB($"Nested object of type {nameof(ClassB)}", new ClassB(9, 1.23, 3.33));
        Console.WriteLine($"{nameof(nestedB)} {nestedB}");
        var nestedC = new NestedC($"Nested object of type {nameof(ClassC)}", new ClassC(6.66, 9.99));
        Console.WriteLine($"{nameof(nestedC)} {nestedC}");
        Console.WriteLine();

        var nestedB_mappedToNestedC = mapper.Map<NestedC>(nestedB);
        Console.WriteLine($"{nameof(nestedB_mappedToNestedC)} {nestedB_mappedToNestedC}");
        var nestedC_mappedToNestedB = mapper.Map<NestedB>(nestedC);
        Console.WriteLine($"{nameof(nestedC_mappedToNestedB)} {nestedC_mappedToNestedB}");
        Console.WriteLine();

        var collectionB = new List<ClassB> 
        {
            new(1, 1.11, 3.33),
            new(2, 2.22, 6.66),
            new(3, 3.33, 9.99),
        };
        Console.WriteLine(collectionB.GetString(nameof(collectionB)));
        Console.WriteLine();

        var collectionC = new List<ClassC>
        {
            new(1.23, 4.56),
            new(2.34, 5.67),
            new(3.45, 6.78),
            new(4.56, 7.89),
        };
        Console.WriteLine(collectionC.GetString(nameof(collectionC)));
        Console.WriteLine();

        var collectionB_mappedToCollectionC = mapper.Map<List<ClassC>>(collectionB);
        Console.WriteLine(collectionB_mappedToCollectionC.GetString(nameof(collectionB_mappedToCollectionC)));
        Console.WriteLine();

        var collectionC_mappedToCollectionB = mapper.Map<List<ClassB>>(collectionC);
        Console.WriteLine(collectionC_mappedToCollectionB.GetString(nameof(collectionC_mappedToCollectionB)));
        Console.WriteLine();
    }
}
