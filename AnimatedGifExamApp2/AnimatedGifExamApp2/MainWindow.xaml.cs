using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace AnimatedGifExamApp2
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Boolean closeBtn = false;
        RandomString generate_id = new RandomString();
        SQLConnection sql = new SQLConnection();
        private App app;
        public double default_width, default_heigth;
        private BackgroundWorker thread = new BackgroundWorker();
        RadioControl radioControl;
        public bool rightBtn = false;
        bool trigger = true;
        BitmapImage pngImg;
        BitmapImage gifImg;

        string id;
        
        public string getId
        {
            get { return id; }
        }
        

        public MainWindow(App app)
        {
            InitializeComponent();
            this.MouseLeftButtonDown += Window_MouseLeftButtonDown;
            this.MouseLeftButtonUp += Window_MouseLeftButtonUp;
            this.MouseRightButtonDown += Window_MouseRightButtonDown;

            thread.DoWork += new DoWorkEventHandler(thread_DoWork);
            radioControl = new RadioControl(this);

            default_width = this.Width;
            default_heigth = this.Height;

            this.app = app;

        }


        private void thread_DoWork(object sender, DoWorkEventArgs e)
        {/*
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                radioControl.Show();
            }));*/
        }

        private void Stop_Animation()
        {
            ImageBehavior.SetAnimatedSource(gif1, pngImg);
            //ImageBehavior.SetRepeatBehavior(gif1, new RepeatBehavior(0.00001));
            //ImageBehavior.SetRepeatBehavior(gif1, RepeatBehavior.Forever);
        }
        private void Loop_Animation()
        {
            ImageBehavior.SetAnimatedSource(gif1, gifImg);
            //ImageBehavior.SetRepeatBehavior(gif1, new RepeatBehavior(0.00001));
            ImageBehavior.SetRepeatBehavior(gif1, RepeatBehavior.Forever);
        }

        private void gif_loop_Click(object sender, RoutedEventArgs e)
        {
            rightBtn = false;
            if (gif_loop.IsCheckable)
            {
                gif_loop.IsCheckable = false;
                gif_loop.IsChecked = false;
                Stop_Animation();
            }
            else
            {
                gif_loop.IsChecked = true;
                gif_loop.IsCheckable = true;
                Loop_Animation();
            }
            UpdateRow();
        }

        private void gif_filp_Click(object sender, RoutedEventArgs e)
        {
            rightBtn = false;

            gif1.RenderTransformOrigin = new Point(0.5, 0.5);
            ScaleTransform flipTrans = new ScaleTransform();
            if (gif_filp.IsCheckable)
            {
                gif_filp.IsCheckable = false;
                gif_filp.IsChecked = false;
                flipTrans.ScaleX = 1;
                //flipTrans.ScaleY = -1;
                gif1.RenderTransform = flipTrans;
            }
            else
            {
                gif_filp.IsCheckable = true;
                gif_filp.IsChecked = true;

                flipTrans.ScaleX = -1;
                //flipTrans.ScaleY = -1;
                gif1.RenderTransform = flipTrans;
            }
            UpdateRow();
        }

        private void gif_top_Click(object sender, RoutedEventArgs e)
        {
            if (gif_top.IsCheckable)
            {
                gif_top.IsCheckable = false;
                gif_top.IsChecked = false;
                this.Topmost = false;
            }
            else
            {
                gif_top.IsCheckable = true;
                gif_top.IsChecked = true;
                this.Topmost = true;

            }
            UpdateRow();
        }

        private void gif_radio_Click(object sender, RoutedEventArgs e)
        {
            //thread.RunWorkerAsync();
            radioControl.WindowState = WindowState.Normal;
            radioControl.Show();
            rightBtn = false;
        }
         

        private void gif_boot_Click(object sender, RoutedEventArgs e)
        {
            if (gif_boot.IsCheckable)
            {
                sql.DeleteRow(id);
                gif_boot.IsCheckable = false;
                gif_boot.IsChecked = false;
            }
            else
            {
                InsertRow();
                gif_boot.IsCheckable = true;
                gif_boot.IsChecked = true;
            }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            closeBtn = true;
            UpdateRow(0);
            sql.CloseConnection();
            this.Close();
            radioControl.Close();
        }


        public void UpdateRow(int isNew = 1)
        {
            if (gif_boot.IsCheckable)
            {
                //MessageBox.Show("save");
                double width = this.Width;
                double height = this.Height;
                double top = this.Top;
                double left = this.Left;
                int loop = (gif_filp.IsCheckable) ? 1 : 0;
                int flip = (gif_loop.IsCheckable) ? 1 : 0;
                int isTop = (gif_top.IsCheckable) ? 1 : 0;

                //실행 
                sql.UpdatetRow(id, width, height, top, left, loop, flip, isTop, isNew);
            }
        }

        public void InsertRow()
        {
            double width = this.Width;
            double height = this.Height;
            double top = this.Top;
            double left = this.Left;
            int loop = (gif_filp.IsCheckable) ? 1 : 0;
            int flip = (gif_loop.IsCheckable) ? 1 : 0;
            int isTop = (gif_top.IsCheckable) ? 1 : 0;

            //실행 
            sql.InsertRow(id, width, height, top, left, loop, flip, isTop);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(!rightBtn)
            {
                Console.WriteLine("Start");
                Loop_Animation();
                DragMove();
            }
            rightBtn = false;
        }
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!gif_loop.IsChecked)
            {
                Stop_Animation();
            }

            UpdateRow();
        }
        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            rightBtn = true;
            Console.WriteLine("Right");
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //카톡 개인 사진으로 찾기 
            //[친구들 움직이는 사진]
            pngImg = new BitmapImage();
            pngImg.BeginInit();
            pngImg.UriSource = new Uri(@"/AnimatedGifExamApp2;component/Resources/죠르디울어.png", UriKind.Relative);
            pngImg.EndInit();

            gifImg = new BitmapImage();
            gifImg.BeginInit();
            gifImg.UriSource = new Uri(@"/AnimatedGifExamApp2;component/Resources/죠르디울어_m.gif", UriKind.Relative);
            gifImg.EndInit();

            gif1.Source = pngImg;
            this.Topmost = true;

            sql.Crate_DB_File_Click();
            sql.Open_DB();

            sql.Crate_Table();
            //sql.InsertRow("AAA", 10, 11, 12, 13);
            //sql.InsertRow("BBB", 11, 12, 13, 14);
            //sql.InsertRow("CCC", 12, 13, 14, 15);
            //sql.InsertRow("DDD", 13, 14, 15, 16);

            //sql.QueryData();

            /* 부팅할 때 기본 셋팅 */

            if (app.getId == null)
            {
                id = generate_id.GenerateString(10, new Random(), generate_id.GetLowerString() + generate_id.GetUpperString() + "123456789");
            }
            else
            {
                id = app.getId;
            }

            if(app.getWidth != 0)
                this.Width = app.getWidth;

            if (app.getHeight != 0)
                this.Height = app.getHeight;

            if (app.getTop != 0)
                this.Top = app.getTop;

            if (app.getLeft != 0)
                this.Left = app.getLeft;

            if (app.getLoop)
            {
                gif_loop.IsChecked = true;
                gif_loop.IsCheckable = true;
                Loop_Animation();
            }

            if(app.getIsBoot)
            {
                gif_boot.IsChecked = true;
                gif_boot.IsCheckable = true;
            }

            if (app.getFlip)
            {
                gif_filp.IsChecked = true;
                gif_filp.IsCheckable = true;
                gif1.RenderTransformOrigin = new Point(0.5, 0.5);
                ScaleTransform flipTrans = new ScaleTransform();
                flipTrans.ScaleX = -1;
                gif1.RenderTransform = flipTrans;
            }

            if(app.getIsNew)
            {
                UpdateRow();

                if (app.getIstop)
                {
                    gif_top.IsCheckable = true;
                    this.Topmost = true;
                }
                else
                {
                    gif_top.IsCheckable = false;
                    this.Topmost = false;
                }
            } else
            {
                gif_top.IsCheckable = true;
                this.Topmost = true;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            border1.Width = this.Width;
            border1.Height = this.Height;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (trigger)
                {
                    border1.Visibility = Visibility.Visible;
                    this.ResizeMode = ResizeMode.CanResizeWithGrip;
                    trigger = false;
                }
                else
                {
                    border1.Visibility = Visibility.Hidden;
                    this.ResizeMode = ResizeMode.NoResize;
                    trigger = true;
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (!closeBtn) UpdateRow(0);

            this.Close();
            radioControl.Close();
        }
    }
        
}
