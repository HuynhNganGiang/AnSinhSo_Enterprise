using System;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.Guards;
using AnSinhSo.Domain.SeedWork.Results;

namespace AnSinhSo.Domain.Aggregates.WelfareProgramAggregate;

public sealed class WelfareProgram : AggregateRoot<WelfareProgramId>
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

#pragma warning disable CS8618
    private WelfareProgram() { }
#pragma warning restore CS8618

    private WelfareProgram(WelfareProgramId id, string code, string name, string? description, bool isActive) : base(id)
    {
        Code = code;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    public static Result<WelfareProgram> Create(WelfareProgramId id, string code, string name, string? description)
    {
        Guard.Against.Empty(code, nameof(code));
        Guard.Against.Empty(name, nameof(name));

        return Result.Success(new WelfareProgram(id, code, name, description, true));
    }

    public void Update(string name, string? description)
    {
        Guard.Against.Empty(name, nameof(name));
        Name = name;
        Description = description;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
