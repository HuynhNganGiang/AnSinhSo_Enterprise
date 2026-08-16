using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AnSinhSo.Application.AI.Rules;
using AnSinhSo.Application.AI.Rules.Contexts;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Aggregates.CitizenAggregate.Enumerations;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate;
using AnSinhSo.Domain.Aggregates.HouseholdAggregate.Enumerations;

using AnSinhSo.Domain.ValueObjects;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using AnSinhSo.Infrastructure.Services.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AnSinhSo.UnitTests.Infrastructure.Services.AI;

public class AiAnalysisServiceTests
{
    private readonly DbContextOptions<AnSinhSoDbContext> _dbOptions;

    public AiAnalysisServiceTests()
    {
        _dbOptions = new DbContextOptionsBuilder<AnSinhSoDbContext>()
            .UseInMemoryDatabase($"AiAnalysisDb_{Guid.NewGuid()}")
            .Options;
    }

    private Citizen CreateCitizen(string cccd)
    {
        return Citizen.Create(
            new CitizenId(Guid.NewGuid()),
            FullName.Create("A", "Van", "Nguyen").Value,
            CitizenNumber.Create(cccd).Value,
            new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            Gender.Male,
            PhoneNumber.Create("0123456789").Value,
            Address.Create("123 Street", "Ward 1", "District 1", "Province A", PostalCode.Create("70000").Value).Value,
            Email.Create("test@example.com").Value
        ).Value;
    }

    private Household CreateHousehold()
    {
        return Household.Create(
            new HouseholdId(Guid.NewGuid()),
            new HouseholdCode("HH-001"),
            Address.Create("123 Street", "Ward 1", "District 1", "Province A", PostalCode.Create("70000").Value).Value
        ).Value;
    }

    [Fact]
    public async Task ScanAllAsync_ShouldGenerateRecommendations_WhenRulesMatch()
    {
        // Arrange
        using var dbContext = new AnSinhSoDbContext(_dbOptions);
        
        var citizen = CreateCitizen("012345678901");
        var household = CreateHousehold();
        
        var relationshipTypeId = new AnSinhSo.Domain.Aggregates.RelationshipTypeAggregate.RelationshipTypeId(Guid.NewGuid());
        household.AddMember(citizen.Id, relationshipTypeId, true);
        
        dbContext.Citizens.Add(citizen);
        dbContext.Households.Add(household);
        await dbContext.SaveChangesAsync();

        var mockHouseholdRule = new Mock<IAiRule<AiHouseholdContext>>();
        mockHouseholdRule.Setup(r => r.IsEnabled).Returns(true);
        mockHouseholdRule.Setup(r => r.RuleCode).Returns("AI-HH-001");
        mockHouseholdRule.Setup(r => r.RuleName).Returns("Test Household Rule");
        mockHouseholdRule.Setup(r => r.Evaluate(It.IsAny<AiHouseholdContext>()))
            .Returns(new AiRuleResult { Matched = true, Score = 50, Reason = "Household test matched", Severity = AiSeverity.Medium });

        var mockCitizenRule = new Mock<IAiRule<AiCitizenContext>>();
        mockCitizenRule.Setup(r => r.IsEnabled).Returns(true);
        mockCitizenRule.Setup(r => r.RuleCode).Returns("AI-CI-001");
        mockCitizenRule.Setup(r => r.RuleName).Returns("Test Citizen Rule");
        mockCitizenRule.Setup(r => r.Evaluate(It.IsAny<AiCitizenContext>()))
            .Returns(new AiRuleResult { Matched = true, Score = 60, Reason = "Citizen test matched", Severity = AiSeverity.Medium });

        var service = new AiAnalysisService(
            dbContext,
            new List<IAiRule<AiHouseholdContext>> { mockHouseholdRule.Object },
            new List<IAiRule<AiCitizenContext>> { mockCitizenRule.Object },
            new Mock<ILogger<AiAnalysisService>>().Object
        );

        // Act
        await service.ScanAllAsync(CancellationToken.None);

        // Assert
        var recommendations = await dbContext.AiRecommendations.ToListAsync();
        Assert.Equal(2, recommendations.Count);
        
        var hhRec = recommendations.SingleOrDefault(r => r.TargetType == AiTargetType.Household);
        Assert.NotNull(hhRec);
        Assert.Equal(household.Id.Value, hhRec.TargetId);
        Assert.Equal(50, hhRec.Score);

        var ciRec = recommendations.SingleOrDefault(r => r.TargetType == AiTargetType.Citizen);
        Assert.NotNull(ciRec);
        Assert.Equal(citizen.Id.Value, ciRec.TargetId);
        Assert.Equal(60, ciRec.Score);
    }
}
