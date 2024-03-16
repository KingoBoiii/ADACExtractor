namespace System.DirectoryServices;

public interface IDomainProvider
{
    ValueTask<IDomainContainer> GetDomainAsync();
}
