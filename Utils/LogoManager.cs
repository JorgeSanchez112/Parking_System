using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Parking.Utils
{
    public static class LogoManager
    {
        private static readonly String logoFolderPath = Path.Combine(Application.StartupPath, "Resources", "Logo");
        private static readonly String logoFileName = "logoParking.png";
        private static readonly String logoFullPath = Path.Combine(logoFolderPath, logoFileName);

        public static String uploadAndSaveLogo()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagenes |*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Seleccionar logo";

                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Directory.CreateDirectory(logoFolderPath);

                        File.Copy(ofd.FileName, logoFullPath, true);

                        return logoFullPath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar el logo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            return null;
        }


        public static Image LoadLogo()
        {
            if (!File.Exists(logoFullPath))
                return null;

            try
            {
                using (var fs = new FileStream(logoFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    // Crea una copia interna de la imagen para que el archivo no quede bloqueado
                    using (var tempImg = Image.FromStream(fs))
                    {
                        return new Bitmap(tempImg);
                    }
                }
            }
            catch (Exception ex)
            {
                // Opcional: log del error
                Console.WriteLine($"Error loading logo: {ex.Message}");
                return null;
            }
        }

        public static String getLogoPath()
        {
            return File.Exists(logoFullPath) ? logoFullPath : null;
        }

        public static bool isLogoPathEmpty()
        {
            return string.IsNullOrEmpty(getLogoPath());
        }


    }
}
