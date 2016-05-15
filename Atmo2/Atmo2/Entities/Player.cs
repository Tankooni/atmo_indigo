using Atmo2.Movements;
using Atmo2.Worlds;
using Indigo;
using Indigo.Content;
using Indigo.Graphics;
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
		public const float SPEED = 2.5f;
		public const float GRAVITY = 0.55f;
		private const float SECONDS_TO_REGAIN = 0.5f;

		public static Abilities Abilities = new Abilities();
		public static MovementInfo MovementInfo = new MovementInfo();
		public static Movement CurrentMove;

		public Action Jump { get; set; }
		public Action Dash { get; set; }

		private Spritemap image;
		private Spritemap wings;
		private Text orbs;
		private float resetPointX;
		private float resetPointY;



		public Player(float x, float y)
			: base(x, y)
		{
			image = new Spritemap(Library.Get<Texture>("content/image/JulepSprites.png"), 22, 29);
			image.RenderStep = -100;
			image.Add("walk", FP.MakeFrames(1, 8), 10, true);
			image.Add("dash", FP.MakeFrames(9, 11), 10, true);
			image.Add("jump", FP.MakeFrames(12, 13), 10, false);
			image.Add("fall", FP.MakeFrames(14, 15), 10, true);
			image.Add("hang", FP.MakeFrames(16, 16), 10, true);
			image.Add("climb", FP.MakeFrames(17, 19), 10, true);
			image.Add("slide", FP.MakeFrames(21, 22), 10, true);
			image.Add("stand", FP.MakeFrames(0, 0), 0, true)
				.Play();

			wings = new Spritemap(Library.Get<Texture>("content/image/JulepJump.png"), 54, 29, OnWingsComplete);
			wings.RenderStep = -99;
			wings.Add("wings", FP.MakeFrames(0, 7), 15, false);
			wings.Visible = false;

			AddComponent<Image>(wings);
			AddComponent<Image>(image);
			

			image.OriginX = image.Width / 2;
			image.OriginY = image.Height;
			wings.OriginX = wings.Width / 2;
			wings.OriginY = wings.Height;

			SetHitbox(16, 24, 8, 24);

			orbs = new Text("", 0, -40);
			AddComponent<Text>(orbs);
			GameWorld.Player = this;
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
			wings.Visible = false;
		}

		public void RefillMoves()
		{
			if(MovementInfo.MovesRemaining < MovementInfo.CurrentMaxMoves)
			{
				MovementInfo.MoveRefill += FP.Elapsed * 1 / SECONDS_TO_REGAIN;
				if((int)Math.Floor(MovementInfo.MoveRefill) == 1)
				{
					MovementInfo.MoveRefill = 0;
					MovementInfo.MovesRemaining++;
				}
			}
		}

		public override bool IsRiding(Solid solid)
		{
			return Bottom == solid.Top;
		}

		public void NodeHandler(System.Xml.XmlNode entity)
		{
		}

		public override void Update()
		{
			base.Update();

			GetInput();
			UpdateCamera();

			orbs.String = MovementInfo.MovesRemaining.ToString();
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

		public void GetInput()
		{
			MovementInfo.Reset();
			MovementInfo.OnGround = Collide(KQ.CollisionTypeSolid, X, Y + 1) != null;
			MovementInfo.AngainstWall += (Collide(KQ.CollisionTypeSolid, X + 1, Y) != null) ? 1 : 0;
			MovementInfo.AngainstWall -= (Collide(KQ.CollisionTypeSolid, X - 1, Y) != null) ? 1 : 0;

			if (Controller.Left())
				MovementInfo.Move -= SPEED;
			if (Controller.Right())
				MovementInfo.Move += SPEED;

			if (Controller.Jump())
				Jump();
			else if (Controller.Dash())
				Dash();

			MovementInfo.VelY += GRAVITY;

			if (CurrentMove != null)
				CurrentMove.Update(MovementInfo);

			MoveX(MovementInfo.Move);
			MoveY(MovementInfo.VelY, OnLand);

			var anim = "stand";
			if(MovementInfo.OnGround)
			{
				RefillMoves();
				if (MovementInfo.Move != 0)
					anim = "walk";
			}
			else
			{
				if (MovementInfo.VelY > 0)
				{
					//if (movementInfo.againstWall != 0)
						//anim = "slide";
					//else
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
				image.FlippedX = wings.FlippedX = MovementInfo.Move < 0;
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
			centerX = FP.Clamp(centerX, FP.HalfWidth, currentRoom.RealRoomMeta.width - FP.HalfWidth);
			centerY = FP.Clamp(centerY, FP.HalfHeight, currentRoom.RealRoomMeta.height - FP.HalfHeight);

			World.Camera.X = centerX;
			World.Camera.Y = centerY;
		}
	}
}
