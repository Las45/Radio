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

namespace Radio
{
    /// <summary>
    /// Interaktionslogik für WindowRadioStation.xaml
    /// </summary>
    public partial class WindowRadioStation : Window
    {
        private string volume;
        private string frequency;
        public string radioName {  get; private set; }
        public bool result {  get; private set; }

        public WindowRadioStation(string volume, string frequency)
        {
            InitializeComponent();
            this.volume = volume;
            this.frequency = frequency;
            LabelFrequency.Content = $"{frequency} MHz";
            LabelVolume.Content = $"{volume}%";
            this.radioName = "";
            this.result = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxStationName.Text == "")
            {
                MessageBox.Show("Bitte ein Stationname eingeben");
                return;
            }
            this.result = true;
            this.radioName=TextBoxStationName.Text;
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.result = false;
            this.Close();
        }
    }
}
