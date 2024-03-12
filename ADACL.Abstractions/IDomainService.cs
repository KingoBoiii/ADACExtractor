using ADACL.Models;

namespace ADACL;

public interface IDomainService
{
    ValueTask<Domain?> GetComputerDomainAsync();
}
