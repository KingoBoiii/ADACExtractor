using ADACExtractor.Models;

namespace ADACExtractor;


public interface IDomainControllerService
{
    ValueTask<IEnumerable<DomainController>> GetDomainControllersAsync(Domain? domain);
}
