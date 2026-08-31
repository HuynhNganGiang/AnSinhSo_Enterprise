using System.Threading;
using System.Threading.Tasks;

namespace AnSinhSo.Tools.Core;

public interface IToolCommand
{
    string Name { get; }
    string Description { get; }
    Task ExecuteAsync(string[] args, CancellationToken cancellationToken = default);
}
