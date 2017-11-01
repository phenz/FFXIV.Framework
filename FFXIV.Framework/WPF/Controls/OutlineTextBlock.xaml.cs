using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FFXIV.Framework.WPF.Controls
{
    /// <summary>
    /// OutlineTextBlock.xaml の相互作用ロジック
    /// </summary>
    public partial class OutlineTextBlock : UserControl
    {
        public OutlineTextBlock()
        {
            this.InitializeComponent();
        }

        #region Text 依存関係プロパティ

        /// <summary>
        /// Text 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty TextProperty
            = DependencyProperty.Register(
            nameof(Text),
            typeof(string),
            typeof(OutlineTextBlock),
            new PropertyMetadata(
                "サンプルテキスト",
                (s, e) => (s as OutlineTextBlock)?.Render()));

        /// <summary>
        /// Text
        /// </summary>
        public string Text
        {
            get => (string)this.GetValue(TextProperty);
            set => this.SetValue(TextProperty, value);
        }

        #endregion Text 依存関係プロパティ

        #region Stroke 依存関係プロパティ

        /// <summary>
        /// Stroke 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty StrokeProperty
            = DependencyProperty.Register(
            nameof(Stroke),
            typeof(Color),
            typeof(OutlineTextBlock),
            new PropertyMetadata(
                Colors.OrangeRed,
                (s, e) => (s as OutlineTextBlock)?.Render()));

        /// <summary>
        /// Stroke
        /// </summary>
        public Color Stroke
        {
            get => (Color)this.GetValue(StrokeProperty);
            set => this.SetValue(StrokeProperty, value);
        }

        #endregion Stroke 依存関係プロパティ

        #region Size 依存関係プロパティ

        private const double DefaultSize = 12;

        /// <summary>
        /// Size 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty SizeProperty
            = DependencyProperty.Register(
            nameof(Size),
            typeof(double),
            typeof(OutlineTextBlock),
            new PropertyMetadata(
                DefaultSize,
                (s, e) => (s as OutlineTextBlock)?.Render()));

        /// <summary>
        /// Size
        /// </summary>
        public double Size
        {
            get => (double)this.GetValue(SizeProperty);
            set => this.SetValue(SizeProperty, value);
        }

        #endregion Size 依存関係プロパティ

        /// <summary>
        /// 描画する
        /// </summary>
        private void Render()
        {
            this.CoreTextBlock.Text = this.Text;
            this.TextEffect.Color = this.Stroke;
            this.ScaleTransform.ScaleX = this.Size / DefaultSize;
        }
    }
}
