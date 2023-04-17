using shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace server
{
	/**
	 * The LobbyRoom is a little bit more extensive than the LoginRoom.
	 * In this room clients change their 'ready status'.
	 * If enough people are ready, they are automatically moved to the GameRoom to play a Game (assuming a game is not already in play).
	 */ 
	class LobbyRoom : SimpleRoom
	{
		//this list keeps tracks of which players are ready to play a game, this is a subset of the people in this room
		private List<TcpMessageChannel> _readyMembers = new List<TcpMessageChannel>();

		public LobbyRoom(TCPGameServer pOwner) : base(pOwner)
		{
		}

		protected override void addMember(TcpMessageChannel pMember)
		{
			base.addMember(pMember);

			//tell the member it has joined the lobby
			RoomJoinedEvent roomJoinedEvent = new RoomJoinedEvent();
			roomJoinedEvent.room = RoomJoinedEvent.Room.LOBBY_ROOM;
			for (int i = 0; i < _members.Count; i++)
			{
				PlayerInfo pI = _server.GetPlayerInfo(_members[i]);
				if (!roomJoinedEvent.miningGameChosen)
				{
					roomJoinedEvent.miningGameChosen = pI.room == MinigameRoom.Mining;
				}
                if (!roomJoinedEvent.cleaningGameChosen)
                {
                    roomJoinedEvent.cleaningGameChosen = pI.room == MinigameRoom.Cleaning;
                }
                if (!roomJoinedEvent.smeltingGameChosen)
                {
                    roomJoinedEvent.smeltingGameChosen = pI.room == MinigameRoom.Smelting;
                }
                if (!roomJoinedEvent.castingGameChosen)
                {
                    roomJoinedEvent.castingGameChosen = pI.room == MinigameRoom.Casting;
                }
			}
			pMember.SendMessage(roomJoinedEvent);
			Log.LogInfo("Someone joined the lobby", this);
			sendLobbyUpdateCount();
		}

		/**
		 * Override removeMember so that our ready count and lobby count is updated (and sent to all clients)
		 * anytime we remove a member.
		 */
		protected override void removeMember(TcpMessageChannel pMember)
		{
			base.removeMember(pMember);
			_readyMembers.Remove(pMember);

			sendLobbyUpdateCount();
		}

		protected override void handleNetworkMessage(ASerializable pMessage, TcpMessageChannel pSender)
		{
			if (pMessage is ChangeReadyStatusRequest) handleReadyNotification(pMessage as ChangeReadyStatusRequest, pSender);
			else if (pMessage is MinigameChosenRequest) handleMinigameChosenRequest(pMessage as MinigameChosenRequest, pSender);
			else if (pMessage is StartGameRequest) handleStartGameRequest(pMessage as StartGameRequest, pSender);
            else if (pMessage is EndGameRequest) handleEndGameRequest(pMessage as EndGameRequest, pSender);
        }

        private void handleEndGameRequest(EndGameRequest endGameRequest, TcpMessageChannel pSender)
        {
			Log.LogInfo("AAAAAAAAAAAAAAAAAAAAAAAH", this);
        }

        private void handleStartGameRequest(StartGameRequest startGameRequest, TcpMessageChannel pSender)
        {
			Log.LogInfo("Handing Startgame request", this);
            GameRoom room = new GameRoom(_server);
            _server.GetGameRooms().Add(room);
			StartGameEvent gameStart = new StartGameEvent();
			gameStart.startRecipe = startGameRequest.startRecipe;
			//_server._hosts[_server.GetPlayerInfo(pSender).lobbyCode];
			pSender.SendMessage(gameStart);
            List<TcpMessageChannel> membersToMove = new List<TcpMessageChannel>();
			for (int i = 0; i < _members.Count; i++)
			{
				//room.AddMember(_members[i]);
				membersToMove.Add(_members[i]);
			}
			for (int i = 0; i < membersToMove.Count; i++)
			{
				//removeMember(membersToMove[i]);
				_members.Remove(membersToMove[i]);
			}
			room.StartGame(membersToMove);

			Log.LogInfo($"Members left: {_members.Count}", this);

        }

        private void handleMinigameChosenRequest(MinigameChosenRequest minigameChosenRequest, TcpMessageChannel pSender)
        {
			MinigameChosenResponse response = new MinigameChosenResponse(); 

			if (_members.Any(c => _server.GetPlayerInfo(c).room == minigameChosenRequest.chosenRoom) && minigameChosenRequest.chosenRoom != MinigameRoom.None)
			{

				//Deny It
				response.chosenRoom = minigameChosenRequest.chosenRoom;
				response.accepted = false;
				pSender.SendMessage(response);
			}
			else
			{
				PlayerInfo pI = _server.GetPlayerInfo(pSender);
                if (pI.room != MinigameRoom.None)
                {
                    MinigameUnChosenEvent unChosenEvent = new MinigameUnChosenEvent();
                    unChosenEvent.room = pI.room;
					Log.LogInfo(unChosenEvent, this);
                    sendToAll(unChosenEvent);
                    _server._hosts[_server.GetPlayerInfo(pSender).lobbyCode].SendMessage(unChosenEvent);
                }

                //Allow it
				pI.room = minigameChosenRequest.chosenRoom; 
                response.chosenRoom = minigameChosenRequest.chosenRoom;
                response.accepted = true;
				pSender.SendMessage(response);
				MinigameChosenEvent minigameEvent = new MinigameChosenEvent();
				
				minigameEvent.room = response.chosenRoom;
				
				sendToAll(minigameEvent);
				_server._hosts[_server.GetPlayerInfo(pSender).lobbyCode].SendMessage(minigameEvent);
			}
        }

        private void handleReadyNotification(ChangeReadyStatusRequest pReadyNotification, TcpMessageChannel pSender)
		{
			//if the given client was not marked as ready yet, mark the client as ready
			/*if (pReadyNotification.ready)
			{
				if (!_readyMembers.Contains(pSender)) _readyMembers.Add(pSender);
			}
			else //if the client is no longer ready, unmark it as ready
			{
				_readyMembers.Remove(pSender);
			}

			//do we have enough people for a game and is there no game running yet?
			if (_readyMembers.Count >= 2 )
			{
				TcpMessageChannel player1 = _readyMembers[0];
				TcpMessageChannel player2 = _readyMembers[1];
				removeMember(player1);
				removeMember(player2);
				GameRoom room = new GameRoom(_server);
				_server.GetGameRooms().Add(room); 
				room.StartGame(player1, player2);
			}

			//(un)ready-ing / starting a game changes the lobby/ready count so send out an update
			//to all clients still in the lobby
			sendLobbyUpdateCount();*/
		}


		private void sendLobbyUpdateCount()
		{
			LobbyInfoUpdate lobbyInfoMessage = new LobbyInfoUpdate();
			lobbyInfoMessage.memberCount = memberCount;
			lobbyInfoMessage.readyCount = _readyMembers.Count;
			sendToAll(lobbyInfoMessage);
		}

	}
}
