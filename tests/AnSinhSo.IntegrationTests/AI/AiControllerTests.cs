using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AnSinhSo.Domain.Aggregates.AiRecommendationAggregate;
using AnSinhSo.IntegrationTests.Authentication;
using AnSinhSo.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using FluentAssertions;

namespace AnSinhSo.IntegrationTests.AI;

public class AiControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public AiControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
        
        // Use Mock Authentication as System Admin for Permissions
        _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "SystemAdmin");
    }

    private async Task SeedRecommendationsAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AnSinhSoDbContext>();

        var reasons = new List<AiReason>
        {
            new AiReason("AI-HH-001", "Poor Household", 50, "Matches poor household criteria")
        };

        var recommendation1 = AiRecommendation.Create(
            "AI-20260816-000001",
            AiTargetType.Household,
            Guid.NewGuid(),
            "1.0",
            AiCategory.Suggestion,
            85,
            AiConfidence.High,
            "High Risk Household",
            "Summary",
            reasons);

        var recommendation2 = AiRecommendation.Create(
            "AI-20260816-000002",
            AiTargetType.Citizen,
            Guid.NewGuid(),
            "1.0",
            AiCategory.Warning,
            45,
            AiConfidence.Medium,
            "Medium Risk Citizen",
            "Summary",
            reasons);

        db.AiRecommendations.Add(recommendation1);
        db.AiRecommendations.Add(recommendation2);
        await db.SaveChangesAsync();
    }

    [Fact]
    public async Task ScanAll_ShouldReturnOk()
    {
        // Act
        var response = await _client.PostAsync("/api/v1/ai/scan-all", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetDashboardSummary_ShouldReturnCorrectData()
    {
        // Arrange
        await SeedRecommendationsAsync();

        // Act
        var response = await _client.GetAsync("/api/v1/ai/dashboard-summary");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("totalRecommendations");
        content.Should().Contain("highRisk");
        content.Should().Contain("mediumRisk");
    }

    [Fact]
    public async Task GetRecommendations_ShouldReturnList()
    {
        // Arrange
        await SeedRecommendationsAsync();

        // Act
        var response = await _client.GetAsync("/api/v1/ai/recommendations");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("AI-20260816-000001");
    }
}
