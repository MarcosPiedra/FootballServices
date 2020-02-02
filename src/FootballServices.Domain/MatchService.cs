using FootballServices.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballServices.Domain
{
    public class MatchService : IMatchService
    {
        private readonly IRepository<Match> repoMatches;
        private readonly IRepository<Referee> repoReferees;
        private readonly IRepository<Player> repoPlayers;
        private readonly IRepository<Manager> repoManagers;

        public MatchService(IRepository<Match> repoMatches,
                            IRepository<Referee> repoReferees,
                            IRepository<Player> repoPlayers,
                            IRepository<Manager> repoManagers)
        {
            this.repoMatches = repoMatches;
            this.repoReferees = repoReferees;
            this.repoPlayers = repoPlayers;
            this.repoManagers = repoManagers;
        }

        public async Task AddAsync(Match match)
        {
            UpdateStatus(match);
            await repoMatches.AddAsync(match);
            await repoMatches.SaveAsync();
        }

        public async Task<Match> FindAsync(int id)
        {
            var match = await repoMatches.FindAsync(id);
            if (match == null)
                return await Task.FromResult(match);

            var awayIdList = match.AwayTeamPlayersIds.ParseIds();
            var houseIdList = match.HouseTeamPlayersIds.ParseIds();
            void findPlayer(int playerId, List<Player> playersToFill)
            {
                var playedFound = repoPlayers.Query.FirstOrDefault(p => p.Id == playerId);
                if (playedFound != null)
                {
                    playersToFill.Add(playedFound);
                }
            }

            awayIdList.ForEach(p => findPlayer(p, match.AwayTeamPlayers));
            houseIdList.ForEach(p => findPlayer(p, match.HouseTeamPlayers));
            match.AwayTeamManager = await repoManagers.FindAsync(match.AwayTeamManagerId) ?? null;
            match.HouseTeamManager = await repoManagers.FindAsync(match.HouseTeamManagerId) ?? null;
            match.Referee = await repoReferees.FindAsync(match.RefereeId) ?? null;

            return match;
        }

        public async Task<List<Match>> GetAllAsync()
        {
            var awayPlayersIds = repoMatches.Query.Select(p => new { p.Id, players = p.AwayTeamPlayersIds.ParseIds() });
            var housePlayersIds = repoMatches.Query.Select(p => new { p.Id, players = p.HouseTeamPlayersIds.ParseIds() });

            var awayPlayers = new List<PlayersMatchRelated>();
            var housePlayers = new List<PlayersMatchRelated>();

            void fillPlayer(int matchId, int playerId, List<PlayersMatchRelated> playersToFill)
            {
                var pmRel = playersToFill.FirstOrDefault(p => p.MatchId == matchId);
                if (pmRel == null)
                {
                    pmRel = new PlayersMatchRelated { MatchId = matchId };
                    playersToFill.Add(pmRel);
                }

                var playedFound = repoPlayers.Query.FirstOrDefault(p => p.Id == playerId);
                if (playedFound != null)
                {
                    pmRel.Players.Add(playedFound);
                }
            }

            await awayPlayersIds.ForEachAsync(m => m.players.ForEach(pId => fillPlayer(m.Id, pId, awayPlayers)));
            await housePlayersIds.ForEachAsync(m => m.players.ForEach(pId => fillPlayer(m.Id, pId, housePlayers)));

            var matches = new List<Match>();
            foreach (var m in await repoMatches.Query.ToListAsync())
            {
                var match = new Match();
                match.Id = m.Id;
                match.Date = m.Date;
                match.Status = m.Status;
                match.AwayTeamPlayers = awayPlayers.FirstOrDefault(p => p.MatchId == m.Id)?.Players;
                match.HouseTeamPlayers = housePlayers.FirstOrDefault(p => p.MatchId == m.Id)?.Players;
                match.AwayTeamManager = await repoManagers.FindAsync(m.AwayTeamManagerId) ?? null;
                match.HouseTeamManager = await repoManagers.FindAsync(m.HouseTeamManagerId) ?? null;
                match.Referee = await repoReferees.FindAsync(m.RefereeId) ?? null;
                matches.Add(match);
            }
            return matches;
        }

        public async Task<List<Match>> GetMatchThatStartInAsync(int minutes)
        {
            minutes = -Math.Abs(minutes);
            var now = DateTime.Now;
            var matches = repoMatches.Query
                                     .Where(m => m.Status == MatchStatus.NoPlayedYet
                                              && !m.IdsIncorrectReported
                                              && m.Date.AddMinutes(minutes) <= now
                                              && m.Date <= now);

            var matchesToReturn = new List<Match>();
            foreach (var m in matches)
            {
                matchesToReturn.Add(await this.FindAsync(m.Id));
            }

            return matchesToReturn;
        }

        public async Task RemoveAsync(Match match)
        {
            repoMatches.Remove(match);
            await repoMatches.SaveAsync();
        }

        public async Task UpdateAsync(Match match)
        {
            UpdateStatus(match);
            repoMatches.Update(match);
            await repoMatches.SaveAsync();
        }

        public void UpdateStatus(Match match)
        {
            //DateTime provider??
            if (match.Date > DateTime.Now)
            {
                match.Status = MatchStatus.Finished;
            }
            else if (DateTime.Now.Subtract(new TimeSpan(0, 90, 0)) <= match.Date)
            {
                match.Status = MatchStatus.Playing;
            }
            else
            {
                match.Status = MatchStatus.NoPlayedYet;
            }
        }

        public async Task UpdateStatusAsync()
        {
            foreach (var m in repoMatches.Query)
            {
                UpdateStatus(m);
            }
            await repoMatches.SaveAsync();
        }
    }

    class PlayersMatchRelated
    {
        public int MatchId { get; set; } = 0;
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
