using ADACL.Models;

namespace ADACL;


public interface IDomainControllerService
{
    ValueTask<IEnumerable<DomainController>> GetDomainControllersAsync(Domain? domain);
}
