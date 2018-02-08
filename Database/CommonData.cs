using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseNamespace
{
    public enum MailServerType
    {
        Disabled = 0,
        SMTP,
        Exchange2007_SP1,
        Exchange2010,
        Exchange2010_SP1,
        Exchange2010_SP2,
        Exchange2013,
        Exchange2013_SP1,
    }
    public enum ContractStatus
    {
        NotSigned = 0,
        Signed = 1,
    }

	public enum Roles
	{
		Admin = 0,
		Manager = 1,
		User = 2,
	}

	public class DbCredentials
	{
		public static string DbHost = "gdprhosted.database.windows.net";
		public static string DbName = "GDPR";
		public static string DbUser = "GDPR";
		public static string DbPassword = "qwerty12#3";
	}

}
