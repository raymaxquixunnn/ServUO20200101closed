using System;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;
using System.Linq;

namespace Server.Engines.Harvest
{
    public class Lumberjacking : HarvestSystem
    {
        private static Lumberjacking m_System;

        public static Lumberjacking System
        {
            get
            {
                if (m_System == null)
                    m_System = new Lumberjacking();

                return m_System;
            }
        }

        private readonly HarvestDefinition  m_Wood;

        public HarvestDefinition lumber
        {
            get
            {
                return this.m_Wood;
            }
        }

        private Lumberjacking()
        {
            HarvestResource[] res;
            HarvestVein[] veins;

            #region Lumberjacking
            HarvestDefinition lumber = this.m_Wood = new HarvestDefinition();

            // Resource banks are every 4x3 tiles
            lumber.BankWidth = 4;
            lumber.BankHeight = 3;

            // Every bank holds from 20 to 45 logs
            lumber.MinTotal = 20000;
            lumber.MaxTotal = 45000;

            // A resource bank will respawn its content every 20 to 30 minutes
            lumber.MinRespawn = TimeSpan.FromMinutes(0.0);
            lumber.MaxRespawn = TimeSpan.FromMinutes(0.0);

            // Skill checking is done on the Lumberjacking skill
            lumber.Skill = SkillName.Lumberjacking;

            // Set the list of harvestable tiles
            lumber.Tiles = m_TreeTiles;

            // Players must be within 2 tiles to harvest
            lumber.MaxRange = 2;

            // Ten logs per harvest action
            lumber.ConsumedPerHarvest = 10000;
            lumber.ConsumedPerFeluccaHarvest = 20000;

            // The chopping effect
            lumber.EffectActions = new int[] { Core.SA ? 7 : 13 };
            lumber.EffectSounds = new int[] { 0x13E };
            lumber.EffectCounts = (Core.AOS ? new int[] { 1 } : new int[] { 1, 2, 2, 2, 3 });
            lumber.EffectDelay = TimeSpan.FromSeconds(0.0);
            lumber.EffectSoundDelay = TimeSpan.FromSeconds(0.1);

            lumber.NoResourcesMessage = 500493; // There's not enough wood here to harvest.
            lumber.FailMessage = 500495; // You hack at the tree for a while, but fail to produce any useable wood.
            lumber.OutOfRangeMessage = 500446; // That is too far away.
            lumber.PackFullMessage = 500497; // You can't place any wood into your backpack!
            lumber.ToolBrokeMessage = 500499; // You broke your axe.

            if (Core.ML)
            {
                res = new HarvestResource[]
                {
                    new HarvestResource(00.0, 00.0, 100.0, 1072540, typeof(Log), typeof(WoodGElemental)),
                    new HarvestResource(65.0, 25.0, 105.0, 1072541, typeof(OakLog), typeof(OakWoodGElemental)),
                    new HarvestResource(80.0, 40.0, 120.0, 1072542, typeof(AshLog), typeof(AshWoodGElemental)),
                    new HarvestResource(95.0, 55.0, 135.0, 1072543, typeof(YewLog), typeof(YewWoodGElemental)),
                    new HarvestResource(100.0, 60.0, 140.0, 1072544, typeof(HeartwoodLog), typeof(HeartwoodGElemental)),
                    new HarvestResource(105.0, 65.0, 145.0, 1072545, typeof(BloodwoodLog), typeof(BloodwoodGElemental)),
                    new HarvestResource(110.0, 70.0, 150.0, 1072546, typeof(FrostwoodLog), typeof(FrostwoodGElemental)),
                    new HarvestResource(115.0, 75.0, 155.0, 1072540, typeof(CalcitewoodLog), typeof(CalcitewoodGElemental)),
                    new HarvestResource(120.0, 80.0, 160.0, 1072540, typeof(GoldwoodLog), typeof(GoldwoodGElemental)),
                    new HarvestResource(125.0, 85.0, 165.0, 1072540, typeof(LabradoritewoodLog), typeof(LabradoritewoodGElemental)),
                    new HarvestResource(125.0, 85.0, 165.0, 1072540, typeof(MoldavitewoodLog), typeof(MoldavitewoodGElemental)),
                    new HarvestResource(130.0, 90.0, 170.0, 1072540, typeof(MorganitewoodLog), typeof(MorganitewoodGElemental)),
                    new HarvestResource(130.0, 90.0, 170.0, 1072540, typeof(QuartzwoodLog), typeof(QuartzwoodGElemental)),
                    new HarvestResource(135.0, 95.0, 175.0, 1072540, typeof(RhodonitewoodLog), typeof(RhodonitewoodGElemental)),
                    new HarvestResource(135.0, 95.0, 175.0, 1072540, typeof(RubywoodLog), typeof(RubywoodGElemental)),
                    new HarvestResource(140.0, 100.0, 180.0, 1072540, typeof(SapphirewoodLog), typeof(SapphirewoodGElemental)),
                    new HarvestResource(140.0, 100.0, 180.0, 1072540, typeof(SugilitewoodLog), typeof(SugilitewoodGElemental)),
                    new HarvestResource(145.0, 105.0, 185.0, 1072540, typeof(TanzanitewoodLog), typeof(TanzanitewoodGElemental)),
                    new HarvestResource(145.0, 105.0, 185.0, 1072540, typeof(TurquoisewoodLog), typeof(TurquoisewoodGElemental)),
                    new HarvestResource(150.0, 110.0, 190.0, 1072540, typeof(VarisitewoodLog), typeof(VarisitewoodGElemental)),
                    new HarvestResource(150.0, 110.0, 190.0, 1072540, typeof(GuardianswoodLog), typeof(GuardianswoodGElemental)),
                };

                veins = new HarvestVein[]
                {
					new HarvestVein(50.0000, 0.0, res[0], null),
					new HarvestVein(25.0000, 0.5, res[1], res[0]),
					new HarvestVein(12.5000, 0.5, res[2], res[0]),
					new HarvestVein(06.2500, 0.5, res[3], res[0]),
					new HarvestVein(03.1250, 0.5, res[4], res[0]),
					new HarvestVein(01.5625, 0.5, res[5], res[0]),
					new HarvestVein(00.7813, 0.5, res[6], res[0]),
					new HarvestVein(00.3906, 0.5, res[7], res[0]),
					new HarvestVein(00.1953, 0.5, res[8], res[0]),
					new HarvestVein(00.0977, 0.5, res[9], res[0]),
					new HarvestVein(00.0488, 0.5, res[10], res[0]),
					new HarvestVein(00.0244, 0.5, res[11], res[0]),
					new HarvestVein(00.0122, 0.5, res[12], res[0]),
					new HarvestVein(00.0061, 0.5, res[13], res[0]),
					new HarvestVein(00.0031, 0.5, res[14], res[0]),
					new HarvestVein(00.0015, 0.5, res[15], res[0]),
					new HarvestVein(00.0008, 0.5, res[16], res[0]),
					new HarvestVein(00.0004, 0.5, res[17], res[0]),
					new HarvestVein(00.0002, 0.5, res[18], res[0]),
					new HarvestVein(00.0001, 0.5, res[19], res[0]),
                };

                lumber.BonusResources = new BonusHarvestResource[]
                {
                    new BonusHarvestResource(0, 82.0, null, null), //Nothing
                    new BonusHarvestResource(100, 10.0, 1072548, typeof(BarkFragment)),
                    new BonusHarvestResource(100, 03.0, 1072550, typeof(LuminescentFungi)),
                    new BonusHarvestResource(100, 02.0, 1072547, typeof(SwitchItem)),
                    new BonusHarvestResource(100, 01.0, 1072549, typeof(ParasiticPlant)),
                    new BonusHarvestResource(100, 01.0, 1072551, typeof(BrilliantAmber)),
                    new BonusHarvestResource(100, 01.0, 1113756, typeof(CrystalShards), Map.TerMur),
                };
            }
            else
            {
                res = new HarvestResource[]
                {
                    new HarvestResource(00.0, 00.0, 100.0, 500498, typeof(Log))
                };

                veins = new HarvestVein[]
                {
                    new HarvestVein(100.0, 0.0, res[0], null)
                };
            }

            lumber.Resources = res;
            lumber.Veins = veins;

            lumber.RaceBonus = Core.ML;
            lumber.RandomizeVeins = false;

            this.Definitions.Add(lumber);
            #endregion
        }

        public override Type MutateType(Type type, Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource)
        {
            var newType = type;

            if (tool is HarvestersAxe && ((HarvestersAxe)tool).Charges > 0)
            {
                if (type == typeof(Log))
                    newType = typeof(Board);
                else if (type == typeof(OakLog))
                    newType = typeof(OakBoard);
                else if (type == typeof(AshLog))
                    newType = typeof(AshBoard);
                else if (type == typeof(YewLog))
                    newType = typeof(YewBoard);
                else if (type == typeof(HeartwoodLog))
                    newType = typeof(HeartwoodBoard);
                else if (type == typeof(BloodwoodLog))
                    newType = typeof(BloodwoodBoard);
                else if (type == typeof(FrostwoodLog))
                    newType = typeof(FrostwoodBoard);
                else if (type == typeof(CalcitewoodLog))
                    newType = typeof(CalcitewoodBoard);
                else if (type == typeof(GoldwoodLog))
                    newType = typeof(GoldwoodBoard);
                else if (type == typeof(LabradoritewoodLog))
                    newType = typeof(LabradoritewoodBoard);
                else if (type == typeof(MoldavitewoodLog))
                    newType = typeof(MoldavitewoodBoard);
                else if (type == typeof(MorganitewoodLog))
                    newType = typeof(MorganitewoodBoard);
                else if (type == typeof(QuartzwoodLog))
                    newType = typeof(QuartzwoodBoard);
                else if (type == typeof(RhodonitewoodLog))
                    newType = typeof(RhodonitewoodBoard);
                else if (type == typeof(RubywoodLog))
                    newType = typeof(RubywoodBoard);
                else if (type == typeof(SapphirewoodLog))
                    newType = typeof(SapphirewoodBoard);
                else if (type == typeof(SugilitewoodLog))
                    newType = typeof(SugilitewoodBoard);
                else if (type == typeof(TanzanitewoodLog))
                    newType = typeof(TanzanitewoodBoard);
                else if (type == typeof(TurquoisewoodLog))
                    newType = typeof(TurquoisewoodBoard);
                else if (type == typeof(VarisitewoodLog))
                    newType = typeof(VarisitewoodBoard);
                else if (type == typeof(GuardianswoodLog))
                    newType = typeof(GuardianswoodBoard);

                if (newType != type)
                {
                    ((HarvestersAxe)tool).Charges--;
                }
            }

            return newType;
        }

        public override Type GetResourceType(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, HarvestResource resource)
        {
            #region Void Pool Items
            HarvestMap hmap = HarvestMap.CheckMapOnHarvest(from, loc, def);

            if (hmap != null && hmap.Resource >= CraftResource.RegularWood && hmap.Resource <= CraftResource.Frostwood)
            {
                hmap.UsesRemaining--;
                hmap.InvalidateProperties();

                CraftResourceInfo info = CraftResources.GetInfo(hmap.Resource);

                if (info != null)
                    return info.ResourceTypes[0];
            }
            #endregion

            return base.GetResourceType(from, tool, def, map, loc, resource);
        }

        public override void SendSuccessTo(Mobile from, Item item, HarvestResource resource)
        {
            if (item != null)
            {
                if (item != null && item.GetType().IsSubclassOf(typeof(BaseWoodBoard)))
                {
                    from.SendLocalizedMessage(1158776); // The axe magically creates boards from your logs.
                    return;
                }
                else
                {
                    foreach (var res in lumber.Resources.Where(r => r.Types != null))
                    {
                        foreach (var type in res.Types)
                        {
                            if (item.GetType() == type)
                            {
                                res.SendSuccessTo(from);
                                return;
                            }
                        }
                    }
                }
            }
            base.SendSuccessTo(from, item, resource);
        }

        public override bool CheckResources(Mobile from, Item tool, HarvestDefinition def, Map map, Point3D loc, bool timed)
        {
            if (HarvestMap.CheckMapOnHarvest(from, loc, def) == null)
                return base.CheckResources(from, tool, def, map, loc, timed);

            return true;
        }

        public override bool CheckHarvest(Mobile from, Item tool)
        {
            if (!base.CheckHarvest(from, tool))
                return false;

            return true;
        }

        public override bool CheckHarvest(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            if (!base.CheckHarvest(from, tool, def, toHarvest))
                return false;

            if (def == this.m_Wood && !(from is PlayerMobile && from.Skills[SkillName.Lumberjacking].Base >= 100.0 && ((PlayerMobile)from).SandMining))
            {
                this.OnBadHarvestTarget(from, tool, toHarvest);
                return false;
            }
            else if (from.Mounted)
            {
                from.SendLocalizedMessage(501864); // You can't mine while riding.
                return false;
            }
            else if (from.IsBodyMod && !from.Body.IsHuman)
            {
                from.SendLocalizedMessage(501865); // You can't mine while polymorphed.
                return false;
            }

            return true;
        }

        public override HarvestVein MutateVein(Mobile from, Item tool, HarvestDefinition def, HarvestBank bank, object toHarvest, HarvestVein vein)
        {
            if (tool is GargoylesPickaxe && def == this.m_Wood)
            {
                int veinIndex = Array.IndexOf(def.Veins, vein);

                if (veinIndex >= 0 && veinIndex < (def.Veins.Length - 1))
                    return def.Veins[veinIndex + 1];
            }

            return base.MutateVein(from, tool, def, bank, toHarvest, vein);
        }

        private static readonly int[] m_Offsets = new int[]
        {
            -1, -1,
            -1, 0,
            -1, 1,
            0, -1,
            0, 1,
            1, -1,
            1, 0,
            1, 1
        };

        public override void OnHarvestFinished(Mobile from, Item tool, HarvestDefinition def, HarvestVein vein, HarvestBank bank, HarvestResource resource, object harvested)
        {
            if (tool is GargoylesPickaxe && def == this.m_Wood && 0.1 > Utility.RandomDouble() && HarvestMap.CheckMapOnHarvest(from, harvested, def) == null)
            {
                HarvestResource res = vein.PrimaryResource;

                if (res == resource && res.Types.Length >= 3)
                {
                    try
                    {
                        Map map = from.Map;

                        if (map == null)
                            return;

                        BaseCreature spawned = Activator.CreateInstance(res.Types[2], new object[] { 25 }) as BaseCreature;

                        if (spawned != null)
                        {
                            int offset = Utility.Random(8) * 2;

                            for (int i = 0; i < m_Offsets.Length; i += 2)
                            {
                                int x = from.X + m_Offsets[(offset + i) % m_Offsets.Length];
                                int y = from.Y + m_Offsets[(offset + i + 1) % m_Offsets.Length];

                                if (map.CanSpawnMobile(x, y, from.Z))
                                {
                                    spawned.OnBeforeSpawn(new Point3D(x, y, from.Z), map);
                                    spawned.MoveToWorld(new Point3D(x, y, from.Z), map);
                                    spawned.Combatant = from;
                                    return;
                                }
                                else
                                {
                                    int z = map.GetAverageZ(x, y);

                                    if (Math.Abs(z - from.Z) < 10 && map.CanSpawnMobile(x, y, z))
                                    {
                                        spawned.OnBeforeSpawn(new Point3D(x, y, z), map);
                                        spawned.MoveToWorld(new Point3D(x, y, z), map);
                                        spawned.Combatant = from;
                                        return;
                                    }
                                }
                            }

                            spawned.OnBeforeSpawn(from.Location, from.Map);
                            spawned.MoveToWorld(from.Location, from.Map);
                            spawned.Combatant = from;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public override void OnHarvestStarted(Mobile from, Item tool, HarvestDefinition def, object toHarvest)
        {
            base.OnHarvestStarted(from, tool, def, toHarvest);

            if (Core.ML)
                from.RevealingAction();
        }

        public override void OnBadHarvestTarget(Mobile from, Item tool, object toHarvest)
        {
            //if (toHarvest is Mobile)
            //    ((Mobile)toHarvest).PrivateOverheadMessage(MessageType.Regular, 0x3B2, 500450, from.NetState); // You can only skin dead creatures.
            if (toHarvest is Item)
                ((Item)toHarvest).LabelTo(from, 500464); // Use this on corpses to carve away meat and hide
            else if (toHarvest is Targeting.StaticTarget || toHarvest is Targeting.LandTarget)
                from.SendLocalizedMessage(500489); // You can't use an axe on that.
            else
                from.SendLocalizedMessage(1005213); // You can't do that
        }

        #region Tile lists
        private static readonly int[] m_TreeTiles = new int[]
        {
            0x4CCA, 0x4CCB, 0x4CCC, 0x4CCD, 0x4CD0, 0x4CD3, 0x4CD6, 0x4CD8,
            0x4CDA, 0x4CDD, 0x4CE0, 0x4CE3, 0x4CE6, 0x4CF8, 0x4CFB, 0x4CFE,
            0x4D01, 0x4D41, 0x4D42, 0x4D43, 0x4D44, 0x4D57, 0x4D58, 0x4D59,
            0x4D5A, 0x4D5B, 0x4D6E, 0x4D6F, 0x4D70, 0x4D71, 0x4D72, 0x4D84,
            0x4D85, 0x4D86, 0x52B5, 0x52B6, 0x52B7, 0x52B8, 0x52B9, 0x52BA,
            0x52BB, 0x52BC, 0x52BD,
            0x4CCE, 0x4CCF, 0x4CD1, 0x4CD2, 0x4CD4, 0x4CD5, 0x4CD7, 0x4CD9,
            0x4CDB, 0x4CDC, 0x4CDE, 0x4CDF, 0x4CE1, 0x4CE2, 0x4CE4, 0x4CE5,
            0x4CE7, 0x4CE8, 0x4CF9, 0x4CFA, 0x4CFC, 0x4CFD, 0x4CFF, 0x4D00,
            0x4D02, 0x4D03, 0x4D45, 0x4D46, 0x4D47, 0x4D48, 0x4D49, 0x4D4A,
            0x4D4B, 0x4D4C, 0x4D4D, 0x4D4E, 0x4D4F, 0x4D50, 0x4D51, 0x4D52,
            0x4D53, 0x4D5C, 0x4D5D, 0x4D5E, 0x4D5F, 0x4D60, 0x4D61, 0x4D62,
            0x4D63, 0x4D64, 0x4D65, 0x4D66, 0x4D67, 0x4D68, 0x4D69, 0x4D73,
            0x4D74, 0x4D75, 0x4D76, 0x4D77, 0x4D78, 0x4D79, 0x4D7A, 0x4D7B,
            0x4D7C, 0x4D7D, 0x4D7E, 0x4D7F, 0x4D87, 0x4D88, 0x4D89, 0x4D8A,
            0x4D8B, 0x4D8C, 0x4D8D, 0x4D8E, 0x4D8F, 0x4D90, 0x4D95, 0x4D96,
            0x4D97, 0x4D99, 0x4D9A, 0x4D9B, 0x4D9D, 0x4D9E, 0x4D9F, 0x4DA1,
            0x4DA2, 0x4DA3, 0x4DA5, 0x4DA6, 0x4DA7, 0x4DA9, 0x4DAA, 0x4DAB,
            0x52BE, 0x52BF, 0x52C0, 0x52C1, 0x52C2, 0x52C3, 0x52C4, 0x52C5,
            0x52C6, 0x52C7
        };
        #endregion
    }
}
