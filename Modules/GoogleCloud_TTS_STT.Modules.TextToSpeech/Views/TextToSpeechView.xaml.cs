using GoogleCloud_TTS_STT.Core;
using GoogleCloud_TTS_STT.Modules.TextToSpeech.EventAggregators;
using GoogleCloud_TTS_STT.Modules.TextToSpeech.SSML;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using Prism.Events;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GoogleCloud_TTS_STT.Modules.TextToSpeech.Views
{
    /// <summary>
    /// Interaction logic for ViewA.xaml
    /// </summary>
    public partial class TextToSpeechView : UserControl
    {

        public TextToSpeechView(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            eventAggregator.GetEvent<SsmlBreakEvent>().Subscribe(AddSsmlBreak, ThreadOption.PublisherThread, false);
            eventAggregator.GetEvent<SpecialCharacterEvent>().Subscribe(AddSpecialCharacterCode, ThreadOption.PublisherThread, false);
        }

        private async void AddSpecialCharacterCode(string content)
        {
            if (TextBox_SSML.Text.Length + content.Length <= AppConstants.MaximumNumberOfSsmlCharactersAllowed_TTS)
            {
                TextBox_SSML.Focus();
                int caretIndex = TextBox_SSML.CaretIndex;
                if (!content.Equals("and"))
                {
                    TextBox_SSML.Text = TextBox_SSML.Text.Insert(caretIndex, $"{content}");
                    TextBox_SSML.CaretIndex = caretIndex + content.Length;
                }
                else
                {
                    TextBox_SSML.Text = TextBox_SSML.Text.Insert(caretIndex, $"{content} ");
                    TextBox_SSML.CaretIndex = caretIndex + content.Length + 1;
                }
            }
            else
            {
                await AppHelper.ShowMessage("Maximum length will be exceeded!", $"Cannot insert '{content}' because the length of remaining" +
                    " characters allowed is less than the number of characters required for this content.");
            }
        }

        private async void AddSsmlBreak(string tagContent)
        {
            if (TextBox_SSML.Text.Length + tagContent.Length <= AppConstants.MaximumNumberOfSsmlCharactersAllowed_TTS)
            {
                TextBox_SSML.Focus();
                int caretIndex = TextBox_SSML.CaretIndex;
                TextBox_SSML.Text = TextBox_SSML.Text.Insert(caretIndex, $"{tagContent} ");
                TextBox_SSML.CaretIndex = caretIndex + tagContent.Length + 1;
            }
            else
            {
                await AppHelper.ShowMessage("Maximum length will be exceeded!", $"Cannot insert '{tagContent}' because the length of remaining" +
                    " characters allowed is less than the number of characters required for this content.");
            }
        }

        //private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    //await ((MetroWindow)Window.GetWindow(this)).ShowChildWindowAsync<bool>(new BreakTag() { IsModal = true, AllowMove = true }, ChildWindowManager.OverlayFillBehavior.WindowContent);
        //}
    }
}
