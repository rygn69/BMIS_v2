using iFMIS_BMS.BusinessLayer.Connector;
using iFMIS_BMS.BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace iFMIS_BMS.BusinessLayer.Layers
{
    public class dp_PorposalYearLayer
    {

        public IEnumerable<dp_PorposalYear_Model> ProposalYears()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select distinct  AccountYear from  tbl_R_BMSProgramAccounts where AccountYear > 2016 and actioncode=1 order by AccountYear desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }

        public IEnumerable<dp_PorposalYear_Model> ProposalYears2()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT distinct [ProposalYear] FROM [IFMIS].[dbo].[tbl_T_BMSBudgetProposal] where ProposalYear > 2016 and [ProposalActionCode]=1  order by [ProposalYear] desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<dp_PorposalYear_Model> ProposalYears3()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select  trnYear from  [IFMIS].[dbo].[tbl_R_BMSTransYear] order by [trnYear] desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<dp_PorposalYear_Model> ProposalYears4()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select  trnYear from  [IFMIS].[dbo].[tbl_R_BMSTransYear_Release] WHERE [tyearend]=1 order by [trnYear] desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }

        public IEnumerable<dp_PorposalYear_Model> WFPYears()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select  trnYear from  [IFMIS].[dbo].[tbl_R_BMSWFPYear] WHERE [tyearend]=1 order by [trnYear] desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<dp_PorposalYear_Model> UtilizationYear()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select  trnYear from  [IFMIS].[dbo].[tbl_R_BMSTransYear_Release] order by [trnYear] desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<dp_PorposalYear_Model> PPAYear()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT [trnYear] FROM [IFMIS].[dbo].[tbl_R_BMSTransYear]order by trnyear desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<dp_PorposalYear_Model> TRyear()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select distinct  AccountYear from  tbl_R_BMSProgramAccounts where actioncode=1  order by AccountYear desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }

        public IEnumerable<dp_PorposalYear_Model> Tyear_ict()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select distinct  AccountYear from  ifmis.dbo.tbl_R_BMSProgramAccounts where actioncode=1 and AccountYear> 2022  order by AccountYear desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<dp_PorposalYear_Model> boi_yearss()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("SELECT [trnYear] FROM [IFMIS].[dbo].[tbl_R_BMSTransYear] order by [trnYear] desc", con);//new SqlCommand("select distinct  ProposalYear from  tbl_T_BMSBudgetProposal order by ProposalYear desc", con);
                con.Open();

                SqlDataReader reader = com.ExecuteReader();

                dp_PorposalYear_Model aw = new dp_PorposalYear_Model();

                aw.ProposalYear_ = "----";

                proposalyear.Add(aw);
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear_ = reader.GetValue(0) is DBNull ? "" : reader.GetValue(0).ToString();

                    proposalyear.Add(app);



                }
               
            }
            return proposalyear;
        }
        public IEnumerable<dp_PorposalYear_Model> GetProposalYear()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select [ProposalYear] from [IFMIS].[dbo].[tbl_R_BMSProposalYear] order by [ProposalYear] desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<dp_PorposalYear_Model> MAFYear()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select  trnYear from  [IFMIS].[dbo].[tbl_R_BMSTransYear] where [tyearend]=1 order by [trnYear] desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<dp_PorposalYear_Model> GetPropYearFund()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select  trnYear from  [IFMIS].[dbo].[tbl_R_BMSTransYear] where trnYear > 2020 order by [trnYear] desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
        public IEnumerable<dp_PorposalYear_Model> GetWFPYear()
        {
            List<dp_PorposalYear_Model> proposalyear = new List<dp_PorposalYear_Model>();
            using (SqlConnection con = new SqlConnection(Common.MyConn()))
            {
                SqlCommand com = new SqlCommand("select [ProposalYear] from [IFMIS].[dbo].[tbl_R_BMSProposalYear] where ProposalYear >= 2022  order by [ProposalYear] desc", con);
                con.Open();
                SqlDataReader reader = com.ExecuteReader();
                while (reader.Read())
                {
                    dp_PorposalYear_Model app = new dp_PorposalYear_Model();

                    app.ProposalYear = reader.GetInt32(0);

                    proposalyear.Add(app);
                }
            }
            return proposalyear;
        }
    }
}