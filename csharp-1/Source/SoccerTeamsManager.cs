using System;
using System.Collections.Generic;
using Codenation.Challenge.Exceptions;
using Source;
using System.Linq;

namespace Codenation.Challenge
{
    public class SoccerTeamsManager : IManageSoccerTeams
    {
        public List<SoccerTeam> SoccerTeam { get; set; } = null;
        public List<SoccerPlayer> SoccerPlayer { get; set; } = null;

        public SoccerTeamsManager()
        {
            SoccerTeam = new List<SoccerTeam>();
            SoccerPlayer = new List<SoccerPlayer>();
        }

        public void AddTeam(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {
            if (SoccerTeam.Exists(x => x.Id == id))
            {
                throw new UniqueIdentifierException();
            }

            SoccerTeam.Add(new SoccerTeam()
            {
                Id = id,
                Name = name,
                CreateDate = createDate,
                MainShirtColor = mainShirtColor,
                SecondayShirtColor = secondaryShirtColor
            });
        }

        public void AddPlayer(long id, long teamId, string name, DateTime birthDate, int skillLevel, decimal salary)
        {
            if (SoccerPlayer.Exists(x => x.Id == id))
            {
                throw new UniqueIdentifierException();
            }

            if (!SoccerTeam.Exists(x => x.Id == teamId))
            {
                throw new TeamNotFoundException();
            }

            SoccerPlayer.Add(new SoccerPlayer()
            {
                Id = id,
                TeamId = teamId,
                Name = name,
                BirthDate = birthDate,
                SkillLevel = skillLevel,
                Salary = salary
            });
        }

        public void SetCaptain(long playerId)
        {
            if (!SoccerPlayer.Exists(x => x.Id == playerId))
            {
                throw new PlayerNotFoundException();
            }

            // busca novo capitão
            var positionNewCaptain = SoccerPlayer.FindIndex(x => x.Id == playerId);

            // verifica se existe capitão
            if (SoccerPlayer.Exists(x => x.IsCaptain && x.TeamId == SoccerPlayer[positionNewCaptain].TeamId))
            {
                var positionActualCaptain = SoccerPlayer.FindIndex(x => x.IsCaptain);
                SoccerPlayer[positionActualCaptain].IsCaptain = false;
            }

            // seta novo capitão
            SoccerPlayer[positionNewCaptain].IsCaptain = true;
        }

        public long GetTeamCaptain(long teamId)
        {
            if (!SoccerTeam.Exists(x => x.Id == teamId))
            {
                throw new TeamNotFoundException();
            }

            if (!SoccerPlayer.Exists(x => x.TeamId == teamId && x.IsCaptain))
            {
                throw new CaptainNotFoundException();
            }

            return SoccerPlayer.Find(x => x.TeamId == teamId && x.IsCaptain).Id;
        }

        public string GetPlayerName(long playerId)
        {
            if (!SoccerPlayer.Exists(x => x.Id == playerId))
            {
                throw new PlayerNotFoundException();
            }

            return SoccerPlayer.Find(x => x.Id == playerId).Name;
        }

        public string GetTeamName(long teamId)
        {
            if (!SoccerTeam.Exists(x => x.Id == teamId))
            {
                throw new TeamNotFoundException();
            }

            return SoccerTeam.Find(x => x.Id == teamId).Name;
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            if (!SoccerTeam.Exists(x => x.Id == teamId))
            {
                throw new TeamNotFoundException();
            }

            return (from player in SoccerPlayer
                    where player.TeamId == teamId
                    orderby player.Id
                    select player.Id).ToList();
        }

        public long GetBestTeamPlayer(long teamId)
        {
            if (!SoccerTeam.Exists(x => x.Id == teamId))
            {
                throw new TeamNotFoundException();
            }

            return (from player in SoccerPlayer
                    where player.TeamId == teamId
                    orderby player.SkillLevel descending
                    select player.Id).First();
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            if (!SoccerTeam.Exists(x => x.Id == teamId))
            {
                throw new TeamNotFoundException();
            }

            return (from player in SoccerPlayer
                    where player.TeamId == teamId
                    orderby player.BirthDate
                    select player.Id).First();
        }

        public List<long> GetTeams()
        {
            if (SoccerTeam == null || SoccerTeam.Count == 0)
            {
                return new List<long>();
            }

            return (from team in SoccerTeam
                    orderby team.Id
                    select team.Id).ToList();
        }

        public long GetHigherSalaryPlayer(long teamId)
        {

            if (!SoccerTeam.Exists(x => x.Id == teamId))
            {
                throw new TeamNotFoundException();
            }

            return (from player in SoccerPlayer
                    where player.TeamId == teamId
                    orderby player.Salary descending, player.Id
                    select player.Id).First();
        }

        public decimal GetPlayerSalary(long playerId)
        {
            if (!SoccerPlayer.Exists(x => x.Id == playerId))
            {
                throw new PlayerNotFoundException();
            }

            return SoccerPlayer.Find(x => x.Id == playerId).Salary;
        }

        public List<long> GetTopPlayers(int top)
        {
            if (SoccerPlayer == null || SoccerPlayer.Count == 0)
            {
                return new List<long>();
            }

            return (from player in SoccerPlayer
                    orderby player.SkillLevel descending
                    select player.Id).Take(top).ToList();
        }

        public string GetVisitorShirtColor(long teamId, long visitorTeamId)
        {
            if (!SoccerTeam.Exists(x => x.Id == teamId) || !SoccerTeam.Exists(x => x.Id == visitorTeamId))
            {
                throw new TeamNotFoundException();
            }

            var secondaryShirtColor = string.Empty;

            if (SoccerTeam.Find(x => x.Id == teamId).MainShirtColor == SoccerTeam.Find(x => x.Id == visitorTeamId).MainShirtColor)
            {
                secondaryShirtColor = SoccerTeam.Find(x => x.Id == visitorTeamId).SecondayShirtColor;
            }
            else
            {
                secondaryShirtColor = SoccerTeam.Find(x => x.Id == visitorTeamId).MainShirtColor;
            }

            return secondaryShirtColor;
        }
    }
}
