using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Application.DataImport;

public interface IDataImportService
{
    Task ExecuteImportAsync(CancellationToken cancellationToken = default);
}
