using Drapp.Metronome.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drapp.Metronome.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MetronomeView : ContentView
    {
        public MetronomeView()
        {
            InitializeComponent();

            BindingContext = new MetronomeViewModel(Color.Bisque, 120, Pattern.CreateDefault());
        }
    }
}