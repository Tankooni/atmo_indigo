using Atmo2.Movements;
using Atmo2.Worlds;
using Indigo;
using Indigo.Content;
using Indigo.Graphics;
using Indigo.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.Audio;
using Atmo2.Movements.PlayerStates;
using System.Diagnostics;
using Indigo.Inputs;

namespace Atmo2.Entities
{
	public class Player : Actor, Indigo.Loaders.IOgmoNodeHandler
	{
        public PlayerController player_controller;
		public Abilities Abilities;
		public MovementInfo MovementInfo;

		public Action Jump { get; set; }
		public Action Dash { get; set; }

        private int spice;
        public int Spice
        {
            get { return spice; }
            set
            {
                spice = MathHelper.Clamp(value, 0, 100);
                if (spice == 0)
                    player_controller.NextState(
                        new PSDeath(this));
            }
        }
		private float energy;
        public float Energy
		{
			get { return energy; }
			set
			{
				energy = MathHelper.Clamp(value, 0, MaxEnergy);
			}
		}
        public int MaxEnergy { get; set; }
        public float EnergyRechargeRate { get; set; }

        public float JumpStrenth { get; set; }
        public float RunSpeed { get; set; }
        public bool IsInvincable { get; set; }

        public Spritemap image;
		public Spritemap Wings;

        public float Alpha
        {
            get { return image.Alpha; }
            set { image.Alpha = value; }
        }

		private Orb jumpOrb;
		private Orb dashOrb;
		private List<Orb> orbs = new List<Orb>();

		// Make this a property at some point
		public float resetPointX;
		public float resetPointY;

		public Player(float x, float y)
			: base(x, y)
		{
			image = new Spritemap(Library.Get<Texture>("content/image/Julep.png"), 87, 71);
            Spice = 100;
            Energy = 0f;
            MaxEnergy = 0;
            EnergyRechargeRate = 2f;

            JumpStrenth = 11f;
            RunSpeed = 4.0f;
			
			image.RenderStep = 1;
			image.Add("stand", FP.MakeFrames(0, 0), 0, true);
			image.Add("walk", FP.MakeFrames(1, 8), 10, true);
			image.Add("charge", FP.MakeFrames(9, 12), 10, true);
			image.Add("jump", FP.MakeFrames(13, 14), 10, true);
			image.Add("fall", FP.MakeFrames(15, 16), 10, true);
			//image.Add("hang", FP.MakeFrames(16, 16), 10, true);
			//image.Add("climb", FP.MakeFrames(17, 19), 10, true);
			image.Add("diveKick", FP.MakeFrames(17, 18), 10, true);
			image.Add("slide", FP.MakeFrames(19, 20), 10, true);
			image.Add("dash", FP.MakeFrames(21, 22), 10, true);
			image.Add("attackNormal", FP.MakeFrames(23, 28), 20, false);
			image.Add("attackDash", FP.MakeFrames(29, 14), 10, false);
			image.Play("stand");

			Wings = new Spritemap(Library.Get<Texture>("content/image/JulepJump.png"), 54, 29, OnWingsComplete);
			Wings.RenderStep = -2;
			Wings.Add("wings", FP.MakeFrames(0, 7), 15, false);
			Wings.Visible = false;

			//AddComponent<Image>(Wings);
			AddComponent<Image>(image);
			
			image.OriginX = image.Width / 2;
			image.OriginY = image.Height;
			Wings.OriginX = Wings.Width / 2;
			Wings.OriginY = Wings.Height;

			SetHitbox(24, 70, 12, 70);

			GameWorld.player = this;
			Type = KQ.CollisionTypePlayer;

			Abilities = new Abilities(this);
			MovementInfo = new MovementInfo(this);

            player_controller = new PlayerController(new PSIdle(this));

			image.Callback += () => { if(image.Complete) player_controller.AnimationComplete(); };

			AddResponse(PickupType.AirDash, OnAirDashPickup);
			AddResponse(PickupType.AirJump, OnAirJumpPickup);
			AddResponse(PickupType.Jump, OnJumpPickup);
			AddResponse(PickupType.Dash, OnDashPickup);
		}

		public void OnWingsComplete()
		{
			Wings.Visible = false;
		}

		public void RefillEnergy(GameTime time)
		{
			Energy = MaxEnergy;/*MathHelper.Clamp(
                time.Elapsed*EnergyRechargeRate + Energy, 0, MaxEnergy);*/
		}

		public override bool IsRiding(Solid solid)
		{
			return Bottom == solid.Top;
		}

		public void NodeHandler(System.Xml.XmlNode entity)
		{
		}

		public override void Update(GameTime time)
		{
			base.Update(time);
			
			if(Keyboard.Space.Pressed)
			{
				Console.WriteLine(player_controller.current_state.ToString());
			}

			player_controller.Update(time);
            MovementInfo.Update(time);
			
			UpdateCamera();

			if(Controller.Select())
			{
				OnAirDashPickup(null);
				OnAirJumpPickup(null);
				OnDashPickup(null);
				OnJumpPickup(null);
			}
        }

		public override void Squish()
		{
            //World.Remove(this);
            this.ResetPlayerPosition();
		}

		public void SetResetPointToCurrentLocation()
		{
			SetResetPoint(X, Y);
		}

		public void SetResetPoint(float x, float y)
		{
			resetPointX = x;
			resetPointY = y;
		}

		public void ResetPlayerPosition()
		{
			X = resetPointX;
			Y = resetPointY;
			UpdateCamera();
		}        

		public void UpdateCamera()
		{
			var centerX = X;
			var centerY = Y;

			var currentRoom = ((GameWorld)(World)).CurrentRoom;
			centerX = MathHelper.Clamp(centerX, Engine.HalfWidth, currentRoom.RealRoomMeta.width - Engine.HalfWidth);
			centerY = MathHelper.Clamp(centerY, Engine.HalfHeight, currentRoom.RealRoomMeta.height - Engine.HalfHeight);

			World.Camera.X = centerX;
			World.Camera.Y = centerY;
		}

		public void OnJumpPickup(object[] param)
		{
			Abilities.GoodJump = true;
			if (jumpOrb == null)
			{
				jumpOrb = new Orb(X, Y, OrbType.Yellow, orbs.Any() ? (Entity)orbs.First() : (Entity)this);
				orbs.Add(World.Add(jumpOrb));
			}
		}
		public void OnAirJumpPickup(object[] param)
		{
			Abilities.DoubleJump = true;
			if(MaxEnergy < 4)
				MaxEnergy++;
			if (jumpOrb == null)
			{
				jumpOrb = new Orb(X, Y, OrbType.Yellow, orbs.Any() ? (Entity)orbs.Last() : (Entity)this);
				orbs.Add(World.Add(jumpOrb));
			}
			else
				jumpOrb.IsActivated = true;
		}
		public void OnDashPickup(object[] param)
		{
			Abilities.GroundDash = true;
			if (dashOrb == null)
			{
				dashOrb = new Orb(X, Y, OrbType.Blue, orbs.Any() ? (Entity)orbs.Last() : (Entity)this);

				orbs.Add(World.Add(dashOrb));
			}
		}
		public void OnAirDashPickup(object[] param)
		{
			Abilities.AirDash = true;
			if (MaxEnergy < 4)
				MaxEnergy++;
			if (dashOrb == null)
			{
				dashOrb = new Orb(X, Y, OrbType.Blue, orbs.Any() ? (Entity)orbs.Last() : (Entity)this);
				orbs.Add(World.Add(dashOrb));
			}
			else
				dashOrb.IsActivated = true;
		}
	}
}