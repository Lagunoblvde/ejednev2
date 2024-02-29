using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VladikHalatik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string jsonFilePath = "zametka.json";
        private List<DataItem> data = new List<DataItem>();
        public MainWindow()
        {
            InitializeComponent();
            Date.SelectedDate = DateTime.Now;
            Date.SelectedDateChanged += Date_SelectedDateChanged;
            Zametki.SelectionChanged += Zametki_SelectionChanged;
            LoadData();
        }

        private void create_Click_1(object sender, RoutedEventArgs e)
        {
            var dataItem = new DataItem
            {
                Name = name.Text,
                PodName = podname.Text,
                Date = Date.SelectedDate.Value
            };

            data.Add(dataItem);

            File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(data, Formatting.Indented));

            if (Date.SelectedDate.Value.Date == dataItem.Date.Date)
            {
                Zametki.Items.Add(dataItem.Name);
            }
        }

        private void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Zametki.Items.Clear();

            var filteredData = data.Where(d => d.Date.Date == Date.SelectedDate.Value.Date).ToList();
            foreach (var item in filteredData)
            {
                Zametki.Items.Add(item.Name);
            }
        }


        private void Zametki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Zametki.SelectedItem != null)
            {
                var selectedItem = data.FirstOrDefault(d => d.Name == Zametki.SelectedItem.ToString());
                if (selectedItem != null)
                {
                    name.Text = selectedItem.Name;
                    podname.Text = selectedItem.PodName;
                }
            }
        }

        private void LoadData()
        {
            if (File.Exists(jsonFilePath))
            {
                string existingDataJson = File.ReadAllText(jsonFilePath);
                data = JsonConvert.DeserializeObject<List<DataItem>>(existingDataJson) ?? new List<DataItem>();

                var filteredData = data.Where(d => d.Date.Date == Date.SelectedDate.Value.Date).ToList();
                foreach (var item in filteredData)
                {
                    Zametki.Items.Add(item.Name);
                }
            }
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            if (Zametki.SelectedIndex != -1)
            {
                string selectedName = Zametki.SelectedItem.ToString();
                var selectedItem = data.FirstOrDefault(d => d.Name == selectedName);
                if (selectedItem != null)
                {
                    
                    data.Remove(selectedItem);

                    File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(data, Formatting.Indented));

                    Zametki.Items.Remove(Zametki.SelectedItem);
                }
            }
        }


        private void acc_Click(object sender, RoutedEventArgs e)
        {
            
            if (Zametki.SelectedIndex != -1)
            {
                string selectedName = Zametki.SelectedItem.ToString();
                var selectedItem = data.FirstOrDefault(d => d.Name == selectedName);

                if (selectedItem != null)
                {
                    selectedItem.Name = name.Text;
                    selectedItem.PodName = podname.Text;

                    File.WriteAllText(jsonFilePath, JsonConvert.SerializeObject(data, Formatting.Indented));

                    Zametki.Items[Zametki.SelectedIndex] = selectedItem.Name;
                }
            }
        }

        public class DataItem
        {
            public string Name { get; set; }
            public string PodName { get; set; }
            public DateTime Date { get; set; }
        }
    }
}