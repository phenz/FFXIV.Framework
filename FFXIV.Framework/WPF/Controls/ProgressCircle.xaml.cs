using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FFXIV.Framework.Extensions;

namespace FFXIV.Framework.WPF.Controls
{
    /// <summary>
    /// ProgressCircle.xaml の相互作用ロジック
    /// </summary>
    public partial class ProgressCircle : UserControl
    {
        public ProgressCircle()
        {
            this.InitializeComponent();
        }

        #region Radius 依存関係プロパティ

        /// <summary>
        /// Radius 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty RadiusProperty
            = DependencyProperty.Register(
            nameof(Radius),
            typeof(double),
            typeof(ProgressCircle),
            new PropertyMetadata(
                0d,
                (s, e) => (s as ProgressCircle)?.Render()));

        /// <summary>
        /// Radius
        /// </summary>
        public double Radius
        {
            get => (double)this.GetValue(RadiusProperty);
            set => this.SetValue(RadiusProperty, value);
        }

        #endregion Radius 依存関係プロパティ

        #region Thickness 依存関係プロパティ

        /// <summary>
        /// Thickness 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty ThicknessProperty
            = DependencyProperty.Register(
            nameof(Thickness),
            typeof(double),
            typeof(ProgressCircle),
            new PropertyMetadata(
                0d,
                (s, e) => (s as ProgressCircle)?.Render()));

        /// <summary>
        /// Thickness
        /// </summary>
        public double Thickness
        {
            get => (double)this.GetValue(ThicknessProperty);
            set => this.SetValue(ThicknessProperty, value);
        }

        #endregion Thickness 依存関係プロパティ

        #region Fill 依存関係プロパティ

        /// <summary>
        /// Fill 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty FillProperty
            = DependencyProperty.Register(
            nameof(Fill),
            typeof(Brush),
            typeof(ProgressCircle),
            new PropertyMetadata(
                Brushes.White,
                (s, e) => (s as ProgressCircle)?.Render()));

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
            typeof(ProgressCircle),
            new PropertyMetadata(
                Brushes.Gray,
                (s, e) => (s as ProgressCircle)?.Render()));

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
            typeof(ProgressCircle),
            new PropertyMetadata(
                0d,
                (s, e) => (s as ProgressCircle)?.Render()));

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
            typeof(ProgressCircle),
            new PropertyMetadata(
                false,
                (s, e) => (s as ProgressCircle)?.Render()));

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
            typeof(ProgressCircle),
            new PropertyMetadata(
                true,
                (s, e) => (s as ProgressCircle)?.Render()));

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
            typeof(ProgressCircle),
            new PropertyMetadata(
                true,
                (s, e) => (s as ProgressCircle)?.Render()));

        /// <summary>
        /// IsStrokeBackground Strokeを背景に対して設定するか？
        /// </summary>
        public bool IsStrokeBackground
        {
            get => (bool)this.GetValue(IsStrokeBackgroundProperty);
            set => this.SetValue(IsStrokeBackgroundProperty, value);
        }

        #endregion IsStrokeBackground 依存関係プロパティ

        #region IsCCW 依存関係プロパティ

        /// <summary>
        /// IsCCW 依存関係プロパティ
        /// </summary>
        public static readonly DependencyProperty IsCCWProperty
            = DependencyProperty.Register(
            nameof(IsCCW),
            typeof(bool),
            typeof(ProgressCircle),
            new PropertyMetadata(
                false,
                (s, e) => (s as ProgressCircle)?.Render()));

        /// <summary>
        /// IsCCW 反時計回りに回転するか？
        /// </summary>
        public bool IsCCW
        {
            get => (bool)this.GetValue(IsCCWProperty);
            set => this.SetValue(IsCCWProperty, value);
        }

        #endregion IsCCW 依存関係プロパティ

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
            // 大きさを設定する
            this.ForeCircle.Radius = this.Radius;
            this.ForeCircle.StrokeThickness = this.Thickness;

            // 背景色と前景色を設定する
            if (this.Fill is SolidColorBrush fill)
            {
                this.BackCircle.Stroke =
                    this.IsDarkBackground ?
                    fill.Color.ChangeBrightness(ToDarkRatio).ToBrush() :
                    fill.Color.ChangeBrightness(ToLightRatio).ToBrush();

                this.ForeCircle.Stroke = fill;
            }
            else
            {
                this.BackCircle.Stroke = Brushes.Black;
                this.ForeCircle.Stroke = this.Fill;
            }

            // 枠の色を設定する
            if (this.Stroke is SolidColorBrush stroke)
            {
                if (this.ForeCircle.Stroke == stroke)
                {
                    this.StrokeCircle.Stroke = this.BackCircle.Stroke;
                }
                else
                {
                    this.StrokeCircle.Stroke = stroke;
                }
            }
            else
            {
                this.StrokeCircle.Stroke = this.Stroke;
            }

            // サークルの角度を算出する
            var angle = !this.IsReverse ?
                (360.1 * this.Progress) - 90 :
                (360.1 - (360.1 * this.Progress)) - 90;

            // バーの幅を設定する
            this.ForeCircle.StartAngle = -90;
            this.ForeCircle.EndAngle = angle;

            // 枠の幅を設定する
            this.StrokeCircle.StartAngle = -90;
            this.StrokeCircle.EndAngle = this.IsStrokeBackground ? 270.1 : angle;

            // 反転？
            this.CircleScale.ScaleX = this.IsCCW ? -1 : 1;
        }
    }
}
