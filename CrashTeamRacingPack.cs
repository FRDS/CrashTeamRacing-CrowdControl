using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrowdControl.Common;
using JetBrains.Annotations;

namespace CrowdControl.Games.Packs
{
    public static class Extensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    [UsedImplicitly]
    public class CrashTeamRacing : PS1EffectPack
    {
        public CrashTeamRacing(IPlayer player, Func<CrowdControlBlock, bool> responseHandler, Action<object> statusUpdateHandler) : base(player, responseHandler, statusUpdateHandler) { }

        private const uint ADDR_CHARACTER = 0x086E84;
        private const uint ADDR_TRACK = 0x08D0FC;
        private const uint ADDR_TRACK_B = 0x098530;
        private const uint ADDR_TIME = 0x098830;
        private const uint ADDR_PAUSE = 0x1FFF88;
        private const uint ADDR_MODE = 0x08D69C;
        private const uint ADDR_BACKCAM = 0x00A002;
        private const uint ADDR_NODRIFT = 0x00A004;
        private const uint ADDR_USEITEM = 0x00A006;
        private const uint ADDR_NOLEFT = 0x00A008;
        private const uint ADDR_NORIGHT = 0x00A00A;
        private const uint ADDR_AWARDA = 0x08FBA4;
        private const uint ADDR_AWARDB = 0x08FBA8;
        private const uint ADDR_AWARDC = 0x08FBAC;
        private const uint ADDR_AWARDD = 0x08FBB0;
        private const uint ADDR_DRIVER1 = 0x09900C;
        private const uint ADDR_CHEATS = 0x096B28;
        

        private uint previousSpeedAddress = 0;

        private bool init = true;

        private Random random = new Random(DateTime.Now.Millisecond);

        public enum ADDR_ITEM : uint
        {
            CrashCove       = 0x1EFC26,
            RooTubes        = 0x1D3522,
            RooTubesBoss    = 0x1BD90A,
            DingoCanyon     = 0x1C1032,
            DragonMines     = 0x1E7AC2,
            DragonMinesBoss = 0x1D3616,
            Sewer           = 0x1DC496,
            MysteryCaves    = 0x1F3D06,
            SkullRock       = 0x18A752,
            TigerTemple     = 0x1F2212,
            RampageRuins    = 0x18C8D6,
            PapuPyramid     = 0x1EB192,
            PapuPyramidBoss = 0x1D562E,
            CocoPark        = 0x1BE7F2,
            BlizzardBluff   = 0x1EB25E,
            TinyArena       = 0x1EEF7E,
            RockyRoad       = 0x1814F2,
            PolarPass       = 0x1F398E,
            HotAir          = 0x1F2B8A,
            HotAirBoss      = 0x1DDDAE,
            CortexCastle    = 0x1F340A,
            NGinLabs        = 0x1F2ACA,
            OxideStation    = 0x1F2C0A,
            OxideStationBoss= 0x1D7D32,
            NitroCourt      = 0x1992CA,
        }

        public enum TRACK : byte
        {
            DingoCanyon     = 0,
            DragonMines     = 1,
            BlizzardBluff   = 2,
            CrashCove       = 3,
            TigerTemple     = 4,
            PapuPyramid     = 5,
            RooTubes        = 6,
            HotAir          = 7,
            Sewer           = 8,
            MysteryCaves    = 9,
            CortexCastle    = 10,
            NGinLabs        = 11,
            PolarPass       = 12,
            OxideStation    = 13,
            CocoPark        = 14,
            TinyArena       = 15,
            Slide           = 16,
            Turbo           = 17,
            NitroCourt      = 18,
            RampageRuins    = 19,
            ParkingLot      = 20,
            SkullRock       = 21,
            NorthBowl       = 22,
            RockyRoad       = 23,
            LabBasement     = 24,
            Gemstone        = 25,
            WarpOne         = 26,
            WarpTwo         = 27,
            WarpThree       = 28,
            WarpFour        = 29,
            Intro           = 30,
            IntroCoco       = 31,
            IntroTiny       = 32,
            IntroPolar      = 33,
            IntroDingo      = 34,
            IntroCortex     = 35,
            IntroSpace      = 36,
            IntroMontage    = 37,
            IntroOxide      = 38,
            Menu            = 39,
            CharSelect      = 40,
            NDBox           = 41,
            EndAny          = 42,
            EndHund         = 43,
            Credits         = 44,
            CreditsCrash    = 45,
            CreditsTiny     = 46,
            CreditsCoco     = 47,
            CreditsNGin     = 48,
            CreditsDingo    = 49,
            CreditsPolar    = 50,
            CreditsPura     = 51,
            CreditsPin      = 52,
            CreditsPapu     = 53,
            CreditsRoo      = 54,
            CreditsJoe      = 55,
            CreditsTropy    = 56,
            CreditsPenta    = 57,
            CreditsFake     = 58,
            CreditsOxide    = 59,
            CreditsGreen    = 60,
            CreditsBlonde   = 61,
            CreditsBlack    = 62,
            CreditsBlue     = 63,
            BlackScreen     = 64
        }

        public enum CHARACTER : byte
        {
            Crash   = 0x00,
            Cortex  = 0x01,
            Tiny    = 0x02,
            Coco    = 0x03,
            NGin    = 0x04,
            Dingo   = 0x05,
            Polar   = 0x06,
            Pura    = 0x07,
            Pin     = 0x08,
            Papu    = 0x09,
            Roo     = 0x0A,
            Joe     = 0x0B,
            Tropy   = 0x0C,
            Penta   = 0x0D,
            Fake    = 0x0E
        }

        public enum ITEM : byte
        {
            Boost       = 0x00,
            Bomb        = 0x01,
            Missile     = 0x02,
            TNT         = 0x03,
            Potion      = 0x04,
            Spring      = 0x05,
            Shield      = 0x06,
            Mask        = 0x07,
            Clock       = 0x08,
            Warp        = 0x09,
            Invis       = 0x0C,
            SuperT      = 0x0D,
            Nothing     = 0x0F,
            Roulette    = 0x10
        }

        public enum PAUSE : byte
        {
            Unpaused = 0x00,
            Paused = 0x01,
            Loading = 0xFF
        }

        public enum MODE : byte
        {
            Trophy = 0x07, //Also overworld and CTR
            Relic = 0x01,
            Boss = 0x00 //Also battle
        }

        public enum Reward
        {
            Trophy,
            SRelic,
            GRelic,
            PRelic,
            CTR,
            Key
        }

        private Dictionary<string, (string name, byte id)> _items = new Dictionary<string, (string, byte)>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"boost", ("Boost", (byte)ITEM.Boost)},
            {"bomb", ("Bomb", (byte)ITEM.Bomb)},
            {"missile", ("Missile", (byte)ITEM.Missile)},
            {"tnt", ("TNT", (byte)ITEM.TNT)},
            {"potion", ("Potion", (byte)ITEM.Potion)},
            {"shield", ("Shield", (byte)ITEM.Shield)},
            {"mask", ("Mask", (byte)ITEM.Mask)},
            {"clock", ("Clock", (byte)ITEM.Clock)},
            {"warp", ("Warp", (byte)ITEM.Warp)}
            //{"invis", ("Invisible", (byte)ITEM.Invis)},
            //{"supert", ("Super Turbo", (byte)ITEM.SuperT)},
            //{"spring", ("Spring", (byte)ITEM.Spring)}
        };

        private Dictionary<string, (uint value, int bank, Reward reward)> _rewards = new Dictionary<string, (uint, int, Reward)>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"DCT", (0x40,0, Reward.Trophy)},
            {"DMT", (0x80,0, Reward.Trophy)},
            {"BBT", (0x100,0, Reward.Trophy)},
            {"CCT", (0x200,0, Reward.Trophy)},
            {"TTT", (0x400,0, Reward.Trophy)},
            {"PPT", (0x800,0, Reward.Trophy)},
            {"RTT", (0x1000,0, Reward.Trophy)},
            {"HAST", (0x2000,0, Reward.Trophy)},
            {"SST", (0x4000,0, Reward.Trophy)},
            {"MCT", (0x8000,0, Reward.Trophy)},
            {"CoCaT", (0x10000,0, Reward.Trophy)},
            {"NGLT", (0x20000,0, Reward.Trophy)},
            {"PoPaT", (0x40000,0, Reward.Trophy)},
            {"OST", (0x80000,0, Reward.Trophy)},
            {"CPT", (0x100000,0, Reward.Trophy)},
            {"TAT", (0x200000,0, Reward.Trophy)},
            {"DCSR", (0x400000,0, Reward.SRelic)},
            {"DMSR", (0x800000,0, Reward.SRelic)},
            {"BBSR", (0x1000000,0, Reward.SRelic)},
            {"CCSR", (0x2000000,0, Reward.SRelic)},
            {"TTSR", (0x4000000,0, Reward.SRelic)},
            {"PPSR", (0x8000000,0, Reward.SRelic)},
            {"RTSR", (0x10000000,0, Reward.SRelic)},
            {"HASSR", (0x20000000,0, Reward.SRelic)},
            {"SSSR", (0x40000000,0, Reward.SRelic)},
            {"MCSR", (0x80000000,0, Reward.SRelic)},
            {"CoCaSR", (0x1,1, Reward.SRelic)},
            {"NGLSR", (0x2,1, Reward.SRelic)},
            {"PoPaSR", (0x4,1, Reward.SRelic)},
            {"OSSR", (0x8,1, Reward.SRelic)},
            {"CPSR", (0x10,1, Reward.SRelic)},
            {"TASR", (0x20,1, Reward.SRelic)},
            {"SCSR", (0x40,1, Reward.SRelic)},
            {"TuTrSR", (0x80,1, Reward.SRelic)},
            {"DCGR", (0x100,1, Reward.GRelic)},
            {"DMGR", (0x200,1, Reward.GRelic)},
            {"BBGR", (0x400,1, Reward.GRelic)},
            {"CCGR", (0x800,1, Reward.GRelic)},
            {"TTGR", (0x1000,1, Reward.GRelic)},
            {"PPGR", (0x2000,1, Reward.GRelic)},
            {"RTGR", (0x4000,1, Reward.GRelic)},
            {"HASGR", (0x8000,1, Reward.GRelic)},
            {"SSGR", (0x10000,1, Reward.GRelic)},
            {"MCGR", (0x20000,1, Reward.GRelic)},
            {"CoCaGR", (0x40000,1, Reward.GRelic)},
            {"NGLGR", (0x80000,1, Reward.GRelic)},
            {"PoPaGR", (0x100000,1, Reward.GRelic)},
            {"OSGR", (0x200000,1, Reward.GRelic)},
            {"CPGR", (0x400000,1, Reward.GRelic)},
            {"TAGR", (0x800000,1, Reward.GRelic)},
            {"SCGR", (0x1000000,1, Reward.GRelic)},
            {"TuTrGR", (0x2000000,1, Reward.GRelic)},
            {"DCPR", (0x4000000,1, Reward.PRelic)},
            {"DMPR", (0x8000000,1, Reward.PRelic)},
            {"BBPR", (0x10000000,1, Reward.PRelic)},
            {"CCPR", (0x20000000,1, Reward.PRelic)},
            {"TTPR", (0x40000000,1, Reward.PRelic)},
            {"PPPR", (0x80000000,1, Reward.PRelic)},
            {"RTPR", (0x1,2, Reward.PRelic)},
            {"HASPR", (0x2,2, Reward.PRelic)},
            {"SSPR", (0x4,2, Reward.PRelic)},
            {"MCPR", (0x8,2, Reward.PRelic)},
            {"CoCaPR", (0x10,2, Reward.PRelic)},
            {"NGLPR", (0x20,2, Reward.PRelic)},
            {"PoPaPR", (0x40,2, Reward.PRelic)},
            {"OSPR", (0x80,2, Reward.PRelic)},
            {"CoPaPR", (0x100,2, Reward.PRelic)},
            {"TAPR", (0x200,2, Reward.PRelic)},
            {"SCPR", (0x400,2, Reward.PRelic)},
            {"TuTrPR", (0x800,2, Reward.PRelic)},
            {"DCC", (0x1000,2, Reward.CTR)},
            {"DMC", (0x2000,2, Reward.CTR)},
            {"BBC", (0x4000,2, Reward.CTR)},
            {"CCC", (0x8000,2, Reward.CTR)},
            {"TTC", (0x10000,2, Reward.CTR)},
            {"PPC", (0x20000,2, Reward.CTR)},
            {"RTC", (0x40000,2, Reward.CTR)},
            {"HASC", (0x80000,2, Reward.CTR)},
            {"SSC", (0x100000,2, Reward.CTR)},
            {"MCC", (0x200000,2, Reward.CTR)},
            {"CoCaC", (0x400000,2, Reward.CTR)},
            {"NGLC", (0x800000,2, Reward.CTR)},
            {"PoPaC", (0x1000000,2, Reward.CTR)},
            {"OSC", (0x2000000,2, Reward.CTR)},
            {"CPC", (0x4000000,2, Reward.CTR)},
            {"TAC", (0x8000000,2, Reward.CTR)}
        };

        private Dictionary<string, (string name, byte id, uint raceOffset, int bossOffset, int relicOffset)> _chars = new Dictionary<string, (string, byte, uint, int, int)>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"crash", ("Crash", (byte)CHARACTER.Crash, 0x0, 0, 0)},
            {"coco", ("Coco", (byte)CHARACTER.Coco, 0x624, -5880, -2416)},
            {"tiny", ("Tiny", (byte)CHARACTER.Tiny, 0xD0, -3784, -320)},
            {"dingo", ("Dingodile", (byte)CHARACTER.Dingo, 0x21C, 368, 3832)},
            {"cortex", ("Cortex", (byte)CHARACTER.Cortex, 0x750, -638, 1872)},
            {"ngin", ("N. Gin", (byte)CHARACTER.NGin, 0x1D8, -3560, -96)},
            {"polar", ("Polar", (byte)CHARACTER.Polar, 0x5E4, -10036, -5064)},
            {"pura", ("Pura", (byte)CHARACTER.Pura, 0x4BC, -444, 3020)}
            //{"roo", ("Ripper Roo", (byte)CHARACTER.Roo)},
            //{"joe", ("Komodo Joe", (byte)CHARACTER.Joe)},
            //{"papu", ("Papu Papu", (byte)CHARACTER.Papu)},
            //{"pin", ("Pinstripe", (byte)CHARACTER.Pin)},
            //{"tropy", ("N. Tropy", (byte)CHARACTER.Tropy)},
        };

        private Dictionary<string, (string name, byte id, uint itemAddress, uint? bossItemAddress, uint speedAddressRelic, uint speedAddressCTR, uint? speedAddressBoss)> _tracks = new Dictionary<string, (string name, byte id, uint itemAddress, uint? bossItemAddress, uint speedAddressRelic, uint speedAddressCTR, uint? speedAddressBoss)>(StringComparer.InvariantCultureIgnoreCase)
        {
            {"CrashCove", ("Crash Cove", (byte)TRACK.CrashCove, (uint)ADDR_ITEM.CrashCove, null, 0x1DF2C5, 0x1EFF7D, null) },
            {"RooTubes", ("Roo's Tubes", (byte)TRACK.RooTubes, (uint)ADDR_ITEM.RooTubes, (uint)ADDR_ITEM.RooTubesBoss, 0x1C22B5, 0x1D3879, 0x1BDC61) },
            {"Sewer", ("Sewer Speedway", (byte)TRACK.Sewer, (uint)ADDR_ITEM.Sewer, null, 0x1CE229,0x1DC7ED,null) },
            {"MysteryCaves", ("Mystery Caves", (byte)TRACK.MysteryCaves, (uint)ADDR_ITEM.MysteryCaves, null,0x1E2ADD,0x1F405D,null) },
            {"BlizzardBluff", ("Blizzard Bluff", (byte)TRACK.BlizzardBluff, (uint)ADDR_ITEM.BlizzardBluff, null,0x1DAD85,0x1EB5B5,null) },
            {"PolarPass", ("Polar Pass", (byte)TRACK.PolarPass, (uint)ADDR_ITEM.PolarPass, null,0x1E2765,0x1F3CE5,null) },
            {"TinyArena", ("Tiny Arena", (byte)TRACK.TinyArena, (uint)ADDR_ITEM.TinyArena, null,0x1E2A1D,0x1EF2D5,null) },
            {"DragonMines", ("Dragon Mines", (byte)TRACK.DragonMines, (uint)ADDR_ITEM.DragonMines, (uint)ADDR_ITEM.DragonMinesBoss,0x1D6A4D,0x1E7E19,0x1D396D) },
            {"CocoPark", ("Coco Park", (byte)TRACK.CocoPark, (uint)ADDR_ITEM.CocoPark, null,0x1AEF01,0x1BEB49,null) },
            {"PapuPyramid", ("Papu Pyramid", (byte)TRACK.PapuPyramid, (uint)ADDR_ITEM.PapuPyramid, (uint)ADDR_ITEM.PapuPyramidBoss,0x1DD031,0x1EB4E9,0x1D5985) },
            {"TigerTemple", ("Tiger Temple", (byte)TRACK.TigerTemple, (uint)ADDR_ITEM.TigerTemple, null,0x1E3659,0x1F2569,null) },
            {"DingoCanyon", ("Dingo Canyon", (byte)TRACK.DingoCanyon, (uint)ADDR_ITEM.DingoCanyon, null,0x1F2569,0x1C1389,null) },
            {"NGinLabs", ("N.Gin Labs", (byte)TRACK.NGinLabs, (uint)ADDR_ITEM.NGinLabs, null,0x1E382D,0x1F2E21,null) },
            {"CortexCastle", ("Cortex Castle", (byte)TRACK.CortexCastle, (uint)ADDR_ITEM.CortexCastle, null,0x1E3005,0x1F3761,null) },
            {"HotAir", ("Hot-Air Speedway", (byte)TRACK.HotAir, (uint)ADDR_ITEM.HotAir, (uint)ADDR_ITEM.HotAirBoss,0x1DFC41,0x1F2EE1,0x1DE105) },
            {"OxideStation", ("Oxide Station", (byte)TRACK.OxideStation, (uint)ADDR_ITEM.OxideStation, (uint)ADDR_ITEM.OxideStationBoss,0x1E2D55,0x1F2F61,0x1D8089) },
            {"SkullRock", ("Skull Rock", (byte)TRACK.SkullRock, (uint)ADDR_ITEM.SkullRock, (uint)ADDR_ITEM.SkullRock,0,0,0x18AAA9) },
            {"RampageRuins", ("Rampage Ruins", (byte)TRACK.RampageRuins, (uint)ADDR_ITEM.RampageRuins, (uint)ADDR_ITEM.RampageRuins,0,0,0x18CC2D) },
            {"RockyRoad", ("Rocky Road", (byte)TRACK.RockyRoad, (uint)ADDR_ITEM.RockyRoad, (uint)ADDR_ITEM.RockyRoad,0,0,0x181849) },
            {"NitroCourt", ("Nitro Court", (byte)TRACK.NitroCourt, (uint)ADDR_ITEM.NitroCourt, (uint)ADDR_ITEM.NitroCourt,0,0,0x199621) }
        };

        public override List<Effect> Effects
        {
            get
            {
                List<Effect> effects = new List<Effect>
                {
                    new Effect("Give Item", "itemgive", ItemKind.Folder),
                    new Effect("Change Character", "charchange", ItemKind.Folder),
                    //new Effect("Team Frenzy (20s)", "frenzy"),
                    new Effect("Cut The Engine", "brake"){Duration=10},
                    new Effect("No Left", "noleft"){Duration=15},
                    new Effect("No Right", "noright"){Duration=15},
                    new Effect("Backwards Camera", "camera"){Duration=15},
                    //new Effect("Invisible", "invisible"),
                    new Effect("No Drifting", "nodrift"){Duration=20},
                    //new Effect("-1 Lap", "minuslap")
                    new Effect("Give Random Reward", "givereward"),
                    new Effect("Remove Random Reward", "takereward"),
                    new Effect("Icy Tracks","icy"){Duration=20}
                };

                effects.AddRange(_items.Select(t => new Effect($"Give Item: {t.Value.name}", $"item_{t.Key}", "itemgive")));
                effects.AddRange(_chars.Select(t => new Effect($"Change Character: {t.Value.name}", $"char_{t.Key}", "charchange")));
                return effects;
            }
        }

        public override List<Common.ItemType> ItemTypes => new List<Common.ItemType>();

        public override List<ROMInfo> ROMTable => new List<ROMInfo>(new[]
        {
            //new ROMInfo("Mega Man 2", null, Patching.Ignore, ROMStatus.ValidPatched,s => Patching.MD5(s, "caaeb9ee3b52839de261fd16f93103e6")),
            new ROMInfo("CTR - Crash Team Racing", null, Patching.Ignore, ROMStatus.ValidPatched,s => true)
        });

        public override Game Game { get; } = new Game(92, "CTR - Crash Team Racing", "CrashTeamRacing", "PS1", ConnectorType.PS1Connector);

        protected override bool IsReady(EffectRequest request) => true;

        protected override void RequestData(DataRequest request) => Respond(request, request.Key, null, false, $"Variable name \"{request.Key}\" not known");

        protected override void StartEffect(EffectRequest request)
        {
            if (!IsReady(request))
            {
                DelayEffect(request, TimeSpan.FromSeconds(2));
                return;
            }

            if(init)
            {
                init = false;
            }

            string[] codeParams = request.FinalCode.Split('_');

            switch (codeParams[0])
            {
                case "item":
                    var item = _items[codeParams[1]];
                    Connector.SendMessage(request.DisplayViewer + " gave you " + item.name);
                    GiveItem(item.id, request);
                    return;

                case "char":
                    var character = _chars[codeParams[1]];
                    ChangeCharacter(character.id, request);
                    Connector.SendMessage(request.DisplayViewer + " changed you to " + character.name);
                    return;
                
                //case "frenzy":
                //    UpdatePrevious();
                //    byte trackStorage = 0;
                //    var s = StartTimed(
                //        //request
                //        request,
                //        //start condition
                //            () => Connector.Read8(ADDR_TRACK, out trackStorage)
                //            && Connector.Read8(ADDR_TRACK_B, out byte b)
                //            && b == trackStorage
                //            && _tracks.Any(i => i.Value.id == trackStorage)
                //            && Connector.Read64LE(ADDR_TIME, out ulong t)
                //            && t > 1000
                //            && Connector.Read8(ADDR_PAUSE, out byte p)
                //            && p == (byte)PAUSE.Unpaused
                //            && Connector.Read8(GetItemAddress(), out byte i)
                //            && i == 0x0F
                //            && Connector.Read8(ADDR_MODE, out byte m)
                //            && m != (byte)MODE.Relic,
                //            //start action
                //            () => Connector.SendMessage(request.DisplayViewer + "is starting a Team Frenzy.")
                //            && Connector.Write8(GetItemAddress(), (byte)ITEM.Roulette),
                //            TimeSpan.FromSeconds(20),
                //            mutex: "item");
                //    s.WhenCompleted.Then(t =>
                //    {
                //        Connector.SendMessage("Frenzy over.");
                //    });
                //    return;

                case "brake":
                    Connector.SendMessage(request.DisplayViewer + " has stopped your engine");
                    StartTimed(request,
                        () => Connector.Read8(ADDR_TRACK, out byte trackStorage)
                            && Connector.Read8(ADDR_TRACK_B, out byte b)
                            && b == trackStorage
                            && _tracks.Any(i => i.Value.id == trackStorage)
                            && Connector.Read64LE(ADDR_TIME, out ulong t)
                            && t > 1000
                            && Connector.Read8(ADDR_PAUSE, out byte p)
                            && p == (byte)PAUSE.Unpaused,
                        () => Connector.Write8(GetSpeedAddress(), 0x00) && Connector.Freeze8(GetSpeedAddress(), 0x00) && SetSpeedAddress(GetSpeedAddress()));

                    return;
                case "camera":
                    Connector.SendMessage(request.DisplayViewer + " flipped your camera");
                    StartTimed(request,
                    () => Connector.Read8(ADDR_BACKCAM, out byte camState)
                    && camState == 0x00,
                    () => Connector.Write8(ADDR_BACKCAM, 0xFF) && Connector.Freeze8(ADDR_BACKCAM, 0xFF));
                    return;
                case "nodrift":
                    Connector.SendMessage(request.DisplayViewer + " diabled drifting");
                    StartTimed(request,
                    () => Connector.Read8(ADDR_NODRIFT, out byte driftState)
                    && driftState == 0x00,
                    () => Connector.Write8(ADDR_NODRIFT, 0xFF) && Connector.Freeze8(ADDR_NODRIFT, 0xFF));
                    return;
                case "noleft":
                    Connector.SendMessage(request.DisplayViewer + " disabled left turns");
                    StartTimed(request,
                    () => Connector.Read8(ADDR_NOLEFT, out byte revState)
                    && revState == 0x00
                    && Connector.Read8(ADDR_NORIGHT, out byte revStateB)
                    && revStateB == 0x00,
                    () => Connector.Write8(ADDR_NOLEFT, 0xFF) && Connector.Freeze8(ADDR_NOLEFT, 0xFF));
                    return;
                case "noright":
                    Connector.SendMessage(request.DisplayViewer + " disabled right turns");
                    StartTimed(request,
                    () => Connector.Read8(ADDR_NORIGHT, out byte revState)
                    && revState == 0x00
                    && Connector.Read8(ADDR_NOLEFT, out byte revStateB)
                    && revStateB == 0x00,
                    () => Connector.Write8(ADDR_NORIGHT, 0xFF) && Connector.Freeze8(ADDR_NORIGHT, 0xFF));
                    return;
                //case "invisible":
                //    byte trackStorageA = 0;
                //    StartTimed(
                //        //request
                //        request,
                //        //condition
                //        () => Connector.Read8(ADDR_TRACK, out trackStorageA)
                //        && Connector.Read8(ADDR_TRACK_B, out byte b)
                //        && b == trackStorageA
                //        && _tracks.Any(i => i.Value.id == trackStorageA)
                //        && Connector.Read64LE(ADDR_TIME, out ulong t)
                //        && t > 1000
                //        && Connector.Read8(ADDR_PAUSE, out byte p)
                //        && p == (byte)PAUSE.Unpaused
                //        && Connector.Read8(GetItemAddress(), out byte o)
                //        && o == (byte)ITEM.Nothing
                //        && Connector.Read8(ADDR_MODE, out byte m)
                //        && m != (byte)MODE.Relic,
                //        //action
                //        () => Connector.Write8(GetItemAddress(), (byte)ITEM.Invis) && Connector.Write8(ADDR_USEITEM, 0xFF),
                //        TimeSpan.FromSeconds(2)
                //        );
                //    ;
                //    return;
                case "givereward":
                    Connector.SendMessage(request.DisplayViewer + " gave you a random reward");
                    TryEffect(
                        request,
                        () => (Connector.Read32(ADDR_AWARDA, out uint awarda)
                        && awarda != 0xFFFF) ||
                        (Connector.Read32(ADDR_AWARDB, out uint awardb)
                        && awardb != 0xFFFF) ||
                        (Connector.Read32(ADDR_AWARDC, out uint awardc)
                        && awardc != 0xFFFF),
                        () => GiveReward());
                    return;
                case "takereward":
                    Connector.SendMessage(request.DisplayViewer + " took a random award");
                    TryEffect(
                        request,
                        () => (Connector.Read32(ADDR_AWARDA, out uint awarda)
                        && awarda != 0x0000) ||
                        (Connector.Read32(ADDR_AWARDB, out uint awardb)
                        && awardb != 0x0000) ||
                        (Connector.Read32(ADDR_AWARDC, out uint awardc)
                        && awardc != 0x0000),
                        () => TakeReward());
                    return;
                case "icy":
                    uint state = 0;
                    Connector.SendMessage(request.DisplayViewer + " made it icy");
                    StartTimed(request,
                    () => Connector.Read32(ADDR_CHEATS, out state)
                    && (state | 0x80000) > state,
                    () => Connector.Write32(ADDR_CHEATS, (state | 0x80000)));
                    return;
            }


        }

        protected override bool StopEffect(EffectRequest request)
        {
            switch (request.BaseCode)
            {
                case "brake":
                    Connector.Unfreeze(previousSpeedAddress);
                    return true;
                case "camera":
                    Connector.Unfreeze(ADDR_BACKCAM);
                    Connector.Write8(ADDR_BACKCAM, 0x00);
                    return true;
                case "nodrift":
                    Connector.Unfreeze(ADDR_NODRIFT);
                    Connector.Write8(ADDR_NODRIFT, 0x00);
                    return true;
                case "noleft":
                    Connector.Unfreeze(ADDR_NOLEFT);
                    Connector.Write8(ADDR_NOLEFT, 0x00);
                    return true;
                case "noright":
                    Connector.Unfreeze(ADDR_NORIGHT);
                    Connector.Write8(ADDR_NORIGHT, 0x00);
                    return true;
                case "invisible":
                    Connector.Unfreeze(ADDR_USEITEM);
                    Connector.Write8(ADDR_USEITEM, 0x00);
                    return true;
                case "icy":
                    uint state = 0;
                    Connector.Read32(ADDR_CHEATS, out state);
                    Connector.Write32(ADDR_CHEATS, state - 0x80000);
                    return true;
                default:
                    return true;
            }
        }

        private void GiveItem(byte Item, EffectRequest request)
        {
            byte trackStorage = 0;
            TryEffect(
                //request
                request,
                //condition
                () => Connector.Read8(ADDR_TRACK, out trackStorage)
                && Connector.Read8(ADDR_TRACK_B, out byte b)
                && b == trackStorage
                && _tracks.Any(i => i.Value.id == trackStorage)
                && Connector.Read64LE(ADDR_TIME, out ulong t)
                && t > 1000
                && Connector.Read8(ADDR_PAUSE, out byte p)
                && p == (byte)PAUSE.Unpaused
                && Connector.Read8(GetItemAddress(), out byte o)
                && o == (byte)ITEM.Nothing
                && Connector.Read8(ADDR_MODE, out byte m)
                && m != (byte)MODE.Relic,
                //action
                () => Connector.Write8(GetItemAddress(), Item)
                );
            //Connector.SendMessage(request.DisplayViewer + " to address " + GetItemAddress());
            return;
        }

        private bool GiveReward()
        {
            /*uint[] possible = new uint[] {0x1,0x2,0x4,0x8,
                                        0x10,0x20,0x40,0x80,
                                        0x100,0x200,0x400,0x800,
                                        0x1000,0x2000,0x4000,0x8000,
                                        0x10000,0x20000,0x40000,0x80000,
                                        0x100000,0x200000,0x400000,0x800000,
                                        0x1000000,0x2000000,0x4000000,0x8000000,
                                        0x10000000,0x20000000,0x40000000,0x80000000 };

            int rng = random.Next(1, 5);
            for (int j = rng; j < 5; j++)
            {
                uint address = 0;
                switch (j)
                {
                    case 1:
                        address = ADDR_AWARDA;
                        break;
                    case 2:
                        address = ADDR_AWARDB;
                        break;
                    case 3:
                        address = ADDR_AWARDC;
                        break;
                    case 4:
                        address = ADDR_AWARDD;
                        break;
                }
                Connector.Read32(address, out uint rewards);
                //Connector.SendMessage("Rng:" + j);
                if (rewards != 0xFFFF)
                {
                    int randomValue = random.Next(0, possible.Length);
                    uint possibilty = possible[randomValue];

                    for (int i = 0; i < possible.Length; i++)
                    {
                        //Connector.SendMessage("Rng:" + rng);
                        //Connector.SendMessage("Value:" + Convert.ToString(rewards, 2));
                        //Connector.SendMessage("Rng:" + j + " Possibilty:" + Convert.ToString(possibilty, 2));

                        uint output = rewards | possibilty;

                        //Connector.SendMessage("Output:" + Convert.ToString(output, 2));

                        if (output > rewards)
                        {
                            Connector.Write32LE(address, output);
                            return true;
                        }
                        randomValue++;
                        possibilty = possible[randomValue % possible.Length];
                    }


                }

            }

            return false;*/

            //select random starting bank



            List<string> rewardList = _rewards.Keys.ToList();
            rewardList.Shuffle();

            uint bank1;
            uint bank2;
            uint bank3;

            Connector.Read32(ADDR_AWARDA, out bank1);
            Connector.Read32(ADDR_AWARDB, out bank2);
            Connector.Read32(ADDR_AWARDC, out bank3);

            for(int i = 0; i < rewardList.Count; i++)
            {
                string reward = rewardList[i];
                int bank = _rewards[reward].bank;
                uint bankVal = 0;
                switch(bank)
                {
                    case 0:
                        bankVal = bank1;
                        break;
                    case 1:
                        bankVal = bank2;
                        break;
                    case 2:
                        bankVal = bank3;
                        break;
                }
                uint value = _rewards[reward].value;

                if((bankVal | value) > bankVal)
                {
                    bankVal = bankVal | value;

                    switch (bank)
                    {
                        case 0:
                            Connector.Write32(ADDR_AWARDA, bankVal);

                            break;
                        case 1:
                            Connector.Write32(ADDR_AWARDB, bankVal);
                            break;
                        case 2:
                            Connector.Write32(ADDR_AWARDC, bankVal);
                            break;
                    }

                    if (_rewards[reward].reward == Reward.GRelic)
                    {
                        string sreward = reward.Replace("GR", "SR");
                        int tempbank = _rewards[sreward].bank;
                        uint tempvalue = _rewards[sreward].value;
                        switch (tempbank)
                        {
                            case 0:
                                Connector.Write32(ADDR_AWARDA, bankVal | tempvalue);
                                break;
                            case 1:
                                Connector.Write32(ADDR_AWARDB, bankVal | tempvalue);
                                break;
                            case 2:
                                Connector.Write32(ADDR_AWARDC, bankVal | tempvalue);
                                break;
                        }

                        Connector.SendMessage("Giving:" + sreward);

                    }
                    else if (_rewards[reward].reward == Reward.PRelic)
                    {
                        string sreward = reward.Replace("PR", "SR");
                        int tempbank = _rewards[sreward].bank;
                        uint tempvalue = _rewards[sreward].value;
                        switch (tempbank)
                        {
                            case 0:
                                Connector.Write32(ADDR_AWARDA, bankVal | tempvalue);
                                break;
                            case 1:
                                Connector.Write32(ADDR_AWARDB, bankVal | tempvalue);
                                break;
                            case 2:
                                Connector.Write32(ADDR_AWARDC, bankVal | tempvalue);
                                break;
                        }

                        bankVal = bankVal | tempvalue;

                        string greward = reward.Replace("PR", "GR");
                        tempbank = _rewards[greward].bank;
                        tempvalue = _rewards[greward].value;
                        switch (tempbank)
                        {
                            case 0:
                                Connector.Write32(ADDR_AWARDA, bankVal | tempvalue);
                                break;
                            case 1:
                                Connector.Write32(ADDR_AWARDB, bankVal | tempvalue);
                                break;
                            case 2:
                                Connector.Write32(ADDR_AWARDC, bankVal | tempvalue);
                                break;
                        }

                        Connector.SendMessage("Giving:" + sreward);
                        Connector.SendMessage("Giving:" + greward);
                    }



                    Connector.SendMessage("Giving:" + reward);

                    return true;
                }
            }

            return true;
        }

        private bool TakeReward()
        {
            /*uint[] possible = new uint[] {0x1,0x2,0x4,0x8,
                                        0x10,0x20,0x40,0x80,
                                        0x100,0x200,0x400,0x800,
                                        0x1000,0x2000,0x4000,0x8000,
                                        0x10000,0x20000,0x40000,0x80000,
                                        0x100000,0x200000,0x400000,0x800000,
                                        0x1000000,0x2000000,0x4000000,0x8000000,
                                        0x10000000,0x20000000,0x40000000,0x80000000 };

            int rng = random.Next(1, 5);
            for (int j = rng; j < 5; j++)
            {
                uint address = 0;
                switch (j)
                {
                    case 1:
                        address = ADDR_AWARDA;
                        break;
                    case 2:
                        address = ADDR_AWARDB;
                        break;
                    case 3:
                        address = ADDR_AWARDC;
                        break;
                    case 4:
                        address = ADDR_AWARDD;
                        break;
                }
                Connector.Read32(address, out uint rewards);
                //Connector.SendMessage("Rng:" + j);
                if (rewards != 0)
                {
                    int randomValue = random.Next(0, possible.Length);
                    uint possibilty = possible[randomValue];

                    for (int i = 0; i < possible.Length; i++)
                    {
                        //Connector.SendMessage("Rng:" + rng);
                        //Connector.SendMessage("Value:" + Convert.ToString(rewards, 2));
                        //Connector.SendMessage("Rng:" + j + " Possibilty:" + Convert.ToString(possibilty, 2));

                        uint output = rewards & ~possibilty;

                        //Connector.SendMessage("Output:" + Convert.ToString(output, 2));

                        if (output < rewards)
                        {
                            Connector.Write32LE(address, output);
                            return true;
                        }
                        randomValue++;
                        possibilty = possible[randomValue % possible.Length];
                    }


                }

            }

            return false;*/

            List<string> rewardList = _rewards.Keys.ToList();
            rewardList.Shuffle();

            uint bank1;
            uint bank2;
            uint bank3;

            Connector.Read32(ADDR_AWARDA, out bank1);
            Connector.Read32(ADDR_AWARDB, out bank2);
            Connector.Read32(ADDR_AWARDC, out bank3);

            for (int i = 0; i < rewardList.Count; i++)
            {
                string reward = rewardList[i];
                int bank = _rewards[reward].bank;
                uint bankVal = 0;
                switch (bank)
                {
                    case 0:
                        bankVal = bank1;
                        break;
                    case 1:
                        bankVal = bank2;
                        break;
                    case 2:
                        bankVal = bank3;
                        break;
                }
                uint value = _rewards[reward].value;

                if ((bankVal & ~value) < bankVal)
                {
                    bankVal = bankVal & ~value;

                    switch (bank)
                    {
                        case 0:
                            Connector.Write32(ADDR_AWARDA, bankVal);

                            break;
                        case 1:
                            Connector.Write32(ADDR_AWARDB, bankVal);
                            break;
                        case 2:
                            Connector.Write32(ADDR_AWARDC, bankVal);
                            break;
                    }

                    if (_rewards[reward].reward == Reward.GRelic)
                    {
                        string sreward = reward.Replace("GR", "PR");
                        int tempbank = _rewards[sreward].bank;
                        uint tempvalue = _rewards[sreward].value;
                        switch (tempbank)
                        {
                            case 0:
                                Connector.Write32(ADDR_AWARDA, bankVal & ~tempvalue);
                                break;
                            case 1:
                                Connector.Write32(ADDR_AWARDB, bankVal & ~tempvalue);
                                break;
                            case 2:
                                Connector.Write32(ADDR_AWARDC, bankVal & ~tempvalue);
                                break;
                        }

                        Connector.SendMessage("Taking:" + sreward);

                    }
                    else if (_rewards[reward].reward == Reward.SRelic)
                    {
                        string sreward = reward.Replace("SR", "PR");
                        int tempbank = _rewards[sreward].bank;
                        uint tempvalue = _rewards[sreward].value;
                        switch (tempbank)
                        {
                            case 0:
                                Connector.Write32(ADDR_AWARDA, bankVal & ~tempvalue);
                                break;
                            case 1:
                                Connector.Write32(ADDR_AWARDB, bankVal & ~tempvalue);
                                break;
                            case 2:
                                Connector.Write32(ADDR_AWARDC, bankVal & ~tempvalue);
                                break;
                        }

                        bankVal = bankVal & ~tempvalue;

                        string greward = reward.Replace("SR", "GR");
                        tempbank = _rewards[greward].bank;
                        tempvalue = _rewards[greward].value;
                        switch (tempbank)
                        {
                            case 0:
                                Connector.Write32(ADDR_AWARDA, bankVal & ~tempvalue);
                                break;
                            case 1:
                                Connector.Write32(ADDR_AWARDB, bankVal & ~tempvalue);
                                break;
                            case 2:
                                Connector.Write32(ADDR_AWARDC, bankVal & ~tempvalue);
                                break;
                        }

                        Connector.SendMessage("Taking:" + sreward);
                        Connector.SendMessage("Taking:" + greward);
                    }



                    Connector.SendMessage("Taking:" + reward);

                    return true;
                }
            }

            return true;
        }

        private void ChangeCharacter(byte Character, EffectRequest request)
        {
            TryEffect(
                //request
                request,
                //condition
                () => Connector.Read8(ADDR_PAUSE, out byte p)
                && p == (byte)PAUSE.Unpaused
                && Connector.Read8(ADDR_MODE, out byte m)
                && m != (byte)MODE.Boss
                && m != (byte)MODE.Relic
                && Connector.Read64LE(ADDR_TIME, out ulong t)
                && t > 1000,
                //action
                () => Connector.Write8(ADDR_CHARACTER, Character)
                );
            return;
        }

        private uint GetItemAddress()
        {
            Connector.Read32LE(ADDR_DRIVER1, out uint driverAddress);
            return driverAddress - 0x80000000 + 0x36;
        }

        private uint GetSpeedAddress()
        {
            Connector.Read32LE(ADDR_DRIVER1, out uint driverAddress);
            return driverAddress - 0x80000000 + 0x38D;
        }

        private bool SetSpeedAddress(uint speedAddress)
        {
            previousSpeedAddress = speedAddress;
            return true;
        }

        public override bool StopAllEffects() => base.StopAllEffects();
    }
}
