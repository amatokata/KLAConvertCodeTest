using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
    class Program
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";

        static void Main(string[] args)
        {
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);
            Console.WriteLine("Listening for client to send the dollar number...");
            listener.Start();


            TcpClient client = listener.AcceptTcpClient();

            while (true)
            {
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];
                byte[] outBuffer = new byte[1000];

                int bytesRead = 0;
                while (bytesRead == 0)
                    bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                // convert the dollar number received into a string
                string number = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                string convertedWords;
                double number2 = 0;
                if (string.IsNullOrEmpty(number))
                {
                    convertedWords = "Please enter something";
                }
                else if (!double.TryParse(number, out number2))
                {
                    convertedWords = "Not a valid number.";
                }
                else if (number2 > 999999999.99)
                {
                    convertedWords = "Number is too big.";
                }
                else if (number == "0")
                {
                    convertedWords = "zero dollars";
                }
                else if (number == "1")
                {
                    convertedWords = "one dollar";
                }
                else if (number.Contains("-"))
                {
                    convertedWords = "Dollar amount cannot be negative.";
                }
                else
                {
                    convertedWords = ConvertToWords(number);
                }
                
                // convert to bytes and write back
                for (int i = 0; i < convertedWords.Length; i++)
                {
                    outBuffer[i] = (byte)(convertedWords[i]);
                }
                nwStream.Write(outBuffer, 0, convertedWords.Length);
            }
            client.Close();
            listener.Stop();
            Console.ReadLine();
        }

        // convert ones to string
        private static String convertOnes(String number)
        {
            int value = Convert.ToInt32(number);
            String[] words = { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            return words[value - 1];
        }

        // convert twos to string
        private static String convertTens(String number)
        {
            int value = Convert.ToInt32(number);
            String word = "";
            switch (value)
            {
                case 10:
                    word = "ten";
                    break;
                case 11:
                    word = "eleven";
                    break;
                case 12:
                    word = "twelve";
                    break;
                case 13:
                    word = "thirteen";
                    break;
                case 14:
                    word = "fourteen";
                    break;
                case 15:
                    word = "fifteen";
                    break;
                case 16:
                    word = "sixteen";
                    break;
                case 17:
                    word = "seventeen";
                    break;
                case 18:
                    word = "eighteen";
                    break;
                case 19:
                    word = "nineteen";
                    break;
                case 20:
                    word = "twenty";
                    break;
                case 30:
                    word = "thirty";
                    break;
                case 40:
                    word = "fourty";
                    break;
                case 50:
                    word = "fifty";
                    break;
                case 60:
                    word = "sixty";
                    break;
                case 70:
                    word = "seventy";
                    break;
                case 80:
                    word = "eighty";
                    break;
                case 90:
                    word = "ninety";
                    break;
                default:
                    if (value > 0)
                    {
                        word = convertTens(number.Substring(0, 1) + "0") + " " + convertOnes(number.Substring(1));
                    }
                    break;
            }
            return word;
        }

        // convert hundreds to string
        private static string convertHundreds(int number)
        {
            string words = "";

            // handle hundreds
            int temp = number / 100;
            if (temp > 0)
            {
                words += convertOnes(temp.ToString()) + " hundred ";
                number %= 100;
            }

            // handle tens and ones
            if (number > 0)
            {
                if (number < 10)
                    words += convertOnes(number.ToString());
                else if (number < 100)
                    words += convertTens(number.ToString());
                else
                    words += convertTens((number / 10 * 10).ToString()) + " " + convertOnes((number % 10).ToString());
            }

            return words.Trim();
        }

        //convert 
        private static String ConvertInt(String numberByte)
        {
            int number  = Convert.ToInt32(numberByte);
            if (number == 0)
                return "Zero";

            string words = "";

            // process millions first
            int temp = number / 1000000;
            if (temp > 0)
            {
                words += convertHundreds(temp) + " million ";
                number %= 1000000;
            }

            // process thousands
            temp = number / 1000;
            if (temp > 0)
            {
                words += convertHundreds(temp) + " thousand ";
                number %= 1000;
            }

            // process remainder
            if (number > 0)
            {
                words += convertHundreds(number);
            }

            return words.Trim();
        }

        

        //convert dollar and cents number to words
        private static String ConvertToWords(String number)
        {
            String words = "", wholeDollar = number, cents = "", sepStr = "", centsValStr = "";
            String dollarStr = "dollars";
            String centStr = "";
            int decimalPlace = number.IndexOf(".");
            if (decimalPlace > 0)
            {
                centStr = "cents";
                wholeDollar = number.Substring(0, decimalPlace);
                cents = number.Substring(decimalPlace + 1);
                if (Convert.ToInt32(cents) > 0)
                {
                    sepStr = "and"; //separate dollars from cents  
                    if (cents.Length == 1)
                        cents = cents + "0";
                    else if (cents.Length == 2)
                        cents = cents.Substring(0, 2);
                    else if (cents.Length >= 3)
                    {
                        String result = "Not a valid number.";
                        return result;
                    }

                    centsValStr = ConvertInt(cents).Trim();
                }
            }

            words = ConvertInt(wholeDollar).Trim() + " " + dollarStr + " " + sepStr + " " + centsValStr + " " + centStr;
            return words;
        }
    }
}


