using ADACExtractor.Models;

namespace ADACExtractor;

public interface IDomainService
{
    ValueTask<Domain?> GetComputerDomainAsync();
}
