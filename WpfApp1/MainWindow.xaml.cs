using EmailContactsExtension;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RegistrationWindow window;
        public ContactManager contactManager = new ContactManager();
        private ObservableCollection<Contact> _contactsCollection1 = new ObservableCollection<Contact>();
        private ObservableCollection<Contact> _fillContactsCollection = new ObservableCollection<Contact>();
        User user;
        Contact temp;
        public MainWindow()
        {
            InitializeComponent();

        }
        public ObservableCollection<Contact> ContactsCollection
        {
            get
            {
                ContactDetails();
                return _contactsCollection1;

            }
            set
            {
                _contactsCollection1 = value;
                //  ContactDetails();
            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            user = new User();
            user = contactManager.GetUser(Login.Text, Pass.Password);

            //Autologowanie
            //user = contactManager.GetUser("mini", "pw");
            if (user == null)
            {
                MessageBox.Show("Incorrect login or password.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ContactsCollection.Clear();
                foreach (var k in user.GetContacts())
                {

                    //  ContactsCollection.Add(new Contact(k.Name, k.Surname, k.Email, k.Phone, k.Gender));

                    ContactsCollection.Add(k);
                }
                Lv.DataContext = ContactsCollection;

                _fillContactsCollection = new ObservableCollection<Contact>(ContactsCollection.Where(p => p.Name != ""));
                Lv2.DataContext = _fillContactsCollection;

                setLoggedState(true);
            }
        }

        private void setLoggedState(bool logged)
        {

            Visibility a, b;
            if (logged)
            {
                a = Visibility.Hidden;
                b = Visibility.Visible;
                label3.Content = user.Login.ToString();
            }
            else
            {
                a = Visibility.Visible;
                b = Visibility.Hidden;
            }
            PleaseLoginLabel.Visibility = a;
            PleaseLogin.Visibility = a;
            labelLogin.Visibility = a;
            Login.Visibility = a;
            labelPassword.Visibility = a;
            Pass.Visibility = a;
            buttonLogin.Visibility = a;
            label1.Visibility = a;
            buttonLogin.Visibility = a;
            buttonRegister.Visibility = a;
            label2.Visibility = b;
            label3.Visibility = b;
            SaveContacts.Visibility = b;
            Logout.Visibility = b;
            SaveContacts.Visibility = b;
            Logout.Visibility = b;


        }
        private void Lv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                var selectedItems = new List<object>();
                foreach (var item in (Lv.SelectedItems))
                {
                    try
                    {
                        selectedItems.Add((Contact)item);
                    }
                    catch (Exception)
                    {

                    }

                }


                foreach (var item in selectedItems)
                {
                    int a = 0;
                    if ((Contact)Lv2.SelectedItem == (Contact)item)
                    {
                        a =  1;
                        

                        if (Lv2.SelectedIndex == Lv2.Items.Count - 1)
                        {
                            a = -1;
                        }

                       
                    }
                    var b = Lv2.SelectedIndex;
                    ContactsCollection.Remove((Contact)item);
                    if (b != -1 && ContactsCollection.Count>0)
                    {
                        //Lv2.SelectedIndex =b+ a;
                        Lv2.SelectedItem= ContactsCollection.ElementAt(Math.Min((a + b), ContactsCollection.Count-1));
                    }
                }
            }


            ContactDetails();
        }
        //foreach (var selectedItem in selectedItems)
        //{
        //    bool canDelete;
        //    try
        //    {
        //        var x = (Contact)selectedItem;
        //        canDelete = true;
        //    }
        //    catch (Exception)
        //    {
        //        canDelete = false;
        //    }

        //    //bool isOfType = Lv.SelectedItem.
        //    if (canDelete)
        //    {
        //        if ((Contact)Lv2.SelectedItem == (Contact)selectedItem)
        //        {
        //            var a = Lv2.SelectedIndex;
        //            Lv2.SelectedIndex = a + 1;

        //            if (Lv2.SelectedIndex == Lv2.Items.Count - 1)
        //            {
        //                Lv2.SelectedIndex = Lv2.Items.Count - 2;
        //            }
        //        }

        //        ContactsCollection.Remove((Contact)selectedItem);
        //    }
        //}
        //while (Lv.SelectedItems != null && canDelete)
        //{
        //    try
        //    {
        //        var x = (Contact)Lv.SelectedItem;
        //        canDelete = true;
        //    }
        //    catch(Exception)
        //    {
        //        canDelete = false;
        //    }

        //    //bool isOfType = Lv.SelectedItem.
        //    if (canDelete)
        //    {
        //        if ((Contact)Lv2.SelectedItem == (Contact)Lv.SelectedItem)
        //        {
        //            var a = Lv2.SelectedIndex;
        //            Lv2.SelectedIndex = a + 1;

        //            if (Lv2.SelectedIndex == Lv2.Items.Count - 1)
        //            {
        //                Lv2.SelectedIndex = Lv2.Items.Count - 2;
        //            }

        //            ContactsCollection.Remove((Contact)Lv.SelectedItem);
        //        }
        //    }


        //}

        //    //}

        //    ContactDetails();
        //}



        private void Lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _fillContactsCollection = new ObservableCollection<Contact>(ContactsCollection.Where(p => p.Name != ""));
            Lv2.DataContext = _fillContactsCollection;
        }



        private void Load_Click(object sender, RoutedEventArgs e)
        {
            setLoggedState(false);
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Open";
            op.Filter = "Load users|*.xml; *.xml";
            if (op.ShowDialog() == true)
            {
                try
                {
                    DeserializeDataSet(op.FileName);
                }
                catch
                {
                    MessageBox.Show("File read error");
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sa = new SaveFileDialog();
            sa.Title = "Save";
            sa.Filter = "Save users|*.xml; *.xml";

            if (sa.ShowDialog() == true)
            {
                File.WriteAllText(sa.FileName, "Test");
                SerializeDataSet(sa.FileName);
                MessageBox.Show("Save successful");

            }
        }


        private void SerializeDataSet(string filename)
        {
            List<userStruct> ToExport = new List<userStruct>();

            foreach (var item in contactManager.GetUsers())
            {
                userStruct us = new userStruct();
                us.user = item;
                us.contacts = item.GetContacts();
                ToExport.Add(us);
            }


            XmlSerializer ser = new XmlSerializer(typeof(List<userStruct>));
            TextWriter writer = new StreamWriter(filename);
            ser.Serialize(writer, ToExport);
            writer.Close();
        }

        private void DeserializeDataSet(string filename)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<userStruct>));
            var reader = new StreamReader(filename);
            var List = (List<userStruct>)ser.Deserialize(reader);
            reader.Close();

            List<User> importedUsers = new List<User>(); ;
            foreach (var item in List)
            {
                item.user.SaveContacts(item.contacts);
                importedUsers.Add(item.user);
            }

            contactManager.SetUsers(importedUsers);
            ContactsCollection.Clear();

            if (user != null)
            {
                foreach (var k in contactManager.GetUser(user.Login, user.Password).GetContacts())
                {
                    ContactsCollection.Add(k);
                }
                Lv.Items.Refresh();
                Lv2.Items.Refresh();
            }
        }

        public struct userStruct
        {
            public User user;
            public List<Contact> contacts;

        }

        private void Lv_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ContactDetails();

        }

        private void Lv_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

            List<Contact> listToUpdateUserContacts = new List<Contact>();

            foreach (var item in ContactsCollection)
            {
                listToUpdateUserContacts.Add(item);
            }

            //user.SaveContacts(listToUpdateUserContacts);
            ContactDetails();
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;
            window = new RegistrationWindow() { Owner = this };
            window.ShowDialog();

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            user = null;
            setLoggedState(false);
            ContactsCollection.Clear();
            ContactInfo.Visibility = Visibility.Collapsed;
            Lv2.UnselectAll();
        }

        private ICommand _addBezierPatch;
        public ICommand AddBezierPatch { get { return _addBezierPatch ?? (_addBezierPatch = new ActionCommand(AddBezierPatchExecuted)); } }
        public void AddBezierPatchExecuted()
        {

        }

        public void RegisterUser(string login, string password)
        {
            contactManager.RegisterUser(login, password);

            window.Close();

        }

        private void SaveContacts_Click(object sender, RoutedEventArgs e)
        {
            List<Contact> listToUpdateUserContacts = new List<Contact>();

            foreach (var item in ContactsCollection)
            {
                listToUpdateUserContacts.Add(item);
            }
            contactManager.GetUser(user.Login, user.Password).SaveContacts(listToUpdateUserContacts);
        }




        private void Lv2_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ContactDetails();
        }

        private void ContactDetails()
        {
            temp = (Contact)Lv2.SelectedItem;

            if (temp == null)
            {
                ContactInfo.Visibility = Visibility.Collapsed;


            }
            else
            {
                ContactInfo.Visibility = Visibility.Visible;
                CDNameContent.Content = temp.Name;
                CDSurnameContent.Content = temp.Surname;
                CDEmailContent.Content = temp.Email;
                CDPhoneContent.Content = temp.Phone;
                if (temp.Gender == Gender.Female)
                {
                    Image1.Source = new BitmapImage(new Uri("D:/Studia/Informatyka MGR/Semestr 1/PwSG/WPF lab1/WpfApp1/WpfApp1/Resources/woman.jpg"));
                }
                else
                {
                    Image1.Source = new BitmapImage(new Uri("D:/Studia/Informatyka MGR/Semestr 1/PwSG/WPF lab1/WpfApp1/WpfApp1/Resources/man.png"));
                }
            }



        }

        private void DeleteImage_Click(object sender, RoutedEventArgs e)
        {
            ////temp = (Contact)Lv2.SelectedItem;
            //if (temp != null)
            //{

            //    ContactsCollection.Remove(temp);
            //    Lv2.Items.Refresh();
            //    Lv.Items.Refresh();

            //}


            var a = Lv2.SelectedIndex;
            Lv2.SelectedIndex = a + 1;

            if (Lv2.SelectedIndex == Lv2.Items.Count - 1)
            {
                Lv2.SelectedIndex = Lv2.Items.Count - 2;
            }

            ContactsCollection.RemoveAt(a);
            Lv2.Items.Refresh();
            Lv.Items.Refresh();
            ContactInfo.Visibility = Visibility.Collapsed;
        }

    }


}
