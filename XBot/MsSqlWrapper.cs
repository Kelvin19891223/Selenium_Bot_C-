using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Configuration;

namespace DentalDoc
{
    class MsSqlWrapper
    {
        public SqlConnection sql_con;
        public SqlCommand sql_cmd;
        private System.Object locker = new System.Object();

        public MsSqlWrapper() { }

        public bool ExecuteNonQuery(string txtQuery)
        {
            try
            {
                lock (locker)
                {
                    CloseConnection();
                    //OutLog("\t\t#CENTRAL# Execute MsSQL NonQuery: " + txtQuery);
                    OpenConnection();
                    sql_cmd = sql_con.CreateCommand();
                    sql_cmd.CommandText = txtQuery;
                    sql_cmd.ExecuteNonQuery();
                    CloseConnection();
                    return true;
                }
            }
            catch (Exception ex)
            {
                OutLog("Database query failed : " + ex.Message);
                return false;
            }
        }

        public DataTable ExecuteQuery(string txtQuery)
        {
            try
            {
                lock (locker)
                {
                    CloseConnection();
                    OpenConnection();
                    DataTable dt = new DataTable();
                    sql_cmd = sql_con.CreateCommand();
                    SqlDataAdapter DB = new SqlDataAdapter(txtQuery, sql_con);
                    DB.SelectCommand.CommandType = CommandType.Text;
                    DB.Fill(dt);
                    CloseConnection();

                    //OutLog("\t\t#CENTRAL# Execute MsSql Query: " + txtQuery + " -> " + dt.Rows.Count.ToString());
                    return dt;
                }
            }
            catch (Exception ex)
            {
                OutLog("Database query failed : " + ex.Message);
                return null;
            }
        }

        public void CreateConnection()
        {
            //String constr = "data source=DESKTOP-6B5KCLM\\SQLEXPRESS;initial catalog=Local_DB;Trusted_Connection=True;persist security info=False;packet size=4096;pooling=True;Max Pool Size=1000";
            String constr = "data source=VULTR-GUEST;initial catalog=Local_DB;Trusted_Connection=True;persist security info=False;packet size=4096;pooling=True;Max Pool Size=1000";
            
            sql_con = new SqlConnection(constr.ToString());
        }

        public static int GetExceptionNumber(SqlException my)
        {
            if (my != null)
            {
                int number = my.Number;
                //if the number is zero, try to get the number of the inner exception
                if (number == 0 && (my = my.InnerException as SqlException) != null)
                {
                    number = my.Number;
                }
                return number;
            }
            return -1;
        }

        public bool OpenConnection()
        {
            try
            {
                sql_con.Open();
                return true;
            }
            catch (SqlException ex)
            {
                //OutLog(sql_con.ConnectionString);
                switch (GetExceptionNumber(ex))
                {
                    case 0:
                        OutLog("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        OutLog("Invalid username/password, please try again");
                        break;
                }
                OutLog(ex.Message);
                return false;
            }
        }
        public bool CloseConnection()
        {
            try
            {
                sql_con.Close();
                return true;
            }
            catch (SqlException ex)
            {
                OutLog(ex.Message);
                return false;
            }
        }

        public bool is_connected()
        {
            lock (locker)
            {
                CloseConnection();
                bool ret = OpenConnection();
                if (ret)
                    CloseConnection();
                return ret;
            }
        }

        public void OutLog(string str)
        {            
            using (StreamWriter writer = new StreamWriter("mssql_log.txt", true))
            {
                writer.WriteLine(str);
            }
        }

        public void saveTeam(String sport_id, String sport_name, String kr_name, String en_name, String rl_status)
        {
            try
            {
                lock (locker)
                {
                    CloseConnection();
                    OpenConnection();

                    string querys = "insert into ref_team(Rt_sports_id,Rt_sports_name,Rt_status,Rt_kr_name1,Rt_kr_name2,Rt_en_name) values(@Rt_sports_id,@Rt_sports_name,@Rt_status,@Rt_kr_name1,@Rt_kr_name2,@Rt_en_name)";

                    //sql_cmd = sql_con.CreateCommand();
                    sql_cmd = new SqlCommand(querys, sql_cmd.Connection);
                    sql_cmd.Parameters.Add("@Rt_sports_id", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@Rt_sports_name", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@Rt_status", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@Rt_kr_name1", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@Rt_kr_name2", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@Rt_en_name", SqlDbType.VarChar, 255);                    

                    sql_cmd.Parameters["@Rt_sports_id"].Value = sport_id;
                    sql_cmd.Parameters["@Rt_sports_name"].Value = sport_name;
                    sql_cmd.Parameters["@Rt_status"].Value = rl_status;
                    sql_cmd.Parameters["@Rt_kr_name1"].Value = kr_name;
                    sql_cmd.Parameters["@Rt_kr_name2"].Value = kr_name;
                    sql_cmd.Parameters["@Rt_en_name"].Value = en_name;
                    
                    int result = sql_cmd.ExecuteNonQuery();
                    CloseConnection();
                }
            }
            catch (Exception ex)
            {
                OutLog("Database query failed : " + ex.Message);                
            }
        }

        public void updateTeam(String sport_name, String kr_name, String en_name)
        {
            try
            {
                lock (locker)
                {
                    CloseConnection();
                    OpenConnection();

                    string querys = "update ref_team set Rt_kr_name1=@Rt_kr_name1, Rt_kr_name2=@Rt_kr_name2, Rt_status=1 where Rt_en_name = @Rt_en_name and Rt_sports_name=@Rt_sports_name";

                    //sql_cmd = sql_con.CreateCommand();
                    sql_cmd = new SqlCommand(querys, sql_cmd.Connection);                    
                    sql_cmd.Parameters.Add("@Rt_sports_name", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@Rt_kr_name1", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@Rt_kr_name2", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@Rt_en_name", SqlDbType.VarChar, 255);

                    
                    sql_cmd.Parameters["@Rt_sports_name"].Value = sport_name;                    
                    sql_cmd.Parameters["@Rt_kr_name1"].Value = kr_name;
                    sql_cmd.Parameters["@Rt_kr_name2"].Value = kr_name;
                    sql_cmd.Parameters["@Rt_en_name"].Value = en_name;

                    int result = sql_cmd.ExecuteNonQuery();
                    CloseConnection();
                }
            }
            catch (Exception ex)
            {
                OutLog("Database query failed : " + ex.Message);
            }
        }

        public void saveLeague(String sports, String league_num, String league, String leagueFull, String status, String country_en, String country_kr)
        {
            try
            {
                lock (locker)
                {
                    CloseConnection();
                    OpenConnection();

                    string querys = "insert into ref_league(rl_sports,rl_league_num,rl_league,rl_league_full,rl_status,rl_Image,rl_Country) values(@rl_sports,@rl_league_num,@rl_league,@rl_league_full,@rl_status,@rl_Image,@rl_Country)";

                    //sql_cmd = sql_con.CreateCommand();
                    sql_cmd = new SqlCommand(querys, sql_cmd.Connection);
                    sql_cmd.Parameters.Add("@rl_sports", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@rl_league_num", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@rl_league", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@rl_league_full", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@rl_status", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@rl_Image", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@rl_Country", SqlDbType.VarChar, 255);

                    sql_cmd.Parameters["@rl_sports"].Value = sports;
                    sql_cmd.Parameters["@rl_league_num"].Value = league_num;
                    sql_cmd.Parameters["@rl_league"].Value = league;
                    sql_cmd.Parameters["@rl_league_full"].Value = leagueFull;
                    sql_cmd.Parameters["@rl_status"].Value = status;
                    sql_cmd.Parameters["@rl_Image"].Value = country_en;
                    sql_cmd.Parameters["@rl_Country"].Value = country_kr;

                    int result = sql_cmd.ExecuteNonQuery();
                    CloseConnection();
                }
            }
            catch (Exception ex)
            {
                OutLog("Database query failed : " + ex.Message);
            }
        }

        public void updateLeague(String sports, String league, String leagueFull)
        {
            try
            {
                lock (locker)
                {
                    CloseConnection();
                    OpenConnection();

                    string querys = "update ref_league set rl_league = @rl_league, rl_status='1' where rl_sports = @rl_sports and rl_league_full = @rl_league_full";

                    //sql_cmd = sql_con.CreateCommand();
                    sql_cmd = new SqlCommand(querys, sql_cmd.Connection);
                    sql_cmd.Parameters.Add("@rl_sports", SqlDbType.VarChar, 255);                    
                    sql_cmd.Parameters.Add("@rl_league", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@rl_league_full", SqlDbType.VarChar, 255);                    

                    sql_cmd.Parameters["@rl_sports"].Value = sports;                    
                    sql_cmd.Parameters["@rl_league"].Value = league;
                    sql_cmd.Parameters["@rl_league_full"].Value = leagueFull;                    

                    int result = sql_cmd.ExecuteNonQuery();
                    CloseConnection();
                }
            }
            catch (Exception ex)
            {
                OutLog("Database query failed : " + ex.Message);
            }
        }

        public void saveData(String event_id, String away, String home, String league, String time, String sport_id, String scrap_date)
        {
            try
            {
                lock (locker)
                {
                    CloseConnection();
                    OpenConnection();

                    String query = "insert into ref_upcoming(event_id, away, home, league, time, sport_id, flag, scrap_date) values(@event_id, @away, @home, @league, @time, @sport_id, @flag,@scrap_date)";

                    //sql_cmd = sql_con.CreateCommand();
                    sql_cmd = new SqlCommand(query, sql_cmd.Connection);
                    sql_cmd.Parameters.Add("@event_id", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@away", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@home", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@league", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@time", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@sport_id", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@flag", SqlDbType.VarChar, 255);
                    sql_cmd.Parameters.Add("@scrap_date", SqlDbType.VarChar, 255);

                    sql_cmd.Parameters["@event_id"].Value = event_id;
                    sql_cmd.Parameters["@away"].Value = away;
                    sql_cmd.Parameters["@home"].Value = home;
                    sql_cmd.Parameters["@league"].Value = league;
                    sql_cmd.Parameters["@time"].Value = time;
                    sql_cmd.Parameters["@sport_id"].Value = sport_id;
                    sql_cmd.Parameters["@flag"].Value = '0';
                    sql_cmd.Parameters["@scrap_date"].Value = scrap_date;

                    int result = sql_cmd.ExecuteNonQuery();
                    CloseConnection();
                }
            }
            catch (Exception ex)
            {
                OutLog("Database query failed : " + ex.Message);
            }
        }

        public String getCountryName(String code)
        {
            String result = "";
            DataTable dt = this.ExecuteQuery(String.Format("select * from ref_country where it='{0}'", code));
            if (dt != null && dt.Rows.Count != 0)
                result = dt.Rows[0]["country_en"].ToString() + ".png";
            return result;
        }        

        public String getSite()
        {
            String result = "";
            DataTable dt = this.ExecuteQuery(String.Format("select * from Set_Site"));
            if (dt != null && dt.Rows.Count != 0)
                result = dt.Rows[0]["Site"].ToString();
            return result;
        }

        public void saveInfoGame(String IG_root, String IG_outcome_id, String RL_Idx, String RL_Sports, String RL_League, String RL_Image, String IG_StartTime, String IG_Team1, String IG_Team2, String IG_Handicap, String IG_Team1Benefit, String IG_DrawBenefit, String IG_Team2Benefit, String IG_Status, String IG_Type, String IG_SITE, String IG_SP, String IG_period_name, String IG_pv_type, String IG_Game_Type, String IG_UpdateTime, String IG_OrderNo, String IG_Finish)
        {
            try
            {
                lock (locker)
                {
                    CloseConnection();
                    OpenConnection();

                    string querys = "insert into Info_Game(IG_root,IG_outcome_id,RL_Idx,RL_Sports,RL_League,RL_Image,IG_StartTime,IG_Team1,IG_Team2,IG_Handicap,IG_Team1Benefit,IG_DrawBenefit,IG_Team2Benefit,IG_Status,IG_Type,IG_SITE,IG_SP,IG_period_name,IG_pv_type,IG_Game_Type,IG_UpdateTime,IG_OrderNo,IG_Finish) values(@IG_root,@IG_outcome_id,@RL_Idx,@RL_Sports,@RL_League,@RL_Image,@IG_StartTime,@IG_Team1,@IG_Team2,@IG_Handicap,@IG_Team1Benefit,@IG_DrawBenefit,@IG_Team2Benefit,@IG_Status,@IG_Type,@IG_SITE,@IG_SP,@IG_period_name,@IG_pv_type,@IG_Game_Type,@IG_UpdateTime,@IG_OrderNo,@IG_Finish)";

                    //sql_cmd = sql_con.CreateCommand();
                    sql_cmd = new SqlCommand(querys, sql_cmd.Connection);
                    sql_cmd.Parameters.Add("@IG_root", SqlDbType.Int);
                    sql_cmd.Parameters.Add("@IG_outcome_id", SqlDbType.Int);
                    sql_cmd.Parameters.Add("@RL_Idx", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@RL_Sports", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@RL_League", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@RL_Image", SqlDbType.NVarChar, 50);
                    sql_cmd.Parameters.Add("@IG_StartTime", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Team1", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Team2", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Handicap", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Team1Benefit", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_DrawBenefit", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Team2Benefit", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Status", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Type", SqlDbType.TinyInt);
                    sql_cmd.Parameters.Add("@IG_SITE", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_SP", SqlDbType.NVarChar, 50);
                    sql_cmd.Parameters.Add("@IG_period_name", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_pv_type", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Game_Type", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_UpdateTime", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_OrderNo", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Finish", SqlDbType.NVarChar, 255);

                    sql_cmd.Parameters["@IG_root"].Value = Convert.ToInt32(IG_root);
                    sql_cmd.Parameters["@IG_outcome_id"].Value = Convert.ToInt32(IG_outcome_id);
                    sql_cmd.Parameters["@RL_Idx"].Value = RL_Idx;
                    sql_cmd.Parameters["@RL_Sports"].Value = RL_Sports;
                    sql_cmd.Parameters["@RL_League"].Value = RL_League;
                    sql_cmd.Parameters["@RL_Image"].Value = RL_Image;
                    sql_cmd.Parameters["@IG_StartTime"].Value = IG_StartTime;
                    sql_cmd.Parameters["@IG_Team1"].Value = IG_Team1;
                    sql_cmd.Parameters["@IG_Team2"].Value = IG_Team2;
                    sql_cmd.Parameters["@IG_Handicap"].Value = IG_Handicap;
                    sql_cmd.Parameters["@IG_Team1Benefit"].Value = IG_Team1Benefit;
                    sql_cmd.Parameters["@IG_DrawBenefit"].Value = IG_DrawBenefit;
                    sql_cmd.Parameters["@IG_Team2Benefit"].Value = IG_Team2Benefit;
                    sql_cmd.Parameters["@IG_Status"].Value = IG_Status;
                    sql_cmd.Parameters["@IG_Type"].Value = Convert.ToInt32(IG_Type);
                    sql_cmd.Parameters["@IG_SITE"].Value = IG_SITE;
                    sql_cmd.Parameters["@IG_SP"].Value = IG_SP;
                    sql_cmd.Parameters["@IG_period_name"].Value = IG_period_name;
                    sql_cmd.Parameters["@IG_pv_type"].Value = IG_pv_type;
                    sql_cmd.Parameters["@IG_Game_Type"].Value = IG_Game_Type;
                    sql_cmd.Parameters["@IG_UpdateTime"].Value = IG_UpdateTime;
                    sql_cmd.Parameters["@IG_OrderNo"].Value = IG_OrderNo;
                    sql_cmd.Parameters["@IG_Finish"].Value = IG_Finish;


                    int result = sql_cmd.ExecuteNonQuery();
                    CloseConnection();
                }
            }
            catch (Exception ex)
            {
                OutLog("Database query failed : " + ex.Message);
            }
        }

        public void updateInfoGame(String IG_root, String IG_outcome_id, String RL_Idx, String RL_Sports, String RL_League, String RL_Image, String IG_StartTime, String IG_Team1, String IG_Team2, String IG_Handicap, String IG_Team1Benefit, String IG_DrawBenefit, String IG_Team2Benefit, String IG_Status, String IG_Type, String IG_SITE, String IG_SP, String IG_period_name, String IG_pv_type, String IG_Game_Type, String IG_UpdateTime, String IG_OrderNo, String IG_Finish)
        {
            try
            {
                lock (locker)
                {
                    CloseConnection();
                    OpenConnection();

                    string querys = "update Info_Game set IG_Handicap=@IG_Handicap,IG_Team1Benefit=@IG_Team1Benefit,IG_DrawBenefit=@IG_DrawBenefit,IG_Team2Benefit=@IG_Team2Benefit,IG_UpdateTime=@IG_UpdateTime where" +
                        " IG_root = @IG_root and IG_outcome_id=@IG_outcome_id and RL_Idx=@RL_Idx and RL_League=@RL_League " +
                        " and IG_StartTime=@IG_StartTime and IG_Team1=@IG_Team1 and IG_Team2=@IG_Team1 and IG_pv_type=@IG_pv_type and IG_Game_Type=@IG_Game_Type";

                    //sql_cmd = sql_con.CreateCommand();
                    sql_cmd = new SqlCommand(querys, sql_cmd.Connection);
                    sql_cmd.Parameters.Add("@IG_root", SqlDbType.Int);
                    sql_cmd.Parameters.Add("@IG_outcome_id", SqlDbType.Int);
                    sql_cmd.Parameters.Add("@RL_Idx", SqlDbType.NVarChar, 255);                    
                    sql_cmd.Parameters.Add("@RL_League", SqlDbType.NVarChar, 255);                    
                    sql_cmd.Parameters.Add("@IG_StartTime", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Team1", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Team2", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Handicap", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Team1Benefit", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_DrawBenefit", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Team2Benefit", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_pv_type", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_Game_Type", SqlDbType.NVarChar, 255);
                    sql_cmd.Parameters.Add("@IG_UpdateTime", SqlDbType.NVarChar, 255);                    

                    sql_cmd.Parameters["@IG_root"].Value = Convert.ToInt32(IG_root);
                    sql_cmd.Parameters["@IG_outcome_id"].Value = Convert.ToInt32(IG_outcome_id);
                    sql_cmd.Parameters["@RL_Idx"].Value = RL_Idx;                    
                    sql_cmd.Parameters["@RL_League"].Value = RL_League;                    
                    sql_cmd.Parameters["@IG_StartTime"].Value = IG_StartTime;
                    sql_cmd.Parameters["@IG_Team1"].Value = IG_Team1;
                    sql_cmd.Parameters["@IG_Team2"].Value = IG_Team2;
                    sql_cmd.Parameters["@IG_Handicap"].Value = IG_Handicap;
                    sql_cmd.Parameters["@IG_Team1Benefit"].Value = IG_Team1Benefit;
                    sql_cmd.Parameters["@IG_DrawBenefit"].Value = IG_DrawBenefit;
                    sql_cmd.Parameters["@IG_Team2Benefit"].Value = IG_Team2Benefit;                    
                    sql_cmd.Parameters["@IG_pv_type"].Value = IG_pv_type;
                    sql_cmd.Parameters["@IG_Game_Type"].Value = IG_Game_Type;
                    sql_cmd.Parameters["@IG_UpdateTime"].Value = IG_UpdateTime;                    
                    
                    int result = sql_cmd.ExecuteNonQuery();
                    CloseConnection();
                }
            }
            catch (Exception ex)
            {
                OutLog("Database query failed : " + ex.Message);
            }
        }
    }
}
