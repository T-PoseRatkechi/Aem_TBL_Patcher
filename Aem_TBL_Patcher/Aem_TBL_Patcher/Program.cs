using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Aem_TBL_Patcher
{
    struct PatchEdit
    {
        public long Offset { get; set; }
        public byte[] BytesEdit { get; set; }
    }

    class Program
    {
        private static string currentDir = String.Empty;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            currentDir = Directory.GetCurrentDirectory();
            CreatePatches();
        }

        private static void CreatePatches()
        {
            string originalFolderDir = $@"{currentDir}\original";
            string moddedFolderDir = $@"{currentDir}\modded";
            string patchesFolderDir = $@"{currentDir}\patches";

            try
            {
                Directory.CreateDirectory(originalFolderDir);
                Directory.CreateDirectory(moddedFolderDir);
                Directory.CreateDirectory(patchesFolderDir);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            string[] modTblFiles = null;

            try
            {
                modTblFiles = Directory.GetFiles(moddedFolderDir, "*.tbl", SearchOption.TopDirectoryOnly);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (modTblFiles == null)
                return;


            foreach(string tblFile in modTblFiles)
            {
                string originalTblFile = $@"{originalFolderDir}\{Path.GetFileName(tblFile)}";

                if (!File.Exists(originalTblFile))
                {
                    Console.WriteLine($"Error: Missing Original TBL File: {Path.GetFileName(tblFile)}");
                    continue;
                }

                string tblTag = GetTblTag(Path.GetFileNameWithoutExtension(tblFile));
                if (tblTag == null)
                {
                    Console.WriteLine("TBL Tag was not found!");
                    return;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{tblTag}: Creating patches...");
                Console.ResetColor();
                try
                {
                    byte[] originalBytes = File.ReadAllBytes(originalTblFile);
                    byte[] moddedBytes = File.ReadAllBytes(tblFile);

                    Console.WriteLine($"Original TBL Size: {originalBytes.Length} bytes\nModded TBL Size: {moddedBytes.Length} bytes");
                    if (originalBytes.Length != moddedBytes.Length)
                    {
                        Console.WriteLine("Error: File size mismatch!");
                        return;
                    }

                    List<PatchEdit> patches = new List<PatchEdit>();

                    for (long byteIndex = 0, totalBytes = originalBytes.Length; byteIndex < totalBytes; byteIndex++)
                    {
                        byte currentOriginalByte = originalBytes[byteIndex];
                        byte currentModdedByte = moddedBytes[byteIndex];

                        // mismatched bytes indicating edited bytes
                        if (currentOriginalByte != currentModdedByte)
                        {
                            PatchEdit newPatch = new PatchEdit();
                            newPatch.Offset = byteIndex;

                            // read ahead for the edited bytes
                            for (long byteEditIndex = byteIndex, byteCount = 0; byteEditIndex < totalBytes; byteEditIndex++, byteCount++)
                            {
                                // exit loop once bytes match again
                                if (originalBytes[byteEditIndex] == moddedBytes[byteEditIndex])
                                {
                                    byteIndex = byteEditIndex;
                                    newPatch.BytesEdit = new byte[byteCount];
                                    Array.Copy(moddedBytes, byteIndex, newPatch.BytesEdit, 0, byteCount);
                                    break;
                                }
                            }

                            patches.Add(newPatch);
                        }
                    }

                    foreach (PatchEdit patch in patches)
                    {
                        Console.WriteLine($"Offset: {patch.Offset.ToString("X")} Length: {patch.BytesEdit.Length}");

                        string outputFile = $@"{currentDir}\patches\{tblTag}_{patch.Offset.ToString("X")}.tblpatch";
                        using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                        {
                            foreach (byte tagByte in Encoding.ASCII.GetBytes(tblTag))
                                fs.WriteByte(tagByte);
                            foreach (byte offsetByte in BitConverter.GetBytes(patch.Offset).Reverse())
                                fs.WriteByte(offsetByte);
                            foreach (byte editByte in patch.BytesEdit)
                                fs.WriteByte(editByte);
                        }
                    }

                    Console.WriteLine($"Total Patches: {patches.Count}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static string GetTblTag(string tblName)
        {
            switch (tblName)
            {
                case "SKILL":
                    return "SKL";
                case "UNIT":
                    return "UNT";
                case "MSG":
                    return "MSG";
                case "PERSONA":
                    return "PSA";
                case "ENCOUNT":
                    return "ENC";
                case "EFFECT":
                    return "EFF";
                case "MODEL":
                    return "MDL";
                case "AICALC":
                    return "AIC";
                default:
                    return null;
            }
        }
    }
}
