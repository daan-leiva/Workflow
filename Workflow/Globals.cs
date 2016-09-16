using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workflow
{
    public class Globals
    {
        public static readonly string odbc_connection_string = "DSN=jobboss32;UID=jbread;PWD=Cloudy2Day";
        public static readonly string binding_connection_string = "Server=ATI-SQL;Database=uniPoint_Live;UID=jbread;PWD=Cloudy2Day";
        public static readonly string generic_IT_error = "Problem connection to database server. Please contact IT support.";
        public static string userName = string.Empty;
        public static bool admin = false;
        public static bool customerServiceAccess = false;
        public static bool qaAccess = false;
        public static bool qeAccess = false;
        public static bool leadAccess = false;
        public static bool meAccess = false;
        public enum Status_Type { ContractReview, QuickRelease, None, Invalid };
    }
}
