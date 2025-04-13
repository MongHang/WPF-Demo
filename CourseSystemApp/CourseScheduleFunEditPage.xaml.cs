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
    /// Interaction logic for CourseScheduleFunEditPage.xaml
    /// </summary>
    public partial class CourseScheduleFunEditPage : Window
    {
        private string _dbConnStr;
        public CourseScheduleFunEditPage()
        {
            InitializeComponent();
            _dbConnStr = ((App)Application.Current).DbConnStr;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null)
            {
                MessageBox.Show("沒有課程資料");
                return;
            }
            var courseSchedule = this.DataContext as CourseScheduleInfo;
            var repo = new CourseScheduleRepository(_dbConnStr);
            repo.UpdateSchedule(courseSchedule);
            MessageBox.Show("資料更新成功");
            this.Close();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
