using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FFXIV.Framework.WPF.Controls
{
    /// <summary>
    /// OutlineTextBlock.xaml の相互作用ロジック
    /// </summary>
    public partial class LightOutlineTextBlock : UserControl
    {
        public LightOutlineTextBlock()
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
            typeof(LightOutlineTextBlock),
            new PropertyMetadata(
                "サンプルテキスト",
                (s, e) => (s as LightOutlineTextBlock)?.Render()));

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
            typeof(LightOutlineTextBlock),
            new PropertyMetadata(
                Colors.OrangeRed,
                (s, e) => (s as LightOutlineTextBlock)?.Render()));

        /// <summary>
        /// Stroke
        /// </summary>
        public Color Stroke
        {
            get => (Color)this.GetValue(StrokeProperty);
            set => this.SetValue(StrokeProperty, value);
        }

        #endregion Stroke 依存関係プロパティ

        /// <summary>
        /// 描画する
        /// </summary>
        private void Render()
        {
            this.CoreTextBlock.Text = this.Text;
            this.TextEffect.Color = this.Stroke;
        }
    }
}
