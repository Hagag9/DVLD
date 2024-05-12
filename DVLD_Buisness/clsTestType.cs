using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD_Buisness
{
    public class clsTestType
    {
        public  enum enMode { AddNew=0, Update=1 };
        public enMode Mode = enMode.AddNew;
        public enum enTestType { VisionTest=1,WrittenTest=2,StreetTest=3};

        public enTestType ID {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Fees { get; set; }

        public clsTestType()
        {
            this.ID = enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;
            this.Mode = enMode.AddNew;
        }
        public clsTestType(enTestType ID ,string Title,string Discription,float Fees)
        {
            this.ID=ID;
            this.Title = Title;
            this.Description = Discription;
            this.Fees = Fees;
            this.Mode= enMode.Update;
        }
        public static clsTestType Find(enTestType TestTypeID )
        {
            string Title = "", discription = "";
            float Fees=0;
            if (clsTestTypeData.GetTestTypeInfoByID((int)TestTypeID, ref Title, ref discription, ref Fees))
                return new clsTestType(TestTypeID, Title, discription, Fees);
            else return null;
        }
        bool _AddNew()
        {
            this.ID = (enTestType)clsTestTypeData.AddNewTestType(this.Title, this.Description, this.Fees);
            return this.Title != "";
        }
        bool _Update()
        {
            return clsTestTypeData.UpdateTestType((int)this.ID, this.Title, this.Description, this.Fees);
        }
        public static DataTable GetAllTestsTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }
        public  bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else return false;
                case enMode.Update:
                    return _Update();
            }
            return false;
        }
    }
}
