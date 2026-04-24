using System.Dynamic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;


namespace Radio
{
    public partial class MainWindow : Window
    {
        public Radio_code radio;
        public MediaPlayer player = new MediaPlayer();

        public Dictionary<int, string[]> radio_sender;
        public MainWindow()
        {
            InitializeComponent();
            }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            radio = new Radio_code(Regler_canvas, 89, 50, Volume_out);
            radio_sender = radio.sender_dic_;
            player.Volume = radio.Volume/100;
            player.Open(new Uri(radio_sender[radio.Frequency][1]));
            player.Play();
            Canvas.SetLeft(radio.label_ra, (radio.Frequency - 88) * 27.5);
            radio.line_ra.X1 = (radio.Frequency - 88) * 27.5 + 25;
            radio.line_ra.X2 = (radio.Frequency - 88) * 27.5 + 25;
            radio.label_ra.Content = $"{radio.Frequency} MHz";
            Aktive_radiosender.Content = radio_sender[radio.Frequency][0];
            Timer();
        }
        private void updaten()
        {
            Canvas.SetLeft(radio.label_ra, (radio.Frequency - 88) * 27.5);
            radio.line_ra.X1 = (radio.Frequency - 88) * 27.5 + 25;
            radio.label_ra.Content = $"{radio.Frequency} MHz";
            radio.line_ra.X2 = (radio.Frequency - 88) * 27.5 + 25;
            player.Stop();
            player.Open(new Uri(radio_sender[radio.Frequency][1]));
            player.Play();
            Aktive_radiosender.Content = radio_sender[radio.Frequency][0];
        }
        private async void Timer()
        {
            await Task.Delay(100);
            s1.Content = radio.sender_array[0];
            s2.Content = radio.sender_array[1];
            s3.Content = radio.sender_array[2];
            s4.Content = radio.sender_array[3];
            s5.Content = radio.sender_array[4];
            
        }
        private void Volume_plus_Click(object sender, RoutedEventArgs e)
        {
            player.Volume = radio.Volume / 100;
            radio.VolumeUp();
        }

        private void Volume_minus_Click(object sender, RoutedEventArgs e)
        {
            radio.VolumeDown();
            player.Volume = radio.Volume/100;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            radio.FrequencyUp();
            if(radio.Frequency<=108){
                updaten();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            radio.FrequencyDown();
            if (radio.Frequency>=88){
                updaten();
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            if (s1.IsChecked == true)
            {
                radio.LoadStation(0, radio_sender);
            }
            else if (s2.IsChecked == true)
            {
                radio.LoadStation(1, radio_sender);
            }
            else if (s3.IsChecked == true)
            {
                radio.LoadStation(2, radio_sender);
            }
            else if (s4.IsChecked == true)
            {
                radio.LoadStation(3, radio_sender);
            }
            else if (s5.IsChecked == true)
            {
                radio.LoadStation(4, radio_sender);
            }
            Timer();
            updaten();

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            WindowRadioStation window = new WindowRadioStation($"{radio.Volume}", $"{radio.Frequency}");
            window.ShowDialog();
            if (window.result)
            {
                string radioname = window.radioName;
                radio_sender[radio.Frequency] = [radioname, radio_sender[radio.Frequency][1]];
                Aktive_radiosender.Content = radioname;
            }

            if (s1.IsChecked == true)
            {
                radio.SaveStation(0, radio_sender);
            }
            else if (s2.IsChecked == true)
            {
                radio.SaveStation(1, radio_sender);
            }
            else if (s3.IsChecked == true)
            {
                radio.SaveStation(2, radio_sender);
            }
            else if (s4.IsChecked == true)
            {
                radio.SaveStation(3, radio_sender);
            }
            else if (s5.IsChecked == true)
            {
                radio.SaveStation(4, radio_sender);
            }
            Timer();

        }

    }
}