using Library1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program1
{
    public partial class Form5 : Form
    {
        public Form5(Form1 f1)
        {
            this.f1 = f1;
            InitializeComponent();
        }
        Form1 f1;

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (oldPassword.ReadOnly == false)
            {
                if ((oldPassword.Text != "") && (newPassword.Text != ""))
                {
                    if (newPassword.Text.Length < 5)
                    {
                        MessageBox.Show("Слишком короткий пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if ((newPassword.Text == "12345678") || (newPassword.Text == "12345") || (newPassword.Text == "qwerty") || (newPassword.Text == "87654321"))
                    {
                        MessageBox.Show("Придумайте пароль получше", "Дурак", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (newPassword.Text != newPasswordAgain.Text)
                    {
                        MessageBox.Show("Пароль не совпадает", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (newPassword.Text == oldPassword.Text)
                    {
                        MessageBox.Show("Старый и новый пароли совпадают", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string[] passwords = new string[2];
                        passwords[0] = f1.GetHashString(oldPassword.Text);
                        passwords[1] = f1.GetHashString(newPassword.Text);
                        f1.Send(SerializeAndDeserialise.Serialize(passwords), SerializeAndDeserialise.Serialize(f1.f3.user.ID), 19);   //change password
                        f1.Read();
                        if (f1.cm.NumberStatus == 20)   //success password change
                        {
                            f1.f3.user.Password = f1.GetHashString(newPassword.Text);
                            Visible = false;
                            oldPassword.Text = "";
                            newPassword.Text = "";
                            newPasswordAgain.Text = "";
                            f1.f7.Show();
                        }
                        else if (f1.cm.NumberStatus == 21)   //unsuccess password change
                        {
                            MessageBox.Show("Неверный старый пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (newPassword.Text != "")
                {
                    if (newPassword.Text.Length < 5)
                    {
                        MessageBox.Show("Слишком короткий пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if ((newPassword.Text == "12345678") || (newPassword.Text == "12345") || (newPassword.Text == "qwerty") || (newPassword.Text == "87654321"))
                    {
                        MessageBox.Show("Придумайте пароль получше", "Дурак", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (newPassword.Text != newPasswordAgain.Text)
                    {
                        MessageBox.Show("Пароль не совпадает", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string[] passwords = new string[2];
                        passwords[0] = null;
                        passwords[1] = f1.GetHashString(newPassword.Text);
                        f1.Send(SerializeAndDeserialise.Serialize(passwords), SerializeAndDeserialise.Serialize(f1.f3.user.ID), 19);   //change password
                        f1.f3.user.Password = f1.GetHashString(newPassword.Text);
                        Visible = false;
                        newPassword.Text = "";
                        newPasswordAgain.Text = "";
                        f1.f3.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Visible = false;
            oldPassword.Text = "";
            newPassword.Text = "";
            newPasswordAgain.Text = "";
            f1.f7.Show();
        }

        private void Form5_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                if (oldPassword.ReadOnly == true)
                {
                    linkLabel1.Visible = false;
                }
                else
                {
                    linkLabel1.Visible = true;
                }
            }
        }

        private void Form5_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Close();
        }
    }
}
