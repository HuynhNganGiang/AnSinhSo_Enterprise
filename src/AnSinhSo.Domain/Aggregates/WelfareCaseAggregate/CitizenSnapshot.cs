using System;
using System.Collections.Generic;
using AnSinhSo.Domain.SeedWork.Entities;
using AnSinhSo.Domain.SeedWork.ValueObjects;

namespace AnSinhSo.Domain.Aggregates.WelfareCaseAggregate;

public sealed class CitizenSnapshot : ValueObject
{
    public string CitizenNumber { get; private set; }
    public string FullName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string Gender { get; private set; }
    public string? HouseholdCode { get; private set; }
    public string Address { get; private set; }
    public string? Phone { get; private set; }
    public DateTime CreatedAtSnapshot { get; private set; }

#pragma warning disable CS8618
    // Required by EF Core
    private CitizenSnapshot() { }
#pragma warning restore CS8618

    private CitizenSnapshot(
        string citizenNumber,
        string fullName,
        DateTime dateOfBirth,
        string gender,
        string? householdCode,
        string address,
        string? phone,
        DateTime createdAtSnapshot)
    {
        CitizenNumber = citizenNumber;
        FullName = fullName;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        HouseholdCode = householdCode;
        Address = address;
        Phone = phone;
        CreatedAtSnapshot = createdAtSnapshot;
    }

    public static CitizenSnapshot Create(
        string citizenNumber,
        string fullName,
        DateTime dateOfBirth,
        string gender,
        string? householdCode,
        string address,
        string? phone)
    {
        return new CitizenSnapshot(
            citizenNumber,
            fullName,
            dateOfBirth,
            gender,
            householdCode,
            address,
            phone,
            DateTime.UtcNow);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return CitizenNumber;
        yield return FullName;
        yield return DateOfBirth;
        yield return Gender;
        yield return HouseholdCode;
        yield return Address;
        yield return Phone;
        yield return CreatedAtSnapshot;
    }
}
