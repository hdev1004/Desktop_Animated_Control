using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AnimatedGifExamApp2
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        private string [] str;
        
        public string getId
        {
            get {
                if (str.Length != 9)
                {
                    return null;
                }
                return str[0]; 
            }
        }
        public double getWidth
        {
            get {
                if (str.Length != 9) return 0;
                return  Convert.ToDouble(str[1]); 
            }
        }
        public double getHeight
        {
            get {
                if (str.Length != 9) return 0;
                return Convert.ToDouble(str[2]); 
            }
        }
        public double getTop
        {
            get {
                if (str.Length != 9) return 0;
                return Convert.ToDouble(str[3]); 
            }
        }
        public double getLeft
        {
            get {
                if (str.Length != 9) return 0;
                return Convert.ToDouble(str[4]); 
            }
        }

        public Boolean getLoop
        {
            get
            {
                if (str.Length == 9)
                {
                    if (str[5].Equals("1")) return true;
                }
                return false;
            }
        }

        public Boolean getFlip
        {
            get
            {
                if (str.Length== 9)
                {
                    if (str[6].Equals("1")) return true;
                }
                return false;
            }
        }

        public Boolean getIstop
        {
            get
            {
                if (str.Length == 9)
                {
                    if (str[7].Equals("1")) return true;
                }
                return false;
            }
        }
        public Boolean getIsNew 
        {
            get
            {
                if (str.Length == 9)
                {
                    if (str[8].Equals("1")|| str[8].Equals("0")) return true;
                }
                return false;
            }
        }
        
        public Boolean getIsBoot
        {
            get
            {
                if (str.Length == 9) return true;
                return false;
            }
        }


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            str = e.Args;
            //MessageBox.Show(str[0] +" " + str[1]);
            MainWindow wnd = new MainWindow(this);
           

            wnd.Show();
        }
    }
}
