using shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

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
		
		private Dictionary<MinigameRoom, int> gameRoomGrades;


        public GameRoom(TCPGameServer pOwner) : base(pOwner)
		{
			gameRoomGrades = new Dictionary<MinigameRoom, int>();
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
		protected override void handleNetworkMessage(ASerializable pMessage, TcpMessageChannel pSender)
		{
			if(pMessage is AddRecipeRequest) handleAddRecipeRequest(pMessage as AddRecipeRequest, pSender);	
			else if(pMessage is SendMetalRequest) handleSendMetalRequest(pMessage as SendMetalRequest, pSender);
			else if (pMessage is SendMetalsRequest) handleSendMetalsRequest(pMessage as SendMetalsRequest, pSender);
			else if (pMessage is FinishItemRequest) handleFinishItemRequest(pMessage as FinishItemRequest, pSender);
			else if(pMessage is EndGameRequest) handleEndGameRequest(pMessage as EndGameRequest, pSender);
        }

		private void handleEndGameRequest(EndGameRequest endGameRequest, TcpMessageChannel pSender)
		{
			Log.LogInfo("Hanlding EndGame",this);
			//if (!_server._hosts.ContainsValue(pSender)) return;
			EndGameEvent endGame = new EndGameEvent();
			endGame.size = 4;
			endGame.grades.Add(getRoomGrade(MinigameRoom.Mining));
            endGame.grades.Add(getRoomGrade(MinigameRoom.Cleaning));
            endGame.grades.Add(getRoomGrade(MinigameRoom.Smelting));
            endGame.grades.Add(getRoomGrade(MinigameRoom.Casting));
            sendToAll(endGame);
			IsGameInPlay= false;
		}

		private RoomGrade getRoomGrade(MinigameRoom room)
		{
			int sum = 0;
			foreach (var kvp in gameRoomGrades)
			{
				if(kvp.Key == room)
				{
					sum += kvp.Value;
				}
			}
            RoomGrade roomGrade = new RoomGrade();
            roomGrade.grade = sum * 10;
            roomGrade.room = room;
			return roomGrade;
        }

        private void handleFinishItemRequest(FinishItemRequest finishItemRequest, TcpMessageChannel pSender)
        {
			//Find the host
			Log.LogInfo("got the request",this);
			for (int i = 0; i < _members.Count; i++)
			{
				if (_server._hosts.ContainsValue(_members[i]))
				{
					//Send the respond to host
					Log.LogInfo("Host found sending item", this);
					FinishItemResponse response = new FinishItemResponse();
					response.recipe = finishItemRequest.recipe;
					gameRoomGrades.Add(MinigameRoom.Casting, finishItemRequest.grade);
					_members[i].SendMessage(response);
					return;
				}
			}
        }

        private void handleSendMetalsRequest(SendMetalsRequest sendMetalsRequest, TcpMessageChannel pSender)
        {
			Log.LogInfo($"Grade received: {sendMetalsRequest.grade}", this);
			for (int i = 0; i < _members.Count; i++)
			{
				if (_server.GetPlayerInfo(_members[i]).room == sendMetalsRequest.to)
				{
					SendMetalsResponse response = new SendMetalsResponse();
					response.from= sendMetalsRequest.from;
					response.to= sendMetalsRequest.to;
					response.metals= sendMetalsRequest.metals;
					response.size= sendMetalsRequest.size;
					response.grade = sendMetalsRequest.grade;
					gameRoomGrades.Add(sendMetalsRequest.from, sendMetalsRequest.grade);
					_members[i].SendMessage(response);
					return;
				}
			}
        }

        private void handleSendMetalRequest(SendMetalRequest sendMetalRequest, TcpMessageChannel pSender)
        {
            Log.LogInfo($"Grade received: {sendMetalRequest.grade}", this);
            for (int i = 0; i < _members.Count; i++)
            {
                if (_server.GetPlayerInfo(_members[i]).room == sendMetalRequest.to)
                {
                    SendMetalResponse response = new SendMetalResponse();
                    response.from = sendMetalRequest.from;
                    response.to = sendMetalRequest.to;
                    response.metal = sendMetalRequest.metal;
                    response.grade = sendMetalRequest.grade;
                    gameRoomGrades.Add(sendMetalRequest.from, sendMetalRequest.grade);
                    _members[i].SendMessage(response);
                    return;
                }
            }
        }

        private void handleAddRecipeRequest(AddRecipeRequest addRecipeRequest, TcpMessageChannel pSender)
        {
			Log.LogInfo(addRecipeRequest, this);
			RecipeAddedMessage mes = new RecipeAddedMessage();
			mes.recipe = addRecipeRequest.recipe;
			sendToAll(mes);
        }
    }
}
