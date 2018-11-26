using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace uSight_Web.Entities
{
    public interface IDetector
    {
        List<String> DetectLicensePlate(IInputArray img, List<IInputOutputArray> licensePlateImagesList, List<IInputOutputArray> filteredLicensePlateImagesList, List<RotatedRect> detectedLicensePlateRegionList);
    }
}