using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> Types = new List<string>();
        private List<budgetMoney> Notes = new List<budgetMoney>();
        private List<budgetMoney> CurrentNote = new List<budgetMoney>();
        private int Money;
        private int SummTotal;
        public MainWindow()
        {
            InitializeComponent();
            Data.SelectedDate = DateTime.Today;
            var desktopFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Notes = serializDeserealiz.Serialization<List<budgetMoney>>(System.IO.Path.Combine(desktopFolderPath, "Notes.json")) ?? new List<budgetMoney>();
            Types = serializDeserealiz.Serialization<List<string>>(System.IO.Path.Combine(desktopFolderPath, "TypeName.json")) ?? new List<string>();
            foreach (var Type in Types.Where(Type => !string.IsNullOrEmpty(Type)))
            {
                TypeName.Items.Add(Type);
            }

            int sum = 0;
            int summ = 0;
            foreach (var Budget in Notes)
            {
                if (Budget.IsIncome == false)
                {
                    summ -= Budget.Ammount;
                }
                else
                {
                    sum += Budget.Ammount;
                }
            }
            SummTotal = sum + summ;
            SummText.Content = SummTotal;                
        
        }
        private void AddTypeClick(object sender, RoutedEventArgs e)
        {
            Window1 window = new Window1();
            if (window.ShowDialog() == true)
            {
                var newItem = window.InputText;
                TypeName.Items.Add(newItem);
                Types.Add(newItem);
                TypeName.SelectedItem = newItem;
            }
        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            AddBudget();
        }

        private void AddBudget()
        {
            Money = Convert.ToInt32(Summ.Text);
            if (!string.IsNullOrEmpty(TypeName.SelectedItem?.ToString()))
            {
                SummTotal += Money;
                SummText.Content = SummTotal;
                budgetMoney budgetValue = new budgetMoney(Data.SelectedDate.Value, Nazvaniye.Text, TypeName.SelectedItem.ToString(), Money,
                    Money >= 0);
                Notes.Add(budgetValue);
                TodayNotes();
            }
        }

        private void Data_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TodayNotes();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            var desktopFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            serializDeserealiz.Deserialization(Notes, System.IO.Path.Combine(desktopFolderPath, "Notes.json"));
            serializDeserealiz.Deserialization(Types, System.IO.Path.Combine(desktopFolderPath, "TypeName.json"));
        }

        private void TodayNotes()
        {
            CurrentNote.Clear();
            foreach (var Budget in Notes)
            {
                if (Budget.CurrentDate == Data.SelectedDate)
                {
                    CurrentNote.Add(Budget);
                }
            }
            DataGrid.ItemsSource = null;
            DataGrid.ItemsSource = CurrentNote;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid.SelectedIndex >= 0)
            {
                Nazvaniye.Text = CurrentNote[DataGrid.SelectedIndex].Name;
                TypeName.SelectedItem = CurrentNote[DataGrid.SelectedIndex].Type;
                Summ.Text = CurrentNote[DataGrid.SelectedIndex].Ammount.ToString();
            }
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            DeleteBudget();
        }
        private void DeleteBudget()
        {
            if (DataGrid.SelectedItem is budgetMoney budget)
            {
                SummTotal -= budget.Ammount;
                SummText.Content = SummTotal;
                Nazvaniye.Text = null;
                TypeName.SelectedItem = null;
                Summ.Text = null;
                Notes.Remove(budget);   
                serializDeserealiz.Deserialization(Notes, System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                @"Notes.json"));
                TodayNotes();
            }
            else
            {
                return;
            }
        }

        private void EditClick(object sender, RoutedEventArgs e)
        {
            int deleteBudget = DataGrid.SelectedIndex;
            AddBudget();
            DataGrid.SelectedIndex = deleteBudget;
            DeleteBudget();
        }

    }
}
