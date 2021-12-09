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
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace ChurchTexting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected void SendSMS()
        {
            // Find your Account SID and Auth Token at twilio.com/console
            // and set the environment variables. See http://twil.io/secure
            string accountSid = TwilioAccountSID.Text;
            string authToken = TwilioToken.Text;

            TwilioClient.Init(accountSid, authToken);

            List<String> finalSMSList = new List<string>();
            String errorList = "";

            if (SMSNumbers.Text.Length > 0)
            {
                String smsNumbersText = SMSNumbers.Text;
                if (smsNumbersText.Contains(","))
                {
                    String[] smsNumbersTextArray = smsNumbersText.Split(",");
                    foreach(String sms in smsNumbersTextArray)
                    {
                        if(sms.Length == 10)
                        {

                            finalSMSList.Add("+1" + sms);
                        }
                        else
                        {
                            errorList += sms + " is not proper phone number format. Must be 10 digits (eg: 5554442222)\n";
                        }
                       
                    }
                }
                else
                {
                    if (SMSNumbers.Text.Length == 10)
                    {
                        finalSMSList.Add("+1" + SMSNumbers.Text);
                    }
                    else
                    {
                        errorList += SMSNumbers.Text + " is not proper phone number format. Must be 10 digits (eg: 5554442222)\n";
                    }
                }
                if(errorList == "")
                {
                    //no errors... send
                    foreach(String sms in finalSMSList)
                    {
                        try
                        {
                            var message = MessageResource.Create(
                                                    body: TextMessage.Text,
                                                    from: new Twilio.Types.PhoneNumber(TwilioNumber.Text),
                                                    to: new Twilio.Types.PhoneNumber(sms), messagingServiceSid: TwilioMessageSID.Text
                                                   );
                            Console.WriteLine(message.Sid);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                       
                    }
                }
                else
                {
                    //errors
                    MessageBox.Show(errorList);
                }


            }
            else
            {
                MessageBox.Show("No phone number to text. No messages sent");
            }




        }

        private void SMSNumbers_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            SendSMS();
        }

        private void TwilioToken_Copy1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
