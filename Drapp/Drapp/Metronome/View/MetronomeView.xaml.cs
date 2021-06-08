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
            /*
            BindingContext = new MetronomeViewModel(120);
            */
            
            InitializeComponent();
        }
    }
}