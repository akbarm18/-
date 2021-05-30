using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp4.Commands;

namespace WpfApp4.ViewModels
{
    class CalculateViewModel : DependencyObject
    {
        ClassHistory hClass,selectedItem;
        
        public ClassHistory SelectedClass
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedClass");
            }
        }
        public ClassHistory HClass {
            get { return hClass; }
            set
            {
                hClass = value;
                OnPropertyChanged("HClass");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public CalculateViewModel()
        {
            HistoryNumbers = new ObservableCollection<ClassHistory>();
            SelectedClass = new ClassHistory(Numbers, OldNumberStr);
        }

        OnlyFalseCommand falseCommand;
        public OnlyFalseCommand FalseCommand
        {
            get
            {
                return falseCommand??(falseCommand= new OnlyFalseCommand());
            }
        }
        AllNotSymbol clearHistory;
        public AllNotSymbol ClearHistory {
            get
            {
                return clearHistory ?? (clearHistory = new AllNotSymbol(() =>
                {
                    HistoryNumbers.Clear();
                }));
            }
        }
        public string Numbers
        {
            get { return (string)GetValue(NumbersProperty); }
            set { SetValue(NumbersProperty, value); }
        }

        double numberSave { get; set; }
        public static readonly DependencyProperty NumbersProperty =
            DependencyProperty.Register("Numbers", typeof(string), typeof(CalculateViewModel), new PropertyMetadata("0"));
        public string OldNumberStr
        {
            get { return (string)GetValue(OldNumberStrProperty); }
            set { SetValue(OldNumberStrProperty, value); }
        }

        public static readonly DependencyProperty OldNumberStrProperty =
            DependencyProperty.Register("OldNumberStr", typeof(string), typeof(CalculateViewModel) );

        public char Sign
        {
            get { return (char)GetValue(SignProperty); }
            set { SetValue(SignProperty, value); }
        }

        public static readonly DependencyProperty SignProperty =
            DependencyProperty.Register("Sign", typeof(char), typeof(CalculateViewModel));

        public double OldNumber
        {
            get { return (double)GetValue(OldNumberProperty); }
            set { SetValue(OldNumberProperty, value); }
        }

        public static readonly DependencyProperty OldNumberProperty =
            DependencyProperty.Register("OldNumber", typeof(double), typeof(CalculateViewModel));
        
        #region EqualNumber
        AllNotSymbol equalCom;
        public AllNotSymbol EqualCom
        {
            get
            {
                return equalCom ?? (equalCom = new AllNotSymbol(() =>
                    {
                        
                        Numbers = OldNumber.ToString();
                        if (Sign == '*')
                        {
                            OldNumberStr = Numbers + " * " + numberSave.ToString() + " = ";
                            Numbers = Convert.ToDouble(Convert.ToDouble(Numbers.Replace(" ",""))* numberSave).ToString();
                        }
                        else if (Sign == '+')
                        {
                            OldNumberStr = Numbers + " + " + numberSave + " = ";
                            Numbers = Convert.ToDouble(Convert.ToDouble(Numbers.Replace(" ", "")) + numberSave).ToString();
                        }
                        else if (Sign == '-')
                        {
                            OldNumberStr = Numbers + " - " + numberSave + " = ";
                            Numbers = Convert.ToDouble(Convert.ToDouble(Numbers.Replace(" ", "")) - numberSave).ToString();
                        }
                        else if (Sign == '/')
                        {
                            OldNumberStr = Numbers + " / " + numberSave + " = ";
                            Numbers = Convert.ToDouble(Convert.ToDouble(Numbers.Replace(" ", "")) / numberSave).ToString();
                        }
                        OldNumber = Convert.ToDouble(Numbers.Replace(" ", ""));
                        HClass = new ClassHistory(Numbers, OldNumberStr);
                        HistoryNumbers.Insert(0,HClass);
                        Tidy();

                    }));
            }
        }
        #endregion

        #region [Tidy]
        void Tidy()
        {
            int j = 3;
            int length = Numbers.Length;
            if ( (length > 3 || length <= 21))
            {
                Numbers = Numbers.Replace(" ", "");

                if (Numbers.IndexOf(',') != -1)
                {
                    string[] mass = Numbers.Split(',');
                    for (int i = mass[0].Length; i >= 3; i -= 3)
                    {
                        mass[0] = mass[0].Insert(mass[0].Length - j, " ");
                        j += 4;
                    }
                    Numbers =mass[0]+","+ mass[1];
                }
                else
                {
                    for (int i = Numbers.Length; i >= 3; i -= 3)
                    {
                        Numbers = Numbers.Insert(Numbers.Length - j, " ");
                        j += 4;
                    }
                }
                
            }
            if (Numbers.Replace(" ", "").Length == 0)
            {
                Numbers = "0";
            }
        }
        #endregion

        #region CalculateCommand
        bool check,mult=false;
        AddNumberCommand calculate;
        public AddNumberCommand Calculate
        {
            get
            {
                return calculate ?? (calculate = new AddNumberCommand(obj =>
                    {
                        if (obj.ToString() == "x")
                        {
                            OldNumber = Convert.ToDouble(Numbers.Replace(" ", ""));
                            OldNumberStr = OldNumber.ToString() + " x ";
                            Sign = '*';
                            mult = true;
                        }
                        else if(obj.ToString() == "+")
                        {
                            OldNumber = Convert.ToDouble(Numbers.Replace(" ", ""));
                            OldNumberStr = OldNumber.ToString() + " + ";
                            Sign = '+';
                        }
                        else if(obj.ToString() == "-")
                        {
                            OldNumber = Convert.ToDouble(Numbers.Replace(" ", ""));
                            OldNumberStr = OldNumber.ToString() + " - ";
                            Sign = '-';

                        }
                        else
                        {
                            OldNumber = Convert.ToDouble(Numbers.Replace(" ", ""));
                            OldNumberStr = OldNumber.ToString() + " / ";
                            Sign = '/';

                        }
                        check = false;
                       

                    }));
            }
        }
        #endregion

        #region [AddNumberCommand]
        AddNumberCommand comm;
        
        public AddNumberCommand command
        {
            get
            {
                return comm ?? (comm = new AddNumberCommand((obj) =>
               {
               if (!check)
               {
                   Numbers = "0";
                   check = !check;
               }
               if ((Numbers.Length == 1 && Numbers[0] == '0'))
               {
                   Numbers = obj.ToString();
                   
               }
               else if (Numbers.Length <= 20)
               {
                   Numbers += obj;
                   Tidy();
               }
                   numberSave = Convert.ToDouble(Numbers.Replace(" ", ""));
                }));
            }
        }
        #endregion

        #region [Del]
        AllNotSymbol del;
        public AllNotSymbol Del
        {
            get
            {
                return del ?? (del = new AllNotSymbol(() =>
                    {
                        if (Numbers.Length != 0&&Convert.ToDouble(Numbers.Replace(" ",""))>=1)
                        {   
                            Numbers = Numbers.Remove(Numbers.Length - 1);
                            Tidy();
                        }
                        else
                        {
                            OldNumberStr = "";
                        }
                        
                    }));
            }
        }
        #endregion

        #region CClear
        AllNotSymbol comClear;
        public AllNotSymbol ComClear
        {
            get
            {
                return comClear ?? (comClear = new AllNotSymbol(() =>
                   {
                       OldNumberStr = "";
                       Numbers = "0";
                   }));
            }
        }
        #endregion

        #region CEClear
        AllNotSymbol comClearCE;
        public AllNotSymbol ComClearCE
        {
            get
            {
                return comClearCE ?? (comClearCE = new AllNotSymbol(() =>
                {
                if (OldNumberStr.IndexOf('=') != -1)
                    {
                        OldNumberStr = "";
                    }
                    Numbers = "0";
                }));
            }
        }
        #endregion

        #region [SquareAndProtsent]
        AddNumberCommand squareAndProtsent;
        public AddNumberCommand SquareAndProtsent
        {
            get
            {
                return squareAndProtsent ?? (squareAndProtsent = new AddNumberCommand(obj =>
                    {
                        string[] words = obj.ToString().Split('/');
                        double num = Convert.ToDouble(Numbers.Replace(" ", ""));
                        if (words[words.Length - 1] == "Root.png")
                        {
                            OldNumberStr = "√(" + num.ToString() + ")";
                            Numbers = Math.Sqrt(num).ToString();
                        }
                        else if (words[words.Length - 1] == "Square.png")
                        {
                            OldNumberStr = "(" + num.ToString() + ")²";
                            Numbers = (num * num).ToString();
                        }
                        else if (words[words.Length - 1] == "½")
                        {
                            OldNumberStr = "(" + num.ToString() + ")/2";
                            Numbers = (num / 2).ToString();
                        }
                        else if (words[words.Length - 1] == "PLus.png")
                        {
                           Numbers = (0-num ).ToString();
                        }
                        else if(words[words.Length - 1] == ",")
                        {
                            if (Numbers.IndexOf(',') == -1)
                            {
                                Numbers += ",";
                            }
                            
                        }else if(words[words.Length - 1] == "%"&&mult)
                        {
                            Numbers = (num / OldNumber).ToString();
                            OldNumberStr = OldNumber.ToString() + " * " + Numbers.Replace(" ","");
                            numberSave = Convert.ToDouble(Numbers.Replace(" ", ""));
                            mult = false;
                        }


                    }));
            }
        }
        #endregion

        public ObservableCollection<ClassHistory> HistoryNumbers { get; set; }

    }
    #region [ClassHistory]
    class ClassHistory 
    {
        string nums;
        string oldNum;
        public string Nums {
            get { return nums; }
            set
            {
                nums = value;
                OnPropertyChanged("Nums");
            }
        }
        public string OldNum {
            get { return oldNum; }
            set
            {
                oldNum = value;
                OnPropertyChanged("OldNum");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
      
        public ClassHistory(string Numbers, string OldNumberStr)
        {
            this.Nums = Numbers;
            this.OldNum = OldNumberStr;
            
        }
    }
    #endregion

}
