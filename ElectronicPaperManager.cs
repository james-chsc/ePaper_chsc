using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using FISCA.Presentation.Controls;
using FISCA.DSAUtil;

namespace ElectronicPaper
{
    public partial class ElectronicPaperManager : BaseForm
    {
        private BackgroundWorker _itemContentLoader = new BackgroundWorker();
        private Dictionary<string, string> _viewers = new Dictionary<string, string>();

        public ElectronicPaperManager()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            InitializeViewerTypeList();
            InitializeSemester();
        }

        private void InitializeViewerTypeList()
        {
            _viewers.Add("Student", "學生");
            _viewers.Add("Class", "班級");
            _viewers.Add("Teacher", "教師");
            _viewers.Add("Course", "課程");
        }

        #region InitializeBackgroundWorker
        private void InitializeBackgroundWorker()
        {
            _itemContentLoader.DoWork += new DoWorkEventHandler(_itemContentLoader_DoWork);
            _itemContentLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_itemContentLoader_RunWorkerCompleted);
            _itemContentLoader.WorkerReportsProgress = true;
        }
        #endregion

        #region InitializeSemester
        private void InitializeSemester()
        {
            try
            {
                for (int i = -2; i <= 2; i++) //只顯示前後兩個學年的選項，其他的用手動輸入。
                {
                    cboSchoolYear.Items.Add(K12.Data.School.DefaultSchoolYear + i);
                }

                cboSchoolYear.Text = K12.Data.School.DefaultSchoolYear.ToString();
                cboSemester.Text = K12.Data.School.DefaultSemester;
            }
            catch { }
        }
        #endregion

        private void _itemContentLoader_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void _itemContentLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void ElectronicPaperManager_Load(object sender, EventArgs e)
        {
            LoadElectronicPaper(cboSchoolYear.Text, cboSemester.Text);
        }

        private void LoadElectronicPaper(string school_year, string semester)
        {
            DSXmlHelper helper = null;
            try
            {
                helper = QueryElectronicPaper.GetDetailList(school_year, semester).GetContent();
            }
            catch (Exception)
            {
                MsgBox.Show("取得電子報表發生錯誤。");
                helper = new DSXmlHelper("BOOM");
            }

            dgvPaperList.SuspendLayout();
            dgvPaperList.Rows.Clear();

            foreach (XmlElement paper in helper.GetElements("Paper"))
            {
                DSXmlHelper paperHelper = new DSXmlHelper(paper);

                DateTime try_datetime;
                if (!DateTime.TryParse(paperHelper.GetText("Timestamp"), out try_datetime))
                    try_datetime = DateTime.MinValue;

                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dgvPaperList,
                    paperHelper.GetText("@ID"),
                    paperHelper.GetText("Name"),
                    paperHelper.GetText("Overview"),
                    paperHelper.GetText("Metadata"),
                    paperHelper.GetText("SchoolYear"),
                    paperHelper.GetText("Semester"),
                    paperHelper.GetText("ItemCount"),
                    _viewers[paperHelper.GetText("ViewerType")],
                    try_datetime.ToString("yyyy-MM-dd hh:mm:ss"));
                dgvPaperList.Rows.Add(row);
            }

            dgvPaperList.ResumeLayout();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (dgvPaperList.SelectedRows.Count <= 0) return;
            DataGridViewRow row = dgvPaperList.SelectedRows[0];

        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (dgvPaperList.SelectedRows.Count <= 0) return;
            DataGridViewRow row = dgvPaperList.SelectedRows[0];

            string name = "" + row.Cells[colName.Index].Value;
            string id = "" + row.Cells[colID.Index].Value;

            ElectronicPaperEditor editor = new ElectronicPaperEditor(name, id);
            if (editor.ShowDialog() != DialogResult.OK)
                return;

            row.Cells[colName.Index].Value = editor.NewName;
            //LoadElectronicPaper(cboSchoolYear.Text, cboSemester.Text);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPaperList.SelectedRows.Count <= 0) return;

            if (MsgBox.Show("您確定要刪除 " + dgvPaperList.SelectedRows.Count.ToString() + " 筆電子報表嗎？", "刪除確認", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var ids = new List<string>();
            foreach (DataGridViewRow row in dgvPaperList.SelectedRows)
            {
                ids.Add("" + row.Cells[colID.Index].Value);
            }

            try
            {
                EditElectronicPaper.Delete(ids.ToArray());
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }

            LoadElectronicPaper(cboSchoolYear.Text, cboSemester.Text);
            //dgvPaperList.ClearSelection();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboSchoolYear_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboSemester_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadElectronicPaper(cboSchoolYear.Text, cboSemester.Text);
        }

        private void cboSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadElectronicPaper(cboSchoolYear.Text, cboSemester.Text);
        }
    }
}