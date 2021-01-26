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
            public BasePatcher[] GamePatchers { get; }

            public GameProps(GameTitle game, BasePatcher[] patchers)
            {
                string currentDir = Directory.GetCurrentDirectory();

                Name = game;
                OriginalFolder = $@"{currentDir}\{Name}\original";
                ModdedFolder = $@"{currentDir}\{Name}\modded";
                PatchesFolder = $@"{currentDir}\{Name}\patches";
                GamePatchers = patchers;
            }
        }

        private static GameProps[] gamesList =
        {
            new GameProps(GameTitle.P4G, new BasePatcher[]
            {
                new Patchers.P4G.AicalcPatcher(),
                new Patchers.P4G.EffectPatcher(),
                new Patchers.P4G.EncountPatcher(),
                new Patchers.P4G.ModelPatcher(),
                new Patchers.P4G.MsgPatcher(),
                new Patchers.P4G.PersonaPatcher(),
                new Patchers.P4G.SkillPatcher(),
                new Patchers.P4G.UnitPatcher()
            }),
            new GameProps(GameTitle.P5, new BasePatcher[]
            {
                new Patchers.P5.AicalcPatcher(),
                new Patchers.P5.ElseAIPatcher(),
                new Patchers.P5.EncountPatcher(),
                new Patchers.P5.ItemPatcher(),
                new Patchers.P5.PersonaPatcher(),
                new Patchers.P5.PlayerPatcher(),
                new Patchers.P5.SkillPatcher(),
                new Patchers.P5.UnitPatcher(),
                new Patchers.P5.VisualPatcher()
            }),
            new GameProps(GameTitle.P3F, new BasePatcher[]
            {
                new Patchers.P3F.AicalcPatcher(),
                new Patchers.P3F.EffectPatcher(),
                new Patchers.P3F.EncountPatcher(),
                new Patchers.P3F.EncountFPatcher(),
                new Patchers.P3F.ModelPatcher(),
                new Patchers.P3F.MsgPatcher(),
                new Patchers.P3F.PersonaPatcher(),
                new Patchers.P3F.PersonaFPatcher(),
                new Patchers.P3F.SkillPatcher(),
                new Patchers.P3F.SkillFPatcher(),
                new Patchers.P3F.UnitPatcher(),
                new Patchers.P3F.UnitFPatcher()
            })
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Aemulus TBL Patcher");

            //_currentDir = Directory.GetCurrentDirectory();

            if (!SetupGames())
                return;

            GeneratePatches();

            Console.WriteLine("Enter any key to exit...");
            Console.ReadLine();
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

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[{game.Name}] Patcher");
                Console.ResetColor();

                // clear game patches folder
                if (!EmptyFolder(game.PatchesFolder))
                    return;

                // generate patches for each modded tbl file
                foreach (string modTbl in modTblFiles)
                {
                    List<PatchEdit> gameTblPatches = new List<PatchEdit>();

                    string tblFile = Path.GetFileName(modTbl);
                    string originalTbl = $@"{game.OriginalFolder}\{tblFile}";

                    // skip modded tbls with missing original counterpart
                    if (!File.Exists(originalTbl))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Couldn't find original TBL file! Missing TBL: {originalTbl}");
                        Console.ResetColor();
                        continue;
                    }

                    LoadTblPatches(game.GamePatchers, gameTblPatches, originalTbl, modTbl);

                    // skip tbl patches if not patches generated
                    if (gameTblPatches.Count < 1)
                        continue;

                    // output patch file for current game
                    try
                    {
                        string outputPatchFile = $@"{game.PatchesFolder}\{Path.GetFileNameWithoutExtension(originalTbl)}_Patches.tbp";

                        // prep patch json
                        Patch gamePatch = new Patch
                        {
                            Version = 1,
                            Patches = gameTblPatches.ToArray()
                        };

                        File.WriteAllText(outputPatchFile, JsonSerializer.Serialize(gamePatch, new JsonSerializerOptions { WriteIndented = true }));

                        Console.ForegroundColor = ConsoleColor.Green;
                        //Console.WriteLine($"[{game.Name}] Total Patches: {gameTblPatches.Count}");
                        Console.WriteLine($"[{game.Name}] Patch file created: {outputPatchFile}");
                        Console.ResetColor();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("Problem outputting game patch file!");
                        return;
                    }
                }
            }
        }

        private static void LoadTblPatches(BasePatcher[] gamePatchers, List<PatchEdit> allPatches, string originalTblPath, string moddedTblPath)
        {
            try
            {
                string tblName = Path.GetFileNameWithoutExtension(originalTblPath).ToUpper();
                // find patcher for current tbl
                int patcherIndex = Array.FindIndex(gamePatchers, (patcher) => patcher._tblName.Equals(tblName));

                // check if game has tbl patcher for tbl
                if (patcherIndex < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{tblName}: No patcher found for TBL!");
                    Console.ResetColor();
                    return;
                }

                BasePatcher tblPatcher = gamePatchers[patcherIndex];

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

        private static bool EmptyFolder(string folder)
        {
            try
            {
                string[] files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                    File.Delete(file);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine($"Problem clearing folder! Folder: {folder}");
                return false;
            }
        }
    }
}
