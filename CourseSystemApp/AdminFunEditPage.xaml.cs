using CourseLibrary.DataModel;
using CourseLibrary.Repostiory;
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
    /// Interaction logic for AdminFunEditPage.xaml
    /// </summary>
    public partial class AdminFunEditPage : Window
    {
        private string _dbConnStr;
        public AdminFunEditPage()
        {
            InitializeComponent();
            _dbConnStr = ((App)Application.Current).DbConnStr;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null)
            {
                MessageBox.Show("No user to save.");
                return;
            }
            var adminUser = this.DataContext as AdminUserInfo;
            var adminUserRepository = new AdminUserRepository(_dbConnStr);
            adminUserRepository.UpdateAdminUser(adminUser);
            MessageBox.Show("User updated successfully.");
            this.Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
