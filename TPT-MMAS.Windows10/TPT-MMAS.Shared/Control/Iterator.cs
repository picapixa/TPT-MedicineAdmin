using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TPT_MMAS.Shared.Control
{
    /// <summary>
    /// A custom control that has integer values and an up/down control.
    /// </summary>
    public sealed class Iterator : Windows.UI.Xaml.Controls.Control
    {
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(object), typeof(Iterator), new PropertyMetadata(null));

        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register("HeaderTemplate", typeof(DataTemplate), typeof(Iterator), new PropertyMetadata(null));

        public static readonly DependencyProperty MinValueProperty =
           DependencyProperty.Register("MinValue", typeof(int), typeof(Iterator), new PropertyMetadata(int.MinValue));

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(Iterator), new PropertyMetadata(int.MaxValue));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(Iterator), new PropertyMetadata(0));


        private bool _ignoreValueValidation;
        private int _fallbackValue;
        private int _prevValue;
        private Button btn_up;
        private Button btn_dn;
        private ContentPresenter headerContentPresenter;
        private TextBox tbx_value;

        public Iterator()
        {
            this.DefaultStyleKey = typeof(Iterator);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            headerContentPresenter = GetTemplateChild("HeaderContentPresenter") as ContentPresenter;
            if (headerContentPresenter.Content != null)
                headerContentPresenter.Visibility = Visibility.Visible;

            _prevValue = Value;
            _fallbackValue = Value;
            tbx_value = GetTemplateChild("tbx_value") as TextBox;
            tbx_value.TextChanging += OnValueTextBoxChanging;
            tbx_value.LostFocus += OnValueTextBoxLostFocus;

            btn_dn = GetTemplateChild("btn_dn") as Button;
            btn_dn.Click += OnDownButtonClick;
            btn_up = GetTemplateChild("btn_up") as Button;
            btn_up.Click += OnUpButtonClick;
            EvaluateCurrentValue(Value);
        }

        private void OnUpButtonClick(object sender, RoutedEventArgs e)
        {
            Value++;
        }

        private void OnDownButtonClick(object sender, RoutedEventArgs e)
        {
            Value--;
        }

        private void OnValueTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (tbx_value.Text.Trim() == "")
                tbx_value.Text = _fallbackValue.ToString();
        }

        private void OnValueTextBoxChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (_ignoreValueValidation)
            {
                _ignoreValueValidation = false;
                return;
            }
            else
            {
                string current = sender.Text;
                int val;
                if (int.TryParse(current, out val))
                {
                    _prevValue = val;
                    EvaluateCurrentValue(val);
                }
                else
                {
                    sender.Text = _prevValue.ToString();
                }



            }

        }

        private void EvaluateCurrentValue(int val)
        {
            btn_up.IsEnabled = (val < MaxValue);
            btn_dn.IsEnabled = (val > MinValue);
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Back || e.Key == Windows.System.VirtualKey.Delete)
            {
                _ignoreValueValidation = true;
            }
            base.OnKeyDown(e);
        }
    }
}
