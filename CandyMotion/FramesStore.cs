using System.Windows.Media.Imaging;

namespace CandyMotion
{
    public class FramesStore
    {
        public WriteableBitmap CurrentFrame { get; set; }
        public WriteableBitmap PreviousFrame { get; set; }

        public void Add(WriteableBitmap frame)
        {
            if (CurrentFrame == null)
            { 
                CurrentFrame = frame; 
                return; 
            }

            if (CurrentFrame != null)
            {
                PreviousFrame = CurrentFrame;
                CurrentFrame = frame;
            }
        }
    }
}
