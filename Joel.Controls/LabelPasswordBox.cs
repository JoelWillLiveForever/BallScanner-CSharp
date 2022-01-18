using System;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Joel.Controls
{
    public class LabelPasswordBox : TextBox
    {
        // label text
        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register(nameof(LabelText), typeof(string), typeof(LabelPasswordBox), new PropertyMetadata(default(string)));

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(SecureString), typeof(LabelPasswordBox), new UIPropertyMetadata(new SecureString()));

        public SecureString Password
        {
            get => (SecureString)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty HiddenTextProperty =
            DependencyProperty.Register(nameof(HiddenText), typeof(string), typeof(LabelPasswordBox), new PropertyMetadata(default(string)));

        public string HiddenText
        {
            get => (string)GetValue(HiddenTextProperty);
            set => SetValue(HiddenTextProperty, value);
        }

        private readonly DispatcherTimer _maskTimer;

        public LabelPasswordBox()
        {
            PreviewTextInput += OnPreviewTextInput;
            PreviewKeyDown += OnPreviewKeyDown;
            CommandManager.AddPreviewExecutedHandler(this, PreviewExecutedHandler);
            _maskTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 1) };
            _maskTimer.Tick += (sender, args) => MaskAllDisplayText();
        }

        static LabelPasswordBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelPasswordBox),
                new FrameworkPropertyMetadata(typeof(LabelPasswordBox)));
        }

        private static void PreviewExecutedHandler(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            if (executedRoutedEventArgs.Command == ApplicationCommands.Copy ||
                executedRoutedEventArgs.Command == ApplicationCommands.Cut ||
                executedRoutedEventArgs.Command == ApplicationCommands.Paste)
            {
                executedRoutedEventArgs.Handled = true;
            }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs textCompositionEventArgs)
        {
            AddToSecureString(textCompositionEventArgs.Text);
            textCompositionEventArgs.Handled = true;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            Key pressedKey = keyEventArgs.Key == Key.System ? keyEventArgs.SystemKey : keyEventArgs.Key;
            switch (pressedKey)
            {
                case Key.Space:
                    AddToSecureString(" ");
                    keyEventArgs.Handled = true;
                    break;
                case Key.Back:
                case Key.Delete:
                    if (SelectionLength > 0)
                    {
                        RemoveFromSecureString(SelectionStart, SelectionLength);
                    }
                    else if (pressedKey == Key.Delete && CaretIndex < Text.Length)
                    {
                        RemoveFromSecureString(CaretIndex, 1);
                    }
                    else if (pressedKey == Key.Back && CaretIndex > 0)
                    {
                        int caretIndex = CaretIndex;
                        if (CaretIndex > 0 && CaretIndex < Text.Length)
                            caretIndex = caretIndex - 1;
                        RemoveFromSecureString(CaretIndex - 1, 1);
                        CaretIndex = caretIndex;
                    }

                    keyEventArgs.Handled = true;
                    break;
            }
        }

        private void AddToSecureString(string text)
        {
            if (SelectionLength > 0)
            {
                RemoveFromSecureString(SelectionStart, SelectionLength);
            }

            foreach (char c in text)
            {
                int caretIndex = CaretIndex;
                Password.InsertAt(caretIndex, c);

                //Console.WriteLine("TO STRING = " + c.ToString());
                //Console.WriteLine(HiddenText == null ? "NULL" : "NOT NULL");

                if (HiddenText == null)
                    HiddenText = "";

                HiddenText = HiddenText.Insert(caretIndex, c.ToString());
                MaskAllDisplayText();
                if (caretIndex == Text.Length)
                {
                    _maskTimer.Stop();
                    _maskTimer.Start();
                    Text = Text.Insert(caretIndex++, c.ToString());
                }
                else
                {
                    Text = Text.Insert(caretIndex++, "•");
                }
                CaretIndex = caretIndex;
            }
        }

        private void RemoveFromSecureString(int startIndex, int trimLength)
        {
            int caretIndex = CaretIndex;
            for (int i = 0; i < trimLength; ++i)
            {
                Password.RemoveAt(startIndex);
               HiddenText = HiddenText.Remove(startIndex, 1);
            }

            Text = Text.Remove(startIndex, trimLength);
            CaretIndex = caretIndex;
        }

        private void MaskAllDisplayText()
        {
            _maskTimer.Stop();
            int caretIndex = CaretIndex;
            Text = new string('•', Text.Length);
            CaretIndex = caretIndex;
        }
    }
}
