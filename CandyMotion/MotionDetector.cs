using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace CandyMotion
{
    public class MotionDetector : INotifyPropertyChanged
    {
        private DispatcherTimer timer;
        private FramesStore framesStore;
        public MotionDetector(CaptureSource myCaptureSource)
        {
            framesStore = new FramesStore();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += (s, e) => myCaptureSource.CaptureImageAsync();
            myCaptureSource.CaptureImageCompleted += (s, e) => FrameCaptured(e.Result);
        }

        public void Start()
        {
            timer.Start();
        }

        private WriteableBitmap detectionFrame;
        public WriteableBitmap DetectionFrame
        {
            get { return detectionFrame; }
            set { detectionFrame = value; NotifyPropertyChanged("DetectionFrame"); }
        }
        
        private void FrameCaptured(WriteableBitmap frame)
        {
            framesStore.Add(frame.ToGrayScale());
            DetectionFrame = GetDetectionFrame(framesStore);
            MotionDetected = DetectionFrame != null ? DetectMotion(DetectionFrame) : false;
        }

        private int detectionThreshold = 15;
        public int DetectionThreshold
        {
            get { return detectionThreshold; }
            set { detectionThreshold = value; }
        }

        private Color hilightColor = Colors.Cyan;
        
        private WriteableBitmap GetDetectionFrame(FramesStore framesStore)
        {
            if (framesStore.CurrentFrame == null || framesStore.PreviousFrame == null)
                return null;

            WriteableBitmap detectionFrame = new WriteableBitmap(framesStore.CurrentFrame.PixelWidth, framesStore.CurrentFrame.PixelHeight);
            for (int index = 0; index < framesStore.CurrentFrame.Pixels.Length; index++)
            {
                byte previousGrayPixel = BitConverter.GetBytes(framesStore.PreviousFrame.Pixels[index])[0];
                byte currentGrayPixel = BitConverter.GetBytes(framesStore.CurrentFrame.Pixels[index])[0];

                if (Math.Abs(previousGrayPixel - currentGrayPixel) > DetectionThreshold)
                {
                    detectionFrame.Pixels[index] = hilightColor.ToArgb();
                }
                else
                {
                    detectionFrame.Pixels[index] = framesStore.CurrentFrame.Pixels[index];
                }
            }
            return detectionFrame;
        }

        private bool motionDetected;
        public bool MotionDetected
        {
            get { return motionDetected; }
            set { motionDetected = value; NotifyPropertyChanged("MotionDetected"); }
        }
        private double detectionPercentage = 0.1;
        public double DetectionPercentage
        {
            get { return detectionPercentage; }
            set { detectionPercentage = value; }
        }
        
        private bool DetectMotion(WriteableBitmap detectionFrame)
        {
            var found = (from f in detectionFrame.Pixels
                         where f == hilightColor.ToArgb()
                         select f).Count();
            var perc = (100 * found) / detectionFrame.Pixels.Length;
            return perc >= (DetectionPercentage * 100);
        }
        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
