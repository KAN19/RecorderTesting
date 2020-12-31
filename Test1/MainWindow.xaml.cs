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
using Microsoft.Win32;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Timers;

namespace Test1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        MediaPlayer media = new MediaPlayer();

        private string _savePath = "";

        private string _filePath;
        Timer timer1 = new Timer();

        public string filePath
        {
            get { return _filePath; }
            set {
                _filePath = value;
                OnPropertyChanged("filePath"); 
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        // Copy tren mang
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            media.Play();

            timer1.Enabled = true;
            timer1.Start();
            record("open new Type waveaudio Alias recsound", "", 0, 0);
            record("record recsound", "", 0, 0);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            media.Stop();
            timer1.Stop();
            timer1.Enabled = false;

            // Save file after record
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Audio Files | *.wav";
            save.DefaultExt = "wav";
            if (save.ShowDialog() == true)
            {
                _savePath = save.FileName;
            }

            record("save recsound " + _savePath, "", 0, 0);
            record("close recsound", "", 0, 0);
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            

            if (openFileDialog.ShowDialog() == true)
                filePath = openFileDialog.FileName;

            media.Open(new Uri(_filePath, UriKind.Relative)); 
           
        }
    }
}
