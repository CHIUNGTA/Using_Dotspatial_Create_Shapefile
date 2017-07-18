using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using DotSpatial.Data;
using DotSpatial.Topology;
using Newtonsoft.Json;

namespace Create_Point_ShapeFile
{
    class Program
    {
        static void Main(string[] args)
        {
            //載入你想要的文字包
            Console.WriteLine("請輸入轉換文字檔(.txt)之連結地址");
            string FileAddress = Console.ReadLine();
            int rows;
            using (StreamReader read = new StreamReader(FileAddress, Encoding.Default))
            {
                rows = (read.ReadToEnd().Split('\n').Length);
            }
            Coordinate[] coordinate = new Coordinate[rows];
            Random rnd = new Random();
            Feature f = new Feature();
            FeatureSet fs = new FeatureSet(f.FeatureType);


            //ReadTxtFile
            System.Text.Encoding encode = System.Text.Encoding.GetEncoding("big5");
            StreamReader file = new StreamReader(FileAddress, encode);
            //Create Success File & Fail File
            string line;
            var Id = 0;
            while ((line = file.ReadLine()) != null && Id <= 200)
            {
                //Rootobject obj = NewMethod(line);
                //將文字包內容，傳送給google map api進行經緯度換算
                string APIUrl = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&&sensor=false&language=zh-tw", line);
                var buffer = new WebClient().DownloadData(APIUrl);
                var json = Encoding.UTF8.GetString(buffer);
                var obj = JsonConvert.DeserializeObject<Rootobject>(json);
                try
                {
                    Console.WriteLine(Id + ":" + line + "," + obj.results[0].geometry.location.lat + "," + obj.results[0].geometry.location.lng);
                    coordinate[Id] = new Coordinate
                        (
                        obj.results[0].geometry.location.lng,
                        obj.results[0].geometry.location.lat
                        );
                    fs.Features.Add(coordinate[Id]);
                }
                catch
                {
                    Console.WriteLine($"第{Id}筆資料錯誤:" + line);
                }
                Id++;
            }
            Console.WriteLine("轉換完成");
            Console.ReadLine();
            fs.SaveAs("D:\\轉換完成.shp", true);
        }
    }
}
