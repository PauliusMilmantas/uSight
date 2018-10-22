using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uSight
{
    //Represents a source of images. Does not work with live VideoCapture yet.
    public class ImageSource
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
    }
}
