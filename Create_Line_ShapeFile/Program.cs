using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotSpatial.Data;
using DotSpatial.Topology;
using System.IO;

namespace Create_Line_ShapeFile
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("請輸入轉換文字檔(.csv)之連結地址");
            var FileAddress = Console.ReadLine();
            try
            {
                int rows;
                //計算行數
                using (StreamReader read = new StreamReader(FileAddress, Encoding.Default))
                {
                    rows = (read.ReadToEnd().Split('\n').Length);
                }


                Feature f = new Feature();
                FeatureSet fs = new FeatureSet(f.FeatureType);
                Coordinate[] c = new Coordinate[(rows - 2) * 2];
                //ReadTxtFile
                System.Text.Encoding encode = System.Text.Encoding.GetEncoding("big5");
                StreamReader file = new StreamReader(FileAddress, encode);
                file.ReadLine();
                string line;
                var id = 0;
                while ((line = file.ReadLine()) != null)
                {
                    string[] ReadLine_Array = line.Split(',');
                    c[id] = new Coordinate(double.Parse(ReadLine_Array[1]), double.Parse(ReadLine_Array[2]));
                    c[id + 1] = new Coordinate(double.Parse(ReadLine_Array[3]), double.Parse(ReadLine_Array[4]));
                    id += 2;
                }
                LineString ls = new LineString(c);
                f = new Feature(ls);
                fs.Features.Add(f);
                fs.SaveAs("D:\\test_shapefile\\testline2.shp", true);
                Console.WriteLine("轉檔完畢");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
    }
}
