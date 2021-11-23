using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Slider.Custom
{
    /// <summary>
    /// ReNewSlider.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReNewSlider : UserControl
    {

        //Max Value. default는 100
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(float), typeof(ReNewSlider), new PropertyMetadata(100f));
        //Min Value. default는 0
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(float), typeof(ReNewSlider), new PropertyMetadata(0f));
        //슬라이더 값
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(float), typeof(ReNewSlider), new PropertyMetadata(0f));
        public static readonly DependencyProperty MoveXProperty =
            DependencyProperty.Register("MoveX", typeof(float), typeof(ReNewSlider), new PropertyMetadata(0f));


        public float Value //슬라이더 값
        {
            get => (float)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        public float Maximum //Max Value
        {
            get => (float)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
        public float Minimum //Min Value
        {
            get => (float)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }
        public float MoveX //MoveX Value
        {
            get => (float)GetValue(MoveXProperty);
            set => SetValue(MoveXProperty, value);
        }

        private bool _isMove = false;
        private Point ptStartMove;
        private Thickness mgnStartMove;
        private Point pt;

        public float _width = 0;

        public ReNewSlider()
        {
            InitializeComponent();

            Loaded += (s,e)=> { _width = (float)xbar.ActualWidth; };
        }

        private void SliderGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            _width = (float)xbar.ActualWidth;
        }

        public System.Windows.Media.Brush Fill
        { //슬라이더 버튼 색상
            set
            {
                ybar.Fill = value;
            }
        }

        private void ybar_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isMove) return;

            pt = e.GetPosition(this);

            MoveX = (float)(mgnStartMove.Left + (pt.X - ptStartMove.X));

            if(MoveX < 0f)
            {
                Value = Minimum;
                MoveX = 0;
                return;
            }
            
            if(MoveX > _width)
            {
                Value = Maximum;
                MoveX = _width;
                return;
            }

            Value = MoveX/_width * (Math.Abs(Maximum) + Math.Abs(Minimum)) + Minimum;

        }

        private void ybar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isMove = true;
            ptStartMove = e.GetPosition(this);
            mgnStartMove = ybar.Margin;
            ybar.CaptureMouse(); //영역 밖에서도 스크롤 유지
        }

        private void ybar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMove = false;
            ybar.ReleaseMouseCapture(); //영역 밖에서도 스크롤 유지 해제
        }

    }
}
