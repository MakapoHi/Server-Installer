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
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;

namespace Server_Installer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum VersionAmx { None, OneNineTwo, OneNineThree, OneNineThreeDev };

        public int SelectedAmx { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton button = (RadioButton)sender;

            if (button.Content.ToString() == "Amx Mod X 1.8.2")
                SelectedAmx = (int)VersionAmx.OneNineTwo;
            else if (button.Content.ToString() == "Amx Mod X 1.8.3")
                SelectedAmx = (int)VersionAmx.OneNineThree;
            else if (button.Content.ToString() == "Не устанавливать")
                SelectedAmx = (int)VersionAmx.None;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string CurrentDir = Directory.GetCurrentDirectory();
            string SteamCmdZipDir = String.Concat(CurrentDir, "\\steamcmd.zip");
            string SteamCmdExeDir = String.Concat(CurrentDir, "\\steamcmd.exe");

            if (!File.Exists(SteamCmdZipDir))
            {
                string SteamCmdUrl = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";

                WebClient webClient = new WebClient();
                webClient.DownloadFile(SteamCmdUrl, "steamcmd.zip");
            }
            if (!File.Exists(SteamCmdExeDir))
                ZipFile.ExtractToDirectory(SteamCmdZipDir, CurrentDir);

            File.Delete(SteamCmdZipDir);

            Start_SteamCmd(SteamCmdExeDir);
        }

        private void Start_SteamCmd(string SteamCmd_Dir = "")
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.Equals("steamcmd"))
                {
                    process.Kill();
                    break;
                }
            }
            Process.Start(SteamCmd_Dir);
        }
    }
}
