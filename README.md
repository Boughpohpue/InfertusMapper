# InfertusMapper

A **lightweight, explicit object-to-object mapper for .NET**, inspired by AutoMapper but designed to stay **simple, predictable, and dependency-free**.

InfertusMapper focuses on:

* clear configuration
* compile-time safety where possible
* zero runtime magic
* minimal API surface

---

## Features

* Convention-based property mapping (same name + same type)
* Explicit per-member configuration (`ForMember`)
* Ability to ignore target members (`Ignore`)
* Support for manual mappings via delegate
* Compiled expression trees for runtime performance
* DI-friendly (`IServiceCollection` extensions)

---

## Installation

> (Once published)

```bash
dotnet add package Infertus.Mapper
```

---

## Basic Usage

### Define your models

```csharp
public class ClassA(double lat, double lon)
{
    public double Lat { get; set; } = lat;
    public double Lon { get; set; } = lon;
}

public class ClassB
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
```

---

### Create a mapping profile

Profiles are where all mapping configuration lives.

```csharp
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
```

#### What happens here?

* `CreateMap<TSource, TTarget>()`
  * Automatically maps properties with **matching name and type**
  * Requires a **parameterless constructor** on `TTarget`

* `ForMember`
  * Overrides or defines explicit mappings

* `Ignore`
  * Excludes a target property from mapping
  
* `RegisterMapping`
  * Allows full control using a delegate
  * Supports types **without parameterless constructors**

---

## Dependency Injection Setup

InfertusMapper integrates cleanly with `Microsoft.Extensions.DependencyInjection`.

### Option 1: Add profiles individually

```csharp
services
    .AddMappingProfile<TestProfile>()
    .AddMapper();
```

---

### Option 2: Add multiple profiles by type

```csharp
services
    .AddMappingProfileTypes(
        typeof(TestProfile),
        typeof(OtherTestProfile))
    .AddMapper();
```

---

### Option 3: One-liner setup

```csharp
services.AddMapper(
    typeof(TestProfile),
    typeof(OtherTestProfile));
```

---

## Using the Mapper

Inject or resolve `IMapper`:

```csharp
var mapper = provider.GetRequiredService<IMapper>();

var a = new ClassA(1.44, 69.3);
var b = mapper.Map<ClassB>(a);
```

Mapping is **type-safe, fast, and deterministic**.

---

## Important Notes & Design Decisions

### Parameterless constructor requirement

Mappings created via `CreateMap<TSource, TTarget>()` require:

```csharp
public TTarget()
```

This is due to expression tree compilation:

```csharp
Expression.New(typeof(TTarget))
```

If your target type **does not** have a default constructor, use:

```csharp
RegisterMapping<TSource, TTarget>(Func<TSource, TTarget>)
```

---

### Mapping compilation timing

Mappings are **compiled once** when:

```csharp
services.AddMapper();
```

All profiles **must be registered before** calling `AddMapper()`.

This is intentional:

* No runtime surprises
* No late-bound mappings
* Fail fast during startup

---

## Demo Application

The repository includes a demo console app showing:

* multiple profiles
* all registration styles
* bidirectional mappings
* ignored members
* delegate-based mappings

See:

```
Infertus.Mapper.DemoConsoleApp
```

---

## Why not AutoMapper?

InfertusMapper intentionally avoids:

* runtime reflection
* complex configuration graphs
* implicit behaviors
* magic conventions

This makes it ideal for:

* small to medium projects
* libraries
* performance-sensitive paths
* developers who prefer explicitness over flexibility

---

## Roadmap Ideas (Not Implemented)

* Constructor parameter mapping
* `ReverseMap()`
* Validation mode (detect unmapped members)

---

## License

GPL-3.0

---