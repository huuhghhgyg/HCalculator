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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignColors;
using MaterialDesignThemes;
using MahApps.Metro.Controls;
using System.Windows.Media.Animation;
using System.Threading;

namespace calculator_wpf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        string firstMath="";
        string secMath="";
        string result="";
        string sym="";
        string history = "";//记录计算历史
        string smatGus = "";//猜测下一步做法
        bool symPushed = false;//符号是否已经输出

        private void doCal(string symbol)//加法方法
        {
            if (sym == "" || secMath == "")
            {
                sym = symbol;//无符号时
                if (firstMath == "" && smatGus != "")
                {
                    firstMath = smatGus;//设置1和符号
                }
                if (symPushed == false)
                {
                    //resultBox.Text += sym;//计算命令由等号按钮执行，calculate方法
                    refresh();
                    symPushed = true;
                }
            }
            else
            {
                if (smatGus != "")
                {
                    firstMath = smatGus;
                    smatGus = "";//智能预算
                    calculate();//计算完结果再次输入符号时表示用户要继续计算
                    sym = symbol;
                    if (symPushed == false)//判断符号输出状态
                    {
                        //resultBox.Text += sym;//计算命令由等号按钮执行，calculate方法
                        refresh();
                        symPushed = true;
                    }
                }
                else
                {
                    calculate();
                    firstMath = smatGus;
                    smatGus = "";
                    sym = symbol;
                    symPushed = true;
                }
            }
            if (firstMath=="")
            {
                firstMath = "0";
            }
            refresh();
        }

        private void refresh()//显示模块
        {
            /*原有回显模块
                        if (sym == "")
                        {
                            if (firstMath == "")
                            {
                                resultBox.Text = "0";
                            }
                            else
                            {
                                resultBox.Text = firstMath;
                            }
                        }
                        else
                        {
                            resultBox.Text = firstMath + sym + secMath;
                            resultBox.ScrollToEnd();
                        }
            */
            if (history == "")//当没有历史纪录时
            {
                resultBox.Text = firstMath + sym + secMath;//显示结果为"1+2"
            }
            else
            {
                resultBox.Text = history + "\r\n" + firstMath + sym + secMath;//有历史记录时加上历史纪录并换行
            }
            resultBox.ScrollToEnd();
        }

        private void backspace()
        {
            if (sym == "")
            {
                if (firstMath != "")
                {
                    firstMath = firstMath.Substring(0, firstMath.Length - 1);
                    refresh();
                }
                else
                {
                    resultBox.Text = "0";
                }
            }
            else if (secMath == "")
            {
                sym = "";
                refresh();
            }
            else
            {
                if (secMath != "")
                {
                    secMath = secMath.Substring(0, secMath.Length - 1);
                    refresh();
                }
            }
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dotDown()
        {
            //int index = firstMath.IndexOf(".");
            //if (index > -1)
            //{ }

            if (sym=="")
            {
                if (firstMath == "")//第一个字符为null
                {
                    firstMath = "0.";
                }
                else
                {
                    int index = firstMath.IndexOf(".");
                    if (index > -1)
                    { }
                    else
                    {
                        firstMath += ".";//如果数值内没有小数点，+="."
                    }
                }
            }
            else
            {
                if (secMath=="")
                {
                    secMath = "";
                }
                else
                {
                    int index = secMath.IndexOf(".");
                    if (index > -1)
                    { }
                    else
                    {
                        secMath += ".";
                    }
                }
            }
            refresh();
        }

        private void pn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sym == "")
                {
                    if (firstMath == "")
                    {
                        if (smatGus == "")
                        {
                            firstMath = "0";
                        }
                        else
                        {
                            firstMath = (Convert.ToDouble(smatGus) * -1).ToString();
                            smatGus = "";
                        }
                    }
                    else
                    {
                        firstMath = (Convert.ToDouble(firstMath) * -1).ToString();//firstmath乘-1再转换为string
                    }
                }
                else
                {
                    secMath = (Convert.ToDouble(secMath) * -1).ToString();//firstmath乘-1再转换为string
                }
                refresh();
            }
            catch
            {
                showMsg("错误", "计算该数负值错误", 1500);
            }
        }

        private void enter(int press)
        {
            if (firstMath=="")//当1为空但用户输入字符时,smartguess取消
            {
                smatGus = "";
                refresh();
                resultBox.Text += "\r\n";//刷新并换行
            }
            if (sym == "")
            {
                firstMath += press.ToString();//输入数值方法
                if (resultBox.Text == "0")
                {
                    resultBox.Text = "";
                }
                resultBox.Text += press;//回显(修改前:press=firstmath)
            }
            else
            {
                secMath += press.ToString();//同上
                resultBox.Text += /*firstMath + sym +*/ press;//(修改前press=secMath)
            }
            refresh();
        }

        private void clearNum()
        {
            firstMath = "";
            secMath = "";
            sym = "";
            resultBox.Text = "0";
            result = "";
            history = "";
        }

        private void calculate()
        {
            if (resultBox.Text == "除数不能为零!\r\n=∞" || resultBox.Text == "0")
            {
                resultBox.Text = "0";
                firstMath = "0";
                secMath = "0";
                sym = "";
                smatGus = "";
                }
                else
                {
                    if (secMath != "")
                    {
                        if (sym == "÷")
                        {
                            result = (Convert.ToDouble(firstMath) / Convert.ToDouble(secMath)).ToString();
                        }
                        if (firstMath=="")
                        {
                            firstMath = "0";
                            refresh();
                        }
                        symPushed = false;//计算时吧符号变量状态设为false
                        switch (sym)
                        {
                            case "+":
                                result = (Convert.ToDouble(firstMath) + Convert.ToDouble(secMath)).ToString();
                                break;
                            case "-":
                                result = (Convert.ToDouble(firstMath) - Convert.ToDouble(secMath)).ToString();
                                break;
                            case "×":
                                result = (Convert.ToDouble(firstMath) * Convert.ToDouble(secMath)).ToString();
                                break;
                        case "÷":
                            if (secMath == "0" && sym == "÷")
                            {
                                showMsg("错误", "除数不能为零，所有数据已经被清零",1500);
                                sym = "";
                                firstMath = "";
                                secMath = "";
                                history = "";
                            }
                            else
                            {
                                result = (Convert.ToDouble(firstMath) / Convert.ToDouble(secMath)).ToString();
                            }
                            break;
                        case "^":
                            result = Math.Pow(Convert.ToDouble(firstMath), Convert.ToDouble(secMath)).ToString();
                            break;
                        }
                        resultBox.Text += "\r\n=" + result;//输出过程已自带
                        smatGus = result;
                        firstMath = "";
                        secMath = "";
                        sym = "";
                        resultBox.ScrollToEnd();//滚动到底部，就能看见计算结果
                        history = resultBox.Text;
                    }
                }
            }

        private void plusBtn_Click(object sender, RoutedEventArgs e)
        {
            doCal("+");
        }

        private void num1_Click_1(object sender, RoutedEventArgs e)
        {
            enter(1);
        }

        private void equals_Click(object sender, RoutedEventArgs e)
        {
            calculate();
        }

        private void num2_Click(object sender, RoutedEventArgs e)
        {
            enter(2);
        }

        private void num3_Click(object sender, RoutedEventArgs e)
        {
            enter(3);
        }

        private void num4_Click(object sender, RoutedEventArgs e)
        {
            enter(4);
        }

        private void num5_Click(object sender, RoutedEventArgs e)
        {
            enter(5);
        }

        private void num6_Click(object sender, RoutedEventArgs e)
        {
            enter(6);
        }

        private void num7_Click(object sender, RoutedEventArgs e)
        {
            enter(7);
        }

        private void num8_Click(object sender, RoutedEventArgs e)
        {
            enter(8);
        }

        private void num9_Click(object sender, RoutedEventArgs e)
        {
            enter(9);
        }

        private void num0_Click(object sender, RoutedEventArgs e)
        {
            enter(0);
        }

        private void minusBtn_Click(object sender, RoutedEventArgs e)
        {
            doCal("-");
        }

        private void multiplyBtn_Click(object sender, RoutedEventArgs e)
        {
            doCal("×");
        }

        private void divideBtn_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (firstMath == "")
            {
                firstMath = "0";
            }
            sym = "÷";
            if (symPushed == false)
            {
                resultBox.Text += sym;
                symPushed = true;
            }
            */
            doCal("÷");
        }

        private void backSpace_Click(object sender, RoutedEventArgs e)
        {
            backspace();
        }
        bool outed = false;
        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            //refresh();
            if (outed==false)
            {
                outed = true;
                ThicknessAnimation ta = new ThicknessAnimation();
                ta.From = new Thickness(264, 157, 0, 0);             //起始值
                ta.To = new Thickness(80, 157, 0, 0);        //结束值
                ta.Duration = TimeSpan.FromSeconds(0.2);         //动画持续时间
                board.BeginAnimation(MarginProperty, ta);//开始动画
                refreshBtn.Content="→";
            }
            else
            {
                outed = false;
                ThicknessAnimation ta = new ThicknessAnimation();
                ta.From = new Thickness(80, 157, 0, 0);             //起始值
                ta.To = new Thickness(264, 157, 0, 0);        //结束值
                ta.Duration = TimeSpan.FromSeconds(0.2);         //动画持续时间
                board.BeginAnimation(MarginProperty, ta);//开始动画
                refreshBtn.Content = "←";
            }
        }

        private void per_Click(object sender, RoutedEventArgs e)
        {
            if (firstMath != "")//避免""乘0.01
            {
                if (sym == "")
                {
                    firstMath = (Convert.ToDouble(firstMath) * 0.01).ToString();//firstmath乘0.01(%)再转换为string
                }
                else
                {
                    if (secMath=="")
                    {
                        firstMath = (Convert.ToDouble(firstMath) * 0.01).ToString();//firstmath乘0.01(%)再转换为string
                    }
                    else
                    {
                        secMath = (Convert.ToDouble(secMath) * 0.01).ToString();//firstmath乘0.01(%)再转换为string
                    }
                }
                refresh();
            }
        }

        private void nn_Click(object sender, RoutedEventArgs e)
        {
            doCal("^");
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            clearNum();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
 //           var key = e.Key;
//            MessageBox.Show(key.ToString());
            switch (e.Key)
            { 
                case Key.Enter:
                    calculate();
                break;
                case Key.NumPad1:
                    enter(1);
                    break;
                case Key.NumPad2:
                    enter(2);
                    break;
                case Key.NumPad3:
                    enter(3);
                    break;
                case Key.NumPad4:
                    enter(4);
                    break;
                case Key.NumPad5:
                    enter(5);
                    break;
                case Key.NumPad6:
                    enter(6);
                    break;
                case Key.NumPad7:
                    enter(7);
                    break;
                case Key.NumPad8:
                    enter(8);
                    break;
                case Key.NumPad9:
                    enter(9);
                    break;
                case Key.NumPad0:
                    enter(0);
                    break;          //按键反应：数字键盘

                case Key.D0:
                    enter(0);
                    break;
                case Key.D1:
                    enter(1);
                    break;
                case Key.D2:
                    enter(2);
                    break;
                case Key.D3:
                    enter(3);
                    break;
                case Key.D4:
                    enter(4);
                    break;
                case Key.D5:
                    enter(5);
                    break;
                case Key.D6:
                    enter(6);
                    break;
                case Key.D7:
                    enter(7);
                    break;
                case Key.D8:
                    enter(8);
                    break;
                case Key.D9:
                    enter(9);
                    break;

                case Key.Add:
                    doCal("+");
                    break;
                case Key.OemPlus:
                    calculate();
                    break;
                case Key.Subtract:
                    doCal("-");
                    break;
                case Key.Multiply:
                    doCal("×");
                    break;
                case Key.Divide:
                    doCal("÷");
                    break;
                case Key.Clear:
                    clearNum();
                    break;
                case Key.Back:
                    backspace();
                    break;
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Decimal:
                    dotDown();
                    break;
                case Key.F5:
                    refresh();
                    break;
            }
        }

        private void dot_Click(object sender, RoutedEventArgs e)
        {
            dotDown();
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    calculate();
                    break;
                case Key.NumPad1:
                    enter(1);
                    break;
                case Key.NumPad2:
                    enter(2);
                    break;
                case Key.NumPad3:
                    enter(3);
                    break;
                case Key.NumPad4:
                    enter(4);
                    break;
                case Key.NumPad5:
                    enter(5);
                    break;
                case Key.NumPad6:
                    enter(6);
                    break;
                case Key.NumPad7:
                    enter(7);
                    break;
                case Key.NumPad8:
                    enter(8);
                    break;
                case Key.NumPad9:
                    enter(9);
                    break;
                case Key.NumPad0:
                    enter(0);
                    break;          //按键反应：数字键盘

                case Key.D0:
                    enter(0);
                    break;
                case Key.D1:
                    enter(1);
                    break;
                case Key.D2:
                    enter(2);
                    break;
                case Key.D3:
                    enter(3);
                    break;
                case Key.D4:
                    enter(4);
                    break;
                case Key.D5:
                    enter(5);
                    break;
                case Key.D6:
                    enter(6);
                    break;
                case Key.D7:
                    enter(7);
                    break;
                case Key.D8:
                    enter(8);
                    break;
                case Key.D9:
                    enter(9);
                    break;

                case Key.Add:
                    doCal("+");
                    break;
                case Key.OemPlus:
                    calculate();
                    break;
                case Key.Subtract:
                    doCal("-");
                    break;
                case Key.Multiply:
                    doCal("×");
                    break;
                case Key.Divide:
                    doCal("÷");
                    break;
                case Key.Clear:
                    clearNum();
                    break;
                case Key.Back:
                    backspace();
                    break;
                case Key.Escape:
                    this.Close();
                    break;
                case Key.Decimal:
                    dotDown();
                    break;
                case Key.F5:
                    refresh();
                    break;
            }
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            info infoForm = new info();
            infoForm.Show();
        }

        private void rnd_Selected(object sender, RoutedEventArgs e)
        {
            out1.Text = "最小值";out2.Text = "最大值";
            clearInput();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //MessageBox.Show(menu1.SelectedIndex.ToString());
                switch (menu1.SelectedIndex)
                {
                    case 1:
                        Random rad = new Random();//实例化随机数产生器rad；
                        int value = rad.Next(int.Parse(box1.Text), int.Parse(box2.Text));//用rad生成大于等于1000，小于等于9999的随机数；
                        resultBox1.Text = value.ToString();
                        break;
                    case 3:
                        resultBox1.Text = (int.Parse(box1.Text) * int.Parse(box2.Text)).ToString();
                        break;
                    case 4:
                        resultBox1.Text = (Convert.ToDouble(box2.Text) * Convert.ToDouble(box1.Text) * Convert.ToDouble(box1.Text)).ToString();
                        break;
                }
            }
            catch
            {
                showMsg("无法计算", "输入的数值不正确或者无法计算", 1500);
            }
        }

        private void clearInput()
        {
            box1.Text = "";box2.Text = "";resultBox1.Text = "";
        }

        private void ab_Selected(object sender, RoutedEventArgs e)
        {
            out1.Text = "长";out2.Text = "宽";
            clearInput();
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            out1.Text = "半径";out2.Text = "π取值";box2.Text = "3.14";
            clearInput();
        }

        private void pi_Click(object sender, RoutedEventArgs e)
        {
            if (sym == "")
            {
                if (firstMath == "")
                {
                    firstMath = Math.PI.ToString();
                }
                else
                {
                    firstMath = (Convert.ToDouble(firstMath) * Math.PI).ToString();
                }
            }
            else
            {
                if (secMath == "")
                {
                    secMath = Math.PI.ToString();
                }
                else
                {
                    secMath = (Convert.ToDouble(secMath) * Math.PI).ToString();
                }
            }
            refresh();
        }

        private void Expander_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.From = new Thickness(0, -139, -0.333, 459);             //起始值
            ta.To = new Thickness(0, 0, 0, 320);        //结束值
            ta.Duration = TimeSpan.FromSeconds(0.2);         //动画持续时间
            Message.BeginAnimation(MarginProperty, ta);//开始动画
        }

        private void showMsg(string title,string msg,int showTime)
        {
            msgTitle.Content = title;
            boxMsg.Content = msg;
            ThicknessAnimation ta = new ThicknessAnimation();
            ta.From = new Thickness(0, -139, -0.333, 459);             
            ta.To = new Thickness(0, 0, 0, 320);        
            ta.Duration = TimeSpan.FromSeconds(0.2);         //动画持续时间
            Message.BeginAnimation(MarginProperty, ta);//开始动画
            Thread t = new Thread(() =>
            {
                Thread.Sleep(showTime);//次线程休眠1秒
                Dispatcher.Invoke(new Action(() =>
                {
                    ThicknessAnimation to = new ThicknessAnimation();
                    to.To = new Thickness(0, -139, -0.333, 459);
                    to.From = new Thickness(0, 0, 0, 320);
                    to.Duration = TimeSpan.FromSeconds(0.2);         //动画持续时间
                    Message.BeginAnimation(MarginProperty, to);//开始动画
                }));
            });
            t.Start();
        }

        private void calTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime time1;
                DateTime time2;
                time1 = DateTime.Parse(date1.Text);
                time2 = DateTime.Parse(date2.Text);

                TimeSpan ts = time1 - time2;
                double dDays = ts.TotalDays;

                dayResult.Text = (Math.Abs(dDays)).ToString() + "天";
            }
            catch(System.FormatException)
            {
                showMsg("计算时错误", "输入的日期格式错误", 1500);
            }
            catch { }
        }

        private string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new
            TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            return dateDiff;
        }
    }
}
