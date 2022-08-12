using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Web;

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
            TextBoxCurrentView.Text = GetCurrentView();
            
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
            textBoxResponse.Text = "";

            string urlHost = "http://localhost:8090";
            string currentViewService = "/api/main/view";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlHost + currentViewService);
            request.Method = "POST";

            NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            outgoingQueryString.Add("j2000", textBoxSetCurrentView.Text);           
            string postData = outgoingQueryString.ToString();
            // string postData = "j2000=[0.242646, -0.726082, -0.643373]";

            // ASCIIEncoding ascii = new ASCIIEncoding();
            // byte[] postBytes = ascii.GetBytes(postData.ToString());
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
            textBoxResponse.Text = responseFromServer;
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();

        }

        private void buttonSetRotation_Click(object sender, EventArgs e)
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
            textBoxResponse.Text = responseFromServer;
            Console.WriteLine(responseFromServer);
            reader.Close();
            dataStream.Close();
            response.Close();

        }
    }
}