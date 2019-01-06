﻿using DiscordMafia.Base;

namespace DiscordMafia.Roles
{
    public class Mafioso : BaseRole
    {
        public override Team Team
        {
            get
            {
                return Team.Mafia;
            }
        }

        public override void OnNightStart(Game game, InGamePlayerInfo currentPlayer)
        {
            base.OnNightStart(game, currentPlayer);
            game.SendAlivePlayersMesssage(currentPlayer);
        }

        public override bool IsReady(GameState currentState)
        {
            switch (currentState)
            {
                case GameState.Night:
                    if (Player.VoteFor == null)
                    {
                        return false;
                    }
                    break;
            }
            return base.IsReady(currentState);
        }
    }
}
