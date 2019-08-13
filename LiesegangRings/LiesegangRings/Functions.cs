using System;
using System.Collections.Generic;
using System.Text;


namespace Functions
{
    public abstract class BasicClass
    {
        public const int MAX = 1000;
        public int[] MassInAlphabet = new int[MAX];
        public int[] MassAfterScrumble = new int[8 * MAX];
        public int[] MassAfterDescrumble = new int[8 * MAX];
        public int[] MassInSecond = new int[8 * MAX];
        public int lenghtOfMass = 0, lengthOfMassInSecond = -1;
        public Dictionary<char, int> alphabet = new Dictionary<char, int>();
        public const int OFFSET = 32;
        public const int NUMBER_OF_SYMBOLS = 105;
        public const int MAX_SIZE_OF_NumInFif = 444;
        public const int LENGTH_OF_NumInSecond = 7;
        public const int LENGTH_OF_NumInFif = 3;
        public StringBuilder result = new StringBuilder();


        public int InFifth(int numInTen)
        {
            int numInFif = 0;
            for (int i = 0; numInTen != 0; i++)
            {
                numInFif += (int)Math.Pow(10, i) * (numInTen % 5);
                numInTen /= 5;
            }
            return numInFif;
        }

        public int OutFifth(int numInFif)
        {
            int numInTen = 0, i = 0;
            while (numInFif != 0)
            {
                numInTen += (int)Math.Pow(5, i) * (numInFif % 10);
                numInFif /= 10;
                i++;
            }
            return numInTen;
        }

        public int InSecond(int numInTen, int locationOfLast)
        {
            int i = locationOfLast + LENGTH_OF_NumInSecond;
            int lastIndex = i;
            while (numInTen != 0)
            {
                MassInSecond[i] = numInTen % 2;
                numInTen /= 2;
                i--;
            }
            return lastIndex;
        }

        //генерация алфавита
        public void Abc()
        {
            for (int i = 0; i < NUMBER_OF_SYMBOLS; i++)
            {
                alphabet[(char)(i + OFFSET)] = InFifth(i);
            }
            alphabet['\n'] = InFifth(NUMBER_OF_SYMBOLS);
        }

        public int OutSecond(int position, Boolean ScramOrDescr)
        {
            int numInTen = 0, i = LENGTH_OF_NumInSecond - 1;
            while ((i != -1) && (ScramOrDescr))
            {
                numInTen += (int)Math.Pow(2, i) * (MassAfterScrumble[position]);
                i--; position++;
            }
            while ((i != -1) && (!ScramOrDescr))
            {
                numInTen += (int)Math.Pow(2, i) * (MassAfterDescrumble[position]);
                i--; position++;
            }
            return numInTen;
        }
    }

    public class CodingClass : BasicClass
    {

        //Ввод сообщения, сопоставление букв концентрациям по алфавиту
        bool InputForCoding(String message)
        {
            bool check = false;
            int length = message.Length;
            lengthOfMassInSecond = -1;
            result.Append(" Your text in alphabet:\n ");
            for (int i = 0; i < length; ++i)
            {
                if (!alphabet.ContainsKey(message[i]))
                {
                    check = true;
                    break;
                }
                MassInAlphabet[lenghtOfMass] = alphabet[message[i]];
                result.Append(MassInAlphabet[lenghtOfMass]);
                result.Append(' ');
                lengthOfMassInSecond = InSecond(OutFifth(MassInAlphabet[lenghtOfMass]), lengthOfMassInSecond);

                lenghtOfMass++;
            }
            result.Append('\n');

            result.Append(" it`s in secondary system:\n");

            for (int i = 0; i <= lengthOfMassInSecond; i++)
            {
                if (i % LENGTH_OF_NumInSecond == 0)
                    result.Append(' ');
                result.Append(MassInSecond[i]);
            }
            result.Append('\n');
            return check;
        }



        //скремблирование
        void Scrumble(int j, int N, int M)
        {
            result.Append(" It`s after scrumble:\n");
            for (int i = 0; i < j; i++)
            {
                if (i % LENGTH_OF_NumInSecond == 0)
                    result.Append(' ');
                if (i < N) MassAfterScrumble[i] = MassInSecond[i];
                else
                    if (i < M) MassAfterScrumble[i] = MassInSecond[i] ^ MassAfterScrumble[i - N];
                else
                    MassAfterScrumble[i] = (MassInSecond[i] ^ MassAfterScrumble[i - N]) ^ MassAfterScrumble[i - M];

                result.Append(MassAfterScrumble[i]);
            }
            result.Append('\n');
        }

        public String Coding(String message, String StrN, String StrM)
        {
            int N = 3, M = 5;
            try
            {
                N = Convert.ToInt32(StrN);
                M = Convert.ToInt32(StrM);
            }
            catch (Exception e)
            {
                result.Clear();
                result.Append(" Invalid key parametrs (only digits)\n ");
                return result.ToString();
            }
            if (N > M)
            {
                int tmp = N;
                N = M; M = tmp;
            }
            Abc();
            // cout << "Write message:\n";
            bool check = InputForCoding(message);
            if (check)
            {
                result.Clear();
                result.Append(" Check entered text!");
                return result.ToString();
            }
            Scrumble(lengthOfMassInSecond + 1, N, M);
            result.Append(" Scrumlered message in fifth system:\n ");

            for (int i = 0; i < (lengthOfMassInSecond + 1) / LENGTH_OF_NumInSecond; i++)
            {
                String tmp = InFifth(OutSecond(i * LENGTH_OF_NumInSecond, true)).ToString();
                while (tmp.Length < LENGTH_OF_NumInFif)
                {
                    tmp = '0' + tmp;
                }
                result.Append(tmp);
                result.Append(' ');
            }
            return result.ToString();
        }


    }

    public class DecodingClass : BasicClass
    {


        bool InputForDecoding(String message)
        {
            //int length = ((message.Length % 4 ) == 3) ? (message.Length / 4 + 1) : (message.Length / 4);
            bool check = false;
            lenghtOfMass = 0;
            lengthOfMassInSecond = -1;
            for (int i = 0; i < message.Length; ++i)
            {

                while (i < message.Length && message[i] != ' ')
                {
                    MassInAlphabet[lenghtOfMass] *= 10;
                    MassInAlphabet[lenghtOfMass] += (message[i] - '0');
                    i++;
                }
                if (MassInAlphabet[lenghtOfMass] > MAX_SIZE_OF_NumInFif && !(MassInAlphabet[lenghtOfMass] > 999 && MassInAlphabet[lenghtOfMass] < 1003))
                {
                    check = true;
                    break;
                }
                lengthOfMassInSecond = InSecond(OutFifth(MassInAlphabet[lenghtOfMass]), lengthOfMassInSecond);
                lenghtOfMass++;
            }
            result.Append(" it`s in secondary system:\n");
            for (int i = 0; i <= lengthOfMassInSecond; i++)
            {
                if (i % LENGTH_OF_NumInSecond == 0)
                    result.Append(' ');
                result.Append(MassInSecond[i]);
            }
            result.Append('\n');
            return check;
        }

        void Descrumble(int j, int N, int M)
        {
            result.Append(" It`s after descrumble:\n");
            for (int i = 0; i < j; i++)
            {
                if (i % LENGTH_OF_NumInSecond == 0)
                    result.Append(' ');
                if (i < N) MassAfterDescrumble[i] = MassInSecond[i];
                else
                    if (i < M) MassAfterDescrumble[i] = MassInSecond[i] ^ MassInSecond[i - N];
                else
                    MassAfterDescrumble[i] = (MassInSecond[i] ^ MassInSecond[i - N]) ^ MassInSecond[i - M];

                result.Append(MassAfterDescrumble[i]);
            }
            result.Append('\n');
        }

        public String Decoding(String message, String StrN, String StrM)
        {

            int N = 3, M = 5;
            try
            {
                N = Convert.ToInt32(StrN);
                M = Convert.ToInt32(StrM);
            }
            catch (Exception e)
            {
                result.Clear();
                result.Append(" Invalid key parametrs (only digits)\n ");
                return result.ToString();
            }
            if (N > M)
            {
                int tmp = N;
                N = M; M = tmp;
            }

            Abc();
            bool check = InputForDecoding(message);

            if (check)
            {
                result.Clear();
                result.Append(" Check entered concentrations!");
                return result.ToString();
            }

            Descrumble(lengthOfMassInSecond + 1, N, M);

            StringBuilder tmpResult = new StringBuilder();
            result.Append(" Message in fifth's system:\n ");
            for (int i = 0; i < (lengthOfMassInSecond + 1) / LENGTH_OF_NumInSecond; i++)
            {
                int tmpInTen = OutSecond(i * LENGTH_OF_NumInSecond, false);
                int tmpInFif = InFifth(tmpInTen);
                result.Append(tmpInFif);
                result.Append(' ');
                if (alphabet.ContainsValue(tmpInFif))
                {
                    if (tmpInTen == NUMBER_OF_SYMBOLS)
                        tmpResult.Append('\n');
                    else
                        tmpResult.Append((char)(tmpInTen + OFFSET));
                }
                else
                {
                    result.Clear();
                    result.Append(" Check entered concentrations!");
                    return result.ToString();
                }
            }
            result.Append("\n Decoded message:\n ");
            result.Append(tmpResult);
            return result.ToString();

        }

    }

}