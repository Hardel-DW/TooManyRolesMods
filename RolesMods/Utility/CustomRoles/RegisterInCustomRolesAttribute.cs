using System;

namespace RolesMods.Utility.CustomRoles {

    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterInCustomRolesAttribute : Attribute {
        public RegisterInCustomRolesAttribute(Type Role) {
            Plugin.Logger.LogInfo("Test");
            Activator.CreateInstance(Role);
        }
    }
}
