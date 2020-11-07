using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimatedGifExamApp2
{
    class RandomString
    {
        private string LowerCaseAlphabet = "abcdefghijklmnopqrstuvwyxz";
        private string UpperCaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public string GetLowerString()
        {
            return LowerCaseAlphabet;
        }

        public string GetUpperString()
        {
            return UpperCaseAlphabet;
        }
        public string GenerateUpperCaseString(int size, Random rng)
        {
            return GenerateString(size, rng, UpperCaseAlphabet);
        }

        public string GenerateLowerCaseString(int size, Random rng)
        {
            return GenerateString(size, rng, LowerCaseAlphabet);
        }

        public string GenerateString(int size, Random rng, string alphabet)
        {
            char[] chars = new char[size];
            for (int i = 0; i < size; i++)
            {
                chars[i] = alphabet[rng.Next(alphabet.Length)];
            }
            return new string(chars);
        }
    }
}
