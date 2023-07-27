using Library1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Program1
{
    public partial class Form8 : Form
    {
        public Form8(Form1 f1)
        {
            this.f1 = f1;
            InitializeComponent();
        }
        Form1 f1;
        public Branch branch;
        int mid;
        protected internal string Id { get; private set; }

        private void Form8_FormClosing(object sender, FormClosingEventArgs e)
        {
            f1.Close();
        }

        private void Form8_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == true)
            {
                label1.Text = branch.Name;
                Text = branch.Name;
                flowLayoutPanel1.Controls.Clear();
                f1.Send(SerializeAndDeserialise.Serialize(branch), null, 7);   //messages
                if (f1.stream.CanRead)
                {
                    f1.Read();
                    if (f1.cm.NumberStatus == 8)   //send messages
                    {
                        Label label = new Label();
                        label.AutoSize = true;
                        label.Font = new System.Drawing.Font("Verdana", 12);
                        label.Text = branch.Description;
                        flowLayoutPanel1.Controls.Add(label);
                        foreach (Library1.Message m in (List<Library1.Message>)SerializeAndDeserialise.Deserialize(f1.cm.First))
                        {
                            Label l = new Label();
                            l.AutoSize = true;
                            l.Font = new System.Drawing.Font("Verdana", 10);
                            l.Text = Environment.NewLine + m.Account.Name + ": " + m.Text + " (" + m.CreationDate.ToString() + ")";
                            l.Click += (s, ea) =>
                            {
                                int i = 0;
                                string n = "";
                                while (l.Text[i] != ':')
                                {
                                    if ((l.Text[i] != '\r') && (l.Text[i] != '\n'))
                                    {
                                        n += l.Text[i];
                                    }
                                    i++;
                                }
                                if (((f1.f3.user.Login == n) || (f1.f3.user.Role == "Admin")) && (f1.f3.user.Role != "Guest"))
                                {
                                    DialogResult res = MessageBox.Show("Удалить сообщение?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (res == DialogResult.Yes)
                                    {
                                        flowLayoutPanel1.Controls.Remove(l);
                                        mid = m.ID;
                                        t.Text = "1";
                                    }
                                }
                            };
                            flowLayoutPanel1.Controls.Add(l);
                        }
                    }
                }
                if (f1.f3.user.Role == "Guest")
                {
                    textBox1.Enabled = false;
                }
                else
                {
                    textBox1.Enabled = true;
                }
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            if (f1.f3.user.Role == "Guest")
            {
                MessageBox.Show("Запрещено", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (textBox1.Text != "")
            {
                Library1.Message m = new Library1.Message();
                m.Text = textBox1.Text;
                m.CreationDate = DateTime.Now;
                m.OwnerID = f1.f3.user.Account.ID;
                m.Account = f1.f3.user.Account;
                m.BranchID = branch.ID;
                m.Branch = branch;
                f1.Send(SerializeAndDeserialise.Serialize(m), null, 9);   //new message
                Label label = new Label();
                label.AutoSize = true;
                label.Font = new System.Drawing.Font("Verdana", 10);
                label.Text = Environment.NewLine + m.Account.Name + ": " + m.Text + " (" + m.CreationDate.ToString() + ")";
                label.Click += (s, ea) =>
                {
                    int i = 0;
                    string n = "";
                    while (label.Text[i] != ':')
                    {
                        n += label.Text[i];
                        i++;
                    }
                    if (((f1.f3.user.Login == n) || (f1.f3.user.Role == "Admin")) && (f1.f3.user.Role != "Guest"))
                    {
                        DialogResult res = MessageBox.Show("Удалить сообщение?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (res == DialogResult.Yes)
                        {
                            flowLayoutPanel1.Controls.Remove(label);
                            mid = m.ID;
                            t.Text = "1";
                        }
                    }
                };
                flowLayoutPanel1.Controls.Add(label);
            }
            else
            {
                MessageBox.Show("Введите сообщение", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox1.Text = "";
            Visible = false;
            f1.f3.Show();
        }

        private void t_TextChanged(object sender, EventArgs e)
        {
            if (t.Text == "1")
            {
                f1.Send(SerializeAndDeserialise.Serialize(mid), null, 10);   //delete message
                t.Text = "0";
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string pathDocument = AppDomain.CurrentDomain.BaseDirectory + "first.docx";
            DocX document = DocX.Create(pathDocument);
            document.MarginLeft = 60.0f;
            document.MarginRight = 60.0f;
            document.MarginTop = 60.0f;
            document.MarginBottom = 60.0f;
            document.InsertParagraph(branch.Name + Environment.NewLine).Bold().Font("Times New Roman").FontSize(14).Alignment = Alignment.center;
            foreach (Label label in flowLayoutPanel1.Controls)
            {
                document.InsertParagraph(label.Text).Font("Times New Roman").FontSize(14).Alignment = Alignment.left;
            }
            document.Save();
        }
        string printContent;
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            printContent = branch.Name + Environment.NewLine;
            foreach (Label label in flowLayoutPanel1.Controls)
            {
                printContent += label.Text;
            }
            PrintDocument printDocument = new PrintDocument();
            PrintDialog printDialog = new PrintDialog();
            printDocument.PrintPage += PrintPageHandler;
            printDialog.Document = printDocument;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDialog.Document.Print();
            }
        }

        private void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(printContent, new System.Drawing.Font("Arial", 16), Brushes.Black, 0, 0);
        }
    }
}
