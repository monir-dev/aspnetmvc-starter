using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using aspnetmvc_starter.Web.Utility;
using aspnetmvc_starter.Web.Utility.Resources;
using ModelStateDictionary = System.Web.ModelBinding.ModelStateDictionary;

namespace aspnetmvc_starter.Web.Utility
{
    public class Common
    {
        #region ENUMS

        public static string GetDescription(Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public enum MinistryTypeEnum
        {
            Ministry = 1,
            Division = 2,
            Other = 3
        }

        public enum AidCategotyEnum
        {
            Loan = 1,
            Grant = 2
        }

        //tblExecutingAgencyType
        public enum ExecutingAgencyTypeEnum
        {
            [Description("Government")]
            Government = 1,
            [Description("Donor")]
            Donor = 2,
            [Description("NGO/CSO")]
            NgoCso = 3,
            [Description("Others")]
            Others = 4

            //Uses of Description
            //ExecutingAgencyTypeEnum x = ExecutingAgencyTypeEnum.NgoCso;
            //string description = x.GetDescription();
        }

        //tblOrganizationType - used for User identification
        public enum OrganizationTypeEnum
        {
            [Description("Source of Fund")]
            SourceOfFund = 1,
            [Description("Ministry")]
            Ministry = 2,
            [Description("NGO/CSO")]
            NgoCso = 3,
            [Description("Economic Relations Division (ERD)")]
            ERD = 4,
            [Description("Foreign Aid Budget & Accounts (FABA)")]
            FABA = 5,
            [Description("Bangladesh Bank")]
            BangladeshBank = 6,
            [Description("NGO Affairs Bureau (NGOAB)")]
            NGOAB = 7,
            [Description("Others")]
            Others = 8

            //Uses of Description
            //OrganizationTypeEnum x = OrganizationTypeEnum.NgoCso;
            //string description = x.GetDescription();
        }

        //tblExecutingAgencyType
        public enum ProjectPermissionTypeEnum
        {
            [Description("Project wise")]
            Project = 1,
            [Description("Fund Source wise")]
            FundSource = 2

            //Uses of Description
            //ProjectPermissionTypeEnum x = ProjectPermissionTypeEnum.NgoCso;
            //string description = x.GetDescription();
        }

        public enum AIDEffectivenessInfoEnum
        {
            AnalyticalWork = 1,
            Mission = 2
        }

        public enum MonthNoEnum
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        public enum DayEnum
        {
            Sunday = 1,
            Monday = 2,
            Tuesday = 3,
            Wednesday = 4,
            Thursday = 5,
            Friday = 6,
            Satureday = 7
        }

        public enum BudgetTypeEnum
        {
            On_Budget = 1,
            Off_Budget = 0,
            Both = 2
        }

        public enum FiguresEnum
        {
            Original = 1,
            Thousands = 1000,
            Lakhs = 100000,
            Millions = 1000000
        }
        #endregion

        #region DateTime

        public static DateTime CurrentDateTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        public static string FormatDate(string strDate, string inputFormat, string outputFormat)
        {
            string outputDate = "";
            try
            {
                DateTime dt = DateTime.ParseExact(strDate, inputFormat, null);
                outputDate = dt.ToString(outputFormat);
            }
            catch
            {

            }

            return outputDate;
        }

        public static string FormatDateTime(DateTime? dtDate)
        {
            if (dtDate == null || dtDate == DateTime.MinValue)
            {
                return "";
            }
            else
            {
                return dtDate.Value.ToString("dd-MM-yyyy");
            }
        }

        public static string FormatDateTime(string txtDate)
        {
            if (string.IsNullOrEmpty(txtDate))
            {
                return "";
            }
            //DateTime dt = DateTime.ParseExact(txtDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dt = Convert.ToDateTime(txtDate);
            if (dt.CompareTo(DateTime.MinValue) == 0)
            {
                return "";
            }
            else
            {
                return dt.ToString("dd-MM-yyyy");
            }
        }

        public static DateTime FormatDateforSQL(string txtDate)
        {
            if (!string.IsNullOrEmpty(txtDate))
            {
                return DateTime.Parse(txtDate, new CultureInfo("fr-Fr", true), DateTimeStyles.None);
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        //public static ArrayList GetMonthRange(DateTime startDate, DateTime endDate)
        //{
        //    ArrayList MonthList = new ArrayList();
        //    endDate = endDate.AddDays(30 - endDate.Day);

        //    MonthList.Add(startDate);
        //    startDate = startDate.AddMonths(1);

        //    while (startDate <= endDate)
        //    {
        //        MonthList.Add(startDate);
        //        startDate = startDate.AddMonths(1);
        //    }
        //    return MonthList;
        //}

        public static DateTime? FormatStringToDate(string txtDate)
        {
            if (!string.IsNullOrEmpty(txtDate))
            {
                try
                {
                    return DateTime.ParseExact(txtDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch { }
            }
            return null;
        }

        public static DateTime FormatDate(string strDate, string inputFormat)
        {
            DateTime outputDate = DateTime.MinValue;
            try
            {
                outputDate = DateTime.ParseExact(strDate, inputFormat, null);

            }
            catch
            {

            }
            return outputDate;
        }

        public static string ErrorListToString(List<string> ErrorList)
        {
            string errMsg = "";
            if (ErrorList.Count > 0)
            {
                errMsg = "Following errors found -";
                foreach (string str in ErrorList)
                {
                    errMsg += "#" + str;
                }
            }


            return errMsg;
        }

        public static int GetMonthNo(string strmonth)
        {
            int intMonthNo = 0;

            switch (strmonth)
            {
                case "January":
                    intMonthNo = 1;
                    break;
                case "February":
                    intMonthNo = 2;
                    break;
                case "March":
                    intMonthNo = 3;
                    break;
                case "April":
                    intMonthNo = 4;
                    break;
                case "May":
                    intMonthNo = 5;
                    break;

                case "June":
                    intMonthNo = 6;
                    break;
                case "July":
                    intMonthNo = 7;
                    break;
                case "August":
                    intMonthNo = 8;
                    break;
                case "September":
                    intMonthNo = 9;
                    break;
                case "October":
                    intMonthNo = 10;
                    break;
                case "November":
                    intMonthNo = 11;
                    break;
                case "December":
                    intMonthNo = 12;
                    break;
            }

            return intMonthNo;
        }

        public static string GetMonthName(int monthNo)
        {
            string MonthName = "";

            switch (monthNo)
            {
                case 1:
                    MonthName = "January";
                    break;
                case 2:
                    MonthName = "February";
                    break;
                case 3:
                    MonthName = "March";
                    break;
                case 4:
                    MonthName = "April";
                    break;
                case 5:
                    MonthName = "May";
                    break;
                case 6:
                    MonthName = "June";
                    break;
                case 7:
                    MonthName = "July";
                    break;
                case 8:
                    MonthName = "August";
                    break;
                case 9:
                    MonthName = "September";
                    break;
                case 10:
                    MonthName = "October";
                    break;
                case 11:
                    MonthName = "November";
                    break;
                case 12:
                    MonthName = "December";
                    break;
            }

            return MonthName;
        }

        public static IList<SelectListItem> PopulateYearList()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            for (int i = DateTime.Now.Year; i >= 1971; i--)
            {
                list.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }
            return list;
        }

        public static IList<SelectListItem> PopulateYearList(int noOfFutureYear)
        {
            IList<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem { Text = "- Select One -", Value = "" });
            for (int i = DateTime.Now.Year + noOfFutureYear; i >= 1971; i--) //for (int i = 1971; i <= DateTime.Now.Year + noOfFutureYear; i++) 
            {
                if (i == DateTime.Now.Year)
                    list.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = true });
                else
                    list.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return list;
        }

        public static IList<SelectListItem> PopulateFiscalYearList(int noOfFutureYear)
        {
            IList<SelectListItem> IncomeYear = new List<SelectListItem>();
            for (int i = DateTime.Now.Year + noOfFutureYear; i >= 1971; i--)
            {
                var iyFormat = i + "-" + (i + 1);
                IncomeYear.Add(new SelectListItem() { Text = iyFormat, Value = iyFormat });
            }
            return IncomeYear;
        }

        public static IList<SelectListItem> PopulateMonthList()
        {
            var list = new List<SelectListItem>();

            list.Add(new SelectListItem() { Text = "January", Value = "January", });
            list.Add(new SelectListItem() { Text = "February", Value = "February", });
            list.Add(new SelectListItem() { Text = "March", Value = "March", });
            list.Add(new SelectListItem() { Text = "April", Value = "April", });
            list.Add(new SelectListItem() { Text = "May", Value = "May", });
            list.Add(new SelectListItem() { Text = "June", Value = "June", });
            list.Add(new SelectListItem() { Text = "July", Value = "July", });
            list.Add(new SelectListItem() { Text = "August", Value = "August", });
            list.Add(new SelectListItem() { Text = "September", Value = "September", });
            list.Add(new SelectListItem() { Text = "October", Value = "October", });
            list.Add(new SelectListItem() { Text = "November", Value = "November", });
            list.Add(new SelectListItem() { Text = "December", Value = "December", });

            return list;
        }

        public static IList<SelectListItem> PopulateMonthNoList()
        {
            var list = new List<SelectListItem>();

            list.Add(new SelectListItem() { Text = "January", Value = "1", });
            list.Add(new SelectListItem() { Text = "February", Value = "2", });
            list.Add(new SelectListItem() { Text = "March", Value = "3", });
            list.Add(new SelectListItem() { Text = "April", Value = "4", });
            list.Add(new SelectListItem() { Text = "May", Value = "5", });
            list.Add(new SelectListItem() { Text = "June", Value = "6", });
            list.Add(new SelectListItem() { Text = "July", Value = "7", });
            list.Add(new SelectListItem() { Text = "August", Value = "8", });
            list.Add(new SelectListItem() { Text = "September", Value = "9", });
            list.Add(new SelectListItem() { Text = "October", Value = "10", });
            list.Add(new SelectListItem() { Text = "November", Value = "11", });
            list.Add(new SelectListItem() { Text = "December", Value = "12", });

            return list;
        }

        public static IList<SelectListItem> PopulateMonthListForReport()
        {
            var list = new List<SelectListItem>();
            DateTime month = new DateTime(DateTime.Now.Year, 1, 1);
            for (int i = 0; i < 12; i++)
            {
                DateTime NextMont = month.AddMonths(i);
                list.Add(new SelectListItem() { Text = NextMont.ToString("MMMM"), Value = NextMont.Month.ToString() });
            }

            return list;
        }

        public static int GetAgebyDateOfBirth(DateTime dateTime)
        {
            if (dateTime != null)
            {
                // get the difference in years
                int years = DateTime.Now.Year - ((DateTime)(dateTime)).Year;
                // subtract another year if we're before the
                // birth day in the current year
                if (DateTime.Now.Month < ((DateTime)(dateTime)).Month ||
                    (DateTime.Now.Month == ((DateTime)(dateTime)).Month &&
                    DateTime.Now.Day < ((DateTime)(dateTime)).Day))
                    years--;
                return years;
            }
            else
                return 0;
        }
        #endregion

        #region DropDownList

        public static IList<SelectListItem> PopulateMenuList(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.MenuName,
                    Value = item.Id.ToString()
                });
            }

            return list;
        }

        public static IList<SelectListItem> PopulateSelectOne()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="- Select One -", Value="0", Selected = true },
            };

            return list.OrderBy(x => x.Text).ToList();
        }
        public static IList<SelectListItem> PopulateNotApplicable()
        {
            var list = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="- N/A -", Value="", Selected = true },
            };

            return list.OrderBy(x => x.Text).ToList();
        }

        public static IList<SelectListItem> PopulateDllList(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                 {
                     Text = item.Name,
                     Value = item.Id.ToString()
                 });
            }

            return list.OrderBy(x => x.Text).ToList();
        }

        public static IList<SelectListItem> PopulateUserWiseBusinessDllList(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.BusinessName,
                    Value = item.Business1.ToString()
                });
            }

            return list.OrderBy(x => x.Text).ToList();
        }

        public static IList<SelectListItem> PopulateUserAndBusinessWiseLevel3DllList(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Level3Name,
                    Value = item.Level31.ToString()
                });
            }

            return list.OrderBy(x => x.Text).ToList();
        }

        public static IList<SelectListItem> PopulateUserAndBusinessWiseLevel2DllList(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Level2Name,
                    Value = item.Level21.ToString()
                });
            }

            return list.OrderBy(x => x.Text).ToList();
        }

        public static IList<SelectListItem> PopulateUserAndBusinessWiseTerritoryDllList(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Base,
                    Value = item.Level11.ToString()
                });
            }

            return list.OrderBy(x => x.Text).ToList();
        }

        public static IList<SelectListItem> PopulateMachineDllList(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.MachineName,
                    Value = item.Id.ToString()
                });
            }

            return list.OrderBy(x => x.Text).ToList();
        }


        public static IList<SelectListItem> PopulateExcelSheetNamesDllList(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                {
                    Text = item,
                    Value = item.ToString()
                });
            }

            return list.OrderBy(x => x.Text).ToList();
        }


        ////CommonConfig
        //public static IList<SelectListItem> PopulateCommonConfigDllList(BMSCommonSevice _aimsCommonservice, string strDisplayName, bool isEdit = false, string selectedValue = "0", string selectText = "- Select One -")
        //{
        //    int totalRecords;
        //    var ddlList = _aimsCommonservice.AIMSUnit.FunctionRepository.GetAllCommonConfig(strDisplayName, 0, "", "", "", 0, 0, out totalRecords);
        //    var list = SelectListItemExtension.PopulateDropdownList(ddlList.ToList<CommonConfigGetResult>(), "Id", "Name", isEdit, selectedValue, selectText).ToList();
        //    return list.OrderBy(x => x.Text).ToList();
        //}

        //public static IList<SelectListItem> PopulateCurrencyByFundSourceList(BMSCommonSevice _aimsCommonservice, int leadDPFundSourceId)
        //{
        //    int totalRecords;
        //    var fundSource = _aimsCommonservice.AIMSUnit.FundSourceRepository.GetByID(leadDPFundSourceId);

        //    var ddlList = _aimsCommonservice.AIMSUnit.FunctionRepository.GetAllCommonConfig("Currency", 0, "", "", "", 0, 0, out totalRecords);
        //    var list = SelectListItemExtension.PopulateDropdownList(ddlList.ToList<CommonConfigGetResult>(), "Id", "Name", true, fundSource != null ? fundSource.CurrencyId.ToString() : "0").ToList();
        //    return list.OrderBy(x => x.Text).ToList();
        //}

        //public static IList<SelectListItem> PopulateFundSourceForPlannedDisbursementList(BMSCommonSevice _aimsCommonservice, int projectId)
        //{

        //    var projectFundingCommitmentList = _aimsCommonservice.AIMSUnit.ProjectFundingCommitmentRepository.GetAll().Where(x => x.ProjectId == projectId).DistinctBy(x => x.FundSourceId).ToList();

        //    var fundSourceList = _aimsCommonservice.AIMSUnit.FundSourceRepository.GetAll().ToList();

        //    var ddlList = new List<tblFundSource>();

        //    foreach (var projectFundingCommitment in projectFundingCommitmentList)
        //    {
        //        var selectFundSource = fundSourceList.Where(x => x.Id == projectFundingCommitment.FundSourceId).ToList();

        //        ddlList.AddRange(selectFundSource);
        //    }

        //    var list = SelectListItemExtension.PopulateDropdownList(ddlList.ToList<tblFundSource>(), "Id", "FundSourceName").ToList();


        //    return list.OrderBy(x => x.Text).ToList();
        //}

        //public static IList<SelectListItem> PopulateMinistryAgencyForAIDEffectivenessList(BMSCommonSevice _aimsCommonservice, int projectId)
        //{

        //    var projectGoBExecutingAgencyList = _aimsCommonservice.AIMSUnit.ProjectGoBExecutingAgencyRepository.GetAll().Where(x => x.ProjectId == projectId).DistinctBy(x => x.GOBExeAgencyMinistryAgencyId).ToList();

        //    var ministryAgencyList = _aimsCommonservice.AIMSUnit.MinistryAgencyRepository.GetAll().ToList();

        //    var ddlList = new List<tblMinistryAgency>();

        //    foreach (var projectGoBExecutingAgency in projectGoBExecutingAgencyList)
        //    {
        //        var selectFundSource = ministryAgencyList.Where(x => x.Id == projectGoBExecutingAgency.GOBExeAgencyMinistryAgencyId).ToList();

        //        ddlList.AddRange(selectFundSource);
        //    }

        //    var list = SelectListItemExtension.PopulateDropdownList(ddlList.ToList<tblMinistryAgency>(), "Id", "AgencyName").ToList();

        //    return list.OrderBy(x => x.Text).ToList();
        //}


        ////FundSource
        //public static IList<SelectListItem> PopulateFundSourceDllList(dynamic ddlList)
        //{
        //    var list = new List<SelectListItem>();
        //    foreach (var item in ddlList)
        //    {
        //        list.Add(new SelectListItem()
        //        {
        //            Text = item.FundSourceName,
        //            Value = item.Id.ToString()
        //        });
        //    }

        //    return list.OrderBy(x => x.Text).ToList();
        //}
        //public static List<SelectListItem> PopulateFundSourceDllList(BMSCommonSevice _aimsCommonservice, bool filterByID)
        //{
        //    var FundSource = new List<SelectListItem>();
        //    if (AppConstant.IsDonor && filterByID)
        //    {
        //        int fundSourceId = AppConstant.OrgTypeFundSourceMinistryId;
        //        FundSource = SelectListItemExtension.PopulateDropdownList(_aimsCommonservice.AIMSUnit.FundSourceRepository.GetAll().Where(q => q.Id == fundSourceId).ToList<tblFundSource>(), "Id", "FundSourceName").ToList();
        //    }
        //    else
        //    {
        //        FundSource = SelectListItemExtension.PopulateDropdownList(_aimsCommonservice.AIMSUnit.FundSourceRepository.GetAll().ToList<tblFundSource>(), "Id", "FundSourceName").ToList();
        //    }

        //    return FundSource.ToList();
        //}
        //NGOCSO
        public static IList<SelectListItem> PopulateNGOCSODllList(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.NGOOrganizationName,
                    Value = item.Id.ToString()
                });
            }

            return list.OrderBy(x => x.Text).ToList();
        }

        //MinistryDivision
        //public static IList<SelectListItem> PopulateMinistryDivisionDllList(dynamic ddlList)
        //{
        //    var list = new List<SelectListItem>();
        //    foreach (var item in ddlList)
        //    {
        //        list.Add(new SelectListItem()
        //        {
        //            Text = item.DivisionName,
        //            Value = item.Id.ToString()
        //        });
        //    }
        //    return list;
        //}

        //MinistryAgency
        public static IList<SelectListItem> PopulateMinistryAgencyDllList(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.AgencyName,
                    Value = item.Id.ToString()
                });
            }

            return list.OrderBy(x => x.Text).ToList();
        }
        //SectorSubSector
        public static IList<SelectListItem> PopulateSectorSubSectorDllList(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.SubSectorName,
                    Value = item.Id.ToString()
                });
            }

            return list.OrderBy(x => x.Text).ToList();
        }

        //District
        public static IList<SelectListItem> PopulateDistrictDDL(dynamic ddlList)
        {
            var list = new List<SelectListItem>();
            foreach (var item in ddlList)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.DistrictName,
                    Value = item.Id.ToString()
                });
            }
            return list.OrderBy(x => x.Text).ToList();
        }

        public static IList<SelectListItem> PopulateMinistryType()
        {
            var qList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="- Select One -", Value="", Selected = true },
                new SelectListItem(){ Text=MinistryTypeEnum.Ministry.ToString(), Value=MinistryTypeEnum.Ministry.ToString()},
                new SelectListItem(){ Text=MinistryTypeEnum.Division.ToString(), Value=MinistryTypeEnum.Division.ToString()},
                new SelectListItem(){ Text=MinistryTypeEnum.Other.ToString(), Value=MinistryTypeEnum.Other.ToString()},
            };

            return qList.OrderBy(x => x.Text).ToList();
        }


        public static IList<SelectListItem> PopulateExecutingAgencyType()
        {
            var qList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="- Select One -", Value="0", Selected = true },
                new SelectListItem(){ Text=ExecutingAgencyTypeEnum.Government.ToString(), Value=ExecutingAgencyTypeEnum.Government.ToString()},
                new SelectListItem(){ Text=ExecutingAgencyTypeEnum.Donor.ToString(), Value=ExecutingAgencyTypeEnum.Donor.ToString()},
                new SelectListItem(){ Text=ExecutingAgencyTypeEnum.NgoCso.ToString(), Value=ExecutingAgencyTypeEnum.NgoCso.ToString()},
                new SelectListItem(){ Text=ExecutingAgencyTypeEnum.Others.ToString(), Value=ExecutingAgencyTypeEnum.Others.ToString()},  
            };

            return qList.OrderBy(x => x.Text).ToList();
        }

        public static IList<SelectListItem> PopulateAreaType()
        {
            var qList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="Upazila", Value="Upazila", Selected = true },
                new SelectListItem(){ Text="Pourashova", Value="Pourashova" },
                new SelectListItem(){ Text="City Corporation", Value="City Corporation" }
            };

            return qList.OrderBy(x => x.Text).ToList();
        }

        public static IList<SelectListItem> PopulateMonetaryValueType()
        {
            var qList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text=FiguresEnum.Original.ToString(), Value=Convert.ToString((int)FiguresEnum.Original), Selected = true },
                new SelectListItem(){ Text=FiguresEnum.Thousands.ToString(), Value=Convert.ToString((int)FiguresEnum.Thousands) },
                new SelectListItem(){ Text=FiguresEnum.Lakhs.ToString(), Value=Convert.ToString((int)FiguresEnum.Lakhs) },
                new SelectListItem(){ Text=FiguresEnum.Millions.ToString(), Value=Convert.ToString((int)FiguresEnum.Millions)},
            };

            //return qList.OrderBy(x => x.Text).ToList();
            return qList.ToList();
        }

        public static IList<SelectListItem> PopulateBudgetType()
        {
            var qList = new List<SelectListItem>()
            {
                
                new SelectListItem(){ Text=BudgetTypeEnum.On_Budget .ToString(), Value=Convert.ToString((int)BudgetTypeEnum.On_Budget), Selected = true },
                new SelectListItem(){ Text=BudgetTypeEnum.Off_Budget.ToString(), Value=Convert.ToString((int)BudgetTypeEnum.Off_Budget) },
                new SelectListItem(){ Text=BudgetTypeEnum.Both.ToString(), Value=Convert.ToString((int)BudgetTypeEnum.Both) },
                //new SelectListItem(){ Text="On-Budget", Value="1", Selected = true },
                //new SelectListItem(){ Text="Off-Budget", Value="0" },
                //new SelectListItem(){ Text="Both", Value="-1" }
            };

            //return qList.OrderBy(x => x.Text).ToList();
            return qList.ToList();
        }

        public static IList<SelectListItem> PopulateProjectPermissionType()
        {
            var qList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text=ProjectPermissionTypeEnum.Project .ToString(), Value=Convert.ToString((int)ProjectPermissionTypeEnum.Project), Selected = true },
                new SelectListItem(){ Text=ProjectPermissionTypeEnum.FundSource.ToString(), Value=Convert.ToString((int)ProjectPermissionTypeEnum.FundSource) }
            };

            return qList.ToList();
        }

        #endregion

        #region Encryption
        private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");

        public static string GetEncryptionKey()
        {
            return "10";
        }

        // a 32 character hexadecimal string.
        public static string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
                //sBuilder.Append(Convert.ToString(data[i], 2).PadLeft(8, '0')); //Convert into binary
            }

            // Return the hexadecimal string.
            return sBuilder.ToString().ToLower();
        }

        // Verify a hash against a string.
        public static bool verifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = getMd5Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// Encrypt the given string using AES.  The string can be decrypted using 
        /// DecryptStringAES().  The sharedSecret parameters must match.
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        public static string EncryptStringAES(string plainText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException("plainText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            string outStr = null;                       // Encrypted string to return
            RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                    }
                    outStr = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return outStr;
        }

        /// <summary>
        /// Decrypt the given string.  Assumes the string was encrypted using 
        /// EncryptStringAES(), using an identical sharedSecret.
        /// </summary>
        /// <param name="cipherText">The text to decrypt.</param>
        /// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        public static string DecryptStringAES(string cipherText, string sharedSecret)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException("cipherText");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentNullException("sharedSecret");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            try
            {
                // generate the key from the shared secret and the salt
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged();
                aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
                aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.                
                byte[] bytes = Convert.FromBase64String(cipherText);
                using (MemoryStream msDecrypt = new MemoryStream(bytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        #endregion

        #region Mail
        /*
        private static SmtpClient _emailClient;
        private static SmtpClient ConfigureClient(string host, int port, string networkUser, string password)
        {
            var client = new SmtpClient();
            client.Host = host; // "smtp.gmail.com";
            client.Port = port;
            //client.EnableSsl = true; //should be enabled for ssl        
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(networkUser, password);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            return client;
        }
         
        public static void SendMail(List<string> reciepientList, string[] copyList, string subject, string emailBody, EmailTemplateConfigDataViewModel model)
        {
            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(model.FromEmailAddress, model.FromName);

            mailMessage.To.Add(reciepientList[0]);
            for (int i = 0; i < reciepientList.Count; i++)
            {
                if (i != 0) mailMessage.CC.Add(reciepientList[i]);
            }
            for (int j = 0; j < copyList.Count(); j++)
            {
                mailMessage.CC.Add(copyList[j]);
            }
            mailMessage.Priority = MailPriority.High;
            mailMessage.Subject = subject;
            mailMessage.Body = emailBody;
            mailMessage.IsBodyHtml = false;

            _emailClient = Common.ConfigureClient(model.SMTPServer, model.SMTPPort, model.SMTPUserName, model.SMTPUserPassword);
            _emailClient.Send(mailMessage);
        }
        */
        #endregion

        /// <summary>
        /// Kendo UI Grid Action Links Generate Method
        /// Create By : Rasel
        /// </summary>
        /// <param name="areaName"></param>
        /// <param name="controllerName"></param>
        /// <param name="id"></param>
        /// <param name="isDetailPermitted"></param>
        /// <param name="isEditPermitted"></param>
        /// <param name="isDeletePermitted"></param>
        /// <returns></returns>
        public static string KendoUIGridActionLinkGenerate(string areaName, string controllerName, string id, bool isDetailPermitted = true, bool isEditPermitted = true, bool isDeletePermitted = true)
        {
            string strLink = string.Empty;

            if (isDetailPermitted)
            {
                //Details Link
                strLink += @"<a class='lnkDetail" + controllerName + " btn btn-minier btn-success' href='/" + areaName + "/" + controllerName + "/Details/" + id + "' title='View' ><i class='fa fa-eye'></i></a>&nbsp;&nbsp;";
            }

            if (isEditPermitted)
            {
                //Edit Link
                strLink += "<a class='lnkEdit" + controllerName + " btn btn-minier btn-info' href='/" + areaName + "/" + controllerName + "/Edit/" + id + "' title='Edit' ><i class='fa fa-edit'></i></a>&nbsp;&nbsp;";
            }

            if (isDeletePermitted)
            {
                //Delete Link
                strLink += "<a class='lnkDelete" + controllerName + " btn btn-minier btn-danger' Id ='js_delete' href='/" + areaName + "/" + controllerName + "/Delete/" + id + "' title='Delete' ><i class='fa fa-trash'></i></a>";
            }

            return strLink;

        }

        #region Exception Handling
        public static string GetSqlExceptionMessage(int number)
        {
            //set default value which is the generic exception message          

            string error = ErrorMessages.ExceptionMessage.ToString(); //MyConfiguration.Texts.GetString(ExceptionKeys.DalExceptionOccured);
            switch (number)
            {
                case 4060:
                    // Invalid Database
                    error = ErrorMessages.InvalidDatabase.ToString();
                    break;
                case 18456:
                    // Login Failed
                    error = ErrorMessages.DBLoginFailed.ToString();
                    break;

                case 547:
                    // ForeignKey Violation
                    error = ErrorMessages.ForeignKeyViolation.ToString();
                    break;
                case 2732:
                    // ForeignKey Violation
                    error = ErrorMessages.ForeignKeyViolation.ToString();
                    break;

                case 2627:
                    // Unique Index/Constriant Violation
                    error = ErrorMessages.UniqueIndex.ToString();
                    break;
                case 2601:
                    // Unique Index/Constriant Violation
                    error = ErrorMessages.UniqueIndex.ToString();
                    break;
                default:
                    // throw a general Exception                   
                    break;
            }

            return error;
        }

        public static string GetCommomMessage(CommonMessageEnum type)
        {
            switch (type)
            {
                case CommonMessageEnum.DeleteFailed:
                    return ErrorMessages.DeleteFailed;

                case CommonMessageEnum.DeleteSuccessful:
                    return ErrorMessages.DeleteSuccessful;

                case CommonMessageEnum.UpdateFailed:
                    return ErrorMessages.UpdateFailed;

                case CommonMessageEnum.UpdateSuccessful:
                    return ErrorMessages.UpdateSuccessful;


                case CommonMessageEnum.InsertFailed:
                    return ErrorMessages.InsertFailed;

                case CommonMessageEnum.InsertSuccessful:
                    return ErrorMessages.InsertSuccessful;
                case CommonMessageEnum.DataNotFound:
                    return ErrorMessages.DataNotFound;
                case CommonMessageEnum.MandatoryInputFailed:
                    return ErrorMessages.MandatoryInputFailed;

                default:
                    return string.Empty;

            }
        }

        public static string GetModelStateErrorMessage(ModelStateDictionary modelStateDictionary)
        {
            string message = @"<div class='content'>";

            foreach (var modelStateValues in modelStateDictionary.Values)
            {
                if (modelStateValues.Errors.Any())
                {
                    foreach (var modelError in modelStateValues.Errors)
                    {
                        message += @"<span class='clearfix alert'>";
                        message += modelError.ErrorMessage;
                        message += "</span>";
                    }
                }
            }

            message += "</div>";

            return message;
        }

        public static string GetModelStateError(ModelStateDictionary ModelState)
        {
            var errors = ModelState
                            .Where(x => x.Value.Errors.Count > 0)
                //.Select(x => x.Value.Errors.FirstOrDefault().ErrorMessage);
                            .Select(x => new { x.Key, x.Value.Errors })
                            .ToArray();

            return "";// errors.FirstOrDefault();
        }

        public static string GetErrorMessage(string ErrorMessage)
        {
            string message = @"<div class='content'>";
            message += @"<span class='clearfix'>";
            message += ErrorMessage;
            message += "</span>";
            message += "</div>";

            return message;
        }

        public static string ValidationSummaryHead
        {
            get
            {
                return "Please fill up the red marked field(s)";
            }
        }

        public static string GetExceptionMessage(Exception ex)
        {
            string message = "Error: There was a problem while processing your request: " + ex.Message;

            try
            {
                if (ex.InnerException != null)
                {
                    Exception inner = ex.InnerException;
                    if (inner is System.Data.Common.DbException)
                        message = "Database is currently experiencing problems. " + inner.Message;
                    else if (inner is UpdateException)
                        message = "Datebase is currently updating problem.";
                    else if (inner is EntityException)
                        message = "Entity is problem.";
                    else if (inner is NullReferenceException)
                        message = "There are one or more required fields that are missing.";
                    else if (inner is ArgumentException)
                    {
                        string paramName = ((ArgumentException)inner).ParamName;
                        message = string.Concat("The ", paramName, " value is illegal.");
                    }
                    else if (inner is ApplicationException)
                    {
                        message = "Exception in application" + inner.Message;
                    }
                    else if (inner is SqlException)
                    {
                        SqlException sqlException = ex.InnerException as SqlException;
                        message = Common.GetSqlExceptionMessage(sqlException.Number);
                    }
                    else
                        message = inner.Message;

                }
            }
            catch (Exception excep)
            {
                message = excep.Message;
            }

            return message;
        }

        public static string GetExceptionMessage(Exception ex, CommonAction actionName)
        {
            string message = "Error: There was a problem while processing your request: " + ex.Message;

            try
            {
                if (ex.InnerException != null)
                {
                    Exception inner = ex.InnerException;

                    if (inner is SqlException)
                    {
                        SqlException sqlException = ex.InnerException as SqlException;
                        message = Common.GetSqlExceptionMessage(sqlException.Number);
                    }
                    else if (inner is System.Data.Common.DbException)
                        message = "Database is currently experiencing problems. " + inner.Message;
                    else if (inner is UpdateException)
                        message = "Datebase is currently updating problem.";
                    else if (inner is EntityException)
                        message = "Entity is problem.";
                    else if (inner is NullReferenceException)
                        message = "There are one or more required fields that are missing.";
                    else if (inner is ArgumentException)
                    {
                        string paramName = ((ArgumentException)inner).ParamName;
                        message = string.Concat("The ", paramName, " value is illegal.");
                    }
                    else if (inner is ApplicationException)
                    {
                        message = "Exception in application" + inner.Message;
                    }
                    else
                        message = inner.Message;
                }
            }
            catch (Exception excep)
            {
                message = excep.Message;
            }

            return message;
        }

        public enum CommonAction
        {
            Save, Update, Delete
        }

        public static string GetModelStateErrorMessage(ModelStateDictionary modelStateDictionary, bool isFormated)
        {
            string message = "";
            if (isFormated)
            {
                message = @"<div class='content'>";

                foreach (var modelStateValues in modelStateDictionary.Values)
                {
                    if (modelStateValues.Errors.Any())
                    {
                        foreach (var modelError in modelStateValues.Errors)
                        {
                            message += @"<span class='clearfix'>";
                            message += modelError.ErrorMessage;
                            message += "</span>";
                        }
                    }
                }

                message += "</div>";
            }
            else
            {
                var errors = modelStateDictionary.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                message = errors.Any() ? errors.First().Errors.First().ErrorMessage : "";
                //return new JsonResult() { Data = errors.Count() > 0 ? errors.First().Errors.First().ErrorMessage : ""};
            }

            return message;
        }
        #endregion
    }
}