using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CalendarApp
{
    /// <summary>
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        public SignInWindow()
        {
            InitializeComponent();
        }

        public void SignIn(object sender, RoutedEventArgs e)
        {
            if(usernameBox.Text.Length > 0)
            {
                User userSignIn = new User(usernameBox.Text);
                userSignIn.Save();
                MainWindow mainWindow = new MainWindow(userSignIn);
                mainWindow.Show();
                this.Close();
            }
        }
    }
}
