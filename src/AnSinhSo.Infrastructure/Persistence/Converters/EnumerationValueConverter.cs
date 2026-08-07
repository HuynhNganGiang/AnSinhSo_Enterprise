using System.Linq;
using AnSinhSo.Domain.Enumerations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AnSinhSo.Infrastructure.Persistence.Converters;

/// <summary>
/// Converter chung cho tất cả các class Enumeration sang cơ sở dữ liệu.
/// </summary>
/// <typeparam name="TEnumeration">Kiểu Enumeration cụ thể.</typeparam>
public class EnumerationValueConverter<TEnumeration> : ValueConverter<TEnumeration, int>
    where TEnumeration : Enumeration
{
    public EnumerationValueConverter()
        : base(
            v => v.Id,
            v => Enumeration.GetAll<TEnumeration>().Single(e => e.Id == v))
    {
    }
}
