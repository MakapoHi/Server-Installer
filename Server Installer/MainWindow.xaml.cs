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
            string currentDir = Directory.GetCurrentDirectory();
            string steamCmdDir = String.Concat(currentDir, "\\steamcmd");

            if (Directory.Exists(steamCmdDir))
                Directory.Delete(steamCmdDir, true);
            Directory.CreateDirectory(steamCmdDir);

            string steamCmdZipDir = String.Concat(steamCmdDir, "\\steamcmd.zip");
            string steamCmdExeDir = String.Concat(steamCmdDir, "\\steamcmd.exe");

            if (!File.Exists(steamCmdZipDir))
            {
                string steamCmdUrl = "https://steamcdn-a.akamaihd.net/client/installer/steamcmd.zip";

                WebClient webClient = new WebClient();
                webClient.DownloadFile(steamCmdUrl, String.Concat(steamCmdDir, "\\steamcmd.zip"));
            }
            if (!File.Exists(steamCmdExeDir))
                ZipFile.ExtractToDirectory(steamCmdZipDir, steamCmdDir);

            File.Delete(steamCmdZipDir);

            Start_SteamCmd(steamCmdExeDir, steamCmdDir);
        }

        private void Start_SteamCmd(string steamCmd_Dir = "", string steamCmdDir = "")
        {
            string scriptDir = String.Concat(steamCmdDir, "\\steamcmd_script.txt");

            StreamWriter streamWriter = new StreamWriter(scriptDir, false, Encoding.Default);
            streamWriter.Write("@ShutdownOnFailedCommand 0\n" +
                                "login anonymous\n" +
                                "force_install_dir .\\server\\\n" +
                                "app_update 10\n" +
                                "app_update 70\n" +
                                "app_update 90 -beta beta validate\n" +
                                "quit");
            streamWriter.Close();

            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.Equals("steamcmd"))
                {
                    process.Kill();
                    break;
                }
            }
            Process.Start(steamCmd_Dir, "+runscript steamcmd_script.txt");
        }
    }
}
