using Parking.Models;
using Parking.Services;
using System;
using System.Drawing;
using System.Drawing.Printing;

namespace Parking.Utils
{
    public static class PrintHelper
    {
        private static PrintDocument printDocument;
        private static String printMode;
        private static VehiclesTypesService _vehiclesTypesService;
        private static PrintData _printData;
        

        public static void printTicket(PrintData printData)
        {
            printMode = "Ticket";
            _printData = printData;
            startPrint();
        }

        public static void printInvoice(PrintData printData)
        {
            printMode = "Factura";
            _printData = printData;
            startPrint();
        }

        private static void startPrint()
        {
            printDocument = new PrintDocument();
            printDocument.PrintPage += printPage;
            printDocument.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custom", 220, 1200);
            printDocument.Print();
        }


        private static void printPage(object sender, PrintPageEventArgs e)
        {
            _vehiclesTypesService = new VehiclesTypesService();
            String labelInfoVehicle = "Placa:";

            Graphics g = e.Graphics;
            Font fontNormal = new Font("Arial", 8, FontStyle.Regular);
            Font frontBold = new Font("Arial", 9, FontStyle.Bold);

            String typeVehicleSpanish = _vehiclesTypesService.GetVehicleTypeSpanish(_printData.VehicleType);


            if (typeVehicleSpanish.Equals("Bicicleta")){
                labelInfoVehicle = "Identificacion:";
            }

            int y = 10;

            if(printMode.Equals("Ticket"))
            {
                g.DrawString($"{_printData.ParkingName}", frontBold, Brushes.Black, new PointF(50, y)); //Needed centralized
                y += 20;
                g.DrawString($"Nit: {_printData.ParkingNit}", fontNormal, Brushes.Black, new PointF(50, y)); //Needed centralized
                y += 20;

                y += DrawWrappedText(g, _printData.ParkingAddress ?? "", fontNormal, Brushes.Black, 10, y, e.PageBounds.Width - 20) + 10;
                y += DrawWrappedText(g, $"Ingreso: {_printData.EntryTime}", fontNormal, Brushes.Black, 10, y, e.PageBounds.Width - 20) + 10;

                g.DrawString($"{labelInfoVehicle} {_printData.VehicleInfo}", fontNormal, Brushes.Black, new PointF(10, y));
                y += 20;
                g.DrawString($"Tipo: {typeVehicleSpanish}", fontNormal, Brushes.Black, new PointF(10, y));
                y += 20;
                g.DrawString($"Valor minuto: ${_printData.FeePerMinute}", fontNormal, Brushes.Black, new PointF(10, y));
                y += 40;

                g.DrawImage(BarcodeHelper.GenerateBarcodeImage(_printData.TicketCode, 120, 60), new Rectangle(50, y, 120, 60));
                y += 70;

                y += DrawWrappedText(g, _printData.TicketInfo ?? "", fontNormal, Brushes.Black, 10, y, e.PageBounds.Width - 20) + 10;

            }
            else if (printMode.Equals("Factura"))
            {
                g.DrawString($"{_printData.ParkingName}", frontBold, Brushes.Black, new PointF(50, y)); //Needed centralized
                y += 20;
                g.DrawString($"Nit: {_printData.ParkingNit}", fontNormal, Brushes.Black, new PointF(50, y)); //Needed centralized
                y += 20;
                y += DrawWrappedText(g, _printData.ParkingAddress ?? "", fontNormal, Brushes.Black, 10, y, e.PageBounds.Width - 20) + 10;
                
                g.DrawString($"{labelInfoVehicle} {_printData.VehicleInfo}", fontNormal, Brushes.Black, new PointF(10, y));
                y += 20;
                g.DrawString($"Tipo: {typeVehicleSpanish}", fontNormal, Brushes.Black, new PointF(10, y));
                y += 20;

                y += DrawWrappedText(g, $"Ingreso: {_printData.EntryTime}", fontNormal, Brushes.Black, 10, y, e.PageBounds.Width - 20) + 10;

                y += DrawWrappedText(g, $"Salida: {_printData.ExitTime}", fontNormal, Brushes.Black, 10, y, e.PageBounds.Width - 20) + 10;

                g.DrawString($"Valor minuto: ${_printData.FeePerMinute}", fontNormal, Brushes.Black, new PointF(10, y));
                y += 20;
                g.DrawString($"Pago total: {_printData.TotalPay}", fontNormal, Brushes.Black, new PointF(10, y));
                y += 20;

                var logo = LogoManager.LoadLogo();
                if (logo != null)
                {
                    g.DrawImage(logo, new Rectangle(50, y, 120, 60));
                    y += 70;
                    logo.Dispose(); // release copy
                }

                y += DrawWrappedText(g, _printData.BillInfo ?? "", fontNormal, Brushes.Black, 10, y, e.PageBounds.Width - 20) + 10;

            }


        }

        private static int DrawWrappedText(Graphics g, string text, Font font, Brush brush, int x, int y, int maxWidth)
        {
            RectangleF layoutRect = new RectangleF(x, y, maxWidth, 1000);
            g.DrawString(text, font, brush, layoutRect);

            SizeF size = g.MeasureString(text, font, maxWidth);
            return (int)size.Height;
        }





    }
}
