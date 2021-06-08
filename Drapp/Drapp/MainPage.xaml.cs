using System;
using Drapp.Metronome.View;
using Xamarin.Forms;

namespace Drapp
{
    public partial class MainPage : ContentPage
    {
        private readonly TestSoundGeneratorView _generatorView;
        private readonly TestSoundMixerView _mixerView;

        private int _currentViewIndex = 0;
        public MainPage()
        {
            _generatorView = new TestSoundGeneratorView();
            _mixerView = new TestSoundMixerView();
            
            InitializeComponent();
            
            stack.Children[0] = _generatorView;
        }

        public void OnPreviousView(object sender, EventArgs eventArgs)
        {
            SwitchView();
        }
        
        public void OnNextView(object sender, EventArgs eventArgs)
        {
            SwitchView();
        }

        private void SwitchView()
        {
            if (_currentViewIndex == 0)
            {
                stack.Children[0] = _mixerView;
                _currentViewIndex = 1;
            }
            else
            {
                stack.Children[0] = _generatorView;
                _currentViewIndex = 0;
            }

        }
    }
}
