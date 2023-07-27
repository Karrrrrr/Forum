using Library1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientObject
    {
        protected internal string ID { get; set; }
        protected internal NetworkStream Stream { get; private set; }
        TcpClient client;
        ServerObject server;
        ComplexMessage cm = new ComplexMessage();

        public ClientObject(TcpClient tcpClient, ServerObject serverObject)
        {
            ID = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverObject;
            serverObject.AddConnection(this);
            Stream = client.GetStream();
        }

        void Send(Mess first, Mess second, int numberStatus)
        {
            cm.First = first;
            cm.Second = second;
            cm.NumberStatus = numberStatus;
            Stream.Write(SerializeAndDeserialise.Serialize(cm).Data, 0, SerializeAndDeserialise.Serialize(cm).Data.Length);
        }

        User toUser(Library1.User user)
        {
            User u = new User() { Email = user.Email, ID = user.ID, Login = user.Login, Password = user.Password, Role = user.Role };
            u.Account = toAccount(user.Account, u);
            return u;
        }
        Library1.User toUser(User user)
        {
            Library1.User u = new Library1.User() { Email = user.Email, ID = user.ID, Login = user.Login, Password = user.Password, Role = user.Role };
            u.Account = toAccount(user.Account, u);
            return u;
        }
        Account toAccount(Library1.Account account, User user)
        {
            Account a = new Account() { Birthday = account.Birthday, Gender = account.Gender, ID = account.ID, Name = account.Name, Picture = account.Picture, User = user };
            if (account.Branch != null)
            {
                a.Branch = toBranch(account.Branch, a, null);
            }
            if (account.Message != null)
            {
                a.Message = toMessage(account.Message, a, null);
            }
            return a;
        }
        Library1.Account toAccount(Account account, Library1.User user)
        {
            Library1.Account a = new Library1.Account() { Birthday = account.Birthday, Gender = account.Gender, ID = account.ID, Name = account.Name, Picture = account.Picture, User = user };
            if (account.Branch != null)
            {
                a.Branch = toBranch(account.Branch, a, null);
            }
            if (account.Message != null)
            {
                a.Message = toMessage(account.Message, a, null);
            }
            return a;
        }
        Branch toBranch(Library1.Branch branch, Account account, Section section)
        {
            Branch b = new Branch() { AccessLevel = branch.AccessLevel, Account = account, Description = branch.Description, ID = branch.ID, Name = branch.Name, OwnerID = branch.OwnerID, Section = section, SectionID = branch.SectionID };
            if (branch.Message != null)
            {
                b.Message = toMessage(branch.Message, null, b);
            }
            return b;
        }
        ICollection<Branch> toBranch(ICollection<Library1.Branch> branches, Account account, Section section)
        {
            List<Branch> b = new List<Branch>();
            foreach (Library1.Branch branch in branches)
            {
                Branch br = new Branch() { AccessLevel = branch.AccessLevel, Account = account, Description = branch.Description, ID = branch.ID, Message = toMessage(branch.Message, account, toBranch(branch, null, null)), Name = branch.Name, OwnerID = branch.OwnerID, Section = section, SectionID = branch.SectionID };
                b.Add(br);
            }
            return b;
        }
        Library1.Branch toBranch(Branch branch, Library1.Account account, Library1.Section section)
        {
            Library1.Branch b = new Library1.Branch() { AccessLevel = branch.AccessLevel, Account = account, Description = branch.Description, ID = branch.ID, Name = branch.Name, OwnerID = branch.OwnerID, Section = section, SectionID = branch.SectionID };
            if (branch.Message != null)
            {
                b.Message = toMessage(branch.Message, null, b);
            }
            return b;
        }
        ICollection<Library1.Branch> toBranch(ICollection<Branch> branches, Library1.Account account, Library1.Section section)
        {
            List<Library1.Branch> b = new List<Library1.Branch>();
            foreach (Branch branch in branches)
            {
                Library1.Branch br = new Library1.Branch() { AccessLevel = branch.AccessLevel, Account = account, Description = branch.Description, ID = branch.ID, Message = toMessage(branch.Message, account, toBranch(branch, null, null)), Name = branch.Name, OwnerID = branch.OwnerID, Section = section, SectionID = branch.SectionID };
                b.Add(br);
            }
            return b;
        }
        Section toSection(Library1.Section section)
        {
            Section s = new Section() { AccessLevel = section.AccessLevel, Description = section.Description, ID = section.ID, Name = section.Name };
            if (section.Branch != null)
            {
                s.Branch = toBranch(section.Branch, null, s);
            }
            return s;
        }
        Library1.Section toSection(Section section)
        {
            Library1.Section s = new Library1.Section() { AccessLevel = section.AccessLevel, Description = section.Description, ID = section.ID, Name = section.Name };
            if (section.Branch != null)
            {
                s.Branch = toBranch(section.Branch, null, s);
            }
            return s;
        }
        Message toMessage(Library1.Message message, Account account, Branch branch)
        {
            Message m = new Message() { Account = account, Branch = branch, BranchID = message.BranchID, CreationDate = message.CreationDate, ID = message.ID, OwnerID = message.OwnerID, Text = message.Text };
            return m;
        }
        ICollection<Message> toMessage(ICollection<Library1.Message> messages, Account account, Branch branch)
        {
            List<Message> m = new List<Message>();
            foreach (Library1.Message message in messages)
            {
                Message mes = new Message() { Account = account, Branch = branch, BranchID = message.BranchID, CreationDate = message.CreationDate, ID = message.ID, OwnerID = message.OwnerID, Text = message.Text };
                m.Add(mes);
            }
            return m;
        }
        Library1.Message toMessage(Message message, Library1.Account account, Library1.Branch branch)
        {
            Library1.Message m = new Library1.Message() { Account = account, Branch = branch, BranchID = message.BranchID, CreationDate = message.CreationDate, ID = message.ID, OwnerID = message.OwnerID, Text = message.Text };
            return m;
        }
        ICollection<Library1.Message> toMessage(ICollection<Message> messages, Library1.Account account, Library1.Branch branch)
        {
            List<Library1.Message> m = new List<Library1.Message>();
            foreach (Message message in messages)
            {
                Library1.Message mes = new Library1.Message() { Account = account, Branch = branch, BranchID = message.BranchID, CreationDate = message.CreationDate, ID = message.ID, OwnerID = message.OwnerID, Text = message.Text };
                m.Add(mes);
            }
            return m;
        }

        public void Process()
        {
            try
            {
                while (true)
                {
                    if (Stream.CanRead)
                    {
                        byte[] myReadBuffer = new byte[6297630];
                        do
                        {
                            Stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                        }
                        while (Stream.DataAvailable);
                        Mess message = new Mess();
                        message.Data = myReadBuffer;
                        ComplexMessage complexMessage = (ComplexMessage)SerializeAndDeserialise.Deserialize(message);

                        if (complexMessage.NumberStatus == 0)   //register
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                User user = toUser((Library1.User)SerializeAndDeserialise.Deserialize(complexMessage.First));
                                Account account = toAccount((Library1.Account)SerializeAndDeserialise.Deserialize(complexMessage.Second), toUser((Library1.User)SerializeAndDeserialise.Deserialize(complexMessage.First)));
                                user.Account = account;
                                account.User = user;
                                db.UserSet.Add(user);
                                db.AccountSet.Add(account);
                                db.SaveChanges();
                            }
                        }
                        else if (complexMessage.NumberStatus == 1)   //enter
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                bool flag = true;
                                foreach (User u in db.UserSet)
                                {
                                    if ((SerializeAndDeserialise.Deserialize(complexMessage.First).ToString() == u.Login) && (SerializeAndDeserialise.Deserialize(complexMessage.Second).ToString() == u.Password))
                                    {
                                        User user = new User() { Login = u.Login, Password = u.Password, Email = u.Email, Role = u.Role, Account = u.Account, ID = u.ID };
                                        Account account = new Account() { Birthday = u.Account.Birthday, Branch = u.Account.Branch, Gender = u.Account.Gender, Message = u.Account.Message, Name = u.Account.Name, Picture = u.Account.Picture, User = u, ID = u.Account.ID };
                                        Send(SerializeAndDeserialise.Serialize(toUser(user)), SerializeAndDeserialise.Serialize(toAccount(account, toUser(user))), 2);   //success enter
                                        flag = false;
                                    }
                                }
                                if (flag)
                                {
                                    Send(null, null, 3);   //unsuccess enter
                                }
                            }
                        }
                        else if (complexMessage.NumberStatus == 4)   //guest enter
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                Send(SerializeAndDeserialise.Serialize(toUser(db.UserSet.Find(2))), SerializeAndDeserialise.Serialize(toAccount(db.AccountSet.Find(2), toUser(db.UserSet.Find(2)))), 2);   //success enter
                            }
                        }
                        else if (complexMessage.NumberStatus == 5)   //sections and branches
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                List<Library1.Section> sections = new List<Library1.Section>();
                                List<Library1.Branch> branches = new List<Library1.Branch>();
                                if (SerializeAndDeserialise.Deserialize(complexMessage.First).ToString() == "Admin")
                                {
                                    foreach (Section section in db.SectionSet)
                                    {
                                        sections.Add(toSection(section));
                                    }
                                    foreach (Branch branch in db.BranchSet)
                                    {
                                        branches.Add(toBranch(branch, toAccount(branch.Account, null), toSection(branch.Section)));
                                    }
                                }
                                else
                                {
                                    foreach (Section section in db.SectionSet)
                                    {
                                        if (section.AccessLevel != "Admin")
                                        {
                                            sections.Add(toSection(section));
                                        }
                                    }
                                    foreach (Branch branch in db.BranchSet)
                                    {
                                        if (branch.AccessLevel != "Admin")
                                        {
                                            branches.Add(toBranch(branch, toAccount(branch.Account, null), toSection(branch.Section)));
                                        }
                                    }
                                }
                                Send(SerializeAndDeserialise.Serialize(sections), SerializeAndDeserialise.Serialize(branches), 6);   //send sections and branches
                            }
                        }
                        else if (complexMessage.NumberStatus == 7)   //messages
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                List<Library1.Message> messages = new List<Library1.Message>();
                                foreach (Message m in db.BranchSet.Find(toBranch(((Library1.Branch)SerializeAndDeserialise.Deserialize(complexMessage.First)), null, null).ID).Message)
                                {
                                    messages.Add(toMessage(m, toAccount(m.Account, null), toBranch(m.Branch, null, null)));
                                }
                                Send(SerializeAndDeserialise.Serialize(messages), null, 8);   //send messages
                            }
                        }
                        else if (complexMessage.NumberStatus == 9)   //new message
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                Message m = toMessage((Library1.Message)SerializeAndDeserialise.Deserialize(complexMessage.First), toAccount(((Library1.Message)SerializeAndDeserialise.Deserialize(complexMessage.First)).Account, null), toBranch(((Library1.Message)SerializeAndDeserialise.Deserialize(complexMessage.First)).Branch, null, null));
                                m.Account = db.AccountSet.Find(m.Account.ID);
                                m.Branch = db.BranchSet.Find(m.Branch.ID);
                                db.MessageSet.Add(m);
                                db.SaveChanges();
                            }
                        }
                        else if (complexMessage.NumberStatus == 10)   //delete message
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                db.MessageSet.Remove(db.MessageSet.Find((int)SerializeAndDeserialise.Deserialize(complexMessage.First)));
                                db.SaveChanges();
                            }
                        }
                        else if (complexMessage.NumberStatus == 11)   //users
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                List<string> users = new List<string>();
                                foreach (User u in db.UserSet)
                                {
                                    users.Add(u.Login);
                                }
                                Send(SerializeAndDeserialise.Serialize(users), null, 12);   //send users
                            }
                        }
                        else if (complexMessage.NumberStatus == 13)   //delete user
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                foreach (User u in db.UserSet)
                                {
                                    if (u.Login == SerializeAndDeserialise.Deserialize(complexMessage.First).ToString())
                                    {
                                        List<Message> messages = new List<Message>();
                                        foreach (Message m in u.Account.Message)
                                        {
                                            messages.Add(m);
                                        }
                                        for (int i = 0; i < messages.Count; i++)
                                        {
                                            db.MessageSet.Remove(db.MessageSet.Find(messages[i].ID));
                                        }
                                        List<Branch> branches = new List<Branch>();
                                        foreach (Branch b in u.Account.Branch)
                                        {
                                            branches.Add(b);
                                        }
                                        for (int i = 0; i < branches.Count; i++)
                                        {
                                            db.MessageSet.Remove(db.MessageSet.Find(branches[i].ID));
                                        }
                                        db.AccountSet.Remove(db.AccountSet.Find(u.ID));
                                        db.UserSet.Remove(u);
                                        break;
                                    }
                                }
                                db.SaveChanges();
                            }
                        }
                        else if (complexMessage.NumberStatus == 14)   //user info
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                foreach (User u in db.UserSet)
                                {
                                    if (u.Login == SerializeAndDeserialise.Deserialize(complexMessage.First).ToString())
                                    {
                                        Send(SerializeAndDeserialise.Serialize(toUser(u)), SerializeAndDeserialise.Serialize(toAccount(u.Account, toUser(u))), 15);   //send user info
                                        break;
                                    }
                                }
                            }
                        }
                        else if (complexMessage.NumberStatus == 16)   //edit user
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                foreach (User u in db.UserSet)
                                {
                                    if (u.Login == SerializeAndDeserialise.Deserialize(complexMessage.First).ToString())
                                    {
                                        u.Login = ((string[])SerializeAndDeserialise.Deserialize(complexMessage.Second))[0];
                                        u.Account.Name = ((string[])SerializeAndDeserialise.Deserialize(complexMessage.Second))[0];
                                        u.Email = ((string[])SerializeAndDeserialise.Deserialize(complexMessage.Second))[1];
                                        u.Account.Gender = ((string[])SerializeAndDeserialise.Deserialize(complexMessage.Second))[2];
                                        u.Role = ((string[])SerializeAndDeserialise.Deserialize(complexMessage.Second))[3];
                                        break;
                                    }
                                }
                                db.SaveChanges();
                            }
                        }
                        else if (complexMessage.NumberStatus == 17)   //recover password
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                foreach (User u in db.UserSet)
                                {
                                    if (u.Email == SerializeAndDeserialise.Deserialize(complexMessage.First).ToString())
                                    {
                                        Send(SerializeAndDeserialise.Serialize(toUser(u)), SerializeAndDeserialise.Serialize(toAccount(u.Account, toUser(u))), 18);   //send recovering user
                                        break;
                                    }
                                }
                            }
                        }
                        else if (complexMessage.NumberStatus == 19)   //change password
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                if (((string[])SerializeAndDeserialise.Deserialize(complexMessage.First))[0] != null)
                                {
                                    if (((string[])SerializeAndDeserialise.Deserialize(complexMessage.First))[0] == db.UserSet.Find((int)SerializeAndDeserialise.Deserialize(complexMessage.Second)).Password)
                                    {
                                        db.UserSet.Find((int)SerializeAndDeserialise.Deserialize(complexMessage.Second)).Password = ((string[])SerializeAndDeserialise.Deserialize(complexMessage.First))[1];
                                        Send(null, null, 20);   //success password change
                                    }
                                    else
                                    {
                                        Send(null, null, 21);   //unsuccess password change
                                    }
                                }
                                else
                                {
                                    db.UserSet.Find((int)SerializeAndDeserialise.Deserialize(complexMessage.Second)).Password = ((string[])SerializeAndDeserialise.Deserialize(complexMessage.First))[1];
                                }
                                db.SaveChanges();
                            }
                        }
                        else if (complexMessage.NumberStatus == 22)   //change user info
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                int a = (int)SerializeAndDeserialise.Deserialize(complexMessage.First);
                                db.UserSet.Find((int)SerializeAndDeserialise.Deserialize(complexMessage.First)).Login = ((ArrayList)SerializeAndDeserialise.Deserialize(complexMessage.Second))[0].ToString();
                                db.AccountSet.Find((int)SerializeAndDeserialise.Deserialize(complexMessage.First)).Name = ((ArrayList)SerializeAndDeserialise.Deserialize(complexMessage.Second))[0].ToString();
                                db.UserSet.Find((int)SerializeAndDeserialise.Deserialize(complexMessage.First)).Email = ((ArrayList)SerializeAndDeserialise.Deserialize(complexMessage.Second))[1].ToString();
                                db.AccountSet.Find((int)SerializeAndDeserialise.Deserialize(complexMessage.First)).Gender = ((ArrayList)SerializeAndDeserialise.Deserialize(complexMessage.Second))[2].ToString();
                                if (((ArrayList)SerializeAndDeserialise.Deserialize(complexMessage.Second)).Count == 4)
                                {
                                    db.AccountSet.Find((int)SerializeAndDeserialise.Deserialize(complexMessage.First)).Birthday = (DateTime)((ArrayList)SerializeAndDeserialise.Deserialize(complexMessage.Second))[3];
                                }
                                db.SaveChanges();
                            }
                        }
                        else if (complexMessage.NumberStatus == 23)   //create branch
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                Branch branch = toBranch((Library1.Branch)SerializeAndDeserialise.Deserialize(complexMessage.First), toAccount(((Library1.Branch)SerializeAndDeserialise.Deserialize(complexMessage.First)).Account, null), null);
                                foreach (Section section in db.SectionSet)
                                {
                                    if (section.Name == SerializeAndDeserialise.Deserialize(complexMessage.Second).ToString())
                                    {
                                        branch.Section = section;
                                    }
                                }
                                branch.Account = db.AccountSet.Find(branch.Account.ID);
                                db.BranchSet.Add(branch);
                                db.SaveChanges();
                            }
                        }
                        else if (complexMessage.NumberStatus == 24)   //create section
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                db.SectionSet.Add(toSection((Library1.Section)SerializeAndDeserialise.Deserialize(complexMessage.First)));
                                db.SaveChanges();
                            }
                        }
                        else if (complexMessage.NumberStatus == 25)   //delete branch
                        {
                            using (ForumContainer db = new ForumContainer())
                            {
                                foreach (Branch branch in db.BranchSet)
                                {
                                    if (branch.Name == SerializeAndDeserialise.Deserialize(complexMessage.First).ToString())
                                    {
                                        List<Message> m = new List<Message>();
                                        foreach (Message mes in branch.Message)
                                        {
                                            m.Add(mes);
                                        }
                                        for (int i = 0; i < m.Count; i++)
                                        {
                                            db.MessageSet.Remove(db.MessageSet.Find(m[i].ID));
                                        }
                                        db.BranchSet.Remove(branch);
                                        break;
                                    }
                                }
                                db.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        client.Close();
                    }
                }
            }
            catch (Exception e)
            { }
            finally
            {
                server.RemoveConnection(this.ID);
                Close();
            }
        }

        protected internal void Close()
        {
            if (Stream != null)
            {
                Stream.Close();
            }
            if (client != null)
            {
                client.Close();
            }
        }
    }
}
