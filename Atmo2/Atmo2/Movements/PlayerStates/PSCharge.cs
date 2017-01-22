using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atmo2.Entities;
using Indigo;
using Utility;
using Utility.Audio;

namespace Atmo2.Movements.PlayerStates
{
    class PSCharge : IPlayerState
    {
        private Player player;
        private float previous_charge_rate;
        private float charge_rate;

        public PSCharge(Player player, float chargeRate=10)
        {
            this.player = player;
            this.charge_rate = chargeRate;
        }
        public void OnEnter()
        {
            this.previous_charge_rate = player.EnergyRechargeRate;
            player.EnergyRechargeRate = charge_rate;

            // Animation
            player.image.Play("dash");

            // Sound
            if (Engine.Random.Chance(1f))
                AudioManager.PlaySoundVariations("charge2");
            else
                AudioManager.PlaySoundVariations("charge");
        }

        public void OnExit()
        {
            player.EnergyRechargeRate = previous_charge_rate;
        }

        public IPlayerState Update(GameTime time)
        {
            player.RefillEnergy(time);

            Enemy enemy = player.Collide(KQ.CollisionTypeEnemy, player.X, player.Y) as Enemy;
            if (enemy != null)
            {
                return new PSOuch(player, enemy.touchDamage);
            }

            if (!Controller.Down())
            {
                return new PSIdle(player);
            }
            return null;
        }
    }
}
