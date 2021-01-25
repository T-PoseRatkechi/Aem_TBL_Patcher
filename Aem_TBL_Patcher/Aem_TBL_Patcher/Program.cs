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
            public GameTitle Name { get; }
            public string OriginalFolder { get; }
            public string ModdedFolder { get; }
            public string PatchesFolder { get; }

            public GameProps(GameTitle game)
            {
                string currentDir = Directory.GetCurrentDirectory();

                Name = game;
                OriginalFolder = $@"{currentDir}\{Name}\original";
                ModdedFolder = $@"{currentDir}\{Name}\modded";
                PatchesFolder = $@"{currentDir}\{Name}\patches";
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

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"[{game.Name}] Patcher");
                Console.ResetColor();

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
                    LoadTblPatches(game.Name, gameTblPatches, originalTbl, modTbl);
                }

                if (gameTblPatches.Count < 1)
                {
                    Console.WriteLine("No patches generated!");
                    continue;
                }

                // output patch file for current game
                try
                {
                    string outputPatchFile = $@"{game.PatchesFolder}\Patches.tbp";

                    // prep patch json
                    Patch gamePatch = new Patch
                    {
                        Version = 1,
                        Patches = gameTblPatches.ToArray()
                    };

                    File.WriteAllText(outputPatchFile, JsonSerializer.Serialize(gamePatch, new JsonSerializerOptions { WriteIndented = true }));

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{game.Name}] Total Patches: {gameTblPatches.Count}");
                    Console.WriteLine($"[{game.Name}] Patch file created: {outputPatchFile}");
                    Console.ResetColor();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine("Problem outputting game patch file!");
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
                    Console.WriteLine("No patcher found for TBL!");
                    return;
                }

                BasePatcher tblPatcher = availablePatchers[game][tblName];

                byte[] originalBytes = File.ReadAllBytes(originalTblPath);
                byte[] moddedBytes = File.ReadAllBytes(moddedTblPath);

                allPatches.AddRange(tblPatcher.GetPatches(originalBytes, moddedBytes));
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
    }
}
