using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLibrary
{
    public class Permission
    {
        public Guid Id { get; set; }
        public int User { get; set; }
        public string PermissionList { get; set; }
    }
}
