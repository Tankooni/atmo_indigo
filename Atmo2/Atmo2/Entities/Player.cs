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

namespace Atmo2.Entities
{
	public class Player : Actor, Indigo.Loaders.IOgmoNodeHandler
	{
		public const float SPEED = 4.0f;
		public const float GRAVITY = 0.3f;

		public static Abilities Abilities = new Abilities();
		public static MovementInfo MovementInfo = new MovementInfo();
		public static Movement CurrentMove;

		public Action Jump { get; set; }
		public Action Dash { get; set; }

        public int Spice { get; set; }
        public float Energy { get; set; }
        public int MaxEnergy { get; set; }
        public float EnergyRechargeRate { get; set; }

		private Spritemap image;
		public Spritemap Wings;

		private List<Orb> orbs = new List<Orb>();

		private float resetPointX;
		private float resetPointY;

		public Player(float x, float y)
			: base(x, y)
		{
			image = new Spritemap(Library.Get<Texture>("content/image/Julep.png"), 87, 71);
            Spice = 100;
            Energy = 0f;
            MaxEnergy = 4;
            EnergyRechargeRate = 2f;
			image.RenderStep = 0;
			image.Add("walk", FP.MakeFrames(1, 8), 10, true);
			//image.Add("dash", FP.MakeFrames(9, 11), 10, true);
			//image.Add("jump", FP.MakeFrames(12, 13), 10, false);
			//image.Add("fall", FP.MakeFrames(14, 15), 10, true);
			//image.Add("hang", FP.MakeFrames(16, 16), 10, true);
			//image.Add("climb", FP.MakeFrames(17, 19), 10, true);
			//image.Add("slide", FP.MakeFrames(20, 21), 10, true);
			image.Add("stand", FP.MakeFrames(0, 0), 0, true);
			image.Play("stand");

			Wings = new Spritemap(Library.Get<Texture>("content/image/JulepJump.png"), 54, 29, OnWingsComplete);
			Wings.RenderStep = -2;
			Wings.Add("wings", FP.MakeFrames(0, 7), 15, false);
			Wings.Visible = false;

			AddComponent<Image>(Wings);
			AddComponent<Image>(image);
			
			image.OriginX = image.Width / 2;
			image.OriginY = image.Height;
			Wings.OriginX = Wings.Width / 2;
			Wings.OriginY = Wings.Height;
			//35, 70
			SetHitbox(24, 70, 12, 70);

			GameWorld.player = this;
			Type = KQ.CollisionTypePlayer;

			Abilities = new Abilities();
			MovementInfo = new MovementInfo();
			Dash = UseAbility(new Dash(this));
			Jump = UseAbility(new Jump(this));
			CurrentMove = null;
		}

		public Action UseAbility(Movement move)
		{
			move.Done = () => CurrentMove = null;

			return () =>
			{
				if (CurrentMove == null && move.Restart(MovementInfo))
					CurrentMove = move;
			};
		}

		public void OnWingsComplete()
		{
			Wings.Visible = false;
		}

		public void RefillMoves(GameTime time)
		{
            Energy = MathHelper.Clamp(
                time.Elapsed*EnergyRechargeRate + Energy, 0, MaxEnergy);
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

			GetInput(time);
			UpdateCamera();
        }

		public override void Squish()
		{
			World.Remove(this);
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

		public void GetInput(GameTime time)
		{
			MovementInfo.Reset();
			MovementInfo.OnGround = Collide(KQ.CollisionTypeSolid, X, Y + 1) != null;
			MovementInfo.AgainstWall += (Collide(KQ.CollisionTypeSolid, X + 1, Y) != null) ? 1 : 0;
			MovementInfo.AgainstWall -= (Collide(KQ.CollisionTypeSolid, X - 1, Y) != null) ? 1 : 0;

            if (Controller.Down() && MovementInfo.OnGround)
            {
                EnergyRechargeRate = 7;
            } else {
                EnergyRechargeRate = 2;
                if (Controller.Left())
                    MovementInfo.Move -= SPEED;
                if (Controller.Right())
                    MovementInfo.Move += SPEED;

                if (Controller.Jump())
                    Jump();
                else if (Controller.Dash())
                    Dash();
            }

			MovementInfo.VelY += GRAVITY;

			if (CurrentMove != null)
				CurrentMove.Update(time, MovementInfo);

			MoveX(MovementInfo.Move);
			MoveY(MovementInfo.VelY, OnLand);

			var anim = "stand";
			if(MovementInfo.OnGround)
			{
				RefillMoves(time);
				if (MovementInfo.Move != 0)
					anim = "walk";
			}
			else
			{
				if (MovementInfo.VelY > 0)
				{
					if (MovementInfo.AgainstWall != 0)
						anim = "slide";
					else
						anim = "fall";
				}
				else
					anim = "jump";
			}

			if(CurrentMove != null)
			{
				var moveAnim = CurrentMove.GetAnimation();
				if (moveAnim != null)
					anim = moveAnim;
			}

			image.Play(anim);
			if (MovementInfo.Move != 0)
			{
				image.FlippedX = Wings.FlippedX = MovementInfo.Move < 0;
			}
		}



		public void OnLand()
		{
			MovementInfo.VelY = 0;
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
	}
}