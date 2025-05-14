using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnoAuto2.Resource.Class
{
    class GlobalClass
    {
        public static int UserIDLoged;
        public static int RoleIDLoged;

        public static Users GetCurrentUser()
        {
            using (var db = new TechnoAutoEntities())
            {
                return db.Users.FirstOrDefault(u => u.user_id == UserIDLoged);
            }
        }
    } 
}
