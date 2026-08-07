using System;
using System.Linq.Expressions;
using AnSinhSo.Domain.Aggregates.CitizenAggregate;
using AnSinhSo.Domain.Specifications;

namespace AnSinhSo.Application.Citizens.Queries;

public class AllCitizensSpecification : Specification<Citizen>
{
    public override Expression<Func<Citizen, bool>> ToExpression()
    {
        return citizen => true;
    }
}

public class SearchCitizenSpecification : Specification<Citizen>
{
    private readonly string _keyword;

    public SearchCitizenSpecification(string keyword)
    {
        _keyword = (keyword ?? string.Empty).ToLower();
    }

    public override Expression<Func<Citizen, bool>> ToExpression()
    {
        return citizen => string.IsNullOrEmpty(_keyword) ||
                          citizen.CitizenNumber.Value.Contains(_keyword) ||
                          citizen.FullName.FirstName.ToLower().Contains(_keyword) ||
                          citizen.FullName.LastName.ToLower().Contains(_keyword);
    }
}
