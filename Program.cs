using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace crawler
{
    class Program
    {
        static void Main()
        {
        	Console.WriteLine("Paste the link to an album");
        	string url = Console.ReadLine();
        	url = url.Replace("https://vk.com/album", "");
        	string [] ids = url.Split(new Char [] {'_', ' '});
        	string api = "https://api.vk.com/method/photos.get.xml?owner_id=" + ids[0] + "&album_id=" + ids[1] + "&v=5.0&rev=0";
            StreamReader sendGet = new StreamReader(HttpWebRequest.Create(api).GetResponse().GetResponseStream());
            string str = sendGet.ReadToEnd();
            File.WriteAllText("pics.xml", str);

            XmlDocument doct = new XmlDocument();
            doct.Load("pics.xml");
            if( doct.SelectSingleNode("//error") !=null )
            {
            	string e = doct.DocumentElement.SelectSingleNode("//error_msg").InnerText;
            	Console.WriteLine(e);
            	File.Delete("pics.xml");
            	Console.ReadKey();
            	return;
            }
            else
            {
            	string n = doct.DocumentElement.SelectSingleNode("/response/count").InnerText;
            	Console.WriteLine("Album contains " + n + " pictures");
            }
            //XmlNode count = doct.DocumentElement.FirstChild;
            //Console.WriteLine("Album contains " + count.InnerText + " pictures");
            /*
             * 
            count="1000" - by default | offset= 
            /response count innertext | if (count > 1000) then offset=
            make a method for api request
            */
            XmlNodeList node = doct.SelectNodes("/response/items/photo"); // /response/photo for api v3 or  //photo
            foreach (XmlNode pic in node)
            {
            	using (StreamWriter sW = new StreamWriter("links.txt", true))
            	{
					if (pic["photo_2560"] != null)
						sW.WriteLine(pic["photo_2560"].InnerText);
					else if (pic["photo_1280"] != null)
						sW.WriteLine(pic["photo_1280"].InnerText);
					else if (pic["photo_807"] != null)
						sW.WriteLine(pic["photo_807"].InnerText);
					else if (pic["photo_604"] != null)
						sW.WriteLine(pic["photo_604"].InnerText);
					else if (pic["photo_130"] != null)
						sW.WriteLine(pic["photo_130"].InnerText);
					else if (pic["photo_75"] != null)
						sW.WriteLine(pic["photo_75"].InnerText);			
            	}
            }
            Console.WriteLine("Type path to save folder");
            string path = Console.ReadLine();
            string[] pics = File.ReadAllLines("links.txt");
            int c = 0;
            var watch = Stopwatch.StartNew();
			WebClient wc = new WebClient();            	
			foreach (string pic in pics)
			{
				string save_path = (path + '\u005c');
				string filename = Path.GetFileName(pic);
				wc.DownloadFile(pic, save_path + filename);
				c++;
			}
            watch.Stop();
			var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(c + " photos downloaded in " + elapsedMs + "ms");
            File.Delete("pics.xml");
            File.Delete("links.txt");
            Console.ReadKey();
             
        }
    }
}

