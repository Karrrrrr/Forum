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
    public partial class Form9 : Form
    {
        public Form9(Form1 f1)
        {
            this.f1 = f1;
            InitializeComponent();
        }
        Form1 f1;

        private void Form9_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Close();
        }

        private void Form9_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                if (f1.f3.user.Role == "Admin")
                {
                    label3.Visible = true;
                    role.Visible = true;
                    section.Visible = true;
                }
                else
                {
                    label3.Visible = false;
                    role.Visible = false;
                    section.Visible = false;
                }
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                f1.Send(SerializeAndDeserialise.Serialize(f1.f3.user.Role), null, 5);   //sections and branches
                f1.Read();
                if (f1.cm.NumberStatus == 6)   //send sections and branches
                {
                    foreach (Section section in (List<Section>)SerializeAndDeserialise.Deserialize(f1.cm.First))
                    {
                        listBox1.Items.Add(section.Name);
                    }
                    foreach (Branch branch in (List<Branch>)SerializeAndDeserialise.Deserialize(f1.cm.Second))
                    {
                        if ((branch.Account == f1.f3.user.Account) || (f1.f3.user.Role == "Admin"))
                        {
                            listBox2.Items.Add(branch.Name);
                        }
                    }
                }
                if (listBox1.Items.Count != 0)
                {
                    listBox1.SelectedIndex = 0;
                }
                if (listBox2.Items.Count != 0)
                {
                    listBox2.SelectedIndex = 0;
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Visible = false;
            name.Text = "";
            description.Text = "";
            role.Text = "";
            f1.f3.Show();
        }

        private void branch_Click(object sender, EventArgs e)
        {
            if (name.Text != "")
            {
                Branch branch = new Branch();
                branch.Name = name.Text;
                if (description.Text != "")
                {
                    branch.Description = description.Text;
                }
                if (role.Text != "")
                {
                    branch.AccessLevel = role.Text;
                }
                else
                {
                    branch.AccessLevel = "User";
                }
                branch.OwnerID = f1.f3.user.ID;
                branch.Account = f1.f3.user.Account;
                branch.Section = new Section();
                f1.Send(SerializeAndDeserialise.Serialize(branch), SerializeAndDeserialise.Serialize(listBox1.SelectedItem), 23);   //create branch
                Visible = false;
                name.Text = "";
                description.Text = "";
                role.Text = "";
                f1.f3.Show();
            }
            else
            {
                MessageBox.Show("Введите имя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void section_Click(object sender, EventArgs e)
        {
            if (name.Text != "")
            {
                Section section = new Section();
                section.Name = name.Text;
                if (description.Text != "")
                {
                    section.Description = description.Text;
                }
                if (role.Text != "")
                {
                    section.AccessLevel = role.Text;
                }
                else
                {
                    section.AccessLevel = "User";
                }
                f1.Send(SerializeAndDeserialise.Serialize(section), null, 24);   //create section
                Visible = false;
                name.Text = "";
                description.Text = "";
                role.Text = "";
                f1.f3.Show();
            }
            else
            {
                MessageBox.Show("Введите имя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if ((listBox2.Items.Count != 0) && (listBox2.SelectedItem != null))
            {
                f1.Send(SerializeAndDeserialise.Serialize(listBox2.SelectedItem), null, 25);   //delete branch
                listBox2.Items.Remove(listBox2.SelectedItem);
            }
        }
    }
}
