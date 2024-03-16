namespace System.DirectoryServices;

public interface IDirectoryServiceContainer
{
    string? ErrorMessage { get; }
    bool IsValid { get; }
}