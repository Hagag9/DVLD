using DVLD.Global_Classes;
using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void BackDataEventHandeler(object sender,int PersonID);
        public BackDataEventHandeler DataBack;
       public enum enMode { AddNew=0,Update=1};
       public enum enGendor { Male=0,Female=1};

        private clsPerson _Person;
        private int _PersonID = -1;
        private enMode _Mode;
        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;  
        }
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _PersonID=PersonID;
            _Mode = enMode.Update;
        }

        private void _ResetDefaultValues()
        {
            _FillCountriesInComboBox();
            if( _Mode == enMode.AddNew ) 
            {
                lblTitle.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else 
            { lblTitle.Text = "Update Person"; }
            if (rbMale.Checked) 
                pbPersonImage.Image = Resources.Male_512;
            else 
                pbPersonImage.Image = Resources.Female_512;

            llRemoveImage.Visible=(pbPersonImage.ImageLocation!=null);

            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            cbCountry.SelectedIndex = cbCountry.FindString("Egypt");

            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            rbMale.Checked = true;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
        }
        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();
            foreach(DataRow row in  dtCountries.Rows) 
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
            
        }
        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);
            if(_Person == null)
            {
                MessageBox.Show($"No Person with ID ={_PersonID}","Person Not Found",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            lblPersonID.Text = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if(_Person.Gendor==0)
                rbMale.Checked=true;
            else rbFemale.Checked=true;

            txtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone;
            txtEmail.Text = _Person.Email;
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);
            if(_Person.ImagePath!="")
                pbPersonImage.ImageLocation=_Person.ImagePath;
            llRemoveImage.Visible=(_Person.ImagePath!="");
         }
     

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if(_Mode==enMode.Update) 
                _LoadData();
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image File |*.jpg;*.jpeg;*.gif;*.png;-.pmb";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory=true;
            if(openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                string SelectedFilePath=openFileDialog1.FileName;
                pbPersonImage.Load(SelectedFilePath);
                llRemoveImage.Visible = true;
            }
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox temp = (TextBox)sender;
            if (string.IsNullOrEmpty(temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(temp, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(temp, null);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
                return;

            if (!clsValidation.EmailValidation(txtEmail.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
                errorProvider1.SetError(txtEmail, null);
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel= true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
                errorProvider1.SetError(txtNationalNo, null);

         if(txtNationalNo.Text.Trim()!=_Person.NationalNo &&clsPerson.isPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel=true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
            }
         else 
                errorProvider1.SetError(txtNationalNo, null);

         }
        private  bool _HandelImagePerson()
        {
            if(_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException)
                    {

                    }
                }
            }
                if(pbPersonImage.ImageLocation!=null)
                {
                    string sourceImageFile = pbPersonImage.ImageLocation.ToString();
                    if (clsUtil.CopyImageToProjectImagesFolder(ref sourceImageFile))
                    {
                        pbPersonImage.ImageLocation = sourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                return true;
         }
        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;
            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Man_32;
            pbPersonImage.Image = Resources.Female_512;
            llRemoveImage.Visible = false;

        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if(pbPersonImage.ImageLocation== null)
                pbPersonImage.Image=Resources.Man_32;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Female_512;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!_HandelImagePerson())
                return;


            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName=txtSecondName.Text.Trim();
            _Person.ThirdName=txtThirdName.Text.Trim();
            _Person.LastName=txtLastName.Text.Trim();
            _Person.NationalNo=txtNationalNo.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address=txtAddress.Text.Trim();
            if (rbMale.Checked)
                _Person.Gendor = (short)enGendor.Male;
            else _Person.Gendor = (short)enGendor.Female;
            _Person.NationalityCountryID = clsCountry.Find(cbCountry.Text).ID;
            if (pbPersonImage.ImageLocation != null)
                _Person.ImagePath = pbPersonImage.ImageLocation;
            else _Person.ImagePath = "";
            
            if(_Person.Save())
            {
                lblPersonID.Text=_Person.PersonID.ToString();
                lblTitle.Text= "Update Person";
                _Mode = enMode.Update;

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DataBack?.Invoke(this, _Person.PersonID);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
