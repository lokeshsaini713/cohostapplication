using System.ComponentModel;

namespace Shared.Common.Enums
{
    public enum UserTypes
    {
        [Description("Admins")]
        Admin = 1,
        [Description("Users")]
        User = 2
    }
    public enum DeviceTypeEnum
    {
        [Description("Android")]
        Android = 1,

        [Description("IOS")]
        IOS = 2,

        [Description("Web")]
        Web = 3,
    }
}
