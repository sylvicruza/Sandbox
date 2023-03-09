using System;
using System.Security.Permissions;
using System.Security;

namespace SandboxDotNetv2._0._0._0
{
    [Serializable]
    public class CustomPermission : CodeAccessPermission
    {
        public CustomPermission()
        {
          
        }

        public override IPermission Copy()
        {
            return new CustomPermission();
        }

        public override void FromXml(SecurityElement securityElement)
        {
            throw new NotImplementedException();
        }

        public override SecurityElement ToXml()
        {
            SecurityElement xmlElement = new SecurityElement("MySecurityElement");
            return xmlElement;
        }

        public override bool IsSubsetOf(IPermission target)
        {
            if (target == null)
                return false;

            if (target.GetType() != GetType())
                return false;

            return true;
        }

        public override IPermission Intersect(IPermission target)
        {
            if (target == null)
                return null;

            if (target.GetType() != GetType())
                return null;

            return Copy();
        }

        public override IPermission Union(IPermission target)
        {
            if (target == null)
                return Copy();

            if (target.GetType() != GetType())
                throw new ArgumentException("Invalid permission type.", "target");

            return Copy();
        }

        public PermissionSet AllAccess()
        {
            return new PermissionSet(PermissionState.Unrestricted);

        }



    }

}

