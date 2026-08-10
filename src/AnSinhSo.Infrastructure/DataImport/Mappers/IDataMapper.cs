using AnSinhSo.Infrastructure.DataImport.Models;

namespace AnSinhSo.Infrastructure.DataImport.Mappers;

public interface IDataMapper<in TDto, TEntity>
{
    ImportResult<TEntity> Map(ImportContext context, TDto dto);
}
