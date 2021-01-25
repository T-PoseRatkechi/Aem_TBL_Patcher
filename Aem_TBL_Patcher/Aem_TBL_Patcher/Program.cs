using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Aem_TBL_Patcher
{

    public struct Patch
    {
        public int Version { get; set; }
        public PatchEdit[] Patches { get; set; }
    }

    public struct PatchEdit
    {
        public string comment { get; set; }
        public string tbl { get; set; }
        public int section { get; set; }
        public long offset { get; set; }
        public string data { get; set; }
    }

    class Program
    {
        private enum GameTitle
        {
            P4G,
            P5,
            P3F
        }

        readonly private struct GameProps
        {
            public GameTitle Game { get; }
            public string OriginalFolder { get; }
            public string ModdedFolder { get; }
            public string PatchesFolder { get; }

            public GameProps(GameTitle game)
            {
                string currentDir = Directory.GetCurrentDirectory();

                Game = game;
                OriginalFolder = $@"{currentDir}\{Game}\original";
                ModdedFolder = $@"{currentDir}\{Game}\modded";
                PatchesFolder = $@"{currentDir}\{Game}\patches";
            }
        }

        private static string _currentDir = String.Empty;

        private static GameProps[] gamesList = { new GameProps(GameTitle.P4G), new GameProps(GameTitle.P5), new GameProps(GameTitle.P3F) };

        private static Dictionary<GameTitle, Dictionary<string, BasePatcher>> availablePatchers = new Dictionary<GameTitle, Dictionary<string, BasePatcher>>()
        {
            {
                GameTitle.P4G,
                new Dictionary<string, BasePatcher>()
                {
                    { "ENCOUNT", new Patchers.P4G.EncountPatcher() }
                }
            },
            {
                GameTitle.P5,
                new Dictionary<string, BasePatcher>()
                {
                    { "ITEM", new Patchers.P5.ItemPatcher() },
                    { "ENCOUNT", new Patchers.P5.EncountPatcher() }
                }
            },
            {
                GameTitle.P3F,
                new Dictionary<string, BasePatcher>()
                {

                }
            }
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Aemulus TBL Patcher");

            _currentDir = Directory.GetCurrentDirectory();

            if (!SetupGames())
                return;

            GeneratePatches();

            //CreatePatches();

            //Console.WriteLine("Enter any key to exit...");
            //Console.ReadLine();
        }

        private static bool SetupGames()
        {
            foreach (GameProps game in gamesList)
            {
                try
                {
                    Directory.CreateDirectory(game.OriginalFolder);
                    Directory.CreateDirectory(game.ModdedFolder);
                    Directory.CreateDirectory(game.PatchesFolder);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Problem creating game folders!");
                    return false;
                }
            }

            return true;
        }

        private static void GeneratePatches()
        {
            // generate patches for each game
            foreach (GameProps game in gamesList)
            {
                // get list of tbl files from current game's modded folder
                string[] modTblFiles = GetTblFiles(game.ModdedFolder);

                // skip game if no modded tbls were found
                if (modTblFiles == null)
                    continue;

                Console.WriteLine($"[{game.Game}] Patcher");

                List<PatchEdit> gameTblPatches = new List<PatchEdit>();

                // generate patches for each modded tbl file
                foreach (string modTbl in modTblFiles)
                {
                    string tblFile = Path.GetFileName(modTbl).ToUpper();
                    string originalTbl = $@"{game.OriginalFolder}\{tblFile}";

                    // skip modded tbls with missing original counterpart
                    if (!File.Exists(originalTbl))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Couldn't find original TBL file! Missing TBL: {originalTbl}");
                        Console.ResetColor();
                        continue;
                    }

                    Console.WriteLine($"{tblFile}: Generating patches...");
                    LoadTblPatches(game.Game, gameTblPatches, originalTbl, modTbl);
                }
            }
        }

        private static void LoadTblPatches(GameTitle game, List<PatchEdit> allPatches, string originalTblPath, string moddedTblPath)
        {
            try
            {
                string tblName = Path.GetFileNameWithoutExtension(originalTblPath).ToUpper();

                // check if game has tbl patcher for tbl
                if (!availablePatchers[game].ContainsKey(tblName))
                {
                    Console.WriteLine("No patcher found for tbl!");
                    Console.ReadLine();
                    return;
                }

                BasePatcher tblPatcher = availablePatchers[game][tblName];

                byte[] originalBytes = File.ReadAllBytes(originalTblPath);
                byte[] moddedBytes = File.ReadAllBytes(moddedTblPath);

                allPatches.AddRange(tblPatcher.GetPatches(originalBytes, moddedBytes));
                Console.WriteLine(allPatches.Count);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Problem loading TBL patches!");
            }
        }

        // return list of tbl files located in folder
        private static string[] GetTblFiles(string folder)
        {
            try
            {
                string[] tblFiles = Directory.GetFiles(folder, "*.tbl", SearchOption.TopDirectoryOnly);

                if (tblFiles.Length > 0)
                    return tblFiles;
                else
                    return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine($"Problem getting list of TBL files! Directory: {folder}");
                return null;
            }
        }

        private static void CreatePatches()
        {
            string originalFolderDir = $@"{_currentDir}\original";
            string moddedFolderDir = $@"{_currentDir}\modded";
            string patchesFolderDir = $@"{_currentDir}\patches";

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

            // collection of all patches
            List<PatchEdit> allPatches = new List<PatchEdit>();

            foreach (string tblFile in modTblFiles)
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

                    /*
                    if (originalBytes.Length != moddedBytes.Length)
                    {
                        ConsoleError($"{Path.GetFileName(tblFile)} (Original): {originalBytes.Length} bytes\n{Path.GetFileName(tblFile)} (Modded): {moddedBytes.Length} bytes");
                        ConsoleError("Error: File size mismatch!");
                        return;
                    }
                    */

                    BasePatcher tblPatcher = GetPatcher(tblTag, originalBytes, moddedBytes);

                    // add current tbl patches to patches collection
                    if (tblPatcher != null)
                        allPatches.AddRange(tblPatcher.GetPatches(originalBytes, moddedBytes));

                    // skip tbl tags with no patches needed
                    //if (patches.Count < 1)
                    //    continue;

                    /*
                    StringBuilder tblLogBuilder = new StringBuilder();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"{tblTag}: Creating patches...");
                    Console.ResetColor();

                    foreach (PatchEdit patch in patches)
                    {
                        tblLogBuilder.AppendLine($"Offset: {patch.offset.ToString("X")} Length: {patch.data.Length}");

                        string outputFile = $@"{currentDir}\patches\{tblTag}_{patch.offset.ToString("X")}.tblpatch";
                        using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                        {
                            foreach (byte tagByte in Encoding.ASCII.GetBytes(tblTag))
                                fs.WriteByte(tagByte);
                            foreach (byte offsetByte in BitConverter.GetBytes(patch.offset).Reverse())
                                fs.WriteByte(offsetByte);
                            foreach (byte editByte in patch.data)
                                fs.WriteByte(editByte);
                        }
                    }

                    string logFilePath = $@"{currentDir}\log_{tblTag}.txt";
                    File.WriteAllText(logFilePath, tblLogBuilder.ToString());
                    Console.WriteLine($"{tblTag} Log: {logFilePath}");

                    Console.WriteLine($"Total Patches: {patches.Count}");
                    */
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }
            }

            Patch patch = new Patch() { 
                Version = 1,
                Patches = allPatches.ToArray(),
            };

            string outputFile = $@"{_currentDir}\patches\Patches.tbp";

            File.WriteAllText(outputFile, JsonSerializer.Serialize(patch, new JsonSerializerOptions() { WriteIndented = true }));
        }

        private static BasePatcher GetPatcher(string tblTag, byte[] original, byte[] modded)
        {
            BasePatcher patcher = null;

            Console.ForegroundColor = ConsoleColor.Cyan;

            switch (tblTag)
            {
                /*
                case "ENC":
                    Console.WriteLine("Using Encount Patcher");
                    patcher = new Patchers.P4G.EncountPatcher(original, modded);
                    break;
                case "SKL":
                    Console.WriteLine("Using Skill Patcher");
                    patcher = new Patchers.P4G.SkillPatcher(original, modded);
                    break;
                case "UNT":
                    Console.WriteLine("Using Unit Patcher");
                    patcher = new Patchers.P4G.UnitPatcher(original, modded);
                    break;
                case "PSA":
                    Console.WriteLine("Using Persona Patcher");
                    patcher = new Patchers.P4G.PersonaPatcher(original, modded);
                    break;
                case "MSG":
                    Console.WriteLine("Using Msg Patcher");
                    patcher = new Patchers.P4G.MsgPatcher(original, modded);
                    break;
                case "MDL":
                    Console.WriteLine("Using Model Patcher");
                    patcher = new Patchers.P4G.ModelPatcher(original, modded);
                    break;
                case "EFF":
                    Console.WriteLine("Using Effect Patcher");
                    patcher = new Patchers.P4G.EffectPatcher(original, modded);
                    break;
                case "AIC":
                    Console.WriteLine("Using Aicalc Patcher");
                    patcher = new Patchers.P4G.AicalcPatcher(original, modded);
                    break;
                */
                case "ITEM":
                    Console.WriteLine("Using P5 Item Patcher");
                    patcher = new Patchers.P5.ItemPatcher();
                    break;
                default:
                    break;
            }

            Console.ResetColor();

            return patcher;
        }

        private static string GetTblTag(string tblName)
        {
            return tblName switch
            {
                "SKILL" => "SKL",
                "UNIT" => "UNT",
                "MSG" => "MSG",
                "PERSONA" => "PSA",
                "ENCOUNT" => "ENC",
                "EFFECT" => "EFF",
                "MODEL" => "MDL",
                "AICALC" => "AIC",
                "ITEM" => "ITEM",
                _ => null,
            };
        }
    }
}
