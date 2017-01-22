using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atmo2.Entities;
using Glide;
using Indigo;
using Indigo.Content;
using Indigo.Graphics;
using Indigo.Utils;

namespace Atmo2.UI
{
    class HUD : Entity
    {
        private Player player;
        private Image img_health;
        private Text text_health;

        private readonly Color[] ENERGY_COLORS = {
            new Color(0xabffab),
            new Color(0xb1cafa),
            new Color(0xffffab),
            new Color(0xffabab)
        };
        private Image[] img_energy;
        private Emitter emt_energy_full;
        private ParticleDefinition emt_energy_full_pd;

        public HUD(Player player)
        {
            this.player = player;

            // Health!
            img_health = new Image(Library.Get<Texture>("content/image/white.png"));
            img_health.ScaleX = 100;
            img_health.ScaleY = 8;
            img_health.Color = new Color(0xAA0000);
            img_health.X = 6;
            img_health.Y = 6;
            img_health.ScrollX = 0;
            img_health.ScrollY = 0;
            img_health.RenderStep = 50;

            AddComponent(img_health);
            text_health = new Text(player.Spice.ToString());
            text_health.X = 6;
            text_health.Y = -1;
            text_health.ScrollX = 0;
            text_health.ScrollY = 0;
            text_health.RenderStep = 51;

            AddComponent(text_health);

            // Energy!
            img_energy = new Image[4];

            for(var i = 0; i < img_energy.Length; i++)
            {
                img_energy[i] = new Image(Library.Get<Texture>("content/image/white.png"));
                img_energy[i].ScaleY = 6;
                img_energy[i].ScaleX = 0;
                img_energy[i].X = 6 + 25 * i;
                img_energy[i].Y = 15;
                img_energy[i].ScrollX = 0;
                img_energy[i].ScrollY = 0;
                img_energy[i].RenderStep = 50;
                img_energy[i].Color = ENERGY_COLORS[i];

                AddComponent(img_energy[i]);
            }

            emt_energy_full = new Emitter(1, 1);
            emt_energy_full.ScrollX = 0;
            emt_energy_full.ScrollY = 0;
            emt_energy_full.X = 2 + 25*player.MaxEnergy;
            emt_energy_full.Y = 17;
            emt_energy_full.RenderStep = 55;

            emt_energy_full_pd = emt_energy_full.Define("full");
            emt_energy_full_pd.Lifetime.Duration = 0.4f;
            emt_energy_full_pd.Lifetime.Variance.Add = 0.2f;
            emt_energy_full_pd.Scale.From = 6;
            emt_energy_full_pd.Scale.To = 3;
            emt_energy_full_pd.Scale.Ease = Ease.BounceOut;
            emt_energy_full_pd.Motion.Distance = -5f;
            emt_energy_full_pd.Motion.DistanceVariance.Add = 15f;
            emt_energy_full_pd.Motion.Angle = -90;
            emt_energy_full_pd.Motion.AngleVariance.Add = 180;
            emt_energy_full_pd.Color.Variance.Subtract = new Color(0x333333);
            //emt_energy_full_pd.Scale.Ease
            //emt_energy_full_pd.Motion.Angle = 50f;
            AddComponent(emt_energy_full);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);

            img_health.ScaleX = player.Spice;
            text_health.String = player.Spice.ToString();

            for(var i = 0; i < img_energy.Length; i++)
            {
                img_energy[i].ScaleX = 25 * MathHelper.Clamp(player.Energy - i, 0, 1);
            }
            if(player.Energy >= player.MaxEnergy - 0.05f && player.MaxEnergy > 0)
            {
                emt_energy_full_pd.Color.From = ENERGY_COLORS[player.MaxEnergy-1];
                emt_energy_full.Emit(emt_energy_full_pd, 0, 0);
            }
        }
    }
}
