using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CandyMotion
{
    public partial class MainPage : UserControl
    {
        private CaptureSource myCaptureSource;
        private MotionDetector myMotionDetector;
        
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;           
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            myCaptureSource = new CaptureSource();
            myMotionDetector = new MotionDetector(myCaptureSource);

            Binding detectionFrameBinding = new Binding {
                Source = myMotionDetector,
                Path = new PropertyPath("DetectionFrame")
            };
            imageBox.SetBinding(Image.SourceProperty, detectionFrameBinding);

            Binding motionBinding = new Binding
            {
                Source = myMotionDetector,
                Path = new PropertyPath("MotionDetected")
            };
            motionTick.SetBinding(CheckBox.IsCheckedProperty, motionBinding);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (myCaptureSource.State == CaptureState.Started) return;
            if (CaptureDeviceConfiguration.GetAvailableVideoCaptureDevices().Count >= 1 &&
                CaptureDeviceConfiguration.RequestDeviceAccess())
            {
                myCaptureSource.VideoCaptureDevice = CaptureDeviceConfiguration.GetDefaultVideoCaptureDevice();                
                VideoBrush video = new VideoBrush();
                video.SetSource(myCaptureSource);
                screen.Fill = video;
                myCaptureSource.Start();
            }
        }

        private void startDetection_Click(object sender, RoutedEventArgs e)
        {
            if (myCaptureSource.State == CaptureState.Started)
            {
                myMotionDetector.Start();
            }              
        }
    }
}
