    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FISCA;
using FISCA.Presentation;

namespace ElectronicPaper
{
    public static class Program
    {
        [MainMethod]
        public static void Main() 
        {
            MotherForm.StartMenu["電子報表管理_彰商版"].Image = MotherForm.StartMenu["電子報表管理"].Image;
            MotherForm.StartMenu["電子報表管理_彰商版"].Click += 
                (sender,e) => new ElectronicPaperManager().ShowDialog();
        }

    }
}
