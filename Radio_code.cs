using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Windows.Media;
using System.Windows;
using System.IO;

namespace Radio
{
    public class Radio_code
    {
        public double MinFrequency { get;} = 88;
        public double MaxFrequency { get;} = 108;
        int _frquency;
        public int Frequency { 
            get { 
                return _frquency;
            } 
            set {
                if (MinFrequency <= value && value <= MaxFrequency)
                {
                    _frquency = value;
                }
            } }

        double _volume;
        public double Volume { 
            get{
                return _volume;
            } 
            set
            {
                if (value < 0)
                {
                    _volume = 0;
                }
                else if (value > 100) 
                { 
                    _volume = 100;
                }
                else
                {
                    _volume = value;
                }
            } }
        public double[] StationMemory { get; private set; }
        private Canvas canvasFrequency;
        private Label canvasVolume;
        public Label label_ra = new Label();
        public Dictionary<int, string[]> sender_dic_= new Dictionary<int, string[]>(){
                {88, ["Kronehit", "https://www.radio.at/s/kronehit.com"]},
                {89, ["Hitradio Ö3", "https://orf-live.ors-shoutcast.at/oe3-q1a"]},
                { 90, ["FM4", "https://orf-live.ors-shoutcast.at/fm4-q1a"] },
                { 91, ["Antenne Vorarlberg", "https://web.radio.antennevorarlberg.at/av-live/stream/mp3"] },
                { 92, ["Radio Vorarlberg", "https://orf-live.ors-shoutcast.at/vbg-q2a"] },
                { 93, ["Bayern 3", "https://dispatcher.rndfnk.com/br/br3/live/mp3/low"] },
                { 94, ["Ö1", "https://orf-live.ors-shoutcast.at/oe1-q1a"] },
                { 95, ["Radio Klassik Stephansdom", "https://stream.radioklassik.at/live/mp3"] },
                { 96, ["Radio Steiermark", "https://orf-live.ors-shoutcast.at/stm-q1a"] },
                { 97, ["Antenne Bayern", "https://stream.antenne.de/antenne"] },
                { 98, ["Bayern 1", "https://dispatcher.rndfnk.com/br/br1/live/mp3/low"] },
                { 99, ["SWR3", "https://liveradio.swr.de/sw282p3/swr3/play.mp3"] },
                { 100, ["Nanoq FM", "https://streamer.radio.co/s96954f0e3/listen"] },
                { 101, ["Rock Antenne", "https://stream.rockantenne.de/rockantenne"] },
                { 102, ["SRF 3", "https://stream.srg-ssr.ch/m/drs3/mp3_128"] },
                { 103, ["SRF 1", "https://stream.srg-ssr.ch/m/drs1/mp3_128"] },
                { 104, ["Hitradio Antenne 1", "https://stream.antenne1.de/a1stg/livestream2.mp3"] },
                { 105, ["Radio Swiss Pop", "https://stream.srg-ssr.ch/m/rsp/mp3_128"] },
                { 106, ["Radio Wien", "https://orf-live.ors-shoutcast.at/wie-q1a"] },
                { 107, ["Radio Burgenland", "https://orf-live.ors-shoutcast.at/bgl-q1a"] },
                { 108, ["SRF Virus", "https://stream.srg-ssr.ch/m/drsvirus/mp3_128"] }
            };
        public Line line_ra = new Line();
        public string[] sender_array = { "Hitradio Ö3","Antenne Vorarlberg","Radio Vorarlberg","Bayern 3","SRF 3" } ;
        public Radio_code(Canvas frequency, Label canvas_vol)
        {
            canvasFrequency = frequency;
            drawFrequency();
            LoadFile();
        }
        public Radio_code(Canvas frequency_canvas, int frequency, double volume, Label canvas_vol):this(frequency_canvas, canvas_vol)
        {
            Frequency = frequency;
            Volume = volume;
            canvas_vol.Content = $"Volumen: {Volume}%";
            canvasVolume = canvas_vol;
        }
        private void drawFrequency()
        {
            
            int freq = 88;
            for(int i = 0; i < (canvasFrequency.ActualWidth); i+=55)
            {
                Line line = new Line();
                line.X1 = i+25;
                line.X2 = i+25;
                line.Y1 = 75;
                line.Y2 = 150;
                line.Stroke = Brushes.Black;
                line.StrokeThickness = 2;

                Label label = new Label();
                label.Content = $"{freq}MHz";
                Canvas.SetLeft(label, i);
                Canvas.SetTop(label, 150);

                canvasFrequency.Children.Add(label);
                canvasFrequency.Children.Add(line);
                freq += 2;

            }
            line_ra.X1 = 165;
            line_ra.X2 = 165;
            line_ra.Y1 = 20;
            line_ra.Y2 = 150;
            line_ra.Stroke = Brushes.Red;
            line_ra.StrokeThickness = 2;
            line_ra.Name = "Regler_strich";

            label_ra.Content = $"{freq}MHz";
            label_ra.Foreground = Brushes.Red;
            label_ra.Name = "Regler_label";
            Canvas.SetLeft(label_ra, 138);
            Canvas.SetTop(label_ra, 0);

            canvasFrequency.Children.Add(label_ra);
            canvasFrequency.Children.Add(line_ra);
        }
        public void VolumeUp() 
        {
            Volume += 5;
            canvasVolume.Content = $"Volumen: {Volume}%";

        }
        public void VolumeDown()
        {
            Volume -= 5;
            canvasVolume.Content = $"Volumen: {Volume}%";

        }
        public void FrequencyUp()
        {
            Frequency += 1;
        }
        public void FrequencyDown() 
        { 
            Frequency -= 1;
        }
        public void LoadStation(int index, Dictionary<int, string[]> sender_dic)
        {
            
            foreach (int i in sender_dic.Keys) 
            {
                if (sender_dic[i][0] == sender_array[index])
                {
                    Frequency = i;
                }
            }
        }
        public void SaveStation(int index, Dictionary<int, string[]> sender_dic)
        {
            sender_array[index] = sender_dic[Frequency][0];
            using (StreamWriter write = new StreamWriter("lieder_aray.txt"))
            {
                write.Write("");
                for(int i= 0; i<5; i++)
                {
                    write.Write(sender_array[i]+";", true);
                }
                write.WriteLine("",true);
                for(int i= 0; i<20; i++)
                {
                    write.WriteLine($"{i+ 88};{sender_dic[i + 88][0]};{sender_dic[i + 88][1]}", true);
                }
            }
        }
        public override string ToString()
        {
            return $"Radio: {Frequency} MHz - Volume: {Volume}%";
        }
        private void LoadFile()
        {
            try
            {
                string[] readed;
                string[] readed_;
                using (StreamReader reader = new StreamReader("lieder_aray.txt"))
                {
                    string reader_str = reader.ReadToEnd();
                    readed = reader_str.Split('\n');
                    sender_array = readed[0].Split(";");
                    for(int i=88; i < 108; i++)
                    {
                        readed_ = readed[i-87].Split(";");

                        sender_dic_[i][0] = readed_[1];
                        sender_dic_[i][1] = readed_[2];
                    }
                }
            }
            catch (Exception e)
            {
                sender_dic_ = new Dictionary<int, string[]>  {
                {88, ["Kronehit", "https://www.radio.at/s/kronehit.com"] },
                {89, ["Hitradio Ö3", "https://orf-live.ors-shoutcast.at/oe3-q1a"] },
                {90, ["FM4", "https://orf-live.ors-shoutcast.at/fm4-q1a"] },
                {91, ["Antenne Vorarlberg", "https://web.radio.antennevorarlberg.at/av-live/stream/mp3"] },
                {92, ["Radio Vorarlberg", "https://orf-live.ors-shoutcast.at/vbg-q2a"] },
                {93, ["Bayern 3", "https://dispatcher.rndfnk.com/br/br3/live/mp3/low"] },
                {94, ["Ö1", "https://orf-live.ors-shoutcast.at/oe1-q1a"] },
                {95, ["Radio Klassik Stephansdom", "https://stream.radioklassik.at/live/mp3"] },
                {96, ["Radio Steiermark", "https://orf-live.ors-shoutcast.at/stm-q1a"] },
                {97, ["Antenne Bayern", "https://stream.antenne.de/antenne"] },
                {98, ["Bayern 1", "https://dispatcher.rndfnk.com/br/br1/live/mp3/low"] },
                {99, ["SWR3", "https://liveradio.swr.de/sw282p3/swr3/play.mp3"] },
                {100, ["Nanoq FM", "https://streamer.radio.co/s96954f0e3/listen"] },
                {101, ["Rock Antenne", "https://stream.rockantenne.de/rockantenne"] },
                {102, ["SRF 3", "https://stream.srg-ssr.ch/m/drs3/mp3_128"] },
                {103, ["SRF 1", "https://stream.srg-ssr.ch/m/drs1/mp3_128"] },
                {104, ["Hitradio Antenne 1", "https://stream.antenne1.de/a1stg/livestream2.mp3"] },
                {105, ["Radio Swiss Pop", "https://stream.srg-ssr.ch/m/rsp/mp3_128"] },
                {106, ["Radio Wien", "https://orf-live.ors-shoutcast.at/wie-q1a"] },
                {107, ["Radio Burgenland", "https://orf-live.ors-shoutcast.at/bgl-q1a"] },
                {108, ["SRF Virus", "https://stream.srg-ssr.ch/m/drsvirus/mp3_128"] }
            };
            }

        }
    }
}
