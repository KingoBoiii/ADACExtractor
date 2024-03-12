using ADACL;
using Cocona;
using Microsoft.Extensions.Logging;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace ADACL.CLI.Commands;

public class ActiveDirectoryShowCommands(ILogger<ActiveDirectoryShowCommands> logger, IDomainService domainService) : CommandBase<ActiveDirectoryShowCommands>(logger)
{
    [Command("users")]
    public async ValueTask ShowUsersAsync()
    {
        var computerDomain = await domainService.GetComputerDomainAsync().ConfigureAwait(false);
        if (computerDomain is null)
        {
            Logger.LogError("No domain found");

            return;
        }

        using var principalContext = new PrincipalContext(ContextType.Domain, computerDomain.Name);

        using var userSearcher = new PrincipalSearcher(new UserPrincipal(principalContext));

        foreach (var result in userSearcher.FindAll())
        {
            var directoryEntry = (result?.GetUnderlyingObject() ?? default) as DirectoryEntry;
            if (directoryEntry is null)
            {
                continue;
            }

            var userPrincipal = await GetUserPrincipalAsync(principalContext, directoryEntry).ConfigureAwait(false);
            if (userPrincipal is null)
            {
                continue;
            }

            Logger.LogInformation(@"Group found '{} ({})'
    Guid:               {}
    Name:               {}
    Display Name:       {}
    SAM Account Name:   {}
    User Principal Name:{}
    Description:        {}",
                userPrincipal.Name, userPrincipal.Guid, userPrincipal.Guid, userPrincipal.Name, userPrincipal.DisplayName, userPrincipal.SamAccountName,
                userPrincipal.UserPrincipalName, userPrincipal.Description);

        }
    }

    [Command("groups")]
    public async ValueTask ShowGroupsAsync()
    {
        var computerDomain = await domainService.GetComputerDomainAsync().ConfigureAwait(false);
        if (computerDomain is null)
        {
            Logger.LogError("No domain found");

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

    private ValueTask<UserPrincipal?> GetUserPrincipalAsync(PrincipalContext principalContext, DirectoryEntry directoryEntry)
    {
        var userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, directoryEntry.Properties["samAccountName"].Value?.ToString() ?? string.Empty);
        if (userPrincipal is null)
        {
            Logger.LogWarning("Cannot find group by 'samAccountName', trying with objectGUID...");

            userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.Guid, directoryEntry.Properties["objectGUID"].Value?.ToString() ?? string.Empty);
        }

        return ValueTask.FromResult<UserPrincipal?>(userPrincipal);
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
