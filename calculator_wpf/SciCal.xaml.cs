using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CalCore;
using MahApps.Metro.Controls;
using System;

namespace calculator_wpf
{
    /// <summary>
    /// SciCal.xaml 的交互逻辑
    /// </summary>
    public partial class SciCal : MetroWindow
    {
        public SciCal()
        {
            InitializeComponent();
        }

        private void equals_Click(object sender, RoutedEventArgs e)
        {
            calculate();
        }

        string lastFormula;
        private void cal()
        {
            if (input.Text != "")
            {
                Core calcore = new Core();
                lastFormula = input.Text;
                outPut.Text += input.Text + "\nResult=" + calcore.adCal(input.Text) + "\n\n";
                outPut.ScrollToEnd();
            }
        }

        private void calculate()
        {
            Core calcore = new Core();
            if (input.Text[0].ToString()=="/")
            {
                switch (input.Text)
                {
                    case "/help":
                        outPut.Text += 
                            "\nF1 上一条算式 \n/help 帮助 /clear 清除 \n/exit 退出 \n\n";
                        break;
                    case "/clear":
                        outPut.Text = "Cleared!";
                        break;
                    case "/exit":
                        Environment.Exit(0);
                        break;
                }
            }
            else
            {
                try
                {
                    cal();
                }
                catch
                {
                    outPut.Text += input.Text+"\n计算错误\n";
                    outPut.ScrollToEnd();
                }
            }
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            Core calcore = new Core();
            try
            {
                if(input.Text=="")
                {
                    inputTitle.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#455A64"));
                    input.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#607D8B"));
                }
                else
                {
                    if (input.Text[0].ToString() == "/")
                    {
                        inputTitle.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00796B"));
                        input.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#009688"));
                    }
                    else
                    {
                        if (calcore.adCal(input.Text) == "Error")
                        {//#D32F2F
                            inputTitle.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C2185B"));
                            //#F44336
                            input.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E91E63"));
                        }
                        else
                        {
                            //#1976D2
                            //inputTitle.Background = 
                            inputTitle.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1976D2"));
                            //#2196F3
                            input.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3"));
                        }
                    }
                }
            }
            catch { }
        }

        private void input_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.F1:
                    input.Text = lastFormula;
                    break;
                case Key.Enter:
                    calculate();
                    input.Text = "";
                    break;
            }
        }
    }
}
