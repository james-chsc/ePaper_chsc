using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;

namespace ElectronicPaper
{
    public partial class ElectronicPaperEditor : BaseForm
    {
        private string _name = "";
        private string _id = "";

        private string _new_name;

        public string NewName
        {
            get { return _new_name; }
        }

        public ElectronicPaperEditor(string name, string id)
        {
            InitializeComponent();
            textBoxX1.Text = _name = name;
            _id = id;
        }

        private void ElectronicPaperEditor_Load(object sender, EventArgs e)
        {
            textBoxX1.SelectAll();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxX1.Text))
            {
                textBoxX1.Text = _name;
                textBoxX1.SelectAll();
                return;
            }

            if (textBoxX1.Text == _name) return;

            try
            {
                EditElectronicPaper.UpdatePaperName(textBoxX1.Text, _id);
                _new_name = textBoxX1.Text;
            }
            catch (Exception ex)
            {
                MsgBox.Show(ex.Message);
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}