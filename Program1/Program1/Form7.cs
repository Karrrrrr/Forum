using Library1;
using System;
using System.Collections;
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
    public partial class Form7 : Form
    {
        public Form7(Form1 f1)
        {
            this.f1 = f1;
            InitializeComponent();
        }
        Form1 f1;
        bool flag;

        private void Form7_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Close();
        }

        private void Form7_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                if (f1.f3.user.Account.Picture == null)
                {
                    pictureBox1.Image = new Bitmap(@"D:\Документы\Шарага\ПТПМ\Лабораторные работы\Лабораторная работа 1\Лабораторная работа 8\Program1\Program1\Аватарки\base.png");
                }
                else
                {
                    pictureBox1.Image = new Bitmap(f1.f3.user.Account.Picture);
                }
                name.Text = f1.f3.user.Login;
                email.Text = f1.f3.user.Email;
                if (f1.f3.user.Account.Gender == "Мужской")
                {
                    male.Checked = true;
                }
                else
                {
                    female.Checked = true;
                }
                if (f1.f3.user.Account.Birthday != null)
                {
                    monthCalendar1.SetDate(f1.f3.user.Account.Birthday.Value);
                }
            }
        }

        private void changePassword_Click(object sender, EventArgs e)
        {
            Visible = false;
            f1.f5.oldPassword.ReadOnly = false;
            f1.f5.Show();
        }

        private void save_Click(object sender, EventArgs e)
        {
            ArrayList inf = new ArrayList();
            inf.Add(name.Text);
            inf.Add(email.Text);
            if (male.Checked == true)
            {
                inf.Add("Мужской");
                f1.f3.user.Account.Gender = "Мужской";
            }
            else
            {
                inf.Add("Женский");
                f1.f3.user.Account.Gender = "Женский";
            }
            if (flag)
            {
                inf.Add(monthCalendar1.SelectionStart);
                f1.f3.user.Account.Birthday = monthCalendar1.SelectionStart;
            }
            f1.Send(SerializeAndDeserialise.Serialize(f1.f3.user.ID), SerializeAndDeserialise.Serialize(inf), 22);   //change user info
            f1.f3.user.Login = name.Text;
            f1.f3.user.Account.Name = name.Text;
            f1.f3.user.Email = email.Text;
        }

        private void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            flag = true;
        }

        private void exit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Visible = false;
            f1.Show();
        }

        private void back_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Visible = false;
            f1.f3.Show();
        }
    }
}
