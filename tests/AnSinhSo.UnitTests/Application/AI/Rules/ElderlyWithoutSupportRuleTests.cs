using System;
using System.Collections.Generic;
using AnSinhSo.Application.AI.Rules.Citizens;
using AnSinhSo.Application.AI.Rules.Contexts;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;
using AnSinhSo.Domain.ValueObjects;
using Xunit;

namespace AnSinhSo.UnitTests.Application.AI.Rules;

public class ElderlyWithoutSupportRuleTests
{
    private readonly ElderlyWithoutSupportRule _rule;

    public ElderlyWithoutSupportRuleTests()
    {
        _rule = new ElderlyWithoutSupportRule();
    }

    private Citizen CreateCitizen(DateTime birthDate)
    {
        return Citizen.Create(
            new CitizenId(Guid.NewGuid()),
            FullName.Create("A", "Van", "Nguyen").Value,
            CitizenNumber.Create("012345678901").Value,
            birthDate,
            Gender.Male,
            PhoneNumber.Create("0123456789").Value,
            Address.Create("123 Street", "Ward 1", "District 1", "Province A", PostalCode.Create("70000").Value).Value,
            Email.Create("test@example.com").Value
        ).Value;
    }

    [Fact]
    public void Evaluate_ShouldReturnMatch_WhenElderlyAndLivingAloneAndNoWelfare()
    {
        // Arrange
        var context = new AiCitizenContext
        {
            Citizen = CreateCitizen(DateTime.UtcNow.AddYears(-70)), // 70 years old
            IsLivingAlone = true,
            WelfareCases = []
        };

        // Act
        var result = _rule.Evaluate(context);

        // Assert
        Assert.True(result.Matched);
        Assert.Equal(_rule.Weight, result.Score);
        Assert.Equal(AiSeverity.Critical, result.Severity);
    }

    [Fact]
    public void Evaluate_ShouldNotMatch_WhenNotElderly()
    {
        // Arrange
        var context = new AiCitizenContext
        {
            Citizen = CreateCitizen(DateTime.UtcNow.AddYears(-60)), // 60 years old
            IsLivingAlone = true,
            WelfareCases = []
        };

        // Act
        var result = _rule.Evaluate(context);

        // Assert
        Assert.False(result.Matched);
    }

    [Fact]
    public void Evaluate_ShouldNotMatch_WhenNotLivingAlone()
    {
        // Arrange
        var context = new AiCitizenContext
        {
            Citizen = CreateCitizen(DateTime.UtcNow.AddYears(-70)), 
            IsLivingAlone = false,
            WelfareCases = []
        };

        // Act
        var result = _rule.Evaluate(context);

        // Assert
        Assert.False(result.Matched);
    }
}
