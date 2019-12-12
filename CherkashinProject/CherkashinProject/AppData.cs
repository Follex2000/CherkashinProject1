using CherkashinProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CherkashinProject
{
    class AppData
    {
        public static Frame MainFrame;
        public static Entity.linecoreBaseEntities Context = new Entity.linecoreBaseEntities();
        public static WindowAddEdit WindowAddEdit = new WindowAddEdit();
        public static Users currentUser = null;
    }
}
