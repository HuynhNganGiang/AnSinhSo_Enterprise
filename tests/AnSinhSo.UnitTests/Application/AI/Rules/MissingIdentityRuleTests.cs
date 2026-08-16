using System;
using AnSinhSo.Application.AI.Rules.Citizens;
using AnSinhSo.Application.AI.Rules.Contexts;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;
using AnSinhSo.Domain.ValueObjects;
using Xunit;

namespace AnSinhSo.UnitTests.Application.AI.Rules;

public class MissingIdentityRuleTests
{
    private readonly MissingIdentityRule _rule;

    public MissingIdentityRuleTests()
    {
        _rule = new MissingIdentityRule();
    }

    [Fact]
    public void Evaluate_ShouldReturnMatch_WhenCitizenNumberIsEmpty()
    {
        // Arrange
        var citizenId = new CitizenId(Guid.NewGuid());
        var citizen = Citizen.Create(
            citizenId,
            FullName.Create("A", "Van", "Nguyen").Value,
            CitizenNumber.Create("012345678901").Value, // Valid number initially
            new DateTime(1990, 1, 1),
            Gender.Male,
            PhoneNumber.Create("0123456789").Value,
            Address.Create("123 Street", "Ward 1", "District 1", "Province A", PostalCode.Create("70000").Value).Value,
            Email.Create("test@example.com").Value
        ).Value;

        // Use reflection to set CitizenNumber to a missing value for testing the rule
        var field = typeof(Citizen).GetProperty("CitizenNumber");
        if (field != null)
        {
            var ctor = typeof(CitizenNumber).GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)[0];
            var emptyNumber = ctor.Invoke(new object[] { "" }) as CitizenNumber;
            field.SetValue(citizen, emptyNumber, null);
        }

        var context = new AiCitizenContext
        {
            Citizen = citizen,
            WelfareCases = []
        };

        // Act
        var result = _rule.Evaluate(context);

        // Assert
        Assert.True(result.Matched);
        Assert.Equal(_rule.Weight, result.Score);
        Assert.Equal(AiSeverity.Medium, result.Severity);
    }

    [Fact]
    public void Evaluate_ShouldNotMatch_WhenCitizenNumberIsNotEmpty()
    {
        // Arrange
        var citizenId = new CitizenId(Guid.NewGuid());
        var citizen = Citizen.Create(
            citizenId,
            FullName.Create("A", "Van", "Nguyen").Value,
            CitizenNumber.Create("012345678901").Value,
            new DateTime(1990, 1, 1),
            Gender.Male,
            PhoneNumber.Create("0123456789").Value,
            Address.Create("123 Street", "Ward 1", "District 1", "Province A", PostalCode.Create("70000").Value).Value,
            Email.Create("test@example.com").Value
        ).Value;

        var context = new AiCitizenContext
        {
            Citizen = citizen,
            WelfareCases = []
        };

        // Act
        var result = _rule.Evaluate(context);

        // Assert
        Assert.False(result.Matched);
    }
}
