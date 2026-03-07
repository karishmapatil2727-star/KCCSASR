using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

/// <summary>
/// Summary description for DeolRoleProvider
/// </summary>
public class DeolRoleProvider : RoleProvider
{
    string[] Name;
    public override void AddUsersToRoles(string[] usernames, string[] roleNames)
    {
        throw new NotImplementedException();
    }

    public void AddUsersToRoles(string[] usernames, string[] roleNames, string currentUser)
    {
        using (DataContext db = new DataContext())
        {
            foreach (string roleName in roleNames)
            {
                foreach (string userName in usernames)
                {
                    var user = db.GetUser(userName);
                    var currentUserAccount = db.GetUser(currentUser);
                    var role = db.GetRoleByName(roleName, false);
                    if (user != null && role != null && currentUserAccount != null)
                    {
                        if (!db.IsUserExistInRole(role.RoleId, user.UserAccountId))
                        {
                            var roleUser = new RoleUserAccount
                            {
                                RUA_RoleId = role.RoleId,
                                RUA_InsertDate = DateTime.Now,
                                RUA_InsertUserAccountId = currentUserAccount.UserAccountId,
                                RUA_UpdateDate = DateTime.Now,
                                RUA_UpdateUserAccountId = currentUserAccount.UserAccountId,
                                RUA_UserAccountId = user.UserAccountId
                            };
                            db.AddUserToRole(roleUser);
                        }
                    }
                }
            }
        }
    }

    public override string ApplicationName
    {
        get
        {
            throw new NotImplementedException();
        }
        set
        {
            throw new NotImplementedException();
        }
    }

    public override void CreateRole(string roleName)
    {
        throw new NotImplementedException();
    }

    public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
    {
        throw new NotImplementedException();
    }

    public override string[] FindUsersInRole(string roleName, string usernameToMatch)
    {
        using (DataContext db = new DataContext())
        {
            int count = 0;
            var user = db.GetUser(usernameToMatch);
            var role = db.GetRoleByName(roleName, false);
            if (user != null && role != null)
            {
                if (db.IsUserExistInRole(role.RoleId, user.UserAccountId))
                {
                    Name[count] = usernameToMatch;
                    return Name;
                }
            }
            return Name;
        }
    }

    public override string[] GetAllRoles()
    {
        using (DataContext db = new DataContext())
        {
            Name = null;
            var roles = db.GetRoles();
            int count = 0;
            foreach (Role role in roles)
            {
                Name[count] = role.R_RoleName;
                count++;
            }
            return Name;
        }
    }

    public override string[] GetRolesForUser(string username)
    {
        using (DataContext db = new DataContext())
        {
            int count = 0;
            Name = null;
            var user = db.GetUser(username);
            var roleUsers = db.GetRolesForUser(user.UserAccountId, false);
            foreach (RoleUserAccount roleUser in roleUsers)
            {
                var role = db.GetRoles().SingleOrDefault(r => r.RoleId == roleUser.RUA_RoleId);
                Name[count] = role.R_RoleName;
            }
            return Name;
        }
    }

    public override string[] GetUsersInRole(string roleName)
    {
        using (DataContext db = new DataContext())
        {
            int count = 0;
            Name = null;
            var user = db.GetUser(roleName);
            var roleUsers = db.GetRolesForUser(user.UserAccountId, false);
            foreach (RoleUserAccount roleUser in roleUsers)
            {
                var role = db.GetRoles().SingleOrDefault(r => r.RoleId == roleUser.RUA_RoleId);
                Name[count] = role.R_RoleName;
            }
            return Name;
        }
    }

    public override bool IsUserInRole(string username, string roleName)
    {
        using (DataContext db = new DataContext())
        {
            var user = db.GetUser(username);
            var role = db.GetRoleByName(roleName, true);
            if (user != null && role != null)
            {
                if (db.IsUserExistInRole(role.RoleId, user.UserAccountId))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public bool IsUserInAnyRole(string username, string[] roleNames)
    {
        using (DataContext db = new DataContext())
        {
            foreach (string roleName in roleNames)
            {
                var user = db.GetUser(username);
                var role = db.GetRoleByName(roleName, true);
                if (user != null && role != null)
                {
                    if (db.IsUserExistInRole(role.RoleId, user.UserAccountId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }

    public bool IsUserInRole(string[] roleNames)
    {
        UserAccount user = new UserAccount();
        return user.IsInRole(roleNames);
    }

    public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
    {
        using (DataContext db = new DataContext())
        {
            foreach (string roleName in roleNames)
            {
                foreach (string userName in usernames)
                {
                    var user = db.GetUser(userName);
                    var role = db.GetRoleByName(roleName, false);
                    if (user != null && role != null)
                    {
                        if (!db.IsUserExistInRole(role.RoleId, user.UserAccountId))
                        {
                            db.DeleteUserFromRole(role.RoleId, user.UserAccountId);
                        }
                    }
                }
            }
        }
    }

    public override bool RoleExists(string roleName)
    {
        using (DataContext db = new DataContext())
        {
            var role = db.GetRoleByName(roleName, false);
            return role != null;
        }
    }
}