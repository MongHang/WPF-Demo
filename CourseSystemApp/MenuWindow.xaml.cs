using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CourseSystemApp
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            this.Loaded += MenuWindow_Loaded;
        }

        private void MenuWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtUserName.Text = ((App)Application.Current).CurrentAdminUser.UserName;
        }

        private void btnUserManagement_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            this.funsFrame.Navigate(new System.Uri("AdminFunPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnTeacherManagement_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCourseManagement_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCourseScheduleManagement_Click(object sender, RoutedEventArgs e)
        {
            this.funsFrame.Navigate(new System.Uri("CourseScheduleFunPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnResetPassWord_Click(object sender, RoutedEventArgs e)
        {
            this.funsFrame.Navigate(new System.Uri("PwdFunPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).CurrentAdminUser = null;

            var window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
