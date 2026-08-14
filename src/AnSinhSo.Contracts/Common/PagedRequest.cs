namespace AnSinhSo.Contracts.Common;

public abstract record PagedRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? Sort { get; init; }
    public string? Keyword { get; init; }
}
