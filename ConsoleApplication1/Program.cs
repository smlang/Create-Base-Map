using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Esri.Model.Map map = Esri.Model.Map.Import(@"D:\OS\Vector\ESRI\OSTerrain50 SD99\SJ99_line.shp");


            Esri.Model.Map map2 = Esri.Model.Map.Import(@"D:\OS\Vector\ESRI\OSTerrain50 SD99\SJ99_point.shp");
        }
    }
}
