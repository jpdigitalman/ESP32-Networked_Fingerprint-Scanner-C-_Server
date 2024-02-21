namespace C_RayFingerNetwork
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textBoxDatabaseCounter = new TextBox();
            webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            richTextBoxConsole = new RichTextBox();
            addressBar = new TextBox();
            goButton = new Button();
            panel1 = new Panel();
            label3 = new Label();
            label1 = new Label();
            label2 = new Label();
            comboBoxIPAddresses = new ComboBox();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // textBoxDatabaseCounter
            // 
            textBoxDatabaseCounter.Location = new Point(399, 17);
            textBoxDatabaseCounter.Margin = new Padding(4, 5, 4, 5);
            textBoxDatabaseCounter.Name = "textBoxDatabaseCounter";
            textBoxDatabaseCounter.ReadOnly = true;
            textBoxDatabaseCounter.Size = new Size(308, 31);
            textBoxDatabaseCounter.TabIndex = 1;
            // 
            // webView
            // 
            webView.AllowExternalDrop = true;
            webView.CreationProperties = null;
            webView.DefaultBackgroundColor = Color.White;
            webView.Location = new Point(9, 113);
            webView.Margin = new Padding(4, 5, 4, 5);
            webView.Name = "webView";
            webView.Size = new Size(703, 508);
            webView.Source = new Uri("https://www.microsoft.com", UriKind.Absolute);
            webView.TabIndex = 4;
            webView.ZoomFactor = 1D;
            webView.NavigationStarting += webView_NavigationStarting;
            webView.NavigationCompleted += webView_NavigationCompleted;
            webView.WebMessageReceived += webView_WebMessageReceived;
            // 
            // richTextBoxConsole
            // 
            richTextBoxConsole.Location = new Point(17, 683);
            richTextBoxConsole.Margin = new Padding(4, 5, 4, 5);
            richTextBoxConsole.Name = "richTextBoxConsole";
            richTextBoxConsole.Size = new Size(1084, 251);
            richTextBoxConsole.TabIndex = 6;
            richTextBoxConsole.Text = "";
            // 
            // addressBar
            // 
            addressBar.Location = new Point(9, 67);
            addressBar.Margin = new Padding(4, 5, 4, 5);
            addressBar.Name = "addressBar";
            addressBar.Size = new Size(585, 31);
            addressBar.TabIndex = 7;
            addressBar.KeyDown += addressBar_KeyDown;
            // 
            // goButton
            // 
            goButton.DialogResult = DialogResult.OK;
            goButton.Location = new Point(604, 67);
            goButton.Margin = new Padding(4, 5, 4, 5);
            goButton.Name = "goButton";
            goButton.Size = new Size(107, 38);
            goButton.TabIndex = 8;
            goButton.Text = "Go";
            goButton.UseVisualStyleBackColor = true;
            goButton.Click += goButton_Click;
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.Fixed3D;
            panel1.Controls.Add(label3);
            panel1.Controls.Add(goButton);
            panel1.Controls.Add(addressBar);
            panel1.Controls.Add(webView);
            panel1.Controls.Add(textBoxDatabaseCounter);
            panel1.Location = new Point(391, 52);
            panel1.Margin = new Padding(4, 5, 4, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(723, 627);
            panel1.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 30);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(163, 25);
            label3.TabIndex = 9;
            label3.Text = "Enter Web Address";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(17, 115);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(232, 40);
            label1.TabIndex = 2;
            label1.Text = "List of Scanners";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(24, 42);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(202, 25);
            label2.TabIndex = 3;
            label2.Text = "Select Server IP Address";
            // 
            // comboBoxIPAddresses
            // 
            comboBoxIPAddresses.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxIPAddresses.FlatStyle = FlatStyle.Popup;
            comboBoxIPAddresses.FormattingEnabled = true;
            comboBoxIPAddresses.Location = new Point(24, 72);
            comboBoxIPAddresses.Margin = new Padding(4, 5, 4, 5);
            comboBoxIPAddresses.Name = "comboBoxIPAddresses";
            comboBoxIPAddresses.Size = new Size(358, 33);
            comboBoxIPAddresses.TabIndex = 5;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(24, 168);
            dataGridView1.Margin = new Padding(4, 5, 4, 5);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 62;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(359, 503);
            dataGridView1.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1130, 948);
            Controls.Add(dataGridView1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboBoxIPAddresses);
            Controls.Add(panel1);
            Controls.Add(richTextBoxConsole);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "C-Ray Foundation Fingerprint Network Controller V0.1 by John-Paul Madueke (Digitalman)";
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)webView).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView;
        private DataGridView dataGridView1;
        private TextBox textBoxDatabaseCounter;
        private Label label1;
        private Label label2;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        public ComboBox comboBoxIPAddresses;
        private RichTextBox richTextBoxConsole;
        private TextBox addressBar;
        private Button goButton;
        private Panel panel1;
        private Label label3;
    }
}