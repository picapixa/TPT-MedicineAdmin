using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TPT_MMAS.Shared.Control
{

    /// <summary>
    /// A custom GridView control that visually represents the layout of the tray containers.
    /// </summary>
    public sealed class PatientGridView : Windows.UI.Xaml.Controls.Control
    {
        #region property definitions
        public int ItemColumns
        {
            get { return (int)GetValue(ItemColumnsProperty); }
            set { SetValue(ItemColumnsProperty, value); }
        }
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public int ItemRows
        {
            get { return (int)GetValue(ItemRowsProperty); }
            set { SetValue(ItemRowsProperty, value); }
        }
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        
        #endregion

        #region DependencyProperty definitions
        public static readonly DependencyProperty ItemColumnsProperty =
            DependencyProperty.Register("ItemColumns", typeof(int), typeof(PatientGridView), new PropertyMetadata(1));

        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(double), typeof(PatientGridView), new PropertyMetadata(0D, ItemHeightChanged));

        public static readonly DependencyProperty ItemRowsProperty =
            DependencyProperty.Register("ItemRows", typeof(int), typeof(PatientGridView), new PropertyMetadata(1));

        public static readonly DependencyProperty ItemTemplateProperty =
           DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(PatientGridView), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register("ItemWidth", typeof(double), typeof(PatientGridView), new PropertyMetadata(0D, ItemWidthChanged));

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(object), typeof(PatientGridView), new PropertyMetadata(null));

        #endregion

        GridView gridView;
        bool _isInitialized = false;
        double _columnWidth;
        double _rowHeight;

        public PatientGridView()
        {
            this.DefaultStyleKey = typeof(PatientGridView);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            gridView = GetTemplateChild("gridView") as GridView;

            gridView.SetValue(ItemsControl.ItemTemplateProperty, ItemTemplate);
            gridView.SizeChanged += GridView_SizeChanged;
            gridView.ItemClick += ItemClick;

            _isInitialized = true;
        }

        private void GridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gridView = sender as GridView;

            if ((e.PreviousSize.Width != e.NewSize.Width) || (e.PreviousSize.Height != e.NewSize.Height))
            {
                RecalculateLayout(e.NewSize.Width, e.NewSize.Height);
            }
        }

        private void RecalculateLayout(double containerWidth, double containerHeight)
        {
            if (containerWidth == 0 || containerHeight == 0 || ItemColumns == 0)
                return;

            if (_columnWidth == 0)
                _columnWidth = CalculateColumnWidth(containerWidth, ItemColumns).Value;
            else
            {
                var cWidth = CalculateColumnWidth(containerWidth, ItemColumns).Value;
                if (cWidth != _columnWidth)
                    _columnWidth = cWidth;
            }

            if (_rowHeight == 0)
                _rowHeight = CalculateRowHeight(containerHeight, ItemRows).Value;
            else
            {
                var cHeight = CalculateRowHeight(containerHeight, ItemRows).Value;
                if (cHeight != _rowHeight)
                    _rowHeight = cHeight;
            }

            ItemWidth = _columnWidth;
            ItemHeight = _rowHeight;
        }

        private static double? CalculateColumnWidth(double containerWidth, int columns)
        {
            if (columns == 0)
                return null;

            var width = (double)(containerWidth / columns);
            return width;

        }


        private static double? CalculateRowHeight(double containerHeight, int rows)
        {
            if (rows == 0)
                return null;

            var height = (double)(containerHeight / rows);
            return height;

        }

        private static void ItemWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as PatientGridView;
            if (self._isInitialized)
                self.RecalculateLayout(self.gridView.ActualWidth, self.gridView.ActualHeight);
        }

        private static void ItemHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as PatientGridView;
            if (self._isInitialized)
                self.RecalculateLayout(self.gridView.ActualWidth, self.gridView.ActualHeight);
        }

        public event ItemClickEventHandler ItemClick;
    }
}
