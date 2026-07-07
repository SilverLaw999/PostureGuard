using System;
using OpenCvSharp;

namespace PostureGuard.Libraries
{
    public class FaceData
    {
        public double Y { get; set; }
        public double Height { get; set; }
        public bool IsValid => Y != -1;
    }

    public class VisionLibrary : IDisposable
    {
        private VideoCapture? _capture;
        private CascadeClassifier? _faceCascade;

        public void Init()
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_frontalface_default.xml");
            _capture = new VideoCapture(0);
            _faceCascade = new CascadeClassifier(path);
        }

        public FaceData GetFaceData()
        {
            using (Mat frame = new Mat())
            {
                _capture?.Read(frame);
                if (frame == null || frame.Empty()) return new FaceData { Y = -1 };

                var faces = _faceCascade?.DetectMultiScale(frame);
                if (faces != null && faces.Length > 0)
                {
                    return new FaceData { Y = faces[0].Y, Height = faces[0].Height };
                }
            }
            return new FaceData { Y = -1 };
        }

        public void Dispose()
        {
            _capture?.Dispose();
            _faceCascade?.Dispose();
        }
    }
}