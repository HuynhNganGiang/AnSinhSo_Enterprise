using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate;

/// <summary>
/// Thực thể đại diện cho Loại Quan Hệ (Chủ hộ, Vợ, Chồng, Con, v.v.)
/// </summary>
public sealed class RelationshipType : AggregateRoot<RelationshipTypeId>
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public string Description { get; private set; }
    
    // Master data thường chỉ kích hoạt / vô hiệu hóa
    public bool IsActive { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private RelationshipType()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private RelationshipType(RelationshipTypeId id, string name, string code, string description)
    {
        Id = id;
        Name = name;
        Code = code;
        Description = description;
        IsActive = true;
    }

    public static RelationshipType Create(RelationshipTypeId id, string name, string code, string description)
    {
        return new RelationshipType(id, name, code, description);
    }
}
