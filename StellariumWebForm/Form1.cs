using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Web;
using System.Text.Json;
using System.Text.Json.Nodes;


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
    }
}