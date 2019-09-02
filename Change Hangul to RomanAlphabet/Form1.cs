using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Change_Hangul_to_RomanAlphabet
{
    public partial class Form1 : Form
    {
        private bool printMessage = true;
        public Form1()
        {
            InitializeComponent();
            Init();
        }

        // API Test
        private void Print(String name)
        {
            string query = name;
            printMessage = true;

            string apiID = "YOUR - CLIENT - ID";
            string apiPW = "YOUR - CLIENT - SECRET";

            try
            {
                string url = "https://openapi.naver.com/v1/krdict/romanization?query=" + query;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers.Add("X-Naver-Client-Id", apiID);
                request.Headers.Add("X-Naver-Client-Secret", apiPW);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string text = reader.ReadLine();
                string result = ParseJson(text);
                Debug.WriteLine(result);

                resultLabel.Text = result;
                resultLabel.Location = new Point((int)((this.Width / 2.0) - (resultLabel.Width / 2.0)), NameTextBox.Location.Y + NameTextBox.Height + 20);
            }
            catch(WebException e)
            {
                //MessageBox.Show("API ID 혹은 PW가 맞지 않습니다.");
                resultLabel.Text = "API ID 혹은 PW가 맞지 않습니다.";
                resultLabel.Location = new Point((int)((this.Width / 2.0) - (resultLabel.Width / 2.0)), NameTextBox.Location.Y + NameTextBox.Height + 20);
            }
        }

        private string ParseJson(string json)
        {
            string data = "";
            try
            {
                List<AItem> infos = new List<AItem>();
                JObject obj = JObject.Parse(json);
                JArray a = (JArray)obj["aResult"];

                IList<AResult> result = a.ToObject<IList<AResult>>();
                //Debug.WriteLine(result[0].aItems[0].Name);
                //Debug.WriteLine(result[0].aItems.Count);

                if(result[0].aItems[0].Name == null)
                {
                    throw new ArgumentOutOfRangeException();
                }

                foreach (AItem item in result[0].aItems)
                {
                    data += "Name : " + item.Name + "      " + "빈도수 : " + item.Score + "\n";
                }
            }
            catch(ArgumentOutOfRangeException e)
            {
                // MessageBox.Show("입력하신 이름은 영문으로 변환할 표본이 존재하지 않습니다.");
                return "입력하신 이름은 영문으로 변환할 표본이 존재하지 않습니다.";
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return " ";
            }
            return data;
        }


        // 시작할때 초기화 해주는 메서드
        private void Init()
        {
            this.Name = "한글 이름을 로마자로 변환";
            this.Text = "한글 이름을 로마자로 변환";
            this.StartPosition = FormStartPosition.CenterScreen;

            double x = (this.Width / 2.0) - (resultLabel.Width / 2.0);
            double y = NameTextBox.Location.Y + NameTextBox.Height + 20;

            NameTextBox.Location = new Point(this.Width / 2 - NameTextBox.Width / 2 , NameTextBox.Height / 2);
            nameBtn.Location = new Point(NameTextBox.Location.X + NameTextBox.Width + 10, NameTextBox.Location.Y);
            inputLabel.Location = new Point(NameTextBox.Location.X - inputLabel.Width - 5, NameTextBox.Location.Y);

            resultLabel.Location = new Point((int)x, (int)y);
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            char[] inputchars = NameTextBox.Text.ToCharArray();
            var sb = new StringBuilder();
            foreach (var item in inputchars)
            {
                if (char.GetUnicodeCategory(item) == UnicodeCategory.OtherLetter)
                {
                    sb.Append(item);
                }
            }
            NameTextBox.Text = sb.ToString().Trim();
        }

        private void NameBtn_Click(object sender, EventArgs e)
        {
            if(NameTextBox.TextLength == 0)
            {
                MessageBox.Show("이름이 없습니다.");
                return;
            }
            String name = NameTextBox.Text;
            Print(name);
        }

        private void NameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                NameBtn_Click(sender, e);
            }
        }
    }
}
