using System;
using System.Collections.Generic;
using System.Text;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using System.IO;

namespace MxdLayerDeleteConsole
{
    class Program
    {
        private static LicenseInitializer m_AOLicenseInitializer = new MxdLayerDeleteConsole.LicenseInitializer();

        [STAThread()]
        static void Main(string[] args)
        {
            //ESRI License Initializer generated code.
            m_AOLicenseInitializer.InitializeApplication(new esriLicenseProductCode[] { esriLicenseProductCode.esriLicenseProductCodeBasic },
            new esriLicenseExtensionCode[] { });
            //ESRI License Initializer generated code.

            string mappath = @"W:\Abstracts\3N-68W-10-W2W2\3N-68W-10_W2W2_new.mxd";

            MapDocument mdoc = new MapDocument();

            bool esriBullshit = mdoc.get_IsPresent(mappath);
            bool realShit = File.Exists(mappath);

            Console.WriteLine("esri says the document " + ((esriBullshit == true) ? "exists " : "does not exist"));

            try
            {
                mdoc.Open(mappath);
                Console.WriteLine("MapCount = " + mdoc.MapCount);
                var map = mdoc.get_Map(0);
                for (int i = 0; i < map.LayerCount; i++)
                {
                    ILayer layer = map.get_Layer(i);
                    Console.WriteLine("Info for layer " + i + ", " + layer.Name);
                    if (layer.Name == "Maps/Parcel_Base_WMAS")
                    {
                        Console.WriteLine("Deleting layer " + layer.Name);
                        map.DeleteLayer(layer);
                    }
                }
                mdoc.Save();
                mdoc.Close();
            }
            catch (Exception ex)
            {
                TextWriter writer = Console.Error;
                writer.WriteLine(String.Format("ERROR: {0}", ex.Message));
                writer.Flush();
                writer.Close();
            }

            //Do not make any call to ArcObjects after ShutDownApplication()
            m_AOLicenseInitializer.ShutdownApplication();
        }
    }
}
