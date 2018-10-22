using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSight
{
    //Represents a source of images. Does not work with live VideoCapture yet.
    public class ImageSource : IEnumerable<Image<Bgr, byte>>
    {
        Image<Bgr, byte> Frame = null;
        VideoCapture stream;
        public DateTime Date { get; set; }

        public int Count
        {
            get
            {
                return Frame == null ? (int)stream.GetCaptureProperty(CapProp.FrameCount) : 1;
            }
            set { }
        }

        //Gets either the single frame or the specified one from the stream
        public Image<Bgr, byte> this[int index]
        {
            get
            {
                if (Frame == null)
                {
                    stream.SetCaptureProperty(CapProp.PosFrames, index);
                    return stream.QueryFrame().ToImage<Bgr, byte>();
                }
                else
                {
                    return Frame;
                }
            }
            set { }
        }

        public ImageSource(VideoCapture capture, DateTime date)
        {
            stream = capture;
            Date = date;
        }

        public ImageSource(Image<Bgr, byte> image, DateTime date)
        {
            Frame = image.Clone();
            Date = date;
        }

        IEnumerator<Image<Bgr, byte>> IEnumerable<Image<Bgr, byte>>.GetEnumerator()
        {
            return new ImageSourceEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ImageSourceEnumerator(this);
        }
    }

    public class ImageSourceEnumerator : IEnumerator<Image<Bgr, byte>>
    {
        private ImageSource imgSource;
        private int currentIndex = -1;

        public ImageSourceEnumerator(ImageSource imgSource)
        {
            this.imgSource = imgSource;
        }

        Image<Bgr, byte> IEnumerator<Image<Bgr, byte>>.Current => imgSource[currentIndex];

        object IEnumerator.Current => imgSource[currentIndex];

        bool IEnumerator.MoveNext()
        {
            currentIndex++;
            if (imgSource.Count == currentIndex)
            {
                return false;
            }
            return true;
        }

        void IEnumerator.Reset()
        {
            currentIndex = -1;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }

                imgSource = null;
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ImageSourceEnumerator() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
