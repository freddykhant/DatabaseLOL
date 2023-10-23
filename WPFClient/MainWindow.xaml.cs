using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ConsoleApp1;
using System.ServiceModel;
using System.IO;

namespace WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataServerInterface foob;
        public MainWindow()
        {
            InitializeComponent();

            ChannelFactory<DataServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();

            string URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<DataServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();

            TotalNum.Text = foob.GetNumEntries().ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            string fName = "", lName = "";
            int bal = 0;
            uint acct = 0, pin = 0;
            byte[] profileImage = null;

            index = Int32.Parse(IndexNum.Text);
            
            foob.GetValuesForEntry(index, out acct, out pin, out bal, out fName, out lName, out profileImage);
            
            try
            {
                FNameBox.Text = fName;
                LNameBox.Text = lName;
                BalanceBox.Text = bal.ToString("C");
                AcctNoBox.Text = acct.ToString();
                PinBox.Text = pin.ToString("D4");

                using (MemoryStream ms = new MemoryStream(profileImage))
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();

                    ProfileImage.Source = image;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid number for the index.");
            }
            catch (FaultException<ErrorData> ex)
            {
                MessageBox.Show($"Server Error: {ex.Detail.ErrorMessage}");
            }
            catch (Exception ex)
            {
                var errorData = new ErrorData { ErrorMessage = ex.Message };
                throw new FaultException<ErrorData>(errorData, "An error occurred while processing your request.");
            }
        }
    }
}
