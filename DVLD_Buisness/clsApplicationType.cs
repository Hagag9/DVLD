﻿using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsApplicationType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int ID { set; get; }
        public string Title { set; get; }
        public float Fees { set; get; }
        public clsApplicationType(int ID,string Title,float Fees)
        {
            this.ID = ID;
            this.Title = Title;
            this.Fees = Fees;
            this.Mode=enMode.Update;
        }
        private  clsApplicationType()
        {
            this.ID = -1;
            this.Title = "";
            this.Fees = 0;
            this.Mode = enMode.AddNew;
        }

        private bool _AddNewApplicationType()
        {
            this.ID = clsApplicationTypeData.AddNewApplicationType(this.Title, this.Fees);
            return this.ID != -1;
        }
        private bool _UpdateApplicationType()
        {
            return clsApplicationTypeData.UpdateApplicationType(this.ID, this.Title, this.Fees);
        }
        public static clsApplicationType Find(int ID)
        {
            string Title = ""; float Fees = 0;
            if (clsApplicationTypeData.GetApplicationTypeInfoByID(ID, ref Title, ref Fees))
                return new clsApplicationType(ID, Title, Fees);
            else 
                return null;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplicationType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else return false;
                case enMode.Update:
                    return _UpdateApplicationType();
            }
            return false;
        }
    }
}
