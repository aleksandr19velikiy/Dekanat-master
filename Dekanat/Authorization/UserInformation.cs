using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dekanat.Authorization
{
    [Serializable]
    class UserInformation
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
