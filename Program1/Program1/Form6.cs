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
    public partial class Form6 : Form
    {
        public Form6(Form1 f1)
        {
            this.f1 = f1;
            InitializeComponent();
        }
        Form1 f1;
        User sUser;

        private void Form6_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                f1.Send(null, null, 11);   //users
                if (f1.stream.CanRead)
                {
                    f1.Read();
                    if (f1.cm.NumberStatus == 12)   //send users
                    {
                        foreach (string u in (List<string>)SerializeAndDeserialise.Deserialize(f1.cm.First))
                        {
                            listBox1.Items.Add(u);
                        }
                    }
                    listBox1.SelectedIndex = 0;
                }
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString() != "admin")
            {
                f1.Send(SerializeAndDeserialise.Serialize(listBox1.SelectedItem), null, 13);   //delete user
                listBox1.Items.Remove(listBox1.SelectedItem);
                login.Text = "";
                email.Text = "";
            }
            else
            {
                MessageBox.Show("Нельзя удалить админа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form6_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Close(); 
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            listBox1.Items.Clear();
            login.Text = "";
            email.Text = "";
            role.Text = "";
            Visible = false;
            f1.f3.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                f1.Send(SerializeAndDeserialise.Serialize(listBox1.SelectedItem), null, 14);   //user info
                if (f1.stream.CanRead)
                {
                    f1.Read();
                    if (f1.cm.NumberStatus == 15)   //send user info
                    {
                        sUser = (User)SerializeAndDeserialise.Deserialize(f1.cm.First);
                        login.Text = sUser.Login;
                        email.Text = sUser.Email;
                        role.Text = sUser.Role;
                        if (((Account)SerializeAndDeserialise.Deserialize(f1.cm.Second)).Gender == "Мужской")
                        {
                            male.Checked = true;
                        }
                        else
                        {
                            female.Checked = true;
                        }
                    }
                }
            }
        }

        private void change_Click(object sender, EventArgs e)
        {
            if ((login.Text != "") && (email.Text != ""))
            {
                string[] inf = new string[4];
                inf[0] = login.Text;
                inf[1] = email.Text;
                if (male.Checked == true)
                {
                    inf[2] = "Мужской";
                }
                else
                {
                    inf[2] = "Женский";
                }
                inf[3] = role.Text;
                f1.Send(SerializeAndDeserialise.Serialize(listBox1.SelectedItem), SerializeAndDeserialise.Serialize(inf), 16);   //edit user
                sUser.Login = login.Text;
                listBox1.Items.Remove(listBox1.SelectedItem);
                listBox1.Items.Add(sUser.Login);
            }
            else
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
