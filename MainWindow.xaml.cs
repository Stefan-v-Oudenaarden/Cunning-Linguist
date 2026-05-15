using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cunning_Linguist
{
    public partial class MainWindow : Window
    {
        private Linguist _linguist;

        public MainWindow()
        {
            _linguist = new();
            InitializeComponent();

            UpdateFromLinguist();
            this.Topmost = true;
        }

        private void UpdateFromLinguist()
        {
            SuggestionsList.Text = _linguist.GetAllSuggestedWords();

            Suggestion.Text = string.Join(" ", _linguist.GetSuggestedWord().ToUpper().ToArray());
        }

        private string[] FloatingLetters()
        {
            string[] FloatingPerCol = new string[5];

            var col0 = string.Concat(FloatingR1C1.Text, FloatingR2C1.Text, FloatingR3C1.Text, FloatingR4C1.Text, FloatingR5C1.Text).ToLower();
            FloatingPerCol[0] = new string(col0.Distinct().OrderBy(c => c).ToArray());

            var col1 = string.Concat(FloatingR1C2.Text, FloatingR2C2.Text, FloatingR3C2.Text, FloatingR4C2.Text, FloatingR5C2.Text).ToLower();
            FloatingPerCol[1] = new string(col1.Distinct().OrderBy(c => c).ToArray());

            var col2 = string.Concat(FloatingR1C3.Text, FloatingR2C3.Text, FloatingR3C3.Text, FloatingR4C3.Text, FloatingR5C3.Text).ToLower();
            FloatingPerCol[2] = new string(col2.Distinct().OrderBy(c => c).ToArray());

            var col3 = string.Concat(FloatingR1C4.Text, FloatingR2C4.Text, FloatingR3C4.Text, FloatingR4C4.Text, FloatingR5C4.Text).ToLower();
            FloatingPerCol[3] = new string(col3.Distinct().OrderBy(c => c).ToArray());

            var col4 = string.Concat(FloatingR1C5.Text, FloatingR2C5.Text, FloatingR3C5.Text, FloatingR4C5.Text, FloatingR5C5.Text).ToLower();
            FloatingPerCol[4] = new string(col4.Distinct().OrderBy(c => c).ToArray());

            return FloatingPerCol;
        }

        private void UpdateLinguist()
        {
            var fixedLetters = new string[5]
            {
                Fixed1.Text.ToLower(),
                Fixed2.Text.ToLower(),
                Fixed3.Text.ToLower(),
                Fixed4.Text.ToLower(),
                Fixed5.Text.ToLower(),
            };

            _linguist.Process(fixedLetters, FloatingLetters(), BadList.Text);
        }

        private void WindowKeyUp(object sender, KeyEventArgs e)
        {
            UpdateLinguist();
            UpdateFromLinguist();
        }

        private void FixedTextboxDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox box)
            {
                box.Text = "";
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            FloatingR1C1.Text = "";
            FloatingR2C1.Text = "";
            FloatingR3C1.Text = "";
            FloatingR4C1.Text = "";
            FloatingR5C1.Text = "";

            FloatingR1C2.Text = "";
            FloatingR2C2.Text = "";
            FloatingR3C2.Text = "";
            FloatingR4C2.Text = "";
            FloatingR5C2.Text = "";

            FloatingR1C3.Text = "";
            FloatingR2C3.Text = "";
            FloatingR3C3.Text = "";
            FloatingR4C3.Text = "";
            FloatingR5C3.Text = "";

            FloatingR1C4.Text = "";
            FloatingR2C4.Text = "";
            FloatingR3C4.Text = "";
            FloatingR4C4.Text = "";
            FloatingR5C4.Text = "";

            FloatingR1C5.Text = "";
            FloatingR2C5.Text = "";
            FloatingR3C5.Text = "";
            FloatingR4C5.Text = "";
            FloatingR5C5.Text = "";

            Fixed1.Text = "";
            Fixed2.Text = "";
            Fixed3.Text = "";
            Fixed4.Text = "";
            Fixed5.Text = "";

            BadList.Text = "";

            UpdateLinguist();
            UpdateFromLinguist();
        }
    }
}