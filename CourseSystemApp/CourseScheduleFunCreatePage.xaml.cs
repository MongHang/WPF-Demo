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
    /// Interaction logic for CourseScheduleFunCreatePage.xaml
    /// </summary>
    public partial class CourseScheduleFunCreatePage : Window
    {
        private CourseScheduleCreateModel _newCourseScheduleModel;
        private readonly string _dbConnStr;
        public CourseScheduleFunCreatePage()
        {
            InitializeComponent();
            _dbConnStr = ((App)Application.Current).DbConnStr;
            _newCourseScheduleModel = new CourseScheduleCreateModel();
            this.DataContext = _newCourseScheduleModel;
            this.Loaded += CourseScheduleFunCreatePage_Loaded;
        }

        private void CourseScheduleFunCreatePage_Loaded(object sender, RoutedEventArgs e)
        {
            var courseRepo = new CourseRepository(_dbConnStr);
            this.courseCombo.ItemsSource = courseRepo.Query();

            var teacherRepo = new TeacherRepository(_dbConnStr);
            this.teacherCombo.ItemsSource = teacherRepo.Query();
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            var result = Save();
            if (result)
            {
                this.Close();
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            var result = Save();
            if (result)
            {
                _newCourseScheduleModel = new CourseScheduleCreateModel();
                this.DataContext = _newCourseScheduleModel;
            }
        }

        private bool Save()
        {
            //檢查必填
            if (_newCourseScheduleModel.CourseId == Guid.Empty
                || _newCourseScheduleModel.TeacherId == Guid.Empty
                || string.IsNullOrWhiteSpace(_newCourseScheduleModel.Location)
                )
            {
                MessageBox.Show("課程/講師/課程地點 必填", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            //合理性驗證
            if (_newCourseScheduleModel.Edate < _newCourseScheduleModel.Sdate)
            {
                MessageBox.Show("課程結束日必須>=開始日", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            if (_newCourseScheduleModel.Sdate < DateTime.Now.Date)
            {
                MessageBox.Show("課程開始日必須>=今天", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var repo = new CourseScheduleRepository(_dbConnStr);

            //驗證重覆開課
            if (repo.IsexistScheduleByTeacher(_newCourseScheduleModel.TeacherId, _newCourseScheduleModel.CourseId, _newCourseScheduleModel.Sdate, _newCourseScheduleModel.Edate))
            {
                MessageBox.Show("課程重覆", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            repo.CreateSchedule(_newCourseScheduleModel);
            MessageBox.Show("開課完成", "Message", MessageBoxButton.OK, MessageBoxImage.Information);
            return true;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
