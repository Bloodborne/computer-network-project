using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Threading;

namespace SocketGUI
{
    public partial class ClientView : Form
    {
        private Socket socketClient;
        private byte[] bytes = new byte[1024];
        private String IP;
        private String Port;
        private IPEndPoint ipEndPoint;
        private Thread threadReceive = null;

        public ClientView()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            IP = IPTextbox.Text;
            Port = PortTextbox.Text;

            SocketConnect();
            if (socketClient.Connected)
            {
                MessageBox.Show(this, System.String.Format("成功连接{0}",
                                         socketClient.RemoteEndPoint.ToString()), " ", MessageBoxButtons.OK);
            }
            else
            {
                showWarningMessage("连接失败");
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (socketClient != null && socketClient.Connected)
            {
                sendMessage();
            }
            else
            {
                showWarningMessage("数据发送失败");
            }

        }


        private void IPTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void historyTextBox_TextChanged(object sender, EventArgs e)
        {
            historyTextBox.ScrollToCaret();
            historyTextBox.Focus();
        }

        private void SocketConnect()
        {
            // Receiving byte array
            if (socketClient == null || !socketClient.Connected || IPTextbox.Text != IP 
                || PortTextbox.Text != Port)
                try
                {
                    // Create one SocketPermission for socket access restrictions
                    SocketPermission permission = new SocketPermission(
                        NetworkAccess.Connect,    // Connection permission
                        TransportType.Tcp,        // Defines transport types
                        "",                       // Gets the IP addresses
                        SocketPermission.AllPorts // All ports
                        );

                    // Ensures the code to have permission to access a Socket
                    permission.Demand();

                    // Creates a network endpoint
                    IPAddress ipAddr = IPAddress.Parse(IP);
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, int.Parse(Port));
                    this.ipEndPoint = ipEndPoint;

                    // Create one Socket object to setup Tcp connection
                    socketClient = new Socket(
                       ipAddr.AddressFamily,// Specifies the addressing scheme
                       SocketType.Stream,   // The type of socket 
                       ProtocolType.Tcp     // Specifies the protocols 
                       );

                    socketClient.NoDelay = false;   // Using the Nagle algorithm

                    // Establishes a connection to a remote host
                    socketClient.Connect(ipEndPoint);
                    updateButtons();

                    // Begins to asynchronously receive data
                    byte[] buffer = new byte[1024];
                    object[] obj = new object[2];
                    obj[0] = buffer;
                    obj[1] = socketClient;
                    socketClient.BeginReceive(
                         buffer,   // An array of type Byt for received data
                    0,             // The zero-based position in the buffer 
                    buffer.Length, // The number of bytes to receive
                    SocketFlags.None,// Specifies send and receive behaviors
                    new AsyncCallback(ReceiveCallback),//An AsyncCallback delegate
                    obj            // Specifies infomation for receive operation
                    );

                }
                catch (Exception ex)
                {

                }
        }


        /// <summary>
        /// Asynchronously receive data from the server.
        /// </summary>
        /// <param name="ar">
        /// the status of an asynchronous operation
        /// </param> 
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Fetch a user-defined object that contains information
                object[] obj = new object[2];
                obj = (object[])ar.AsyncState;

                // Received byte array
                byte[] buffer = (byte[])obj[0];

                // A Socket to handle remote host communication.
                Socket handler = (Socket)obj[1];

                if (isSocketDisconnected(handler))
                {
                    disconnect();
                    return;
                }

                // Received message
                string content = string.Empty;

                // The number of bytes received.
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    content += Encoding.Unicode.GetString(buffer, 0,
                        bytesRead);

                    // If message contains "<Client Quit>", finish receiving
                    if (content.IndexOf("<Server Quit>") > -1)
                    {
                        String time = DateTime.Now.ToString();
                        // Convert byte array to string
                        string str =
                            content.Substring(0, content.LastIndexOf("<Server Quit>"));
                        historyTextBox.AppendText(System.String.Format("Server {0}\r\n  {1}\r\n", time,str));
                        historyTextBox.Focus();

                        // Prepare the reply message
                        byte[] byteData =
                            Encoding.Unicode.GetBytes(str);
                    }

                    // Continues to asynchronously receive data
                    byte[] buffernew = new byte[1024];
                    obj[0] = buffernew;
                    obj[1] = handler;
                    handler.BeginReceive(buffernew, 0, buffernew.Length,
                        SocketFlags.None,
                        new AsyncCallback(ReceiveCallback), obj);

                }
            }
            catch (Exception ex)
            {
                //historyTextBox.AppendText(System.String.Format("Exception: {0}\r\n", ex.ToString()));
                //historyTextBox.Focus();
            }
        }

        /// <summary>
        /// judge the socket if is connected
        /// </summary>
        /// <param name="handler">
        /// A socket client
        /// </param> 
        private bool isSocketDisconnected(Socket handler)
        {
            return handler.Poll(10, SelectMode.SelectRead);
        }

        private void sendMessage()
        {
            try
            {
                // Sending message
                //<Client Quit> is the sign for end of data
                String theMessage;

                if (MessageTextbox.TextLength > 0)
                {
                    theMessage = MessageTextbox.Text;
                }
                else
                {
                    showWarningMessage("发送内容不能为空，请重新输入");
                    return;
                }

                byte[] msg = Encoding.Unicode.GetBytes(theMessage + "<Client Quit>");

                // Sends data to a connected Socket.
                String time = DateTime.Now.ToString();
                socketClient.SendTo(msg, this.ipEndPoint);
                historyTextBox.AppendText("clinet " + time + "\r\n  " + theMessage + "\r\n");
                MessageTextbox.Clear();
            }
            catch (Exception ex)
            {

            }
        }

        private void updateButtons()
        {
            connectButton.Enabled = false;
            stopButton.Enabled = true;
            sendMessageButton.Enabled = true;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            disconnect();
        }

        private void disconnect()
        {
            if (socketClient != null && socketClient.Connected)
            {
                // Disables sends and receives on a Socket.
                socketClient.Shutdown(SocketShutdown.Both);

                //Closes the Socket connection and releases all resources
                socketClient.Close();
                //threadReceive.Abort();

                //historyTextBox.AppendText("success to close\r\n");
                //historyTextBox.Focus();
                connectButton.Enabled = true;
                stopButton.Enabled = false;
            }
            else
            {
                //historyTextBox.AppendText("fail to close\r\n");
                //historyTextBox.Focus();
            }
        }

        private void MessageTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Control)
                this.sendButton_Click(null, null);
        }

        public void showWarningMessage(String message)
        {
            MessageBox.Show(this, message, "Warning!", MessageBoxButtons.OK);
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
