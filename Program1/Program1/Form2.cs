using Library1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program1
{
    public partial class Form2 : Form
    {
        public Form2(Form1 f1)
        {
            this.f1 = f1;
            InitializeComponent();
        }
        Form1 f1;

        private void button1_Click(object sender, EventArgs e)
        {
            if ((login.Text != "") && (email.Text != "") && (password.Text != ""))
            {
                if (login.Text.Length < 5)
                {
                    MessageBox.Show("Слишком короткий логин", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (password.Text.Length < 5)
                {
                    MessageBox.Show("Слишком короткий пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if ((password.Text == "12345678") || (password.Text == "12345") || (password.Text == "qwerty") || (password.Text == "87654321"))
                {
                    MessageBox.Show("Придумайте пароль получше", "Дурак", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (password.Text != passwordAgain.Text)
                {
                    MessageBox.Show("Пароль не совпадает", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    User user = new User();
                    Account account = new Account();
                    user.Login = login.Text;
                    account.Name = login.Text;
                    user.Password = f1.GetHashString(password.Text);
                    user.Email = email.Text;
                    if (male.Checked == true)
                    {
                        account.Gender = "Мужской";
                    }
                    else
                    {
                        account.Gender = "Женский";
                    }
                    user.Role = "User";
                    user.Account = account;
                    account.User = user;
                    f1.Send(SerializeAndDeserialise.Serialize(user), SerializeAndDeserialise.Serialize(account), 0);   //register
                    f1.f3.user = user;
                    Visible = false;
                    login.Text = "";
                    email.Text = "";
                    password.Text = "";
                    passwordAgain.Text = "";
                    f1.f3.Show();
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Visible = false;
            login.Text = "";
            email.Text = "";
            password.Text = "";
            passwordAgain.Text = "";
            f1.Show();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Close();
        }
    }
}
