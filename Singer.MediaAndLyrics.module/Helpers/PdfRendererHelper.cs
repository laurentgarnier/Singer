using PdfiumViewer;
using PdfiumViewer.Core;
using PdfiumViewer.Enums;
using System.Windows;

namespace Singer.MediaAndLyrics.Module.Helpers
{
    public static class PdfRendererHelper
    {
        public static readonly DependencyProperty PdfFileProperty =
            DependencyProperty.RegisterAttached("PdfFile", typeof(string), typeof(PdfRendererHelper), new UIPropertyMetadata(null, PdfFilePropertyChanged));

        public static PdfDocument GetPdfFile(DependencyObject obj)
        {
            return (PdfDocument)obj.GetValue(PdfFileProperty);
        }

        public static void SetPdfFile(DependencyObject obj, PdfDocument value)
        {
            obj.SetValue(PdfFileProperty, value);
        }

        public static void PdfFilePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PdfRenderer renderer = o as PdfRenderer;
            if (renderer != null)
            {
                var pdfFile = e.NewValue as string;

                renderer.PagesDisplayMode = PdfViewerPagesDisplayMode.SinglePageMode;
                renderer.OpenPdf(pdfFile);
                renderer.PagesDisplayMode = renderer.Document.PageCount > 1 ? PdfViewerPagesDisplayMode.BookMode : PdfViewerPagesDisplayMode.SinglePageMode;
            }
        }

        // Close pdf
        public static readonly DependencyProperty ClosePdfFileProperty =
            DependencyProperty.RegisterAttached("ClosePdfFile", typeof(bool), typeof(PdfRendererHelper), new UIPropertyMetadata(false, ClosePdfFilePropertyChanged));

        public static bool GetClosePdfFile(DependencyObject obj)
        {
            return (bool)obj.GetValue(ClosePdfFileProperty);
        }

        public static void SetClosePdfFile(DependencyObject obj, bool value)
        {
            obj.SetValue(ClosePdfFileProperty, value);
        }

        public static void ClosePdfFilePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PdfRenderer renderer = o as PdfRenderer;
            if (renderer != null)
            {
                if((bool)e.NewValue)
                    renderer.UnLoad();
            }
        }

    }
}
