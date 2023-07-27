using Library1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program1
{
    public partial class Form4 : Form
    {
        public Form4(Form1 f1)
        {
            this.f1 = f1;
            InitializeComponent();
        }
        Form1 f1;
        int code;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Visible = false;
            textBox1.Text = "";
            f1.Show();
        }

        private void recover_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                MailAddress from = new MailAddress("give.me.dollz@mail.ru", "ForumService");
                MailAddress to = new MailAddress(textBox1.Text);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Восстановление пароля";
                f1.Send(SerializeAndDeserialise.Serialize(textBox1.Text), null, 17);   //recover password
                Random r = new Random();
                code = r.Next();
                m.Body = "<h1>Временный код для восстановления: " + code + "</h1>";
                f1.Read();
                if (f1.cm.NumberStatus == 18)   //send recovering user
                {
                    f1.f3.user = (User)SerializeAndDeserialise.Deserialize(f1.cm.First);
                    f1.f3.user.Account = (Account)SerializeAndDeserialise.Deserialize(f1.cm.Second);
                }
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                smtp.Credentials = new NetworkCredential("give.me.dollz@mail.ru", "forumservice");
                smtp.EnableSsl = true;
                smtp.Send(m);

                label2.Text = "Код:";
                textBox1.Text = "";
                recover.Visible = false;
                enter.Visible = true;
            }
            else
            {
                MessageBox.Show("Заполните поле", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void enter_Click(object sender, EventArgs e)
        {
            if ((int.TryParse(textBox1.Text, out int n)) && (int.Parse(textBox1.Text) == code))
            {
                Visible = false;
                f1.f5.oldPassword.ReadOnly = true;
                textBox1.Text = "";
                f1.f5.Show();
            }
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Close();
        }
    }
}
