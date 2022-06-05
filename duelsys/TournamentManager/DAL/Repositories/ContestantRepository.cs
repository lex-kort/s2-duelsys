﻿using MySql.Data.MySqlClient;
using System.Data;

using DAL.Interfaces;
using DTO.Users;
using DTO;

namespace DAL.Repositories
{
    public class ContestantRepository : BaseRepository, IContestantRepository
    {
        public ContestantRepository(DbContext dbContext) : base(dbContext) { }

        public bool Deregister(int userID, int tournamentID)
        {
            try
            {
                string query = "DELETE FROM syn_contestants WHERE user_id = @UserID AND tournament_id = @TournamentID;";
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@TournamentID", tournamentID);
                ExecuteNonQuery(cmd);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ContestantDTO? GetContestant(int tournamentID, int contestantID)
        {
            try
            {
                string query = @"SELECT c.*, a.name, a.surname 
                                FROM syn_contestants AS c
                                INNER JOIN syn_accounts AS a
                                ON a.id = c.user_id
                                WHERE user_id = @UserID AND tournament_id = @TournamentID;";
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Parameters.AddWithValue("@UserID", contestantID);
                cmd.Parameters.AddWithValue("@TournamentID", tournamentID);
                DataTable dt = ExecuteReader(cmd);
                DataRow row = dt.Rows[0];
                return InstantiateDTO(row);
            }
            catch
            {
                return null;
            }
        }

        public IList<ContestantDTO> GetContestants(int tournamentID)
        {
            try
            {
                string query = @"SELECT c.*, a.name, a.surname 
                                FROM syn_contestants AS c
                                INNER JOIN syn_accounts AS a
                                ON a.id = c.user_id
                                WHERE tournament_id = @TournamentID;";
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Parameters.AddWithValue("@TournamentID", tournamentID);
                DataTable results = ExecuteReader(cmd);
                List<ContestantDTO> contestants = new List<ContestantDTO>();
                foreach(DataRow row in results.Rows)
                {
                    contestants.Add(InstantiateDTO(row));
                }
                return contestants;
            }
            catch
            {
                throw;
            }
        }

        public bool Register(int userID, int tournamentID)
        {
            try
            {
                string query = "INSERT INTO syn_contestants (user_id, tournament_id, wins, losses) VALUES (@UserID, @TournamentID, @Wins, @Losses);";
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@TournamentID", tournamentID);
                cmd.Parameters.AddWithValue("@Wins", 0);
                cmd.Parameters.AddWithValue("@Losses", 0);
                ExecuteNonQuery(cmd);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public TournamentDTO? GetTournament(int tournamentID)
        {
            try
            {
                string query = "SELECT * FROM syn_tournaments WHERE id = @ID;";
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Parameters.AddWithValue("@ID", tournamentID);
                DataRow? row = ExecuteReader(cmd).Rows[0];
                return new TournamentDTO(
                    Convert.ToInt32(row["ID"]),
                    row["title"].ToString()!,
                    row["description"].ToString()!,
                    row["sport"].ToString()!,
                    row["team_type"].ToString()!,
                    row["city"].ToString()!,
                    row["address"].ToString()!,
                    Convert.ToInt32(row["min_contestants"]),
                    Convert.ToInt32(row["max_contestants"]),
                    row["start_date"].ToString()!,
                    row["end_date"].ToString()!,
                    row["status"].ToString()!,
                    row["system"].ToString()!
                );
            }
            catch
            {
                return null;
            }
        }

        private ContestantDTO InstantiateDTO(DataRow row)
        {
            return new ContestantDTO(
                    Convert.ToInt32(row["user_id"]),
                    row["name"].ToString()!,
                    row["surname"].ToString()!,
                    Convert.ToInt32(row["tournament_id"]),
                    Convert.ToInt32(row["wins"]),
                    Convert.ToInt32(row["losses"])
                );
        }

        public void SaveResults(int tournamentID, int winnerID, int wins, int loserID, int losses)
        {
            try
            {
                string query = @"UPDATE syn_contestants SET wins = @Wins WHERE tournament_id = @TournamentID AND user_id = @WinnerID;
                                UPDATE syn_contestants SET losses = @Losses WHERE tournament_id = @TournamentID AND user_id = @LoserID;";
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Parameters.AddWithValue("@TournamentID", tournamentID);
                cmd.Parameters.AddWithValue("@WinnerID", winnerID);
                cmd.Parameters.AddWithValue("@Wins", wins);
                cmd.Parameters.AddWithValue("@LoserID", loserID);
                cmd.Parameters.AddWithValue("@Losses", losses);
                ExecuteNonQuery(cmd);
            }
            catch
            {
                throw;
            }
        }
    }
}
