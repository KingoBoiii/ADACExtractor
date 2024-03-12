using Cocona;
using Microsoft.Extensions.Logging;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace ADACExtractor.CLI.Commands;

public class ActiveDirectoryShowCommands(ILogger<ActiveDirectoryShowCommands> logger) : ActiveDirectoryCommandBase<ActiveDirectoryShowCommands>(logger)
{
    [Command("groups")]
    public async ValueTask InspectGroupsAsync()
    {
        if (!TryGetComputerDomain(out var computerDomain, out var errorMessage))
        {
            Logger.LogError("No domain found: {}", errorMessage);

            return;
        }

        using var principalContext = new PrincipalContext(ContextType.Domain, computerDomain.Name);

        using var groupSearcher = new PrincipalSearcher(new GroupPrincipal(principalContext));

        foreach (var result in groupSearcher.FindAll())
        {
            var directoryEntry = (result?.GetUnderlyingObject() ?? default) as DirectoryEntry;
            if (directoryEntry is null)
            {
                continue;
            }

            var groupPrincipal = await GetGroupPrincipalAsync(principalContext, directoryEntry).ConfigureAwait(false);
            if (groupPrincipal is null)
            {
                continue;
            }

            Logger.LogInformation(@"Group found '{} ({})'
    Guid:               {}
    Name:               {}
    Display Name:       {}
    SAM Account Name:   {}
    User Principal Name:{}
    Description:        {}
    Member Count:       {}
    Is Security Group:  {}
    Group Scope:        {}", 
                groupPrincipal.Name, groupPrincipal.Guid, groupPrincipal.Guid, groupPrincipal.Name, groupPrincipal.DisplayName, groupPrincipal.SamAccountName,
                groupPrincipal.UserPrincipalName, groupPrincipal.Description, groupPrincipal.Members.Count, groupPrincipal.IsSecurityGroup, groupPrincipal.GroupScope.ToString());
            
        }
    }

    private ValueTask<GroupPrincipal?> GetGroupPrincipalAsync(PrincipalContext principalContext, DirectoryEntry directoryEntry)
    {
        var groupPrincipal = GroupPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, directoryEntry.Properties["samAccountName"].Value?.ToString() ?? string.Empty);
        if (groupPrincipal is null)
        {
            Logger.LogWarning("Cannot find group by 'samAccountName', trying with objectGUID...");

            groupPrincipal = GroupPrincipal.FindByIdentity(principalContext, IdentityType.Guid, directoryEntry.Properties["objectGUID"].Value?.ToString() ?? string.Empty);
        }

        return ValueTask.FromResult<GroupPrincipal?>(groupPrincipal);
    }
}
