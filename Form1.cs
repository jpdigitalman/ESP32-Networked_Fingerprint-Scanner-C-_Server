using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.Web.WebView2;
using Microsoft.Web.WebView2.Core;
using System;
using static C_RayFingerNetwork.Form1;
using static System.Net.Mime.MediaTypeNames;
using System.Timers;
using System.Data;
using Newtonsoft.Json;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using System.Buffers.Text;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text.Json;

/*
 *  All rights reserved.
 *  You are granted the right to modify and use this code for your purposes.
 *  Author: John-Paul Madueke
 *  
 *  
 *  
 */


namespace C_RayFingerNetwork
{
    /// <summary>
    /// /
    /// </summary>
    public partial class Form1 : Form
    {
        // Delegate for updating the UI controls
        public delegate void UpdateUITextDelegate(string text);
        private delegate void UpdateUIGridDelegate(Dictionary<string, string> connectedScanners);
        //private System.Windows.Forms.WebView2 webView21;
        private StringWriter consoleOutput;
        //MessageServer scannernetwork = new MessageServer();
        private ChatServer chatServer;
        Messenger msg = new Messenger();

        public Form1()
        {
            InitializeComponent();

            // Redirect the console output to a StringWriter
            consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);
                     
            ///
            System.Timers.Timer timer = new System.Timers.Timer(3500);
            timer.Elapsed += UpdateTextBox;
            Thread timerThread = new Thread(() => timer.Start());
            timerThread.Start();
            ///
            System.Timers.Timer timergrid = new System.Timers.Timer(1500);
            timergrid.Elapsed += UpdateDataGridView;
            Thread gridThread = new Thread(() => timergrid.Start());
            gridThread.Start();


            ListIP_Address();

            this.Resize += new System.EventHandler(this.Form_Resize);
            // Set the event handler for navigation completed
            InitializeAsync();

            //dataGridView1.Columns.Add("Column1", "Column 1");
            //dataGridView1.Columns.Add("Column2", "Column 2");
            //Controls.Add(dataGridView1);
        }

        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            //webView.CoreWebView2.WebMessageReceived += UpdateAddressBar;
            //await webView.CoreWebView2.ExecuteScriptAsync($"alert('')");
            //await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            //await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
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
        private void UpdateTextBox(object sender, ElapsedEventArgs e)
        {
            try
            {
                int i = Messenger.numberofsavedtemplates;
                Invoke(new UpdateUITextDelegate(UpdateTextBoxUI), $"Number of Fingerprints {i}");

            }
            catch (Exception er)
            {

            }
        }

        // Method to update DataGridView in a separate thread
        private void UpdateDataGridView(object sender, ElapsedEventArgs e)
        {
            try
            {
                //Console.WriteLine("ticking..");
                Invoke(new UpdateUIGridDelegate(UpdateDataGridViewUI), Messenger.connectedScanners);
            }
            catch (Exception er)
            {

            }

            msg.LoadData();
            Messenger.numberofsavedtemplates = msg.GetItemCount();
            try
            {
                Invoke(new UpdateUITextDelegate(UpdateTextBoxUI), $"Number of Fingerprint Found {Messenger.numberofsavedtemplates}");
            }
            catch (Exception er)
            {

            }
        }

        // Method to update TextBox on the UI thread
        public void UpdateTextBoxUI(string text)
        {
            if (textBoxDatabaseCounter.InvokeRequired)
            {
                // If called from a different thread, invoke it on the UI thread
                textBoxDatabaseCounter.Invoke(new Action(() => UpdateTextBoxUI(text)));
            }
            else
            {
                // Update the TextBox directly
                textBoxDatabaseCounter.Text = text;
            }
        }

        // Method to update DataGridView on the UI thread
        private void UpdateDataGridViewUI(Dictionary<string, string> connectedScanners)
        {
            dataGridView1.DataSource = new BindingSource(connectedScanners, null);
            dataGridView1.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConsoleReader.StartReading(UpdateConsoleRichTextBox);

            //scannernetwork.MessageService();
            Console.Write("Server starting...");
            chatServer = new ChatServer(IPAddress.Any, 3000);
            chatServer.Start();
            Console.Write("Server started...");
            
            Messenger.numberofsavedtemplates = msg.GetItemCount();
            Invoke(new UpdateUITextDelegate(UpdateTextBoxUI), $"Number of Fingerprint Found {Messenger.numberofsavedtemplates}");

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
            chatServer.Stop();
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                try
                {
                    webView.CoreWebView2.Navigate(ValidateURL(addressBar.Text));
                    //Console.WriteLine("Navigating to " + addressBar.Text + "....");
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

        private void webView_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            // Read the current URL
            string currentUrl = e.Uri;// addressBar.Text;// webView.CoreWebView2.Source; //webView.Source.ToString();

            // Set the current URL to the TextBox using Invoke for thread synchronization
            addressBar.Invoke((MethodInvoker)delegate
            {
                addressBar.Text = currentUrl;
            });
            Console.WriteLine("Navigating to " + currentUrl + "....");
        }

        private void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            // Read the current URL
            string currentUrl = webView.CoreWebView2.Source; //webView.Source.ToString();

            // Set the current URL to the TextBox using Invoke for thread synchronization
            addressBar.Invoke((MethodInvoker)delegate
            {
                addressBar.Text = currentUrl;
            });
            Console.WriteLine("Navigated to " + currentUrl + "....");
        }

        private void addressBar_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key is pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Programmatically click the "Go" button
                goButton.PerformClick();

                // Suppress the Enter key to prevent it from being processed further
                e.SuppressKeyPress = true;
            }
        }

        //webView.CoreWebView2.AddWebMessageReceivedHandler(HandleWebMessageReceived);
        private void webView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            // Handle the received message from the HTML page
            string message = e.TryGetWebMessageAsString();
            //MessageBox.Show($"Message received from HTML: {message}", "Host Application");
            Console.WriteLine(message);
            if(message == "digitalman")
            {
                // Send a message to the WebView2 page and set the value of the textbox
                //webView.CoreWebView2.ExecuteScriptAsync("window.chrome.webview.postMessage('Thanks from Host!');");
                string newip = comboBoxIPAddresses.Text;
                webView.CoreWebView2.ExecuteScriptAsync("document.querySelector('input[name=\"textbox\"]').value = '" + newip + "';");
            }
            else
            {
                Console.WriteLine("Invalid Response from webpage");
            }
            
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


    public class ChatSession : TcpSession
    {
        Messenger messenger = new Messenger();
        public ChatSession(TcpServer server) : base(server)
        {
            
        }

        private void UpdateTextBox(object? sender, ElapsedEventArgs e)
        {
            Console.WriteLine("bingo..");
            Send("1");
        }

        void timing()
        {
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.Elapsed += UpdateTextBox;
            Thread timerThread = new Thread(() => timer.Start());
            timerThread.Start();
        }

        protected override void OnConnected()
        {
            //Console.WriteLine($"Chat TCP session with Id {Id} connected!");            
            try
            {
                //timing();
                Messenger.connectedScanners.Add(Id.ToString(), "");
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
            try
            {
                Messenger.connectedScanners.Remove(Id.ToString());
            }
            catch(Exception x)
            {
                Console.WriteLine(x.Message);
            }
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Console.WriteLine("Incoming Command: " + message);
            Console.WriteLine("Size of Data: " + (int)size);
            Console.WriteLine($"Current ID: {Id} ");

            if(size == 900 )
            {
                Console.WriteLine("saving to database");
                messenger.AddNewItem(buffer);
                messenger.LoadData();
                return;
            }

            if (message.Contains("scanner_name"))
            {
                var scannername = message.Replace("scanner_name: ", "");
                Console.WriteLine("Scanner Name: " + scannername);
                if (Messenger.connectedScanners.ContainsKey(Id.ToString()))
                {
                    Messenger.connectedScanners[Id.ToString()] = scannername;
                    Console.WriteLine($"Key {scannername} Added");
                }
                else
                {
                    Messenger.connectedScanners.Add(Id.ToString(), scannername);
                    Console.WriteLine($"Key {Id} Created");
                    Console.WriteLine($"Key {scannername} Added");
                }
                return;
            }
            else if (message.Contains("GET_DATA="))
            {

                int db_count = messenger.GetItemCount();

                for(int i = 0; i < db_count; i++)
                {
                    Send(messenger.GetItemById(i), 0, 900);
                }

                /*
                int valx;
                int.TryParse(message.Replace("GET_DATA=", ""), out valx);
                Send(messenger.GetItemById(valx), 0, 900);
                */
                /*
                int startIndex, stopIndex;
                // Extract the part after the '=' sign
                string dataPart = message.Substring(message.IndexOf('=') + 1);
                dataPart = dataPart.Trim();
                string[] values = dataPart.Split(',');
                if (values.Length == 2)
                {
                    if (int.TryParse(values[0], out startIndex) && int.TryParse(values[1], out stopIndex))
                    {
                        Console.WriteLine($"Original String: {message}");
                        Console.WriteLine($"Extracted Values: {values[0]} and {values[1]}");
                        Console.WriteLine($"First Integer: {startIndex}");
                        Console.WriteLine($"Second Integer: {stopIndex}");

                        //Send(messenger.GetItemById(0), 0, 900);
                        //List<byte[]> lst = new List<byte[]>();
                        //Console.WriteLine(BitConverter.ToString(messenger.GetItemById(0)));

                        List<string> base64List = new List<string>();

                        for (int s = startIndex; s < stopIndex; s++)
                        {
                            string base64String = Convert.ToBase64String(messenger.GetItemById(s));
                            base64List.Add(base64String);

                            //lst.Add(messenger.GetItemById(0));
                            //Send(messenger.GetItemById(0), 0, 900);
                            //Console.WriteLine($"Data sent: {s}");
                        }

                        // Serialize the List<string> to JSON
                        string json = System.Text.Json.JsonSerializer.Serialize(base64List);

                        // Convert the JSON string to bytes
                        byte[] jsonData = Encoding.UTF8.GetBytes(json);
                        Console.WriteLine("length of json: " + jsonData.Length.ToString());
                        Send(jsonData, 0, jsonData.Length);

                    }
                    else
                    {
                        Console.WriteLine("Invalid integer format in the string.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid format. Expected two values separated by a comma.");
                }
                */
            }

            if (start_template_rx == false)
            {
                DecodeCommand(message, (int)size);
            }
            else
            {
                //RxTemplate(buffer, (int)size);
                //start_template_rx = false;
            }

            // Multicast message to all connected sessions
            //Server.Multicast(message);

            // If the buffer starts with '!' the disconnect the current session
            if (message == "!!!")
                Disconnect();
        }

        

        protected override void DecodeCommand(string message, int size)
        {
            switch (message)
            {
                case "get template count":
                    Console.WriteLine("template count");
                    Messenger.numberofsavedtemplates = messenger.GetItemCount();
                    string response = "template_count=" + Messenger.numberofsavedtemplates.ToString();
                    Send(response);                    
                    break;

                // ready for template arrival
                case "start_template_rx":
                    start_template_rx = true;
                    break;

                default:
                    // Default case if none of the above conditions match
                    // This is optional, depending on your requirements
                    break;
            }
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Chat TCP session caught an error with code {error}");
        }
    }
    
    public class Messenger
    {
        private const string DatabaseFilePath = "database.json";
        private List<DataItem> data;

        public Messenger()
        {
            LoadData();
        }
        public void LoadData()
        {
            if (File.Exists(DatabaseFilePath))
            {
                string json = File.ReadAllText(DatabaseFilePath);
                data = JsonConvert.DeserializeObject<List<DataItem>>(json);
            }
            else
            {
                data = new List<DataItem>();
            }

            UpdateDataGridView();
        }
        private void UpdateDataGridView()
        {
            //dataGridView1.DataSource = null;
            //dataGridView1.DataSource = data;
        }

        private void SaveData()
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(DatabaseFilePath, json);
            LoadData();
        }
        public void shuffle()
        {
            //
        }
        public void AddNewItem(byte[] template)
        {
            // Serialize the byte array to a JSON string
            string jsontemplate = JsonConvert.SerializeObject(template);

            DataItem newItem = new DataItem { Name = "FingerTemplate", Template = jsontemplate };
            data.Add(newItem);
            SaveData();
            UpdateDataGridView();
            Console.WriteLine("Save in Database Success");
        }

        public void DeleteItemById(int id)
        {
            DataItem itemToRemove = data.FirstOrDefault(item => item.Id == id);
            if (itemToRemove != null)
            {
                data.Remove(itemToRemove);
                SaveData();
                UpdateDataGridView();
            }
            else
            {
                MessageBox.Show("Item not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public byte[] GetItemById(int id)
        {
            //LoadData();
            Console.WriteLine("Position of Data to get: " + id.ToString());
            try
            {
                int datacount = 0;
                byte[] bytes = new byte[900];
                foreach (var items in data)
                {
                    if(datacount == id)
                    {
                        // Deserialize the JSON string to a byte array
                        byte[] recoveredByteArray = JsonConvert.DeserializeObject<byte[]>(items.Template);

                        Console.WriteLine($"Bytes size: {recoveredByteArray.Length}");

                        return recoveredByteArray;
                        /*
                        foreach(var bits in bytes)
                        {
                            Console.WriteLine($"Bit Count: {bitcount}, Template: {bits.ToString()}");
                            bitcount++;
                        }
                        */
                        //Console.WriteLine($"Item retrieved:\nName: {items.Name}\nTemplate: {Encoding.Default.GetString(bytes)}");
                        //break;
                    }
                    datacount++;
                }
                 
                return bytes;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                byte[] buffer = new byte[1];
                return buffer;
            }
        }

        public int GetItemCount()
        {
            LoadData();
            return data.Count;
        }


        public static int numberofsavedtemplates { get; set; }
        public static Dictionary<string, string> connectedScanners = new Dictionary<string, string>();

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
    public class DataItem
    {
        private static int idCounter = 1;

        public int Id { get; }
        public string Name { get; set; }
        public string Template { get; set; }

        public DataItem()
        {
            Id = idCounter++;
        }
    }
}