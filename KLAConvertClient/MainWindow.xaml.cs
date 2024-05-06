using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KLAConvertClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        TcpClient client = new TcpClient(SERVER_IP, PORT_NO);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

       

        private void UserInputBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string number = UserInputBox.Text;
            if (!string.IsNullOrWhiteSpace(number))
            {
                NetworkStream nwStream = client.GetStream();
                byte[] byteSend = ASCIIEncoding.ASCII.GetBytes(number);
                nwStream.Write(byteSend, 0, byteSend.Length);


                byte[] byteRead = new byte[client.ReceiveBufferSize];
                int bytesRead = nwStream.Read(byteRead, 0, client.ReceiveBufferSize);
                String messageToShow = Encoding.ASCII.GetString(byteRead, 0, bytesRead);
                //String messageToShow = "Hello World";
                //MessageBox.Show(Encoding.ASCII.GetString(byteRead, 0, bytesRead));
                OutputBox.Text = messageToShow;
            } else
            {
                // If pass empty into server, it freezes; have to handle in client.
                OutputBox.Text = "Please enter something.";
            }
            
            
            
        }

    }
}