using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Resynk
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private Regex _numMatch;

        public UserControl1()
        {
            InitializeComponent();
            _numMatch = new Regex(@"^[0-9]+$");
        }

        private void value_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string text = tb.Text.Insert(tb.CaretIndex, e.Text);

            e.Handled = !_numMatch.IsMatch(text);
        }

        /// <summary>
        /// Maximum value for the Numeric Up Down control
        /// </summary>
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maximum.  
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(int),
            typeof(UserControl1), new UIPropertyMetadata(100));

        /// <summary>
        /// Minimum value of the numeric up down control.
        /// </summary>
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minimum.  
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int),
                                        typeof(UserControl1), new UIPropertyMetadata(0));

        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }
            set
            {
                TextBoxValue.Text = value.ToString();
                SetValue(ValueProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for Value. 
        // This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(UserControl1),
              new PropertyMetadata(0, new PropertyChangedCallback(OnSomeValuePropertyChanged)));

        private static void OnSomeValuePropertyChanged(
        DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            UserControl1 numericBox = target as UserControl1;
            numericBox.TextBoxValue.Text = e.NewValue.ToString();
        }

        private void value_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (!_numMatch.IsMatch(tb.Text)) 
                tb.Text = "";
            Value = Convert.ToInt32(tb.Text);
            if (Value < Minimum) Value = Minimum;
            if (Value > Maximum) Value = Maximum;
            RaiseEvent(new RoutedEventArgs(ValueChangedEvent));
        }

        // Value changed
        private static readonly RoutedEvent ValueChangedEvent =
            EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(UserControl1));

        /// <summary>The ValueChanged event is called when the 
        /// TextBoxValue of the control changes.</summary>
        public event RoutedEventHandler ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if (Value < Maximum)
            {
                Value++;
                RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
        }

        //Increase button clicked
        private static readonly RoutedEvent IncreaseClickedEvent =
            EventManager.RegisterRoutedEvent("IncreaseClicked", RoutingStrategy.Bubble,
                                            typeof(RoutedEventHandler), typeof(UserControl1));

        /// <summary>The IncreaseClicked event is called when the 
        /// Increase button clicked</summary>
        public event RoutedEventHandler IncreaseClicked
        {
            add { AddHandler(IncreaseClickedEvent, value); }
            remove { RemoveHandler(IncreaseClickedEvent, value); }
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (Value > Minimum)
            {
                Value--;
                RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
            }
        }

        //Decrease button clicked
        private static readonly RoutedEvent DecreaseClickedEvent =
            EventManager.RegisterRoutedEvent("DecreaseClicked", RoutingStrategy.Bubble,
                                            typeof(RoutedEventHandler), typeof(UserControl1));

        /// <summary>The DecreaseClicked event is called when the 
        /// Decrease button clicked</summary>
        public event RoutedEventHandler DecreaseClicked
        {
            add { AddHandler(DecreaseClickedEvent, value); }
            remove { RemoveHandler(DecreaseClickedEvent, value); }
        }

        /// <summary>
        /// Checking for Up and Down events and updating the value accordingly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void value_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsDown && e.Key == Key.Up && Value < Maximum)
            {
                Value++;
                RaiseEvent(new RoutedEventArgs(IncreaseClickedEvent));
            }
            else if (e.IsDown && e.Key == Key.Down && Value > Minimum)
            {
                Value--;
                RaiseEvent(new RoutedEventArgs(DecreaseClickedEvent));
            }
        }
    }
}
