using System;
using System.Collections.Generic;

namespace Server.Engines.BulkOrders
{
    public class LargeFletchingBOD : LargeBOD
    {
        public override BODType BODType { get { return BODType.Fletching; } }

        public static double[] m_FletchingingMaterialChances = new double[]
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
        public LargeFletchingBOD()
        {
            LargeBulkEntry[] entries;
            bool useMaterials = true;

            switch (Utility.Random(5))
            {
                default:
                case 0:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeHumanBows1);
                    break;
                case 1:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeHumanBows2);
                    break;
                case 2:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeAmmunition);
                    useMaterials = false;
                    break;
                case 3:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeElvenBows1);
                    break;
                case 4:
                    entries = LargeBulkEntry.ConvertEntries(this, LargeBulkEntry.LargeElvenBows2);
                    break;
            }

            int hue = 1425;
            int amountMax = 20;
            bool reqExceptional = useMaterials && 0.825 > Utility.RandomDouble();

            BulkMaterialType material;

            if (useMaterials)
                material = GetRandomMaterial(BulkMaterialType.OakWood, m_FletchingingMaterialChances);
            else
                material = BulkMaterialType.None;

            this.Hue = hue;
            this.AmountMax = amountMax;
            this.Entries = entries;
            this.RequireExceptional = reqExceptional;
            this.Material = material;
        }

        public LargeFletchingBOD(int amountMax, bool reqExceptional, BulkMaterialType mat, LargeBulkEntry[] entries)
        {
            this.Hue = 1425;
            this.AmountMax = amountMax;
            this.Entries = entries;
            this.RequireExceptional = reqExceptional;
            this.Material = mat;
        }

        public LargeFletchingBOD(Serial serial)
            : base(serial)
        {
        }

        public override int ComputeFame()
        {
            return FletchingRewardCalculator.Instance.ComputeFame(this);
        }

        public override int ComputeGold()
        {
            return FletchingRewardCalculator.Instance.ComputeGold(this);
        }

        public override List<Item> ComputeRewards(bool full)
        {
            List<Item> list = new List<Item>();

            RewardGroup rewardGroup = FletchingRewardCalculator.Instance.LookupRewards(FletchingRewardCalculator.Instance.ComputePoints(this));

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