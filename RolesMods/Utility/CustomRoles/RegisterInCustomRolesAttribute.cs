using System;
using System.Collections.Generic;
using System.Reflection;

namespace RolesMods.Utility.CustomRoles {

    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterInCustomRolesAttribute : Attribute {
        public static List<object> AllRoles = new List<object>();

        public RegisterInCustomRolesAttribute(Type Role) {
            ConstructorInfo ctor = Role.GetConstructor(Type.EmptyTypes);
            object Instance = ctor.Invoke(Type.EmptyTypes);
            AllRoles.Add(Instance);
        }

        public static void Register() {
            foreach (var Role in AllRoles) {
                if (Role != null) {
                    PropertyInfo Instance = Role.GetType().GetProperty("SetIntance");
                    if (Instance != null)
                        Instance.SetValue(Role, Role);
                }
            }
        }
    }
}
