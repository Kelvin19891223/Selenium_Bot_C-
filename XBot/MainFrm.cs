using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Net;
using Newtonsoft.Json.Linq;
using GPwdBot;

namespace DentalDoc
{
    public partial class MainFrm : Form
    {
        public Hashtable sport = new Hashtable();
        public Hashtable sportId = new Hashtable();
        public String token = "27122-QwOqeoNmOkwt9X";
        public MainFrm()
        {
            InitializeComponent();
            InitSetting();
            //refreshGrid();
        }

        public void InitSetting()
        {
            sport.Add(91, "Volleyball");
            sport.Add(17, "Ice Hockey");
            sport.Add(12, "American Football");
            sport.Add(1, "Soccer");
            sport.Add(18, "Basketball");
            sport.Add(13, "Tennis");
            sport.Add(16, "Baseball");

            sportId.Add("Volleyball", 9);
            sportId.Add("Ice Hockey", 6);
            sportId.Add("American Football", 10);
            sportId.Add("Soccer", 7);
            sportId.Add("Basketball", 2);
            sportId.Add("Tennis", 8);
            sportId.Add("Baseball",1);
        }

        public void refreshGrid()
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                DataTable dt = MainApp.mSql.ExecuteQuery("select * from upcoming where flag='0' order by scrap_date, sport_id, time");
                grid_main.Rows.Clear();

                if (dt != null)
                {
                    try
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                            dtDateTime = dtDateTime.AddSeconds(Convert.ToInt32(dt.Rows[i]["time"]));
                            grid_main.Rows.Add(i + 1, sport[Convert.ToInt32(dt.Rows[i]["sport_id"])], dt.Rows[i]["event_id"], dtDateTime.ToString("yyyy/MM/dd"), dt.Rows[i]["away"], dt.Rows[i]["home"], dt.Rows[i]["league"]);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
        }

        public void ThreadProc()
        {
            while (true)
            {
                getUpcoming();                
            }
        }

        public void ThreadProc1()
        {
            while(true)
            {
                //getResult();
            }
        }

        public void getUpcoming()
        {
            //MainApp.mSql.ExecuteNonQuery(String.Format("delete from upcoming where scrap_date='{0}' and flag=1", DateTime.Today.AddDays(-2).ToString("yyyyMMdd")));
            
            foreach (DictionaryEntry item in sport)
            {
                var sportKey = item.Key;
                String sportName = item.Value as String;
                String day = DateTime.UtcNow.ToString("yyyyMMdd");
                String url = String.Format("https://api.betsapi.com/v1/sbobet/upcoming?sport_id={0}&token={1}&day={2}", sportKey, token, day);
                //String url = String.Format("https://api.betsapi.com/v1/sbobet/upcoming?sport_id={0}&token={1}", sportKey, token);
                MainApp.log_info(String.Format("Start to get the {0}", sportName));
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    int tpage = 0;

                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                            String responseJSON = reader.ReadToEnd();
                            JObject applyJObj = JObject.Parse(responseJSON);
                            tpage = Convert.ToInt32(Math.Ceiling(Double.Parse(applyJObj["pager"]["total"].ToString()) / 50));

                            //Get Data
                            for (int i = 1; i <= tpage; i++)
                            {
                                url = String.Format("https://api.betsapi.com/v1/sbobet/upcoming?sport_id={0}&token={1}&page={2}&day={3}", sportKey, token, i, day);
                                //url = String.Format("https://api.betsapi.com/v1/sbobet/upcoming?sport_id={0}&token={1}&page={2}", sportKey, token, i);
                                MainApp.log_info(String.Format("Start to get the {0} on page {1}", sportName, i));
                                HttpWebRequest requests = (HttpWebRequest)WebRequest.Create(url);
                                try
                                {
                                    using (WebResponse res = requests.GetResponse())
                                    {
                                        using (Stream resStream = res.GetResponseStream())
                                        {
                                            using (StreamReader redr = new StreamReader(resStream, System.Text.Encoding.UTF8))
                                            {
                                                try
                                                {
                                                    responseJSON = redr.ReadToEnd();
                                                    applyJObj = JObject.Parse(responseJSON);

                                                    JArray array = JArray.Parse(applyJObj["results"].ToString());                                                    
                                                    foreach (JObject results in array)
                                                    {
                                                        try
                                                        {
                                                            MainApp.log_info(String.Format("Start to get the {0}", results["id"].ToString()));                                                            
                                                            String event_id = results["id"].ToString();
                                                            String away = results["away"]["name"].ToString();
                                                            String home = results["home"]["name"].ToString();
                                                            String league = results["league"]["name"].ToString();
                                                            String time = results["time"].ToString();
                                                            String sportId = results["sport_id"].ToString();

                                                            //MainApp.mSql.saveData(event_id, away, home, league, time, sportId, day);

                                                            System.DateTime dtDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                                            dtDate = dtDate.AddSeconds(Convert.ToInt32(results["time"].ToString()));

                                                            System.DateTime now = DateTime.UtcNow;
                                                            if (DateTime.Compare(dtDate, now) > 0)
                                                                setTeamLeague(sportName, results);
                                                        }
                                                        catch(Exception ex)
                                                        {
                                                            MainApp.log_info(String.Format("Error to get the {0}", results["id"].ToString()));
                                                        }                                                        
                                                    }

                                                    applyJObj = null;
                                                    redr.Close();
                                                    //redr.Dispose();
                                                }
                                                catch (Exception ex)
                                                {
                                                    MainApp.log_info(String.Format("Error catch the {0} on page {1}.", sportName, i));
                                                }                                                
                                            }
                                        }

                                        res.Close();
                                    }

                                    //GC.Collect();
                                }
                                catch (Exception ex)
                                {
                                    MainApp.log_info(String.Format("Error catch the {0} on page {1}", sportName, i));
                                }
                            }
                            applyJObj = null;
                            reader.Close();
                            reader.Dispose();
                        }
                    }

                }
                catch (WebException ex)
                {
                    MainApp.log_info(String.Format("Error catch the {0}", sportName));
                }
            }


            foreach (DictionaryEntry item in sport)
            {
                var sportKey = item.Key;
                String sportName = item.Value as String;
                String day = DateTime.UtcNow.AddDays(1).ToString("yyyyMMdd");
                String url = String.Format("https://api.betsapi.com/v1/sbobet/upcoming?sport_id={0}&token={1}&day={2}", sportKey, token, day);
                //String url = String.Format("https://api.betsapi.com/v1/sbobet/upcoming?sport_id={0}&token={1}", sportKey, token);
                MainApp.log_info(String.Format("Start to get the {0}", sportName));
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    int tpage = 0;

                    using (WebResponse response = request.GetResponse())
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                            String responseJSON = reader.ReadToEnd();
                            JObject applyJObj = JObject.Parse(responseJSON);
                            tpage = Convert.ToInt32(Math.Ceiling(Double.Parse(applyJObj["pager"]["total"].ToString()) / 50));

                            //Get Data
                            for (int i = 1; i <= tpage; i++)
                            {
                                url = String.Format("https://api.betsapi.com/v1/sbobet/upcoming?sport_id={0}&token={1}&page={2}&day={3}", sportKey, token, i, day);
                                //url = String.Format("https://api.betsapi.com/v1/sbobet/upcoming?sport_id={0}&token={1}&page={2}", sportKey, token, i);
                                MainApp.log_info(String.Format("Start to get the {0} on page {1}", sportName, i));
                                HttpWebRequest requests = (HttpWebRequest)WebRequest.Create(url);
                                try
                                {
                                    using (WebResponse res = requests.GetResponse())
                                    {
                                        using (Stream resStream = res.GetResponseStream())
                                        {
                                            using (StreamReader redr = new StreamReader(resStream, System.Text.Encoding.UTF8))
                                            {
                                                try
                                                {
                                                    responseJSON = redr.ReadToEnd();
                                                    applyJObj = JObject.Parse(responseJSON);

                                                    JArray array = JArray.Parse(applyJObj["results"].ToString());
                                                    foreach (JObject results in array)
                                                    {
                                                        try
                                                        {
                                                            MainApp.log_info(String.Format("Start to get the {0}", results["id"].ToString()));
                                                            String event_id = results["id"].ToString();
                                                            String away = results["away"]["name"].ToString();
                                                            String home = results["home"]["name"].ToString();
                                                            String league = results["league"]["name"].ToString();
                                                            String time = results["time"].ToString();
                                                            String sportId = results["sport_id"].ToString();

                                                            //MainApp.mSql.saveData(event_id, away, home, league, time, sportId, day);

                                                            System.DateTime dtDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                                            dtDate = dtDate.AddSeconds(Convert.ToInt32(results["time"].ToString()));

                                                            System.DateTime now = DateTime.UtcNow;
                                                            if (DateTime.Compare(dtDate, now) > 0)
                                                                setTeamLeague(sportName, results);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            MainApp.log_info(String.Format("Error to get the {0}", results["id"].ToString()));
                                                        }
                                                    }

                                                    applyJObj = null;
                                                    redr.Close();
                                                    //redr.Dispose();
                                                }
                                                catch (Exception ex)
                                                {
                                                    MainApp.log_info(String.Format("Error catch the {0} on page {1}.", sportName, i));
                                                }
                                            }
                                        }

                                        res.Close();
                                    }

                                    //GC.Collect();
                                }
                                catch (Exception ex)
                                {
                                    MainApp.log_info(String.Format("Error catch the {0} on page {1}", sportName, i));
                                }
                            }
                            applyJObj = null;
                            reader.Close();
                            reader.Dispose();
                        }
                    }

                }
                catch (WebException ex)
                {
                    MainApp.log_info(String.Format("Error catch the {0}", sportName));
                }
            }
        }

        public void getResult()
        {
            DataTable dt = MainApp.mSql.ExecuteQuery("SELECT * FROM [Local_DB].[dbo].[Info_Game] where IG_StartTime >= GETDATE()");
            if(dt != null && dt.Rows.Count != 0)
            {
                for(int i=0; i<dt.Rows.Count; i++)
                {

                }
            }
        }

        public void setTeamLeague(String sportName, JObject results)
        {
            String url = String.Format("https://api.betsapi.com/v1/sbobet/result?token={0}&event_id={1}&LNG_ID=71", token, results["id"].ToString());
            try
            {
                HttpWebRequest requests = (HttpWebRequest)WebRequest.Create(url);
                using (WebResponse res = requests.GetResponse())
                {
                    using (Stream resStream = res.GetResponseStream())
                    {
                        using (StreamReader redr = new StreamReader(resStream, System.Text.Encoding.UTF8))
                        {
                            String responseJSON = redr.ReadToEnd();
                            JObject applyJObj = JObject.Parse(responseJSON);

                            JArray array = JArray.Parse(applyJObj["results"].ToString());
                            foreach (JObject item in array)
                            {
                                String away = results["away"]["name"].ToString();
                                String home = results["home"]["name"].ToString();
                                String league = results["league"]["name"].ToString();
                                away = away.Replace("'", "''");
                                home = home.Replace("'", "''");
                                league = league.Replace("'", "''");
                                //check if the away exist in the db
                                DataTable dt = MainApp.mSql.ExecuteQuery(String.Format("select * from ref_team where rt_en_name = '{0}' and Rt_sports_name='{1}'", away, sportName));
                                if (dt == null || dt.Rows.Count == 0)
                                {
                                    away = away.Replace("''", "'");
                                    String K_away = "";
                                    try
                                    {
                                        K_away = item["home"]["name"].ToString();
                                        MainApp.mSql.saveTeam(sportId[sportName].ToString(), sportName, K_away, away, "1");
                                    } catch(Exception ex)
                                    {
                                        K_away = away;
                                        MainApp.mSql.saveTeam(sportId[sportName].ToString(), sportName, K_away, away, "3");
                                    }                                    
                                } else
                                {
                                    dt = MainApp.mSql.ExecuteQuery(String.Format("select * from ref_team where rt_en_name = '{0}' and Rt_sports_name='{1}' and Rt_status='1'", away, sportName));
                                    if( dt == null || dt.Rows.Count == 0)
                                    {
                                        //update
                                        try
                                        {
                                            away = away.Replace("''", "'");
                                            String K_away = item["away"]["name"].ToString();
                                            MainApp.mSql.updateTeam(sportName, K_away, away);
                                        } catch(Exception ex)
                                        {

                                        }
                                    }
                                }

                                DataTable dt1 = MainApp.mSql.ExecuteQuery(String.Format("select * from ref_team where rt_en_name = '{0}' and Rt_sports_name='{1}'", home, sportName));
                                if (dt1 == null || dt1.Rows.Count == 0)
                                {
                                    home = home.Replace("''", "'");
                                    String K_home = "";
                                    try
                                    {
                                        K_home = item["away"]["name"].ToString();
                                        MainApp.mSql.saveTeam(sportId[sportName].ToString(), sportName, K_home, home,"1");
                                    }
                                    catch(Exception ex)
                                    {
                                        K_home = home;
                                        MainApp.mSql.saveTeam(sportId[sportName].ToString(), sportName, K_home, home, "3");
                                    }                                    
                                } else
                                {
                                    dt1 = MainApp.mSql.ExecuteQuery(String.Format("select * from ref_team where rt_en_name = '{0}' and Rt_sports_name='{1}' and Rt_status='1'", home, sportName));
                                    if (dt1 == null || dt1.Rows.Count == 0)
                                    {
                                        //update
                                        try
                                        {
                                            home = home.Replace("''", "'");
                                            String K_home = item["home"]["name"].ToString();
                                            MainApp.mSql.updateTeam(sportName, K_home, home);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }

                                DataTable dt2 = MainApp.mSql.ExecuteQuery(String.Format("select * from Ref_League where rl_sports ='{0}' and rl_league_full='{1}'", sportName, league));
                                DataTable country = MainApp.mSql.ExecuteQuery(String.Format("select * from Ref_Country where '{0}' like '%' + country_en + '%'", results["league"]["name"].ToString()));
                                String country_en = "";
                                String country_kr = "";
                                if(country != null && country.Rows.Count != 0)
                                {
                                    country_en = country.Rows[0]["country_en"].ToString() + ".png";
                                    country_kr = country.Rows[0]["country_kr"].ToString();
                                }

                                if (dt2 == null || dt2.Rows.Count == 0)
                                {
                                    league = league.Replace("''", "'");
                                    String K_League = "";
                                    try
                                    {
                                        K_League = item["league"]["name"].ToString();                                        
                                        MainApp.mSql.saveLeague(sportName, sportId[sportName].ToString(), K_League, league,"1", country_en, country_kr);
                                    } catch(Exception ex)
                                    {
                                        K_League = league;
                                        MainApp.mSql.saveLeague(sportName, sportId[sportName].ToString(), K_League, league, "3", country_en, country_kr);
                                    }   
                                } else
                                {
                                    dt2 = MainApp.mSql.ExecuteQuery(String.Format("select * from Ref_League where rl_sports ='{0}' and rl_league_full='{1}' and rl_status='1'", sportName, league));
                                    if(dt2 == null || dt2.Rows.Count == 0)
                                    {
                                        try
                                        {
                                            league = league.Replace("''", "'");
                                            String K_League = item["league"]["name"].ToString();
                                            MainApp.mSql.updateLeague(sportName, K_League, league);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }

                                System.DateTime dtDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                dtDate = dtDate.AddSeconds(Convert.ToInt32(results["time"].ToString()));

                                System.DateTime now = DateTime.UtcNow;
                                if (DateTime.Compare(dtDate, now) > 0)
                                {
                                    //DataTable dt3 = MainApp.mSql.ExecuteQuery(String.Format("select * from Info_Game where ig_root = '{0}' and IG_outcome_id='{1}'", results["id"].ToString(), results["our_event_id"].ToString()));
                                    //if (dt3 == null || dt3.Rows.Count == 0)
                                    {
                                        MainApp.log_info(String.Format("Start to get {0} event", results["id"].ToString()));
                                        String url1 = String.Format("https://api.betsapi.com/v1/sbobet/event?token={0}&event_id={1}&LNG_ID=71", token, results["id"].ToString());
                                        try
                                        {
                                            HttpWebRequest requests1 = (HttpWebRequest)WebRequest.Create(url1);
                                            using (WebResponse res1 = requests1.GetResponse())
                                            {
                                                using (Stream resStream1 = res1.GetResponseStream())
                                                {
                                                    using (StreamReader redr1 = new StreamReader(resStream1, System.Text.Encoding.UTF8))
                                                    {
                                                        String responseJSON1 = redr1.ReadToEnd();
                                                        JObject applyJObj1 = JObject.Parse(responseJSON1);

                                                        JArray array1 = JArray.Parse(applyJObj1["results"][0]["markets"].ToString());
                                                        foreach (JObject itm in array1)
                                                        {
                                                            try
                                                            {
                                                                if (itm["display"].ToString() != null)
                                                                {
                                                                    DataTable bet_type = MainApp.mSql.ExecuteQuery(String.Format("select * from info_bet_type where bt_sports_name='{0}' and bt_en_name='{1}'", sportName, itm["display"].ToString()));
                                                                    if(bet_type != null && bet_type.Rows.Count != 0)
                                                                    {
                                                                        String IG_root = results["id"].ToString();
                                                                        String IG_outcome_id = results["our_event_id"].ToString();
                                                                        String RL_Idx = sportName;
                                                                        String RL_Sports = sportId[sportName].ToString();
                                                                        String RL_League = results["league"]["name"].ToString();

                                                                        String RL_Image = country_en;// MainApp.mSql.getCountryName(item["league"]["cc"].ToString());

                                                                        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                                                        dtDateTime = dtDateTime.AddSeconds(Convert.ToInt32(results["time"].ToString()));

                                                                        String IG_StartTime = dtDateTime.ToString("yyyy-MM-dd H:mm:ss");
                                                                        String IG_Team1 = results["home"]["name"].ToString();
                                                                        String IG_Team2 = results["away"]["name"].ToString();
                                                                        String IG_Status = "S";
                                                                        String IG_Type = bet_type.Rows[0]["BT_Game_Type"].ToString();
                                                                        String IG_Site = MainApp.mSql.getSite();
                                                                        String IG_SP = bet_type.Rows[0]["BT_SpcYN"].ToString();
                                                                        String IG_Period_name = bet_type.Rows[0]["BT_scope"].ToString();
                                                                        String IG_Pv_type = itm["display"].ToString();
                                                                        String IG_Game_Type = bet_type.Rows[0]["BT_Kr_name"].ToString();

                                                                        System.DateTime dtDateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                                                        dtDateTime1 = dtDateTime1.AddSeconds(Convert.ToInt32(applyJObj1["results"][0]["updated_at"].ToString()));

                                                                        String IG_UpdateTime = dtDateTime1.ToString("yyyy-MM-dd H:mm:ss");
                                                                        String IG_OrderNo = bet_type.Rows[0]["BT_OrderNo"].ToString();
                                                                        String IG_Finish = "0";

                                                                        JArray array2 = JArray.Parse(itm["odds"].ToString());

                                                                        String IG_Handicap = "";
                                                                        String IG_Team1Benefit = "";
                                                                        String IG_DrawBenefit = "";
                                                                        String IG_Team2Benefit = "";
                                                                        if (array2.Count == 2)
                                                                        {
                                                                            IG_Handicap = itm["handicap"].ToString();
                                                                            IG_Team1Benefit = array2[0].ToString();
                                                                            IG_DrawBenefit = itm["handicap"].ToString();
                                                                            IG_Team2Benefit = array2[1].ToString();
                                                                        }
                                                                        else
                                                                        {
                                                                            IG_Handicap = itm["handicap"].ToString();
                                                                            IG_Team1Benefit = array2[0].ToString();
                                                                            IG_DrawBenefit = array2[1].ToString();
                                                                            IG_Team2Benefit = array2[2].ToString();
                                                                        }

                                                                        if (!(array2.Count == 2 && (IG_Team1Benefit.Substring(IG_Team1Benefit.Length - 3, 3) == ".25" || IG_Team1Benefit.Substring(IG_Team1Benefit.Length - 3, 3) == ".75" || IG_Team2Benefit.Substring(IG_Team2Benefit.Length - 3, 3) == ".25" || IG_Team2Benefit.Substring(IG_Team2Benefit.Length - 3, 3) == ".75")))
                                                                        {
                                                                            //Save
                                                                            DataTable dt3 = MainApp.mSql.ExecuteQuery(String.Format("select * from Info_Game where ig_root = '{0}' and IG_outcome_id='{1}' and RL_Idx='{2}' and RL_League='{3}' and IG_StartTime='{4}' and IG_Team1='{5}' and IG_Team2='{6}' and IG_pv_type='{7}' and IG_Game_Type='{8}'", results["id"].ToString(), results["our_event_id"].ToString(), RL_Idx, RL_League.Replace("'", "''"), IG_StartTime, IG_Team1.Replace("'", "''"), IG_Team2.Replace("'", "''"), IG_Pv_type, IG_Game_Type));
                                                                            if (dt3 != null && dt3.Rows.Count != 0)
                                                                            {
                                                                                MainApp.mSql.updateInfoGame(IG_root, IG_outcome_id, RL_Idx, RL_Sports, RL_League, RL_Image, IG_StartTime, IG_Team1, IG_Team2, IG_Handicap, IG_Team1Benefit, IG_DrawBenefit, IG_Team2Benefit, IG_Status, IG_Type, IG_Site, IG_SP, IG_Period_name, IG_Pv_type, IG_Game_Type, IG_UpdateTime, IG_OrderNo, IG_Finish);
                                                                            }
                                                                            else
                                                                            {
                                                                                MainApp.mSql.saveInfoGame(IG_root, IG_outcome_id, RL_Idx, RL_Sports, RL_League, RL_Image, IG_StartTime, IG_Team1, IG_Team2, IG_Handicap, IG_Team1Benefit, IG_DrawBenefit, IG_Team2Benefit, IG_Status, IG_Type, IG_Site, IG_SP, IG_Period_name, IG_Pv_type, IG_Game_Type, IG_UpdateTime, IG_OrderNo, IG_Finish);
                                                                            }
                                                                        }
                                                                    }                                                                    
                                                                }
                                                            }
                                                            catch(Exception ex)
                                                            {

                                                            }                                                            
                                                        }
                                                        redr1.Close();
                                                    }

                                                }
                                                res1.Close();
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }                                
                            }

                            redr.Close();
                            //redr.Dispose();
                        }
                    }
                    res.Close();
                    //res.Dispose();
                    //res = null;
                    //GC.Collect();
                }
            }
            catch (Exception ex)
            {
                MainApp.log_info(String.Format("Error catch the {0} event", results["id"].ToString()));
            }               
        }        

        private void btn_start_Click_1(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(ThreadProc));
            if (t.IsAlive)
                t.Abort();
            t.Start();

            //Thread t1 = new Thread(new ThreadStart(ThreadProc1));
            //if (t1.IsAlive)
            //    t1.Abort();
            //t1.Start();
        }
    }
}
