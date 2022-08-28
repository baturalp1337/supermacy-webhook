using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Collections.Specialized;

namespace supermacy_webhook
{
    public class main
    {
        private static string webhookurl = "";
        private static bool webhookisinitiliazed = false;
        private static WebClient wc = new WebClient();
        private static HttpClient hc = new HttpClient();

        public static bool initwebhook(string url)
        {
            if (url == "")
            {
                webhookisinitiliazed = false;
                Console.WriteLine("[FATAL ERROR] Webhook is not valid!");
                return false;
            }
            else
            {
                webhookurl = url;
                webhookisinitiliazed = true;
                return true;
            }
        }
        public static bool sendmsg(string content)
        {
            try
            {
                if (webhookisinitiliazed)
                {
                    wc.UploadValues(webhookurl, new NameValueCollection
                {
                    {
                        "content", content
                    }
                }
                );
                    return true;
                }
                else
                {
                    Console.WriteLine("[FATAL ERROR] Webhook is not initiliazed!");
                    return false;
                }               
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FATAL ERROR] " + ex.Message.ToString());
                return false;
            }
        }
        public static bool sendimg(string filepath)
        {
            try
            {
                if (webhookisinitiliazed)
                {
                    MultipartFormDataContent content = new MultipartFormDataContent();
                    byte[] img = File.ReadAllBytes(filepath);
                    content.Add(new ByteArrayContent(img, 0, img.Length), Path.GetExtension(filepath), filepath);
                    hc.PostAsync(webhookurl, content).Wait();
                    hc.Dispose();
                    return true;
                }
                else
                {
                    Console.WriteLine("[FATAL ERROR] Webhook is not initiliazed!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[FATAL ERROR] " + ex.Message.ToString());
                return false;
            }
        }
    }
}
