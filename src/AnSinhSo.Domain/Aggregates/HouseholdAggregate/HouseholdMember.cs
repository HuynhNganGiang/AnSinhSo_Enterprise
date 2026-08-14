using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate;
using AnSinhSo.Domain.SeedWork.Entities;

namespace AnSinhSo.Domain.Aggregates.HouseholdAggregate;

/// <summary>
/// Thực thể đại diện cho một thành viên trong hộ gia đình.
/// </summary>
public sealed class HouseholdMember : Entity<HouseholdMemberId>
{
    /// <summary>
    /// Định danh của công dân.
    /// </summary>
    public CitizenId CitizenId { get; private set; }

    /// <summary>
    /// Cho biết thành viên này có phải là chủ hộ hay không.
    /// </summary>
    public bool IsHead { get; private set; }

    /// <summary>
    /// Định danh của loại quan hệ (RelationshipType).
    /// </summary>
    public RelationshipTypeId RelationshipTypeId { get; private set; }

    /// <summary>
    /// Constructor ẩn dành cho EF Core.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private HouseholdMember()
    {
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Khởi tạo một thành viên hộ gia đình mới.
    /// </summary>
    /// <param name="id">Định danh thành viên hộ gia đình.</param>
    /// <param name="citizenId">Định danh công dân.</param>
    /// <param name="relationshipTypeId">Định danh loại quan hệ.</param>
    /// <param name="isHead">Có phải chủ hộ không.</param>
    internal HouseholdMember(HouseholdMemberId id, CitizenId citizenId, RelationshipTypeId relationshipTypeId, bool isHead)
    {
        Id = id;
        CitizenId = citizenId;
        RelationshipTypeId = relationshipTypeId;
        IsHead = isHead;
    }

    /// <summary>
    /// Cập nhật trạng thái chủ hộ.
    /// </summary>
    /// <param name="isHead">Trạng thái chủ hộ mới.</param>
    internal void SetHeadStatus(bool isHead)
    {
        IsHead = isHead;
    }

    internal void ChangeRelationship(RelationshipTypeId relationshipTypeId)
    {
        RelationshipTypeId = relationshipTypeId;
    }
}
