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
                //threadReceive = new Thread(new ThreadStart(() =>
                //    {
                //        while (true)
                //        {
                //            //bool part2=socketClient.Available == 0;
                //            if (!socketClient.Connected || socketClient.Poll(10, SelectMode.SelectRead))
                //            {
                //                disconnect();
                //                break;
                //            }
                //            byte[] bytes = new byte[1024 * 1024 * 2];


                //            int bytesRec = socketClient.Receive(bytes);
                //            String theMessage;
                //            // Converts byte array to string
                //            theMessage = Encoding.Unicode.GetString(bytes, 0, bytesRec);

                //            // Continues to read the data till data isn't available
                //            while (socketClient.Available > 0)
                //            {
                //                bytesRec = socketClient.Receive(bytes);
                //                theMessage += Encoding.Unicode.GetString(bytes, 0, bytesRec);
                //            }

                //            historyTextBox.AppendText("server : " + theMessage + "\r\n");
                //            historyTextBox.Focus();
                //        }
                //    }
                //    ));
                //threadReceive.IsBackground = true;
                //threadReceive.Start();

                historyTextBox.AppendText(System.String.Format("Socket connected to {0}\r\n",
                                         socketClient.RemoteEndPoint.ToString()));
                historyTextBox.Focus();
            }
            else
            {
                historyTextBox.AppendText("fail to connect \r\n");
                historyTextBox.Focus();
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (socketClient != null && socketClient.Connected)
            {
                //SocketConnect();
                sendMessage();
            }
            else
            {
                historyTextBox.AppendText("what the heck?\r\n");
                historyTextBox.Focus();
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
            if (socketClient == null || !socketClient.Connected || IPTextbox.Text != IP || PortTextbox.Text != Port)
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
                    //               IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 4510);
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
                         buffer,        // An array of type Byt for received data
                    0,             // The zero-based position in the buffer 
                    buffer.Length, // The number of bytes to receive
                    SocketFlags.None,// Specifies send and receive behaviors
                    new AsyncCallback(ReceiveCallback),//An AsyncCallback delegate
                    obj            // Specifies infomation for receive operation
                    );

                }
                catch (Exception ex)
                {
                    //logBox.AppendText(System.String.Format("Exception: {0}\r\n", ex.ToString()));
                    //logBox.Focus();
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
                        // Convert byte array to string
                        string str =
                            content.Substring(0, content.LastIndexOf("<Server Quit>"));
                        historyTextBox.AppendText(System.String.Format("Server : {0}\r\n", str));
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

                // Create one SocketPermission for socket access restrictions
                //SocketPermission permission = new SocketPermission(
                //    NetworkAccess.Connect,    // Connection permission
                //    TransportType.Tcp,        // Defines transport types
                //    "",                       // Gets the IP addresses
                //    SocketPermission.AllPorts // All ports
                //    );

                //// Ensures the code to have permission to access a Socket
                //permission.Demand();

                //// Creates a network endpoint
                ////               IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 4510);
                //IPAddress ipAddr = IPAddress.Parse(IP);
                //IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, int.Parse(Port));

                //// Create one Socket object to setup Tcp connection
                //socketClient = new Socket(
                //   ipAddr.AddressFamily,// Specifies the addressing scheme
                //   SocketType.Stream,   // The type of socket 
                //   ProtocolType.Tcp     // Specifies the protocols 
                //   );

                //socketClient.NoDelay = false;   // Using the Nagle algorithm

                //// Establishes a connection to a remote host
                //socketClient.Connect(ipEndPoint);
                //socketClient.BeginConnect(ipEndPoint, new AsyncCallback(Connect), socketClient);
                /////////////////////////////////////////////////////////////////////////////////////

                // Sending message
                //<Client Quit> is the sign for end of data
                string theMessage = MessageTextbox.Text.Length > 0 ? MessageTextbox.Text : "Hello World ";

                byte[] msg = Encoding.Unicode.GetBytes(theMessage + "<Client Quit>");

                // Sends data to a connected Socket.
                //for(int i=0;i<3;i++)
                socketClient.SendTo(msg, this.ipEndPoint);
                historyTextBox.AppendText("clinet : " + theMessage + "\r\n");
                MessageTextbox.Clear();
                // Receives data from a bound Socket.
                //int bytesRec = socketClient.Receive(bytes);

                //// Converts byte array to string
                //theMessage = Encoding.Unicode.GetString(bytes, 0, bytesRec);

                //// Continues to read the data till data isn't available
                //while (socketClient.Available > 0)
                //{
                //    bytesRec = socketClient.Receive(bytes);
                //    theMessage += Encoding.Unicode.GetString(bytes, 0, bytesRec);
                //}

                //historyTextBox.AppendText(System.String.Format("server: {0}\r\n", theMessage));
                //historyTextBox.Focus();
            }
            catch (Exception ex)
            {
                logBox.AppendText(System.String.Format("Exception: {0}\r\n", ex.ToString()));
                logBox.Focus();
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

                historyTextBox.AppendText("success to close\r\n");
                historyTextBox.Focus();
                connectButton.Enabled = true;
                stopButton.Enabled = false;
            }
            else
            {
                historyTextBox.AppendText("fail to close\r\n");
                historyTextBox.Focus();
            }
        }

        private void logBox_TextChanged(object sender, EventArgs e)
        {
            logBox.ScrollToCaret();
            logBox.Focus();
        }

        private void MessageTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.sendButton_Click(null, null);
        }
    }
}
