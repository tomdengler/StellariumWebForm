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
        string lastOtherFolder;
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
            UpdateStellariumCurrentView();

        }

        private void UpdateStellariumCurrentView()
        {
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
            UpdateStellariumRotation();
        }

        private void UpdateStellariumRotation()
        {
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
            fileDialog.Filter= "CR2 files|*.cr2|All files (*.*)|*.*";
            fileDialog.FilterIndex = 1;

            if (lastOtherFolder != "")
                fileDialog.InitialDirectory = lastOtherFolder;
            else
                fileDialog.InitialDirectory = textBoxWatchFolder.Text;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBoxFileToProcess.Text = fileDialog.FileName;
                lastOtherFolder = Path.GetDirectoryName(fileDialog.FileName);
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

            lastOtherFolder = "";
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

            if (checkBoxFov.Checked)
            {
                argList.Add("-fov " + textBoxFov.Text);
            }

            if (checkBoxRA.Checked)
                argList.Add("-ra " + textBoxRAhrs.Text);

            if (checkBoxDec.Checked)
                argList.Add("-spd " + textBoxDec.Text);

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
                        textBoxRunAstapResults.AppendText("\r\nastap timed out before completing");
                        return;
                    }
                    else
                    {
                        //Process was not responding; force the process to close.
                        p.Kill();
                        textBoxRunAstapResults.AppendText("\r\nastap process killed because itwas not responding");
                        return;
                    }
                }
                else
                    textBoxRunAstapResults.AppendText("\r\nastap completed");

            }
            else
            {
                p.WaitForExit();
                textBoxRunAstapResults.AppendText("\r\nastap completed");
            }

            if (File.Exists(dstWcsFilename))
            {
                string fileText = File.ReadAllText(dstWcsFilename);
                textBoxRunAstapResults.AppendText("\n\r" + fileText);

            }

            if (File.Exists(dstIniFilename))
            {
                string iniFileText = File.ReadAllText(dstIniFilename);
                textBoxRunAstapResults.AppendText("\n\r"+dstIniFilename);
                Dictionary<String, String> iniDict = processIniFile(dstIniFilename);

                if (iniDict["PLTSOLVD"]!="T")
                {
                    textBoxRunAstapResults.AppendText("\r\n\r\nPlate solver failed");
                    return;
                }
                double RA = Convert.ToDouble(iniDict["CRVAL1"]);
                double dec = Convert.ToDouble(iniDict["CRVAL2"]);
                double rot = Convert.ToDouble(iniDict["CROTA2"]);

                textBoxRAhrs.Text = Math.Round(RA / 15.0, 2).ToString();
                textBoxDec.Text = Math.Round(dec +90.0, 2).ToString();

                textBoxRunAstapResults.AppendText("\r\n\r\nRA: " + RA);
                textBoxRunAstapResults.AppendText("\r\ndec: " + dec);
                textBoxRunAstapResults.AppendText("\r\nrot: " + rot);


                RA *=  Math.PI/ 180;
                dec *= Math.PI / 180;
                rot = -(180 + rot);

                string xyz = GetStellariumXYZ(RA, dec);
                textBoxRunAstapResults.AppendText("\r\n\r\n"+xyz);
                textBoxSetCurrentView.Text = xyz;
                textBoxSetRotation.Text = "" + rot;

                if (checkBoxAutoSet.Checked)
                {
                    UpdateStellariumCurrentView();
                    UpdateStellariumRotation();
                }


            }
            else
            {
                textBoxRunAstapResults.AppendText("\r\nIni file not found (????)" + dstIniFilename);
            }
        }

        private string GetStellariumXYZ(double RA, double dec)
        {
            double x = Math.Cos(dec) * Math.Cos(RA);
            double y = Math.Cos(dec) * Math.Sin(RA);
            double z = Math.Sin(dec);
            string xyz = "[" + x + "," + y + "," + z + "]";
            return xyz;
        }

        private static Dictionary<string, string>  processIniFile(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var dict = new Dictionary<string, string>();

            foreach (var s in lines)
            {
                var split = s.Split("=");
                dict.Add(split[0], split[1]);
            }

            return dict;
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

        private void checkBoxNightVision_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNightVision.Checked)
            {
                ChangeColorScheme(1);
            }
            else
                ChangeColorScheme(0);
        }

        private void ChangeColorScheme(int v)
        {

            Color newBGColor = Color.Maroon;
            Color newFGColor = Color.Orange;

            if (v == 1)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.BackColor = newBGColor;
                this.ForeColor = newFGColor;
                textBoxRunAstapResults.ScrollBars = ScrollBars.None;

                BorderStyle bs = BorderStyle.FixedSingle;
                UpdateBorderStyles(bs,FlatStyle.Flat);

                foreach (Control c in this.Controls)
                {
                    UpdateColorControls(c, newBGColor, newFGColor);
                }
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.BackColor = Form1.DefaultBackColor;
                this.ForeColor = Form1.DefaultForeColor;
                textBoxRunAstapResults.ScrollBars = ScrollBars.Vertical;

                BorderStyle bs = BorderStyle.Fixed3D;
                UpdateBorderStyles(bs,FlatStyle.Standard);

                foreach (Control c in this.Controls)
                {
                    UpdateColorControlsDefault(c);
                }
            }

            
        }

        private void UpdateBorderStyles(BorderStyle bs, FlatStyle fs)
        {
            TextBoxCurrentView.BorderStyle = bs;
            textBoxSetCurrentView.BorderStyle = bs;
            textBoxWatchFolder.BorderStyle = bs;
            textBoxSetRotation.BorderStyle = bs;
            textBoxJ2000.BorderStyle = bs;
            textBoxJNow.BorderStyle = bs;
            textBoxAltAz.BorderStyle = bs;
            textBoxRADec.BorderStyle = bs;
            textBoxFileToProcess.BorderStyle = bs;
            textBoxRunAstapResults.BorderStyle = bs;
            textBoxFov.BorderStyle = bs;
            textBoxRAhrs.BorderStyle = bs; 

            ButtonCurrentView.FlatStyle = fs;
            buttonSetCurrentView.FlatStyle = fs;
            buttonSetRotation.FlatStyle = fs;
            buttonWatchFolder.FlatStyle = fs;
            buttonMostRecent.FlatStyle = fs;
            buttonOther.FlatStyle = fs;
            button1.FlatStyle = fs;

            checkBoxNightVision.FlatStyle = fs;
            checkBoxFov.FlatStyle = fs;
            checkBoxRA.FlatStyle = fs;

        }

        public void UpdateColorControls(Control myControl,Color newBGColor, Color newFGColor)
        {
            myControl.BackColor = newBGColor;
            myControl.ForeColor = newFGColor;
            foreach (Control subC in myControl.Controls)
            {
                UpdateColorControls(subC,newBGColor, newFGColor);
            }
        }

        public void UpdateColorControlsDefault(Control myControl)
        {
            myControl.BackColor = default(Color);
            myControl.ForeColor = default(Color);

            foreach (Control subC in myControl.Controls)
            {
                UpdateColorControlsDefault(subC);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonView_Click(object sender, EventArgs e)
        {
            OpenWithDefaultProgram(textBoxFileToProcess.Text);
        }

        public static void OpenWithDefaultProgram(string path)
        {
            using Process fileopener = new Process();

            fileopener.StartInfo.FileName = "explorer";
            fileopener.StartInfo.Arguments = "\"" + path + "\"";
            fileopener.Start();
        }
    }
}