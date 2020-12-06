using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace Octo_FireFly.Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                return;
            }

            Update(args[0]).Wait();
        }

        private static async Task Update(string downloadUrl)
        {
            var updateFile = "update.zip";

            var httpClient = new HttpClient();
            var stream = await httpClient.GetStreamAsync(downloadUrl);
            var writer = new StreamWriter(updateFile);
            await stream.CopyToAsync(writer.BaseStream);
            writer.Close();

            if (File.Exists(updateFile))
            {
                ZipFile.ExtractToDirectory(updateFile, ".", true);
            }

            File.Delete(updateFile);

            // reboot
            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "sudo",
                        Arguments = "reboot",
                        RedirectStandardOutput = false,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };
                process.Start();
            }
            catch(Exception)
            {

            }
        }
    }
}
