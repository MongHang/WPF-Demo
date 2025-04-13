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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using CourseLibrary;

namespace CourseSystemApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _dbConnStr;
        public MainWindow()
        {
            InitializeComponent();

            //1. 讀取連線字串
            _dbConnStr = ((App)Application.Current).DbConnStr;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            //1. 檢查必填
            if (string.IsNullOrWhiteSpace(this.txtUsername.Text) || string.IsNullOrWhiteSpace(this.txtPassword.Password))
            {
                MessageBox.Show("請輸入帳號及密碼", "訊息", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //2. 檢查帳號是否存在
            var adminUserRepository = new AdminUserRepository(_dbConnStr);
            var adminUser = adminUserRepository.GetAdminUser(this.txtUsername.Text);
            if (adminUser == null)
            {
                MessageBox.Show("登入失敗", "訊息", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //3. 檢查密碼是否相符
            var pwdHash = Helper.PwdHash(this.txtPassword.Password, adminUser.Id.ToString());
            if (pwdHash != adminUser.Password)
            {
                MessageBox.Show("登入失敗", "訊息", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //4. 登入成功
            ((App)Application.Current).CurrentAdminUser = adminUser;   

            var window = new MenuWindow();
            window.Show();
            this.Close();
        }
    }
}
