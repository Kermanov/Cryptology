using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrithemiusCipher.Logic
{
    public enum Alphabet
    {
        English,
        Ukrainian
    }


    public static class TrithemiusCipher
    {
        private static Dictionary<Alphabet, string> alphabets;
        static TrithemiusCipher()
        {
            alphabets = new Dictionary<Alphabet, string>
            {
                { Alphabet.English, " 'abcdefghijklmnopqrstuvwxyz" },
                { Alphabet.Ukrainian, " 'абвгґдеєжзиіїйклмнопрстуфхцчшщьюя" }
            };
        }

        public static bool IsKeyValid(string coef, out int intCoef)
        {
            return int.TryParse(coef, out intCoef);
        }

        public static bool IsKeyValid(string moto, Alphabet alphabet)
        {
            return IsTextValid(moto, alphabet);
        }

        public static bool IsTextValid(string text, Alphabet alphabet)
        {
            foreach (var symbol in text)
            {
                if (!alphabets[alphabet].Contains(symbol))
                {
                    return false;
                }
            }

            return true;
        }

        private static string Shift(string text, int shift, Alphabet alphabet)
        {
            var stringBuilder = new StringBuilder(text.Length);
            foreach (var symbol in text)
            {
                int index = alphabets[alphabet].IndexOf(symbol);
                index = (index + shift + alphabets[alphabet].Length) % alphabets[alphabet].Length;
                stringBuilder.Append(alphabets[alphabet][index]);
            }

            return stringBuilder.ToString();
        }

        private static string ShiftViaFunction(string text, Alphabet alphabet, Func<int, int> func)
        {
            var stringBuilder = new StringBuilder(text.Length);
            for (int i = 0; i < text.Length; ++i)
            {
                int index = alphabets[alphabet].IndexOf(text[i]);
                int pos = index + func(i);
                while (pos < 0)
                {
                    pos += alphabets[alphabet].Length;
                }

                index = pos % alphabets[alphabet].Length;
                stringBuilder.Append(alphabets[alphabet][index]);
            }

            return stringBuilder.ToString();
        }

        public static string Encrypt(string text, Alphabet alphabet, int coefA, int coefB)
        {
            return ShiftViaFunction(text, alphabet, p => coefA * p + coefB);
        }

        public static string Encrypt(string text, Alphabet alphabet, int coefA, int coefB, int coefC)
        {
            return ShiftViaFunction(text, alphabet, p => coefA * p * p + coefB * p + coefC);
        }

        public static string Encrypt(string text, Alphabet alphabet, string moto)
        {
            return ShiftViaFunction(text, alphabet, p =>
            {
                return alphabets[alphabet].IndexOf(moto[p % moto.Length]);
            });
        }

        public static string Decrypt(string text, Alphabet alphabet, int coefA, int coefB)
        {
            return ShiftViaFunction(text, alphabet, p => -(coefA * p + coefB));
        }

        public static string Decrypt(string text, Alphabet alphabet, int coefA, int coefB, int coefC)
        {
            return ShiftViaFunction(text, alphabet, p => -(coefA * p * p + coefB * p + coefC));
        }

        public static string Decrypt(string text, Alphabet alphabet, string moto)
        {
            return ShiftViaFunction(text, alphabet, p =>
            {
                return -alphabets[alphabet].IndexOf(moto[p % moto.Length]);
            });
        }

        public static string CrackMoto(string encryptedText, string decryptedText, Alphabet alphabet)
        {
            var deltasDict = new Dictionary<int, int>();
            for (int i = 0; i < encryptedText.Length; ++i)
            {
                int delta = (alphabets[alphabet].IndexOf(encryptedText[i]) 
                    - alphabets[alphabet].IndexOf(decryptedText[i]) 
                    + alphabets[alphabet].Length) 
                    % alphabets[alphabet].Length;

                if (deltasDict.ContainsKey(delta))
                {
                    deltasDict[delta]++;
                }
                else
                {
                    deltasDict.Add(delta, 1);
                }
            }

            var averageKeyLength = (double)encryptedText.Length / deltasDict.Values.Min();
            if (averageKeyLength == encryptedText.Length)
            {
                return null;
            }

            int floorKeyLength = (int)Math.Floor(averageKeyLength);
            int ceilKeyLength = (int)Math.Ceiling(averageKeyLength);

            var floorKeyBuilder = new StringBuilder(floorKeyLength);
            for (int i = 0; i < floorKeyLength; ++i)
            {
                int delta = (alphabets[alphabet].IndexOf(encryptedText[i])
                    - alphabets[alphabet].IndexOf(decryptedText[i])
                    + alphabets[alphabet].Length)
                    % alphabets[alphabet].Length;
                floorKeyBuilder.Append(alphabets[alphabet][delta]);
            }

            var ceilKeyBuilder = new StringBuilder(ceilKeyLength);
            for (int i = 0; i < ceilKeyLength; ++i)
            {
                int delta = (alphabets[alphabet].IndexOf(encryptedText[i])
                    - alphabets[alphabet].IndexOf(decryptedText[i])
                    + alphabets[alphabet].Length)
                    % alphabets[alphabet].Length;
                ceilKeyBuilder.Append(alphabets[alphabet][delta]);
            }

            return floorKeyBuilder.ToString() + "\n" + ceilKeyBuilder.ToString();
        }
    }
}
