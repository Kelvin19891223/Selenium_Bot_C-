using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DentalDoc
{
    static class MainApp
    {
        public static MainFrm m_main_frm;
        public static String dbFile = "db.db";
        //public static SQLiteWrapper m_sql = new SQLiteWrapper();
        public static log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static System.Object g_locker = new object();
        public static MsSqlWrapper mSql = new MsSqlWrapper();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //m_sql.CreatConnection("db.db");
            //m_sql.Open();

            mSql.CreateConnection();
            
            m_main_frm = new MainFrm();
            Application.Run(m_main_frm);
        }

        public static void log_info(string msg)
        {
            lock (g_locker)
            {
                try
                {
                    logger.Info(msg);
                    if (m_main_frm != null)
                    {                        
                        string fname = "Log.txt";
                        while (file_writable(fname) == false) ;
                        File.AppendAllLines(fname, new string[] { DateTime.Now.ToString("HH:mm:ss ") + msg });
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static bool file_writable(string file)
        {
            try
            {
                using (Stream stream = new FileStream(file, FileMode.Append))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
