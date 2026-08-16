using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Results;
using MediatR;

namespace AnSinhSo.Application.AI.Queries.GetAiDashboardSummary;

public class GetAiDashboardSummaryQuery : IRequest<Result<AiDashboardSummaryDto>>
{
}

public class AiDashboardSummaryDto
{
    public int TotalRecommendations { get; set; }
    public int HighRisk { get; set; }
    public int MediumRisk { get; set; }
    public int LowRisk { get; set; }
    public int PoorHouseholds { get; set; }
    public int DuplicatedBenefits { get; set; }
    public int ElderlyWithoutSupport { get; set; }
    public int MissingIdentity { get; set; }
    public int Reviewed { get; set; }
    public int Pending { get; set; }
    
    public List<TopTriggeredRuleDto> TopTriggeredRules { get; set; } = new();
}

public class TopTriggeredRuleDto
{
    public string RuleCode { get; set; } = string.Empty;
    public int Count { get; set; }
}
