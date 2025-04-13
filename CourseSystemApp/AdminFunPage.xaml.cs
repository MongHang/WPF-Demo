using CourseLibrary.DataModel;
using CourseLibrary.Repostiory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseSystemApp
{
    /// <summary>
    /// Interaction logic for AdminFunPage.xaml
    /// </summary>
    public partial class AdminFunPage : Page
    {
        private string _dbConnStr;
        private ObservableCollection<AdminUserInfo> _adminUsers;

        public AdminFunPage()
        {
            InitializeComponent();
            _dbConnStr = ((App)Application.Current).DbConnStr;
        }

        private void queryButton_Click(object sender, RoutedEventArgs e)
        {
            Query();
        }

        private void Query()
        {
            var adminUserRepository = new AdminUserRepository(_dbConnStr);
            _adminUsers = adminUserRepository.Query(new AdminUserQueryParameter()
            {
                UserName = this.userName.Text,
                Email = this.userEmail.Text
            });

            this.adminUserList.ItemsSource = _adminUsers;
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            var createPage = new AdminFunCreatePage();
            createPage.ShowDialog();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            var currentBtn = sender as Button;
            var adminUser = currentBtn.CommandParameter as AdminUserInfo;

            var editPage = new AdminFunEditPage();
            editPage.DataContext = adminUser;
            editPage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            editPage.ShowDialog();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var currentBtn = sender as Button;
            var deladminUser = currentBtn.CommandParameter as AdminUserInfo;

            var result = MessageBox.Show($"確認要刪除 {deladminUser.UserName} 的資料?", "Confirm Delete"
                , MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            //若是目前登入者資料，不允許刪除
            var currentUser = ((App)Application.Current).CurrentAdminUser;
            if (currentUser.Id == deladminUser.Id)
            {
                MessageBox.Show($"不允許刪除目前登入者", "Confirm"
               , MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            var adminUserRepository = new AdminUserRepository(_dbConnStr);
            adminUserRepository.DeleteAdminUser(deladminUser.Id);

            MessageBox.Show($"刪除成功", "Confirm", MessageBoxButton.OK, MessageBoxImage.Information);

            Query();
        }
    }
}
