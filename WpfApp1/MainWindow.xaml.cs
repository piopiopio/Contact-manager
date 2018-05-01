using EmailContactsExtension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ContactManager contactManager = new ContactManager();
        private ObservableCollection<Contact> _contactsCollection = new ObservableCollection<Contact>();

        public MainWindow()
        {
            InitializeComponent();
        }
        public ObservableCollection<Contact> ContactsCollection
        {
            get { return _contactsCollection; }
            set
            {
                _contactsCollection = value;

            }
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            //User user = contactManager.GetUser(Login.Text, Pass.Text);

            //Autologowanie
            User user = contactManager.GetUser("mini", "pw");
            if (user == null)
            {
                MessageBox.Show("Incorrect login or password.", "An error occured", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ContactsCollection.Clear();
                foreach (var k in user.GetContacts())
                {
                    ContactsCollection.Add(k);
                }
                Lv.DataContext = _contactsCollection;
                Lv2.DataContext = _contactsCollection;
            }
        }
        private void Lv_KeyDown(object sender, KeyEventArgs e)
        {

            if (Key.Delete == e.Key)
            {
                //foreach (Contact listViewItem in ((ListView)sender).SelectedItems)
                //{
                //    _contactsCollection.Remove(listViewItem);
                //}
               
                if (e.Key == Key.Delete)
                {
                    ObservableCollection<Contact> ToDelete = new ObservableCollection<Contact>();
                    var temp =  Lv.SelectedItems;
                    foreach (var item in temp)
                    {
                        ToDelete.Add((Contact)item);
                    }

                    foreach (var item in ToDelete)
                    {
                        _contactsCollection.Remove(item);
                    }


                }
            }
        }
    }
}
