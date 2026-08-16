using System;
using System.Collections.Generic;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;

namespace AnSinhSo.Application.AI.Rules.Contexts;

public class AiHouseholdContext
{
    public Household Household { get; set; } = null!;
    public List<Citizen> Members { get; set; } = new();
    public List<WelfareCase> WelfareCases { get; set; } = new();
    public decimal TotalEstimatedMonthlyIncome { get; set; } // Simplified for rules
}

public class AiCitizenContext
{
    public Citizen Citizen { get; set; } = null!;
    public bool IsLivingAlone { get; set; }
    public List<WelfareCase> WelfareCases { get; set; } = new();
}
