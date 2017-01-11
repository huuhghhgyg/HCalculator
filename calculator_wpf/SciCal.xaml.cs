using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CalCore;
using MahApps.Metro.Controls;

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
            Class1 calcore = new Class1();
            outPut.Text += input.Text + "\nResult=" + calcore.adCal(input.Text)+"\n\n";
            outPut.ScrollToEnd();
        }

        private void input_TextChanged(object sender, TextChangedEventArgs e)
        {
            Class1 calcore = new Class1();
            try
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
            catch { }
        }
    }
}
