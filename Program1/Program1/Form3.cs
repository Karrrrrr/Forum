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
    public partial class Form3 : Form
    {
        public Form3(Form1 f1)
        {
            this.f1 = f1;
            InitializeComponent();
        }
        Form1 f1;
        public User user;

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Visible = false;
            f1.f7.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Visible = false;
            f1.f6.Show();
        }

        private void Form3_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                if (user.Account.Picture == null)
                {
                    pictureBox1.Image = Image.FromFile(@"D:\Документы\Шарага\ПТПМ\Лабораторные работы\Лабораторная работа 1\Лабораторная работа 8\Program1\Program1\Аватарки\base.png");
                }
                else
                {
                    pictureBox1.Image = new Bitmap(user.Account.Picture);
                }
                f1.Send(SerializeAndDeserialise.Serialize(user.Role), null, 5);   //sections and branches
                if (f1.stream.CanRead)
                {
                    f1.Read();
                    if (f1.cm.NumberStatus == 6)   //send sections and branches
                    {
                        flowLayoutPanel1.Controls.Clear();
                        foreach (Section section in (List<Section>)SerializeAndDeserialise.Deserialize(f1.cm.First))
                        {
                            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                            flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
                            flowLayoutPanel.AutoSize = true;
                            Label label = new Label();
                            label.AutoSize = true;
                            label.Font = new Font("Verdana", 12);
                            label.Text = section.Name + ": " + section.Description;
                            flowLayoutPanel.Controls.Add(label);
                            foreach (Branch branch in (List<Branch>)SerializeAndDeserialise.Deserialize(f1.cm.Second))
                            {
                                if (branch.Section.Name == section.Name)
                                {
                                    LinkLabel linkLabel = new LinkLabel();
                                    linkLabel.AutoSize = true;
                                    linkLabel.Font = new Font("Verdana", 10);
                                    linkLabel.Text = branch.Account.Name + ": " + branch.Name;
                                    linkLabel.Click += (s, ea) =>
                                    {
                                        f1.f8.branch = branch;
                                        Visible = false;
                                        f1.f8.Show();
                                    };
                                    flowLayoutPanel.Controls.Add(linkLabel);
                                }
                            }
                            flowLayoutPanel1.Controls.Add(flowLayoutPanel);
                        }
                    }
                    if (user.Role == "Admin")
                    {
                        admin.Visible = true;
                    }
                    else
                    {
                        admin.Visible = false;
                    }
                    if (user.Role == "Guest")
                    {
                        label2.Visible = false;
                    }
                    else
                    {
                        label2.Visible = true;
                    }
                    if (user.Login == "guest")
                    {
                        pictureBox1.Visible = false;
                        exit.Visible = true;
                    }
                    else
                    {
                        pictureBox1.Visible = true;
                        exit.Visible = false;
                    }
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Visible = false;
            f1.f9.Show();
        }

        private void exit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Visible = false;
            f1.Show();
        }
    }
}
