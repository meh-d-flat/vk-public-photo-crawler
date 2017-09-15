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
        	//https://vk.com/album-44792216_248342530
        	url = url.Replace("https://vk.com/album", "");
        	string [] ids = url.Split(new Char [] {'_', ' '});
        	string api = "https://api.vk.com/method/photos.get.xml?owner_id=" + ids[0] + "&album_id=" + ids[1];
            var sendGet = new StreamReader(HttpWebRequest.Create(api).GetResponse().GetResponseStream());
            string str = sendGet.ReadToEnd();
            File.WriteAllText("pics.xml", str);

            var doct = new XmlDocument();
            doct.Load("pics.xml");
            XmlNodeList node = doct.SelectNodes("/response/photo");
            foreach (XmlNode pic in node)
            {
            try
            {
                if (pic["src_xxxbig"] != null)
                {
                    using (StreamWriter streamWriter1 = new StreamWriter("links.txt", true))
                        streamWriter1.WriteLine(pic["src_xxxbig"].InnerText);
                }
                else
                    if (pic["src_xxbig"] != null)
                    {
                        using (StreamWriter streamWriter2 = new StreamWriter("links.txt", true))
                            streamWriter2.WriteLine(pic["src_xxbig"].InnerText);
                    }
                    else
                        if (pic["src_xbig"] != null)
                        {
                            using (StreamWriter streamWriter3 = new StreamWriter("links.txt", true))
                                streamWriter3.WriteLine(pic["src_xbig"].InnerText);
                        }
                        else
                            if (pic["src_big"] != null)
                            {
                                using (StreamWriter streamWriter4 = new StreamWriter("links.txt", true))
                                    streamWriter4.WriteLine(pic["src_big"].InnerText);
                            }

            }
            catch(Exception)
            {
                Console.WriteLine("ex");
            }

            }
            Console.WriteLine("type path to save folder");
            string path = Console.ReadLine();
            string[] pics = System.IO.File.ReadAllLines("links.txt");
            foreach (string pic in pics)
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                string save_path = (path + '\u005c');
                string filename = System.IO.Path.GetFileName(pic);
                wc.DownloadFile(pic, save_path + filename);
            }
            Console.WriteLine("Done!");
            Console.WriteLine("Want to generate an html gallery? [y/n]");
            string yo = Console.ReadLine();
            if (yo != null && yo == "y"){
            Process gallery = new  Process();
            gallery.StartInfo.FileName = (Directory.GetCurrentDirectory() + @"\resize.exe");
            gallery.StartInfo.Arguments = (path + '\u005c');
            gallery.StartInfo.WorkingDirectory = (path + '\u005c');
            gallery.Start();}
            else
            Console.ReadKey();

        }
    }
}
