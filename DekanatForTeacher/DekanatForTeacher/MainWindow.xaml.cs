using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using AutoMapper;
using System.Data.Entity;
using DekanatForTeacher.ViewModels;
using DekanatForTeacher.Mapping;

namespace DekanatForTeacher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly dbStudentsFinalEntities _dbContext;
        private readonly IMapper _mapper;
        private readonly int? _teacherId;

        private string _teacherFullName;
        public string TeacherFullName
        {
            get { return _teacherFullName; }
            set
            {
                _teacherFullName = value;
                OnPropertyChanged("TeacherFullName");
            }
        }

        private ObservableCollection<MarksViewModel> _marksCollection; 
        public ObservableCollection<MarksViewModel> MarksCollection
        {
            get { return _marksCollection; }
            set
            {
                _marksCollection = value;
                OnPropertyChanged("MarksCollection");
            }
        }

        private readonly List<AssociatedSubjects> _associatedSubjectsCollection;
        public MainWindow()
        {
            InitializeComponent();

            GroupSubjectComboBox.SelectionChanged += UpdateStudentMarksDataGrid;
            UpdateMarksButton.Click += UpdateMarksButton_Click;
            CalculateMarksButton.Click += CalculateMarksButton_Click;
            OpenReportWindowButton.Click += OpenReportWindowButton_Click;

            _dbContext = new dbStudentsFinalEntities();

            _mapper = Mapping.Mapping.Get();
        }

        public MainWindow(int? teacherId) : this()
        {
            _teacherId = teacherId;

            TeacherFullName = _dbContext.Teachers.Where(t => t.TeacherId == _teacherId)
                .Select(t => t.LastName + " " + t.FirstName + " " + t.MiddleName).First();

            _associatedSubjectsCollection =
                _dbContext.AssociatedSubjects.Where(i => i.Teacher == _teacherId).ToList();
            GroupSubjectComboBox.ItemsSource =
                _associatedSubjectsCollection.Select(i => i.Groups.Courses.CourseName + " "
                                                          + i.Groups.GroupNum + " - " +
                                                          i.SubjectInfo1.SubjectNames.SubjectName);
        }

        private void UpdateStudentMarksDataGrid(object sender, SelectionChangedEventArgs e)
        {
            var studyGroup = _associatedSubjectsCollection[GroupSubjectComboBox.SelectedIndex].StudyGroup;

            MarksCollection =
                _mapper.Map<List<Students>, ObservableCollection<MarksViewModel>>(_dbContext
                .Students.Where(s => s.StudyGroup == studyGroup).ToList());

            var subjectInfo = _associatedSubjectsCollection[GroupSubjectComboBox.SelectedIndex].SubjectInfo;

            if (!_dbContext.Marks.Any(m => m.Students.StudyGroup == studyGroup && m.SubjectInfo == subjectInfo)) return;

            foreach (var marksCollectionItem in MarksCollection)
            {
                var studentId = marksCollectionItem.StudentId;

                for (int i = 0; i < 5; i++) //hardcode
                {
                    marksCollectionItem.MarksArray[i] = _dbContext.Marks.First(
                        m => m.Student == studentId && m.SubjectInfo == subjectInfo && m.MarkType == i + 1).Mark;
                }
            }
        }

        private void UpdateMarks()
        {
            var subjectInfo = _associatedSubjectsCollection[GroupSubjectComboBox.SelectedIndex].SubjectInfo;

            foreach (var marksCollectionItem in MarksCollection)
            {
                var studentId = marksCollectionItem.StudentId;

                for (int i = 0; i < 5; i++) //harcode
                {
                    if (!_dbContext.Marks.Any(
                            m => m.Student == studentId && m.SubjectInfo == subjectInfo && m.MarkType == i + 1))
                    {
                        _dbContext.Marks.Add(new Marks
                        {
                            Student = studentId,
                            Mark = marksCollectionItem.MarksArray[i],
                            MarkDate = DateTime.Now,
                            SubjectInfo = subjectInfo,
                            MarkType = i + 1
                        });
                    }
                    else
                    {
                        _dbContext.Marks.First(
                            m => m.Student == studentId && m.SubjectInfo == subjectInfo && m.MarkType == i + 1)
                            .Mark = marksCollectionItem.MarksArray[i];
                    }
                }
            }

            try
            {
                _dbContext.SaveChanges();
                MessageBox.Show("Оцінки успішно оновлено!");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void UpdateMarksButton_Click(object sender, EventArgs e)
        {
            if (GroupSubjectComboBox.SelectedIndex == -1)
            {
                return;
            }

            UpdateMarks();
        }

        private void CalculateMarksButton_Click(object sender, EventArgs e)
        {
            if (MarksCollection == null) return;

            foreach (var marksCollectionItem in MarksCollection)
            {
                int? result = 0;

                for (int i = 0; i < 4; i++) //hardcode
                {
                    result += marksCollectionItem.MarksArray[i];
                }

                marksCollectionItem.MarkType5 = result;
            }
        }

        private void OpenReportWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (MarksCollection == null) return;

            var studyGroup = _associatedSubjectsCollection[GroupSubjectComboBox.SelectedIndex].StudyGroup;
            var subjectInfo = _associatedSubjectsCollection[GroupSubjectComboBox.SelectedIndex].SubjectInfo;

            new ReportWindow(studyGroup, subjectInfo).Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
