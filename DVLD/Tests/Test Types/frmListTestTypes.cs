using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.Test_Types
{
    public partial class frmListTestTypes : Form
    {
        DataTable _dtAllTestTypes;
        public frmListTestTypes()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
;       }

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _dtAllTestTypes=clsTestType.GetAllTestsTypes();
            dgvTestTypes.DataSource = _dtAllTestTypes;
            lblRecords.Text=dgvTestTypes.RowCount.ToString();
            if(dgvTestTypes.Rows.Count>0)
            {
                dgvTestTypes.Columns[0].HeaderText = "ID";
                dgvTestTypes.Columns[0].Width =120;

                dgvTestTypes.Columns[1].HeaderText = "Title";
                dgvTestTypes.Columns[1].Width = 200;


                dgvTestTypes.Columns[1].HeaderText = "Discription";
                dgvTestTypes.Columns[1].Width = 400;
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmEditTestType frm = new frmEditTestType((clsTestType.enTestType)dgvTestTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
    }
}
