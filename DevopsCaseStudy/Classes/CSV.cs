using System.IO;

namespace DevopsCaseStudy {
    public class CSV {
        private string CSVtext;

        public void generateCSV(string text) {
            CSVtext += text + "\n";
        }

        public void writeCSV(string fileName) {
            File.WriteAllText(fileName + ".csv", CSVtext);
            CSVtext = "";
        }
    }
}