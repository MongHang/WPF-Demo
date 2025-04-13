using CourseLibrary;
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
    /// Interaction logic for AdminFunCreatePage.xaml
    /// </summary>
    public partial class AdminFunCreatePage : Window
    {
        private string _dbConnStr;
        private AdminUserInfo _adminUser;

        public AdminFunCreatePage()
        {
            InitializeComponent();
            _dbConnStr = ((App)Application.Current).DbConnStr;
            _adminUser = new AdminUserInfo();
            this.DataContext = _adminUser;
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            var actionResult = SaveAdminUser();

            if(!actionResult)
                return;

            MessageBox.Show("User created successfully.");
            this.Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            var actionResult = SaveAdminUser();

            if (!actionResult)
                return;

            MessageBox.Show("User created successfully.");
            _adminUser = new AdminUserInfo();
            this.DataContext = _adminUser;
        }

        private bool SaveAdminUser()
        {
            if (string.IsNullOrEmpty(_adminUser.UserName) || string.IsNullOrEmpty(_adminUser.Email))
            {
                MessageBox.Show("Please input user name and email.");
                return false;
            }

            var adminUserRepository = new AdminUserRepository(_dbConnStr);

            // Check if the user name is already used
            var existingUser = adminUserRepository.GetAdminUser(_adminUser.UserName);
            if (existingUser != null)
            {
                MessageBox.Show("User name already exists.");
                return false;
            }

            _adminUser.Id = Guid.NewGuid();
            _adminUser.Password = Helper.PwdHash("123456", _adminUser.Id.ToString());
            adminUserRepository.CreateAdminUser(_adminUser);
            return true;
        }

    }
}
