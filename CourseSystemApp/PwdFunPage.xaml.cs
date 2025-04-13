using CourseLibrary;
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

namespace CourseSystemApp
{
    /// <summary>
    /// Interaction logic for PwdFunPage.xaml
    /// </summary>
    public partial class PwdFunPage : Page
    {
        private string _dbConnStr;
        public PwdFunPage()
        {
            InitializeComponent();
            _dbConnStr = ((App)Application.Current).DbConnStr;
        }

        private void PwdChgBtn_Click(object sender, RoutedEventArgs e)
        {
            // 1.驗證必填欄位
            if (string.IsNullOrWhiteSpace(this.OldPwd.Password) ||
               string.IsNullOrWhiteSpace(this.NewPwd.Password) ||
               string.IsNullOrWhiteSpace(this.ConfirmPwd.Password))
            {
                MessageBox.Show("所有欄位必填", "訊息", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //2.驗證新密碼與確認密碼是否相符
            if (this.NewPwd.Password != this.ConfirmPwd.Password)
            {
                MessageBox.Show("新密碼與確認密碼不相符", "訊息", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            //3.驗證舊密碼是否正確
            var adminUser = ((App)Application.Current).CurrentAdminUser;
            var pwdHash = Helper.PwdHash(this.OldPwd.Password, adminUser.Id.ToString());
            if (pwdHash != adminUser.Password)
            {
                MessageBox.Show("原始密碼錯誤", "訊息", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //4.更新密碼
            var newPwdHash = Helper.PwdHash(this.NewPwd.Password, adminUser.Id.ToString());
            var dbService = new AdminUserRepository(_dbConnStr);
            dbService.UpdatePassword(adminUser.Id, newPwdHash);

            //5.自動登出
            ((App)Application.Current).CurrentAdminUser = null;
            var loginWindow = new MainWindow();
            loginWindow.Show();

            var currentWindow = Window.GetWindow(this);
            if (currentWindow != null)
                currentWindow.Close();

        }
    }
}
