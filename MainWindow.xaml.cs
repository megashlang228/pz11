using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pz11
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Caretaker caretaker = new Caretaker();
        Originator originator = new Originator();

        public MainWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            var timer = new Timer(5000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;

        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {

            var momento = originator.CreateMomento();
            caretaker.push(momento.State);
        }



        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Open);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Load(fileStream, DataFormats.Rtf);
            }
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Rich Text Format (*.rtf)|*.rtf|All files (*.*)|*.*";
            if (dlg.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dlg.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.Save(fileStream, DataFormats.Rtf);
            }
        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontFamily.SelectedItem != null)
            {
                TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                range.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);

            }
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            range.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
            originator.State.FontSize = Convert.ToInt32(cmbFontSize.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            originator.State = caretaker.pop();
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);

            range.Text = originator.State.Text;
            range.ApplyPropertyValue(Inline.FontSizeProperty, originator.State.FontSize.ToString());

            if (originator.State.IsBold)
                range.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            else
                range.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);

            if (originator.State.IsItalics)
                range.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            else
                range.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);

            if (originator.State.IsUnderline)
                range.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            else
                range.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Baseline);

        }


        private void btnBold_Click(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            object temp = range.GetPropertyValue(FontWeightProperty);
            if (temp.Equals(FontWeights.Bold))
                range.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            
            if (temp.Equals(FontWeights.Normal))
                range.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);

            originator.State.IsBold = !originator.State.IsBold;
        }

        private void btnItalic_Click(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            object temp = range.GetPropertyValue(Inline.FontStyleProperty);
            if (temp.Equals(FontStyles.Italic))
                range.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            if (temp.Equals(FontStyles.Normal))
                range.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            originator.State.IsItalics = !originator.State.IsItalics;
        }

        private void btnUnderline_Click(object sender, RoutedEventArgs e)
        {
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            object temp = range.GetPropertyValue(Inline.TextDecorationsProperty);
            if (temp.Equals(TextDecorations.Underline))
                range.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Baseline);
            else
                range.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            originator.State.IsUnderline = !originator.State.IsUnderline;

        }

        private void rtbEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextRange range = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            originator.State.Text = range.Text;
        }
    }
}
