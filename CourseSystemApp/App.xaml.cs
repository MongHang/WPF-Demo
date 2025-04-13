using CourseLibrary.DataModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CourseSystemApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string DbConnStr { get; } = ConfigurationManager.ConnectionStrings["CourseDB"].ConnectionString;
        public AdminUserInfo CurrentAdminUser { get; set; }
    }
}
