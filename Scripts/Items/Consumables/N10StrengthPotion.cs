using System;

namespace Server.Items
{
    public class N10StrengthPotion : BaseStrengthPotion
    {
        [Constructable]
        public N10StrengthPotion()
            : base(PotionEffect.Strength)
        {
        }

        public N10StrengthPotion(Serial serial)
            : base(serial)
        {
        }

		public override string DefaultName { get { return "N10StrengthPotion"; } }

        public override int StrOffset
        {
            get
            {
                return 10;
            }
        }
        public override TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromMinutes(20.0);
            }
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