﻿using System;
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

        private static EncountPatcher encPatcher = new EncountPatcher();

        static void Main(string[] args)
        {
            Console.WriteLine("Aemulus TBL Patcher");
            currentDir = Directory.GetCurrentDirectory();
            CreatePatches();
            Console.WriteLine("Enter any key to exit...");
            Console.ReadLine();
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
                if (Directory.Exists(patchesFolderDir))
                {
                    Directory.Delete(patchesFolderDir, true);
                }
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
                    Console.WriteLine("TBL tag was not found!");
                    return;
                }

                try
                {
                    byte[] originalBytes = File.ReadAllBytes(originalTblFile);
                    byte[] moddedBytes = File.ReadAllBytes(tblFile);

                    if (originalBytes.Length != moddedBytes.Length)
                    {
                        ConsoleError($"{Path.GetFileName(tblFile)} (Original): {originalBytes.Length} bytes\n{Path.GetFileName(tblFile)} (Modded): {moddedBytes.Length} bytes");
                        ConsoleError("Error: File size mismatch!");
                        return;
                    }


                    List<PatchEdit> patches = null;

                    if (tblTag.Equals("ENC"))
                    {
                        Console.WriteLine("Using encounter patcher");
                        patches = encPatcher.GetPatches(originalBytes, moddedBytes);
                    }
                    else
                    {
                        patches = new List<PatchEdit>();

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
                                        newPatch.BytesEdit = new byte[byteCount];
                                        Array.Copy(moddedBytes, byteIndex, newPatch.BytesEdit, 0, byteCount);
                                        byteIndex = byteEditIndex - 1;
                                        break;
                                    }
                                }

                                patches.Add(newPatch);
                            }
                        }
                    }

                    // skip tbl tags with no patches needed
                    if (patches.Count < 1)
                        continue;

                    StringBuilder tblLogBuilder = new StringBuilder();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"{tblTag}: Creating patches...");
                    Console.ResetColor();

                    foreach (PatchEdit patch in patches)
                    {
                        tblLogBuilder.AppendLine($"Offset: {patch.Offset.ToString("X")} Length: {patch.BytesEdit.Length}");

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

                    string logFilePath = $@"{currentDir}\log_{tblTag}.txt";
                    File.WriteAllText(logFilePath, tblLogBuilder.ToString());
                    Console.WriteLine($"{tblTag} Log: {logFilePath}");

                    Console.WriteLine($"Total Patches: {patches.Count}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void ConsoleError(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        // given a tbl tag and the current offset, calculates the correct offset the replacement should be
        private long GetStartOffset(string tblTag, string byteOffset)
        {

            return -1;
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
