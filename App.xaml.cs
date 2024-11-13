using Firebase.Database;

namespace course4Hotel
{
    public partial class App : Application
    {
        private readonly FirebaseClient _firebaseClient;

        public App()
        {
            InitializeComponent();

            _firebaseClient = new FirebaseClient("https://course4hotel-default-rtdb.firebaseio.com/");
            MainPage = new NavigationPage(new View.SinginPage(_firebaseClient));
            //MainPage = new View.SinginPage(_firebaseClient);
            
        }
    }
}
