using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnimatedGifExamApp2
{
    /// <summary>
    /// RadioControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RadioControl : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        
        double screenWidth;
        double screenHeight;
        double windowWidth;
        double windowHeight;

        MainWindow getParent;
        public RadioControl(MainWindow getParent)
        {
            InitializeComponent();
            this.getParent = getParent;
            this.MouseLeftButtonDown += Window_MouseLeftButtonDown;

            screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            windowWidth = this.Width;
            windowHeight = this.Height;

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Mini_Click(object sender, RoutedEventArgs e)
        {
            //최소화
            this.WindowState = WindowState.Minimized;
        }

        private void Resizeable_Click(object sender, RoutedEventArgs e)
        {
            //바꾸기 전 체크
            Change_Check();
        }
        private void Change_Resize()
        {
            double parentWidth = getParent.default_width;
            double parentHeight = getParent.default_heigth;

            //(B * C) / A
            double radioWidth = (parentWidth * Convert.ToDouble(input.Text) / 100);
            double radioHeight = (parentHeight * Convert.ToDouble(input.Text) / 100);

            getParent.Width = getParent.border1.Width = radioWidth;
            getParent.Height = getParent.border1.Height = radioHeight;

            getParent.Left = (screenWidth / 2) - (windowWidth / 2) - (getParent.Width / 2);
            getParent.Top = (screenHeight / 2) - (windowHeight / 2) - (getParent.Height / 2);

            getParent.UpdateRow();
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            double inputNum;
            if (input.Text != "")
            {
                inputNum = Convert.ToDouble(input.Text);
                if (inputNum > 300)
                {
                    input.Text = "300";
                    SnackToggle("300초과", 1);
                }
            }
            
        }

        private void input_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Space))
            {
                e.Handled = true;
            }
            else if (e.Key.Equals(Key.Enter))
            {
                //바꾸기 전 체크
                Change_Check();
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key.Equals(Key.Escape))
            {
                this.Hide();
            }
        }

        private void SnackToggle(String message, long secode = 1)
        {
            SnackbarTwo.IsActive = true;
            snackMsg.Content = message;
            //타이머

            if (!timer.IsEnabled)
            {
                timer.Interval = TimeSpan.FromTicks(secode * 10000000);   // ticks 10000000 = 1초
                timer.Tick += (s, a) =>
                {
                    SnackbarTwo.IsActive = false;
                    timer.Stop();
                };
                timer.Start();
            }
        }

        private void Change_Check()
        {
            if (input.Text == "")
            {
                SnackToggle("숫자를 입력해주세요.", 1);
            }
            else if (Convert.ToDouble(input.Text) == 0)
            {
                SnackToggle("0을 입력할 순 없습니다.", 1);
            }
            else
            {
                Change_Resize();
            }
        }

    }
}
