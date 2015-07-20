using System.IO;

namespace redirects.Util {
    public class File {
        public static string LoadToString(string ApplicationBasePath, string FilePath) {
            string text = null;
            var filename = ApplicationBasePath + "/" + FilePath;
            using (var sr = new StreamReader(filename)) {
                text = sr.ReadToEnd();
            }
            return text;
        }
    }
}
