using Firebase.Database;

namespace course4Hotel
{
    public partial class MainPage : ContentPage
    {
        private readonly FirebaseClient _firebaseClient;
        int count = 0;

        public MainPage(FirebaseClient firebaseClient)
        {
            InitializeComponent();
            _firebaseClient = firebaseClient;
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
