using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Configuration;
using System.Diagnostics;

namespace StellariumWebForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ButtonCurrentView_Click(object sender, EventArgs e)
        {
            // ex: {"altAz":"[0.999722, 9.99722e-06, 0.201384]","j2000":"[-0.34071, 0.572616, -0.772028]","jNow":"[-0.342442, 0.571848, -0.771831]"}
            string currentView = GetCurrentView();
            TextBoxCurrentView.Text = currentView;

            JsonNode currentViewNode = JsonNode.Parse(currentView)!;
            string j2000 = currentViewNode!["j2000"].ToString();
            string jNow = currentViewNode!["jNow"].ToString();
            string altAz = currentViewNode!["altAz"].ToString();

            textBoxJ2000.Text = j2000;
            textBoxJNow.Text = jNow;
            textBoxAltAz.Text = altAz;

            string radecJ2000 = RaDecFromJsonVec3D(j2000);
            textBoxRADec.Text = radecJ2000;
        }

        static string GetCurrentView()
        {
            string urlHost = "http://localhost:8090";
            string currentViewService = "/api/main/view";

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(urlHost + currentViewService);
            HttpWebResponse response = (HttpWebResponse)myReq.GetResponse();

            Stream receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format.
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

            Console.WriteLine("Response stream received.");
            string retVal = readStream.ReadToEnd();
            Console.WriteLine(retVal);
            response.Close();
            readStream.Close();

            return retVal;
        }

        private void buttonSetCurrentView_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";

            string urlHost = "http://localhost:8090";
            string currentViewService = "/api/main/view";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlHost + currentViewService);
            request.Method = "POST";

            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            outgoingQueryString.Add("j2000", textBoxSetCurrentView.Text);           
            string postData = outgoingQueryString.ToString();
            byte[] postBytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = postBytes.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(postBytes, 0, postBytes.Length);
            dataStream.Flush();
            dataStream.Close();


            // todo: trap a bad response
            WebResponse response = request.GetResponse();
            

            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            toolStripStatusLabel1.Text = responseFromServer;
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();

        }

        private void buttonSetRotation_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "";
            string urlHost = "http://localhost:8090";
            string currentViewService = "/api/stelproperty/set";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlHost + currentViewService);
            request.Method = "POST";

            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            outgoingQueryString.Add("id", "Oculars.selectedCCDRotationAngle");
            outgoingQueryString.Add("value", textBoxSetRotation.Text);
            string postData = outgoingQueryString.ToString();

            byte[] postBytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = postBytes.Length;
            request.ContentType = "application/x-www-form-urlencoded";

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(postBytes, 0, postBytes.Length);
            dataStream.Flush();
            dataStream.Close();

            WebResponse response = request.GetResponse();
            dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            toolStripStatusLabel1.Text = responseFromServer;
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();
        }

        private string RaDecFromJsonVec3D(string jsonVec3D)
        {
            JsonNode currentViewNode = JsonNode.Parse(jsonVec3D)!;
            double x = (double)currentViewNode![0];
            double y = (double)currentViewNode![1];
            double z = (double)currentViewNode![2];

            double dec = Math.Atan2(z,Math.Sqrt(x * x + y * y));
            dec=dec*180.0/Math.PI;
            if (dec > 90)
                dec = 90 - dec;

            double RA = Math.Atan2(y, x) * 180 / Math.PI;  // RA
            if (RA < 0)
                RA += 360;
            RA /= 15;

            double DecDeg = Math.Truncate(dec);
            double DecMin = Math.Truncate(60 * (dec - DecDeg));
            double DecSec = Math.Round(60 * 60 * (dec - (DecDeg + DecMin / 60)), 2);

            double RAHours = Math.Truncate(RA);
            double RAMin = Math.Truncate(60 * (RA - RAHours)); // trunc(60*(AA18-AB18*15)/15,0)
            double RASec = Math.Round(60 * 60 * (RA - (RAHours + RAMin / 60)) , 2); // round(60*60*((AA18/15)-(AB18+AC18/60)),2)

            string RaStr = RAHours + "h" + RAMin + "m" + RASec + "s";
            string DecStr = DecDeg + "\u00b0" + DecMin + "\'" + DecSec + "\"";

            return RaStr + "/" + DecStr;
        }

        private void buttonWatchFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                AddUpdateAppSettings("WatchFolder", folderName);
                textBoxWatchFolder.Text = folderName;
            }

        }

        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        private void buttonMostRecent_Click(object sender, EventArgs e)
        {
            string watchFolder = textBoxWatchFolder.Text;
            
            if (watchFolder == "")
            {
                return;
            }

            var file = new DirectoryInfo(watchFolder).GetFiles().OrderByDescending(o => o.CreationTime).FirstOrDefault();
            textBoxFileToProcess.Text = file.FullName;
        }

        public static string GetMostRecentFile(string path)
        {
            var file = new DirectoryInfo(path).GetFiles().OrderByDescending(o => o.CreationTime).FirstOrDefault();
            return file.FullName;
        }

        private void buttonOther_Click(object sender, EventArgs e)
        {
            FileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = textBoxWatchFolder.Text;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFileToProcess.Text = fileDialog.FileName;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var appSettings = ConfigurationManager.AppSettings;

            string watchFolder = appSettings["WatchFolder"] ?? "";
            textBoxWatchFolder.Text = watchFolder;

            string astapLocation = appSettings["AstapLocation"] ?? "";
            if (astapLocation != "")
                labelAstapLocation.Text = astapLocation + "\\astap.exe";
            else
                labelAstapLocation.Text = "astap.exe location not yet set, check Settings";
        }

        private void ToolStripMenuItemAstap_Click(object sender, EventArgs e)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string astapLocation = appSettings["AstapLocation"] ?? "";
            
            FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            if (astapLocation != "")
            {
                folderBrowserDialog1.InitialDirectory = astapLocation;
            }

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                AddUpdateAppSettings("AstapLocation", folderName);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string astapLocation = appSettings["AstapLocation"] ?? "";
            string tempFolder = appSettings["TempFolder"] ?? "";

            if (astapLocation == "")
            {
                textBoxRunAstapResults.Text = "ASSTAP Location not yet set, go to settings ASTAP to set";
                return;
            }

            if (tempFolder == "")
            {
                textBoxRunAstapResults.Text = "Temp folder location not yet set, go to settings to set";
                return;
            }

            if (!Directory.Exists(tempFolder))
            {
                textBoxRunAstapResults.Text = "Temp folder does not exist, go to settings to set";
                return;
            }

            string astapExe = astapLocation + "\\astap.exe";
            if (!File.Exists(astapExe))
            {
                textBoxRunAstapResults.Text = astapExe + "not found, check the ASTAP to set";
                return;
            }

            string fileToProcess = textBoxFileToProcess.Text;

            if (fileToProcess == "")
            {
                textBoxRunAstapResults.Text = "File to process is not set";
                return;
            }

            if (!File.Exists(fileToProcess))
            {
                textBoxRunAstapResults.Text = fileToProcess + "not found.";
                return;
            }

            RunAstap(astapExe, fileToProcess, tempFolder);

        }

        private void RunAstap(string astapExe, string fileToProcess, string tempFolder)
        {
            //string exeCommandLine = astapExe + " -f " + fileToProcess;
            //textBoxRunAstapResults.Text = exeCommandLine;

            // example call: "C:\Program Files\astap\astap.exe"  -ra 16.174166671 -spd 124.560 -fov 0.85
            // // -z 0 -r 30 -s 500 -f "C:\APT_Images\Camera_1\TemporaryStorage"\ImageToSolve.CR2

            string srcFilename = Path.GetFileName(fileToProcess);
            string dstFileExt = Path.GetExtension(fileToProcess);
            string dstFilename = Path.Combine(tempFolder, "FileToProcess"+dstFileExt);
            string dstIniFilename = Path.Combine(tempFolder, "FileToProcess.ini");
            string dstWcsFilename = Path.Combine(tempFolder, "FileToProcess.wcs");

            File.Copy(fileToProcess, dstFilename, true);
            File.Delete(dstIniFilename);
            File.Delete(dstWcsFilename);

            List<String> argList = new List<String>();
            argList.Add("-f " + dstFilename);
            string args = String.Join(" ",argList.ToArray());

            string exeCommandLine = astapExe + " " + args;
            textBoxRunAstapResults.Text = exeCommandLine;

            int timeout = 1000 * 60;

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.Arguments = String.Join(" ", args);
            psi.FileName = astapExe;
            Process p = Process.Start(psi);
            p.WaitForInputIdle();

            if (timeout != 0)
            {
                p.WaitForExit(timeout);
                if (p.HasExited == false)
                {
                    //Process is still running.
                    //Test to see if the process is hung up.
                    if (p.Responding)
                    {
                        //Process was responding; close the main window.
                        p.CloseMainWindow();
                        textBoxRunAstapResults.AppendText("astap timed out before completing");
                        return;
                    }
                    else
                    {
                        //Process was not responding; force the process to close.
                        p.Kill();
                        textBoxRunAstapResults.AppendText("astap process killed because itwas not responding");
                        return;
                    }
                }
                else
                    textBoxRunAstapResults.AppendText("astap completed");

            }
            else
            {
                p.WaitForExit();
                textBoxRunAstapResults.AppendText("astap completed");
            }

            if (File.Exists(dstIniFilename))
            {
                string iniFileText = File.ReadAllText(dstIniFilename);
                textBoxRunAstapResults.AppendText(iniFileText);
            }
            else
            {
                textBoxRunAstapResults.AppendText("\nIni file not found (????)"+ dstIniFilename);
            }
        }

        private void tempFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var appSettings = ConfigurationManager.AppSettings;
            string tempFolder = appSettings["TempFolder"] ?? "";

            FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            if (tempFolder != "")
            {
                folderBrowserDialog1.InitialDirectory = tempFolder;
            }

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                AddUpdateAppSettings("TempFolder", folderName);
            }

        }
    }
}