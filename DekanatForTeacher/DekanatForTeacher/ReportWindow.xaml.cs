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
using Microsoft.Reporting.WinForms;

namespace DekanatForTeacher
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private readonly int _studyGroup;
        private readonly int _subjectInfo;

        private bool _isReportViewerLoaded;

        public ReportWindow()
        {
            InitializeComponent();

            ReportViewer.Load += ReportViewer_Load;
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (!_isReportViewerLoaded)
            {
                var reportDataSource = new ReportDataSource();

                var studentMarksDataSet = new StudentMarksDataSet();

                studentMarksDataSet.BeginInit();

                reportDataSource.Name = "DataSet1";
                reportDataSource.Value = studentMarksDataSet.MarksView;
                ReportViewer.LocalReport.DataSources.Add(reportDataSource);
                ReportViewer.LocalReport.ReportEmbeddedResource = "DekanatForTeacher.StudentMarksReport1.rdlc";

                studentMarksDataSet.EndInit();

                var studentMarksTableAdapter = new StudentMarksDataSetTableAdapters.MarksViewTableAdapter
                {
                    ClearBeforeFill = true
                };

                studentMarksTableAdapter.Fill(studentMarksDataSet.MarksView, _studyGroup, _subjectInfo);

                ReportViewer.RefreshReport();

                _isReportViewerLoaded = true;
            }
        }

        public ReportWindow(int studyGroup, int subjectInfo) : this()
        {
            _studyGroup = studyGroup;
            _subjectInfo = subjectInfo;
        }
    }
}
