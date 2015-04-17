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

namespace SocketGUI
{
    public partial class ServerView : Form
    {
        private Socket sListener;
        private Dictionary<String, Socket> didCommunication = new Dictionary<string, Socket>();    //与client正在通信的scoket
        private EndPoint remoteEndPoint;

        private String IP;
        private IPAddress ipAddr;
        private IPEndPoint ipEndPoint;

        public ServerView()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Resolves a host name to an IPHostEntry instance
            IPHostEntry ipHost = Dns.GetHostEntry("");

            // Gets IPV4 address associated with a localhost         

            foreach (IPAddress ip in ipHost.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    ipAddr = ipHost.AddressList[4];
                //historyTextBox.AppendText(System.String.Format(ip.ToString()+"\r\n"));
            }
            int port = 4510;

            // Creates a network endpoint
            ipEndPoint = new IPEndPoint(ipAddr, port);

            IPTextbox.Text = ipAddr.ToString();
            PortTextbox.Text = port.ToString();
        }

        private void listenButton_Click(object sender, EventArgs e)
        {
            if (sListener == null || !sListener.Connected)
            {
                // Creates one SocketPermission object for access restrictions
                SocketPermission permission = new SocketPermission(
                    NetworkAccess.Accept,     // Allowed to accept connections
                    TransportType.Tcp,        // Defines transport types
                    "",                       // The IP addresses of local host
                    SocketPermission.AllPorts // Specifies all ports
                    );

                try
                {
                    // Ensures the code to have permission to access a Socket
                    permission.Demand();

                    // Create one Socket object to listen the incoming connection
                    sListener = new Socket(
                        ipAddr.AddressFamily,
                        SocketType.Stream,
                        ProtocolType.Tcp
                        );

                    // Associates a Socket with a local endpoint
                    sListener.Bind(ipEndPoint);

                    // Places a Socket in a listening state and specifies the maximum
                    // Length of the pending connections queue
                    sListener.Listen(10);

                    historyTextBox.AppendText(System.String.Format("Waiting for a connection on port {0}\r\n",
                        ipEndPoint));
                    historyTextBox.Focus();

                    // Begins an asynchronous operation to accept an attempt
                    AsyncCallback aCallback = new AsyncCallback(AcceptCallback);
                    sListener.BeginAccept(aCallback, sListener);

                    listenButton.Enabled = false;
                }
                catch (Exception ex)
                {
                    historyTextBox.AppendText(System.String.Format("Exception: {0}\r\n",
                        ex.ToString()));
                    historyTextBox.Focus();
                    return;
                }
            }
            else
            {
                historyTextBox.AppendText("have been created the listener");
                historyTextBox.Focus();
            }

        }

        /// <summary>
        /// Asynchronously accepts an incoming connection attempt and creates
        /// a new Socket to handle remote host communication.
        /// </summary>     
        /// <param name="ar">the status of an asynchronous operation
        /// </param> 
        public void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = null;
            // A new Socket to handle remote host communication
            Socket handler = null;
            try
            {
                // Receiving byte array
                byte[] buffer = new byte[1024];
                // Get Listening Socket object
                listener = (Socket)ar.AsyncState;
                // Create a new socket
                handler = listener.EndAccept(ar);

                // Using the Nagle algorithm
                handler.NoDelay = false;

                //////////////////////////////////////////////////////////////////////////////////
                remoteEndPoint = handler.RemoteEndPoint;
                IP = remoteEndPoint.ToString().Split(':')[0];    //没有用
                if (!didCommunication.ContainsKey(remoteEndPoint.ToString()))
                {
                    addClient(handler, remoteEndPoint);
                }
                //////////////////////////////////////////////////////////////////////////////////

                // Creates one object array for passing data
                object[] obj = new object[2];
                obj[0] = buffer;
                obj[1] = handler;

                // Begins to asynchronously receive data

                didCommunication[remoteEndPoint.ToString()].BeginReceive(
                    //handler.BeginReceive(
                    buffer,        // An array of type Byt for received data
                    0,             // The zero-based position in the buffer 
                    buffer.Length, // The number of bytes to receive
                    SocketFlags.None,// Specifies send and receive behaviors
                    new AsyncCallback(ReceiveCallback),//An AsyncCallback delegate
                    obj            // Specifies infomation for receive operation
                    );

                // Begins an asynchronous operation to accept an attempt
                AsyncCallback aCallback = new AsyncCallback(AcceptCallback);
                listener.BeginAccept(aCallback, listener);


            }
            catch (Exception ex)
            {
                //historyTextBox.AppendText(System.String.Format("$Exception: {0}\r\n", ex.ToString()));
                //historyTextBox.Focus();
            }
        }

        /// <summary>
        /// Asynchronously receive data from a connected Socket.
        /// </summary>
        /// <param name="ar">
        /// the status of an asynchronous operation
        /// </param> 
        public void ReceiveCallback(IAsyncResult ar)
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
                    deleteClient(handler);
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
                    if (content.IndexOf("<Client Quit>") > -1)
                    {
                        // Convert byte array to string
                        string str =
                            content.Substring(0, content.LastIndexOf("<Client Quit>"));
                        historyTextBox.AppendText(System.String.Format("client : {0}\r\n", str));
                        historyTextBox.Focus();

                        // Prepare the reply message
                        byte[] byteData =
                            Encoding.Unicode.GetBytes(str);

                        // Sends data asynchronously to a connected Socket
                        //handler.BeginSend(byteData, 0, byteData.Length, 0,
                        //    new AsyncCallback(SendCallback), handler);
                    }
                    //tortured by this "else"!!
                    //else
                    {
                        // Continues to asynchronously receive data
                        byte[] buffernew = new byte[1024];
                        obj[0] = buffernew;
                        obj[1] = handler;
                        handler.BeginReceive(buffernew, 0, buffernew.Length,
                            SocketFlags.None,
                            new AsyncCallback(ReceiveCallback), obj);
                    }
                }
            }
            catch (Exception ex)
            {
                //historyTextBox.AppendText(System.String.Format("Exception: {0}\r\n", ex.ToString()));
                //historyTextBox.Focus();
            }
        }

        /// <summary>
        /// Sends data asynchronously to a connected Socket.
        /// </summary>
        /// <param name="ar">
        /// The status of an asynchronous operation
        /// </param> 
        public void SendCallback(IAsyncResult ar)
        {
            try
            {
                // A Socket which has sent the data to remote host
                Socket handler = (Socket)ar.AsyncState;

                // The number of bytes sent to the Socket
                int bytesSend = handler.EndSend(ar);

                //historyTextBox.AppendText(System.String.Format(
                //    "Sent {0} bytes to Client\r\n", bytesSend));
                //historyTextBox.Focus();
            }
            catch (Exception ex)
            {
                historyTextBox.AppendText(System.String.Format("Exception: {0}\r\n", ex.ToString()));
                historyTextBox.Focus();
            }
        }

        /// <summary>
        /// send message to all connected clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendButton_Click(object sender, EventArgs e)
        {
            String message = MessageTextbox.TextLength > 0 ? MessageTextbox.Text : "Hello~ ";

            byte[] msg = Encoding.Unicode.GetBytes(message + "<Server Quit>");

            // Sends data asynchronously to all connected Socket
            int i = 0;
            foreach (Socket socket in didCommunication.Values)
            {
                socket.BeginSend(msg, 0, msg.Length, 0,
                new AsyncCallback(SendCallback), socket);
                i++;
            }

            //didCommunication[remoteEndPoint.ToString()].Send(msg);
            historyTextBox.AppendText("server : " + message + "\r\n");
            historyTextBox.Focus();
            MessageTextbox.Clear();
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

        /// <summary>
        /// when a client is online,create the relative information
        /// </summary>
        /// <param name="handler">
        /// A socket client
        /// </param> 
        private void addClient(Socket handler, EndPoint remoteEndPoint)
        {
            didCommunication.Add(remoteEndPoint.ToString(), handler);

            Object item = remoteEndPoint.ToString();
            onlineClientListBox.Items.Add(item);
        }

        /// <summary>
        /// when a client is offline,delete the relative information
        /// </summary>
        /// <param name="handler">
        /// A socket client
        /// </param> 
        private void deleteClient(Socket handler)
        {
            String endPoint = handler.RemoteEndPoint.ToString();
            Object item = endPoint;

            didCommunication.Remove(endPoint);
            onlineClientListBox.Items.Remove(item);

            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }

        private void IPTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void historyTextBox_TextChanged(object sender, EventArgs e)
        {
            historyTextBox.ScrollToCaret();
            historyTextBox.Focus();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (sListener != null && didCommunication.Count > 0)
            {

                List<string> keys = new List<string>();
                foreach (var key in didCommunication.Keys)
                    keys.Add(key);

                foreach (String key in keys)
                {
                    Socket socket = didCommunication[key];
                    deleteClient(socket);
                }

                sListener.Close();
                //sListener.Dispose();

                listenButton.Enabled = true;
                historyTextBox.AppendText("successfully close\r\n");
                historyTextBox.Focus();
            }
            else
            {
                historyTextBox.AppendText("fail to close\r\n");
                historyTextBox.Focus();
            }
        }

        private void onlineListRichTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// In input message box,send the text when tap the "Enter" key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.sendButton_Click(null, null);
        }


    }
}
