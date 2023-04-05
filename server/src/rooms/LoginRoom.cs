using shared;
using System;

namespace server
{
	/**
	 * The LoginRoom is the first room clients 'enter' until the client identifies himself with a PlayerJoinRequest. 
	 * If the client sends the wrong type of request, it will be kicked.
	 *
	 * A connected client that never sends anything will be stuck in here for life,
	 * unless the client disconnects (that will be detected in due time).
	 */ 
	class LoginRoom : SimpleRoom
	{
		//arbitrary max amount just to demo the concept
		private const int MAX_MEMBERS = 50;

		public LoginRoom(TCPGameServer pOwner) : base(pOwner)
		{
		}

		protected override void addMember(TcpMessageChannel pMember)
		{
			base.addMember(pMember);

			//notify the client that (s)he is now in the login room, clients can wait for that before doing anything else
			RoomJoinedEvent roomJoinedEvent = new RoomJoinedEvent();
			roomJoinedEvent.room = RoomJoinedEvent.Room.LOGIN_ROOM;
			pMember.SendMessage(roomJoinedEvent);
		}

		protected override void handleNetworkMessage(ASerializable pMessage, TcpMessageChannel pSender)
		{
			if (pMessage is PlayerJoinRequest)
			{
				handlePlayerJoinRequest(pMessage as PlayerJoinRequest, pSender);
			}else if (pMessage is HostJoinRequest)
            {
                handleHostJoinRequest(pMessage as HostJoinRequest, pSender);
            }
            else //if member sends something else than a PlayerJoinRequest
			{
				Log.LogInfo("Declining client, auth request not understood", this);

				//don't provide info back to the member on what it is we expect, just close and remove
				removeAndCloseMember(pSender);
			}
		}

        private void handleHostJoinRequest(HostJoinRequest hostJoinRequest, TcpMessageChannel pSender)
        {
			Log.LogInfo("Connection Succeeded",this);
			HostJoinResponse response =	new HostJoinResponse();
			_server._hosts.Add(response.passcode, pSender);
            pSender.SendMessage(response);
        }

        /**
		 * Tell the client he is accepted and move the client to the lobby room.
		 */
        private void handlePlayerJoinRequest (PlayerJoinRequest pMessage, TcpMessageChannel pSender)
		{
			Log.LogInfo("Moving new client to accepted...", this);
			
			PlayerJoinResponse playerJoinResponse = new PlayerJoinResponse();
            if (_server.GetPlayerInfo((p) => p.passCode == pMessage.passCode).Count == 0)
			{
				playerJoinResponse.result = PlayerJoinResponse.RequestResult.ACCEPTED;
				PlayerInfo newPlayerInfo = new PlayerInfo();
				newPlayerInfo.passCode = pMessage.passCode;
				_server.AddPlayerInfo(pSender, newPlayerInfo);
				removeMember(pSender);
				_server.GetLobbyRoom().AddMember(pSender);
			}
            playerJoinResponse.result = PlayerJoinResponse.RequestResult.DENIED;
            pSender.SendMessage(playerJoinResponse);
		}

	}
}
