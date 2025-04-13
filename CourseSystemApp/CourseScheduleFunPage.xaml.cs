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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseSystemApp
{
    /// <summary>
    /// Interaction logic for CourseScheduleFunPage.xaml
    /// </summary>
    public partial class CourseScheduleFunPage : Page
    {
        private string _dbConnStr;
        private ObservableCollection<CourseScheduleInfo> _courseScheduleList;
        public CourseScheduleFunPage()
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
            var repo = new CourseScheduleRepository(_dbConnStr);
            _courseScheduleList = repo.Query(
                new CourseScheduleQueryParameter()
                {
                    CourseName = this.courseName.Text,
                    TeacherName = this.techName.Text
                });
            this.courseSchedultList.ItemsSource = _courseScheduleList;
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new CourseScheduleFunCreatePage();
            win.ShowDialog();
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var courseScheduleInfo = btn.CommandParameter as CourseScheduleInfo;
            var edieWin = new CourseScheduleFunEditPage();
            edieWin.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            edieWin.DataContext = courseScheduleInfo;
            edieWin.ShowDialog();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var courseScheduleInfo = btn.CommandParameter as CourseScheduleInfo;

            var result = MessageBox.Show($"確認要刪除 {courseScheduleInfo.CourseName} 的資料?", "Confirm Delete"
               , MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            //無人選課才能被刪除
            var repo = new StudentScheduleRepository(_dbConnStr);
            if (repo.IsExistOnStuSchedule(courseScheduleInfo.Id))
            {
                MessageBox.Show($"{courseScheduleInfo.CourseName} 課程已有學員選課，無法刪除", "Confirm Delete"
               , MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var courseScheduleRepo = new CourseScheduleRepository(_dbConnStr);
            courseScheduleRepo.DelSchedule(courseScheduleInfo.Id);
            MessageBox.Show($"{courseScheduleInfo.CourseName} 已刪除", "Confirm Delete"
             , MessageBoxButton.OK, MessageBoxImage.Information);

            Query();
        }
    }
}
