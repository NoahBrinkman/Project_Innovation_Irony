using shared;
using System;
using System.Collections.Generic;

namespace server
{
	/**
	 * This room runs a single Game (at a time). 
	 * 
	 * The 'Game' is very simple at the moment:
	 *	- all client moves are broadcasted to all clients
	 *	
	 * The game has no end yet (that is up to you), in other words:
	 * all players that are added to this room, stay in here indefinitely.
	 */
	class GameRoom : SimpleRoom
	{
		public bool IsGameInPlay { get; private set; }

		//wraps the board to play on...
		public int MemberCount => memberCount;
		public GameRoom(TCPGameServer pOwner) : base(pOwner)
		{
		}

		public void StartGame (List<TcpMessageChannel> players)
		{
			if (IsGameInPlay) throw new Exception("Programmer error duuuude.");

			IsGameInPlay = true;
			//GameRoomEnteredMessage gm = new GameRoomEnteredMessage();
			for (int i = 0; i < players.Count; i++)
			{
				addMember(players[i]);
			}
		//	sendToAll(gm);
		}

		protected override void addMember(TcpMessageChannel pMember)
		{
			base.addMember(pMember);

			//notify client he has joined a game room 
			RoomJoinedEvent roomJoinedEvent = new RoomJoinedEvent();
			roomJoinedEvent.room = RoomJoinedEvent.Room.GAME_ROOM;
			switch (_server.GetPlayerInfo(pMember).room)
			{
				case MinigameRoom.Mining:
					roomJoinedEvent.miningGameChosen = true;
					break;
                case MinigameRoom.Cleaning:
                    roomJoinedEvent.cleaningGameChosen = true;
                    break;
                case MinigameRoom.Smelting:
                    roomJoinedEvent.smeltingGameChosen = true;
                    break;
                case MinigameRoom.Casting:
                    roomJoinedEvent.castingGameChosen = true;
                    break;
            }
			pMember.SendMessage(roomJoinedEvent);
		}

		public override void Update()
		{
			//demo of how we can tell people have left the game...
			int oldMemberCount = memberCount;
			base.Update();
			int newMemberCount = memberCount;

			if (oldMemberCount != newMemberCount)
			{
				Log.LogInfo("People left the game...", this);
				
			}
		}

		protected override void handleNetworkMessage(ASerializable pMessage, TcpMessageChannel pSender)
		{
			
        }

      

	}
}
