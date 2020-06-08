using System;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Net;
using System.IO;

namespace Yiban
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string atime = Convert.ToInt64(ts.TotalSeconds).ToString() + "." + Convert.ToInt64(ts.TotalMilliseconds).ToString().Substring(10, 2);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.yiban.cn/login/doLoginAjax");
            Encoding encoding = Encoding.UTF8;
            string param = string.Format("account={0}&password={1}&captcha={2}&keysTime={3}", txtUsername.Text, RSAEncrypt(txtPassword.Text), txtCaptcha.Text, atime);
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
            }
            MessageBox.Show(atime);
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
    }
}