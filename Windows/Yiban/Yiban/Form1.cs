using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Net;
using System.IO;

namespace Yiban
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "17677059909";
            textBox2.Text = "luoxinaini1314";
            string username = textBox1.Text;
            string password = RSAEncrypt(textBox2.Text);
            //MessageBox.Show(password);
            var atime = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000 - 15;
            MessageBox.Show(atime.ToString());

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.yiban.cn/login/doLoginAjax");
            Encoding encoding = Encoding.UTF8;
            string param = "account=" + username + "&password=" + password + "&captcha=&keysTime=" + atime;
            //encoding.GetBytes(postData);
            byte[] bs = Encoding.ASCII.GetBytes(param);
            string responseData = string.Empty;
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bs.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
                reqStream.Close();
            }
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    responseData = reader.ReadToEnd().ToString();
                }
                //context.Response.Write(responseData);
            }
            MessageBox.Show(responseData);
        }
        public static string RSAEncrypt(string content)
        {
            string publickey = @"<RSAKeyValue><Modulus>MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDy2DPjtWVgiHhT4lTiNIk4rhn2L7o8Zfy0ncIYYLjWzICiW+NuEnSvPm9bhBOdyzM/qYw6Jwk4X0cayOo1IBOjNKF9drI2eu+zCjwmmgFrS8WuwNjUw+UIhfB8DkmUccDCSNvIYcDg8oUZ1RvAXdB1NFwa56VJ4Xl8uv9o4HMsWwID</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(publickey);
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            return Convert.ToBase64String(cipherbytes);
        }

        public static string Post(string url, string content)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            #region 添加Post 参数
            byte[] data = Encoding.UTF8.GetBytes(content);
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}
