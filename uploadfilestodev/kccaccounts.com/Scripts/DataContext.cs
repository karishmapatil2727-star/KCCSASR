using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Linq;
using System.Data;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.Security;
using System.Web;
using System.Data.Objects;
using System.Data.EntityClient;
using System.Data.EntityModel;
using UniversityDAL;
//using ExamNodalCentre;


// we need UniversitySolutionDataContext database connection in 
public class DataContext : DbContext
{
    Security security = new Security();
    public DbSet<UserAccount> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleUserAccount> RoleUserAccounts { get; set; }
    public DbSet<MenuItemCustom> MenuItemCustoms { get; set; }
    public DbSet<RoleMenuItem> RoleMenuItems { get; set; }
    // Helper methods. User can also directly access "Users" property
    public void AddUser(UniversityDAL.UserAccount user)
    {
        using (EntitiesModel db = new EntitiesModel())
        {
            db.Add(user);
            db.SaveChanges();
        }
        ////user.UserAccountId = InNewRecord(user.UserAccountId);
        //Users.Add(user);
        ////Entry(user).State = EntityState.Added;
        //SaveChanges();
    }

    protected int InNewRecord(int id)
    {
        if (id == 0)
        {
            var user = Users.ToList().OrderByDescending(u => u.UserAccountId).FirstOrDefault();
            if (user != null)
                return user.UserAccountId + 1;
            return 1;
        }
        return id;
    }

    public void Update(UserAccount user)
    {
        Users.Add(user);
        Entry(user).State = EntityState.Modified;
        SaveChanges();
    }


    public bool ChangePassword(EntitiesModel db, string username, string newPassword)
    {
        db.SaveChanges();
        return true;
    }

    public string UserPassword(string username)
    {
        using (EntitiesModel db = new EntitiesModel())
        {
            var password = (from u in db.UserAccounts
                            where u.UA_AccountName == username
                            select new
                            {
                                Password = security.Decrypt(u.UA_AccountPassword)
                            }).FirstOrDefault();

            return password.Password;
        }
    }

    public string UserCorrespondencePassword(string username)
    {
        using (EntitiesModel db = new EntitiesModel())
        {
            var password = (from u in db.UserAccounts
                            where u.UA_AccountName == username
                            select new
                            {
                                Password = security.Decrypt(u.UA_CorrespondencePassword)
                            }).FirstOrDefault();

            return password.Password;
        }
    }
    public string UserCorrespondencePassword(int UserAccountId)
    {
        using (EntitiesModel db = new EntitiesModel())
        {
            var password = (from u in db.UserAccounts
                            where u.UserAccountId == UserAccountId
                            select new
                            {
                                Password = u.UA_CorrespondencePassword
                            }).FirstOrDefault();

            return password.Password;
        }
    }
    public string UserPassword(int userId)
    {
        using (EntitiesModel db = new EntitiesModel())
        {
            var password = db.UserAccounts.FirstOrDefault(p => p.UserAccountId == userId);
            return security.Decrypt(password.UA_AccountPassword);
        }
    }

    public void Delete(UserAccount user)
    {
        Users.Remove(user);
        Entry(user).State = EntityState.Deleted;
        SaveChanges();
    }

    public UserAccount GetUser(string userName)
    {
        var user = Users.SingleOrDefault(u => u.UA_AccountName == userName);
        return user;
    }

    public UniversityDAL.UserAccount GetUser(ref EntitiesModel db, int userAccountId)
    {
        return db.UserAccounts.SingleOrDefault(u => u.UserAccountId == userAccountId);
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        base.OnModelCreating(modelBuilder);
    }

    public UserAccount GetUser(int userAccountId)
    {
        var user = Users.SingleOrDefault(u => u.UserAccountId == userAccountId);
        return user;
    }

    public UserAccount GetUserByUserIdPassword(string userName, string password)
    {
        var user = Users.SingleOrDefault(u => u.UA_AccountName == userName && u.UA_AccountPassword == password && u.UA_IsActive==true);
        if (user != null)
        {
            user.UA_SecondLastLoginDate = user.UA_LastLoginDate;
            user.UA_LastLoginDate = DateTime.Now;
            this.SaveChanges();
            user.Roles = LoadRoleByUserAccountID(user.UserAccountId);
        }
        return user;
    }
    public UserAccount GetUserByUserIdPasswordForCorrespondenceAuthentication(int UserAccountID, string password)
    {
        var user = Users.SingleOrDefault(u => u.UserAccountId == UserAccountID && u.UA_CorrespondencePassword == password);
        if (user != null)
        {
            user.UA_SecondLastLoginDate = user.UA_LastLoginDate;
            user.UA_LastLoginDate = DateTime.Now;
            this.SaveChanges();
            user.Roles = LoadRoleByUserAccountID(user.UserAccountId);
        }
        return user;
    }



    public UserAccount GetUserByUserAccountIDPassword(int UserAccountID, string password)
    {
        var user = Users.SingleOrDefault(u => u.UserAccountId == UserAccountID && u.UA_AccountPassword == password);
        if (user != null)
        {
            user.UA_SecondLastLoginDate = user.UA_LastLoginDate;
            user.UA_LastLoginDate = DateTime.Now;
            this.SaveChanges();
            user.Roles = LoadRoleByUserAccountID(user.UserAccountId);
        }
        return user;
    }


    public string[] LoadRoleByUserAccountID(int userAccountId)
    {
        string[] roles = (from ru in this.RoleUserAccounts
                          join r in this.Roles on ru.RUA_RoleId equals r.RoleId into role
                          from r in role.DefaultIfEmpty()
                          where ru.RUA_UserAccountId == userAccountId
                          select r.R_RoleShortName).ToArray();
        //string[] array = new string[roles.Count];
        //int count = 0;
        //foreach (Role role in roles)
        //{
        //    array[count] = role.R_RoleShortName;
        //    count++;
        //}
        return roles;
    }

    public string[] LoadRole(int userAccountId)
    {
        List<Role> roles = (from ru in this.RoleUserAccounts
                            join r in this.Roles on ru.RUA_RoleId equals r.RoleId into role
                            from r in role.DefaultIfEmpty()
                            where ru.RUA_UserAccountId == userAccountId
                            select r).ToList();
        string[] array = new string[roles.Count];
        int count = 0;
        foreach (Role role in roles)
        {
            array[count] = role.R_RoleShortName;
            count++;
        }
        return array;
    }

    public bool LoadRoleWithDate(int userAccountId, int menuCode)
    {
        var query = (from m in this.RoleMenuItems
                     join ru in this.RoleUserAccounts on m.RM_RoleID equals ru.RUA_RoleId into roleUser
                     from ru in roleUser.DefaultIfEmpty()
                     where ru.RUA_UserAccountId == userAccountId && m.RM_MenuItemCode == menuCode
                     && m.RM_IsCustom == false && m.RM_CustomUserAccountID == 0
                     && m.RM_StartDate <= DateTime.Now
                     && m.RM_EndDate >= DateTime.Now

                     select new
                     {
                         m.RoleMenuItemId
                     }).ToList();

        return query.Count > 0;
    }

    public bool LoadCustomMenu(int userAccountId, int menuCode)
    {
        var query = (from m in RoleMenuItems
                     where m.RM_MenuItemCode == menuCode && m.RM_CustomUserAccountID == userAccountId && m.RM_IsCustom == true && m.RM_RoleID == 0
                     && m.RM_StartDate <= DateTime.Now
                     && m.RM_EndDate >= DateTime.Now
                     select new
                     {
                         m.RoleMenuItemId
                     }).ToList();

        return query.Count > 0;
    }


    public string RoleName(int roleId)
    {
        using (DataContext db = new DataContext())
        {
            var role = Roles.SingleOrDefault(r => r.RoleId == roleId);
            if (role != null)
                return role.R_RoleShortName;
            return String.Empty;
        }
    }

    public UserAccount GetUserByEmailAddress(string email)
    {
        var user = Users.SingleOrDefault(u => u.UA_EmailAddress == email);
        return user;
    }

    public UniversityDAL.UserAccount GetUserByEmailAddress(int userId, string email)
    {
        using (EntitiesModel db = new EntitiesModel())
        {
            return db.UserAccounts.SingleOrDefault(u => u.UA_EmailAddress == email && u.UserAccountId != userId);
        }
    }

    public List<UniversityDAL.RoleMenuItem> GetRoleMenuItem()
    {
        using (EntitiesModel db = new EntitiesModel())
        {
            return db.RoleMenuItems.ToList();
        }
    }

    public Role GetRole(string roleShortName)
    {
        var role = Roles.SingleOrDefault(r => r.R_RoleShortName == roleShortName);
        return role;
    }

    public void CreateRole(Role role)
    {
        this.Roles.Add(role);
        SaveChanges();
    }

    public void DeleteRole(Role role)
    {
        this.Roles.Remove(role);
        SaveChanges();
    }

    public List<Role> GetRoles()
    {
        return this.Roles.ToList();
    }

    public List<RoleUserAccount> GetRolesForUser(int id, bool IsByRoleName)
    {
        List<RoleUserAccount> roleUserAccount = new List<RoleUserAccount>();
        if (!IsByRoleName)
            roleUserAccount = this.RoleUserAccounts.Where(ru => ru.RUA_UserAccountId == id).ToList();
        else
            roleUserAccount = this.RoleUserAccounts.Where(ru => ru.RUA_RoleId == id).ToList();
        return roleUserAccount;
    }

    public void AddUserToRole(RoleUserAccount roleUser)
    {
        this.RoleUserAccounts.Add(roleUser);
        SaveChanges();
    }

    public bool IsUserExistInRole(int roleId, int userId)
    {
        var roleUserAccount = RoleUserAccounts.SingleOrDefault(ru => ru.RUA_RoleId == roleId && ru.RUA_UserAccountId == userId);
        return roleUserAccount != null;
    }

    public void DeleteUserFromRole(int roleId, int userId)
    {
        var roleUserAccount = RoleUserAccounts.SingleOrDefault(ru => ru.RUA_RoleId == roleId && ru.RUA_UserAccountId == userId);
        this.RoleUserAccounts.Remove(roleUserAccount);
        SaveChanges();
    }

    public Role GetRoleByName(string roleName, bool IsByShortname)
    {
        Role role = new Role();
        if (!IsByShortname)
            role = Roles.SingleOrDefault(r => r.R_RoleName == roleName);
        else
            role = Roles.SingleOrDefault(r => r.R_RoleShortName == roleName);
        return role;
    }

    public WebUserAccount LoadWebUser(int userAccountId)
    {
        LoginLogs log = new LoginLogs();
        DataSet ds = log.LoginLoginGetDefaultValuesForCurrentUser(userAccountId);

        WebUserAccount user = new WebUserAccount();
        user.ID = userAccountId;
        user.RoleId = Convert.ToInt32(ds.Tables[0].Rows[0]["RoleId"]);
        user.UA_AccountName = ds.Tables[0].Rows[0]["UA_AccountName"].ToString();
        user.UA_FacUserId = Convert.ToInt32(ds.Tables[0].Rows[0]["UA_FacUserId"]);
        user.UA_InstId = Convert.ToInt32(ds.Tables[0].Rows[0]["UA_InstId"]);
        user.UA_DisplayName = ds.Tables[0].Rows[0]["UA_DisplayName"].ToString();
        user.UA_LastLoginDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["UA_LastLoginDate"]);
        user.UA_LastPasswordChargedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["UA_LastPasswordChargedDate"]);
        user.UA_SecondLastLoginDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["UA_SecondLastLoginDate"]);
        user.UA_UserType = ds.Tables[0].Rows[0]["UA_UserType"].ToString();
        user.InstShortname = ds.Tables[0].Rows[0]["InstShortname"].ToString();
        user.UserAccountId = userAccountId;
        user.InstName = ds.Tables[0].Rows[0]["InstName"].ToString();
        user.StudentBranchId = Convert.ToInt32(ds.Tables[0].Rows[0]["StudentBranchId"]);
        user.StudentSemId = Convert.ToInt32(ds.Tables[0].Rows[0]["StudentSemId"]);
        user.DepartmentID = Convert.ToInt32(ds.Tables[0].Rows[0]["DepartmentID"]);
        if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
        {
            int[] Collection = new int[ds.Tables[1].Rows.Count];
            int count = 0;
            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                Collection[count] = Convert.ToInt32(dr["RUA_RoleId"].ToString());
                count++;
            }
            user.UserRoles = Collection;
        }
        user.FinId = Convert.ToInt16(ds.Tables[2].Rows[0]["Fin_ID"].ToString());
        user.UA_InstType = Convert.ToInt32(ds.Tables[0].Rows[0]["UA_InstType"].ToString());
        user.LeaveSessionId = Convert.ToInt32(ds.Tables[3].Rows[0]["LS_ID"].ToString());
        return user;
    }
}