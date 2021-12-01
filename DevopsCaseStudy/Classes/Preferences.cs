namespace DevopsCaseStudy {
    public class Preferences {
        private string browserName;
        private bool browserRemote;

        public Preferences() {
            this.browserName = "Chrome";
            this.browserRemote = false;
        }

        public Preferences(string browserName, bool browserRemote) {
            this.browserName = browserName;
            this.browserRemote = browserRemote;
        }

        public string getBrowserName() {
            return this.browserName;
        }

        public void setBrowserName(string browserName) {
            this.browserName = browserName;
        }
        
        public bool getBrowserRemote() {
            return this.browserRemote;
        }

        public void setBrowserRemote(bool browserRemote) {
            this.browserRemote = browserRemote;
        }
    }
}