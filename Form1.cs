using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.Web.WebView2;
using Microsoft.Web.WebView2.Core;

namespace C_RayFingerNetwork
{
    /// <summary>
    /// /
    /// </summary>
    public partial class Form1 : Form
    {
        // Delegate for updating the UI controls
        private delegate void UpdateUITextDelegate(string text);
        private delegate void UpdateUIGridDelegate(string col1, string col2);
        //private System.Windows.Forms.WebView2 webView21;
        private StringWriter consoleOutput;
        MessageServer scannernetwork = new MessageServer();

        public Form1()
        {
            InitializeComponent();

            // Redirect the console output to a StringWriter
            consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            dataGridView1.Columns.Add("Column1", "Column 1");
            dataGridView1.Columns.Add("Column2", "Column 2");
            Controls.Add(dataGridView1);

            // Start separate threads for updating TextBox and DataGridView
            Thread textBoxThread = new Thread(UpdateTextBox);
            Thread gridThread = new Thread(UpdateDataGridView);

            textBoxThread.Start();
            gridThread.Start();

            ListIP_Address();

            this.Resize += new System.EventHandler(this.Form_Resize);
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);
            goButton.Left = this.ClientSize.Width - goButton.Width;
            addressBar.Width = goButton.Left - addressBar.Left;
        }

        void ListIP_Address()
        {
            List<string> ipAddresses = GetLocalIPAddress();

            // Sort the list based on the specified criteria
            ipAddresses.Sort(new IPAddressComparer());

            // Assuming you have a ComboBox named comboBoxIPAddresses
            foreach (var ipAddress in ipAddresses)
            {
                comboBoxIPAddresses.Items.Add(ipAddress);
            }

            if (comboBoxIPAddresses.Items.Count > 0)
            {
                comboBoxIPAddresses.SelectedIndex = 0;
            }

            // Handle ComboBox selection events as needed
            comboBoxIPAddresses.SelectedIndexChanged += (sender, e) =>
            {
                string selectedIPAddress = comboBoxIPAddresses.SelectedItem.ToString();
                Console.WriteLine("Selected IP address: " + selectedIPAddress);
            };
        }


        static List<string> GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddresses = host.AddressList
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .Select(ip => ip.ToString())
                .ToList();

            return ipAddresses;
        }

        // Method to update TextBox in a separate thread
        private void UpdateTextBox()
        {
            for (int i = 1; i <= 10; i++)
            {
                // Simulate some processing
                Thread.Sleep(1000);

                // Update TextBox using Invoke
                try
                {
                    Invoke(new UpdateUITextDelegate(UpdateTextBoxUI), $"Update {i} for TextBox");
                }
                catch (Exception er)
                {

                }
                
            }
        }

        // Method to update DataGridView in a separate thread
        private void UpdateDataGridView()
        {
            for (int i = 1; i <= 10; i++)
            {
                // Simulate some processing
                Thread.Sleep(1500);

                // Update DataGridView using Invoke
                try
                {
                    Invoke(new UpdateUIGridDelegate(UpdateDataGridViewUI), $"Row {i}", $"Value {i}");
                }
                catch(Exception er)
                {

                }
                
            }
        }

        // Method to update TextBox on the UI thread
        private void UpdateTextBoxUI(string text)
        {
            textBox1.Text = text;
        }

        // Method to update DataGridView on the UI thread
        private void UpdateDataGridViewUI(string col1, string col2)
        {
            dataGridView1.Rows.Add(col1, col2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConsoleReader.StartReading(UpdateConsoleRichTextBox);

            scannernetwork.MessageService();
        }

        private void UpdateConsoleRichTextBox(string text, Color? textColor = null)
        {
            // Update the RichTextBox with the console output
            if (richTextBoxConsole.InvokeRequired)
            {
                richTextBoxConsole.Invoke(new Action(() => UpdateConsoleRichTextBox(text, textColor)));
            }
            else
            {
                richTextBoxConsole.SelectionStart = richTextBoxConsole.TextLength;
                richTextBoxConsole.SelectionLength = 0;

                if (textColor.HasValue)
                {
                    richTextBoxConsole.SelectionColor = textColor.Value;
                }

                richTextBoxConsole.AppendText(text);
                richTextBoxConsole.SelectionColor = richTextBoxConsole.ForeColor;

                richTextBoxConsole.ScrollToCaret();
            }
        }

        private void WebViewInit()
        {
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConsoleReader.StopReading();
            scannernetwork.Stop();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                try
                {
                    webView.CoreWebView2.Navigate(ValidateURL(addressBar.Text));
                }
                catch(Exception ex)
                {
                    ShowWarningDialog(ex.Message);
                }
                
            }
        }
        //
        private void ShowWarningDialog(string message)
        {
            MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private string ValidateURL(string input)
        {
            if (IsValidWebAddress(input))
            {
                Console.WriteLine(input + " Valid address detected..");
                return input;
            }

            if (!input.StartsWith("http://"))
            {
                if (IsIPAddress(input))
                {
                    Console.WriteLine("repairing Address");
                    return "http://" + input;
                }
                
                // Throw an error or display a warning
                MessageBox.Show("Invalid URL format. Please start with 'http://' and you can use IP addresses.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("Invalid URL format. Please start with 'http://' and you can use IP addresses.", "Error");
                Console.WriteLine("Loading default IP...");
                string addr = "http://192.168.0.1";
                addressBar.Text = addr;
                return addr;
            }
            Console.WriteLine("Loading Google address...");
            string addr1 = "https://goodle.com";
            addressBar.Text = addr1;
            return addr1;
        }

        public static bool IsValidWebAddress(string input)
        {
            // Try to create a Uri from the input
            if (Uri.TryCreate(input, UriKind.Absolute, out Uri uriResult))
            {
                // Check if the Uri scheme is http or https
                return uriResult.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) ||
                       uriResult.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        private bool IsIPAddress(string input)
        {
            // Check if the input contains a valid IP address
            if (IPAddress.TryParse(input, out IPAddress ipAddress))
            {
                return ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork;
            }

            return false;
        }
    }
    public static class ConsoleReader
    {
        private static Action<string, Color?> callback;
        private static TextWriter originalOutput;

        public static void StartReading(Action<string, Color?> updateCallback)
        {
            callback = updateCallback;
            originalOutput = Console.Out;

            // Redirect the console output to the specified callback
            Console.SetOut(new CallbackTextWriter(callback));
        }

        public static void StopReading()
        {
            // Restore the original console output
            Console.SetOut(originalOutput);
        }
    }

    public class CallbackTextWriter : TextWriter
    {
        private readonly Action<string, Color?> callback;

        public CallbackTextWriter(Action<string, Color?> callback)
        {
            this.callback = callback;
        }

        public override void Write(char value)
        {
            callback?.Invoke(value.ToString(), null);
        }

        public override void Write(string value)
        {
            callback?.Invoke(value, null);
        }

        public override Encoding Encoding => Encoding.Default;
    }

    public class MessageServer
    {
        public MessageServer()
        {

        }

        static bool saveImage = true;
        // TCP server port
        static int port = 3000;
        public static ChatServer messageServer = new ChatServer(IPAddress.Any, port);

        // Initialize the database handler
        public static DatabaseHandler dbHandler = new DatabaseHandler("databasefile1.db");
        //Check Database for finger print Match exists
        public static IEnumerable<Subject> candidates = new List<Subject>();

        public static double threshold = ((double)71.00 / (double)100.00) * (double)31.89; // 22; //exact threshold 31.89

        public void MessageService()
        {

            Console.WriteLine("Initialising Finterprint Network Service");

            //Console.WriteLine($"FINGER server port: {port}");

            // Create a new TCP chat server
            messageServer = new ChatServer(IPAddress.Any, port);

            // Start the server
            Console.Write("Server starting...");
            messageServer.Start();

            Console.WriteLine("Server Started!: " + messageServer.Address);
            //string ipAddress = GetLocalIPAddress();
            //Console.WriteLine("Server is listening on IP: " + ipAddress);
            

            Console.WriteLine("Loading Saved FingerPrint Data....");
            candidates = (IEnumerable<Subject>)dbHandler.LoadDB(); //load DB once on startup//

            Console.WriteLine("Finger Print Database Loaded Successully");

            Console.WriteLine("Match Threshold: " + threshold);
        }

        public void Stop()
        {
            //Stop the server
            Console.Write("Server stopping...");
            messageServer.Stop();
            Console.WriteLine("Done!");
        }

        public void BroadCast()
        {
            // Multicast admin message to all sessions
            messageServer.Multicast("(admin) ");
        }

        private Form1 mainform;

        public MessageServer(Form1 form)
        {
            mainform = form;
        }

        
    }

    // Custom comparer to sort IP addresses based on the specified criteria
    public class IPAddressComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            return GetPriority(x).CompareTo(GetPriority(y));
        }

        private int GetPriority(string ipAddress)
        {
            if (ipAddress.StartsWith("192.168"))
                return 0;
            if (ipAddress.StartsWith("10"))
                return 1;
            if (ipAddress.StartsWith("172.16") || ipAddress.StartsWith("172.17") || ipAddress.StartsWith("172.18") || ipAddress.StartsWith("172.19") ||
                ipAddress.StartsWith("172.20") || ipAddress.StartsWith("172.21") || ipAddress.StartsWith("172.22") || ipAddress.StartsWith("172.23") ||
                ipAddress.StartsWith("172.24") || ipAddress.StartsWith("172.25") || ipAddress.StartsWith("172.26") || ipAddress.StartsWith("172.27") ||
                ipAddress.StartsWith("172.28") || ipAddress.StartsWith("172.29") || ipAddress.StartsWith("172.30") || ipAddress.StartsWith("172.31"))
                return 2;

            return 3; // Default priority if none of the specified criteria match
        }
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="serializedFingerTemplate"></param>
    public record SubjectProbe(int Id, string Name, byte[] serializedFingerTemplate) { }
    public record Subject(int Id, string Name, byte[] Template) { }

    class ChatSession : TcpSession
    {
        public ChatSession(TcpServer server) : base(server) { }

        Dictionary<string, List<byte>> keyValueData = new Dictionary<string, List<byte>>();

        protected override void OnConnected()
        {
            //Console.WriteLine($"Chat TCP session with Id {Id} connected!");            
            try
            {
                keyValueData.Add(Id.ToString(), new List<byte>());
                Console.WriteLine($"Key {Id} Created");
                // Send invite message
                string message = "Connected!";
                SendAsync(message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("Id Key already exists caught: " + e.Message);
            }
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"Chat TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Console.WriteLine("Incoming: " + message);
            Console.WriteLine("Size of Data: " + (int)size);
            Console.WriteLine($"Current ID: {Id} ");

            if (keyValueData.TryGetValue(Id.ToString(), out List<byte> inheritedList))
            {
                var dt = buffer.Length;
                Console.WriteLine("Data Lenght: " + dt);
                Console.WriteLine("Inherited List: " + inheritedList.Count);

                for (int i = 0; i < (int)size; i++)
                {
                    inheritedList.Add(buffer[i]);
                }

                keyValueData[Id.ToString()] = new List<byte>(inheritedList);

                if (inheritedList.Count == 36864)
                {
                    //
                    Console.WriteLine("Done getting full fingerprint data. Counted: " + inheritedList.Count);

                    //FingerPrint fp = new FingerPrint();
                    //fp.GetImage(Id, inheritedList.ToArray());
                }
                else
                {
                    Console.WriteLine("Number of Inherited Count: " + inheritedList.Count);
                }
            }
            else
            {
                Console.WriteLine("Key not found: ");
            }

            // Multicast message to all connected sessions
            //Server.Multicast(message);

            // If the buffer starts with '!' the disconnect the current session
            if (message == "!")
                Disconnect();
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP session caught an error with code {error}");
        }
    }

    public class ChatServer : TcpServer
    {
        public ChatServer(IPAddress address, int port) : base(address, port) { }

        protected override TcpSession CreateSession() { return new ChatSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP server caught an error with code {error}");
        }
    }
}