using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FFXIV.Framework.Extensions;

namespace FFXIV.Framework.WPF.Controls
{
    /// <summary>
    /// ProgressBar.xaml の相互作用ロジック
    /// </summary>
    public partial class RichProgressBar : UserControl
    {
        public RichProgressBar()
        {
            this.InitializeComponent();
            this.Render();
        }

        #region Fill 依存関係プロパティ

        /// <summary>
        /// Fill 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty FillProperty
            = DependencyProperty.Register(
            nameof(Fill),
            typeof(Brush),
            typeof(RichProgressBar),
            new PropertyMetadata(
                Brushes.White,
                (s, e) => (s as RichProgressBar)?.Render()));

        /// <summary>
        /// Fill
        /// </summary>
        public Brush Fill
        {
            get => (Brush)this.GetValue(FillProperty);
            set => this.SetValue(FillProperty, value);
        }

        #endregion Fill 依存関係プロパティ

        #region Stroke 依存関係プロパティ

        /// <summary>
        /// Stroke 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty StrokeProperty
            = DependencyProperty.Register(
            nameof(Stroke),
            typeof(Brush),
            typeof(RichProgressBar),
            new PropertyMetadata(
                Brushes.Gray,
                (s, e) => (s as RichProgressBar)?.Render()));

        /// <summary>
        /// Stroke
        /// </summary>
        public Brush Stroke
        {
            get => (Brush)this.GetValue(StrokeProperty);
            set => this.SetValue(StrokeProperty, value);
        }

        #endregion Stroke 依存関係プロパティ

        #region Progress 依存関係プロパティ

        /// <summary>
        /// Progress 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ProgressProperty
            = DependencyProperty.Register(
            nameof(Progress),
            typeof(double),
            typeof(RichProgressBar),
            new PropertyMetadata(
                0d,
                (s, e) => (s as RichProgressBar)?.Render()));

        /// <summary>
        /// Progress
        /// </summary>
        public double Progress
        {
            get => (double)this.GetValue(ProgressProperty);
            set
            {
                var progress = value;
                if (progress < 0d)
                {
                    progress = 0d;
                }

                if (progress > 1.0d)
                {
                    progress = 1.0d;
                }

                this.SetValue(ProgressProperty, progress);
            }
        }

        #endregion Progress 依存関係プロパティ

        #region IsReverse 依存関係プロパティ

        /// <summary>
        /// IsReverse 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty IsReverseProperty
            = DependencyProperty.Register(
            nameof(IsReverse),
            typeof(bool),
            typeof(RichProgressBar),
            new PropertyMetadata(
                false,
                (s, e) => (s as RichProgressBar)?.Render()));

        /// <summary>
        /// IsReverse バーの進行方向を逆にするか？
        /// </summary>
        public bool IsReverse
        {
            get => (bool)this.GetValue(IsReverseProperty);
            set => this.SetValue(IsReverseProperty, value);
        }

        #endregion IsReverse 依存関係プロパティ

        #region IsDarkBackground 依存関係プロパティ

        /// <summary>
        /// IsDarkBackground 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty IsDarkBackgroundProperty
            = DependencyProperty.Register(
            nameof(IsDarkBackground),
            typeof(bool),
            typeof(RichProgressBar),
            new PropertyMetadata(
                true,
                (s, e) => (s as RichProgressBar)?.Render()));

        /// <summary>
        /// IsDarkBackground バーの背景を暗色にするか？
        /// </summary>
        public bool IsDarkBackground
        {
            get => (bool)this.GetValue(IsDarkBackgroundProperty);
            set => this.SetValue(IsDarkBackgroundProperty, value);
        }

        #endregion IsDarkBackground 依存関係プロパティ

        #region IsStrokeBackground 依存関係プロパティ

        /// <summary>
        /// IsStrokeBackground 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty IsStrokeBackgroundProperty
            = DependencyProperty.Register(
            nameof(IsStrokeBackground),
            typeof(bool),
            typeof(RichProgressBar),
            new PropertyMetadata(
                true,
                (s, e) => (s as RichProgressBar)?.Render()));

        /// <summary>
        /// IsStrokeBackground Strokeを背景に対して設定するか？
        /// </summary>
        public bool IsStrokeBackground
        {
            get => (bool)this.GetValue(IsStrokeBackgroundProperty);
            set => this.SetValue(IsStrokeBackgroundProperty, value);
        }

        #endregion IsStrokeBackground 依存関係プロパティ

        /// <summary>
        /// 暗くする倍率
        /// </summary>
        public const double ToDarkRatio = 0.35d;

        /// <summary>
        /// 明るくする倍率
        /// </summary>
        public const double ToLightRatio = 10.00d;

        /// <summary>
        /// 描画する
        /// </summary>
        private void Render()
        {
            // 背景色と前景色を設定する
            if (this.Fill is SolidColorBrush fill)
            {
                this.BackBar.Fill =
                    this.IsDarkBackground ?
                    fill.Color.ChangeBrightness(ToDarkRatio).ToBrush() :
                    fill.Color.ChangeBrightness(ToLightRatio).ToBrush();

                this.ForeBar.Fill = fill;
            }
            else
            {
                this.BackBar.Fill = Brushes.Black;
                this.ForeBar.Fill = this.Fill;
            }

            // 枠の色を設定する
            if (this.Stroke is SolidColorBrush stroke)
            {
                this.StrokeBar.Stroke = stroke;
            }
            else
            {
                this.StrokeBar.Stroke = this.Stroke;
            }

            // バーの幅を算出する
            var width = !this.IsReverse ?
                this.Width * this.Progress :
                this.Width * (1.0 - this.Progress);

            // バーの幅を設定する
            this.ForeBar.Width = width;

            // 枠の幅を決める
            this.StrokeBar.Width = this.IsStrokeBackground ?
                this.Width :
                width;
        }
    }
}
