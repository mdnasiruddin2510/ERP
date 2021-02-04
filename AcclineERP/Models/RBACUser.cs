using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain;
using Application.Interfaces;
using System.Reflection;
using Data.Context;
using System.Data.SqlClient;
using App.Domain.ViewModel;
using AcclineSysSet;

namespace AcclineERP.Models
{
    public class RBACUser
    {
        public int UserID { get; set; }
        public bool IsSysAdmin { get; set; }
        public string UserName { get; set; }
        private List<UserRole> Roles = new List<UserRole>();
        
        public RBACUser(string _username)
        {
            this.UserName = _username;
            this.IsSysAdmin = false;
            GetDatabaseUserRolesPermissions();
        }

        private void GetDatabaseUserRolesPermissions()
        {
            using (AcclineERPContext _data = new AcclineERPContext())
            {
                SecUserInfo _user = _data.SecUserInfo.Where(u => u.UserName == this.UserName).FirstOrDefault();
                if (_user != null)
                {
                    this.UserID = _user.UserID;
                    foreach (SecUserInGroup _group in _user.SecUserInGroups)
                    {

                        SecUserGroup _uGroup = _data.SecUserGroup.Where(u => u.GroupID == _group.GroupID).FirstOrDefault();

                        UserRole _userRole = new UserRole { Role_Id = _group.GroupID, RoleName = _uGroup.GroupName };

                        List<SecFormRight> _fRightList =  _data.SecFormRight.Where(u => u.GroupID == _group.GroupID).ToList();

                        foreach (SecFormRight _fRight in _fRightList)
                        {

                            SecFormProcess _fproc = _data.SecFormProcess.Where(u => u.FormProcessID == _fRight.FormProcessID).FirstOrDefault();
                            SecFormList _frmLst = _data.SecFormList.Where(u => u.FormID == _fproc.FormID).FirstOrDefault();

                            _userRole.Permissions.Add(new RolePermission { FormName = _frmLst.FormName, PermissionName = _fproc.ProcessName });
                        }
                        this.Roles.Add(_userRole);

                        //if (!this.IsSysAdmin)
                        //    this.IsSysAdmin = _group.IsSysAdmin;
                    }
                }

            }
                
            //}
        }

        public bool HasPermission(string requiredPermission)
        {
            bool bFound = false;
            foreach (UserRole role in this.Roles)
            {
                bFound = (role.Permissions.Where(p => p.PermissionName.ToLower() == requiredPermission.ToLower()).ToList().Count > 0);
                if (bFound)
                    break;
            }
            return bFound;
        }

        public bool HasPermissionToMenu(string requiredForm)
        {
            bool bFound = false;
            foreach (UserRole role in this.Roles)
            {
                bFound = (role.Permissions.Where(p => p.FormName.ToLower() == requiredForm.ToLower()).ToList().Count > 0);
                if (bFound)
                    break;
            }
            return bFound;
        }

        public bool HasRole(string role)
        {
            return (Roles.Where(p => p.RoleName == role).ToList().Count > 0);
        }

        public bool HasRoles(string roles)
        {
            bool bFound = false;
            string[] _roles = roles.ToLower().Split(';');
            foreach (UserRole role in this.Roles)
            {
                try
                {
                    bFound = _roles.Contains(role.RoleName.ToLower());
                    if (bFound)
                        return bFound;
                }
                catch (Exception)
                {
                }
            }
            return bFound;
        }

        public AcclineSysSet.Module HasModule()
        {
            AcclineSysSet.Module Mod = new AcclineSysSet.Module();
            return Mod;
        }

    }

    public class UserRole
    {
        public int Role_Id { get; set; }
        public string RoleName { get; set; }
        public List<RolePermission> Permissions = new List<RolePermission>();
    }

    public class RolePermission
    {        
        public string FormName { get; set; }
        public string PermissionName { get; set; }
    }

}
