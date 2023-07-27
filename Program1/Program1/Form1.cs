using Library1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program1
{
    public partial class Form1 : Form
    {
        public TcpClient client = new TcpClient("127.0.0.1", 8888);
        public Byte[] data;
        public NetworkStream stream;
        public Mess m1, m2, m;
        public ComplexMessage cm = new ComplexMessage();
        public Form1()
        {
            f2 = new Form2(this);
            f3 = new Form3(this);
            f4 = new Form4(this);
            f5 = new Form5(this);
            f6 = new Form6(this);
            f7 = new Form7(this);
            f8 = new Form8(this);
            f9 = new Form9(this);
            InitializeComponent();
            stream = client.GetStream();
        }
        Form2 f2;
        public Form3 f3;
        Form4 f4;
        public Form5 f5;
        public Form6 f6;
        public Form7 f7;
        public Form8 f8;
        public Form9 f9;

        public string GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = "";
            foreach (byte b in byteHash)
            {
                hash += string.Format("{0:x2}", b);
            }
            return hash;
        }

        public void Send(Mess first, Mess second, int numberStatus)
        {
            cm.First = first;
            cm.Second = second;
            cm.NumberStatus = numberStatus;
            stream.Write(SerializeAndDeserialise.Serialize(cm).Data, 0, SerializeAndDeserialise.Serialize(cm).Data.Length);
        }

        public void Read()
        {
            byte[] readingData = new byte[6297630];
            do
            {
                stream.Read(readingData, 0, readingData.Length);
            }
            while (stream.DataAvailable);
            Mess message = new Mess();
            message.Data = readingData;
            cm = (ComplexMessage)SerializeAndDeserialise.Deserialize(message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((login.Text != "") && (password.Text != ""))
            {
                Send(SerializeAndDeserialise.Serialize(login.Text), SerializeAndDeserialise.Serialize(GetHashString(password.Text)), 1);   //enter
                if (stream.CanRead)
                {
                    Read();
                    if (cm.NumberStatus == 2)   //success enter
                    {
                        User u = (User)SerializeAndDeserialise.Deserialize(cm.First);
                        Account a = (Account)SerializeAndDeserialise.Deserialize(cm.Second);
                        u.Account = a;
                        a.User = u;
                        if (u.Role == "Admin")
                        {
                            f3.admin.Visible = true;
                        }
                        else
                        {
                            f3.admin.Visible = false;
                        }
                        f3.user = u;
                        Visible = false;
                        login.Text = "";
                        password.Text = "";
                        f3.Show();
                    }
                    else if (cm.NumberStatus == 3)   //unsuccess enter
                    {
                        MessageBox.Show("Неверный логин или пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void register_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Visible = false;
            login.Text = "";
            password.Text = "";
            f2.Show();
        }

        private void forgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Visible = false;
            login.Text = "";
            password.Text = "";
            f4.Show();
        }

        private void guest_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Send(null, null, 4);   //guest enter
            if (stream.CanRead)
            {
                Read();
                if (cm.NumberStatus == 2)   //success enter
                {
                    User u = (User)SerializeAndDeserialise.Deserialize(cm.First);
                    Account a = (Account)SerializeAndDeserialise.Deserialize(cm.Second);
                    u.Account = a;
                    f3.user = u;
                    Visible = false;
                    login.Text = "";
                    password.Text = "";
                    f3.Show();
                }
            }
        }
    }
}
