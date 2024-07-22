using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace C8S.Common.Extensions
{
    public static class StringEx
    {
        #region Private Constants
        private const string RemoveNonalphanumeric = @"[^A-Za-z0-9]";
        private const string AppendAlphanumeric = @"0123456789abcdefghijklmnopqrstuvwxyz";
        private const string AppendAnychar = @"0123456789abcdefghijklmnopqrstuvwxyz`~!@#$%^&*()-_=+[]{}\|;:'"",.<>/?";
        private static readonly Random Rnd = new Random();
        #endregion

        #region Public Methods (Check)
        public static string? NullOrEmptyToNull(this string? input) =>
            string.IsNullOrEmpty(input) ? null : input;
        #endregion

        #region Public Methods (Append)
        public static string AppendRandom(this string original, int length = 6)
        {
            return AppendRandomInternal(original, length, AppendAnychar);
        }
        public static string AppendRandomAlphaOnly(this string original, int length = 6)
        {
            return AppendRandomInternal(original, length, AppendAlphanumeric);
        }
        public static string AppendUniqueDateTime(this string original)
        {
            return $"{original}{DateTime.Now:yyyyMMddHHmmssfff}";
        }

        private static string AppendRandomInternal(string original, int length, string allowedChars)
        {
            var sbResult = new StringBuilder(original);
            for (int count = 0; count < length; count++)
            {
                var index = Convert.ToInt32(Math.Floor(allowedChars.Length * Rnd.NextDouble()));
                sbResult.Append(allowedChars[index]);
            }
            return sbResult.ToString();
        }
        #endregion

        #region Public Methods (Compare)
        public static bool AreSame(string? original, string? toCompare)
        {
            return (original ?? string.Empty) == (toCompare ?? string.Empty);
        }
        public static int DistanceFrom(this string a, string b)
        {
            if (string.IsNullOrEmpty(a)) return b?.Length ?? 0;
            if (string.IsNullOrEmpty(b)) return a.Length;

            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                    (
                      Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                      distances[i - 1, j - 1] + cost
                    );
                }
            return distances[lengthA, lengthB];
        }
        #endregion

        #region Public Methods (Modifiy)
        public static string? Obscure(this string? input, int showLength = 5)
        {
            if (string.IsNullOrEmpty(input)) return input;

            if (input.StartsWith("http://")) return "http://" + input.Substring(7).Obscure(showLength);
            if (input.StartsWith("https://")) return "https://" + input.Substring(8).Obscure(showLength);

            if (input.Length < showLength * 2) return new string('*', input.Length);
            return input.Substring(0, showLength) +
                   new string('*', showLength) +
                   input.Substring(input.Length - showLength, showLength);
        }

        public static string? LimitTo(this string input, int limit = 12, bool useEllipses = true)
        {
            if (string.IsNullOrEmpty(input?.Trim())) return input;

            input = input.Trim();
            if (input.Length <= limit) return input;

            return useEllipses ?
                input.Substring(0, limit - 3) + "..." :
                input.Substring(0, limit);
        }

        public static string? ToFirstWord(this string input)
        {
            if (string.IsNullOrEmpty(input?.Trim())) return input;
            input = input.Trim();
            var spaceIndex = input.IndexOf(' ');
            if (spaceIndex < 0) return input;
            return input.Substring(0, spaceIndex);
        }

        public static string? ToNonFirstWords(this string input)
        {
            if (string.IsNullOrEmpty(input?.Trim())) return input;
            input = input.Trim();

            var firstWord = input.ToFirstWord();
            return input.Substring(firstWord?.Length ?? 0).Trim();
        }

        public static string RemoveNonAlphanumericWithSpaces(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            input = input.ToLower();
            input = input.Replace('.', ' ');
            input = input.Replace('-', ' ');
            input = Regex.Replace(input, @"\s+", " ");
            char[] arr = input.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray();
            return new string(arr);
        }

        public static string RemoveNonAlphanumeric(this string input)
        {
            char[] arr = input.Where(char.IsLetterOrDigit).ToArray();
            return new string(arr);
        }

        public static string SplitCamelCase(this string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }
        public static string SplitPascalCase(this string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        public static string ToPascalCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var charArray = input.ToCharArray();
            var makeLower = false;
            for (var index = 0; index < charArray.Length; index++)
            {
                var c = charArray[index];
                if (char.IsLetter(c))
                {
                    charArray[index] = makeLower ? char.ToLower(c) : char.ToUpper(c);
                    makeLower = true;
                }
                else makeLower = false;
            }

            return new string(charArray);
        }
        public static bool IsAllCapitals(this string input)
        {
            foreach (var c in input)
                if (char.IsLetter(c) && char.IsLower(c))
                    return false;
            return true;
        }

        public static string OxfordJoin(string separator, string conjunction, IEnumerable<string> strings)
        {
            var list = strings.ToList();
            if (list.Count == 0) return string.Empty;
            if (list.Count == 1) return list[0];
            if (list.Count == 2) return $"{list[0]}{conjunction}{list[1]}";

            list[^1] = $"{conjunction}{list[^1]}";
            return string.Join(separator, list);
        }
        #endregion

        #region Public Methods (Encode)
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        // see: https://stackoverflow.com/questions/59646922/aes-encryption-using-c-sharp
        public static string Encrypt(this string content, string password)
        {
            if (string.IsNullOrEmpty(content)) throw new ArgumentNullException(nameof(content));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            byte[] bytes = Encoding.UTF8.GetBytes(content);

            using SymmetricAlgorithm crypt = Aes.Create();
            using HashAlgorithm hash = MD5.Create();
            using MemoryStream memoryStream = new MemoryStream();

            // don't need to regenerate the IV since it's randomized when creating the Aes algo
            crypt.Key = hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // if we use the inline using declaration, then we have to flush & close also
            using (CryptoStream cryptoStream = new CryptoStream(
                memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(bytes, 0, bytes.Length);
            }

            string base64IV = Convert.ToBase64String(crypt.IV);
            string base64Cipher = Convert.ToBase64String(memoryStream.ToArray());

            return base64IV + "!" + base64Cipher;
        }

        public static string Decrypt(this string encrypted, string password)
        {
            if (string.IsNullOrEmpty(encrypted)) throw new ArgumentNullException(nameof(encrypted));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            var parts = encrypted.Split('!');
            if (parts.Length < 2) throw new ArgumentOutOfRangeException(nameof(encrypted));

            var base64IV = parts[0];
            if (string.IsNullOrEmpty(base64IV)) throw new ArgumentOutOfRangeException(nameof(encrypted));
            var base64Cipher = parts[1];
            if (string.IsNullOrEmpty(base64Cipher)) throw new ArgumentOutOfRangeException(nameof(encrypted));

            var iv = Convert.FromBase64String(base64IV);
            var cipher = Convert.FromBase64String(base64Cipher);
            using MemoryStream cipherStream = new MemoryStream(cipher);

            using SymmetricAlgorithm crypt = Aes.Create();

            using HashAlgorithm hash = MD5.Create();
            crypt.Key = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            crypt.IV = iv;

            using CryptoStream cryptoStream = new CryptoStream(
                cipherStream, crypt.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader decryptReader = new StreamReader(cryptoStream);

            var decrypted = decryptReader.ReadToEnd();
            return decrypted;
        }
        #endregion
    }
}
