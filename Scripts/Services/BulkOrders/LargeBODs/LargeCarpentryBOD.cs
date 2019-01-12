using System;
using System.Collections.Generic;

namespace Server.Engines.BulkOrders
{
    public class LargeCarpentryBOD : LargeBOD
    {
        public override BODType BODType { get { return BODType.Carpentry; } }

        public static double[] m_CarpentryingMaterialChances = new double[]
        {
            1.0, // None
            0.0, // Oak
            0.0, // Ash
            0.0, // Yew
            0.0, // Heartwood
            0.0, // Bloodwood
            0.0 // Frostwood
        };

        [Constructable]
        public LargeCarpentryBOD()
        {
            LargeBulkEntry[] entries;
            bool useMaterials = true;

            switch (Utility.Random(7))
            {
                default:
                case 0:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeArmoire);
                    break;
                case 1:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeCabinets);
                    break;
                case 2:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeChests);
                    break;
                case 3:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeElvenWeapons);
                    break;
                case 4:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeInstruments);
                    break;
                case 5:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeWeapons);
                    break;
                case 6:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeWoodFurniture);
                    break;
            }

            int hue = 1512;
            int amountMax = 20;
            bool reqExceptional = (0.825 > Utility.RandomDouble());

            BulkMaterialType material;

            if (useMaterials)
                material = GetRandomMaterial(BulkMaterialType.OakWood, m_CarpentryingMaterialChances);
            else
                material = BulkMaterialType.None;

            this.Hue = hue;
            this.AmountMax = amountMax;
            this.Entries = entries;
            this.RequireExceptional = reqExceptional;
            this.Material = material;
        }

        public LargeCarpentryBOD(int amountMax, bool reqExceptional, BulkMaterialType mat, LargeBulkEntry[] entries)
        {
            this.Hue = 1512;
            this.AmountMax = amountMax;
            this.Entries = entries;
            this.RequireExceptional = reqExceptional;
            this.Material = mat;
        }

        public LargeCarpentryBOD(Serial serial)
            : base(serial)
        {
        }

        public override int ComputeFame()
        {
            return CarpentryRewardCalculator.Instance.ComputeFame(this);
        }

        public override int ComputeGold()
        {
            return CarpentryRewardCalculator.Instance.ComputeGold(this);
        }

        public override List<Item> ComputeRewards(bool full)
        {
            List<Item> list = new List<Item>();

            RewardGroup rewardGroup = CarpentryRewardCalculator.Instance.LookupRewards(CarpentryRewardCalculator.Instance.ComputePoints(this));

            if (rewardGroup != null)
            {
                if (full)
                {
                    for (int i = 0; i < rewardGroup.Items.Length; ++i)
                    {
                        Item item = rewardGroup.Items[i].Construct();

                        if (item != null)
                            list.Add(item);
                    }
                }
                else
                {
                    RewardItem rewardItem = rewardGroup.AcquireItem();

                    if (rewardItem != null)
                    {
                        Item item = rewardItem.Construct();

                        if (item != null)
                            list.Add(item);
                    }
                }
            }

            return list;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}